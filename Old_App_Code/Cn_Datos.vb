Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlDbType
Imports System.Collections

Public Class Cn_Datos

    Public Shared Function Crea_Conexion(ByVal cadena As String) As SqlConnection
        Dim Cone As SqlConnection = New SqlConnection(cadena)
        Return Cone
    End Function

    Public Shared Function Crea_ConexionSTD() As SqlConnection
        Dim Cone As SqlConnection = New SqlConnection(HttpContext.Current.Session("Sitio"))
        Return Cone
    End Function

    Public Shared Function crear_Trans(ByVal cn As SqlConnection) As SqlTransaction
        cn.Open()
        Dim tr As SqlTransaction = cn.BeginTransaction()
        Return tr
    End Function

    Public Shared Function Crea_Comando(ByVal Consulta As String, ByVal Tipo As CommandType, ByVal cone As SqlConnection) As SqlCommand
        Dim Com As SqlCommand = New SqlCommand(Consulta, cone)
        Com.CommandType = Tipo
        Return Com
    End Function

    Public Shared Function Crea_ComandoT(ByVal cn As SqlConnection, ByVal tr As SqlTransaction, ByVal Tipo As CommandType, ByVal Texto As String) As SqlCommand
        Dim cmd As String = Texto
        Dim comandoSQL As New SqlCommand(cmd, cn, tr)
        comandoSQL.CommandType = Tipo
        Return comandoSQL
    End Function

    Public Shared Function EjecutaConsulta(ByVal command As SqlCommand) As DataTable
        Dim tabla As DataTable
        Try
            command.Connection.Open()
            Dim reader As SqlDataReader = command.ExecuteReader()
            tabla = New DataTable()
            tabla.Load(reader)
            reader.Close()
            command.Connection.Close()
        Catch ex As Exception
            Throw ex
        Finally
            command.Connection.Close()
        End Try
        Return tabla
    End Function

    Public Shared Function EjecutaConsultaT(ByVal command As SqlCommand) As DataTable
        Dim tabla As DataTable
        Try
            'command.Connection.Open()
            Dim reader As SqlDataReader = command.ExecuteReader()
            tabla = New DataTable()
            tabla.Load(reader)
            reader.Close()
            'command.Connection.Close()
        Catch ex As Exception
            Throw ex
        Finally
            'command.Connection.Close()
        End Try
        Return tabla
    End Function

    Public Shared Function EjecutarNonQuery(ByVal command As SqlCommand) As Integer
        Dim renglonesAfectados As Integer = -1
        Try
            command.Connection.Open()
            renglonesAfectados = command.ExecuteNonQuery()
        Catch ex1 As Exception
            Throw ex1
        Finally
            command.Connection.Close()
        End Try
        Return renglonesAfectados
    End Function

    Public Shared Function EjecutarNonQueryT(ByVal command As SqlCommand) As Integer
        Dim renglonesAfectados As Integer = -1
        Try
            renglonesAfectados = command.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
            'command.Connection.Close();
        Finally

        End Try
        Return renglonesAfectados
    End Function

    Public Shared Function EjecutarScalarT(ByVal command As SqlCommand) As Integer
        Dim Identidad As Object = ""
        Try
            Identidad = command.ExecuteScalar()
        Catch ex As Exception
            Throw ex
            'command.Connection.Close();
        Finally
        End Try
        Return Convert.ToInt32(Identidad)
    End Function

    Public Shared Function EjecutarScalar(ByVal command As SqlCommand) As Object
        Dim valorObtenido As Object = ""

        Try
            command.Connection.Open()
            valorObtenido = command.ExecuteScalar()
        Catch ex As Exception
            Throw ex
        Finally
            command.Connection.Close()
        End Try
        Return valorObtenido
    End Function

    Public Shared Function Crea_Parametro(ByRef cm As SqlCommand, ByVal nombre As String, ByVal tipo As SqlDbType, ByVal valor As Object) As SqlParameter
        Dim para As New SqlParameter()
        If tipo = SqlDbType.Int Or tipo = SqlDbType.TinyInt Then
            'numerico
            para = cm.CreateParameter()
            para.ParameterName = nombre
            para.Value = Integer.Parse(valor.ToString())
            para.SqlDbType = SqlDbType.Int
            cm.Parameters.Add(para)
            Return para
        End If
        If tipo = SqlDbType.BigInt Then
            'numerico
            para = cm.CreateParameter()
            para.ParameterName = nombre
            para.Value = Long.Parse(valor.ToString())
            para.SqlDbType = SqlDbType.BigInt
            cm.Parameters.Add(para)
            Return para
        End If
        If tipo = SqlDbType.VarChar Or tipo = SqlDbType.Text Then
            'string o text
            para = cm.CreateParameter()
            para.ParameterName = nombre
            para.Value = valor.ToString().Trim().ToUpper()
            para.SqlDbType = SqlDbType.VarChar
            cm.Parameters.Add(para)
            Return para
        End If
        If tipo = SqlDbType.Money Then
            para = cm.CreateParameter()
            para.ParameterName = nombre
            para.Value = Double.Parse(valor.ToString())
            para.SqlDbType = SqlDbType.Money
            cm.Parameters.Add(para)
            Return para
        End If
        If tipo = SqlDbType.DateTime Or tipo = SqlDbType.Date Then
            para = cm.CreateParameter()
            para.ParameterName = nombre
            para.Value = CDate(valor)
            para.SqlDbType = SqlDbType.DateTime
            cm.Parameters.Add(para)
            'break;
            Return para
        End If
        If tipo = SqlDbType.Time Then
            para = cm.CreateParameter()
            para.ParameterName = nombre
            para.Value = valor.ToString.Trim
            para.SqlDbType = SqlDbType.Time
            cm.Parameters.Add(para)
            'break;
            Return para
        End If
        If tipo = SqlDbType.Image Then
            para = cm.CreateParameter()
            para.ParameterName = nombre
            para.SqlDbType = SqlDbType.Image
            para.Value = valor
            cm.Parameters.Add(para)
            Return para
        End If
        Return para
    End Function

    Public Shared Function CreaComando(ByVal Procedimiento As String) As SqlCommand
        Dim cmd As New SqlCommand(Procedimiento, Crea_ConexionSTD)
        cmd.CommandType = CommandType.StoredProcedure
        Return cmd
    End Function

End Class
