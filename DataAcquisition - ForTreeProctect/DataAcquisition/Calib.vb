Public Class Calib

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rd0.Click, rd3.Click, rd2.Click, rd1.Click
        Dim str As String
        str = "@1" + Microsoft.VisualBasic.Right(sender.name, 1) + ",!"


        Form.SerialPort1.WriteLine(str)



        'ListView1.Items.Item(0).SubItems(0).Text = "SDFASD"


    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles wr0.Click, wr3.Click, wr2.Click, wr1.Click
        Dim str As String

        str = "@2" + Microsoft.VisualBasic.Right(sender.name, 1) + ","

        If Microsoft.VisualBasic.Right(sender.name, 1) = "0" Then
            For i As Integer = 0 To 20
                str = str + ListView1.Items.Item(i).SubItems(0).Text + ","
            Next
        End If

        If Microsoft.VisualBasic.Right(sender.name, 1) = "1" Then
            For i As Integer = 0 To 20
                str = str + ListView2.Items.Item(i).SubItems(0).Text + ","
            Next
        End If

        If Microsoft.VisualBasic.Right(sender.name, 1) = "2" Then
            For i As Integer = 0 To 20
                str = str + ListView3.Items.Item(i).SubItems(0).Text + ","
            Next
        End If

        If Microsoft.VisualBasic.Right(sender.name, 1) = "3" Then
            For i As Integer = 0 To 20
                str = str + ListView4.Items.Item(i).SubItems(0).Text + ","
            Next
        End If

        str = str + "!"
        Form.SerialPort1.WriteLine(str)
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Label1.Text = Form.AL1.Text
        Label2.Text = Form.AL2.Text
        Label3.Text = Form.AL3.Text
        Label4.Text = Form.AL4.Text

        If Form.Calib_flag Then

            For i As Integer = 0 To 20
                ListView1.Items.Item(i).SubItems(0).Text = Form.Calib_data1(i)
                ListView2.Items.Item(i).SubItems(0).Text = Form.Calib_data2(i)
                ListView3.Items.Item(i).SubItems(0).Text = Form.Calib_data3(i)
                ListView4.Items.Item(i).SubItems(0).Text = Form.Calib_data4(i)
            Next
            Form.Calib_flag = False

        End If
    End Sub


    Private Sub Calib_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        With (ListView1)
            .Clear()
            .View = View.Details
            .FullRowSelect = True
            .Columns.Add("实际位移量mm", 110)
            .Columns.Add("电压值mV", 80)
        End With

        For i As Integer = 0 To 20
            Dim itm As ListViewItem = ListView1.Items.Add((-5000 + i * 500).ToString)
            itm.SubItems.AddRange({(-5000 + i * 500).ToString})
        Next

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''列表2                                               ''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        With (ListView2)
            .Clear()
            .View = View.Details
            .FullRowSelect = True
            .Columns.Add("实际位移量mm", 110)
            .Columns.Add("电压值mV", 80)


        End With

        For i As Integer = 0 To 20
            Dim itm As ListViewItem = ListView2.Items.Add((-5000 + i * 500).ToString)
            itm.SubItems.AddRange({(-5000 + i * 500).ToString})
        Next

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''列表3                                               ''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        With (ListView3)
            .Clear()
            .View = View.Details
            .FullRowSelect = True
            .Columns.Add("实际位移量mm", 110)
            .Columns.Add("电压值mV", 80)


        End With

        For i As Integer = 0 To 20
            Dim itm As ListViewItem = ListView3.Items.Add((-5000 + i * 500).ToString)
            itm.SubItems.AddRange({(-5000 + i * 500).ToString})
        Next

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''列表4                                               ''''''''''''''''''''''''''''''
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        With (ListView4)
            .Clear()
            .View = View.Details
            .FullRowSelect = True
            .Columns.Add("实际位移量mm", 110)
            .Columns.Add("电压值mV", 80)


        End With

        For i As Integer = 0 To 20
            Dim itm As ListViewItem = ListView4.Items.Add((-5000 + i * 500).ToString)
            itm.SubItems.AddRange({(-5000 + i * 500).ToString})
        Next

    End Sub


End Class