<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim StripLine1 As System.Windows.Forms.DataVisualization.Charting.StripLine = New System.Windows.Forms.DataVisualization.Charting.StripLine()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim DataPoint1 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0.0R, 0.0R)
        Dim DataPoint2 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0.041666666666666664R, 12.0R)
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series3 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series4 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series5 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series6 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series7 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series8 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series9 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series10 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form))
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.speed_ok = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.BL2 = New System.Windows.Forms.Label()
        Me.BL1 = New System.Windows.Forms.Label()
        Me.BCB4 = New System.Windows.Forms.CheckBox()
        Me.BL4 = New System.Windows.Forms.Label()
        Me.BCB3 = New System.Windows.Forms.CheckBox()
        Me.BL3 = New System.Windows.Forms.Label()
        Me.BCB2 = New System.Windows.Forms.CheckBox()
        Me.BCB1 = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.WeightCalib1 = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.PT1 = New System.Windows.Forms.TextBox()
        Me.WeightCalib2 = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.PL1 = New System.Windows.Forms.Label()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.speed_set = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.AC1 = New System.Windows.Forms.Button()
        Me.AL2 = New System.Windows.Forms.Label()
        Me.AL1 = New System.Windows.Forms.Label()
        Me.AL4 = New System.Windows.Forms.Label()
        Me.AL3 = New System.Windows.Forms.Label()
        Me.ACB4 = New System.Windows.Forms.CheckBox()
        Me.ACB3 = New System.Windows.Forms.CheckBox()
        Me.ACB2 = New System.Windows.Forms.CheckBox()
        Me.ACB1 = New System.Windows.Forms.CheckBox()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.LSPEED = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.B_SAVE = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.LSampCnt = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.LSumSamp = New System.Windows.Forms.Label()
        Me.Timer4 = New System.Windows.Forms.Timer(Me.components)
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.L_CursorX = New System.Windows.Forms.Label()
        Me.B_ClearAllGrat = New System.Windows.Forms.Button()
        Me.L_CursorY = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.L_Start = New System.Windows.Forms.Label()
        Me.L_End = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.LabelTime = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.LabelTemp = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.LabelDate = New System.Windows.Forms.Label()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.LabelWDir = New System.Windows.Forms.Label()
        Me.LabelWSpeed = New System.Windows.Forms.Label()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox6.SuspendLayout()
        Me.SuspendLayout()
        '
        'SerialPort1
        '
        Me.SerialPort1.BaudRate = 115200
        Me.SerialPort1.PortName = "COM5"
        Me.SerialPort1.WriteBufferSize = 4096
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 300
        '
        'speed_ok
        '
        Me.speed_ok.Location = New System.Drawing.Point(126, 17)
        Me.speed_ok.Name = "speed_ok"
        Me.speed_ok.Size = New System.Drawing.Size(54, 33)
        Me.speed_ok.TabIndex = 20
        Me.speed_ok.Text = "确定"
        Me.speed_ok.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.BL2)
        Me.GroupBox2.Controls.Add(Me.BL1)
        Me.GroupBox2.Controls.Add(Me.BCB4)
        Me.GroupBox2.Controls.Add(Me.BL4)
        Me.GroupBox2.Controls.Add(Me.BCB3)
        Me.GroupBox2.Controls.Add(Me.BL3)
        Me.GroupBox2.Controls.Add(Me.BCB2)
        Me.GroupBox2.Controls.Add(Me.BCB1)
        Me.GroupBox2.Location = New System.Drawing.Point(149, 435)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(103, 261)
        Me.GroupBox2.TabIndex = 28
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "电感标定后位移/mm"
        '
        'BL2
        '
        Me.BL2.AutoSize = True
        Me.BL2.Font = New System.Drawing.Font("Impact", 11.25!)
        Me.BL2.ForeColor = System.Drawing.Color.Blue
        Me.BL2.Location = New System.Drawing.Point(26, 76)
        Me.BL2.Name = "BL2"
        Me.BL2.Size = New System.Drawing.Size(50, 19)
        Me.BL2.TabIndex = 33
        Me.BL2.Text = "123.45"
        Me.BL2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'BL1
        '
        Me.BL1.AutoSize = True
        Me.BL1.Font = New System.Drawing.Font("Impact", 11.25!)
        Me.BL1.ForeColor = System.Drawing.Color.Blue
        Me.BL1.Location = New System.Drawing.Point(26, 35)
        Me.BL1.Name = "BL1"
        Me.BL1.Size = New System.Drawing.Size(50, 19)
        Me.BL1.TabIndex = 33
        Me.BL1.Text = "123.45"
        Me.BL1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'BCB4
        '
        Me.BCB4.AutoSize = True
        Me.BCB4.Location = New System.Drawing.Point(21, 199)
        Me.BCB4.Name = "BCB4"
        Me.BCB4.Size = New System.Drawing.Size(60, 16)
        Me.BCB4.TabIndex = 19
        Me.BCB4.Text = "通道四"
        Me.BCB4.UseVisualStyleBackColor = True
        '
        'BL4
        '
        Me.BL4.AutoSize = True
        Me.BL4.Font = New System.Drawing.Font("Impact", 11.25!)
        Me.BL4.ForeColor = System.Drawing.Color.Blue
        Me.BL4.Location = New System.Drawing.Point(26, 176)
        Me.BL4.Name = "BL4"
        Me.BL4.Size = New System.Drawing.Size(50, 19)
        Me.BL4.TabIndex = 33
        Me.BL4.Text = "123.45"
        Me.BL4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'BCB3
        '
        Me.BCB3.AutoSize = True
        Me.BCB3.Location = New System.Drawing.Point(21, 151)
        Me.BCB3.Name = "BCB3"
        Me.BCB3.Size = New System.Drawing.Size(60, 16)
        Me.BCB3.TabIndex = 18
        Me.BCB3.Text = "通道三"
        Me.BCB3.UseVisualStyleBackColor = True
        '
        'BL3
        '
        Me.BL3.AutoSize = True
        Me.BL3.Font = New System.Drawing.Font("Impact", 11.25!)
        Me.BL3.ForeColor = System.Drawing.Color.Blue
        Me.BL3.Location = New System.Drawing.Point(26, 129)
        Me.BL3.Name = "BL3"
        Me.BL3.Size = New System.Drawing.Size(50, 19)
        Me.BL3.TabIndex = 33
        Me.BL3.Text = "123.45"
        Me.BL3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'BCB2
        '
        Me.BCB2.AutoSize = True
        Me.BCB2.Location = New System.Drawing.Point(21, 98)
        Me.BCB2.Name = "BCB2"
        Me.BCB2.Size = New System.Drawing.Size(60, 16)
        Me.BCB2.TabIndex = 17
        Me.BCB2.Text = "通道二"
        Me.BCB2.UseVisualStyleBackColor = True
        '
        'BCB1
        '
        Me.BCB1.AutoSize = True
        Me.BCB1.Location = New System.Drawing.Point(21, 57)
        Me.BCB1.Name = "BCB1"
        Me.BCB1.Size = New System.Drawing.Size(60, 16)
        Me.BCB1.TabIndex = 16
        Me.BCB1.Text = "通道一"
        Me.BCB1.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.GroupBox5)
        Me.GroupBox3.Controls.Add(Me.Label11)
        Me.GroupBox3.Controls.Add(Me.PL1)
        Me.GroupBox3.Location = New System.Drawing.Point(260, 435)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(270, 261)
        Me.GroupBox3.TabIndex = 29
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "压力/Kg"
        '
        'GroupBox5
        '
        Me.GroupBox5.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox5.Controls.Add(Me.Label10)
        Me.GroupBox5.Controls.Add(Me.WeightCalib1)
        Me.GroupBox5.Controls.Add(Me.Label9)
        Me.GroupBox5.Controls.Add(Me.PT1)
        Me.GroupBox5.Controls.Add(Me.WeightCalib2)
        Me.GroupBox5.Controls.Add(Me.Label5)
        Me.GroupBox5.Location = New System.Drawing.Point(116, 29)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(148, 212)
        Me.GroupBox5.TabIndex = 36
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "标定"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(35, 131)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(65, 12)
        Me.Label10.TabIndex = 35
        Me.Label10.Text = "载重实际值"
        '
        'WeightCalib1
        '
        Me.WeightCalib1.Location = New System.Drawing.Point(28, 49)
        Me.WeightCalib1.Name = "WeightCalib1"
        Me.WeightCalib1.Size = New System.Drawing.Size(97, 22)
        Me.WeightCalib1.TabIndex = 34
        Me.WeightCalib1.Text = "零点标定"
        Me.WeightCalib1.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(24, 107)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(101, 12)
        Me.Label9.TabIndex = 35
        Me.Label9.Text = "载重为已知重量时"
        '
        'PT1
        '
        Me.PT1.Location = New System.Drawing.Point(42, 151)
        Me.PT1.Name = "PT1"
        Me.PT1.Size = New System.Drawing.Size(58, 21)
        Me.PT1.TabIndex = 31
        Me.PT1.Text = "10000"
        Me.PT1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'WeightCalib2
        '
        Me.WeightCalib2.Location = New System.Drawing.Point(26, 180)
        Me.WeightCalib2.Name = "WeightCalib2"
        Me.WeightCalib2.Size = New System.Drawing.Size(97, 22)
        Me.WeightCalib2.TabIndex = 34
        Me.WeightCalib2.Text = "载重标定"
        Me.WeightCalib2.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(41, 29)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 12)
        Me.Label5.TabIndex = 35
        Me.Label5.Text = "载重为0时"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(126, 35)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(0, 12)
        Me.Label11.TabIndex = 35
        '
        'PL1
        '
        Me.PL1.AutoSize = True
        Me.PL1.Font = New System.Drawing.Font("Impact", 11.25!)
        Me.PL1.ForeColor = System.Drawing.Color.Blue
        Me.PL1.Location = New System.Drawing.Point(27, 90)
        Me.PL1.Name = "PL1"
        Me.PL1.Size = New System.Drawing.Size(44, 19)
        Me.PL1.TabIndex = 33
        Me.PL1.Text = "30.43"
        Me.PL1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBox1
        '
        Me.ComboBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold)
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(947, 486)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(78, 26)
        Me.ComboBox1.TabIndex = 150
        '
        'speed_set
        '
        Me.speed_set.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold)
        Me.speed_set.Location = New System.Drawing.Point(11, 22)
        Me.speed_set.Name = "speed_set"
        Me.speed_set.Size = New System.Drawing.Size(52, 24)
        Me.speed_set.TabIndex = 31
        Me.speed_set.Text = "0.01"
        Me.speed_set.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(859, 493)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 12)
        Me.Label1.TabIndex = 32
        Me.Label1.Text = "通信端口："
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.AC1)
        Me.GroupBox1.Controls.Add(Me.AL2)
        Me.GroupBox1.Controls.Add(Me.AL1)
        Me.GroupBox1.Controls.Add(Me.AL4)
        Me.GroupBox1.Controls.Add(Me.AL3)
        Me.GroupBox1.Controls.Add(Me.ACB4)
        Me.GroupBox1.Controls.Add(Me.ACB3)
        Me.GroupBox1.Controls.Add(Me.ACB2)
        Me.GroupBox1.Controls.Add(Me.ACB1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 435)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(114, 261)
        Me.GroupBox1.TabIndex = 28
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "电感电压原始值 /mV"
        '
        'AC1
        '
        Me.AC1.Location = New System.Drawing.Point(17, 229)
        Me.AC1.Name = "AC1"
        Me.AC1.Size = New System.Drawing.Size(82, 26)
        Me.AC1.TabIndex = 34
        Me.AC1.Text = "标定"
        Me.AC1.UseVisualStyleBackColor = True
        '
        'AL2
        '
        Me.AL2.AutoSize = True
        Me.AL2.Font = New System.Drawing.Font("Impact", 11.25!)
        Me.AL2.ForeColor = System.Drawing.Color.Blue
        Me.AL2.Location = New System.Drawing.Point(33, 80)
        Me.AL2.Name = "AL2"
        Me.AL2.Size = New System.Drawing.Size(50, 19)
        Me.AL2.TabIndex = 33
        Me.AL2.Text = "123.45"
        Me.AL2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'AL1
        '
        Me.AL1.AutoSize = True
        Me.AL1.Font = New System.Drawing.Font("Impact", 11.25!)
        Me.AL1.ForeColor = System.Drawing.Color.Blue
        Me.AL1.Location = New System.Drawing.Point(33, 35)
        Me.AL1.Name = "AL1"
        Me.AL1.Size = New System.Drawing.Size(50, 19)
        Me.AL1.TabIndex = 33
        Me.AL1.Text = "123.45"
        Me.AL1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'AL4
        '
        Me.AL4.AutoSize = True
        Me.AL4.Font = New System.Drawing.Font("Impact", 11.25!)
        Me.AL4.ForeColor = System.Drawing.Color.Blue
        Me.AL4.Location = New System.Drawing.Point(33, 170)
        Me.AL4.Name = "AL4"
        Me.AL4.Size = New System.Drawing.Size(50, 19)
        Me.AL4.TabIndex = 33
        Me.AL4.Text = "123.45"
        Me.AL4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'AL3
        '
        Me.AL3.AutoSize = True
        Me.AL3.Font = New System.Drawing.Font("Impact", 11.25!)
        Me.AL3.ForeColor = System.Drawing.Color.Blue
        Me.AL3.Location = New System.Drawing.Point(33, 125)
        Me.AL3.Name = "AL3"
        Me.AL3.Size = New System.Drawing.Size(50, 19)
        Me.AL3.TabIndex = 33
        Me.AL3.Text = "123.45"
        Me.AL3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ACB4
        '
        Me.ACB4.AutoSize = True
        Me.ACB4.Location = New System.Drawing.Point(28, 197)
        Me.ACB4.Name = "ACB4"
        Me.ACB4.Size = New System.Drawing.Size(60, 16)
        Me.ACB4.TabIndex = 19
        Me.ACB4.Text = "通道四"
        Me.ACB4.UseVisualStyleBackColor = True
        '
        'ACB3
        '
        Me.ACB3.AutoSize = True
        Me.ACB3.Location = New System.Drawing.Point(28, 149)
        Me.ACB3.Name = "ACB3"
        Me.ACB3.Size = New System.Drawing.Size(60, 16)
        Me.ACB3.TabIndex = 18
        Me.ACB3.Text = "通道三"
        Me.ACB3.UseVisualStyleBackColor = True
        '
        'ACB2
        '
        Me.ACB2.AutoSize = True
        Me.ACB2.Location = New System.Drawing.Point(28, 104)
        Me.ACB2.Name = "ACB2"
        Me.ACB2.Size = New System.Drawing.Size(60, 16)
        Me.ACB2.TabIndex = 17
        Me.ACB2.Text = "通道二"
        Me.ACB2.UseVisualStyleBackColor = True
        '
        'ACB1
        '
        Me.ACB1.AutoSize = True
        Me.ACB1.Location = New System.Drawing.Point(28, 59)
        Me.ACB1.Name = "ACB1"
        Me.ACB1.Size = New System.Drawing.Size(60, 16)
        Me.ACB1.TabIndex = 16
        Me.ACB1.Text = "通道一"
        Me.ACB1.UseVisualStyleBackColor = True
        '
        'Timer2
        '
        Me.Timer2.Enabled = True
        Me.Timer2.Interval = 200
        '
        'LSPEED
        '
        Me.LSPEED.AutoSize = True
        Me.LSPEED.Font = New System.Drawing.Font("微软雅黑", 12.0!, System.Drawing.FontStyle.Bold)
        Me.LSPEED.Location = New System.Drawing.Point(755, 634)
        Me.LSPEED.Name = "LSPEED"
        Me.LSPEED.Size = New System.Drawing.Size(76, 22)
        Me.LSPEED.TabIndex = 33
        Me.LSPEED.Text = "0.01min"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("微软雅黑", 12.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.Location = New System.Drawing.Point(74, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(41, 22)
        Me.Label3.TabIndex = 33
        Me.Label3.Text = "min"
        '
        'B_SAVE
        '
        Me.B_SAVE.Location = New System.Drawing.Point(970, 547)
        Me.B_SAVE.Name = "B_SAVE"
        Me.B_SAVE.Size = New System.Drawing.Size(83, 38)
        Me.B_SAVE.TabIndex = 35
        Me.B_SAVE.Text = "导出数据"
        Me.B_SAVE.UseVisualStyleBackColor = True
        Me.B_SAVE.Visible = False
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label3)
        Me.GroupBox4.Controls.Add(Me.speed_set)
        Me.GroupBox4.Controls.Add(Me.speed_ok)
        Me.GroupBox4.Location = New System.Drawing.Point(861, 611)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(192, 60)
        Me.GroupBox4.TabIndex = 38
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "采样周期设置"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(671, 638)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 12)
        Me.Label2.TabIndex = 41
        Me.Label2.Text = "采样周期:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(687, 556)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(77, 12)
        Me.Label7.TabIndex = 32
        Me.Label7.Text = "总采集点数："
        '
        'LSampCnt
        '
        Me.LSampCnt.AutoSize = True
        Me.LSampCnt.BackColor = System.Drawing.Color.PaleGreen
        Me.LSampCnt.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LSampCnt.Location = New System.Drawing.Point(772, 554)
        Me.LSampCnt.Name = "LSampCnt"
        Me.LSampCnt.Size = New System.Drawing.Size(15, 16)
        Me.LSampCnt.TabIndex = 32
        Me.LSampCnt.Text = "0"
        Me.LSampCnt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(687, 586)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(65, 12)
        Me.Label8.TabIndex = 32
        Me.Label8.Text = "总采时间："
        '
        'LSumSamp
        '
        Me.LSumSamp.AutoSize = True
        Me.LSumSamp.BackColor = System.Drawing.Color.PaleGreen
        Me.LSumSamp.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LSumSamp.Location = New System.Drawing.Point(772, 584)
        Me.LSumSamp.Name = "LSumSamp"
        Me.LSumSamp.Size = New System.Drawing.Size(15, 16)
        Me.LSumSamp.TabIndex = 32
        Me.LSumSamp.Text = "0"
        Me.LSumSamp.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Timer4
        '
        Me.Timer4.Enabled = True
        Me.Timer4.Interval = 50
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(899, 425)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(126, 24)
        Me.Button3.TabIndex = 43
        Me.Button3.Text = "时间轴自动跟随"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Chart1
        '
        Me.Chart1.BackColor = System.Drawing.Color.Black
        Me.Chart1.BorderlineColor = System.Drawing.SystemColors.Control
        Me.Chart1.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
        Me.Chart1.BorderlineWidth = 2
        Me.Chart1.BorderSkin.BorderWidth = 5
        ChartArea1.Area3DStyle.LightStyle = System.Windows.Forms.DataVisualization.Charting.LightStyle.Realistic
        ChartArea1.AxisX.InterlacedColor = System.Drawing.Color.White
        ChartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds
        ChartArea1.AxisX.IsLabelAutoFit = False
        ChartArea1.AxisX.IsMarginVisible = False
        ChartArea1.AxisX.IsMarksNextToAxis = False
        ChartArea1.AxisX.LabelStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.Red
        ChartArea1.AxisX.LabelStyle.Format = "HH:mm:ss"
        ChartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds
        ChartArea1.AxisX.LabelStyle.IsEndLabelVisible = False
        ChartArea1.AxisX.LineColor = System.Drawing.Color.Blue
        ChartArea1.AxisX.LineWidth = 3
        ChartArea1.AxisX.MajorGrid.Interval = 0.0R
        ChartArea1.AxisX.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds
        ChartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray
        ChartArea1.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot
        ChartArea1.AxisX.MajorTickMark.Interval = 0.0R
        ChartArea1.AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds
        ChartArea1.AxisX.MajorTickMark.LineColor = System.Drawing.Color.Blue
        ChartArea1.AxisX.MajorTickMark.LineWidth = 2
        ChartArea1.AxisX.MajorTickMark.Size = 1.7!
        ChartArea1.AxisX.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea
        ChartArea1.AxisX.MinorGrid.Enabled = True
        ChartArea1.AxisX.MinorGrid.Interval = Double.NaN
        ChartArea1.AxisX.MinorGrid.IntervalOffset = Double.NaN
        ChartArea1.AxisX.MinorGrid.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.NotSet
        ChartArea1.AxisX.MinorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds
        ChartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.Gray
        ChartArea1.AxisX.MinorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot
        ChartArea1.AxisX.MinorTickMark.Enabled = True
        ChartArea1.AxisX.MinorTickMark.Interval = Double.NaN
        ChartArea1.AxisX.MinorTickMark.IntervalOffset = Double.NaN
        ChartArea1.AxisX.MinorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.NotSet
        ChartArea1.AxisX.MinorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds
        ChartArea1.AxisX.MinorTickMark.LineColor = System.Drawing.Color.Blue
        ChartArea1.AxisX.MinorTickMark.LineWidth = 2
        ChartArea1.AxisX.MinorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea
        ChartArea1.AxisX.ScaleBreakStyle.LineColor = System.Drawing.Color.White
        ChartArea1.AxisX.ScaleBreakStyle.LineWidth = 10
        ChartArea1.AxisX.ScaleBreakStyle.MaxNumberOfBreaks = 5
        ChartArea1.AxisX.ScaleBreakStyle.Spacing = 0.5R
        ChartArea1.AxisX.ScaleView.MinSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds
        ChartArea1.AxisX.ScaleView.SizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds
        ChartArea1.AxisX.ScaleView.SmallScrollMinSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds
        ChartArea1.AxisX.ScaleView.SmallScrollSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds
        ChartArea1.AxisX.ScrollBar.BackColor = System.Drawing.Color.Blue
        ChartArea1.AxisX.ScrollBar.ButtonColor = System.Drawing.Color.Red
        ChartArea1.AxisX.ScrollBar.LineColor = System.Drawing.SystemColors.Control
        ChartArea1.AxisX.ScrollBar.Size = 15.0R
        ChartArea1.AxisX.TitleFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea1.AxisX.TitleForeColor = System.Drawing.Color.Blue
        ChartArea1.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.[True]
        ChartArea1.AxisX2.LineColor = System.Drawing.Color.Blue
        ChartArea1.AxisX2.LineWidth = 3
        ChartArea1.AxisX2.MajorTickMark.LineColor = System.Drawing.Color.Empty
        ChartArea1.AxisX2.TitleFont = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea1.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.[True]
        ChartArea1.AxisY.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea1.AxisY.IsLabelAutoFit = False
        ChartArea1.AxisY.LabelStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.Red
        ChartArea1.AxisY.LabelStyle.Format = "0.000"
        ChartArea1.AxisY.LineColor = System.Drawing.Color.Blue
        ChartArea1.AxisY.LineWidth = 3
        ChartArea1.AxisY.MajorTickMark.Interval = 0.0R
        ChartArea1.AxisY.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.Blue
        ChartArea1.AxisY.MajorTickMark.Size = 0.7!
        ChartArea1.AxisY.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea
        ChartArea1.AxisY.MaximumAutoSize = 100.0!
        ChartArea1.AxisY.MinorTickMark.Enabled = True
        ChartArea1.AxisY.MinorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea1.AxisY.MinorTickMark.LineColor = System.Drawing.Color.Blue
        ChartArea1.AxisY.MinorTickMark.Size = 0.4!
        ChartArea1.AxisY.MinorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea
        ChartArea1.AxisY.ScrollBar.BackColor = System.Drawing.Color.Blue
        ChartArea1.AxisY.ScrollBar.ButtonColor = System.Drawing.Color.Yellow
        ChartArea1.AxisY.ScrollBar.LineColor = System.Drawing.SystemColors.Control
        ChartArea1.AxisY.ScrollBar.Size = 15.0R
        ChartArea1.AxisY.StripLines.Add(StripLine1)
        ChartArea1.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Rotated90
        ChartArea1.AxisY.Title = "L / mm"
        ChartArea1.AxisY.TitleFont = New System.Drawing.Font("微软雅黑", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea1.AxisY.TitleForeColor = System.Drawing.Color.Yellow
        ChartArea1.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.[True]
        ChartArea1.AxisY2.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea1.AxisY2.IsLabelAutoFit = False
        ChartArea1.AxisY2.IsMarginVisible = False
        ChartArea1.AxisY2.LabelStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea1.AxisY2.LabelStyle.ForeColor = System.Drawing.Color.Red
        ChartArea1.AxisY2.LabelStyle.Format = "0.0"
        ChartArea1.AxisY2.LineColor = System.Drawing.Color.Blue
        ChartArea1.AxisY2.LineWidth = 3
        ChartArea1.AxisY2.MajorTickMark.Interval = 0.0R
        ChartArea1.AxisY2.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea1.AxisY2.MajorTickMark.LineColor = System.Drawing.Color.Blue
        ChartArea1.AxisY2.MajorTickMark.Size = 0.7!
        ChartArea1.AxisY2.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea
        ChartArea1.AxisY2.MaximumAutoSize = 100.0!
        ChartArea1.AxisY2.MinorTickMark.Enabled = True
        ChartArea1.AxisY2.MinorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea1.AxisY2.MinorTickMark.LineColor = System.Drawing.Color.Blue
        ChartArea1.AxisY2.MinorTickMark.Size = 0.4!
        ChartArea1.AxisY2.MinorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea
        ChartArea1.AxisY2.ScaleView.MinSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea1.AxisY2.ScaleView.SmallScrollSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea1.AxisY2.ScrollBar.BackColor = System.Drawing.Color.Blue
        ChartArea1.AxisY2.ScrollBar.ButtonColor = System.Drawing.Color.Red
        ChartArea1.AxisY2.ScrollBar.LineColor = System.Drawing.SystemColors.Control
        ChartArea1.AxisY2.ScrollBar.Size = 15.0R
        ChartArea1.AxisY2.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Rotated90
        ChartArea1.AxisY2.Title = "Pa / MPa"
        ChartArea1.AxisY2.TitleFont = New System.Drawing.Font("微软雅黑", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea1.AxisY2.TitleForeColor = System.Drawing.Color.Yellow
        ChartArea1.BackColor = System.Drawing.Color.Black
        ChartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
        ChartArea1.CursorX.Interval = 50.0R
        ChartArea1.CursorX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Milliseconds
        ChartArea1.CursorX.IsUserEnabled = True
        ChartArea1.CursorX.LineColor = System.Drawing.Color.WhiteSmoke
        ChartArea1.CursorY.AxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary
        ChartArea1.CursorY.Interval = 0.001R
        ChartArea1.CursorY.IsUserEnabled = True
        ChartArea1.CursorY.LineColor = System.Drawing.Color.White
        ChartArea1.InnerPlotPosition.Auto = False
        ChartArea1.InnerPlotPosition.Height = 87.14864!
        ChartArea1.InnerPlotPosition.Width = 85.59033!
        ChartArea1.InnerPlotPosition.X = 6.65796!
        ChartArea1.InnerPlotPosition.Y = 6.95759!
        ChartArea1.Name = "ChartArea1"
        ChartArea1.Position.Auto = False
        ChartArea1.Position.Height = 94.0!
        ChartArea1.Position.Width = 96.0!
        ChartArea1.Position.X = 2.5!
        ChartArea1.Position.Y = 1.0!
        Me.Chart1.ChartAreas.Add(ChartArea1)
        Me.Chart1.Cursor = System.Windows.Forms.Cursors.Cross
        Me.Chart1.Location = New System.Drawing.Point(12, 11)
        Me.Chart1.Name = "Chart1"
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine
        Series1.LabelBackColor = System.Drawing.Color.Yellow
        Series1.MarkerBorderColor = System.Drawing.Color.Transparent
        Series1.MarkerBorderWidth = 15
        Series1.MarkerColor = System.Drawing.Color.Yellow
        Series1.MarkerSize = 20
        Series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Star4
        Series1.Name = "Series1"
        DataPoint1.LabelBackColor = System.Drawing.Color.Yellow
        DataPoint2.LabelBackColor = System.Drawing.Color.Yellow
        Series1.Points.Add(DataPoint1)
        Series1.Points.Add(DataPoint2)
        Series1.SmartLabelStyle.CalloutLineColor = System.Drawing.Color.White
        Series1.SmartLabelStyle.Enabled = False
        Series1.ToolTip = "我是X1"
        Series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime
        Series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Double]
        Series2.ChartArea = "ChartArea1"
        Series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine
        Series2.Name = "Series2"
        Series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime
        Series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Double]
        Series3.ChartArea = "ChartArea1"
        Series3.Name = "Series3"
        Series4.ChartArea = "ChartArea1"
        Series4.Name = "Series4"
        Series5.ChartArea = "ChartArea1"
        Series5.Name = "Series5"
        Series6.ChartArea = "ChartArea1"
        Series6.Name = "Series6"
        Series7.ChartArea = "ChartArea1"
        Series7.Name = "Series7"
        Series8.ChartArea = "ChartArea1"
        Series8.Name = "Series8"
        Series9.ChartArea = "ChartArea1"
        Series9.Name = "Series9"
        Series9.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary
        Series10.ChartArea = "ChartArea1"
        Series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline
        Series10.Name = "Series10"
        Series10.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary
        Me.Chart1.Series.Add(Series1)
        Me.Chart1.Series.Add(Series2)
        Me.Chart1.Series.Add(Series3)
        Me.Chart1.Series.Add(Series4)
        Me.Chart1.Series.Add(Series5)
        Me.Chart1.Series.Add(Series6)
        Me.Chart1.Series.Add(Series7)
        Me.Chart1.Series.Add(Series8)
        Me.Chart1.Series.Add(Series9)
        Me.Chart1.Series.Add(Series10)
        Me.Chart1.Size = New System.Drawing.Size(1060, 408)
        Me.Chart1.TabIndex = 44
        Me.Chart1.Text = "Chart1"
        '
        'L_CursorX
        '
        Me.L_CursorX.AutoSize = True
        Me.L_CursorX.BackColor = System.Drawing.Color.Black
        Me.L_CursorX.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L_CursorX.ForeColor = System.Drawing.Color.Yellow
        Me.L_CursorX.Location = New System.Drawing.Point(122, 22)
        Me.L_CursorX.Name = "L_CursorX"
        Me.L_CursorX.Size = New System.Drawing.Size(22, 18)
        Me.L_CursorX.TabIndex = 45
        Me.L_CursorX.Text = "X:"
        '
        'B_ClearAllGrat
        '
        Me.B_ClearAllGrat.BackColor = System.Drawing.SystemColors.Control
        Me.B_ClearAllGrat.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.B_ClearAllGrat.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.B_ClearAllGrat.Location = New System.Drawing.Point(862, 547)
        Me.B_ClearAllGrat.Name = "B_ClearAllGrat"
        Me.B_ClearAllGrat.Size = New System.Drawing.Size(86, 38)
        Me.B_ClearAllGrat.TabIndex = 34
        Me.B_ClearAllGrat.Text = "数据清零"
        Me.B_ClearAllGrat.UseVisualStyleBackColor = True
        '
        'L_CursorY
        '
        Me.L_CursorY.AutoSize = True
        Me.L_CursorY.BackColor = System.Drawing.Color.Black
        Me.L_CursorY.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L_CursorY.ForeColor = System.Drawing.Color.Yellow
        Me.L_CursorY.Location = New System.Drawing.Point(257, 22)
        Me.L_CursorY.Name = "L_CursorY"
        Me.L_CursorY.Size = New System.Drawing.Size(21, 18)
        Me.L_CursorY.TabIndex = 45
        Me.L_CursorY.Text = "Y:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Black
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Yellow
        Me.Label6.Location = New System.Drawing.Point(930, 391)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(53, 18)
        Me.Label6.TabIndex = 152
        Me.Label6.Text = "T / sec"
        '
        'L_Start
        '
        Me.L_Start.AutoSize = True
        Me.L_Start.BackColor = System.Drawing.Color.Black
        Me.L_Start.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L_Start.ForeColor = System.Drawing.Color.Yellow
        Me.L_Start.Location = New System.Drawing.Point(637, 22)
        Me.L_Start.Name = "L_Start"
        Me.L_Start.Size = New System.Drawing.Size(43, 18)
        Me.L_Start.TabIndex = 45
        Me.L_Start.Text = "Start:"
        '
        'L_End
        '
        Me.L_End.AutoSize = True
        Me.L_End.BackColor = System.Drawing.Color.Black
        Me.L_End.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L_End.ForeColor = System.Drawing.Color.Yellow
        Me.L_End.Location = New System.Drawing.Point(793, 22)
        Me.L_End.Name = "L_End"
        Me.L_End.Size = New System.Drawing.Size(38, 18)
        Me.L_End.TabIndex = 45
        Me.L_End.Text = "End:"
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox1.ErrorImage = Nothing
        Me.PictureBox1.Location = New System.Drawing.Point(595, 267)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(477, 154)
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(687, 496)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 12)
        Me.Label4.TabIndex = 32
        Me.Label4.Text = "采集器时间："
        '
        'LabelTime
        '
        Me.LabelTime.AutoSize = True
        Me.LabelTime.BackColor = System.Drawing.Color.PaleGreen
        Me.LabelTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTime.Location = New System.Drawing.Point(772, 494)
        Me.LabelTime.Name = "LabelTime"
        Me.LabelTime.Size = New System.Drawing.Size(15, 16)
        Me.LabelTime.TabIndex = 32
        Me.LabelTime.Text = "0"
        Me.LabelTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(687, 526)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(77, 12)
        Me.Label13.TabIndex = 32
        Me.Label13.Text = "采集器温度："
        '
        'LabelTemp
        '
        Me.LabelTemp.AutoSize = True
        Me.LabelTemp.BackColor = System.Drawing.Color.PaleGreen
        Me.LabelTemp.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTemp.Location = New System.Drawing.Point(772, 524)
        Me.LabelTemp.Name = "LabelTemp"
        Me.LabelTemp.Size = New System.Drawing.Size(15, 16)
        Me.LabelTemp.TabIndex = 32
        Me.LabelTemp.Text = "0"
        Me.LabelTemp.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(687, 466)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(77, 12)
        Me.Label12.TabIndex = 32
        Me.Label12.Text = "采集器时间："
        '
        'LabelDate
        '
        Me.LabelDate.AutoSize = True
        Me.LabelDate.BackColor = System.Drawing.Color.PaleGreen
        Me.LabelDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelDate.Location = New System.Drawing.Point(772, 464)
        Me.LabelDate.Name = "LabelDate"
        Me.LabelDate.Size = New System.Drawing.Size(15, 16)
        Me.LabelDate.TabIndex = 32
        Me.LabelDate.Text = "0"
        Me.LabelDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.Label17)
        Me.GroupBox6.Controls.Add(Me.Label16)
        Me.GroupBox6.Controls.Add(Me.LabelWDir)
        Me.GroupBox6.Controls.Add(Me.LabelWSpeed)
        Me.GroupBox6.Location = New System.Drawing.Point(547, 435)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(90, 261)
        Me.GroupBox6.TabIndex = 28
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "风速风向"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(28, 144)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(47, 12)
        Me.Label17.TabIndex = 34
        Me.Label17.Text = "风向 °"
        Me.Label17.Visible = False
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(19, 51)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(53, 12)
        Me.Label16.TabIndex = 34
        Me.Label16.Text = "风速 m/s"
        '
        'LabelWDir
        '
        Me.LabelWDir.AutoSize = True
        Me.LabelWDir.Font = New System.Drawing.Font("Impact", 11.25!)
        Me.LabelWDir.ForeColor = System.Drawing.Color.Blue
        Me.LabelWDir.Location = New System.Drawing.Point(17, 164)
        Me.LabelWDir.Name = "LabelWDir"
        Me.LabelWDir.Size = New System.Drawing.Size(50, 19)
        Me.LabelWDir.TabIndex = 33
        Me.LabelWDir.Text = "123.45"
        Me.LabelWDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.LabelWDir.Visible = False
        '
        'LabelWSpeed
        '
        Me.LabelWSpeed.AutoSize = True
        Me.LabelWSpeed.Font = New System.Drawing.Font("Impact", 11.25!)
        Me.LabelWSpeed.ForeColor = System.Drawing.Color.Blue
        Me.LabelWSpeed.Location = New System.Drawing.Point(17, 74)
        Me.LabelWSpeed.Name = "LabelWSpeed"
        Me.LabelWSpeed.Size = New System.Drawing.Size(50, 19)
        Me.LabelWSpeed.TabIndex = 33
        Me.LabelWSpeed.Text = "123.45"
        Me.LabelWSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1084, 708)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.L_End)
        Me.Controls.Add(Me.L_Start)
        Me.Controls.Add(Me.L_CursorY)
        Me.Controls.Add(Me.L_CursorX)
        Me.Controls.Add(Me.Chart1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.LSPEED)
        Me.Controls.Add(Me.B_SAVE)
        Me.Controls.Add(Me.B_ClearAllGrat)
        Me.Controls.Add(Me.LabelTemp)
        Me.Controls.Add(Me.LSumSamp)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.LabelDate)
        Me.Controls.Add(Me.LabelTime)
        Me.Controls.Add(Me.LSampCnt)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox6)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Form"
        Me.Text = "实时数据采集系统                                                                         " & _
            "                       成都科瑞测量仪器有限公司                                             " & _
            "                      技术咨询QQ：361230309"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SerialPort1 As System.IO.Ports.SerialPort
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents speed_ok As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents BCB4 As System.Windows.Forms.CheckBox
    Friend WithEvents BCB3 As System.Windows.Forms.CheckBox
    Friend WithEvents BCB2 As System.Windows.Forms.CheckBox
    Friend WithEvents BCB1 As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents speed_set As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BL2 As System.Windows.Forms.Label
    Friend WithEvents BL1 As System.Windows.Forms.Label
    Friend WithEvents BL4 As System.Windows.Forms.Label
    Friend WithEvents BL3 As System.Windows.Forms.Label
    Friend WithEvents PL1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents AL2 As System.Windows.Forms.Label
    Friend WithEvents AL1 As System.Windows.Forms.Label
    Friend WithEvents AL4 As System.Windows.Forms.Label
    Friend WithEvents AL3 As System.Windows.Forms.Label
    Friend WithEvents ACB4 As System.Windows.Forms.CheckBox
    Friend WithEvents ACB3 As System.Windows.Forms.CheckBox
    Friend WithEvents ACB2 As System.Windows.Forms.CheckBox
    Friend WithEvents ACB1 As System.Windows.Forms.CheckBox
    Friend WithEvents WeightCalib1 As System.Windows.Forms.Button
    Friend WithEvents AC1 As System.Windows.Forms.Button
    Friend WithEvents PT1 As System.Windows.Forms.TextBox
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents LSPEED As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents B_ClearAllGrat As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents B_SAVE As System.Windows.Forms.Button
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents LSampCnt As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents LSumSamp As System.Windows.Forms.Label
    Friend WithEvents Timer4 As System.Windows.Forms.Timer
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Chart1 As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents L_CursorX As System.Windows.Forms.Label
    Friend WithEvents L_CursorY As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents L_Start As System.Windows.Forms.Label
    Friend WithEvents L_End As System.Windows.Forms.Label
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents WeightCalib2 As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents LabelTime As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents LabelTemp As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents LabelDate As System.Windows.Forms.Label
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents LabelWDir As System.Windows.Forms.Label
    Friend WithEvents LabelWSpeed As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label

End Class
