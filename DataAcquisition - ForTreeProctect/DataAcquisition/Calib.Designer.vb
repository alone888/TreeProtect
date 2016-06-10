<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Calib
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
        Me.rd0 = New System.Windows.Forms.Button()
        Me.wr0 = New System.Windows.Forms.Button()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ListView2 = New System.Windows.Forms.ListView()
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ListView3 = New System.Windows.Forms.ListView()
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ListView4 = New System.Windows.Forms.ListView()
        Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.rd1 = New System.Windows.Forms.Button()
        Me.wr1 = New System.Windows.Forms.Button()
        Me.rd2 = New System.Windows.Forms.Button()
        Me.wr2 = New System.Windows.Forms.Button()
        Me.rd3 = New System.Windows.Forms.Button()
        Me.wr3 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'rd0
        '
        Me.rd0.Location = New System.Drawing.Point(12, 545)
        Me.rd0.Name = "rd0"
        Me.rd0.Size = New System.Drawing.Size(78, 27)
        Me.rd0.TabIndex = 0
        Me.rd0.Text = "读取标定表"
        Me.rd0.UseVisualStyleBackColor = True
        '
        'wr0
        '
        Me.wr0.Location = New System.Drawing.Point(108, 545)
        Me.wr0.Name = "wr0"
        Me.wr0.Size = New System.Drawing.Size(87, 27)
        Me.wr0.TabIndex = 0
        Me.wr0.Text = "写入标定表"
        Me.wr0.UseVisualStyleBackColor = True
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.ListView1.Font = New System.Drawing.Font("宋体", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.ListView1.GridLines = True
        Me.ListView1.LabelEdit = True
        Me.ListView1.Location = New System.Drawing.Point(11, 18)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(184, 489)
        Me.ListView1.TabIndex = 1
        Me.ListView1.UseCompatibleStateImageBehavior = False
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "电压值"
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "实际位移"
        '
        'ListView2
        '
        Me.ListView2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader4})
        Me.ListView2.Font = New System.Drawing.Font("宋体", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.ListView2.GridLines = True
        Me.ListView2.LabelEdit = True
        Me.ListView2.Location = New System.Drawing.Point(235, 18)
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(184, 489)
        Me.ListView2.TabIndex = 1
        Me.ListView2.UseCompatibleStateImageBehavior = False
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "电压值"
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "实际位移"
        '
        'ListView3
        '
        Me.ListView3.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader5, Me.ColumnHeader6})
        Me.ListView3.Font = New System.Drawing.Font("宋体", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.ListView3.GridLines = True
        Me.ListView3.LabelEdit = True
        Me.ListView3.Location = New System.Drawing.Point(467, 18)
        Me.ListView3.Name = "ListView3"
        Me.ListView3.Scrollable = False
        Me.ListView3.Size = New System.Drawing.Size(184, 489)
        Me.ListView3.TabIndex = 1
        Me.ListView3.UseCompatibleStateImageBehavior = False
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "电压值"
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "实际位移"
        '
        'ListView4
        '
        Me.ListView4.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader7, Me.ColumnHeader8})
        Me.ListView4.Font = New System.Drawing.Font("宋体", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.ListView4.GridLines = True
        Me.ListView4.LabelEdit = True
        Me.ListView4.Location = New System.Drawing.Point(677, 18)
        Me.ListView4.Name = "ListView4"
        Me.ListView4.Size = New System.Drawing.Size(184, 489)
        Me.ListView4.TabIndex = 1
        Me.ListView4.UseCompatibleStateImageBehavior = False
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "电压值"
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "实际位移"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 500
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Impact", 11.25!)
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(240, 516)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 19)
        Me.Label2.TabIndex = 34
        Me.Label2.Text = "123.45"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Impact", 11.25!)
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(12, 514)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 19)
        Me.Label1.TabIndex = 34
        Me.Label1.Text = "123.45"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Impact", 11.25!)
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(463, 515)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(50, 19)
        Me.Label3.TabIndex = 34
        Me.Label3.Text = "123.45"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Impact", 11.25!)
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(673, 515)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(50, 19)
        Me.Label4.TabIndex = 34
        Me.Label4.Text = "123.45"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'rd1
        '
        Me.rd1.Location = New System.Drawing.Point(235, 545)
        Me.rd1.Name = "rd1"
        Me.rd1.Size = New System.Drawing.Size(78, 27)
        Me.rd1.TabIndex = 0
        Me.rd1.Text = "读取标定表"
        Me.rd1.UseVisualStyleBackColor = True
        '
        'wr1
        '
        Me.wr1.Location = New System.Drawing.Point(331, 545)
        Me.wr1.Name = "wr1"
        Me.wr1.Size = New System.Drawing.Size(87, 27)
        Me.wr1.TabIndex = 0
        Me.wr1.Text = "写入标定表"
        Me.wr1.UseVisualStyleBackColor = True
        '
        'rd2
        '
        Me.rd2.Location = New System.Drawing.Point(467, 545)
        Me.rd2.Name = "rd2"
        Me.rd2.Size = New System.Drawing.Size(78, 27)
        Me.rd2.TabIndex = 0
        Me.rd2.Text = "读取标定表"
        Me.rd2.UseVisualStyleBackColor = True
        '
        'wr2
        '
        Me.wr2.Location = New System.Drawing.Point(563, 545)
        Me.wr2.Name = "wr2"
        Me.wr2.Size = New System.Drawing.Size(87, 27)
        Me.wr2.TabIndex = 0
        Me.wr2.Text = "写入标定表"
        Me.wr2.UseVisualStyleBackColor = True
        '
        'rd3
        '
        Me.rd3.Location = New System.Drawing.Point(678, 545)
        Me.rd3.Name = "rd3"
        Me.rd3.Size = New System.Drawing.Size(78, 27)
        Me.rd3.TabIndex = 0
        Me.rd3.Text = "读取标定表"
        Me.rd3.UseVisualStyleBackColor = True
        '
        'wr3
        '
        Me.wr3.Location = New System.Drawing.Point(774, 545)
        Me.wr3.Name = "wr3"
        Me.wr3.Size = New System.Drawing.Size(87, 27)
        Me.wr3.TabIndex = 0
        Me.wr3.Text = "写入标定表"
        Me.wr3.UseVisualStyleBackColor = True
        '
        'Calib
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(873, 584)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ListView4)
        Me.Controls.Add(Me.ListView3)
        Me.Controls.Add(Me.ListView2)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.wr3)
        Me.Controls.Add(Me.wr2)
        Me.Controls.Add(Me.wr1)
        Me.Controls.Add(Me.rd3)
        Me.Controls.Add(Me.wr0)
        Me.Controls.Add(Me.rd2)
        Me.Controls.Add(Me.rd1)
        Me.Controls.Add(Me.rd0)
        Me.Name = "Calib"
        Me.Text = "Calib"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents rd0 As System.Windows.Forms.Button
    Friend WithEvents wr0 As System.Windows.Forms.Button
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ListView2 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ListView3 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ListView4 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents rd1 As System.Windows.Forms.Button
    Friend WithEvents wr1 As System.Windows.Forms.Button
    Friend WithEvents rd2 As System.Windows.Forms.Button
    Friend WithEvents wr2 As System.Windows.Forms.Button
    Friend WithEvents rd3 As System.Windows.Forms.Button
    Friend WithEvents wr3 As System.Windows.Forms.Button
End Class
