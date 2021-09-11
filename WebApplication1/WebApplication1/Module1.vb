Imports System.Data.SqlClient
Imports System.IO
Imports System.Net

Module Module1
    ' estas variables son estaticas y compartidas por todos los hilos(la misma pag abierta desde distintas pestañas, ventanas o pcs)
    ' -> usar vars de sesión
    Public ipServidor, nombreServidor, platafServer, loginTx, rutaSAT, loginRxSAT, passS, asesoriaPrecioBase, refBanco
    Private connectionString As String = ConfigurationManager.ConnectionStrings("ideConnectionString").ConnectionString

    Public Function redondea(ByVal num As Decimal) As Long
        If num - Math.Truncate(num) > 0.5 Then
            redondea = Math.Ceiling(num)
        Else
            redondea = Math.Floor(num)
        End If
    End Function

    Public Function ExecuteReaderFunction(myCommand As SqlCommand) As SqlDataReader
        Dim myConnection = New SqlConnection(connectionString)
        myCommand.Connection = myConnection
        myConnection.Open()
        Return myCommand.ExecuteReader(CommandBehavior.CloseConnection)
    End Function
    Public Function ExecuteScalarFunction(myCommand As SqlCommand)
        Using myConnection = New SqlConnection(connectionString)
            myCommand.Connection = myConnection
            myConnection.Open()
            Return myCommand.ExecuteScalar()
        End Using
    End Function
    Public Function ExecuteNonQueryFunction(myCommand As SqlCommand)
        Using myConnection = New SqlConnection(connectionString)
            myCommand.Connection = myConnection
            myConnection.Open()
            Return myCommand.ExecuteNonQuery()
        End Using
    End Function

End Module
