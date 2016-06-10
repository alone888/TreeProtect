'CRC16计算函数，采样查表法

'*/********************************************************************************************************
'** 函数名称: Crc16Lookup()
'** 功能描述: CRC16校验，校验公式X16+X15+X2+1，查表方式
'** 输　入: 校验位数组
'** 输　出: CRC16校验码，低位在前
'** 全局变量: 无
'** 调用模块: GetCRCLo（），GetCRCHi（）
'**
'** 作　者: 李杰
'** 日　期: 2012年12月6日
'**-------------------------------------------------------------------------------------------------------
'** 修改人: 
'** 日　期: 
'**-------------------------------------------------------------------------------------------------------
'********************************************************************************************************/
Public Class CRC16
    Dim crc16str
    'crc16查表
    Public Function Crc16Lookup(ByVal data() As Byte)
        Dim CRC16Hi As Byte
        Dim CRC16Lo As Byte
        Dim ReturnData(1) As String
        CRC16Hi = &HFF
        CRC16Lo = &HFF
        Dim i As Integer
        Dim iIndex As Long
        For i = 0 To UBound(data)
            iIndex = CRC16Lo Xor data(i)
            CRC16Lo = CRC16Hi Xor GetCRCLo(iIndex)        '低位处理
            CRC16Hi = GetCRCHi(iIndex)                    '高位处理
        Next i
        ReturnData(0) = Hex(CRC16Hi)     'CRC高位
        ReturnData(1) = Hex(CRC16Lo)        'CRC低位
        If ReturnData(0).Length = 1 Then
            ReturnData(0) = "0" & ReturnData(0)
        End If
        If ReturnData(1).Length = 1 Then
            ReturnData(1) = "0" & ReturnData(1)
        End If
        Return ReturnData(1) & ReturnData(0)
    End Function

    'CRC低位字节值表
    Function GetCRCLo(ByVal Ind As Long) As Byte
        GetCRCLo = Choose(Ind + 1, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, _
    &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40, &H1, &HC0, &H80, &H41, &H1, &HC0, &H80, &H41, &H0, &HC1, &H81, &H40)
    End Function

    'CRC高位字节值表
    Function GetCRCHi(ByVal Ind As Long) As Byte
        GetCRCHi = Choose(Ind + 1, &H0, &HC0, &HC1, &H1, &HC3, &H3, &H2, &HC2, &HC6, &H6, &H7, &HC7, &H5, &HC5, &HC4, &H4, &HCC, &HC, &HD, &HCD, &HF, &HCF, &HCE, &HE, &HA, &HCA, &HCB, &HB, &HC9, &H9, &H8, &HC8, &HD8, &H18, &H19, &HD9, &H1B, &HDB, &HDA, &H1A, &H1E, &HDE, &HDF, &H1F, &HDD, &H1D, &H1C, &HDC, &H14, &HD4, &HD5, &H15, &HD7, &H17, &H16, &HD6, &HD2, &H12, &H13, &HD3, &H11, &HD1, &HD0, &H10, &HF0, &H30, &H31, &HF1, &H33, &HF3, &HF2, &H32, &H36, &HF6, &HF7, &H37, &HF5, &H35, &H34, &HF4, &H3C, &HFC, &HFD, &H3D, &HFF, &H3F, &H3E, &HFE, &HFA, &H3A, &H3B, &HFB, &H39, &HF9, &HF8, &H38, &H28, &HE8, &HE9, &H29, &HEB, &H2B, &H2A, &HEA, &HEE, &H2E, &H2F, &HEF, &H2D, &HED, &HEC, &H2C, &HE4, &H24, &H25, &HE5, &H27, &HE7, &HE6, &H26, &H22, &HE2, &HE3, &H23, &HE1, &H21, &H20, &HE0, &HA0, &H60, _
    &H61, &HA1, &H63, &HA3, &HA2, &H62, &H66, &HA6, &HA7, &H67, &HA5, &H65, &H64, &HA4, &H6C, &HAC, &HAD, &H6D, &HAF, &H6F, &H6E, &HAE, &HAA, &H6A, &H6B, &HAB, &H69, &HA9, &HA8, &H68, &H78, &HB8, &HB9, &H79, &HBB, &H7B, &H7A, &HBA, &HBE, &H7E, &H7F, &HBF, &H7D, &HBD, &HBC, &H7C, &HB4, &H74, &H75, &HB5, &H77, &HB7, &HB6, &H76, &H72, &HB2, &HB3, &H73, &HB1, &H71, &H70, &HB0, &H50, &H90, &H91, &H51, &H93, &H53, &H52, &H92, &H96, &H56, &H57, &H97, &H55, &H95, &H94, &H54, &H9C, &H5C, &H5D, &H9D, &H5F, &H9F, &H9E, &H5E, &H5A, &H9A, &H9B, &H5B, &H99, &H59, &H58, &H98, &H88, &H48, &H49, &H89, &H4B, &H8B, &H8A, &H4A, &H4E, &H8E, &H8F, &H4F, &H8D, &H4D, &H4C, &H8C, &H44, &H84, &H85, &H45, &H87, &H47, &H46, &H86, &H82, &H42, &H43, &H83, &H41, &H81, &H80, &H40)
    End Function

    '*/********************************************************************************************************
    '** 函数名称: Crc16Compu()
    '** 功能描述: CRC16校验，校验公式X16+X15+X2+1，循环方式
    '** 输　入: 校验位数组
    '** 输　出: CRC16校验码，低位在前
    '** 全局变量: 无
    '** 调用模块:无
    '**
    '** 作　者: 李杰
    '** 日　期: 2012年12月6日
    '**-------------------------------------------------------------------------------------------------------
    '** 修改人: 
    '** 日　期: 
    '**-------------------------------------------------------------------------------------------------------
    '********************************************************************************************************/

    'crc计算
    Public Function Crc16Compu(ByVal data() As Byte)
        Dim flag As Integer
        Dim Addressreg_crc = &HFFFF
        Dim ReturnData(1) As String
        For i As Integer = 0 To UBound(data)
            Addressreg_crc = Addressreg_crc Xor data(i)
            For j As Integer = 0 To 7
                flag = Addressreg_crc And &H1
                If flag Then
                    Addressreg_crc = Int(Addressreg_crc / 2)
                    Addressreg_crc = Addressreg_crc And &H7FFF
                    Addressreg_crc = Addressreg_crc Xor &HA001
                Else
                    Addressreg_crc = Addressreg_crc / 2
                    Addressreg_crc = Addressreg_crc And &H7FFF
                End If
            Next j
        Next i
        If Addressreg_crc < 0 Then
            Addressreg_crc = Addressreg_crc - &HFFFF0000
        End If
        ReturnData(0) = Hex((Addressreg_crc And &HFF00) / &H100)     'CRC高位
        ReturnData(1) = Hex(Addressreg_crc And &HFF)             'CRC低位
        If ReturnData(0).Length = 1 Then
            ReturnData(0) = "0" & ReturnData(0)
        End If
        If ReturnData(1).Length = 1 Then
            ReturnData(1) = "0" & ReturnData(1)
        End If
        Return ReturnData(1) & ReturnData(0)
    End Function
End Class