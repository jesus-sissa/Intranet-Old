Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports IntranetSIAC.Cn_Datos
Imports IntranetSIAC.FuncionesGlobales

Public Class Cn_Login
    Public Shared Function Usuarios_Login(ByVal Id_Empleado As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = Crea_Comando("Usr_UsuariosLogin_Read", CommandType.StoredProcedure, Crea_ConexionSTD)
            Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function Login_Insertar(ByVal EmpleadoID As Integer, ByVal SucursalId As Integer, ByVal ModuloClave As String, ByVal EstacioN As String, ByVal EstacionIP As String, ByVal EstacionMAC As String, ByVal ModuloVersion As String) As Integer
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Usr_Login_Create", CommandType.StoredProcedure, Cnn)

        Try
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.BigInt, EmpleadoID)
            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.BigInt, SucursalId)
            Crea_Parametro(Cmd, "@Clave_Modulo", SqlDbType.VarChar, ModuloClave)
            Crea_Parametro(Cmd, "@Estacion", SqlDbType.VarChar, EstacioN)
            Crea_Parametro(Cmd, "@EstacionIP", SqlDbType.VarChar, EstacionIP)
            Crea_Parametro(Cmd, "@EstacionMAC", SqlDbType.VarChar, EstacionMAC)
            Crea_Parametro(Cmd, "@Version", SqlDbType.VarChar, ModuloVersion)

            Dim valor As Integer = EjecutarScalar(Cmd)
            Return valor
        Catch e As Exception
            Return 0
        End Try
    End Function

    Public Shared Function Login_CerrarSesion(ByVal LoginId As Integer, ByVal EstacioN As String, ByVal EstacionIP As String, ByVal EstacionMac As String) As Boolean
        Try
            Dim Cnn As SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlCommand = Crea_Comando("Usr_Login_CierraSesion", CommandType.StoredProcedure, Cnn)

            Crea_Parametro(Cmd, "@Id_Login", SqlDbType.Int, LoginId)
            Crea_Parametro(Cmd, "@Estacion", SqlDbType.VarChar, EstacioN)
            Crea_Parametro(Cmd, "@EstacionIP", SqlDbType.VarChar, EstacionIP)
            Crea_Parametro(Cmd, "@EstacionMAC", SqlDbType.VarChar, EstacionMac)

            EjecutarNonQuery(Cmd)
            Return True
        Catch Ex As Exception
            'TrataEx(Ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_Log_Create(ByVal UsuarioID As Integer, ByVal ClaveModulo As String, ByVal Descripcion As String, ByVal EstacioN As String, ByVal EstacionIP As String, ByVal EstacionMac As String, ByVal ModuloVersion As String, ByVal LoginId As Integer, ByVal SucursalID As Integer) As Boolean
        Dim CmD As New SqlCommand
        Dim Id_Log As Integer
        Try
            CmD = Crea_Comando("Usr_Log_Create", CommandType.StoredProcedure, Cn_Datos.Crea_ConexionSTD)
            Crea_Parametro(CmD, "@Id_Empleado", SqlDbType.Int, UsuarioID)
            Crea_Parametro(CmD, "@Clave_Modulo", SqlDbType.VarChar, ClaveModulo)
            Crea_Parametro(CmD, "@Descripcion", SqlDbType.VarChar, Descripcion)
            Crea_Parametro(CmD, "@Estacion", SqlDbType.VarChar, EstacioN)
            Crea_Parametro(CmD, "@EstacionIP", SqlDbType.VarChar, EstacionIP)
            Crea_Parametro(CmD, "@EstacionMAC", SqlDbType.VarChar, EstacionMac)
            Crea_Parametro(CmD, "@Modulo_Version", SqlDbType.VarChar, ModuloVersion)
            Crea_Parametro(CmD, "@Id_Login", SqlDbType.Int, LoginId)

            Id_Log = EjecutarScalar(CmD)

            Return Id_Log > 0
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    'Public Shared Function Permisos_Read(ByVal Id As Integer, ByVal ModuloClave As String) As DataTable
    '    Try
    '        Dim Cnn As SqlConnection = Crea_ConexionSTD()
    '        Dim Cmd As SqlCommand = Crea_Comando("Usr_Permisos_Get", CommandType.StoredProcedure, Cnn)

    '        Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id)
    '        Crea_Parametro(Cmd, "@Clave_Modulo", SqlDbType.VarChar, ModuloClave)

    '        Dim Tbl As DataTable = EjecutaConsulta(Cmd)
    '        Return Tbl
    '    Catch ex As Exception
    '        'TrataEx(ex)
    '        Return Nothing
    '    End Try
    'End Function

    Public Shared Function UsuariosContra_Update(ByVal Id_Usuario As Integer, ByVal ModuloClave As String, ByVal Contra As String, ByVal EstacioN As String, ByVal EstacionIp As String, ByVal EstacionMac As String, ByVal ModuloVersion As String) As Integer
        Dim Valor As Integer
        Dim Id_Contra As Integer = 0

        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim tr As SqlTransaction = crear_Trans(Cnn)
        Dim Cmd As SqlCommand

        Try
            Cmd = Crea_ComandoT(Cnn, tr, CommandType.StoredProcedure, "Usr_Contras_Create")
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Usuario)
            Crea_Parametro(Cmd, "@Clave_Modulo", SqlDbType.VarChar, ModuloClave)
            Crea_Parametro(Cmd, "@Contra", SqlDbType.VarChar, Contra)
            Crea_Parametro(Cmd, "@Estacion", SqlDbType.VarChar, EstacioN)
            Crea_Parametro(Cmd, "@EstacionIP", SqlDbType.VarChar, EstacionIp)
            Crea_Parametro(Cmd, "@EstacionMAC", SqlDbType.VarChar, EstacionMac)
            Crea_Parametro(Cmd, "@Modulo_Version", SqlDbType.VarChar, ModuloVersion)

            Id_Contra = EjecutarScalarT(Cmd)

            Cmd = Crea_ComandoT(Cnn, tr, CommandType.StoredProcedure, "Usr_UsuariosContra_Update")
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.BigInt, Id_Usuario)
            Crea_Parametro(Cmd, "@Contra", SqlDbType.VarChar, Contra)

            Valor = EjecutarNonQueryT(Cmd)

            tr.Commit()
            Cmd.Dispose()
            Return Valor
        Catch ex As Exception
            tr.Rollback()
            Return 0
            'TrataEx(ex)
        End Try
    End Function

    Public Shared Function UsuariosContra_Consultar10(ByVal Id As Integer) As DataTable
        Try
            Dim Cnn As SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlCommand = Crea_Comando("Usr_Contras_Get10", CommandType.StoredProcedure, Cnn)

            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id)
            Dim Tbl As DataTable = EjecutaConsulta(Cmd)
            Return Tbl
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Login_ObtenerMensajes(ByVal Clave_Modulo As String, ByVal SucursalID As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim cnn As SqlConnection = Crea_ConexionSTD()
        Dim cmd As SqlCommand = Crea_Comando("Usr_ModulosMensajes_Get", CommandType.StoredProcedure, cnn)
        Dim dt As DataTable = Nothing
        Try
            Crea_Parametro(cmd, "@Clave_Modulo", SqlDbType.VarChar, Clave_Modulo)
            dt = EjecutaConsulta(cmd)
            If dt.Rows.Count > 0 Then
                Return dt
            Else
                Return Nothing
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_ObtenTurno(ByVal SucursalID As Integer) As Integer
        Try
            Dim Cnn As SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlCommand = Crea_Comando("Bo_Turnos_ActualGet", CommandType.StoredProcedure, Cnn)

            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.BigInt, SucursalID)

            Return EjecutarScalar(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return 0
        End Try
    End Function

End Class
