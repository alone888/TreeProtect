Imports System.Data.OleDb
Imports System.Windows.Forms
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.DataVisualization.Charting


Public Class Form
    Public CrcClass As New CRC16 'CRC16����
    Public Crc16Data As String 'crcУ����
    Public Declare Function timeGetTime Lib "winmm.dll" () As Long
    Public Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)
    Dim DispDec(999) As Decimal '��ʵ����
    Dim SendBytes(999) As String '��������
    Dim g_SerialData As String '���ڽ��յ�������
    Dim SendFlag(39) As Boolean '���ڷ������ݵı�־
    Dim PulseFlag As Boolean '���������־
    Dim StateFlag As Boolean '����״̬��־
    Dim ContinuousFlag As Boolean '�����������Σ�ֹͣ���Σ���־
    Dim g_DataFlag As Boolean = False '��ͼ��־
    Dim DecOff As Double = 4 / 2 '����ƫ����
    Dim MultipleNum As Double = 4094 / 4 '��ʵ�����򴮿����ݵı���

    Dim state As Boolean 'FPGA�Ƿ�æµ״̬
    Dim KeyFlag As Boolean = False '�������Σ�ֹͣ���Σ�ת����־

    Dim FitDeci(999) As Decimal '��ԭʼ��������ϻ��߲���ϵõ��Ľ��
    Dim FitFlag As Boolean = False '��ϱ�־
    Dim MaxShowPointNum As Integer = 1000 '����ͨ����ʱ�䷽���������ʾ���ݵ���
    Dim MsPerPoint As Long = 0 'ÿ�����Ӧ�ĺ�����
    Dim MaxSampPointNum As Integer '�ܹ��Ĳ�������  �����ܲ���ʱ��Ͳ������ڼ���

    '�궨����
    Public Calib_data1(21) As String
    Public Calib_data2(21) As String
    Public Calib_data3(21) As String
    Public Calib_data4(21) As String
    Public Calib_flag As Boolean

    Public g_time, g_data, g_temper, g_WDir, g_WSpeed As String
    Public g_ntemper As Double
    Public g_nWSpeed As Double



    Dim GratCnt_max As Integer = 15000
    'ʵʱ��ʾ��ǰֵ
    Dim now_data(10) As Double
    'ԭʼ����
    Dim Org_data(10) As Double

    '����  ---   �궨
    Dim offset(10) As Double


    Dim pa1_offset_dem As Double = 0
    Dim pa2_offset_dem As Double = 0

    Dim delay_time As Double = 0.01
    Dim samp_time As Double = 0.1

    Dim timercnt As UInt32 = 0
    '��ͼ��־
    Dim show_flag(10) As Boolean

    Dim show_flag_A As Boolean = False

    Dim show_flag_B As Boolean = False
    Dim MAX_DATA_NUM As Integer = 500000

    'ʵʱ��ͼ����
    Dim show_data(10, MAX_DATA_NUM) As Double '0---9
    Dim show_data_time(MAX_DATA_NUM) As Long '��Ӧ��ʱ��
    Dim Save_data(10, MAX_DATA_NUM) As Double '0---9


    Dim show_name(10) As String
    Dim date_now As String
    Dim g_date_Start As Date = Now '���βɼ���ʼ��ʱ��
    Dim file_name_now As String
    Dim file_rw_data As String

    Dim iniFilePath As String
    Dim COMID As String
    Dim iniStr(10) As String

    '��������
    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '��ȡ��һ�ε������ļ�
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

            'ÿ�����ݵ�ʵ������
            SerialPort1.PortName = iniStr(0)
            delay_time = iniStr(1)
            samp_time = iniStr(2)
            offset(8) = iniStr(3)
            offset(9) = iniStr(4)
        Catch

        End Try


        Dim str As String
        '��⴮���Ƿ��
        Try
            SerialPort1.Open()
            'MsgBox("���ڶ˿�ƥ��ɹ���������²�������", MsgBoxStyle.OkOnly, "��ʾ")
        Catch ex As Exception
            'MsgBox(ex.Message & "��������ѡ�񴮿ڶ˿ڣ�", MsgBoxStyle.OkOnly, "��ʾ")
        End Try


        '���ڶ˿�����
        For i As Integer = 1 To 20
            str = "COM" & i
            ComboBox1.Items.Add(str)
        Next

        ComboBox1.SelectedText = SerialPort1.PortName


        '***************************************************
        '��ǰʱ����Ϊ�����ļ���ʱ��
        '***************************************************
        date_now = Now
        date_now = CStr(Year(date_now)) & CStr(Month(date_now)) & CStr(DateAndTime.Day(date_now)) & CStr(Hour(date_now)) & CStr(Minute(date_now)) & CStr(Second(date_now))
        'date_now = Format(date_now, "yyyy/mm/dd HH:MM:ss")
        file_name_now = "C:\my_grat\Start" & date_now & ".txt"
        'Shell("cmd.exe /c md C:\my_grat")
        LoadText(file_name_now, "Date,SunTime,Samp/min,A1/mm,A2/mm,A3/mm,A4/mm,B1/mm,B2/mm,B3/mm,B4/mm,Pa1/MPa,Pa2/MPa")

        '����ʱ��,���ھ���������
        lastMs = Now.Millisecond + Now.Second * 1000 + Now.Minute * 60000
        lastMs = lastMs Mod (1000 * 60 * 20)

        '***************************************************
        '���û�ͼ���������
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


        'X����Сѡ������()  ��λ����̫������ ������
        'Chart1.ChartAreas(0).AxisX.ScaleView.MinSize = 5 '5����������
        'Chart1.ChartAreas(0).AxisX.ScaleView.MinSizeType = DateTimeIntervalType.Seconds


    End Sub

    'ʵʱ��ʾ ʱ������
    Dim PId As Integer = 0

    Dim interval As Integer = 1
    Dim selectedInterval As Integer = 1
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        '�����Ƿ���ʾĳ������
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




        PId = g_Data_Index '��ǰ�����ܸ���
        If PId <= 2 Then
            Exit Sub
        End If
        PId -= 1




        '����������¼���
        For j As Integer = 0 To 9
            Chart1.Series(j).Points.Clear()
        Next


        '��������ʱ��Ÿı�һ�μ��
        If (PId Mod MaxPointNum) = 0 And (PId / MaxPointNum) <> 0 Then
            interval = PId / MaxPointNum
        End If

        '������ļ�� ��ԭʼ������ͼ����������
        Dim i As Integer = 0
        If PId < MaxPointNum Then
            For i = 0 To PId
                For j As Integer = 0 To 7
                    Chart1.Series(j).Points.Add(OrgPointsGrat(j, i * interval)) 'λ��
                Next
                For j As Integer = 8 To 9
                    Chart1.Series(j).Points.Add(OrgPointsP(j - 8, i * interval)) 'ѹ��
                Next
            Next
        Else
            For i = 0 To (PId / interval) - 1
                For j As Integer = 0 To 7
                    Chart1.Series(j).Points.Add(OrgPointsGrat(j, i * interval))
                Next
                For j As Integer = 8 To 9
                    Chart1.Series(j).Points.Add(OrgPointsP(j - 8, i * interval)) 'ѹ��
                Next
            Next
        End If

        '������ݺ����������ʼ����������--todo
        Chart1.ChartAreas(0).AxisX.Minimum = Chart1.Series(0).Points(0).XValue

        '***************************************************
        '��X��̶ȼ���
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
        'Y����С����Ŀ���
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
        '�Ƿ��������
        '***************************************************
        'Label4.Text = Chart1.Series(0).Points.Count
        If autoTrackFlag Then
            Chart1.ChartAreas(0).AxisX.ScaleView.Position = OrgPointsGrat(0, PId).XValue - Chart1.ChartAreas(0).AxisX.ScaleView.Size
        End If




        '***************************************************
        'ѡ����������ݴ���
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


            '���ѡ�п��ԭʼ����
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

            '�����������ݵĸ���
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

            '�Ŵ���Ƿ���Ҫ�����ʾ�����
            If (orgEndId - orgStartId) > MaxPointNum Then
                selectedInterval = (orgEndId - orgStartId) / MaxPointNum
            End If

            '������������
            'Dim SeriesId As Integer
            Dim OrgDateID As Integer = orgStartId
            For SeriesId = SeriesStartId To PId '����ʼ�㿪ʼ��������
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
        '���ÿ�����������ʾ
        '***************************************************
        Dim nPoints As DataPointCollection = Chart1.Series(0).Points
        Dim PointTime As DateTime = DateTime.FromOADate(nPoints(nPoints.Count - 1).XValue) 'ʱ��
        For tipId As Integer = 0 To nPoints.Count - 1
            For j As Integer = 0 To 3
                If show_flag(j) Then
                    Chart1.Series(j).Points(tipId).ToolTip = "( " + PointTime.ToString("HH:mm:ss") + ", A" + (j + 1).ToString + ": " + Chart1.Series(j).Points(tipId).YValues(0).ToString("f3") + " )" '��ʱ�䣬��ֵ��
                End If
            Next
            For j As Integer = 4 To 7
                If show_flag(j) Then
                    Chart1.Series(j).Points(tipId).ToolTip = "( " + PointTime.ToString("HH:mm:ss") + ", B" + (j - 4 + 1).ToString + ": " + Chart1.Series(j).Points(tipId).YValues(0).ToString("f3") + " )" '��ʱ�䣬��ֵ��
                End If
            Next
            For j As Integer = 8 To 9
                If show_flag(j) Then
                    Chart1.Series(j).Points(tipId).ToolTip = "( " + PointTime.ToString("HH:mm:ss") + ", Pa" + (j - 8 + 1).ToString + ": " + Chart1.Series(j).Points(tipId).YValues(0).ToString("f1") + " )" '��ʱ�䣬��ֵ��
                End If
            Next
        Next

    End Sub


    '���ڽ���
    Private Sub SerialPort1_DataReceived(ByVal sender As System.Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        Dim Str As String = ""
        Dim BufAnaly As String = ""
        Try
            If SerialPort1.BytesToRead > 0 Then
                Threading.Thread.Sleep(80) '��ӵ���ʱ
                Str = Str + SerialPort1.ReadExisting '��ȡ�������е�����
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
    'CRCУ���
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
                g_temper += " ��"

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
        Dim boardCode As String = Microsoft.VisualBasic.Left(Str, 2) '��ַ��
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
    ''' С��8λ��ʮ������ת�з���ʮ���ƣ�����0~FFFFFFFF��
    ''' </summary>
    ''' <param name="StrData">ʮ�������ַ���</param>
    ''' <returns>�����ŵ�ʮ������</returns>
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
    ''' ʮ������ת�з���ʮ���ƣ�����0~FFFFFFFF��
    ''' </summary>
    ''' <param name="StrData">ʮ�������ַ���</param>
    ''' <returns>�����ŵ�ʮ������</returns>
    ''' <remarks>myfunction</remarks>
    Public Function Hex_To_Int(ByVal StrData As String) As Integer
        Dim IntData As Integer
        If Val("&H" & Microsoft.VisualBasic.Left(StrData, 1)) > 7 Then
            '���λΪ1ʱ��ԭ��䲹��
            StrData = (Val("&H" & Microsoft.VisualBasic.Left(StrData, 1)) - 8) & Mid(StrData, 2)
            IntData = -(Val("&H7FFFFFFF") - Val("&H" & StrData) + 1)
        ElseIf Val("&H" & Mid(StrData, 5, 1)) > 7 And Val("&H" & Microsoft.VisualBasic.Left(StrData, 4)) = 0 Then
            'ǰ��16λΪ0�ҵ�15λΪ1ʱ�������ԭ��
            IntData = 2 * Val("&H00007FFF") + Val("&H" & StrData) + 2
        Else
            IntData = Val("&H" & StrData)
        End If

        Return IntData
    End Function

    ''' <summary>
    ''' �̶��ַ������ȣ�ǰ�油0��
    ''' </summary>
    ''' <param name="str">Ҫת�����ַ���</param>
    ''' <param name="strlength">ת������ַ�������</param>
    ''' <returns>�̶����ȵ��ַ���</returns>
    ''' <remarks>myfunction</remarks>
    Public Function FixStrLength(ByVal str As String, ByVal strlength As Integer, Optional ByVal c As Char = "0") As String
        For i As Integer = str.Length To strlength - 1
            str = c & str
        Next
        Return Mid(str, 1, strlength)
    End Function

    'ʮ�������ַ�ת��������
    Private Function StrToDec(ByVal str As String) As Integer
        Dim dec As Integer
        str = "&H" & str
        dec = str
        Return dec
    End Function
    '��intת��2���ַ�����string
    Private Function DecToStr2(ByVal data As Integer) As String
        Dim str As String = Hex(data)
        For i As Integer = str.Length To 1
            str = "0" & str
        Next
        Return str
    End Function
    '��string��ɹ̶�4Ϊ��string
    Private Function StrToStr4(ByVal str As String) As String
        For i As Integer = str.Length To 3
            str = "0" & str
        Next
        Return str
    End Function
    'ʵʱ��ʾ ��դ����
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


    Dim lastMs As Integer '�ϴεĺ�����  
    Dim totalPointCnt As Long = 0 '�ܵ���
    '��ʱ�������
    Private Sub Timer4_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer4.Tick
        Dim SumTimeToSav As String
        Dim SumTimeS As Long '������ʼ�ɼ������ڵ�����


        Dim DiffMs As Integer '�����
        Dim NowMs As Integer '��ǰ����
        NowMs = Now.Millisecond + Now.Second * 1000 + Now.Minute * 60000
        NowMs = NowMs Mod (1000 * 60 * 20)

        '���Ƿ�ת�����
        If NowMs >= lastMs Then
            DiffMs = NowMs - lastMs
        Else
            DiffMs = 1000 * 60 * 20 + NowMs - lastMs '���20����
        End If
        'delay_time = 0.001
        '���ղ���Ƶ�ʵ���ʱ���ɼ�����
        If DiffMs >= delay_time * 1000 * 60 Then
            lastMs += delay_time * 1000 * 60
            lastMs = lastMs Mod (1000 * 60 * 20)

            '�������������ʾ
            add_data2show()

            '���㿪ʼ�����ڵ���ʱ��
            SumTimeS = DateDiff("s", g_date_Start, Now)
            '��ʱ��ת�����ַ���
            SumTimeToSav = Int(SumTimeS \ 3600).ToString("D2") & ":" & Int((SumTimeS Mod 3600) \ 60).ToString("D2") & ":" & Int(SumTimeS Mod 60).ToString("D2")
            LSumSamp.Text = SumTimeToSav

            '����һ������
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

    '������ݵ�ԭʼ����
    Private Sub add_data2show()
        Dim i As Integer = 0
        For i = 0 To 7
            Save_data(i, g_Data_Index) = now_data(i)
            '�ȳ�ʼ��
            OrgPointsGrat(i, g_Data_Index) = New DataPoint
            'mschart�õ�����
            OrgPointsGrat(i, g_Data_Index).SetValueXY(DateTime.Now, now_data(i)) '��ʽ��
            'OrgPointsGrat(i, g_Data_Index).SetValueXY(DateTime.Now, 15000 * Math.Sin((g_Data_Index + 3 * i) / 5) / 1000)

        Next
        For i = 8 To 9
            'mschart�õ�����
            '�ȳ�ʼ��
            OrgPointsP(i - 8, g_Data_Index) = New DataPoint
            'OrgPointsP(i - 8, g_Data_Index).SetValueXY(DateTime.Now, now_data(i))' ��ʽ��
            OrgPointsP(i - 8, g_Data_Index).SetValueXY(DateTime.Now, 200 * (Math.Sin((g_Data_Index + 3 * i) / 5) + 1) / 10)
        Next
        'show_data_time(g_Data_Index) = DateDiff("s", g_date_Start, Now) * 1000 + Now.Millisecond '��ǰ��ʱ���뿪ʼʱ�����ĺ�����
        'datediff("s","1970-01-01 00:00:00",now)
        g_Data_Index = g_Data_Index + 1

    End Sub


    'ѡ�񴮿�
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
            MsgBox("���ڶ˿�ƥ��ɹ���������²�������", MsgBoxStyle.OkOnly, "��ʾ")
            'B_SendData.Enabled = True
        Catch ex As Exception
            MsgBox(ex.Message & "��������ѡ�񴮿ڶ˿ڣ�", MsgBoxStyle.OkOnly, "��ʾ")
        End Try

    End Sub
    '�رմ���
    Private Sub MainForm_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        SerialPort1.Close()
        ChangeTxtLine(iniFilePath, 1, SerialPort1.PortName)
        ChangeTxtLine(iniFilePath, 2, speed_set.Text)

        ChangeTxtLine(iniFilePath, 4, offset(8))
        ChangeTxtLine(iniFilePath, 5, offset(9))
    End Sub
    '�رճ�����ʾ
    Private Sub MainForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If MsgBox("�Ƿ��˳�����", MsgBoxStyle.OkCancel, "��ʾ��") = MsgBoxResult.Cancel Then
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
            MsgBox("������Ĳ���ʱ�䲻������Χ�ڣ�0.01~20min�������������룡", MsgBoxStyle.OkOnly, "��ʾ")
            speed_set.Text = delay_time.ToString
        Else
            delay_time = Val(speed_set.Text)
        End If
    End Sub

    Private Sub time_ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'If (Val(time_set.Text) > 480 Or Val(time_set.Text) < 0.1) Then
        '    MsgBox("������Ĳ���ʱ�䲻������Χ�ڣ�0.1~480h�������������룡", MsgBoxStyle.OkOnly, "��ʾ")
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


        date_now = Now.ToString("yyyy��MM��dd��hhʱmm��ss��")
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

        SaveFileDialog1.Filter = "Csv Documents ��*.csv��|*.csv|Text Documents ��*.txt��|*.txt"
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then

            FilePath_x = SaveFileDialog1.FileName
            FileContent_x = "cmd.exe /c copy " & file_name_now & " " & FilePath_x
            Shell(FileContent_x)
            MsgBox("���ݵ����ɹ���", MsgBoxStyle.OkOnly, "��ʾ")
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

    ''A��ʾ����
    'Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    show_flag_A = Not show_flag_A
    '    If show_flag_A Then
    '        Button2.Text = "�ر�����"

    '        ACB1.Checked = True
    '        ACB2.Checked = True
    '        ACB3.Checked = True
    '        ACB4.Checked = True
    '        show_flag(0) = True
    '        show_flag(1) = True
    '        show_flag(2) = True
    '        show_flag(3) = True
    '    Else
    '        Button2.Text = "��ʾ����"
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

    ''B��ʾ����
    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    show_flag_B = Not show_flag_B
    '    If show_flag_B Then
    '        Button1.Text = "�ر�����"
    '        BCB1.Checked = True
    '        BCB2.Checked = True
    '        BCB3.Checked = True
    '        BCB4.Checked = True
    '        show_flag(4) = True
    '        show_flag(5) = True
    '        show_flag(6) = True
    '        show_flag(7) = True
    '    Else
    '        Button1.Text = "��ʾ����"
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
            L_CursorX.Text = "X�� "
            L_CursorY.Text = "Y�� "
        Else
            L_CursorX.Text = "X�� " + DateTime.FromOADate(Chart1.ChartAreas(0).CursorX.Position).ToString("HH:mm:ss")
            L_CursorY.Text = "Y�� " + Chart1.ChartAreas(0).CursorY.Position.ToString("f3") + " mm"
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

