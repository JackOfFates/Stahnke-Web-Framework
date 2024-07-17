Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports Microsoft.VisualBasic

Namespace Security

    Public Class Encryption
        Private Shared passPhrase As String = "FDh5A67FHe46DFhs2Fd5A46gf6Lu49iK"
        Private Shared hashAlgorithm As String = "SHA1"
        Private Shared passwordIterations As Integer = 4
        Private Shared initVector As String = "@1B2c3D4e5F6g7H8"
        Private Shared keySize As Integer = 256
        Public Shared Function Encrypt(ByVal Input As String, saltValue As String) As String
            Dim initVectorBytes As Byte() = Encoding.ASCII.GetBytes(initVector)
            Dim saltValueBytes As Byte() = Encoding.ASCII.GetBytes(saltValue)
            Dim plainTextBytes As Byte() = Encoding.UTF8.GetBytes(Input)
            Dim password As New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)
            Dim keyBytes As Byte() = (New Rfc2898DeriveBytes(passPhrase, saltValueBytes, passwordIterations)).GetBytes(keySize \ 8)
            Dim symmetricKey As New RijndaelManaged() With {.Mode = CipherMode.CBC}
            Dim encryptor As ICryptoTransform = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes)
            Dim memoryStream As New MemoryStream()
            Dim cryptoStream As New CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length)
            cryptoStream.FlushFinalBlock()
            Dim cipherTextBytes As Byte() = memoryStream.ToArray()
            memoryStream.Close()
            cryptoStream.Close()
            Dim cipherText As String = Convert.ToBase64String(cipherTextBytes)
            Return cipherText
        End Function
        Public Shared Function Decrypt(ByVal Input As String, saltValue As String) As String
            Dim initVectorBytes As Byte() = Encoding.ASCII.GetBytes(initVector)
            Dim saltValueBytes As Byte() = Encoding.ASCII.GetBytes(saltValue)
            Dim cipherTextBytes As Byte() = Convert.FromBase64String(Input)
            Dim password As New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)
            Dim keyBytes As Byte() = (New Rfc2898DeriveBytes(passPhrase, saltValueBytes, passwordIterations)).GetBytes(keySize \ 8)
            Dim symmetricKey As New RijndaelManaged() With {.Mode = CipherMode.CBC}
            Dim decryptor As ICryptoTransform = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes)
            Dim memoryStream As New MemoryStream(cipherTextBytes)
            Dim cryptoStream As New CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)
            Dim plainTextBytes As Byte() = New Byte(cipherTextBytes.Length - 1) {}
            Dim decryptedByteCount As Integer = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length)
            memoryStream.Close()
            cryptoStream.Close()
            Dim plainText As String = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount)
            Return plainText
        End Function

        Public Shared Function DestroyString(Input As String) As String
            Dim a As Integer = Math.Ceiling(Input.Length / 8)
            Dim c As String = Input.Substring(a, Input.Length - a)
            Dim cc As String = Input.Substring(0, a)
            Dim e As String = (c & cc)
            Dim f As Integer = e.Length - 1
            Dim ff As Integer = Math.Max(8, f)
            Dim max = Math.Log(ff)
            Dim h As String = ""
            For i As Integer = ff To 0 Step -1
                h = h & e.Chars(CInt(Interpolate(Log(i), 0, max, 0, f))).ToString
            Next
            Return h
        End Function

        Private Shared Function Log(d As Double) As Double
            If d = 0 Then
                Return 0
            Else
                Return Math.Log(d)
            End If
        End Function

        Private Shared Function Interpolate(value As Double, min As Double, max As Double, start As Double, [end] As Double) As Double
            Return start + ((([end] - start) / (max - min)) * (value - min))
        End Function

    End Class

End Namespace
