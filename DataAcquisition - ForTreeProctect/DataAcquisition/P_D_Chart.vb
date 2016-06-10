Imports System.Windows.Forms.DataVisualization.Charting

Public Class P_D_Chart



    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
       
    End Sub


    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Dim interval As Integer = 1
        'g_Data_Index()
        Dim PId As Integer = 0
        PId = g_Data_Index '当前数据总个数
        If PId <= 2 Then
            Exit Sub
        End If
        PId -= 1

        '清掉数据重新加载

        Chart1.Series(0).Points.Clear()

        '整数倍的时候才改变一次间距
        If (PId Mod MaxPointNum) = 0 And (PId / MaxPointNum) <> 0 Then
            interval = PId / MaxPointNum
        End If

        '根据算的间隔 把原始数据往图表数据上套
        Dim i As Integer = 0
        If PId < MaxPointNum Then
            For i = 0 To PId
                Dim tmpPoint As New DataPoint
                tmpPoint.XValue = OrgPointsP(ComboBox2.SelectedIndex, i * interval).YValues(0)
                tmpPoint.YValues(0) = OrgPointsGrat(ComboBox2.SelectedIndex, i * interval).YValues(0)
                Chart1.Series(0).Points.Add(tmpPoint) '位移

            Next
        Else
            For i = 0 To (PId / interval) - 1
                Dim tmpPoint As New DataPoint
                tmpPoint.XValue = OrgPointsGrat(ComboBox2.SelectedIndex, i * interval).YValues(0)
                tmpPoint.YValues(0) = OrgPointsGrat(ComboBox2.SelectedIndex, i * interval).YValues(0)
                Chart1.Series(0).Points.Add(tmpPoint) '位移
            Next
        End If


    End Sub

    Private Sub P_D_Chart_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim str As String

        For i As Integer = 0 To 3
            Str = "A" + (i + 1).ToString
            ComboBox1.Items.Add(str)
        Next
        For i As Integer = 4 To 7
            Str = "B" + (i - 4 + 1).ToString
            ComboBox1.Items.Add(str)
        Next
        For i As Integer = 0 To 1
            str = "Pa" + (i + 1).ToString
            ComboBox2.Items.Add(str)
        Next
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0


    End Sub

    Private Sub Chart1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart1.Click
        If Double.IsNaN(Chart1.ChartAreas(0).CursorX.Position) Or Double.IsNaN(Chart1.ChartAreas(0).CursorY.Position) Then
            L_CursorX.Text = "X： "
            L_CursorY.Text = "Y： "
        Else
            L_CursorX.Text = "X： " + Chart1.ChartAreas(0).CursorX.Position.ToString("f1") + " Pa"
            L_CursorY.Text = "Y： " + Chart1.ChartAreas(0).CursorY.Position.ToString("f3") + " mm"
        End If
    End Sub


End Class