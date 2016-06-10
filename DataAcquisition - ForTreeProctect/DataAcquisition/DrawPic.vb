Public Class DrawPic

    Dim XOffsetSet As Integer = 30                                  'x�����Ե
    Dim YOffsetSet As Integer = 20                                  'y���±�Ե
    Dim XUintStr As String = "U/V"                                  'X���ͷλ���ַ���
    Dim YUintStr As String = "T/0.1ms"                              'X���ͷλ���ַ���
    Dim UintStrBrush As New SolidBrush(Color.Black)                 '��λ�ַ�������ɫ
    Dim UintStrFont As New Font("����", 11)                         '��λ�ַ���������
    Dim UintStrPen As Pen = Pens.Black                              '����ϵ����ɫ��ֱ�ߣ�
    Dim XStart As Integer = 0                                       '����ϵ��PictureBox����ʾ��ʼX��λ��
    Dim YStart As Integer = 0                                       '����ϵ��PictureBox����ʾ��ʼY��λ��


    Dim PicBackCol As Color = Color.White                           'PictureBox�ı�����ɫ
    Public DispScale As Decimal = 1                                    'X��Ŵ���С����
    Dim LinePen As Pen = Pens.Red                                   '���ߵ���ɫ
    Dim SpaceWidth As Integer = 100                                 '���������ּ��
    Dim FPen As Pen = Pens.Blue                                     '�����־����ɫ


    Dim sFont As New Font("����", 11)                               '�����ֵ�����


    Dim rBrush As New SolidBrush(Color.Black) '�����ɫ
    Dim bbrush As New SolidBrush(Color.Blue) 'cnfsc��ɫ 


    '*******************************************************************************************************'

    '*******************************************************************************************************'
    Public Sub DrawPicInit(ByVal X As Integer, ByVal Y As Integer, ByVal xStr As String, ByVal yStr As String, ByVal Scale As Decimal, ByVal Space As Integer)
        XUintStr = xStr
        YUintStr = yStr
        XStart = X
        YStart = Y
        DispScale = Scale
        SpaceWidth = Space
    End Sub

    Public Sub DrawCoordinatePic(ByVal pic As PictureBox, ByVal Xwidth As Integer, ByVal Ywidth As Integer, ByVal Data() As Decimal)
        Dim width As Integer = Xwidth 'PictureBox���
        Dim height As Integer = Ywidth 'PictureBox�߶�
        Dim XOffset As Integer = XOffsetSet
        Dim YOffset As Integer = YOffsetSet

        '˫����
        Dim bmp As New Bitmap(width, height)
        Dim g As Graphics = Graphics.FromImage(bmp)
        '���ԭ����ɫ
        g.Clear(PicBackCol) '������ɫ
        '����������
        Dim DataLen As Integer
        Dim yMax As Decimal
        Dim yMin As Decimal
        Dim yPre, xPre As Integer
        Dim yNow, xNow As Integer
        Dim X0 As Integer
        Dim yMaxHalfStr As String
        '���������������ֵ��Сֵ
        DataLen = Data.Length
        yMax = Data(0)
        yMin = Data(0)
        For i As Integer = 0 To DataLen - 1
            If yMax < Data(i) Then
                yMax = Data(i)
            End If
            If yMin > Data(i) Then
                yMin = Data(i)
            End If
        Next

        xPre = Int(0)
        Try
            yPre = Int((Data(0) - yMin) * (height - 2 * YOffset) / (yMax - yMin))
        Catch ex As Exception
            yPre = 0
        End Try
        X0 = Int((0 - yMin) * (height - 2 * YOffset) / (yMax - yMin))
        For i As Integer = 1 To DataLen - 1
            xNow = Int(i * DispScale)
            Try
                yNow = Int((Data(i) - yMin) * (height - 2 * YOffset) / (yMax - yMin))
            Catch ex As Exception
                yNow = 0
            End Try
            g.DrawLine(LinePen, XOffset + xPre, height - YOffset - yPre, XOffset + xNow, height - YOffset - yNow)
            xPre = xNow
            yPre = yNow
            If i Mod SpaceWidth = SpaceWidth - 1 Then
                g.DrawLine(FPen, XOffset + xNow, height - YOffset - X0, XOffset + xNow, height - YOffset - 5 - X0)
                g.DrawString(i + 1, sFont, bbrush, XOffset + xNow - 10, height - YOffset - X0)
            End If
        Next

        'g.DrawLine(Pens.Blue, XOffset, height - YOffset - yMax, width - XOffset, height - YOffset - yMax)
        'g.DrawString(yMaxStr, sFont, bbrush, 0, height - YOffset - yMax - 8)
        'yMaxHalf = yMax / 2
        'yMaxHalfStr = Math.Round(yMaxStr / 2, 2)
        'g.DrawLine(Pens.Blue, XOffset, height - YOffset - yMaxHalf, width - XOffset, height - YOffset - yMaxHalf)
        'g.DrawString(yMaxHalfStr, sFont, bbrush, 0, height - YOffset - yMaxHalf - 8)

        '*******************************************************************************************************'
        '����ϵ��ԭ��
        'X
        g.DrawLine(UintStrPen, XOffset, YOffset, XOffset, height - YOffset) 'X��
        g.DrawString(XUintStr, UintStrFont, UintStrBrush, 0, YOffset - 10)
        'Y
        'g.DrawLine(UintStrPen, XOffset, height - YOffset, width - XOffset, height - YOffset) 'Y��
        g.DrawLine(UintStrPen, XOffset, height - YOffset - X0, width - XOffset, height - YOffset - X0) 'Y��
        g.DrawString(YUintStr, UintStrFont, UintStrBrush, width - XOffset - 30, height - YOffset - X0)
        'ԭ���ַ�����0��
        g.DrawString("0", UintStrFont, UintStrBrush, XOffset - 10, height - YOffset)

        '���ϼ�ͷ
        Dim point1 As New PointF(XOffset - 3, YOffset)
        Dim point2 As New PointF(XOffset, YOffset - 10)
        Dim point3 As New PointF(XOffset + 3, YOffset)
        Dim curvePoints1 As PointF() = {point1, point2, point3}
        g.DrawPolygon(UintStrPen, curvePoints1)

        '���Ҽ�ͷ
        Dim point4 As New PointF(width - XOffset, height - YOffset - 3 - X0)
        Dim point5 As New PointF(width - XOffset + 10, height - YOffset - X0)
        Dim point6 As New PointF(width - XOffset, height - YOffset + 3 - X0)
        Dim curvePoints2 As PointF() = {point4, point5, point6}
        g.DrawPolygon(UintStrPen, curvePoints2)
        '*******************************************************************************************************'
        pic.CreateGraphics.DrawImage(bmp, XStart, YStart)
    End Sub
End Class
