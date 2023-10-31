Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports IntranetSIAC.Cn_Datos
Imports IntranetSIAC.FuncionesGlobales
Imports System.Object
Imports System.Linq.Enumerable
Imports System.IO

Public Class Cn_Soporte

#Region "Variables Privadas"

    Private Session As HttpSessionState
    Private Request As HttpRequest

#End Region

#Region "Constructores"

    Public Sub New(ByVal MySession As HttpSessionState, ByVal MyRequest As HttpRequest)
        Session = MySession
        Request = MyRequest
    End Sub

#End Region

#Region "Propiedades"

    Public Property Id_Usuario() As Integer
        Get
            Dim res As Integer = 0
            Integer.TryParse(Session("UsuarioID"), res)
            Return res
        End Get
        Set(ByVal value As Integer)
            Session("UsuarioID") = value
        End Set
    End Property

    Public Property Id_Sucursal() As Integer
        Get
            Dim res As Integer = 0
            Integer.TryParse(Session("SucursalID"), res)
            Return res
        End Get
        Set(ByVal value As Integer)
            Session("SucursalID") = value
        End Set
    End Property

    Public Property ClaveModulo() As String
        Get
            Return Session("ModuloClave")
        End Get
        Set(ByVal value As String)
            Session("ModuloClave") = value
        End Set
    End Property

    Public Property Nivel_Usuario() As Integer
        Get
            Dim res As Byte = 0
            Byte.TryParse(Session("usuarioNivel"), res)
            Return res
        End Get
        Set(ByVal value As Integer)
            Session("usuarioNivel") = value
        End Set
    End Property

    Public Property NombreUsuario() As String
        Get
            Return Session("UsuarioNombre")
        End Get
        Set(ByVal value As String)
            Session("UsuarioNombre") = value
        End Set
    End Property

#End Region

#Region "Base"

    Public Function fn_ValidaPermisos() As Boolean

        If Id_Usuario = 0 Then Return False
        Dim userID As Integer = 0
        If Nivel_Usuario = 1 Then userID = Id_Usuario

        Dim cmd As SqlCommand = CreaComando("Usr_Permisos_Get")
        Crea_Parametro(cmd, "@Clave_Modulo", SqlDbType.VarChar, ClaveModulo)
        Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, userID)
        Dim dt_Opciones As Data.DataTable = EjecutaConsulta(cmd)
        dt_Opciones.CaseSensitive = False

        Dim dr_Enlaces As DataRow() = Nothing
        Dim rutaCompleta As String = String.Empty
        Dim carpetaAplicacion As String = Request.ApplicationPath.ToUpper
        Dim vacio As String = ""
        If carpetaAplicacion = "/" Then vacio = "/"
        rutaCompleta = Request.Url.AbsolutePath.Trim.ToUpper.Replace(carpetaAplicacion, vacio)
        dr_Enlaces = dt_Opciones.Select("Enlace = '~" & rutaCompleta & "'")

        Return dr_Enlaces.Count > 0

    End Function

#End Region

#Region "Master"

    Public Sub fn_LoadMenu(ByRef Mapa As Menu, ByVal SucursalId As Integer, ByVal Id_Usuario As Integer, ByVal Clave_Modulo As String)
        Mapa.Items.Clear()

        Try

            Dim cnn As SqlConnection = Crea_ConexionSTD()
            Dim cmd As SqlCommand = Crea_Comando("Usr_UsuariosLogin_Read", CommandType.StoredProcedure, cnn)
            Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, Id_Usuario)
            Dim dt_Tipo As DataTable = EjecutaConsulta(cmd)
            If dt_Tipo Is Nothing OrElse dt_Tipo.Rows.Count = 0 Then Exit Sub

            cmd = Crea_Comando("Usr_MenusModulo_Get", CommandType.StoredProcedure, cnn)
            Crea_Parametro(cmd, "@Clave_Modulo", SqlDbType.VarChar, Clave_Modulo)
            Dim tbl_Menus As DataTable = EjecutaConsulta(cmd)

            cmd = Crea_Comando("Usr_Permisos_Get", CommandType.StoredProcedure, cnn)
            Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, IIf(dt_Tipo.Rows(0).Item("Tipo") = 1, Id_Usuario, 0))
            Crea_Parametro(cmd, "@Clave_Modulo", SqlDbType.VarChar, Clave_Modulo)
            Dim tbl_Opciones As DataTable = EjecutaConsulta(cmd)

            For Each r As DataRow In tbl_Menus.Rows
                If r("Status") = "ACTIVO" Then
                    Dim mi_menu As New MenuItem(r("Descripcion"))

                    For Each dr_Opcion As DataRow In tbl_Opciones.Select("Id_Menu = " & r("Id_Menu"))
                        Dim ci As New MenuItem(dr_Opcion("Nombre_Corto").ToString.ToUpper)
                        ci.NavigateUrl = dr_Opcion("Enlace")
                        mi_menu.ChildItems.Add(ci)
                    Next
                    If mi_menu.ChildItems.Count > 0 Then
                        Mapa.Items.Add(mi_menu)
                    End If
                End If
            Next
        Catch ex As Exception
            TrataEx(ex)
        End Try
    End Sub

#End Region

#Region "Alertas"

    Public Shared Function fn_AlertasGeneradas_Guardar(ByVal SucursalId As Integer, ByVal UsuarioId As Integer, ByVal EstacioN As String, ByVal Clave_AlertaTipo As String, ByVal Detalles As String) As Boolean
        Try
            Dim cmd As SqlCommand = Crea_Comando("Cat_AlertasGeneradas_Create", CommandType.StoredProcedure, Crea_ConexionSTD())
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Crea_Parametro(cmd, "@Clave_AlertaTipo", SqlDbType.VarChar, Clave_AlertaTipo)
            Crea_Parametro(cmd, "@Detalles", SqlDbType.Text, Detalles)
            Crea_Parametro(cmd, "@Id_EmpleadoGenera", SqlDbType.Int, UsuarioId)
            Crea_Parametro(cmd, "@Estacion_Genera", SqlDbType.VarChar, EstacioN)
            Crea_Parametro(cmd, "@Tipo_Alerta", SqlDbType.Int, 1)   'Alerta por Incidencia
            EjecutarScalar(cmd)
            Return True
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_AlertasGeneradas_Validar(ByVal SucursalId As Integer, ByVal UsuarioId As Integer, claveAlertaTipo As String) As Boolean
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlClient.SqlCommand = Crea_Comando("Cat_AlertasGeneradas_Existe", CommandType.StoredProcedure, Cnn)
        Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
        Crea_Parametro(Cmd, "@Clave_AlertaTipo", SqlDbType.VarChar, claveAlertaTipo) 'tenia '05' ????

        Try
            Dim dt As DataTable = EjecutaConsulta(Cmd)
            If dt IsNot Nothing AndAlso dt.Rows.Count = 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_AlertasGeneradas_ObtenerMails(ByVal Clave_AlertaTipo As String) As DataTable
        Try
            Dim cnn As SqlConnection = Crea_ConexionSTD()
            Dim cmd As SqlCommand = Crea_Comando("Cat_AlertasDestinos_GetMail", CommandType.StoredProcedure, cnn)
            Crea_Parametro(cmd, "@Clave_AlertaTipo", SqlDbType.VarChar, Clave_AlertaTipo)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            'TrataEx(ex, SucursalId, UsuarioID)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Empleados.aspx"

    Public Shared Function fn_Default_GetEmpleados(ByVal Id_Sucursal As Integer, ByVal Id_EmpleadoJefe As Integer, ByVal NuevoIngreso As Char) As DataTable
        Try
            Dim cmd As SqlCommand = Crea_Comando("Cat_Empleados_GetIntranet", CommandType.StoredProcedure, Crea_ConexionSTD)

            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, Id_Sucursal)
            Crea_Parametro(cmd, "@Id_EmpleadoJefe", SqlDbType.Int, Id_EmpleadoJefe)
            Crea_Parametro(cmd, "@NuevoIngreso", SqlDbType.VarChar, NuevoIngreso)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Empleados_ObtenValores(ByVal Id_Empleado As Integer) As DataRow
        'Aqui se obtienen todos los valores de un registro en particular
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Cat_Empleados_ReadWEB", CommandType.StoredProcedure, Cnn)

        Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
        Try
            Dim Tbl As DataTable = EjecutaConsulta(Cmd)
            If Tbl IsNot Nothing AndAlso Tbl.Rows.Count > 0 Then
                Return Tbl.Rows(0)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Rasgos_ObtenValores(ByVal Id As Integer, ByVal Id_Sucursal As Integer, ByVal UsuarioID As Integer) As DataRow
        'Aqui se obtiene todos los Rasgos de un Empleado seleccionado
        Try
            Dim Cnn As SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlCommand = Crea_Comando("Cat_EmpleadosRasgos_Read", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id)

            Dim dt As DataTable = EjecutaConsulta(Cmd)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Return dt.Rows(0)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_EmpleadosEscolares_ObtenValores(ByVal Id_Empleado As Integer, ByVal Id_Sucursal As Integer, ByVal UsuarioID As Integer) As DataRow
        'Aqui se obtienen todos los valores de un registro en particular
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Cat_EmpleadosEscolares_ReadWEB", CommandType.StoredProcedure, Cnn)

        Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
        Try
            Dim Tbl As DataTable = EjecutaConsulta(Cmd)

            If Tbl IsNot Nothing AndAlso Tbl.Rows.Count > 0 Then
                Return Tbl.Rows(0)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_CursosRecibidos_ObtenValores(ByVal Id As Integer, ByVal Id_Sucursal As Integer, ByVal UsuarioID As Integer) As DataTable
        'Aqui se obtienen todos los CURSOS RECIBIDOS de un Empleado en particular
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Cat_EmpleadosCursos_Read", CommandType.StoredProcedure, Cnn)
        Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id)

        Try
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_DatosFamiliares_ObtenValores(ByVal Id As Integer, ByVal Id_Sucursal As Integer, ByVal UsuarioID As Integer) As DataTable
        'Aqui se obtienen todos los DATOS FAMILIARES de un Empleado en particular
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Cat_EmpleadosFamiliares_Read", CommandType.StoredProcedure, Cnn)

        Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id)

        Try
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Empleos_ObtenValores(ByVal Id As Integer, ByVal Id_Sucursal As Integer, ByVal UsuarioID As Integer) As DataTable
        'Aqui se obtienen todos los EMPLEOS anteriores de un Empleado en particular
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Cat_EmpleadosLaborales_Read", CommandType.StoredProcedure, Cnn)

        Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id)
        Try
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Referencias_ObtenValores(ByVal Id As Integer, ByVal Id_Sucursal As Integer, ByVal UsuarioID As Integer) As DataTable
        'Aqui se obtienen todas las REFERENCIAS de un Empleado en particular
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Cat_EmpleadosReferencias_Read", CommandType.StoredProcedure, Cnn)

        Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id)
        Try
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_EmpleadosIngresos_ObtenValores(ByVal Id_Empleado As Integer, ByVal Id_Sucursal As Integer, ByVal UsuarioID As Integer) As DataRow
        'Aqui se obtienen todos los valores de un registro en particular
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Cat_EmpleadosIngresos_Read", CommandType.StoredProcedure, Cnn)

        Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
        Try
            Dim Tbl As DataTable = EjecutaConsulta(Cmd)

            If Tbl IsNot Nothing AndAlso Tbl.Rows.Count > 0 Then
                Return Tbl.Rows(0)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_SeñasParticulares_ObtenValores(ByVal Id As Integer, ByVal Id_Sucursal As Integer, ByVal UsuarioID As Integer) As DataTable
        'Aqui se obtienen todas las Señas Particulares de un Empleado en particular
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Cat_EmpleadosSenas_Read", CommandType.StoredProcedure, Cnn)

        Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id)
        Try
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_DocumentosRequeridos_ObtenValores(ByVal Id As Integer) As DataTable
        'Aqui se obtiene toda la Papeleria Recibida de un Empleado seleccionado
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Cat_EmpleadosDoctosR_GetEmpleado", CommandType.StoredProcedure, Cnn)
        Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id)
        Try
            Dim DT As DataTable = EjecutaConsulta(Cmd)
            Return DT
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Empleados_LeerImagenes(ByVal Id_Empleado As Integer, ByVal Id_Sucursal As Integer, ByVal UsuarioID As Integer) As DataRow
        Dim con As SqlConnection = Crea_ConexionSTD()
        Dim com As SqlCommand = Crea_Comando("Cat_EmpleadosI_Read", CommandType.StoredProcedure, con)
        Crea_Parametro(com, "@Id_Empleado", SqlDbType.Int, Id_Empleado)

        Try
            'Leer de SQL
            Dim dt As DataTable = EjecutaConsulta(com)

            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Empleados_ActualizarNuevoIngreso(ByVal Id_Empleado As Integer, ByVal UsuarioValidaIN As Integer, ByVal EstacionValidaIN As String, ByVal ComentariosValidaIN As String, ByVal Id_Sucursal As Integer, ByVal UsuarioID As Integer) As Boolean
        Dim cmd As SqlCommand = Crea_Comando("CAt_Empleados_UpdateNuevoIngreso", CommandType.StoredProcedure, Crea_ConexionSTD)
        Try
            Crea_Parametro(cmd, "@IDEmpleado", SqlDbType.Int, Id_Empleado)
            Crea_Parametro(cmd, "@UsuarioValidaNI", SqlDbType.Int, UsuarioValidaIN)
            Crea_Parametro(cmd, "@EstacionValidaNI", SqlDbType.VarChar, EstacionValidaIN)
            Crea_Parametro(cmd, "@ComentariosValidaNI", SqlDbType.VarChar, ComentariosValidaIN)

            Return EjecutarNonQuery(cmd) > 0
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

#End Region

#Region "Prospectos"

    Public Shared Function fn_Prospectos_GetProspectos(ByVal Id_EmpleadoJefe As Integer) As DataTable
        Dim cmd As SqlCommand = Crea_Comando("Cat_EmpleadosP_GetWEB", CommandType.StoredProcedure, Crea_ConexionSTD)
        Try
            Crea_Parametro(cmd, "@Id_EmpleadoJefe", SqlDbType.Int, Id_EmpleadoJefe)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Prospectos_ObtenDatosGenerales(ByVal Id_Sucursal As Integer, ByVal Id_Empleado As Integer, ByVal UsuarioID As Integer) As DataRow

        'Aqui se obtienen todos los valores de un registro en particular
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Cat_EmpleadosP_Get", CommandType.StoredProcedure, Cnn)

        Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, Id_Sucursal)
        Crea_Parametro(Cmd, "@Id_EmpleadoP", SqlDbType.Int, Id_Empleado)

        Try
            Dim Tbl As DataTable = EjecutaConsulta(Cmd)

            If Tbl Is Nothing OrElse Tbl.Rows.Count = 0 Then
                Return Nothing
            Else
                Return Tbl.Rows(0)
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Prospectos_GetEntrevistas(ByVal Id_Empleado As Integer, ByVal Id_Sucursal As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim cmd As SqlCommand = Crea_Comando("Cat_EmpleadosPentrevistas_Get", CommandType.StoredProcedure, Crea_ConexionSTD)
        Try
            Crea_Parametro(cmd, "@Id_EmpleadoP", SqlDbType.Int, Id_Empleado)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Prospectos_GuardarEntrevista(ByVal Id_EmpleadoP As Integer, ByVal FechaEntrevista As Date, ByVal EmpleadoEntrevista As Integer, ByVal EstacionRegistro As String, ByVal Apto As String, ByVal Comentarios As String, ByVal Id_Sucursal As Integer, ByVal UsuarioID As Integer) As Boolean

        Dim cmd As SqlCommand = Crea_Comando("Cat_EmpleadosPentrevistas_Create", CommandType.StoredProcedure, Crea_ConexionSTD)

        Try
            Crea_Parametro(cmd, "@Id_EmpleadoP", SqlDbType.Int, Id_EmpleadoP)
            Crea_Parametro(cmd, "@Fecha_Entrevista", SqlDbType.Date, FechaEntrevista)
            Crea_Parametro(cmd, "@Empleado_Entrevista", SqlDbType.Int, EmpleadoEntrevista)
            Crea_Parametro(cmd, "@Usuario_Registro", SqlDbType.Int, EmpleadoEntrevista)
            Crea_Parametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, EstacionRegistro)
            Crea_Parametro(cmd, "@Apto", SqlDbType.VarChar, Apto)
            Crea_Parametro(cmd, "@Comentarios", SqlDbType.VarChar, Comentarios)

            Return EjecutarNonQuery(cmd) > 0

        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try

    End Function

#End Region

#Region "Plantilla Laboral"

    Public Shared Function fn_PlantillaLaboral_ObtenerDepto(ByVal DepartamentoId As Integer, ByVal SucursalID As Integer, ByVal UsuarioID As Integer) As DataRow
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand
        Dim dt As DataTable

        Try
            Cmd = Crea_Comando("Cat_Departamentos_Read", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_Departamento", SqlDbType.Int, DepartamentoId)

            dt = EjecutaConsulta(Cmd)
            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_PlantillaLaboral_LlenarLista(ByVal SucursalID As Integer, ByVal Id_Depto As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand

        Try
            Cmd = Crea_Comando("Cat_PuestosPlantilla_Get", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalID)
            Crea_Parametro(Cmd, "@Id_Departamento", SqlDbType.Int, Id_Depto)

            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_PlantillaLaboral_Actualizar(ByVal PlantillaID As Integer, ByVal Id_Departamento As Integer, ByVal Id_Puesto As Integer, ByVal SucursalID As Integer, ByVal PlantillaRequerida_Anterior As Integer, ByVal PlantillaRequerida_Nueva As Integer, ByVal PlantillaActual As Integer, ByVal Comentarios As String, ByVal UsuarioID As Integer, ByVal Estacion As String) As Boolean
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Trans As SqlClient.SqlTransaction = crear_Trans(Cnn)
        Dim cmd As SqlClient.SqlCommand

        Try
            'Aquí se actualiza la Plantilla Requerida de un Puesto
            cmd = Crea_ComandoT(Cnn, Trans, CommandType.StoredProcedure, "Cat_PuestosPlantilla_Update")
            Crea_Parametro(cmd, "@Id_Plantilla", SqlDbType.Int, PlantillaID)
            Crea_Parametro(cmd, "@PlantillaRequerida", SqlDbType.Int, PlantillaRequerida_Nueva)
            Crea_Parametro(cmd, "@PlantillaActual", SqlDbType.Int, PlantillaActual)
            EjecutarNonQueryT(cmd)

            'Aquí se inserta un registro en el Historial
            cmd = Crea_ComandoT(Cnn, Trans, CommandType.StoredProcedure, "Cat_PuestosPlantillaH_Create")
            cmd.Parameters.Clear()
            Crea_Parametro(cmd, "@Id_Plantilla", SqlDbType.Int, PlantillaID)
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalID)
            Crea_Parametro(cmd, "@Id_Departamento", SqlDbType.Int, Id_Departamento)
            Crea_Parametro(cmd, "@Id_Puesto", SqlDbType.Int, Id_Puesto)
            Crea_Parametro(cmd, "@Plantilla_RequeridaAnt", SqlDbType.Int, PlantillaRequerida_Anterior)
            Crea_Parametro(cmd, "@Plantilla_Requerida", SqlDbType.Int, PlantillaRequerida_Nueva)
            Crea_Parametro(cmd, "@Plantilla_Actual", SqlDbType.Int, PlantillaActual)
            Crea_Parametro(cmd, "@Comentarios", SqlDbType.VarChar, Comentarios)
            Crea_Parametro(cmd, "@Usuario_Registro", SqlDbType.Int, UsuarioID)
            Crea_Parametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, Estacion)
            Crea_Parametro(cmd, "@Modo_Registro", SqlDbType.Int, 2)
            EjecutarNonQueryT(cmd)
        Catch ex As Exception
            Trans.Rollback()
            TrataEx(ex)
            Return False
        End Try

        Trans.Commit()
        Return True
    End Function

    Public Shared Function fn_PlantillaLaboral_ObtenerPuestos(ByVal SucursalID As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand

        Try
            Cmd = Crea_Comando("Cat_Puestos_Get", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Pista", SqlDbType.VarChar, "")

            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_PlantillaLaboral_Guardar(ByVal SucursalId As Integer, ByVal Id_Departamento As Integer, ByVal Id_Puesto As Integer, ByVal Plantilla_Requerida As Integer, ByVal PlantillaActual As Integer, ByVal Comentarios As String, ByVal UsuarioID As Integer, ByVal Estacion As String) As Boolean

        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Trans As SqlClient.SqlTransaction = crear_Trans(Cnn)
        Dim Cmd As SqlCommand

        Try
            Cmd = Crea_ComandoT(Cnn, Trans, CommandType.StoredProcedure, "Cat_PuestoPlantilla_Create")
            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Crea_Parametro(Cmd, "@Id_Departamento", SqlDbType.Int, Id_Departamento)
            Crea_Parametro(Cmd, "@Id_Puesto", SqlDbType.Int, Id_Puesto)
            Crea_Parametro(Cmd, "@PlantillaRequerida", SqlDbType.Int, Plantilla_Requerida)
            Crea_Parametro(Cmd, "@PlantillaActual", SqlDbType.Int, PlantillaActual)
            Crea_Parametro(Cmd, "@Comentarios", SqlDbType.VarChar, Comentarios)
            Dim PlantillaID As Integer = EjecutarScalarT(Cmd)

            Cmd = Crea_ComandoT(Cnn, Trans, CommandType.StoredProcedure, "Cat_PuestosPlantillaH_Create")
            Cmd.Parameters.Clear()
            Crea_Parametro(Cmd, "@Id_Plantilla", SqlDbType.Int, PlantillaID)
            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Crea_Parametro(Cmd, "@Id_Departamento", SqlDbType.Int, Id_Departamento)
            Crea_Parametro(Cmd, "@Id_Puesto", SqlDbType.Int, Id_Puesto)
            Crea_Parametro(Cmd, "@Plantilla_RequeridaAnt", SqlDbType.Int, 0)
            Crea_Parametro(Cmd, "@Plantilla_Requerida", SqlDbType.Int, Plantilla_Requerida)
            Crea_Parametro(Cmd, "@Plantilla_Actual", SqlDbType.Int, PlantillaActual)
            Crea_Parametro(Cmd, "@Comentarios", SqlDbType.VarChar, Comentarios)
            Crea_Parametro(Cmd, "@Usuario_Registro", SqlDbType.Int, UsuarioID)
            Crea_Parametro(Cmd, "@Estacion_Registro", SqlDbType.VarChar, Estacion)
            Crea_Parametro(Cmd, "@Modo_Registro", SqlDbType.Int, 2)
            EjecutarNonQueryT(Cmd)

        Catch ex As Exception
            Trans.Rollback()
            TrataEx(ex)
            Return False
        End Try

        Trans.Commit()
        Return True
    End Function

    Public Shared Function fn_PlantillaLaboral_ObtenerEmpleadosXPuesto(ByVal SucursalId As Integer, ByVal DepartamentoID As Integer, ByVal PuestoID As Integer, ByVal UsuarioID As Integer) As Integer

        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand

        Try
            Cmd = Crea_Comando("Cat_Empleados_GetByPuestoWEB", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Crea_Parametro(Cmd, "@Id_Departamento", SqlDbType.Int, DepartamentoID)
            Crea_Parametro(Cmd, "@Id_Puesto", SqlDbType.Int, PuestoID)
            Crea_Parametro(Cmd, "@Status", SqlDbType.VarChar, "A")

            Dim tabla As DataTable = EjecutaConsulta(Cmd)
            Return tabla.Rows.Count

        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

#End Region

#Region "Cursos Programados"

    Public Shared Function fn_CursosProgramados_ObtenerCursos(ByVal SucursalId As Integer, ByVal DepartamentoID As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim Dt As DataTable
        'Aqui se llena el listview
        Dim Cnn As SqlClient.SqlConnection = Crea_ConexionSTD()
        Try
            Dim Cmd As SqlClient.SqlCommand = Crea_Comando("Cap_Programacion_Get30Dias", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Crea_Parametro(Cmd, "@Id_Departamento", SqlDbType.Int, DepartamentoID)

            Dt = Cn_Datos.EjecutaConsulta(Cmd)
            If Dt.Rows.Count > 0 Then
                Return Dt
            Else
                Return Nothing
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_CursosProgramados_ObtenerEmpleados(ByVal DepartamentoID As Integer, ByVal SucursalId As Integer, ByVal UsuarioID As Integer) As DataTable
        Try
            Dim cmd As SqlCommand
            cmd = Crea_Comando("Cat_Empleados_ComboGetAP_ByDepto", CommandType.StoredProcedure, Crea_ConexionSTD)
            Crea_Parametro(cmd, "@Id_Departamento", SqlDbType.Int, DepartamentoID)
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_CursosProgramados_Guardar(ByVal Id_Programacion As Integer, ByVal Id_Empleado As Integer, ByVal Usuario_Registro As Integer, ByVal Estacion_Registro As String, ByVal SucursalId As Integer, ByVal UsuarioID As Integer) As Boolean

        Dim cmd As SqlCommand = Crea_Comando("Cap_ProgramacionEmpleados_Create", CommandType.StoredProcedure, Crea_ConexionSTD)

        Try
            Crea_Parametro(cmd, "@Id_Programacion", SqlDbType.Int, Id_Programacion)
            Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            Crea_Parametro(cmd, "@Usuario_Registro", SqlDbType.Int, Usuario_Registro)
            Crea_Parametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, Estacion_Registro)

            Return EjecutarNonQuery(cmd) > 0

        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try

    End Function

    Public Shared Function fn_CursosProgramados_ObtenerEmpleadosProgramados(ByVal Id_Programacion As Integer, ByVal SucursalId As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim Dt As DataTable
        'Aqui se llena el listview
        Dim Cnn As SqlClient.SqlConnection = Crea_ConexionSTD()
        Try
            Dim Cmd As SqlClient.SqlCommand = Crea_Comando("Cap_ProgramacionEmpleados_Get", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_Programacion", SqlDbType.Int, Id_Programacion)

            Dt = Cn_Datos.EjecutaConsulta(Cmd)
            Return Dt
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_CursosProgramados_Borrar(ByVal Id_Programacion As Integer, ByVal Id_Empleado As Integer, ByVal SucursalId As Integer, ByVal UsuarioID As Integer) As Boolean

        Dim cmd As SqlCommand = Crea_Comando("Cap_ProgramacionEmpleados_Delete", CommandType.StoredProcedure, Crea_ConexionSTD)

        Try
            Crea_Parametro(cmd, "@Id_Programacion", SqlDbType.Int, Id_Programacion)
            Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)

            Return EjecutarNonQuery(cmd) > 0

        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try

    End Function

    Public Shared Function fn_CursosProgramados_ObtenerTemas(ByVal Id_Programacion As Integer, ByVal SucursalId As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim Dt As DataTable
        'Aqui se llena el listview
        Dim Cnn As SqlClient.SqlConnection = Crea_ConexionSTD()
        Try
            Dim Cmd As SqlClient.SqlCommand = Crea_Comando("Cap_ProgramacionTemas_Get", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_Programacion", SqlDbType.Int, Id_Programacion)

            Dt = Cn_Datos.EjecutaConsulta(Cmd)
            Return Dt
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Reporte de Incidentes/Accidentes"

    Public Shared Function fn_Default_GetUsuarios(ByVal SucursalId As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim cmd As SqlCommand = Crea_Comando("Cat_Empleados_ComboGetActivos", CommandType.StoredProcedure, Crea_ConexionSTD)
        Try
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_RIA_Guardar(ByRef trans As SqlTransaction, ByVal SucursalId As Integer, ByVal Tipo As Integer, ByVal EntidadID As Integer, ByVal FechaIA As Date, ByVal HoraIA As String, ByVal Descripcion As String, ByVal Notas As String, ByVal Usuario_Registro As Integer, ByVal Estacion_Registro As String, ByVal Usuario_Seguimiento As Integer, ByVal Fecha_EstimadaFin As Date, ByVal Descripcion_Entidad As String) As Integer

        Try
            Dim cmd As SqlCommand = Crea_ComandoT(trans.Connection, trans, CommandType.StoredProcedure, "Cat_RIA_Create")
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Crea_Parametro(cmd, "@Tipo", SqlDbType.Int, Tipo)
            Crea_Parametro(cmd, "@Id_Entidad", SqlDbType.Int, EntidadID)
            Crea_Parametro(cmd, "@Fecha_RIA", SqlDbType.Date, FechaIA)
            Crea_Parametro(cmd, "@Hora_RIA", SqlDbType.Time, HoraIA)
            Crea_Parametro(cmd, "@Descripcion", SqlDbType.VarChar, Descripcion)
            Crea_Parametro(cmd, "@Notas_Adicionales", SqlDbType.VarChar, Notas)
            Crea_Parametro(cmd, "@Usuario_Registro", SqlDbType.Int, Usuario_Registro)
            Crea_Parametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, Estacion_Registro)
            Crea_Parametro(cmd, "@Usuario_Seguimiento", SqlDbType.Int, Usuario_Seguimiento)
            If Usuario_Seguimiento > 0 Then
                Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, "I")
                Crea_Parametro(cmd, "@Fecha_EstimadaFin", SqlDbType.Date, Fecha_EstimadaFin)
            Else
                Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, "A")
            End If

            Crea_Parametro(cmd, "@Modo_Captura", SqlDbType.Int, 1)
            Crea_Parametro(cmd, "@Descripcion_Entidad", SqlDbType.VarChar, Descripcion_Entidad)

            Dim ID As Integer = EjecutarScalarT(cmd)
            Return ID

        Catch ex As Exception
            TrataEx(ex)
            Return 0
        End Try

    End Function

    Public Shared Function fn_RIA_GuardarD(ByRef trans As SqlTransaction, ByVal SucursalId As Integer, ByVal IDRIA As Integer, ByVal Id_Entidad As Integer, ByVal Estacion_Registro As String, ByVal Descripcion As String, ByVal FechaSeguimiento As Date, ByVal HoraSeguimiento As String) As Integer

        Try
            Dim cmd As SqlCommand = Crea_ComandoT(trans.Connection, trans, CommandType.StoredProcedure, "Cat_RIAD_Create")
            Crea_Parametro(cmd, "@Id_RIA", SqlDbType.Int, IDRIA)
            Crea_Parametro(cmd, "@Tipo", SqlDbType.Int, 2)
            Crea_Parametro(cmd, "@Id_Entidad", SqlDbType.Int, Id_Entidad)
            Crea_Parametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, Estacion_Registro)
            Crea_Parametro(cmd, "@Descripcion", SqlDbType.VarChar, Descripcion)
            Crea_Parametro(cmd, "@Fecha", SqlDbType.Date, FechaSeguimiento)
            Crea_Parametro(cmd, "@Hora", SqlDbType.Time, HoraSeguimiento)
            Dim Id_RIAD As Integer = EjecutarScalarT(cmd)

            Return Id_RIAD
        Catch ex As Exception
            TrataEx(ex)
            Return 0
        End Try

    End Function

    Public Shared Function fn_IncidentesAccidentes_Guardar(ByVal SucursalId As Integer, ByVal Tipo As Integer, ByVal EntidadID As Integer, ByVal FechaIA As Date, ByVal HoraIA As String, ByVal Descripcion As String, ByVal Notas As String, ByVal Usuario_Registro As Integer, ByVal Estacion_Registro As String, ByVal Usuario_Seguimiento As Integer, ByVal Fecha_EstimadaFin As Date, ByVal Descripcion_Entidad As String, ByVal Descripcion_Detalle As String, ByRef Id_RIAD As Integer, ByVal Usuarios As String()) As Integer

        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim trans As SqlTransaction = crear_Trans(Cnn)

        'Aquí se Inserta los datos de la cabecera en CAT_RIA
        Dim Id_RIA As Integer = fn_RIA_Guardar(trans, SucursalId, Tipo, EntidadID, FechaIA, HoraIA, Descripcion, Notas, Usuario_Registro, Estacion_Registro, Usuario_Seguimiento, Fecha_EstimadaFin, Descripcion_Entidad)

        If Id_RIA = 0 Then
            trans.Rollback()
            Return 0
        End If

        'Aquí se Inserta el detalle del Seguimiento en CAT_RIAD
        Id_RIAD = fn_RIA_GuardarD(trans, SucursalId, Id_RIA, Usuario_Registro, Estacion_Registro, Descripcion_Detalle, FechaIA, HoraIA)
        If Id_RIAD = 0 Then
            trans.Rollback()
            Return 0
        End If

        'Aquí se Insertan los Usuarios para Seguimiento
        Dim cmd As SqlCommand = Crea_ComandoT(Cnn, trans, CommandType.StoredProcedure, "Cat_RiaU_Create")
        Dim campos() As String
        Try
            For Each elemento As String In Usuarios
                cmd.Parameters.Clear()
                campos = Split(elemento, ",")
                Crea_Parametro(cmd, "@Id_RIA", SqlDbType.Int, Id_RIA)
                Crea_Parametro(cmd, "@Tipo", SqlDbType.Int, 2)
                Crea_Parametro(cmd, "@Id_Entidad", SqlDbType.Int, campos(0))
                Crea_Parametro(cmd, "@Usuario_Registro", SqlDbType.VarChar, Usuario_Registro)
                Crea_Parametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, Estacion_Registro)
                If campos(1) = "PRINCIPAL" Then
                    Crea_Parametro(cmd, "@Rol", SqlDbType.Int, 1)
                ElseIf campos(1) = "SECUNDARIO" Then
                    Crea_Parametro(cmd, "@Rol", SqlDbType.Int, 2)
                End If

                EjecutarNonQueryT(cmd)
            Next
            trans.Commit()
            Return Id_RIA
        Catch ex As Exception
            trans.Rollback()
            TrataEx(ex)
            Return 0
        End Try

    End Function

    Public Shared Function fn_IncidentesAccidentes_GuardarUsuario(ByVal IDRIA As Integer, ByVal Usuario As Integer, ByVal Usuario_Registro As Integer, ByVal Estacion_Registro As String, ByVal SucursalId As Integer, ByVal TipoUsuario As Integer) As Boolean
        'Guardar Usuario Seguimiento (uno por uno)
        Dim cmd As SqlCommand = Crea_Comando("Cat_RiaU_Create", CommandType.StoredProcedure, Crea_ConexionSTD)
        Try
            Crea_Parametro(cmd, "@Id_RIA", SqlDbType.Int, IDRIA)
            Crea_Parametro(cmd, "@Tipo", SqlDbType.Int, 2)
            Crea_Parametro(cmd, "@Id_Entidad", SqlDbType.Int, Usuario)
            Crea_Parametro(cmd, "@Usuario_Registro", SqlDbType.Int, Usuario_Registro)
            Crea_Parametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, Estacion_Registro)
            Crea_Parametro(cmd, "@Rol", SqlDbType.Int, TipoUsuario)
            Return EjecutarNonQuery(cmd) > 0
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_IncidentesAccidentes_GetEmpleados(ByVal SucursalId As Integer, ByVal Usuario_Registro As Integer) As DataTable
        Dim cmd As SqlCommand = Crea_Comando("Cat_EmpleadosCombo_Get", CommandType.StoredProcedure, Crea_ConexionSTD)
        Try
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_IncidentesAccidentes_GetUnidades(ByVal SucursalId As Integer, ByVal Usuario_Registro As Integer) As DataTable
        Dim cmd As SqlCommand = Crea_Comando("Cat_Unidades_ComboGet", CommandType.StoredProcedure, Crea_ConexionSTD)
        Try
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_IncidentesAccidentes_GetClientes(ByVal SucursalId As Integer, ByVal Usuario_Registro As Integer) As DataTable
        Dim cmd As SqlCommand = Crea_Comando("Cat_Clientes_GetBuscaCliente", CommandType.StoredProcedure, Crea_ConexionSTD)
        Try
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_IncidentesAccidentes_GetRutas(ByVal SucursalId As Integer, ByVal Usuario_Registro As Integer) As DataTable
        Dim cmd As SqlCommand = Crea_Comando("Cat_Rutas_ComboGet_Intranet", CommandType.StoredProcedure, Crea_ConexionSTD)
        Try
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_IncidentesAccidentes_GuardarImagenes(ByVal SucursalId As Integer, ByVal Usuario_Registro As Integer, ByVal IDRIA As Integer, ByVal Imagen As Byte(), ByVal Descripcion As String, ByVal ID_RIAD As Integer) As Boolean
        Dim Cnn As SqlConnection = Crea_ConexionSTD()

        Try
            Dim cmd As SqlCommand = Crea_Comando("Cat_RIAI_Create", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(cmd, "@Id_RIA", SqlDbType.Int, IDRIA)
            Crea_Parametro(cmd, "@Imagen", SqlDbType.Image, Imagen)
            Crea_Parametro(cmd, "@Descripcion", SqlDbType.VarChar, Descripcion)
            Crea_Parametro(cmd, "@ID_RIAD", SqlDbType.Int, ID_RIAD)
            EjecutarNonQuery(cmd)

            Return True
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_IncidentesAccidentes_ObtenerDatos(ByVal Id_RIA As Integer, ByVal SucursalId As Integer, ByVal Usuario_Registro As Integer) As DataRow
        Dim con As SqlConnection = Crea_ConexionSTD()
        Dim com As SqlCommand = Crea_Comando("Cat_RIA_Read", CommandType.StoredProcedure, con)
        Crea_Parametro(com, "@Id_RIA", SqlDbType.Int, Id_RIA)

        Try
            'Leer de SQL
            Dim dt As DataTable = EjecutaConsulta(com)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Return dt.Rows(0)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_IncidentesAccidentes_LeerImagenes(ByVal Id_RIA As Integer, ByVal SucursalId As Integer, ByVal Usuario_Registro As Integer) As DataTable
        Dim con As SqlConnection = Crea_ConexionSTD()
        Dim com As SqlCommand = Crea_Comando("Cat_RIAI_Read", CommandType.StoredProcedure, con)
        Crea_Parametro(com, "@Id_RIA", SqlDbType.Int, Id_RIA)

        Try
            'Leer de SQL
            Dim dt As DataTable = EjecutaConsulta(com)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Return dt
            Else
                Return Nothing
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_IncidentesAccidentes_AmpliarImagenes(ByVal Id_RIAI As Integer, ByVal SucursalId As Integer, ByVal Usuario_Registro As Integer) As DataTable
        Dim con As SqlConnection = Crea_ConexionSTD()
        Dim com As SqlCommand = Crea_Comando("Cat_RIAI_ReadByID", CommandType.StoredProcedure, con)
        Crea_Parametro(com, "@Id_RIAI", SqlDbType.Int, Id_RIAI)

        Try
            Return EjecutaConsulta(com)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Consulta RIA"

    Public Shared Function fn_ConsultaRIA_LlenarLista(ByVal SucursalId As Integer, ByVal Usuario_Seguimiento As Integer, ByVal Tipo As Integer, ByVal FInicio As String, ByVal FFin As String, ByVal Status As String, ByVal UsuarioId As Integer) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand

        Try
            Cmd = Crea_Comando("Cat_RIA_Reporte", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Crea_Parametro(Cmd, "@Usuario_Seguimiento", SqlDbType.Int, Usuario_Seguimiento)
            Crea_Parametro(Cmd, "@Tipo", SqlDbType.Int, Tipo)
            Crea_Parametro(Cmd, "@FInicio", SqlDbType.VarChar, FInicio)
            Crea_Parametro(Cmd, "@FFin", SqlDbType.VarChar, FFin)
            Crea_Parametro(Cmd, "@Status", SqlDbType.VarChar, Status)
            Crea_Parametro(Cmd, "@UsuarioID", SqlDbType.Int, UsuarioId)

            Dim dt As DataTable = EjecutaConsulta(Cmd)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                Return Nothing
            Else
                Return dt
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_ConsultaRIA_GuardarD(ByVal SucursalId As Integer, ByVal IDRIA As Integer, ByVal Id_Entidad As Integer, ByVal Usuario_Seguimiento As Integer, ByVal Estacion_Registro As String, ByVal Descripcion As String, ByVal Fecha As Date, ByVal Hora As String) As Boolean
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim trans As SqlTransaction = crear_Trans(Cnn)

        Try
            If fn_RIA_GuardarD(trans, SucursalId, IDRIA, Id_Entidad, Estacion_Registro, Descripcion, Fecha, Hora) = 0 Then
                trans.Rollback()
                Cnn.Close()
                Return False
            End If

            Dim cmd As SqlCommand = Crea_ComandoT(Cnn, trans, CommandType.StoredProcedure, "Cat_RIA_Asignar")
            Crea_Parametro(cmd, "@Id_RIA", SqlDbType.Int, IDRIA)
            Crea_Parametro(cmd, "@Usuario_Seguimiento", SqlDbType.Int, Usuario_Seguimiento)
            EjecutarNonQueryT(cmd)

            trans.Commit()
            Return True
        Catch ex As Exception
            trans.Rollback()
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_ConsultaRIA_LlenarDetalle(ByVal IDRIA As Integer, ByVal SucursalID As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand

        Try
            Cmd = Crea_Comando("Cat_RIAD_Reporte", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_RIA", SqlDbType.Int, IDRIA)

            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_ConsultaRIA_ObtenerUsuarios(ByVal IDRIA As Integer, ByVal SucursalID As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand

        Try
            Cmd = Crea_Comando("Cat_RIAU_Get", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_RIA", SqlDbType.Int, IDRIA)

            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Seguimiento RIA"

    Public Shared Function fn_SeguimientoRIA_LlenarLista(ByVal SucursalID As Integer, ByVal Usuario_Seguimiento As Integer) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand

        Try
            Cmd = Crea_Comando("Cat_RIA_GetByUsuario", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalID)
            Crea_Parametro(Cmd, "@Usuario_Seguimiento", SqlDbType.Int, Usuario_Seguimiento)
            Crea_Parametro(Cmd, "@Status", SqlDbType.VarChar, "I")

            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_SeguimientoRIA_Guardar(ByVal SucursalId As Integer, ByVal Usuario_Registro As Integer, ByVal IDRIA As Integer, ByVal Estacion_Registro As String, ByVal Descripcion As String, ByVal Status As Char, ByVal FechaSeguimiento As Date, ByVal HoraSeguimiento As String) As Integer
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim trans As SqlTransaction = crear_Trans(Cnn)

        'Aquí se Inserta el detalle del Seguimiento en CAT_RIAD
        Dim Id_RIAD As Integer = fn_RIA_GuardarD(trans, SucursalId, IDRIA, Usuario_Registro, Estacion_Registro, Descripcion, FechaSeguimiento, HoraSeguimiento)
        If Id_RIAD = 0 Then
            trans.Rollback()
            Cnn.Close()
            Return 0
        End If

        If Status = "V" Then
            Try
                Dim cmd As SqlCommand = Crea_ComandoT(Cnn, trans, CommandType.StoredProcedure, "Cat_RIA_Finalizar")
                Crea_Parametro(cmd, "@Id_RIA", SqlDbType.Int, IDRIA)
                Crea_Parametro(cmd, "@Usuario_Fin", SqlDbType.Int, Usuario_Registro)
                Crea_Parametro(cmd, "@Estacion_Fin", SqlDbType.VarChar, Estacion_Registro)
                EjecutarNonQueryT(cmd)
            Catch ex As Exception
                trans.Rollback()
                Cnn.Close()
                TrataEx(ex)
                Return 0
            End Try
        End If

        trans.Commit()
        Cnn.Close()
        Return Id_RIAD
    End Function

#End Region

#Region "Enviar Mensajes"

    Public Shared Function fn_EnviarMensajes_LlenarLista(ByVal SucursalID As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim cmd As SqlCommand = Crea_Comando("Usr_Modulos_Get", CommandType.StoredProcedure, Crea_ConexionSTD)
        Try
            Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, "A")
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_EnviarMensajes_LlenarListaU(ByVal SucursalID As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim cmd As SqlCommand = Crea_Comando("Usr_UsuariosCombo_Get", CommandType.StoredProcedure, Crea_ConexionSTD)
        Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalID)
        Try
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_EnviarMensajes_Enviar(ByVal Modulos() As String, ByVal Asunto As String, ByVal Mensaje As String, ByVal UsuarioId As Integer, ByVal EstacioN As String, ByVal ModuloClave As String, ByVal SucursalID As Integer) As Boolean
        Dim Tr As SqlTransaction = crear_Trans(Crea_ConexionSTD)
        Dim cmd As SqlCommand

        For Each M As String In Modulos
            cmd = Crea_ComandoT(Tr.Connection, Tr, CommandType.StoredProcedure, "Cat_Mensajes_Create")
            Crea_Parametro(cmd, "@Usuario_Registro", SqlDbType.Int, UsuarioId)
            Crea_Parametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, EstacioN)
            Crea_Parametro(cmd, "@Modulo_Registro", SqlDbType.VarChar, ModuloClave)
            Crea_Parametro(cmd, "@Asunto", SqlDbType.VarChar, Asunto)
            Crea_Parametro(cmd, "@Mensaje", SqlDbType.VarChar, Mensaje)
            Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, "A")
            Crea_Parametro(cmd, "@Modulo_Destino", SqlDbType.VarChar, M)

            Try
                If EjecutarNonQueryT(cmd) = 0 Then
                    Tr.Rollback()
                    Return False
                End If
            Catch ex As Exception
                Tr.Rollback()
                TrataEx(ex)
                Return False
            End Try
        Next

        Tr.Commit()
        Return True
    End Function

    Public Shared Function fn_EnviarMensajesU_Enviar(ByVal Usuarios() As Integer, ByVal Asunto As String, ByVal Mensaje As String, ByVal UsuarioId As Integer, ByVal EstacioN As String, ByVal ModuloClave As String, ByVal SucursalID As Integer) As Boolean
        Dim Tr As SqlTransaction = crear_Trans(Crea_ConexionSTD)
        Dim cmd As SqlCommand

        For Each M As Integer In Usuarios
            cmd = Crea_ComandoT(Tr.Connection, Tr, CommandType.StoredProcedure, "Cat_Mensajes_Create")
            Crea_Parametro(cmd, "@Usuario_Registro", SqlDbType.Int, UsuarioId)
            Crea_Parametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, EstacioN)
            Crea_Parametro(cmd, "@Modulo_Registro", SqlDbType.VarChar, ModuloClave)
            Crea_Parametro(cmd, "@Asunto", SqlDbType.VarChar, Asunto)
            Crea_Parametro(cmd, "@Mensaje", SqlDbType.VarChar, Mensaje)
            Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, "A")
            Crea_Parametro(cmd, "@Modulo_Destino", SqlDbType.VarChar, "")
            Crea_Parametro(cmd, "@Usuario_Destino", SqlDbType.VarChar, M)

            Try
                If EjecutarNonQueryT(cmd) = 0 Then
                    Tr.Rollback()
                    Return False
                End If
            Catch ex As Exception
                Tr.Rollback()
                TrataEx(ex)
                Return False
            End Try
        Next

        Tr.Commit()
        Return True
    End Function

    Public Shared Function fn_EnviarMensajesU_ObtenerMails(ByVal SucursalID As Integer, ByVal UsuarioId As Integer) As DataTable
        Dim cnn As SqlConnection = Crea_ConexionSTD()
        Dim cmd As SqlCommand = Crea_Comando("Cat_Empleados_GetRequiereMail", CommandType.StoredProcedure, cnn)

        Try
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.VarChar, SucursalID)
            Dim tbl As DataTable = EjecutaConsulta(cmd)
            If tbl IsNot Nothing AndAlso tbl.Rows.Count > 0 Then
                Return tbl
            Else
                Return Nothing
            End If

        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Ver Mensajes"
    Public Shared Function fn_VerMensajes_LlenarLista(ByVal ModuloClave As String, ByVal Status As Char, ByVal UsuarioId As Integer, ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal Remitente As Integer, ByVal SucursalID As Integer) As DataTable
        Dim cmd As SqlCommand = Crea_Comando("Cat_Mensajes_GetNuevo", CommandType.StoredProcedure, Crea_ConexionSTD)
        Try
            Crea_Parametro(cmd, "@Clave_Modulo", SqlDbType.VarChar, ModuloClave)
            Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, Status)
            Crea_Parametro(cmd, "@Usuario_Destino", SqlDbType.Int, UsuarioId)
            Crea_Parametro(cmd, "@FechaDesde", SqlDbType.Date, FechaDesde)
            Crea_Parametro(cmd, "@FechaHasta", SqlDbType.Date, FechaHasta)
            Crea_Parametro(cmd, "@Remitente", SqlDbType.Int, Remitente)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_VerMensajes_LlenarListaEnviados(ByVal UsuarioId As Integer, ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal Destinatario As Integer, ByVal SucursalID As Integer) As DataTable
        Dim cmd As SqlCommand = Crea_Comando("Cat_Mensajes_GetEnviados", CommandType.StoredProcedure, Crea_ConexionSTD)
        Try
            Crea_Parametro(cmd, "@Usuario_Registro", SqlDbType.Int, UsuarioId)
            Crea_Parametro(cmd, "@FechaDesde", SqlDbType.Date, FechaDesde)
            Crea_Parametro(cmd, "@FechaHasta", SqlDbType.Date, FechaHasta)
            Crea_Parametro(cmd, "@Usuario_Destino", SqlDbType.Int, Destinatario)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_VerMensajes_LlenarDetalle(ByVal Id As Integer, ByVal SucursalID As Integer, ByVal UsuarioID As Integer) As DataRow
        Dim cmd As SqlCommand = Crea_Comando("Cat_Mensajes_Read", CommandType.StoredProcedure, Crea_ConexionSTD)
        Crea_Parametro(cmd, "@Id_Mensaje", SqlDbType.Int, Id)

        Try
            Dim Tbl As DataTable = EjecutaConsulta(cmd)
            If Tbl IsNot Nothing AndAlso Tbl.Rows.Count > 0 Then
                Return Tbl.Rows(0)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_VerMensajes_Status(ByVal Id As Integer, ByVal Status As Char, ByVal UsuarioID As Integer, ByVal EstacioN As String, ByVal SucursalID As Integer) As Boolean
        Dim cmd As SqlCommand = Crea_Comando("Cat_Mensajes_Status", CommandType.StoredProcedure, Crea_ConexionSTD)
        Crea_Parametro(cmd, "@Id_Mensaje", SqlDbType.Int, Id)
        Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, Status)
        Crea_Parametro(cmd, "@Estacion_Atendido", SqlDbType.VarChar, EstacioN)
        Crea_Parametro(cmd, "@Usuario_Atendido", SqlDbType.Int, UsuarioID)

        Try
            Return EjecutarNonQuery(cmd) > 0
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_VerMensajes_Responder(ByVal UsuarioId As Integer, ByVal EstacioN As String, ByVal ModuloClave As String, ByVal Asunto As String, ByVal Mensaje As String, ByVal ModuloDestino As String, ByVal UsuarioDestino As Integer, ByVal SucursalID As Integer) As Boolean
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim cmd As SqlCommand = Crea_Comando("Cat_Mensajes_Create", CommandType.StoredProcedure, Cnn)

        Crea_Parametro(cmd, "@Usuario_Registro", SqlDbType.Int, UsuarioId)
        Crea_Parametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, EstacioN)
        Crea_Parametro(cmd, "@Modulo_Registro", SqlDbType.VarChar, ModuloClave)
        Crea_Parametro(cmd, "@Asunto", SqlDbType.VarChar, Asunto)
        Crea_Parametro(cmd, "@Mensaje", SqlDbType.VarChar, Mensaje)
        Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, "A")
        Crea_Parametro(cmd, "@Modulo_Destino", SqlDbType.VarChar, "")
        Crea_Parametro(cmd, "@Usuario_Destino", SqlDbType.Int, UsuarioDestino)

        Try
            EjecutarScalar(cmd)
            Return True
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

#End Region

#Region "Actualizaciones"

    Public Shared Function fn_Actualizaciones_LlenarLista(ByVal Clave_Modulo As String, ByVal SucursalID As Integer, ByVal UsuarioID As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = Crea_Comando("Usr_Actualizaciones_GetIntranet", CommandType.StoredProcedure, Crea_ConexionSTD)
            Crea_Parametro(cmd, "@Clave_Modulo", SqlDbType.VarChar, Clave_Modulo)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Insumos"

    Public Shared Function fn_Insumos_ObtenerMaterial() As DataTable
        Dim con As SqlConnection = Crea_ConexionSTD()
        Try
            Dim cmd As SqlCommand = Crea_Comando("Materias_Get", CommandType.StoredProcedure, con)
            'Crea_Parametro(cmd, "@Id_Departamento", SqlDbType.Int, Id_Departamento)
            'Crea_Parametro(cmd, "@Tipo", SqlDbType.Int, Tipo)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Insumos_Guardara(ByVal SucursalId As Integer, ByVal Usuario_Solicita As Integer, ByVal Estacion_Solicita As String, ByVal Solicitudes As String, ByVal Observaciones As String, ByVal Tipo As Integer) As Integer
        Dim cnn As SqlConnection = Crea_ConexionSTD()
        Dim tr As SqlTransaction = crear_Trans(cnn)

        Try
            Dim cmd As SqlCommand = Crea_ComandoT(cnn, tr, CommandType.StoredProcedure, "Mat_Solicitudes_Create")
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Crea_Parametro(cmd, "@Usuario_Solicita", SqlDbType.Int, Usuario_Solicita)
            Crea_Parametro(cmd, "@Estacion_Solicita", SqlDbType.VarChar, Estacion_Solicita)
            Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, "A")
            Crea_Parametro(cmd, "@Observaciones", SqlDbType.VarChar, Observaciones)
            Crea_Parametro(cmd, "@Tipo", SqlDbType.Int, Tipo)
            Dim SolicitudID = EjecutarScalarT(cmd)

            If SolicitudID = 0 Then
                tr.Rollback()
                Return 0
            End If

            Dim Total As Integer = 0
            Dim Solicitud() As String = Solicitudes.Split(";")
            Dim Valores() As String
            cmd = Crea_ComandoT(cnn, tr, CommandType.StoredProcedure, "Mat_SolicitudesD_Create")
            For Elem As Integer = 0 To Solicitud.Length - 1
                cmd.Parameters.Clear()
                Valores = Solicitud(Elem).Split(",")
                '0    1        2         3
                'Id Clave Descripción Cantidad

                Crea_Parametro(cmd, "@Id_Solicitud", SqlDbType.Int, SolicitudID)
                Crea_Parametro(cmd, "@Id_Consumible", SqlDbType.Int, Valores(0))
                Crea_Parametro(cmd, "@Cantidad_Solicitada", SqlDbType.Int, Valores(3))
                Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, "A")
                If EjecutarNonQueryT(cmd) > 0 Then Total += 1
            Next

            If Solicitud.Length = Total Then
                tr.Commit()
                Return SolicitudID
            Else
                tr.Rollback()
                Return 0
            End If
        Catch ex As Exception
            tr.Rollback()
            TrataEx(ex)
            Return 0
        End Try

    End Function

    Public Shared Function fn_Insumos_ObtenerConsumibles(ByVal Id_Departamento As Integer, ByVal Tipo As Integer, ByVal Status As String, ByVal SucursalID As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim con As SqlConnection = Crea_ConexionSTD()
        Try
            Dim cmd As SqlCommand = Crea_Comando("Mat_Consumibles_GetIntranet", CommandType.StoredProcedure, con)
            Crea_Parametro(cmd, "@Id_Departamento", SqlDbType.Int, Id_Departamento)
            Crea_Parametro(cmd, "@Tipo", SqlDbType.Int, Tipo)
            Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, Status)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Insumos_Guardar(ByVal SucursalId As Integer, ByVal Usuario_Solicita As Integer, ByVal Estacion_Solicita As String, ByVal Solicitudes As String, ByVal Observaciones As String, ByVal Tipo As Integer) As Integer
        Dim cnn As SqlConnection = Crea_ConexionSTD()
        Dim tr As SqlTransaction = crear_Trans(cnn)

        Try
            Dim cmd As SqlCommand = Crea_ComandoT(cnn, tr, CommandType.StoredProcedure, "Mat_Solicitudes_Create")
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Crea_Parametro(cmd, "@Usuario_Solicita", SqlDbType.Int, Usuario_Solicita)
            Crea_Parametro(cmd, "@Estacion_Solicita", SqlDbType.VarChar, Estacion_Solicita)
            Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, "A")
            Crea_Parametro(cmd, "@Observaciones", SqlDbType.VarChar, Observaciones)
            Crea_Parametro(cmd, "@Tipo", SqlDbType.Int, Tipo)
            Dim SolicitudID = EjecutarScalarT(cmd)

            If SolicitudID = 0 Then
                tr.Rollback()
                Return 0
            End If

            Dim Total As Integer = 0
            Dim Solicitud() As String = Solicitudes.Split(";")
            Dim Valores() As String
            cmd = Crea_ComandoT(cnn, tr, CommandType.StoredProcedure, "Mat_SolicitudesD_Create")
            For Elem As Integer = 0 To Solicitud.Length - 1
                cmd.Parameters.Clear()
                Valores = Solicitud(Elem).Split(",")
                '0    1        2         3
                'Id Clave Descripción Cantidad

                Crea_Parametro(cmd, "@Id_Solicitud", SqlDbType.Int, SolicitudID)
                Crea_Parametro(cmd, "@Id_Consumible", SqlDbType.Int, Valores(0))
                Crea_Parametro(cmd, "@Cantidad_Solicitada", SqlDbType.Int, Valores(3))
                Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, "A")
                If EjecutarNonQueryT(cmd) > 0 Then Total += 1
            Next

            If Solicitud.Length = Total Then
                tr.Commit()
                Return SolicitudID
            Else
                tr.Rollback()
                Return 0
            End If
        Catch ex As Exception
            tr.Rollback()
            TrataEx(ex)
            Return 0
        End Try

    End Function

    Public Shared Function fn_Insumos_ObtenerSolicitudes(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Fecha_Desde As Date, ByVal Fecha_Hasta As Date, ByVal Status As Char) As DataTable
        Dim con As SqlConnection = Crea_ConexionSTD()
        Try
            Dim cmd As SqlCommand = Crea_Comando("Mat_Solicitudes_Get", CommandType.StoredProcedure, con)
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalID)
            Crea_Parametro(cmd, "@Fecha_Desde", SqlDbType.Date, Fecha_Desde)
            Crea_Parametro(cmd, "@Fecha_Hasta", SqlDbType.Date, Fecha_Hasta)
            Crea_Parametro(cmd, "@Usuario_Solicita", SqlDbType.Int, UsuarioID)
            Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, Status)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Insumos_ObtenerSolicitudesDetalle(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Solicitud As Integer) As DataTable
        Try
            Dim cnn As SqlConnection = Crea_ConexionSTD()
            Dim cmd As SqlCommand = Crea_Comando("Mat_SolicitudesD_Get", CommandType.StoredProcedure, cnn)
            Crea_Parametro(cmd, "@Id_Solicitud", SqlDbType.Int, Id_Solicitud)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Insumos_LeerDatosSolicitud(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Solicitud As Integer) As DataRow
        Try
            Dim cnn As SqlConnection = Crea_ConexionSTD()
            Dim cmd As SqlCommand = Crea_Comando("Mat_Solicitudes_Read", CommandType.StoredProcedure, cnn)
            Crea_Parametro(cmd, "@Id_Solicitud", SqlDbType.Int, Id_Solicitud)
            Dim dt As DataTable = EjecutaConsulta(cmd)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Return dt.Rows(0)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Insumos_LeerDetalleSolicitud(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Solicitud As Integer) As DataTable
        Try
            Dim cnn As SqlConnection = Crea_ConexionSTD()
            Dim cmd As SqlCommand = Crea_Comando("Mat_SolicitudesD_Read", CommandType.StoredProcedure, cnn)
            Crea_Parametro(cmd, "@Id_Solicitud", SqlDbType.Int, Id_Solicitud)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Insumos_Read(ByVal Id_Solicitud) As String
        Try
            Dim cnn As SqlConnection = Crea_ConexionSTD()
            Dim cmd As SqlCommand = Crea_Comando("Mat_Solicitudes_Read", CommandType.StoredProcedure, cnn)
            Crea_Parametro(cmd, "@Id_Solicitud", SqlDbType.Int, Id_Solicitud)
            Dim dt As DataTable = EjecutaConsulta(cmd)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Return dt.Rows(0)("Observaciones")
            Else
                Return ""
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return ""
        End Try
    End Function

    Public Shared Function fn_Insumos_Cancelar(ByVal Id_Solicitud As Integer, ByVal SucursalID As Integer, ByVal UsuarioID As Integer, _
                                               ByVal EstacioN As String) As Boolean

        Try
            Dim cnn As SqlConnection = Crea_ConexionSTD()
            Dim cmd As SqlCommand = Crea_Comando("Mat_Solicitudes_Cancelar", CommandType.StoredProcedure, cnn)
            Crea_Parametro(cmd, "@Id_Solicitud", SqlDbType.Int, Id_Solicitud)
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalID)
            Crea_Parametro(cmd, "@Usuario_Cancela", SqlDbType.Int, UsuarioID)
            Crea_Parametro(cmd, "@Estacion_Cancela", SqlDbType.VarChar, EstacioN)
            Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, "C")

            If EjecutarNonQuery(cmd) > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try

    End Function

#End Region

#Region "Validación de Cartas de Acceso"

    Public Shared Function fn_CartasAccesoValidar_LlenarLista(ByVal SucursalID As Integer, ByVal UsuarioID As Integer) As DataTable
        Try
            Dim Cnn As SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlCommand = Crea_Comando("Cat_CartasAcceso_GetPendientes", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Status", SqlDbType.VarChar, "A")

            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_CartasAccesoValidar_LlenarDetalle(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Carta As Integer) As DataTable
        Try
            Dim Cnn As SqlClient.SqlConnection = Cn_Datos.Crea_ConexionSTD
            Dim Cmd As SqlClient.SqlCommand = Cn_Datos.Crea_Comando("Cat_CartasAccesoD_Get", CommandType.StoredProcedure, Cnn)
            Cn_Datos.Crea_Parametro(Cmd, "@Id_Carta", SqlDbType.Int, Id_Carta)

            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_CartasAccesoValidar_Validar(ByVal Id_Carta As Integer, ByVal Usuario_Valida As Integer, ByVal Estacion_Valida As String, ByVal Observaciones_Valida As String, ByVal Id_Sucursal As Integer, ByVal UsuarioID As Integer) As Boolean
        Try
            Dim cmd As SqlCommand = Crea_Comando("Cat_CartasAcceso_Validar", CommandType.StoredProcedure, Crea_ConexionSTD)
            Crea_Parametro(cmd, "@Id_Carta", SqlDbType.Int, Id_Carta)
            Crea_Parametro(cmd, "@Usuario_Valida", SqlDbType.Int, Usuario_Valida)
            Crea_Parametro(cmd, "@Estacion_Valida", SqlDbType.VarChar, Estacion_Valida)
            Crea_Parametro(cmd, "@Observaciones_Valida", SqlDbType.VarChar, Observaciones_Valida)

            Return EjecutarNonQuery(cmd) > 0
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function


    Public Shared Function fn_CartasAccesoConsulta_UsuarioAutoriza(ByVal Id_Empleado As Integer) As DataRow
        Try
            Dim Cnn As SqlClient.SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlClient.SqlCommand = Crea_Comando("Cat_Empleados_Read", CommandType.StoredProcedure, Cnn)

            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)

            Dim dt As DataTable = EjecutaConsulta(Cmd)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Return dt.Rows(0)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Generar Cartas de Acceso"

    Public Shared Function fn_GenerarCartasAcceso_LlenarComboEmpleados(ByVal SucursalId As Integer, ByVal Usuario_Registro As Integer) As DataTable
        Dim cmd As SqlCommand = Crea_Comando("Cat_EmpleadosCombo_Get", CommandType.StoredProcedure, Crea_ConexionSTD)
        Try
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_GenerarCartasAcceso_Nuevo(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal EstacioN As String, ByVal UsuariosAutorizados As String, ByVal Observaciones As String, ByVal UsuarioAutoriza As Integer, ByVal FechaInicio As Date, ByVal FechaFin As Date, ByVal Tipo As String, ByVal EmpleadoDestino As Integer, ByVal EmpresaVisitante As String) As Boolean
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim trans As SqlTransaction = crear_Trans(Cnn)
        Dim Id_Carta As Integer = 0

        Try
            Dim Cmd As SqlCommand = Crea_ComandoT(Cnn, trans, CommandType.StoredProcedure, "Cat_CartasAcceso_Create")
            Crea_Parametro(Cmd, "@UsuarioRegistro", SqlDbType.Int, UsuarioID)
            Crea_Parametro(Cmd, "@EstacionRegistro", SqlDbType.VarChar, EstacioN)
            Crea_Parametro(Cmd, "@Observaciones", SqlDbType.VarChar, Observaciones)
            Crea_Parametro(Cmd, "@UsuarioAutoriza", SqlDbType.Int, UsuarioAutoriza)
            Crea_Parametro(Cmd, "@FechaInicio", SqlDbType.Date, FechaInicio)
            Crea_Parametro(Cmd, "@FechaFin", SqlDbType.Date, FechaFin)
            Crea_Parametro(Cmd, "@Tipo", SqlDbType.Int, Tipo)
            Crea_Parametro(Cmd, "@Empleado_Destino", SqlDbType.Int, EmpleadoDestino)
            Id_Carta = EjecutarScalarT(Cmd)

            If Id_Carta = 0 Then
                trans.Rollback()
                Return False
            End If

            Dim Personal() As String = Split(UsuariosAutorizados, ";")
            Dim campos() As String
            For Each elemento As String In Personal
                Cmd.Parameters.Clear()
                campos = Split(elemento, ",")
                Cmd = Crea_ComandoT(Cnn, trans, CommandType.StoredProcedure, "Cat_CartasAccesoD_Create")
                Crea_Parametro(Cmd, "@Id_Carta", SqlDbType.Int, Id_Carta)
                Crea_Parametro(Cmd, "@NombreVisitante", SqlDbType.VarChar, campos(1))
                Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, campos(0))
                Crea_Parametro(Cmd, "@EmpresaVisitante", SqlDbType.VarChar, EmpresaVisitante)
                If EjecutarNonQueryT(Cmd) = 0 Then
                    trans.Rollback()
                    Return False
                End If
            Next

        Catch ex As Exception
            TrataEx(ex)
            trans.Rollback()
            Return False
        End Try
        trans.Commit()
        Return True
    End Function

#End Region

#Region "Jornadas"
    Public Shared Function fn_Jornadas_ObtenerDepartamentos(ByVal DepartamentoID As Integer, ByVal SucursalId As Integer, ByVal UsuarioID As Integer) As DataTable
        Try
            Dim cmd As SqlCommand
            cmd = Crea_Comando("Cat_DepartamentosCombo_Get", CommandType.StoredProcedure, Crea_ConexionSTD)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Jornadas_ObtenerPuestos(ByVal DepartamentoID As Integer, ByVal SucursalId As Integer, ByVal UsuarioID As Integer) As DataTable
        Try
            Dim cmd As SqlCommand
            cmd = Crea_Comando("Cat_PuestosCombo_Get", CommandType.StoredProcedure, Crea_ConexionSTD)
            Crea_Parametro(cmd, "@Id_Departamento", SqlDbType.Int, DepartamentoID)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Jornadas_GetEmpleados(ByVal Id_Sucursal As Integer, ByVal Id_Departamento As Integer, ByVal NuevoIngreso As Char, ByVal UsuarioID As Integer, ByVal Id_Puesto As Integer) As DataTable
        Dim cmd As SqlCommand = Crea_Comando("Cat_Empleados_Get", CommandType.StoredProcedure, Crea_ConexionSTD)
        Try
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, Id_Sucursal)
            Crea_Parametro(cmd, "@Pista", SqlDbType.VarChar, "")
            Crea_Parametro(cmd, "@Id_Departamento", SqlDbType.Int, Id_Departamento)
            Crea_Parametro(cmd, "@Id_Puesto", SqlDbType.Int, Id_Puesto)
            Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, "A")
            Crea_Parametro(cmd, "@NuevoIngreso", SqlDbType.VarChar, NuevoIngreso)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Jornadas_ObtenerEmpleados(ByVal SucursalId As Integer, ByVal Id_Departamento As Integer, ByVal Id_Puesto As Integer, ByVal UsuarioID As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = Crea_Comando("Cat_Empleados_GetJornadas", CommandType.StoredProcedure, Crea_ConexionSTD)
            Crea_Parametro(cmd, "@Id_Departamento", SqlDbType.Int, Id_Departamento)
            Crea_Parametro(cmd, "@Id_Puesto", SqlDbType.Int, Id_Puesto)
            Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, 0)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Jornadas_ConsultaJornadas(ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = Crea_Comando("Cat_EmpleadosJornadas_GetRegFaltas", CommandType.StoredProcedure, Crea_ConexionSTD)
            Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.BigInt, Id_Empleado)
            Crea_Parametro(cmd, "@FInicio", SqlDbType.Date, DateAdd(DateInterval.Day, -5, Now.Date).ToShortDateString)
            Crea_Parametro(cmd, "@FFin", SqlDbType.Date, DateAdd(DateInterval.Day, 30, Now.Date).ToShortDateString)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Jornadas_LlenarListaPlantillas(ByVal SucursalId As Integer, ByVal UsuarioID As Integer) As DataTable
        Try
            Dim Cnn As SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlCommand = Crea_Comando("Cat_Jornadas_Get", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Status", SqlDbType.VarChar, "A")

            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Jornadas_PlantillasDetalle(ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Id_Jornada As Integer) As DataTable
        Try
            Dim Cnn As SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlCommand = Crea_Comando("Cat_JornadasD_Get", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_Jornada", SqlDbType.VarChar, Id_Jornada)

            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Jornadas_Eliminar(ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Dias() As String) As Boolean
        Dim cnn As SqlConnection = Crea_ConexionSTD()
        Dim trans As SqlTransaction = crear_Trans(cnn)
        Dim cmd As SqlCommand

        For Each D As Integer In Dias
            cmd = Crea_ComandoT(trans.Connection, trans, CommandType.StoredProcedure, "Cat_EmpleadosJornadas_Delete")
            Crea_Parametro(cmd, "@Id_Jornada", SqlDbType.BigInt, D)

            Try
                If EjecutarNonQueryT(cmd) = 0 Then
                    trans.Rollback()
                    Return False
                End If
            Catch ex As Exception
                trans.Rollback()
                TrataEx(ex)
                Return False
            End Try
        Next
        trans.Commit()
        Return True
    End Function

    Public Shared Function fn_Jornadas_EliminarXRango(ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer, ByVal Desde As Date, ByVal Hasta As Date) As Boolean
        Dim cnn As SqlConnection = Crea_ConexionSTD()
        Dim cmd As SqlCommand

        'Eliminar todos los registros del Empleado seleccionado en el rango de fechas definido
        Try
            cmd = Crea_Comando("Cat_EmpleadosJornadas_DeleteRango", CommandType.StoredProcedure, cnn)
            Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            Crea_Parametro(cmd, "@Desde", SqlDbType.Date, Desde)
            Crea_Parametro(cmd, "@Hasta", SqlDbType.Date, Hasta)

            EjecutarNonQuery(cmd)
            Return True
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_Jornadas_GuardarManual(ByVal Empleados() As String, ByVal Dias As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal Jornada1 As String, ByVal Jornada2 As String, ByVal Turno As Integer, ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Estacion As String) As Boolean
        Dim CantidadDias As Integer = DateDiff(DateInterval.Day, Desde, Hasta)
        Dim Fecha As Date
        Dim DiaSemana As String
        Dim Nuevo As Boolean = False
        Dim Dt_EmpleadoJornadas As DataTable = Nothing
        Dim FilasAfectadas As Integer = 0

        Dim ArrDia() As String
        Dim Dia As String
        Select Case Dias
            Case 8 'De L-V
                Dia = "2,3,4,5,6"
            Case 9 'De L-S
                Dia = "2,3,4,5,6,7"
            Case 10 'De L-D
                Dia = "1,2,3,4,5,6,7"
            Case 11 'S y D
                Dia = "1,7"
            Case Else
                Dia = Dias
        End Select
        ArrDia = Split(Dia, ",")

        Dim cmd As SqlCommand
        Dim Cnn As SqlConnection = Cn_Datos.Crea_ConexionSTD
        Dim Tr As SqlTransaction = Cn_Datos.crear_Trans(Cnn)
        Try
            For Each elemento As String In Empleados
                cmd = Crea_ComandoT(Cnn, Tr, CommandType.StoredProcedure, "Cat_EmpleadosJornadas_Read")
                Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, elemento)
                Crea_Parametro(cmd, "@Turno", SqlDbType.Int, Turno)
                Crea_Parametro(cmd, "@Desde", SqlDbType.Date, Desde)
                Crea_Parametro(cmd, "@Hasta", SqlDbType.Date, Hasta)
                Dt_EmpleadoJornadas = EjecutaConsultaT(cmd)

                For ILocal As Integer = 0 To CantidadDias
                    Fecha = DateAdd(DateInterval.Day, ILocal, Desde)
                    DiaSemana = Microsoft.VisualBasic.DatePart(DateInterval.Weekday, Fecha, FirstDayOfWeek.Sunday)

                    For e As Integer = 0 To (ArrDia.Length - 1)
                        If ArrDia(e) = DiaSemana Then
                            If Dt_EmpleadoJornadas IsNot Nothing AndAlso Dt_EmpleadoJornadas.Rows.Count > 0 Then
                                For Each dr_EmpleadoJ As DataRow In Dt_EmpleadoJornadas.Rows
                                    If Fecha = dr_EmpleadoJ("Fecha") And Turno = dr_EmpleadoJ("Turno") Then
                                        'Actualizar registros que coincidan
                                        cmd = Crea_ComandoT(Cnn, Tr, CommandType.StoredProcedure, "Cat_EmpleadosJornadas_Update")
                                        Crea_Parametro(cmd, "@Id_Jornada", SqlDbType.Int, dr_EmpleadoJ("Id_Jornada"))
                                        Crea_Parametro(cmd, "@Jornada1", SqlDbType.VarChar, Jornada1)
                                        Crea_Parametro(cmd, "@Jornada2", SqlDbType.VarChar, Jornada2)
                                        Crea_Parametro(cmd, "@Usuario_Registro", SqlDbType.Int, UsuarioID)
                                        Crea_Parametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, Estacion)
                                        EjecutarNonQueryT(cmd)
                                        FilasAfectadas += 1
                                        Nuevo = False
                                        Exit For
                                    Else
                                        Nuevo = True
                                    End If
                                Next
                            Else
                                Nuevo = True
                            End If
                            If Nuevo Then
                                'Insertar un nuevo registro
                                cmd = Crea_ComandoT(Cnn, Tr, CommandType.StoredProcedure, "Cat_EmpleadosJornadas_Create")
                                Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, elemento)
                                Crea_Parametro(cmd, "@Fecha", SqlDbType.Date, Fecha)
                                Crea_Parametro(cmd, "@Dia", SqlDbType.Int, DiaSemana)
                                Crea_Parametro(cmd, "@Jornada1", SqlDbType.VarChar, Jornada1)
                                Crea_Parametro(cmd, "@Jornada2", SqlDbType.VarChar, Jornada2)
                                Crea_Parametro(cmd, "@Usuario_Registro", SqlDbType.VarChar, UsuarioID)
                                Crea_Parametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, Estacion)
                                Crea_Parametro(cmd, "@Turno", SqlDbType.Int, Turno)
                                Cn_Datos.EjecutarNonQueryT(cmd)
                                FilasAfectadas += 1
                                Nuevo = False
                            End If
                            'Salir del FOR porque ya se modifico o inserto,
                            'y no se necesita revisar otro día
                            Exit For
                        End If
                    Next
                Next ILocal
            Next
        Catch ex As Exception
            Tr.Rollback()
            TrataEx(ex)
            Return False
        End Try
        Tr.Commit()
        Return True
    End Function

    Public Shared Function fn_Jornadas_GuardarXPlantilla(ByVal Empleados() As String, ByVal Desde As Date, ByVal Hasta As Date, ByVal Id_JornadaPlantilla As Integer, ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Estacion As String) As Boolean
        Dim CantidadDias As Integer = DateDiff(DateInterval.Day, Desde, Hasta)
        Dim Fecha As Date
        Dim DiaSemana As String
        Dim DosTurnos As Integer = 0
        Dim Dt_PlantillaJornada As DataTable = Nothing

        Dim cmd As SqlCommand
        Dim Cnn As SqlConnection = Cn_Datos.Crea_ConexionSTD
        Dim Tr As SqlTransaction = Cn_Datos.crear_Trans(Cnn)
        Try
            cmd = Crea_ComandoT(Cnn, Tr, CommandType.StoredProcedure, "Cat_JornadasD_Read")
            Crea_Parametro(cmd, "@Id_Jornada", SqlDbType.Int, Id_JornadaPlantilla)
            Dt_PlantillaJornada = EjecutaConsultaT(cmd)

            For Each Elemento As String In Empleados
                'Eliminar todos los registros en el rango de fechas
                cmd = Crea_ComandoT(Cnn, Tr, CommandType.StoredProcedure, "Cat_EmpleadosJornadas_DeleteRango")
                Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, Elemento)
                Crea_Parametro(cmd, "@Desde", SqlDbType.Date, Desde)
                Crea_Parametro(cmd, "@Hasta", SqlDbType.Date, Hasta)
                EjecutarNonQueryT(cmd)

                For ILocal As Integer = 0 To CantidadDias
                    Fecha = DateAdd(DateInterval.Day, ILocal, Desde)
                    DiaSemana = Microsoft.VisualBasic.DatePart(DateInterval.Weekday, Fecha, FirstDayOfWeek.Sunday)

                    For Each dr_Plantilla As DataRow In Dt_PlantillaJornada.Rows
                        If dr_Plantilla("Dia") = DiaSemana Then
                            'Insertar un nuevo registro
                            cmd = Crea_ComandoT(Cnn, Tr, CommandType.StoredProcedure, "Cat_EmpleadosJornadas_Create")
                            Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, Elemento)
                            Crea_Parametro(cmd, "@Fecha", SqlDbType.Date, Fecha)
                            Crea_Parametro(cmd, "@Dia", SqlDbType.Int, DiaSemana)
                            Crea_Parametro(cmd, "@Jornada1", SqlDbType.VarChar, dr_Plantilla("Jornada1a") & "/" & dr_Plantilla("Jornada1b"))
                            Crea_Parametro(cmd, "@Jornada2", SqlDbType.VarChar, dr_Plantilla("Jornada2a") & "/" & dr_Plantilla("Jornada2b"))
                            Crea_Parametro(cmd, "@Usuario_Registro", SqlDbType.VarChar, UsuarioID)
                            Crea_Parametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, Estacion)
                            Crea_Parametro(cmd, "@Turno", SqlDbType.Int, dr_Plantilla("Turno"))
                            Cn_Datos.EjecutarNonQueryT(cmd)
                            DosTurnos += 1
                            If DosTurnos = 2 Then DosTurnos = 0 : Exit For
                        End If
                    Next
                Next
            Next
        Catch ex As Exception
            Tr.Rollback()
            TrataEx(ex)
            Return False
        End Try
        Tr.Commit()
        Return True
    End Function

    Public Shared Function fn_Jornadas_Iguales(ByVal EmpleadosSeleccionados() As String, ByVal Turno As Integer, ByVal Desde As Date, ByVal Hasta As Date, ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Estacion As String) As Boolean
        Dim Cnn As SqlConnection = Cn_Datos.Crea_ConexionSTD
        Dim cmd As SqlCommand
        Dim DT As DataTable = Nothing
        Try
            For Each elemento As String In EmpleadosSeleccionados
                cmd = Crea_Comando("Cat_EmpleadosJornadas_Read", CommandType.StoredProcedure, Cnn)
                Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, elemento)
                Crea_Parametro(cmd, "@Turno", SqlDbType.Int, Turno)
                Crea_Parametro(cmd, "@Desde", SqlDbType.Date, Desde)
                Crea_Parametro(cmd, "@Hasta", SqlDbType.Date, Hasta)
                DT = EjecutaConsulta(cmd)
                If DT.Rows.Count > 0 Then
                    Return True
                End If
            Next
            Return False
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

#End Region

#Region "Registro de Faltas"
    Public Shared Function fn_Faltas_ObtenerEmpleados(ByVal SucursalId As Integer, ByVal Id_Departamento As Integer, ByVal UsuarioID As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = Crea_Comando("Cat_Empleados_ComboGetByDepto", CommandType.StoredProcedure, Crea_ConexionSTD)
            Crea_Parametro(cmd, "@Id_Departamento", SqlDbType.Int, Id_Departamento)
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Faltas_Guardar(ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Estacion As String, ByVal Faltas() As String) As Boolean
        Dim campos() As String
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Tr As SqlTransaction = crear_Trans(Cnn)
        Dim cmd As SqlCommand = Crea_ComandoT(Cnn, Tr, CommandType.StoredProcedure, "Cat_EmpleadosJornadas_Faltas")
        Try
            For Each elemento As String In Faltas
                campos = Split(elemento, ",")
                cmd.Parameters.Clear()
                Crea_Parametro(cmd, "@Id_Jornada", SqlDbType.Int, campos(0))
                Crea_Parametro(cmd, "@Tipo_Falta", SqlDbType.Int, CInt(campos(1)))
                Crea_Parametro(cmd, "@Usuario_RegistroFalta", SqlDbType.VarChar, UsuarioID)
                Crea_Parametro(cmd, "@Estacion_RegistroFalta", SqlDbType.VarChar, Estacion)
                EjecutarNonQueryT(cmd)
            Next
            Tr.Commit()
            Return True
        Catch ex As Exception
            Tr.Rollback()
            TrataEx(ex)
            Return False
        End Try
    End Function

#End Region

#Region "Registrar Asistencias"

    Public Shared Function fn_RegistroAsistencia_Buscar(ByVal Clave As String, ByVal Fecha_Asistencia As Date, ByVal SucursalId As Integer, ByVal UsuarioID As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = Crea_Comando("Cat_Empleados_JornadasAsistencias", CommandType.StoredProcedure, Crea_ConexionSTD)
            Crea_Parametro(cmd, "@Clave", SqlDbType.VarChar, Clave)
            Crea_Parametro(cmd, "@Fecha", SqlDbType.Date, Fecha_Asistencia)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_RegistrarAsistencia_LlenarListaFaltas(ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = Crea_Comando("Cat_EmpleadosJornadas_GetFaltas", CommandType.StoredProcedure, Crea_ConexionSTD)
            Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            Crea_Parametro(cmd, "@Desde", SqlDbType.Date, DateAdd(DateInterval.Day, -45, Now.Date))
            Crea_Parametro(cmd, "@Hasta", SqlDbType.Date, Now.Date)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_RegistroAsistencia_Guardar(ByVal Dt_Guardia As DataTable, ByVal Tipo_Falta As Integer, ByVal Cantidad_HorasE As Decimal, _
                                                         ByVal Observaciones As String, ByVal Minutos_Retardo As Integer, ByVal Id_JornadaRecupera As Integer, ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Estacion As String) As Boolean

        Dim cnn As SqlConnection = Crea_ConexionSTD()
        Dim tr As SqlTransaction = crear_Trans(cnn)
        Try
            Dim cmd As SqlCommand = Crea_ComandoT(cnn, tr, CommandType.StoredProcedure, "Cat_EmpleadosJornadas_Asistencia")
            Crea_Parametro(cmd, "@Id_Guardia", SqlDbType.Int, Dt_Guardia.Rows(0)("Id_Empleado"))
            Crea_Parametro(cmd, "@Id_EmpleadoJornada", SqlDbType.Int, Dt_Guardia.Rows(0)("Id_EmpleadoJornada"))

            'Si se tenía guardado un tipo de falta distinto a una asistencia normal (0) se mandará el valor que se tenía
            'si no, se manda la asistencia (0) o falta (1)
            If Dt_Guardia.Rows(0)("Tipo_Falta") <> 0 And Dt_Guardia.Rows(0)("Tipo_Falta") <> 1 Then
                Crea_Parametro(cmd, "@Tipo_Falta", SqlDbType.TinyInt, Dt_Guardia.Rows(0)("Tipo_Falta"))
            Else
                Crea_Parametro(cmd, "@Tipo_Falta", SqlDbType.TinyInt, Tipo_Falta)
            End If

            'Si el tipo de Falta es 1 significa es una Falta y por eso se mandará el registro para la falta
            'además si Asistió o no
            If Tipo_Falta = 1 Then
                Crea_Parametro(cmd, "@Usuario_RegistroFalta", SqlDbType.Int, UsuarioID)
                Crea_Parametro(cmd, "@Estacion_RegistroFalta", SqlDbType.VarChar, Estacion)
                Crea_Parametro(cmd, "@Asistio", SqlDbType.VarChar, "N")
            Else
                Crea_Parametro(cmd, "@Asistio", SqlDbType.VarChar, "S")
            End If

            'Se mandarán las horas extras trabajadas cuando se haya capturado un valor en la ventana de Asistencia
            If Cantidad_HorasE > 0 Then Crea_Parametro(cmd, "@Cantidad_HorasE", SqlDbType.Decimal, Cantidad_HorasE)

            'Cuando no sea un día Festivo vendrá el valor N sino el nombre del día festivo
            If Dt_Guardia.Rows(0)("Festivo") = "N" Then
                Crea_Parametro(cmd, "@Id_TarifaHora", SqlDbType.Int, Dt_Guardia.Rows(0)("Tarifa_HoraNormal"))
                Crea_Parametro(cmd, "@Precio_HorasE", SqlDbType.Money, Dt_Guardia.Rows(0)("Tarifa_HoraNormalMoney"))
                Crea_Parametro(cmd, "@Festivo", SqlDbType.VarChar, "N")
            Else
                Crea_Parametro(cmd, "@Id_TarifaHora", SqlDbType.Int, Dt_Guardia.Rows(0)("Tarifa_HoraFestiva"))
                Crea_Parametro(cmd, "@Precio_HorasE", SqlDbType.Money, Dt_Guardia.Rows(0)("Tarifa_HoraFestivaMoney"))
                Crea_Parametro(cmd, "@Festivo", SqlDbType.VarChar, "S")
            End If

            Crea_Parametro(cmd, "@Observaciones", SqlDbType.VarChar, Observaciones)

            'Se mandan los minutos que llego tarde si se escribio un valor en la ventana de Asistencia
            If Minutos_Retardo > 0 Then Crea_Parametro(cmd, "@Minutos_Retardo", SqlDbType.Int, Minutos_Retardo)

            Dim JornadaModificada As Short = EjecutarNonQueryT(cmd)

            'Cuando se mande un Id_JornadaRecuperar significa que se esta recuperando una falta y el Id_Jornada es la Jornada de la Falta
            Dim FaltaModificada As Short = 0
            If Id_JornadaRecupera > 0 Then
                'Al Día que tenía Falta se le agregará el Id_Jornada de la jornada que se esta trabajando
                'con esto se sabrá que se recupera una falta
                cmd = Crea_ComandoT(cnn, tr, CommandType.StoredProcedure, "Cat_EmpleadosJornadas_UpdateRecuperar")
                Crea_Parametro(cmd, "@Id_EmpleadoJornada", SqlDbType.Int, Id_JornadaRecupera) 'La Jornada que se le agregará en el campo de Id_JornadaRecupera
                Crea_Parametro(cmd, "@Id_JornadaRecupera", SqlDbType.Int, Dt_Guardia.Rows(0)("Id_EmpleadoJornada")) 'La Jornada que se trabaja se mandará para el campo Id_JornadaRecupera

                FaltaModificada = EjecutarNonQueryT(cmd)
            End If

            If JornadaModificada > 0 AndAlso Id_JornadaRecupera > 0 AndAlso FaltaModificada > 0 Then
                tr.Commit()
                cnn.Close()
                Return True
            ElseIf JornadaModificada > 0 AndAlso Id_JornadaRecupera = 0 Then
                tr.Commit()
                cnn.Close()
                Return True
            Else
                tr.Rollback()
                cnn.Close()
                Return False
            End If
        Catch ex As Exception
            tr.Rollback()
            cnn.Close()
            TrataEx(ex)
            Return False
        End Try
    End Function
#End Region

#Region "Consulta de Jornadas"

    ''' <summary>
    ''' Llena la lista de Jornadas
    ''' </summary>
    ''' <param name="Id_Departamento">Es el ID del Departamento del Usuario firmado</param>
    ''' <returns>Devuelve un DataTable con los datos de la consulta</returns>
    ''' <remarks>La consulta esta filtrada por Departamento, Empleado(s), Rango de Fechas y Tipo de Falta</remarks>
    Public Shared Function fn_JornadasConsulta_LlenarLista(ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Id_Departamento As Integer, ByVal Id_Empleado As Integer, ByVal FInicio As String, ByVal FFin As String, ByVal TipoFalta As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = Crea_Comando("Cat_EmpleadosJornadas_Get", CommandType.StoredProcedure, Crea_ConexionSTD)
            Crea_Parametro(cmd, "@Id_Departamento", SqlDbType.Int, Id_Departamento)
            Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            Crea_Parametro(cmd, "@FInicio", SqlDbType.Date, FInicio)
            Crea_Parametro(cmd, "@FFin", SqlDbType.Date, FFin)
            Crea_Parametro(cmd, "@Tipo_Falta", SqlDbType.Int, TipoFalta)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_HorasChecadas_ObtenerEmpleados(ByVal SucursalId As Integer, ByVal Id_Departamento As Integer, ByVal UsuarioID As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = Crea_Comando("Cat_Empleados_ComboGetByDepto", CommandType.StoredProcedure, Crea_ConexionSTD)
            Crea_Parametro(cmd, "@Id_Departamento", SqlDbType.Int, Id_Departamento)
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
    'Horas checadas
    Public Shared Function fn_HorasChecadas_LlenarLista(ByVal Id_Departamento As Integer, ByVal Id_Empleado As Integer, ByVal FechaInicio As String, ByVal FechaFin As String) As DataTable
        Try
            Dim cmd As SqlCommand = Crea_Comando("Cat_RelojesES_GetHoraChecada", CommandType.StoredProcedure, Crea_ConexionSTD)
            Crea_Parametro(cmd, "@Id_Departamento", SqlDbType.Int, Id_Departamento)
            Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            Crea_Parametro(cmd, "@FechaInicio", SqlDbType.Date, FechaInicio) 'stab VArchar 27jun14
            Crea_Parametro(cmd, "@FechaFin", SqlDbType.Date, FechaFin)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
#End Region

#Region "Registro de Permisos"
    Public Shared Function fn_PermisosRegistro_ObtenerJornada(ByVal Id_Empleado As Integer, ByVal Fecha As Date, ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Estacion As String) As DataRow
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Try
            Dim cmd As SqlCommand = Crea_Comando("Cat_EmpleadosJornadas_GetUnaJornada", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            Crea_Parametro(cmd, "@Fecha", SqlDbType.Date, Fecha)
            Dim dt As DataTable = EjecutaConsulta(cmd)

            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_PermisosRegistro_Guardar(ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Estacion As String, ByVal Id_Empleado As Integer, ByVal Fecha_Incidencia As Date, ByVal Tipo_Incidencia As Integer, ByVal Descripcion As String, ByVal Motivo As String, ByVal Observaciones As String) As Boolean
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Try
            Dim cmd As SqlCommand = Crea_Comando("Cat_EmpleadosIncidencias_Create", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            Crea_Parametro(cmd, "@Fecha_Incidencia", SqlDbType.Date, Fecha_Incidencia)
            Crea_Parametro(cmd, "@Tipo_Incidencia", SqlDbType.Int, Tipo_Incidencia)
            Crea_Parametro(cmd, "@Descripcion", SqlDbType.Text, Descripcion)
            Crea_Parametro(cmd, "@Motivo", SqlDbType.Text, Motivo)
            Crea_Parametro(cmd, "@Observaciones", SqlDbType.Text, Observaciones)
            Crea_Parametro(cmd, "@Usuario_Registro", SqlDbType.Int, UsuarioID)
            Crea_Parametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, Estacion)
            Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, "A")
            EjecutarNonQuery(cmd)
            Return True
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

#End Region

#Region "Registro de Usuarios"

    Public Shared Function fn_Usuarios_Consultar(ByVal SucursalId As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Usr_Usuarios_Get", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Dim dt As DataTable = EjecutaConsulta(Cmd)
            Return dt
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Usuarios_ConsultarEmpleados(ByVal SucursalId As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Cat_EmpleadosNoUsuarios_Get", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Dim dt As DataTable = EjecutaConsulta(Cmd)
            Return dt
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_Usuarios_EliminarHoras(ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer) As Boolean
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Usr_UsuariosHorasId_Delete", CommandType.StoredProcedure, Cnn)

        Try
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            EjecutarNonQuery(Cmd)
            Return True
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_Usuarios_EliminarPrivilegios(ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer) As Boolean
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Usr_UsuariosPrivilegios_Delete", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            EjecutarNonQuery(Cmd)
            Return True
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_Usuarios_Eliminar(ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer) As Integer
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Usr_Usuarios_Delete", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            EjecutarNonQuery(Cmd)
            Return True
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_UsuariosContra_Reiniciar(ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer, ByVal Password As String) As Integer
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Usr_UsuariosContra_Restart", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            Crea_Parametro(Cmd, "@Password", SqlDbType.VarChar, Password)
            Return EjecutarNonQuery(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_Usuarios_Bloquear(ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer, ByVal Status As String) As Integer
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Usr_Usuarios_Status", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            Crea_Parametro(Cmd, "@Status", SqlDbType.VarChar, Status)

            Return EjecutarNonQuery(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try

    End Function

    Public Shared Function fn_Usuarios_ClaveExpira(ByVal Id_Empleado As Integer, ByVal ClaveExpira As String) As Integer
        '19/12/2014 new
        Dim Cnn As SqlClient.SqlConnection = Cn_Datos.Crea_ConexionSTD
        Dim Cmd As SqlClient.SqlCommand = Cn_Datos.Crea_Comando("Usr_Usuarios_UpdateClaveEx", CommandType.StoredProcedure, Cnn)
        Cn_Datos.Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
        Cn_Datos.Crea_Parametro(Cmd, "@Clave_Expira", SqlDbType.VarChar, ClaveExpira)
        Try
            Return Cn_Datos.EjecutarNonQuery(Cmd)

        Catch ex As Exception
            TrataEx(ex)
            Return -1
        End Try

    End Function

    Public Shared Function fn_Usuarios_Agregar(ByVal SucursalId As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer, ByVal Password As String, ByVal Tipo As Byte, ClaveExpira As Char) As Integer
        Dim Cnn As SqlConnection = Crea_ConexionSTD() 'modif 19/12/2014
        Dim Cmd As SqlCommand = Crea_Comando("Usr_Usuarios_Create", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            Crea_Parametro(Cmd, "@Password", SqlDbType.VarChar, Password)
            Crea_Parametro(Cmd, "@Tipo", SqlDbType.Int, Tipo)
            Crea_Parametro(Cmd, "@Usuario_Registro", SqlDbType.Int, UsuarioID)
            Crea_Parametro(Cmd, "@Status", SqlDbType.VarChar, "A")
            Crea_Parametro(Cmd, "@Clave_Expira", SqlDbType.VarChar, ClaveExpira)
            Return EjecutarNonQuery(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return -1
        End Try

    End Function

#End Region

#Region "Privilegios de Usuarios"

    Public Shared Function fn_UsuariosPrivilegios_LlenarListaModulos(ByVal SucursalID As Integer, ByVal UsuarioID As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = Crea_Comando("Usr_Modulos_ComboGet", CommandType.StoredProcedure, Crea_ConexionSTD)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_UsuariosPrivilegios_ObtenerMenus(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Clave_Modulo As String) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Usr_MenusModulo_Get", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Clave_Modulo", SqlDbType.VarChar, Clave_Modulo)
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_UsuariosPrivilegios_ObtenerOpciones(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Menu As Integer) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Usr_OpcionesMenu_Get", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Id_Menu", SqlDbType.VarChar, Id_Menu)
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_UsuarioPrivilegios_ObtenerControles(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Opcion As Integer) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Usr_OpcionesControles_Get", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Id_Opcion", SqlDbType.VarChar, Id_Opcion)
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_UsuariosPrivilegios_PrivilegiosOpciones(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer, ByVal Clave_Modulo As String) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Usr_Permisos_Get", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            Crea_Parametro(Cmd, "@Clave_Modulo", SqlDbType.VarChar, Clave_Modulo)

            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_UsuariosPrivilegios_PrivilegiosControles(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Usuario As Integer, ByVal Clave_Modulo As String) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Usr_PermisosControles_Get", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Usuario)
            Crea_Parametro(Cmd, "@Clave_Modulo", SqlDbType.VarChar, Clave_Modulo)

            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_UsuariosPrivilegios_Eliminar(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer, ByVal Id_Menu As Integer) As Integer
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Usr_PermisosMenu_Delete", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            Crea_Parametro(Cmd, "@Id_Menu", SqlDbType.Int, Id_Menu)

            Return EjecutarNonQuery(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_UsuariosPrivilegios_AgregarOpciones(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer, ByVal Opciones() As Integer) As Boolean
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim trans As SqlTransaction = crear_Trans(Cnn)
        Dim Cmd As SqlCommand = Crea_ComandoT(Cnn, trans, CommandType.StoredProcedure, "Usr_Permisos_Create")
        Try
            For Each Opcion As Integer In Opciones
                Cmd.Parameters.Clear()
                Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
                Crea_Parametro(Cmd, "@Id_Opcion", SqlDbType.Int, Opcion)

                EjecutarNonQueryT(Cmd)
            Next
            trans.Commit()
            Return True
        Catch ex As Exception
            trans.Rollback()
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_UsuariosPrivilegios_EliminarControles(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer, ByVal Id_Opcion As Integer) As Integer
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Usr_UsuariosPrivilegiosOpcion_Delete", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            Crea_Parametro(Cmd, "@Id_Opcion", SqlDbType.Int, Id_Opcion)

            Return EjecutarNonQuery(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_UsuariosPrivilegios_AgregarControles(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer, ByVal Controles() As Integer) As Boolean
        Dim Cnn As SqlClient.SqlConnection = Cn_Datos.Crea_ConexionSTD
        Dim trans As SqlTransaction = crear_Trans(Cnn)
        Dim Cmd As SqlCommand = Crea_ComandoT(Cnn, trans, CommandType.StoredProcedure, "Usr_PermisosControles_Create")
        Try
            For Each Control As Integer In Controles
                Cmd.Parameters.Clear()
                Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
                Crea_Parametro(Cmd, "@Id_Control", SqlDbType.Int, Control)

                EjecutarNonQueryT(Cmd)
            Next
            trans.Commit()
            Return True
        Catch ex As Exception
            trans.Rollback()
            TrataEx(ex)
            Return False
        End Try
    End Function

#End Region

#Region "Horarios de Acceso"

    Public Shared Function fn_UsuariosHoras_Consultar(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Usr_UsuariosHoras_Get", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_UsuariosHoras_Eliminar(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer) As Integer
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Usr_UsuariosHorasId_Delete", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            Return EjecutarNonQuery(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_UsuariosHoras_Agregar(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Empleado As Integer, ByVal Horarios() As String) As Integer
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim trans As SqlTransaction = crear_Trans(Cnn)
        Dim Cmd As SqlCommand

        Dim arrDiv() As String
        Try
            'Borrar todos los Horarios anteriores
            Cmd = Crea_ComandoT(Cnn, trans, CommandType.StoredProcedure, "Usr_UsuariosHorasId_Delete")
            Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            EjecutarNonQueryT(Cmd)

            Cmd.CommandText = "Usr_UsuariosHoras_Create"
            'Guardar los nuevos Horarios
            'Cada elemento del array contiene un string compuesto por el Día y la Hora (p.e. "1,8")
            For Each elemento As String In Horarios
                Cmd.Parameters.Clear()
                arrDiv = Split(elemento, ",")
                Crea_Parametro(Cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
                Crea_Parametro(Cmd, "@Dia", SqlDbType.Int, arrDiv(0))
                Crea_Parametro(Cmd, "@Hora", SqlDbType.Int, arrDiv(1))

                EjecutarNonQueryT(Cmd)
            Next

            trans.Commit()
            Return True
        Catch ex As Exception
            trans.Rollback()
            TrataEx(ex)
            Return False
        End Try
    End Function

#End Region

#Region "Recepción Custodias Temporales"

    Public Shared Function fn_CustodiasTemporales_ObtenerCias(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Cia As Integer) As DataTable
        Try
            Dim Cnn As SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlCommand = Crea_Comando("Cat_Cias_GetNoPropia", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_Cia", SqlDbType.Int, Id_Cia)
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_CustodiasTemporales_ObtenerBovedas(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Tipo_Boveda As Char, ByVal Status As Char) As DataTable



        Try
            Dim Cnn As SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlCommand = Crea_Comando("Cat_Bovedas_ComboGet", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalID)
            Crea_Parametro(Cmd, "@Tipo_Boveda", SqlDbType.VarChar, Tipo_Boveda)
            Crea_Parametro(Cmd, "@Status", SqlDbType.VarChar, Status)

            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_CustodiasTemporales_ObtenerClientes(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Cia As Integer, ByVal Status As Char) As DataTable
        Try
            Dim Cnn As SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlCommand = Crea_Comando("Cat_ClientesProcesoCia_Get", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalID)
            Crea_Parametro(Cmd, "@Id_Cia", SqlDbType.Int, Id_Cia)
            Crea_Parametro(Cmd, "@Status", SqlDbType.VarChar, Status)

            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_CustodiasTemporales_ObtenerCajasBancarias(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_ClienteP As Integer) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand

        Try
            Cmd = Crea_Comando("Cat_ClientesBancos_Read", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_ClienteP", SqlDbType.Int, Id_ClienteP)

            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_CustodiasTemporales_Monedas(ByVal SucursalID As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Cat_Monedas_GetTipoCambio", CommandType.StoredProcedure, Cnn)

        Try
            Dim Tbl As DataTable = EjecutaConsulta(Cmd)

            If Tbl Is Nothing OrElse Tbl.Rows.Count = 0 Then
                Return Nothing
            Else
                Tbl.Columns(2).ReadOnly = False
                Tbl.Columns(3).ReadOnly = False
                Return Tbl
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_CustodiasTemporales_Envases(ByVal SucursalID As Integer, ByVal UsuarioID As Integer) As DataTable
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("CAT_TipoEnvase_GET", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Pista", SqlDbType.VarChar, "")
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_CustodiasTemporales_ObtenerTipoCambio(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Moneda As Integer) As Decimal
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim Cmd As SqlCommand = Crea_Comando("Cat_TipoCambio_GetByMoneda", CommandType.StoredProcedure, Cnn)
        Try
            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalID)
            Crea_Parametro(Cmd, "@Id_Moneda", SqlDbType.Int, Id_Moneda)
            Return EjecutarScalar(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return 0
        End Try
    End Function

    Public Shared Function fn_CustodiasTemporales_ExisteRemision(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Numero_Remision As Int64, ByVal Id_Cia As Integer) As Boolean
        Try
            Dim Cnn As SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlCommand = Crea_Comando("Cat_Remisiones_Existe", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalID)
            Crea_Parametro(Cmd, "@Numero_Remision", SqlDbType.BigInt, Numero_Remision)
            Crea_Parametro(Cmd, "@Id_Cia", SqlDbType.Int, Id_Cia)
            Dim dt As DataTable
            dt = EjecutaConsulta(Cmd)
            If dt IsNot Nothing Then
                If dt.Rows.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_CustodiasTemporales_ObtenClienteDestino(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_CajaBancaria As Integer) As DataRow
        Try
            Dim Cnn As SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlCommand = Crea_Comando("Pro_CajasBancarias_Read", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_CajaBancaria", SqlDbType.Int, Id_CajaBancaria)

            Dim dt As DataTable = EjecutaConsulta(Cmd)
            If dt.Rows.Count = 0 Then
                Return Nothing
            Else
                Return dt.Rows(0)
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_CustodiasTemporales_Guardar(ByVal NumeroRemision As Long, ByVal EnvasesCN As Integer, ByVal EnvasesSN As Integer, _
                                                     ByVal Importe As Decimal, ByVal IdCia As Integer, ByVal BovedaID As Integer, _
                                                     ByVal ClienteP As Integer, ByVal CajaBancaria As Integer, ByVal Envases() As String, ByVal Monedas() As String, _
                                                     ByVal Proceso As Boolean, ByVal Id_ClienteD As Integer, ByVal Id_ClienteP As Integer, _
                                                     ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal MonedaId As Integer, ByVal EstacioN As String, ByVal TurnoId As Integer) As Boolean
        'Aqui se inserta un nuevo registro
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim trans As SqlTransaction
        Dim Cmd As SqlCommand
        Dim RemisionID As Integer
        Dim Rutas As Integer = 0
        Dim moneda() As String
        Dim envase() As String

        trans = crear_Trans(Cnn)
        'Insertar la Remision
        Cmd = Crea_ComandoT(Cnn, trans, CommandType.StoredProcedure, "CAT_Remisiones_Create")

        Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalID)
        Crea_Parametro(Cmd, "@Numero_Remision", SqlDbType.BigInt, NumeroRemision)
        Crea_Parametro(Cmd, "@Envases", SqlDbType.Int, EnvasesCN)
        Crea_Parametro(Cmd, "@EnvasesSN", SqlDbType.Int, EnvasesSN)
        Crea_Parametro(Cmd, "@Moneda_Local", SqlDbType.Int, MonedaId)
        Crea_Parametro(Cmd, "@ImporteTotal", SqlDbType.Money, Importe)
        Crea_Parametro(Cmd, "@Id_Cia", SqlDbType.Int, IdCia)
        Crea_Parametro(Cmd, "@Usuario", SqlDbType.Int, UsuarioID)
        Crea_Parametro(Cmd, "@Cliente_Destino", SqlDbType.Int, Id_ClienteD)
        Crea_Parametro(Cmd, "@Id_ClienteP", SqlDbType.Int, Id_ClienteP)
        Crea_Parametro(Cmd, "@Estacion_Registro", SqlDbType.VarChar, EstacioN)

        Try

            RemisionID = EjecutarScalarT(Cmd)


            'Insertar los importes por Moneda
            For c As Integer = 0 To Monedas.Length - 1
                moneda = Split(Monedas(c), ";")

                Cmd.Parameters.Clear()
                Cmd = Crea_ComandoT(Cnn, trans, CommandType.StoredProcedure, "CAT_RemisionesD_Create")

                Crea_Parametro(Cmd, "@Id_Remision", SqlDbType.Int, RemisionID)
                Crea_Parametro(Cmd, "@Id_Moneda", SqlDbType.Int, CInt(moneda(0)))
                Crea_Parametro(Cmd, "@Importe_Efectivo", SqlDbType.Money, CDec(moneda(1)))
                Crea_Parametro(Cmd, "@Importe_Documentos", SqlDbType.Money, CDec(moneda(2)))

                EjecutarNonQueryT(Cmd)

            Next

            'Insertar los Envases
            If Envases IsNot Nothing Then
                For e As Integer = 0 To Envases.Length - 1
                    envase = Split(Envases(e), ";")

                    Cmd.Parameters.Clear()
                    Cmd = Crea_ComandoT(Cnn, trans, CommandType.StoredProcedure, "CAT_Envases_Create")

                    Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalID)
                    Crea_Parametro(Cmd, "@Id_Remision", SqlDbType.Int, RemisionID)
                    Crea_Parametro(Cmd, "@Id_TipoE", SqlDbType.Int, CInt(envase(0)))
                    Crea_Parametro(Cmd, "@Numero", SqlDbType.VarChar, envase(1))
                    Crea_Parametro(Cmd, "@Usuario_Registro", SqlDbType.Int, UsuarioID)

                    EjecutarNonQueryT(Cmd)

                Next
            End If

            Cmd = Crea_ComandoT(Cnn, trans, CommandType.StoredProcedure, "Cat_RutasTipo_Get")

            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalID)
            Crea_Parametro(Cmd, "@Tipo", SqlDbType.Int, 3)
            Crea_Parametro(Cmd, "@Pista", SqlDbType.VarChar, "REXT")

            Rutas = Cn_Datos.EjecutarScalarT(Cmd)

            'Insertar en Bo_Boveda
            Dim Tipo, TipoP, EntidadO, EntidadD As Integer

            If Proceso Then Tipo = 2 : TipoP = 1 Else Tipo = 3 : TipoP = 0
            If Proceso Then EntidadO = ClienteP : EntidadD = 0 Else EntidadO = CajaBancaria : EntidadD = CajaBancaria

            Cmd = Crea_ComandoT(Cnn, trans, CommandType.StoredProcedure, "BO_Boveda_Create")
            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalID)
            Crea_Parametro(Cmd, "@Id_Boveda", SqlDbType.Int, BovedaID)
            Crea_Parametro(Cmd, "@Tipo", SqlDbType.Int, Tipo)
            Crea_Parametro(Cmd, "@Entidad_Origen", SqlDbType.Int, EntidadO)
            Crea_Parametro(Cmd, "@Entidad_Destino", SqlDbType.Int, EntidadD)
            Crea_Parametro(Cmd, "@TipoP", SqlDbType.Int, TipoP)
            Crea_Parametro(Cmd, "@Id_TurnoEntrada", SqlDbType.Int, TurnoId)
            Crea_Parametro(Cmd, "@Usuario_Registro", SqlDbType.Int, UsuarioID)
            Crea_Parametro(Cmd, "@Id_Remision", SqlDbType.Int, RemisionID)
            Crea_Parametro(Cmd, "@Ruta_Entrada", SqlDbType.Int, Rutas)
            Crea_Parametro(Cmd, "@Ruta_Salida", SqlDbType.Int, 0)
            Crea_Parametro(Cmd, "@Horario_Entrega", SqlDbType.VarChar, "")
            Crea_Parametro(Cmd, "@DotacionPro", SqlDbType.VarChar, "N")
            Crea_Parametro(Cmd, "@DotacionMorr", SqlDbType.VarChar, "N")
            Crea_Parametro(Cmd, "@DotacionATM", SqlDbType.VarChar, "N")
            Crea_Parametro(Cmd, "@Material", SqlDbType.VarChar, "N")
            Crea_Parametro(Cmd, "@ConcentracionATM", SqlDbType.VarChar, "N")
            Crea_Parametro(Cmd, "@CustodiaATM", SqlDbType.VarChar, "N")
            Crea_Parametro(Cmd, "@Dpto_Registro", SqlDbType.VarChar, "B")

            EjecutarNonQueryT(Cmd)

            trans.Commit()
            Cnn.Close()

            Return True
        Catch ex As Exception
            trans.Rollback()
            Cnn.Close()
            TrataEx(ex)
            Return False
        End Try
    End Function

#End Region

#Region "Consulta de Custodias Temporales"

    Public Shared Function fn_ConsultaCustTemp_DespachoRutas(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Cia As Integer) As DataTable
        Try
            Dim Cnn As SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlCommand = Crea_Comando("Bo_Boveda_GetProcesoSaltillo", CommandType.StoredProcedure, Cnn)

            Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalID)
            Crea_Parametro(Cmd, "@Id_Cia", SqlDbType.Int, Id_Cia)
            Crea_Parametro(Cmd, "@Tipo_Boveda", SqlDbType.VarChar, "P")

            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_ConsultaCustTemp_ObtenerRutas(ByVal SucursalID As Integer, ByVal UsuarioID As Integer) As DataTable
        Try
            Dim Cnn As SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlCommand = Crea_Comando("Tv_Puntos_IO_Saltillo", CommandType.StoredProcedure, Cnn)

            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function fn_ConsultaCustTemp_GuardarDespacho(ByVal Id_Punto As Integer, ByVal Remisiones() As String, ByVal Ruta_Salida As Integer, _
                                                               ByVal SucursalID As Integer, ByVal UsuarioID As Integer) As Boolean
        'Aqui se inserta un nuevo registro
        Dim Cnn As SqlConnection = Crea_ConexionSTD()
        Dim trans As SqlTransaction
        Dim Cmd As SqlCommand
        Dim remision() As String

        trans = crear_Trans(Cnn)

        Try
            For i As Integer = 0 To Remisiones.Length - 1
                'Insertar en TV_PuntosRemisiones
                Cmd = Crea_ComandoT(Cnn, trans, CommandType.StoredProcedure, "Tv_PuntosRemisiones_Create_Saltillo")

                Cmd.Parameters.Clear()
                remision = Split(Remisiones(i), ";")
                Crea_Parametro(Cmd, "@Id_Punto", SqlDbType.Int, CInt(remision(0)))
                Crea_Parametro(Cmd, "@Id_Remision", SqlDbType.Int, CInt(remision(1)))
                Crea_Parametro(Cmd, "@Usuario_Registro", SqlDbType.Int, UsuarioID)
                Crea_Parametro(Cmd, "@Id_Despacho", SqlDbType.Int, 0)

                EjecutarNonQueryT(Cmd)

                'Actualizo datos de Salida en Boveda
                Cmd.Parameters.Clear()
                Cmd = Crea_ComandoT(Cnn, trans, CommandType.StoredProcedure, "Bo_BovedaStatusSalida_Update_Saltillo")
                Crea_Parametro(Cmd, "@Id_Remision", SqlDbType.Int, CInt(remision(1)))
                Crea_Parametro(Cmd, "@Ruta_Salida", SqlDbType.Int, Ruta_Salida)
                Crea_Parametro(Cmd, "@Usuario_Salida", SqlDbType.Int, UsuarioID)

                EjecutarNonQueryT(Cmd)
            Next

            trans.Commit()
            Cnn.Close()
            Return True
        Catch ex As Exception
            trans.Rollback()
            Cnn.Close()
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Shared Function fn_ConsultaCustTemp_CambiarStatus(ByVal SucursalID As Integer, ByVal UsuarioID As Integer, ByVal Id_Punto As Integer, ByVal Status As String) As Boolean
        Try
            Dim Cnn As SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlCommand = Crea_Comando("TV_Puntos_UpdateStatus_Saltillo", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_Punto", SqlDbType.Int, Id_Punto)
            Crea_Parametro(Cmd, "@Status", SqlDbType.VarChar, Status)

            EjecutarNonQuery(Cmd)
            Return True
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

#End Region

#Region "EXPORTA GRIDVIEW A EXCEL"

    Public Shared Function fn_Exportar_Excel(ByVal Grilla As GridView, ByVal Titulo As String, ByVal Cadena1 As String, ByVal Cadena2 As String) As Boolean
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        Try
            Grilla.RenderControl(htw)
            With HttpContext.Current.Response
                .Clear()
                .Buffer = True
                .ContentType = "application/vnd.ms-excel" 'vnd.ms-word'exporta a word
                .AddHeader("Content-Disposition", "attachment;filename=ArchivoExcel.xls")
                .Charset = "UTF-8"
                .ContentEncoding = Encoding.Default
                .Output.Write("<br><b>" & Titulo & "</b>")
                .Output.Write("<br><b>" & Cadena1 & "</b>")
                .Output.Write("<br><b>" & Cadena2 & "</b>" & "<br>")
                .Output.Write(sb.ToString())
                .Flush()
            End With

        Catch ex As Exception
            TrataEx(ex)
            Return False
        Finally
            HttpContext.Current.Response.End()
        End Try
        Return True
    End Function

#End Region

    Public Shared Function fn_ObtenerDatosEmpresa(ByVal EmpresaID As Integer) As String
        Try
            Dim Cnn As SqlConnection = Crea_ConexionSTD()
            Dim Cmd As SqlCommand = Crea_Comando("Cat_Empresas_Read", CommandType.StoredProcedure, Cnn)
            Crea_Parametro(Cmd, "@Id_Empresa", SqlDbType.Int, EmpresaID)

            Dim dt_DatosEmpresa As DataTable = EjecutaConsulta(Cmd)
            If dt_DatosEmpresa.Rows.Count > 0 Then
                Return dt_DatosEmpresa.Rows(0)("Nombre")
            Else
                Return ""
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return ""
        End Try

    End Function

#Region "Conexión central 17/12/2014"
    Public Function fn_Consulta_sucursalesPropias() As DataTable
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand

            cmd = Crea_Comando("SucursalesPropias_Get", CommandType.StoredProcedure, cnn)
            Return EjecutaConsulta(cmd)

        Catch ex As Exception
            Return Nothing
        End Try

    End Function

#End Region

#Region "Avisos de Baja de Empleado 10 Agosto 2015"

    Public Shared Function fn_Empleados_GuardarAvisodeBaja(ByVal Id_EmpleadoBaja As Integer, ByVal FechaBaja As Date, ByVal EstacionRegistro As String, ByVal ComentariosBaja As String, UsuarioRegistro As Integer) As Boolean
        Dim cmd As SqlCommand = Crea_Comando("Cat_EmpleadosBajasAviso_Create", CommandType.StoredProcedure, Crea_ConexionSTD)
        Try

            Crea_Parametro(cmd, "@Id_Empleado", SqlDbType.Int, Id_EmpleadoBaja)
            Crea_Parametro(cmd, "@FechaBaja", SqlDbType.Date, FechaBaja)
            Crea_Parametro(cmd, "@Observaciones", SqlDbType.VarChar, ComentariosBaja)
            Crea_Parametro(cmd, "@Usuario_Registro", SqlDbType.Int, UsuarioRegistro)
            Crea_Parametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, EstacionRegistro)

            Return EjecutarNonQuery(cmd) > 0

        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try

    End Function

    Public Shared Function fn_Empleados_GetAvisoBaja(ByVal Id_Sucursal As Integer, ByVal Id_EmpleadoJefe As Integer, fechaInicio As Date, fechaFin As Date) As DataTable
        Try
            Dim cmd As SqlCommand = Crea_Comando("Cat_EmpleadosBajasAviso_Get", CommandType.StoredProcedure, Crea_ConexionSTD)
            Crea_Parametro(cmd, "@Id_Sucursal", SqlDbType.Int, Id_Sucursal)
            Crea_Parametro(cmd, "@Id_EmpleadoJefe", SqlDbType.Int, Id_EmpleadoJefe)
            Crea_Parametro(cmd, "@FechaInicio", SqlDbType.Date, fechaInicio)
            Crea_Parametro(cmd, "@FechaFin", SqlDbType.Date, fechaFin)
            Crea_Parametro(cmd, "@Status", SqlDbType.VarChar, "A")

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

End Class
