Imports System.Data.OleDb
Imports System.Windows.Forms
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.DataVisualization.Charting


Public Class Form
    Public CrcClass As New CRC16 'CRC16的类
    Public Crc16Data As String 'crc校验码
    Public Declare Function timeGetTime Lib "winmm.dll" () As Long
    Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)
    Dim DispDec(999) As Decimal '真实数据
    Dim SendBytes(999) As String '串口数据
    Dim g_SerialData As String '串口接收到的数据
    Dim SendFlag(39) As Boolean '串口发送数据的标志
    Dim PulseFlag As Boolean '发送脉冲标志
    Dim StateFlag As Boolean '发送状态标志
    Dim ContinuousFlag As Boolean '发送连续波形（停止波形）标志
    Dim g_DataFlag As Boolean = False '画图标志
    Dim DecOff As Double = 4 / 2 '数据偏移量
    Dim MultipleNum As Double = 4094 / 4 '真实数据域串口数据的比例

    Dim state As Boolean 'FPGA是否忙碌状态
    Dim KeyFlag As Boolean = False '连续波形（停止波形）转换标志

    Dim FitDeci(999) As Decimal '从原始数据中拟合或者不拟合得到的结果
    Dim FitFlag As Boolean = False '拟合标志
    Dim MaxShowPointNum As Integer = 1000 '单个通道在时间方向上最大显示数据点数
    Dim MsPerPoint As Long = 0 '每个点对应的毫秒数
    Dim MaxSampPointNum As Integer '总共的采样点数  根据总采样时间和采样周期计算

    '标定数据
    Public Calib_data1(21) As String
    Public Calib_data2(21) As String
    Public Calib_data3(21) As String
    Public Calib_data4(21) As String
    Public Calib_flag As Boolean

    Public g_time, g_data, g_temper, g_WDir, g_WSpeed As String
    Public g_ntemper As Double
    Public g_nWSpeed As Double



    Dim GratCnt_max As Integer = 15000
    '实时显示当前值
    Dim now_data(10) As Double
    '原始数据
    Dim Org_data(10) As Double

    '清零  ---   标定
    Dim offset(10) As Double


    Dim pa1_offset_dem As Double = 0
    Dim pa2_offset_dem As Double = 0

    Dim delay_time As Double = 0.01
    Dim samp_time As Double = 0.1

    Dim timercnt As UInt32 = 0
    '画图标志
    Dim show_flag(10) As Boolean

    Dim show_flag_A As Boolean = False

    Dim show_flag_B As Boolean = False
    Dim MAX_DATA_NUM As Integer = 500000

    '实时画图数据
    Dim show_data(10, MAX_DATA_NUM) As Double '0---9
    Dim show_data_time(MAX_DATA_NUM) As Long '对应的时间
    Dim Save_data(10, MAX_DATA_NUM) As Double '0---9


    Dim show_name(10) As String
    Dim date_now As String
    Dim g_date_Start As Date = Now '本次采集开始的时间
    Dim file_name_now As String
    Dim file_rw_data As String

    Dim iniFilePath As String
    Dim COMID As String
    Dim iniStr(10) As String

    '启动函数
    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '读取上一次的配置文件
        MyBase.KeyPreview = True
        Try
            iniFilePath = "C:\my_grat\info.txt"
            Shell("cmd.exe /c md C:\my_grat")
            Dim starIniStr(10) As String
            starIniStr(0) = "COM5"
            starIniStr(1) = "0.01"
            starIniStr(2) = "0.1"
            starIniStr(3) = "0.1"
            starIniStr(4) = "0.1"
            starIniStr(5) = "0.1"
            starIniStr(6) = "0.1"
            starIniStr(7) = "0.1"
            LoadText(iniFilePath, starIniStr)
            iniStr = ReadTextAllLine(iniFilePath)

            '每个数据的实际意义
            SerialPort1.PortName = iniStr(0)
            delay_time = iniStr(1)
            samp_time = iniStr(2)
            offset(8) = iniStr(3)
            offset(9) = iniStr(4)
        Catch

        End Try


        Dim str As String
        '检测串口是否打开
        Try
            SerialPort1.Open()
            'MsgBox("串口端口匹配成功，请进行下步操作！", MsgBoxStyle.OkOnly, "提示")
        Catch ex As Exception
            'MsgBox(ex.Message & "，请重新选择串口端口！", MsgBoxStyle.OkOnly, "提示")
        End Try


        '串口端口名称
        For i As Integer = 1 To 20
            str = "COM" & i
            ComboBox1.Items.Add(str)
        Next

        ComboBox1.SelectedText = SerialPort1.PortName


        '***************************************************
        '当前时间设为保存文件的时间
        '***************************************************
        date_now = Now
        date_now = CStr(Year(date_now)) & CStr(Month(date_now)) & CStr(DateAndTime.Day(date_now)) & CStr(Hour(date_now)) & CStr(Minute(date_now)) & CStr(Second(date_now))
        'date_now = Format(date_now, "yyyy/mm/dd HH:MM:ss")
        file_name_now = "C:\my_grat\Start" & date_now & ".txt"
        'Shell("cmd.exe /c md C:\my_grat")
        LoadText(file_name_now, "Date,SunTime,Samp/min,A1/mm,A2/mm,A3/mm,A4/mm,B1/mm,B2/mm,B3/mm,B4/mm,Pa1/MPa,Pa2/MPa")

        '保存时间,用于精采样数据
        lastMs = Now.Millisecond + Now.Second * 1000 + Now.Minute * 60000
        lastMs = lastMs Mod (1000 * 60 * 20)

        '***************************************************
        '配置画图区域的属性
        '***************************************************
        Chart1.Series(0).Color = Color.Yellow
        Chart1.Series(1).Color = Color.Blue
        Chart1.Series(2).Color = Color.Green
        Chart1.Series(3).Color = Color.Red
        Chart1.Series(4).Color = Color.White
        Chart1.Series(5).Color = Color.Orange
        Chart1.Series(6).Color = Color.Purple
        Chart1.Series(7).Color = Color.Gray
        Chart1.Series(8).Color = Color.Pink
        Chart1.Series(9).Color = Color.LimeGreen

        For i As Integer = 0 To 9
            Chart1.Series(i).XValueType = DataVisualization.Charting.ChartValueType.DateTime
            Chart1.Series(i).YValueType = DataVisualization.Charting.ChartValueType.Double
            Chart1.Series(i).ChartType = DataVisualization.Charting.SeriesChartType.Line
        Next
        'For i As Integer = 0 To 3
        '    Chart1.Series(i).ToolTip = "A" + (i + 1).ToString
        'Next
        'For i As Integer = 4 To 7
        '    Chart1.Series(i).ToolTip = "B" + (i - 4 + 1).ToString
        'Next
        'For i As Integer = 8 To 9
        '    Chart1.Series(i).ToolTip = "Pa" + (i - 8 + 1).ToString
        'Next

        For i As Integer = 0 To 9
            Chart1.Series(i).MarkerSize = 8
            Chart1.Series(i).MarkerStyle = MarkerStyle.Diamond
            Chart1.Series(i).MarkerColor = Chart1.Series(i).Color
        Next


        'X轴最小选择区域()  单位问题太纠结了 不加了
        'Chart1.ChartAreas(0).AxisX.ScaleView.MinSize = 5 '5秒后才能缩放
        'Chart1.ChartAreas(0).AxisX.ScaleView.MinSizeType = DateTimeIntervalType.Seconds


    End Sub

    '实时显示 时间曲线
    Dim PId As Integer = 0

    Dim interval As Integer = 1
    Dim selectedInterval As Integer = 1
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        '设置是否显示某个曲线
        Chart1.Series(0).Color = Color.Yellow
        Chart1.Series(1).Color = Color.Blue
        Chart1.Series(2).Color = Color.Green
        Chart1.Series(3).Color = Color.Red
        Chart1.Series(4).Color = Color.White
        Chart1.Series(5).Color = Color.Orange
        Chart1.Series(6).Color = Color.Purple
        Chart1.Series(7).Color = Color.Gray
        Chart1.Series(8).Color = Color.Pink
        Chart1.Series(9).Color = Color.LimeGreen
        For j As Integer = 0 To 9
            Chart1.Series(j).MarkerColor = Chart1.Series(j).Color
        Next
        For j As Integer = 0 To 9
            If Not show_flag(j) Then
                Chart1.Series(j).MarkerColor = Color.Transparent
                Chart1.Series(j).Color = Color.Transparent
            End If
        Next




        PId = g_Data_Index '当前数据总个数
        If PId <= 2 Then
            Exit Sub
        End If
        PId -= 1




        '清掉数据重新加载
        For j As Integer = 0 To 9
            Chart1.Series(j).Points.Clear()
        Next


        '整数倍的时候才改变一次间距
        If (PId Mod MaxPointNum) = 0 And (PId / MaxPointNum) <> 0 Then
            interval = PId / MaxPointNum
        End If

        '根据算的间隔 把原始数据往图表数据上套
        Dim i As Integer = 0
        If PId < MaxPointNum Then
            For i = 0 To PId
                For j As Integer = 0 To 7
                    Chart1.Series(j).Points.Add(OrgPointsGrat(j, i * interval)) '位移
                Next
                For j As Integer = 8 To 9
                    Chart1.Series(j).Points.Add(OrgPointsP(j - 8, i * interval)) '压力
                Next
            Next
        Else
            For i = 0 To (PId / interval) - 1
                For j As Integer = 0 To 7
                    Chart1.Series(j).Points.Add(OrgPointsGrat(j, i * interval))
                Next
                For j As Integer = 8 To 9
                    Chart1.Series(j).Points.Add(OrgPointsP(j - 8, i * interval)) '压力
                Next
            Next
        End If

        '清除数据后把坐标轴起始点重新设置--todo
        Chart1.ChartAreas(0).AxisX.Minimum = Chart1.Series(0).Points(0).XValue

        '***************************************************
        '让X轴刻度加密
        '***************************************************
        If PId > 30 Then
            Chart1.ChartAreas(0).AxisX.MinorGrid.Interval = Chart1.ChartAreas(0).AxisX.MajorTickMark.Interval / 2
            Chart1.ChartAreas(0).AxisX.MinorTickMark.Interval = Chart1.ChartAreas(0).AxisX.MajorTickMark.Interval / 2
            Chart1.ChartAreas(0).AxisX.LabelStyle.Interval = Chart1.ChartAreas(0).AxisX.MajorTickMark.Interval / 2
            Chart1.ChartAreas(0).AxisX.MinorGrid.IntervalType = Chart1.ChartAreas(0).AxisX.MajorTickMark.IntervalType
            Chart1.ChartAreas(0).AxisX.MinorTickMark.IntervalType = Chart1.ChartAreas(0).AxisX.MajorTickMark.IntervalType
            Chart1.ChartAreas(0).AxisX.LabelStyle.IntervalType = Chart1.ChartAreas(0).AxisX.MajorTickMark.IntervalType
            'Chart1.ChartAreas(0).AxisX.LabelStyle.Interval = 1
        End If



        '***************************************************
        'Y轴最小坐标的控制
        '***************************************************
        Dim maxY As Double
        Dim minY As Double
        Dim SeriesID As Integer = 0

        maxY = Chart1.Series(0).Points.FindMaxByValue.YValues(0)
        minY = Chart1.Series(0).Points.FindMinByValue.YValues(0)
        For SeriesID = 0 To 7
            If maxY < Chart1.Series(SeriesID).Points.FindMaxByValue.YValues(0) Then
                maxY = Chart1.Series(SeriesID).Points.FindMaxByValue.YValues(0)
            End If
        Next
        For SeriesID = 0 To 7
            If minY > Chart1.Series(SeriesID).Points.FindMinByValue.YValues(0) Then
                minY = Chart1.Series(SeriesID).Points.FindMinByValue.YValues(0)
            End If
        Next

        If (maxY - minY) < 0.01 Then
            Chart1.ChartAreas(0).AxisY.Maximum = maxY + 0.005
            Chart1.ChartAreas(0).AxisY.Minimum = minY - 0.005
        Else
            Chart1.ChartAreas(0).AxisY.Maximum = maxY + (maxY - minY) / 10
            Chart1.ChartAreas(0).AxisY.Minimum = minY - (maxY - minY) / 10
        End If

        maxY = Chart1.Series(8).Points.FindMaxByValue.YValues(0)
        minY = Chart1.Series(8).Points.FindMinByValue.YValues(0)
        For SeriesID = 8 To 9
            If maxY < Chart1.Series(SeriesID).Points.FindMaxByValue.YValues(0) Then
                maxY = Chart1.Series(SeriesID).Points.FindMaxByValue.YValues(0)
            End If
        Next
        For SeriesID = 8 To 9
            If minY > Chart1.Series(SeriesID).Points.FindMinByValue.YValues(0) Then
                minY = Chart1.Series(SeriesID).Points.FindMinByValue.YValues(0)
            End If
        Next

        If (maxY - minY) < 0.01 Then
            Chart1.ChartAreas(0).AxisY2.Maximum = maxY + 0.005
            Chart1.ChartAreas(0).AxisY2.Minimum = minY - 0.005
        Else
            Chart1.ChartAreas(0).AxisY2.Maximum = maxY + (maxY - minY) / 10
            Chart1.ChartAreas(0).AxisY2.Minimum = minY - (maxY - minY) / 10
        End If

        '***************************************************
        '是否跟踪数据
        '***************************************************
        'Label4.Text = Chart1.Series(0).Points.Count
        If autoTrackFlag Then
            Chart1.ChartAreas(0).AxisX.ScaleView.Position = OrgPointsGrat(0, PId).XValue - Chart1.ChartAreas(0).AxisX.ScaleView.Size
        End If




        '***************************************************
        '选择区域的数据处理
        '***************************************************
        If System.Double.IsNaN(Chart1.ChartAreas(0).AxisX.ScaleView.Position) Then
            L_Start.Text = "Start: " + DateTime.FromOADate(Chart1.Series(0).Points(0).XValue).ToString("HH:mm:ss")
            L_End.Text = "End: " + DateTime.FromOADate(Chart1.Series(0).Points(Chart1.Series(0).Points.Count - 1).XValue).ToString("HH:mm:ss")
            'L_PiontIntvalNum.Text = interval
        Else
            'Dim startViewTime As DateTime = DateTime.FromOADate(Chart1.ChartAreas(0).AxisX.ScaleView.Position)
            'Dim n_endViewTime As DateTime = DateTime.FromOADate(Chart1.ChartAreas(0).AxisX.ScaleView.Position).AddSeconds(Chart1.ChartAreas(0).AxisX.ScaleView.Size)


            Dim startViewTime As Double = Chart1.ChartAreas(0).AxisX.ScaleView.Position
            Dim endViewTime As Double = Chart1.ChartAreas(0).AxisX.ScaleView.Position + Chart1.ChartAreas(0).AxisX.ScaleView.Size

            L_Start.Text = "Start: " + DateTime.FromOADate(startViewTime).ToString("HH:mm:ss")
            L_End.Text = "End: " + DateTime.FromOADate(endViewTime).ToString("HH:mm:ss")


            '清除选中框的原始数据
            Dim tmpId As Integer
            Dim SeriesStartId As Integer '
            For tmpId = Chart1.Series(0).Points.Count - 1 To 0 Step -1
                If Chart1.Series(0).Points(tmpId).XValue <= endViewTime And Chart1.Series(0).Points(tmpId).XValue >= startViewTime Then
                    For j As Integer = 0 To 9
                        Chart1.Series(j).Points.RemoveAt(tmpId)
                    Next
                ElseIf Chart1.Series(0).Points(tmpId).XValue <= startViewTime Then
                    SeriesStartId = tmpId
                    Exit For
                End If
            Next

            '计算完整数据的个数
            Dim orgStartId As Integer
            Dim orgEndId As Integer
            For tmpId = 0 To PId
                If OrgPointsGrat(0, tmpId).XValue >= startViewTime Then
                    orgStartId = tmpId
                    Exit For
                End If
            Next
            For tmpId = 0 To PId
                If OrgPointsGrat(0, tmpId).XValue >= endViewTime Then
                    orgEndId = tmpId
                    Exit For
                End If
            Next

            '放大后是否需要间隔显示坐标点
            If (orgEndId - orgStartId) > MaxPointNum Then
                selectedInterval = (orgEndId - orgStartId) / MaxPointNum
            End If

            '加入完整数据
            'Dim SeriesId As Integer
            Dim OrgDateID As Integer = orgStartId
            For SeriesId = SeriesStartId To PId '从起始点开始插入数据
                For j As Integer = 0 To 7
                    Chart1.Series(j).Points.Insert(SeriesId + 1, OrgPointsGrat(j, OrgDateID))
                Next
                For j As Integer = 8 To 9
                    Chart1.Series(j).Points.Insert(SeriesId + 1, OrgPointsP(j - 8, OrgDateID))
                Next

                OrgDateID += 1 * selectedInterval

                If IsNothing(OrgPointsGrat(0, OrgDateID)) Then
                    Exit For
                End If
                If OrgPointsGrat(0, OrgDateID).XValue >= endViewTime Then
                    Exit For
                End If
            Next

            'L_PiontIntvalNum.Text = selectedInterval
        End If

        '***************************************************
        '添加每个点的数据显示
        '***************************************************
        Dim nPoints As DataPointCollection = Chart1.Series(0).Points
        Dim PointTime As DateTime = DateTime.FromOADate(nPoints(nPoints.Count - 1).XValue) '时间
        For tipId As Integer = 0 To nPoints.Count - 1
            For j As Integer = 0 To 3
                If show_flag(j) Then
                    Chart1.Series(j).Points(tipId).ToolTip = "( " + PointTime.ToString("HH:mm:ss") + ", A" + (j + 1).ToString + ": " + Chart1.Series(j).Points(tipId).YValues(0).ToString("f3") + " )" '（时间，数值）
                End If
            Next
            For j As Integer = 4 To 7
                If show_flag(j) Then
                    Chart1.Series(j).Points(tipId).ToolTip = "( " + PointTime.ToString("HH:mm:ss") + ", B" + (j - 4 + 1).ToString + ": " + Chart1.Series(j).Points(tipId).YValues(0).ToString("f3") + " )" '（时间，数值）
                End If
            Next
            For j As Integer = 8 To 9
                If show_flag(j) Then
                    Chart1.Series(j).Points(tipId).ToolTip = "( " + PointTime.ToString("HH:mm:ss") + ", Pa" + (j - 8 + 1).ToString + ": " + Chart1.Series(j).Points(tipId).YValues(0).ToString("f1") + " )" '（时间，数值）
                End If
            Next
        Next

    End Sub


    '串口接收
    Private Sub SerialPort1_DataReceived(ByVal sender As System.Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        Dim Str As String = ""
        Dim BufAnaly As String = ""
        Try
            If SerialPort1.BytesToRead > 0 Then
                Threading.Thread.Sleep(80) '添加的延时
                Str = Str + SerialPort1.ReadExisting '读取缓冲区中的数据
                SerialPort1.DiscardInBuffer()
                While 1
                    If (InStr(Str, "#") > 0) And (InStr(Str, "!") > 0) Then
                        Str = Microsoft.VisualBasic.Mid(Str, InStr(Str, "#") + 1)
                        If InStr(Str, "!") > 0 Then
                            BufAnaly = Microsoft.VisualBasic.Left(Str, InStr(Str, "!") - 1)
                            SerialDataAnalysis(BufAnaly)
                        End If
                    Else
                        Exit While
                    End If

                End While
            End If

        Catch ex As Exception
            ' MessageBox.Show(ex.Message)
        End Try
    End Sub
    'CRC校验等
    Private Sub SerialDataAnalysis(ByVal str As String)
        Try
            Dim substr(30) As String
            substr = str.Split(",")
            If substr.Length = 22 Then
                If substr(0) = "10" Then
                    For i As Integer = 0 To 20
                        Calib_data1(i) = substr(i + 1)
                        Calib.Label1.Text = "asdf"
                    Next
                    Calib_flag = True
                End If
                If substr(0) = "11" Then
                    For i As Integer = 0 To 20
                        Calib_data2(i) = substr(i + 1)
                        Calib.Label1.Text = "asdf"
                    Next
                    Calib_flag = True
                End If
                If substr(0) = "12" Then
                    For i As Integer = 0 To 20
                        Calib_data3(i) = substr(i + 1)
                        Calib.Label1.Text = "asdf"
                    Next
                    Calib_flag = True
                End If
                If substr(0) = "13" Then
                    For i As Integer = 0 To 20
                        Calib_data4(i) = substr(i + 1)
                        Calib.Label1.Text = "asdf"
                    Next
                    Calib_flag = True
                End If
            End If


            If substr.Length = 16 Then
                totalPointCnt = totalPointCnt + 1
                g_time = substr(2)
                g_data = substr(1)
                g_ntemper = substr(12)
                g_ntemper = g_ntemper / 100
                g_temper = g_ntemper.ToString("f2")
                g_temper += " ℃"

                g_WDir = substr(15)
                g_nWSpeed = substr(14)
                g_nWSpeed = g_nWSpeed / 10
                g_WSpeed = g_nWSpeed.ToString("f1")

                Org_data(0) = substr(3) / 1000
                Org_data(1) = substr(4) / 1000
                Org_data(2) = substr(5) / 1000
                Org_data(3) = substr(6) / 1000

                Org_data(4) = substr(8) / 1000
                Org_data(5) = substr(9) / 1000
                Org_data(6) = substr(10) / 1000
                Org_data(7) = substr(11) / 1000

                Org_data(8) = substr(7) / 1000

            End If

            For i As Integer = 0 To 9
                now_data(i) = Org_data(i) + offset(i)
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub DataAnalysis_gratcnt()
        Dim Str As String = g_SerialData
        Dim date_len As Integer = 8
        Dim boardCode As String = Microsoft.VisualBasic.Left(Str, 2) '地址码
        If (boardCode = "01") Then
            Dim sag1 As String = Microsoft.VisualBasic.Mid(Str, 3, date_len)
            Org_data(0) = Hex_To_Int32(sag1) / 1000
            Dim sag2 As String = Microsoft.VisualBasic.Mid(Str, 3 + date_len, date_len)
            Org_data(1) = Hex_To_Int32(sag2) / 1000
            Dim sag3 As String = Microsoft.VisualBasic.Mid(Str, 3 + date_len + date_len, date_len)
            Org_data(2) = Hex_To_Int32(sag3) / 1000
            Dim sag4 As String = Microsoft.VisualBasic.Mid(Str, 3 + date_len + date_len + date_len, date_len)
            Org_data(3) = Hex_To_Int32(sag4) / 1000
        ElseIf (boardCode = "02") Then
            Dim sbg1 As String = Microsoft.VisualBasic.Mid(Str, 3, date_len)
            Org_data(4) = Hex_To_Int32(sbg1) / 1000
            Dim sbg2 As String = Microsoft.VisualBasic.Mid(Str, 3 + date_len, date_len)
            Org_data(5) = Hex_To_Int32(sbg2) / 1000
            Dim sbg3 As String = Microsoft.VisualBasic.Mid(Str, 3 + date_len + date_len, date_len)
            Org_data(6) = Hex_To_Int32(sbg3) / 1000
            Dim sbg4 As String = Microsoft.VisualBasic.Mid(Str, 3 + date_len + date_len + date_len, date_len)
            Org_data(7) = Hex_To_Int32(sbg4) / 1000

            Dim spa1 As String = Microsoft.VisualBasic.Mid(Str, 3 + date_len + date_len + date_len + date_len, date_len)
            Org_data(8) = Hex_To_Int32(spa1)

            Dim spa2 As String = Microsoft.VisualBasic.Mid(Str, 3 + date_len + date_len + date_len + date_len + date_len, date_len)
            Org_data(9) = Hex_To_Int32(spa2)
        End If

        For i As Integer = 0 To 9
            now_data(i) = Org_data(i) + offset(i)
        Next
    End Sub

    ''' <summary>
    ''' 小于8位的十六进制转有符号十进制（输入0~FFFFFFFF）
    ''' </summary>
    ''' <param name="StrData">十六进制字符串</param>
    ''' <returns>带符号的十进制数</returns>
    ''' <remarks>myfunction</remarks>
    Public Function Hex_To_Int32(ByVal StrData As String) As Integer
        If Val("&H" & Microsoft.VisualBasic.Left(StrData, 1)) >= 7 Then
            StrData = FixStrLength(StrData, 8, "F")
        Else
            StrData = FixStrLength(StrData, 8, "0")
        End If
        Return Hex_To_Int(StrData)
    End Function

    ''' <summary>
    ''' 十六进制转有符号十进制（输入0~FFFFFFFF）
    ''' </summary>
    ''' <param name="StrData">十六进制字符串</param>
    ''' <returns>带符号的十进制数</returns>
    ''' <remarks>myfunction</remarks>
    Public Function Hex_To_Int(ByVal StrData As String) As Integer
        Dim IntData As Integer
        If Val("&H" & Microsoft.VisualBasic.Left(StrData, 1)) > 7 Then
            '最高位为1时，原码变补码
            StrData = (Val("&H" & Microsoft.VisualBasic.Left(StrData, 1)) - 8) & Mid(StrData, 2)
            IntData = -(Val("&H7FFFFFFF") - Val("&H" & StrData) + 1)
        ElseIf Val("&H" & Mid(StrData, 5, 1)) > 7 And Val("&H" & Microsoft.VisualBasic.Left(StrData, 4)) = 0 Then
            '前高16位为0且第15位为1时，补码变原码
            IntData = 2 * Val("&H00007FFF") + Val("&H" & StrData) + 2
        Else
            IntData = Val("&H" & StrData)
        End If

        Return IntData
    End Function

    ''' <summary>
    ''' 固定字符串长度（前面补0）
    ''' </summary>
    ''' <param name="str">要转换的字符串</param>
    ''' <param name="strlength">转换后的字符串长度</param>
    ''' <returns>固定长度的字符串</returns>
    ''' <remarks>myfunction</remarks>
    Public Function FixStrLength(ByVal str As String, ByVal strlength As Integer, Optional ByVal c As Char = "0") As String
        For i As Integer = str.Length To strlength - 1
            str = c & str
        Next
        Return Mid(str, 1, strlength)
    End Function

    '十六进制字符转换成整数
    Private Function StrToDec(ByVal str As String) As Integer
        Dim dec As Integer
        str = "&H" & str
        dec = str
        Return dec
    End Function
    '将int转成2个字符串的string
    Private Function DecToStr2(ByVal data As Integer) As String
        Dim str As String = Hex(data)
        For i As Integer = str.Length To 1
            str = "0" & str
        Next
        Return str
    End Function
    '将string变成固定4为的string
    Private Function StrToStr4(ByVal str As String) As String
        For i As Integer = str.Length To 3
            str = "0" & str
        Next
        Return str
    End Function
    '实时显示 光栅数据
    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick

        AL1.Text = (now_data(0)).ToString("f3")
        AL2.Text = (now_data(1)).ToString("f3")
        AL3.Text = (now_data(2)).ToString("f3")
        AL4.Text = (now_data(3)).ToString("f3")

        BL1.Text = (now_data(4)).ToString("f3")
        BL2.Text = (now_data(5)).ToString("f3")
        BL3.Text = (now_data(6)).ToString("f3")
        BL4.Text = (now_data(7)).ToString("f3")

        'PL1.Text = (now_data(8) * 40 / 5000).ToString("f1")
        'PL2.Text = (now_data(9) * 40 / 5000).ToString("f1")
        PL1.Text = (now_data(8)).ToString("f3")

        LabelDate.Text = g_data
        LabelTime.Text = g_time
        LabelTemp.Text = g_temper
        LabelWSpeed.Text = g_WSpeed
        LabelWDir.Text = g_WDir


        LSPEED.Text = delay_time.ToString & " min"


    End Sub


    Dim lastMs As Integer '上次的毫秒数  
    Dim totalPointCnt As Long = 0 '总点数
    '定时添加数据
    Private Sub Timer4_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer4.Tick
        Dim SumTimeToSav As String
        Dim SumTimeS As Long '秒数开始采集到现在的秒数


        Dim DiffMs As Integer '毫秒差
        Dim NowMs As Integer '当前毫秒
        NowMs = Now.Millisecond + Now.Second * 1000 + Now.Minute * 60000
        NowMs = NowMs Mod (1000 * 60 * 20)

        '考虑翻转的情况
        If NowMs >= lastMs Then
            DiffMs = NowMs - lastMs
        Else
            DiffMs = 1000 * 60 * 20 + NowMs - lastMs '最大20分钟
        End If
        'delay_time = 0.001
        '按照采样频率到达时间后采集数据
        If DiffMs >= delay_time * 1000 * 60 Then
            lastMs += delay_time * 1000 * 60
            lastMs = lastMs Mod (1000 * 60 * 20)

            '添加数据用来显示
            add_data2show()

            '计算开始到现在的总时间
            SumTimeS = DateDiff("s", g_date_Start, Now)
            '总时间转换成字符串
            SumTimeToSav = Int(SumTimeS \ 3600).ToString("D2") & ":" & Int((SumTimeS Mod 3600) \ 60).ToString("D2") & ":" & Int(SumTimeS Mod 60).ToString("D2")
            LSumSamp.Text = SumTimeToSav

            '保存一条数据
            'file_rw_data = DiffMs & "," & Now.Millisecond & "," & Now & "," & SumTimeToSav & "," & delay_time.ToString & "," & Save_data(0, data_index - 1) & "," & Save_data(1, data_index - 1) & "," & Save_data(2, data_index - 1) & "," & Save_data(3, data_index - 1) & "," & Save_data(4, data_index - 1) & "," & Save_data(5, data_index - 1) & "," & Save_data(6, data_index - 1) & "," & Save_data(7, data_index - 1) & "," & show_data(8, data_index - 1) & "," & show_data(9, data_index - 1)
            file_rw_data = Now & "," & SumTimeToSav & "," & delay_time.ToString & "," & Save_data(0, g_Data_Index - 1) & "," & Save_data(1, g_Data_Index - 1) & "," & Save_data(2, g_Data_Index - 1) & "," & Save_data(3, g_Data_Index - 1) & "," & Save_data(4, g_Data_Index - 1) & "," & Save_data(5, g_Data_Index - 1) & "," & Save_data(6, g_Data_Index - 1) & "," & Save_data(7, g_Data_Index - 1) & "," & show_data(8, g_Data_Index - 1) & "," & show_data(9, g_Data_Index - 1)
            If (file_name_now <> "") Then
                WriteText(file_name_now, file_rw_data)
            End If


            LSampCnt.Text = totalPointCnt

            If SerialPort1.IsOpen Then
                SerialPort1.WriteLine("@01,!")
            End If


        End If


    End Sub

    '添加数据到原始数据
    Private Sub add_data2show()
        Dim i As Integer = 0
        For i = 0 To 7
            Save_data(i, g_Data_Index) = now_data(i)
            '先初始化
            OrgPointsGrat(i, g_Data_Index) = New DataPoint
            'mschart用的数据
            OrgPointsGrat(i, g_Data_Index).SetValueXY(DateTime.Now, now_data(i)) '正式用
            'OrgPointsGrat(i, g_Data_Index).SetValueXY(DateTime.Now, 15000 * Math.Sin((g_Data_Index + 3 * i) / 5) / 1000)

        Next
        For i = 8 To 9
            'mschart用的数据
            '先初始化
            OrgPointsP(i - 8, g_Data_Index) = New DataPoint
            'OrgPointsP(i - 8, g_Data_Index).SetValueXY(DateTime.Now, now_data(i))' 正式用
            OrgPointsP(i - 8, g_Data_Index).SetValueXY(DateTime.Now, 200 * (Math.Sin((g_Data_Index + 3 * i) / 5) + 1) / 10)
        Next
        'show_data_time(g_Data_Index) = DateDiff("s", g_date_Start, Now) * 1000 + Now.Millisecond '当前的时间与开始时间相差的毫秒数
        'datediff("s","1970-01-01 00:00:00",now)
        g_Data_Index = g_Data_Index + 1

    End Sub


    '选择串口
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Try
            SerialPort1.Close()
        Catch ex As Exception

        End Try

        SerialPort1.PortName = ComboBox1.SelectedItem.ToString()
        ChangeTxtLine(iniFilePath, 1, SerialPort1.PortName)
        Try
            SerialPort1.Open()
            'SerialPort1.
            MsgBox("串口端口匹配成功，请进行下步操作！", MsgBoxStyle.OkOnly, "提示")
            'B_SendData.Enabled = True
        Catch ex As Exception
            MsgBox(ex.Message & "，请重新选择串口端口！", MsgBoxStyle.OkOnly, "提示")
        End Try

    End Sub
    '关闭窗口
    Private Sub MainForm_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        SerialPort1.Close()
        ChangeTxtLine(iniFilePath, 1, SerialPort1.PortName)
        ChangeTxtLine(iniFilePath, 2, speed_set.Text)

        ChangeTxtLine(iniFilePath, 4, offset(8))
        ChangeTxtLine(iniFilePath, 5, offset(9))
    End Sub
    '关闭程序提示
    Private Sub MainForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If MsgBox("是否退出程序？", MsgBoxStyle.OkCancel, "提示！") = MsgBoxResult.Cancel Then
            e.Cancel = True
        End If
    End Sub

    Private Sub AC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AC1.Click
        'offset(0) = 0 - Org_data(0)
        Calib.Show()
    End Sub

    Private Sub AC2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        offset(1) = 0 - Org_data(1)
    End Sub

    Private Sub AC3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        offset(2) = 0 - Org_data(2)
    End Sub

    Private Sub AC4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        offset(3) = 0 - Org_data(3)
    End Sub

    Private Sub BC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        offset(4) = 0 - Org_data(4)
    End Sub

    Private Sub BC2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        offset(5) = 0 - Org_data(5)
    End Sub

    Private Sub BC3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        offset(6) = 0 - Org_data(6)
    End Sub

    Private Sub BC4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        offset(7) = 0 - Org_data(7)
    End Sub





    Private Sub ACALL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        offset(0) = 0 - Org_data(0)
        offset(1) = 0 - Org_data(1)
        offset(2) = 0 - Org_data(2)
        offset(3) = 0 - Org_data(3)
    End Sub

    Private Sub BCALL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        offset(4) = 0 - Org_data(4)
        offset(5) = 0 - Org_data(5)
        offset(6) = 0 - Org_data(6)
        offset(7) = 0 - Org_data(7)
    End Sub

    Private Sub speed_ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles speed_ok.Click

        If (Val(speed_set.Text) > 20 Or Val(speed_set.Text) < 0.001) Then
            MsgBox("您输入的采样时间不在允许范围内（0.01~20min），请重新输入！", MsgBoxStyle.OkOnly, "提示")
            speed_set.Text = delay_time.ToString
        Else
            delay_time = Val(speed_set.Text)
        End If
    End Sub

    Private Sub time_ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'If (Val(time_set.Text) > 480 Or Val(time_set.Text) < 0.1) Then
        '    MsgBox("您输入的采样时间不在允许范围内（0.1~480h），请重新输入！", MsgBoxStyle.OkOnly, "提示")
        '    time_set.Text = samp_time.ToString
        'Else
        '    samp_time = Val(time_set.Text)
        '    LSampTime.Text = time_set.Text + "h"
        'End If
    End Sub

    Private Sub B_ClearAllGrat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles B_ClearAllGrat.Click
        offset(0) = 0 - Org_data(0)
        offset(1) = 0 - Org_data(1)
        offset(2) = 0 - Org_data(2)
        offset(3) = 0 - Org_data(3)

        offset(4) = 0 - Org_data(4)
        offset(5) = 0 - Org_data(5)
        offset(6) = 0 - Org_data(6)
        offset(7) = 0 - Org_data(7)

        g_Data_Index = 0
        totalPointCnt = 0

        g_date_Start = Now


        date_now = Now.ToString("yyyy年MM月dd日hh时mm分ss秒")
        file_name_now = "C:\my_grat\" & date_now & ".csv"

        Shell("cmd.exe /c md C:\my_grat")
        LoadText(file_name_now, "Date,SumTime,Samp/min,A1/mm,A2/mm,A3/mm,A4/mm,B1/mm,B2/mm,B3/mm,B4/mm,Pa1/MPa,Pa2/MPa")
        lastMs = Now.Millisecond + Now.Second * 1000 + Now.Minute * 60000
        lastMs = lastMs Mod (1000 * 60 * 20)
    End Sub

    Private Sub ACB1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ACB1.CheckedChanged
        show_flag(0) = Not show_flag(0)
    End Sub

    Private Sub ACB2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ACB2.CheckedChanged
        show_flag(1) = Not show_flag(1)
    End Sub

    Private Sub ACB3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ACB3.CheckedChanged
        show_flag(2) = Not show_flag(2)
    End Sub

    Private Sub ACB4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ACB4.CheckedChanged
        show_flag(3) = Not show_flag(3)
    End Sub

    Private Sub BCB1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BCB1.CheckedChanged
        show_flag(4) = Not show_flag(4)
    End Sub

    Private Sub BCB2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BCB2.CheckedChanged
        show_flag(5) = Not show_flag(5)
    End Sub

    Private Sub BCB3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BCB3.CheckedChanged
        show_flag(6) = Not show_flag(6)
    End Sub

    Private Sub BCB4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BCB4.CheckedChanged
        show_flag(7) = Not show_flag(7)
    End Sub

    Private Sub PCB1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PCB1.CheckedChanged
        show_flag(8) = Not show_flag(8)
    End Sub

    Private Sub PCB2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        show_flag(9) = Not show_flag(9)
    End Sub

    Private Sub B_SAVE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles B_SAVE.Click
        Dim FilePath_x As String
        Dim FileContent_x As String

        SaveFileDialog1.Filter = "Csv Documents （*.csv）|*.csv|Text Documents （*.txt）|*.txt"
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then

            FilePath_x = SaveFileDialog1.FileName
            FileContent_x = "cmd.exe /c copy " & file_name_now & " " & FilePath_x
            Shell(FileContent_x)
            MsgBox("数据导出成功！", MsgBoxStyle.OkOnly, "提示")
        End If

    End Sub

    'Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim str_tmp As String
    '    str_tmp = ComboBox2.SelectedItem.ToString()
    '    GratCnt_max = Val(str_tmp)
    '    Dim ii As Integer = 0
    '    Dim jj As Integer = 0
    '    For ii = 0 To 7
    '        For jj = 0 To data_index
    '            If GratCnt_max = 15000 Then
    '                show_data(ii, jj) = (Save_data(ii, jj) + 15) / 2 * 1000
    '            ElseIf GratCnt_max = 1500 Then
    '                show_data(ii, jj) = (Save_data(ii, jj) + 1.5) / 2 * 1000
    '            ElseIf GratCnt_max = 150 Then
    '                show_data(ii, jj) = (Save_data(ii, jj) + 0.15) / 2 * 1000
    '            End If
    '        Next

    '    Next

    'End Sub

    Private Sub time_set_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Label6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    ''A显示曲线
    'Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    show_flag_A = Not show_flag_A
    '    If show_flag_A Then
    '        Button2.Text = "关闭曲线"

    '        ACB1.Checked = True
    '        ACB2.Checked = True
    '        ACB3.Checked = True
    '        ACB4.Checked = True
    '        show_flag(0) = True
    '        show_flag(1) = True
    '        show_flag(2) = True
    '        show_flag(3) = True
    '    Else
    '        Button2.Text = "显示曲线"
    '        ACB1.Checked = False
    '        ACB2.Checked = False
    '        ACB3.Checked = False
    '        ACB4.Checked = False
    '        show_flag(0) = False
    '        show_flag(1) = False
    '        show_flag(2) = False
    '        show_flag(3) = False
    '    End If


    'End Sub

    ''B显示曲线
    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    show_flag_B = Not show_flag_B
    '    If show_flag_B Then
    '        Button1.Text = "关闭曲线"
    '        BCB1.Checked = True
    '        BCB2.Checked = True
    '        BCB3.Checked = True
    '        BCB4.Checked = True
    '        show_flag(4) = True
    '        show_flag(5) = True
    '        show_flag(6) = True
    '        show_flag(7) = True
    '    Else
    '        Button1.Text = "显示曲线"
    '        BCB1.Checked = False
    '        BCB2.Checked = False
    '        BCB3.Checked = False
    '        BCB4.Checked = False
    '        show_flag(4) = False
    '        show_flag(5) = False
    '        show_flag(6) = False
    '        show_flag(7) = False
    '    End If

    'End Sub

    Dim autoTrackFlag As Boolean
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If autoTrackFlag Then
            autoTrackFlag = False
        Else
            autoTrackFlag = True
        End If
    End Sub

    'Private Sub Chart1_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Chart1.KeyPress, MyBase.KeyPress

    '    If e.KeyChar.Equals(Keys.ControlKey) Then
    '        Chart1.ChartAreas(0).CursorX.IsUserSelectionEnabled = True
    '        Chart1.ChartAreas(0).CursorY.IsUserSelectionEnabled = True
    '    Else
    '        Chart1.ChartAreas(0).CursorX.IsUserSelectionEnabled = False
    '        Chart1.ChartAreas(0).CursorY.IsUserSelectionEnabled = False
    '    End If
    'End Sub

    'Private Sub MainForm_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
    '    If e.KeyChar.Equals(Keys.ControlKey) Then
    '        Chart1.ChartAreas(0).CursorX.IsUserSelectionEnabled = True
    '        Chart1.ChartAreas(0).CursorY.IsUserSelectionEnabled = True
    '    Else
    '        Chart1.ChartAreas(0).CursorX.IsUserSelectionEnabled = False
    '        Chart1.ChartAreas(0).CursorY.IsUserSelectionEnabled = False
    '    End If
    'End Sub

    Private Sub MainForm_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Keys.Z Then
            Chart1.ChartAreas(0).CursorX.IsUserSelectionEnabled = True
            Chart1.ChartAreas(0).CursorY.IsUserSelectionEnabled = True
        End If
    End Sub

    Private Sub MainForm_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyValue = Keys.Z Then
            Chart1.ChartAreas(0).CursorX.IsUserSelectionEnabled = False
            Chart1.ChartAreas(0).CursorY.IsUserSelectionEnabled = False
        End If
    End Sub

    Private Sub B_ClearAllGrat_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles B_ClearAllGrat.MouseDown
        'B_ClearAllGrat.BackgroundImage = DataAcquisition.My.Resources.Resources.but1Down
    End Sub

    Private Sub B_ClearAllGrat_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles B_ClearAllGrat.MouseUp
        ' B_ClearAllGrat.BackgroundImage = DataAcquisition.My.Resources.Resources.but1
    End Sub

    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles L_CursorY.Click, L_End.Click
        'Chart1.Series(0).ChartType = SeriesChartType.FastLine
    End Sub

    Private Sub Chart1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart1.Click
        If Double.IsNaN(Chart1.ChartAreas(0).CursorX.Position) Or Double.IsNaN(Chart1.ChartAreas(0).CursorY.Position) Then
            L_CursorX.Text = "X： "
            L_CursorY.Text = "Y： "
        Else
            L_CursorX.Text = "X： " + DateTime.FromOADate(Chart1.ChartAreas(0).CursorX.Position).ToString("HH:mm:ss")
            L_CursorY.Text = "Y： " + Chart1.ChartAreas(0).CursorY.Position.ToString("f3") + " mm"
        End If

    End Sub



    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        P_D_Chart.Show()
    End Sub

    Private Sub WeightCalib1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WeightCalib1.Click
        SerialPort1.WriteLine("@30,!")
    End Sub

    Private Sub WeightCalib2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WeightCalib2.Click
        Dim cmdstr As String
        Dim load As Integer
        load = PT1.Text
        cmdstr = "@40,"
        cmdstr += load.ToString("d")
        cmdstr += ",!"
        SerialPort1.WriteLine(cmdstr)
    End Sub
End Class

