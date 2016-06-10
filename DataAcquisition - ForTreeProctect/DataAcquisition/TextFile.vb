'文本操作函数
'** 作　者: 李杰
'** 日　期: 2013年7月4日
'**-------------------------------------------------------------------------------------------------------
'** 修改人： 
'** 日　期: 
'** 说  明：
'**-------------------------------------------------------------------------------------------------------
Imports System
Imports System.Text
Imports System.IO
Module TextFile
    '读取文本文件中的某一行
    Public Function ReaddTextLine(ByVal FilePath As String, ByVal lineID As Integer)
        Dim CurLineStr As String = ""
        Dim swObject As StreamReader
        swObject = New StreamReader(FilePath)
        For i As Integer = 1 To lineID
            CurLineStr = swObject.ReadLine()
        Next
        swObject.Close()
        Return CurLineStr
    End Function
    '读取文本文件的所有内容（返回一个数组）
    Public Function ReadTextAllLine(ByVal FilePath As String)
        Dim LineCount As Integer
        LineCount = TextLineCount(FilePath)

        Dim readtext(LineCount - 1) As String

        Dim swObject As StreamReader
        swObject = New StreamReader(FilePath)

        For i As Integer = 0 To LineCount - 1
            readtext(i) = swObject.ReadLine
        Next
        'For i As Integer = 0 To LineCount - 6
        '    readtext(i) = readtext(i + 5)
        'Next

        swObject.Close()
        Return readtext
    End Function
    '加载文本文件
    Public Function LoadText(ByVal FilePath As String)
        '加载当前已注册用户到combox
        Dim swObject As StreamReader '构造函数
        Try
            swObject = New StreamReader(FilePath)
        Catch ex As Exception '打不开文件先创建再打开
            Dim swObjectW As StreamWriter
            swObjectW = New StreamWriter(FilePath)
            swObjectW.Close()
            swObjectW = Nothing
            swObject = New StreamReader(FilePath)
        End Try
        swObject.Close()
        swObject = Nothing
        Return True
    End Function
    '加载文本文件（生成文本文件时写入第一行数据）
    Public Function LoadText(ByVal FilePath As String, ByVal FirstLineStr As String)
        '加载当前已注册用户到combox
        Dim swObject As StreamReader '构造函数
        Try
            swObject = New StreamReader(FilePath)
        Catch ex As Exception '打不开文件先创建再打开
            Dim swObjectW As StreamWriter
            swObjectW = New StreamWriter(FilePath)
            swObjectW.WriteLine(FirstLineStr)
            swObjectW.Close()
            swObjectW = Nothing
            swObject = New StreamReader(FilePath)
        End Try
        swObject.Close()
        swObject = Nothing
        Return True
    End Function
    '加载文本文件（生成文本文件时写入一组数据）
    Public Function LoadText(ByVal FilePath As String, ByVal LineStr() As String)
        '加载当前已注册用户到combox
        Dim swObject As StreamReader '构造函数
        Try
            swObject = New StreamReader(FilePath)
        Catch ex As Exception '打不开文件先创建再打开
            Dim swObjectW As StreamWriter
            swObjectW = New StreamWriter(FilePath)
            For i As Integer = 0 To LineStr.Length - 1
                swObjectW.WriteLine(LineStr(i))
            Next
            swObjectW.Close()
            swObjectW = Nothing
            swObject = New StreamReader(FilePath)
        End Try
        swObject.Close()
        swObject = Nothing
        Return True
    End Function
    '更改文本文件中的某一行
    Public Sub ChangeTxtLine(ByVal FilePath As String, ByVal lineID As Integer, ByVal changeStr As String)
        Dim readtxt As StreamReader
        readtxt = New StreamReader(FilePath)
        Dim awp(100) As String
        Dim i, n As Integer
        n = 0
        For i = 1 To 99
            awp(i) = readtxt.ReadLine
            If Not IsNothing(awp(i)) Then

                If lineID = i Then
                    awp(i) = changeStr
                End If
            Else
                readtxt.Close()
                Exit For
            End If
        Next
        Dim sw As StreamWriter '= IO.File.CreateText(FilePath)
        sw = New StreamWriter(FilePath, False)
        For i = 1 To 99
            If Not awp(i) = "" And Not awp(i) = Nothing Then
                sw.WriteLine(awp(i))
                'Else
                '    Exit For
            End If
        Next
        sw.Close()
    End Sub
    '更改文本文件中的某一行
    'Public Sub ChangeTxtLine(ByVal FilePath As String, ByVal lineID As long, ByVal changeStr As String)
    '    Dim readtxt As System.IO.StreamReader
    '    readtxt = New System.IO.StreamReader(FilePath)
    '    Dim TextLine(1000) As String
    '    Dim i As Integer = 0
    '    Do
    '        TextLine(i) = readtxt.ReadLine

    '        If lineID = i Then
    '            TextLine(i) = changeStr
    '        End If

    '        i = i + 1
    '    Loop Until IsNothing(TextLine(i - 1))
    '    readtxt.Close()

    '    Dim sw As IO.StreamWriter '= IO.File.CreateText(FilePath)
    '    sw = New StreamWriter(FilePath, False)
    '    For i = 0 To TextLine.LongLength - 1
    '        If Not TextLine(i) = Nothing Then
    '            sw.WriteLine(TextLine(i))
    '        End If
    '    Next
    '    sw.Close()
    'End Sub

    '写入文本文件的最后一行
    Public Function WriteText(ByVal FilePath As String, ByVal SavStr As String)
        Dim sw As StreamWriter
        '写入末尾
        sw = File.AppendText(FilePath) '不要多线程调用
        sw.Close()
        Return True
    End Function
    '写入文本文件中一个数组
    Public Function WriteText(ByVal FilePath As String, ByVal SavStr As Array)
        Dim sw As IO.StreamWriter
        sw = New StreamWriter(FilePath, False)
        For i As Integer = 0 To SavStr.LongLength - 1
            If SavStr(i) IsNot Nothing Then
                sw.WriteLine(SavStr(i))
            Else
                sw.WriteLine("")
            End If
        Next
        sw.Close()
        Return True
    End Function
    '删除文本文件
    Public Function DeleText(ByVal FilePath As String)
        Try
            File.Delete(FilePath)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
    '读取文件中的用户名和密码
    Public Function SplitUser(ByVal FilePath As String)

        Dim TextLineStr As String '一行的字符串
        Dim LineCount As Integer '行数

        LineCount = TextLineCount(FilePath)

        Dim UserAndPassword(1, LineCount - 1) As String
        Dim SplitStr As String = "="

        Dim swObject As StreamReader
        swObject = New StreamReader(FilePath)

        For i As Integer = 0 To LineCount - 1
            TextLineStr = swObject.ReadLine()
            UserAndPassword(0, i) = Left(TextLineStr, InStr(TextLineStr, SplitStr) - 1)
            UserAndPassword(1, i) = Right(TextLineStr, TextLineStr.Length - InStr(TextLineStr, SplitStr))
        Next

        swObject.Close()

        Return UserAndPassword

    End Function
    '计算文件的行数
    Public Function TextLineCount(ByVal FilePath As String)
        Dim TextLineStr As String '一行的字符串
        Dim LineCount As Integer '行数
        Dim swObject As StreamReader
        swObject = New StreamReader(FilePath)

        Do
            TextLineStr = swObject.ReadLine()
            LineCount = LineCount + 1
        Loop Until TextLineStr = Nothing Or TextLineStr = ""

        swObject.Close()

        Return LineCount - 1
    End Function
    '删除文本文件中的某一行
    Public Function DeleTextLine(ByVal FilePath As String, ByVal lineID As Integer)
        Dim LineCount As Integer '行数
        LineCount = TextLineCount(FilePath)

        Dim TextLineStr(LineCount) As String

        Dim readtxt As StreamReader = File.OpenText(FilePath)
        For i As Integer = 1 To LineCount
            TextLineStr(i) = readtxt.ReadLine
        Next
        readtxt.Close()

        Dim sw As IO.StreamWriter = IO.File.CreateText(FilePath)
        For i As Integer = 1 To LineCount
            If i <> lineID Then
                sw.WriteLine(TextLineStr(i))
            End If
        Next
        sw.Close()

        Return True
    End Function



    '写入文本文件中一个数组
    Public Function WriteText(ByVal FilePath As String, ByVal SavStr()() As Integer, ByVal YLen As Integer, ByVal XLen As Integer)
        Dim sw As IO.StreamWriter
        sw = New StreamWriter(FilePath, False)
        sw.WriteLine(YLen)
        sw.WriteLine(XLen)

        For i As Integer = 0 To YLen - 1
            For j As Integer = 0 To XLen - 1
                'If SavStr(i)(j) IsNot Nothing Then
                sw.WriteLine(SavStr(i)(j))
                'Else
                'sw.WriteLine("")
                'End If
            Next
        Next
        sw.Close()
        Return True
    End Function


    Public Function ReadText(ByVal FilePath As String, ByVal SavStr()() As String)
        Dim swObject As StreamReader
        swObject = New StreamReader(FilePath)
        Dim Ylen As Integer
        Dim Xlen As Integer

        Ylen = swObject.ReadLine()
        Xlen = swObject.ReadLine()
        Dim RdStr(Ylen - 1)() As Integer

        For i As Integer = 0 To Ylen - 1
            ReDim RdStr(i)(Xlen - 1)
        Next

        For i As Integer = 1 To Ylen - 1
            For j As Integer = 1 To Xlen - 1
                RdStr(i)(j) = swObject.ReadLine()
            Next
        Next
        swObject.Close()

        Return RdStr


    End Function

End Module