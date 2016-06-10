<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class P_D_Chart
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim ChartArea3 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim StripLine3 As System.Windows.Forms.DataVisualization.Charting.StripLine = New System.Windows.Forms.DataVisualization.Charting.StripLine()
        Dim Series3 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim DataPoint5 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0.0R, 0.0R)
        Dim DataPoint6 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(15.0R, 40.0R)
        Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.L_CursorY = New System.Windows.Forms.Label()
        Me.L_CursorX = New System.Windows.Forms.Label()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Chart1
        '
        Me.Chart1.BackColor = System.Drawing.Color.Black
        Me.Chart1.BorderlineColor = System.Drawing.SystemColors.Control
        Me.Chart1.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
        Me.Chart1.BorderlineWidth = 2
        Me.Chart1.BorderSkin.BorderWidth = 5
        ChartArea3.Area3DStyle.LightStyle = System.Windows.Forms.DataVisualization.Charting.LightStyle.Realistic
        ChartArea3.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea3.AxisX.IsLabelAutoFit = False
        ChartArea3.AxisX.LabelStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea3.AxisX.LabelStyle.ForeColor = System.Drawing.Color.Red
        ChartArea3.AxisX.LabelStyle.Format = "0.0"
        ChartArea3.AxisX.LabelStyle.Interval = 0.0R
        ChartArea3.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea3.AxisX.LineColor = System.Drawing.Color.Blue
        ChartArea3.AxisX.LineWidth = 3
        ChartArea3.AxisX.MajorGrid.Interval = 0.0R
        ChartArea3.AxisX.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea3.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray
        ChartArea3.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDot
        ChartArea3.AxisX.MajorTickMark.Interval = 0.0R
        ChartArea3.AxisX.MajorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea3.AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea3.AxisX.MajorTickMark.LineColor = System.Drawing.Color.Blue
        ChartArea3.AxisX.MajorTickMark.LineWidth = 2
        ChartArea3.AxisX.MajorTickMark.Size = 1.7!
        ChartArea3.AxisX.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea
        ChartArea3.AxisX.MinorTickMark.Enabled = True
        ChartArea3.AxisX.MinorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea3.AxisX.MinorTickMark.LineColor = System.Drawing.Color.Blue
        ChartArea3.AxisX.MinorTickMark.LineWidth = 2
        ChartArea3.AxisX.MinorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea
        ChartArea3.AxisX.ScaleBreakStyle.Enabled = True
        ChartArea3.AxisX.ScaleView.SizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea3.AxisX.ScrollBar.BackColor = System.Drawing.Color.Red
        ChartArea3.AxisX.ScrollBar.ButtonColor = System.Drawing.Color.Blue
        ChartArea3.AxisX.ScrollBar.LineColor = System.Drawing.SystemColors.Control
        ChartArea3.AxisX.ScrollBar.Size = 15.0R
        ChartArea3.AxisX.Title = "Pa /MPa"
        ChartArea3.AxisX.TitleFont = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea3.AxisX.TitleForeColor = System.Drawing.Color.Yellow
        ChartArea3.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.[True]
        ChartArea3.AxisX2.LineColor = System.Drawing.Color.Blue
        ChartArea3.AxisX2.LineWidth = 3
        ChartArea3.AxisX2.MajorTickMark.LineColor = System.Drawing.Color.Empty
        ChartArea3.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.[True]
        ChartArea3.AxisY.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea3.AxisY.IsLabelAutoFit = False
        ChartArea3.AxisY.LabelStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea3.AxisY.LabelStyle.ForeColor = System.Drawing.Color.Red
        ChartArea3.AxisY.LabelStyle.Format = "0.000"
        ChartArea3.AxisY.LabelStyle.Interval = 0.0R
        ChartArea3.AxisY.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea3.AxisY.LineColor = System.Drawing.Color.Blue
        ChartArea3.AxisY.LineWidth = 3
        ChartArea3.AxisY.MajorGrid.Interval = 0.0R
        ChartArea3.AxisY.MajorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea3.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray
        ChartArea3.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDot
        ChartArea3.AxisY.MajorTickMark.Interval = 0.0R
        ChartArea3.AxisY.MajorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea3.AxisY.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea3.AxisY.MajorTickMark.LineColor = System.Drawing.Color.Blue
        ChartArea3.AxisY.MajorTickMark.Size = 0.7!
        ChartArea3.AxisY.MajorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea
        ChartArea3.AxisY.MinorGrid.Enabled = True
        ChartArea3.AxisY.MinorGrid.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Milliseconds
        ChartArea3.AxisY.MinorTickMark.Enabled = True
        ChartArea3.AxisY.MinorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Milliseconds
        ChartArea3.AxisY.MinorTickMark.LineColor = System.Drawing.Color.Blue
        ChartArea3.AxisY.MinorTickMark.Size = 0.4!
        ChartArea3.AxisY.MinorTickMark.TickMarkStyle = System.Windows.Forms.DataVisualization.Charting.TickMarkStyle.InsideArea
        ChartArea3.AxisY.ScrollBar.BackColor = System.Drawing.Color.Red
        ChartArea3.AxisY.ScrollBar.ButtonColor = System.Drawing.Color.Blue
        ChartArea3.AxisY.ScrollBar.LineColor = System.Drawing.SystemColors.Control
        ChartArea3.AxisY.ScrollBar.Size = 15.0R
        ChartArea3.AxisY.StripLines.Add(StripLine3)
        ChartArea3.AxisY.Title = "L / mm"
        ChartArea3.AxisY.TitleFont = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea3.AxisY.TitleForeColor = System.Drawing.Color.Yellow
        ChartArea3.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.[True]
        ChartArea3.AxisY2.LineColor = System.Drawing.Color.Blue
        ChartArea3.AxisY2.LineWidth = 3
        ChartArea3.AxisY2.MajorTickMark.LineColor = System.Drawing.Color.Empty
        ChartArea3.AxisY2.MinorTickMark.LineColor = System.Drawing.Color.Empty
        ChartArea3.BackColor = System.Drawing.Color.Black
        ChartArea3.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
        ChartArea3.CursorX.Interval = 0.1R
        ChartArea3.CursorX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea3.CursorX.IsUserEnabled = True
        ChartArea3.CursorX.IsUserSelectionEnabled = True
        ChartArea3.CursorX.LineColor = System.Drawing.Color.White
        ChartArea3.CursorY.Interval = 0.001R
        ChartArea3.CursorY.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea3.CursorY.IsUserEnabled = True
        ChartArea3.CursorY.IsUserSelectionEnabled = True
        ChartArea3.CursorY.LineColor = System.Drawing.Color.White
        ChartArea3.InnerPlotPosition.Auto = False
        ChartArea3.InnerPlotPosition.Height = 86.3197!
        ChartArea3.InnerPlotPosition.Width = 93.9677!
        ChartArea3.InnerPlotPosition.X = 3.87971!
        ChartArea3.InnerPlotPosition.Y = 5.40381!
        ChartArea3.Name = "ChartArea1"
        ChartArea3.Position.Auto = False
        ChartArea3.Position.Height = 94.0!
        ChartArea3.Position.Width = 91.0!
        ChartArea3.Position.X = 6.0!
        ChartArea3.Position.Y = 3.0!
        Me.Chart1.ChartAreas.Add(ChartArea3)
        Me.Chart1.Cursor = System.Windows.Forms.Cursors.Cross
        Me.Chart1.Location = New System.Drawing.Point(12, 12)
        Me.Chart1.Name = "Chart1"
        Series3.ChartArea = "ChartArea1"
        Series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series3.Color = System.Drawing.Color.Yellow
        Series3.MarkerBorderColor = System.Drawing.Color.Yellow
        Series3.MarkerColor = System.Drawing.Color.Yellow
        Series3.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Diamond
        Series3.Name = "Series11"
        DataPoint5.MarkerSize = 10
        Series3.Points.Add(DataPoint5)
        Series3.Points.Add(DataPoint6)
        Series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Double]
        Series3.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Double]
        Me.Chart1.Series.Add(Series3)
        Me.Chart1.Size = New System.Drawing.Size(984, 592)
        Me.Chart1.TabIndex = 0
        Me.Chart1.Text = "Chart1"
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Text = "NotifyIcon1"
        Me.NotifyIcon1.Visible = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(516, 619)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(215, 34)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 50
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(114, 619)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(109, 28)
        Me.ComboBox1.TabIndex = 2
        '
        'ComboBox2
        '
        Me.ComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(319, 619)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(104, 28)
        Me.ComboBox2.TabIndex = 2
        '
        'L_CursorY
        '
        Me.L_CursorY.AutoSize = True
        Me.L_CursorY.BackColor = System.Drawing.Color.Black
        Me.L_CursorY.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L_CursorY.ForeColor = System.Drawing.Color.Yellow
        Me.L_CursorY.Location = New System.Drawing.Point(331, 34)
        Me.L_CursorY.Name = "L_CursorY"
        Me.L_CursorY.Size = New System.Drawing.Size(21, 18)
        Me.L_CursorY.TabIndex = 47
        Me.L_CursorY.Text = "Y:"
        '
        'L_CursorX
        '
        Me.L_CursorX.AutoSize = True
        Me.L_CursorX.BackColor = System.Drawing.Color.Black
        Me.L_CursorX.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.L_CursorX.ForeColor = System.Drawing.Color.Yellow
        Me.L_CursorX.Location = New System.Drawing.Point(196, 34)
        Me.L_CursorX.Name = "L_CursorX"
        Me.L_CursorX.Size = New System.Drawing.Size(22, 18)
        Me.L_CursorX.TabIndex = 46
        Me.L_CursorX.Text = "X:"
        '
        'P_D_Chart
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(1031, 703)
        Me.Controls.Add(Me.L_CursorY)
        Me.Controls.Add(Me.L_CursorX)
        Me.Controls.Add(Me.ComboBox2)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Chart1)
        Me.Name = "P_D_Chart"
        Me.Text = "L-Pa 图"
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Chart1 As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents L_CursorY As System.Windows.Forms.Label
    Friend WithEvents L_CursorX As System.Windows.Forms.Label
End Class
