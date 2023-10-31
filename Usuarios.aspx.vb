Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.Cn_Login
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data

Partial Public Class Usuarios
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub
        Call LlenarUsuarios()

        Dim Columnas As Byte
        Columnas = gv_Usuarios.Columns.Count
        For i As Byte = 0 To Columnas - 1
            gv_Usuarios.Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
        Next
    End Sub

    Sub LlenarUsuarios()
        Dim dt_Usuarios As DataTable = fn_Usuarios_Consultar(Session("SucursalID"), Session("UsuarioID"))

        If dt_Usuarios IsNot Nothing AndAlso dt_Usuarios.Rows.Count > 0 Then
            gv_Usuarios.DataSource = dt_Usuarios
            gv_Usuarios.DataBind()
        Else
            Call MuestraGridUsuariosVacio()
        End If
    End Sub

    Sub MuestraGridUsuariosVacio()
        gv_Usuarios.DataSource = fn_CreaGridVacio("Id_Empleado,ID,Clave,Nombre,VenceClave,Tipo,Departamento,Puesto,Status,ClaveExpira")
        gv_Usuarios.DataBind()
        gv_Usuarios.SelectedIndex = -1
    End Sub

    Protected Sub gv_Usuarios_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Usuarios.PageIndexChanging
        gv_Usuarios.PageIndex = e.NewPageIndex
        Call LlenarUsuarios()
    End Sub

    Protected Sub gv_Usuarios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Usuarios.RowCommand
        If gv_Usuarios.Rows(0).Cells(2).Text = "&nbsp;" Then Exit Sub

        Select Case e.CommandName
            Case "Reiniciar"

                'Asignar como Clave su Número de Empleado
                Dim ContraHash As String = FormsAuthentication.HashPasswordForStoringInConfigFile(gv_Usuarios.Rows(e.CommandArgument).Cells(1).Text, "SHA1")

                Dim conta As Integer = Cn_Soporte.fn_UsuariosContra_Reiniciar(Session("SucursalID"), Session("UsuarioID"), gv_Usuarios.DataKeys(e.CommandArgument).Values("Id_Empleado"), ContraHash)
                If conta > 0 Then
                    Cn_Login.fn_Log_Create(Session("UsuarioID"), "33", "REINICIAR CONTRASEÑA A USUARIO: " & gv_Usuarios.Rows(e.CommandArgument).Cells(2).Text, Session("EstacioN"), Session("EstacionIP"), Session("EstacionMac"), Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))
                    fn_Alerta("La Contraseña se Reinició correctamente.")
                    Call LlenarUsuarios()
                Else
                    Cn_Login.fn_Log_Create(Session("UsuarioID"), "33", "ERROR AL REINICIAR CONTRASEÑA A USUARIO: " & gv_Usuarios.Rows(e.CommandArgument).Cells(2).Text, Session("EstacioN"), Session("EstacionIP"), Session("EstacionMac"), Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))
                    fn_Alerta("Ocurrió un problema al intentar Reiniciar la Contraseña.")
                End If

                gv_Usuarios.PageIndex = 0
                Call LlenarUsuarios()

            Case "Bloq_Desbloq"

                If gv_Usuarios.Rows(e.CommandArgument).Cells(7).Text = "BLOQUEADO" Then

                    Cn_Login.fn_Log_Create(Session("UsuarioID"), "33", "DESBLOQUEAR USUARIO: " & gv_Usuarios.Rows(e.CommandArgument).Cells(2).Text, Session("EstacioN"), Session("EstacionIP"), Session("EstacionMac"), Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

                    Cn_Soporte.fn_Usuarios_Bloquear(Session("SucursalID"), Session("UsuarioID"), gv_Usuarios.DataKeys(e.CommandArgument).Values("Id_Empleado"), "A")
                ElseIf gv_Usuarios.Rows(e.CommandArgument).Cells(7).Text = "ACTIVO" Then

                    Cn_Login.fn_Log_Create(Session("UsuarioID"), "33", "BLOQUEAR USUARIO: " & gv_Usuarios.Rows(e.CommandArgument).Cells(2).Text, Session("EstacioN"), Session("EstacionIP"), Session("EstacionMac"), Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

                    Cn_Soporte.fn_Usuarios_Bloquear(Session("SucursalID"), Session("UsuarioID"), gv_Usuarios.DataKeys(e.CommandArgument).Values("Id_Empleado"), "B")
                End If

                gv_Usuarios.PageIndex = 0
                Call LlenarUsuarios()

            Case "Eliminar"

                'Call Cn_Login.fn_Log_Create(Session("UsuarioID"), "33", "ELIMINAR USUARIO: " & gv_Usuarios.Rows(e.CommandArgument).Cells(2).Text, Session("EstacioN"), Session("EstacionIP"), Session("EstacionMac"), Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

                ''Eliminar primero sus Horarios y Privilegios para que no queden registros huérfanos

                ''Eliminar los Horarios del Usuario que se va a Eliminar
                'Call Cn_Soporte.fn_Usuarios_EliminarHoras(Session("SucursalID"), Session("UsuarioID"), gv_Usuarios.DataKeys(e.CommandArgument).Values("Id_Empleado"))

                ''Eliminar los Privilegios del Usuarios que se va a Eliminar
                'Call Cn_Soporte.fn_Usuarios_EliminarPrivilegios(Session("SucursalID"), Session("UsuarioID"), gv_Usuarios.DataKeys(e.CommandArgument).Values("Id_Empleado"))

                ''Eliminar el Usuario
                'Call Cn_Soporte.fn_Usuarios_Eliminar(Session("SucursalID"), Session("UsuarioID"), gv_Usuarios.DataKeys(e.CommandArgument).Values("Id_Empleado"))

                ''Actualizar la lista desplegable de Empleados
                'Call LlenarEmpleados()

                ''Actualizar lista de Usuario
                'gv_Usuarios.PageIndex = 0
                'Call LlenarUsuarios()

            Case "ClaveExpira"

                If gv_Usuarios.Rows(e.CommandArgument).Cells(8).Text = "SI EXPIRA" Then

                    If (fn_Usuarios_ClaveExpira(gv_Usuarios.Rows(e.CommandArgument).Cells(0).Text, "N")) = -1 Then
                        fn_Alerta("Ocurrió un Error al Intentar cambiar la clave expira al Usuario.")
                        Exit Sub
                    End If
                    Call Cn_Login.fn_Log_Create(Session("UsuarioID"), "33", "CAMBIO A CONTRASEÑA SI EXPIRA DEL USUARIO: " & gv_Usuarios.Rows(e.CommandArgument).Cells(2).Text, Session("EstacioN"), Session("EstacionIP"), Session("EstacionMac"), Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

                ElseIf gv_Usuarios.Rows(e.CommandArgument).Cells(8).Text = "NO EXPIRA" Then

                    If (fn_Usuarios_ClaveExpira(gv_Usuarios.Rows(e.CommandArgument).Cells(0).Text, "S")) = -1 Then
                        fn_Alerta("Ocurrió un Error al Intentar cambiar la clave expira al Usuario.")
                        Exit Sub
                    End If
                    Call Cn_Login.fn_Log_Create(Session("UsuarioID"), "33", "CAMBIO A CONTRASEÑA NO EXPIRA DEL USUARIO: " & gv_Usuarios.Rows(e.CommandArgument).Cells(2).Text, Session("EstacioN"), Session("EstacionIP"), Session("EstacionMac"), Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

                End If

                Call LlenarUsuarios()
        End Select
    End Sub

    'Sub LlenarEmpleados()
    '    Dim dt_Empleados As DataTable = fn_Usuarios_ConsultarEmpleados(Session("SucursalID"), Session("UsuarioID"))

    '    If dt_Empleados IsNot Nothing Then
    '        If dt_Empleados.Rows.Count > 0 Then
    '            fn_LlenarDDL_VariosCampos(ddl_Empleados, dt_Empleados, "Nombre", "Id_Empleado")
    '        End If
    '    End If
    'End Sub

    'Protected Sub btn_Agregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Agregar.Click
    '    Dim Clave As String = ""
    '    Dim TipoU As Integer = 0

    '    If ddl_Empleados.SelectedIndex = 0 Then
    '        fn_Alerta("Seleccione el Empleado que se va a Agregar como Usuario.")
    '        Exit Sub
    '    End If

    '    If ddl_TipoUsuario.SelectedValue = 0 Then
    '        fn_Alerta("Seleccione el tipo de Usuario.")
    '        Exit Sub
    '    End If
    '    TipoU = ddl_TipoUsuario.SelectedValue

    '    If TipoU = 2 And ddl_ClaveExpira.SelectedValue = "0" Then
    '        fn_Alerta("Debe seleccionar si la contraseña del usuario va a expirar.")
    '        Exit Sub
    '    End If

    '    Dim dt As DataTable = fn_Usuarios_ConsultarEmpleados(Session("SucursalID"), Session("UsuarioID"))
    '    For Each dr As DataRow In dt.Rows
    '        If dr("Id_Empleado") = ddl_Empleados.SelectedValue Then
    '            Clave = dr("Clave")
    '            Exit For
    '        End If
    '    Next

    '    'Asignar como Clave su Número de Empleado
    '    Dim ContraHash As String = FormsAuthentication.HashPasswordForStoringInConfigFile(Clave, "SHA1")
    '    If Cn_Soporte.fn_Usuarios_Agregar(Session("SucursalID"), Session("UsuarioID"), ddl_Empleados.SelectedValue, ContraHash, TipoU, ddl_ClaveExpira.SelectedValue) = -1 Then
    '        fn_Alerta("Ocurrió un Error al Intentar agregar al nuevo usuario.")
    '        Exit Sub
    '    End If
    '    Cn_Login.fn_Log_Create(Session("UsuarioID"), "33", "AGREGAR NUEVO USUARIO: " & ddl_Empleados.SelectedItem.Text, Session("EstacioN"), Session("EstacionIP"), Session("EstacionMac"), Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

    '    'Listar los Empleados
    '    Call LlenarEmpleados()

    '    gv_Usuarios.PageIndex = 0
    '    Call LlenarUsuarios()
    'End Sub

    'Protected Sub ddl_TipoUsuario_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_TipoUsuario.SelectedIndexChanged
    '    If ddl_TipoUsuario.SelectedValue = 2 Then
    '        ddl_ClaveExpira.Enabled = True
    '        ddl_ClaveExpira.SelectedValue = "0"
    '    Else
    '        ddl_ClaveExpira.SelectedValue = "S"
    '        ddl_ClaveExpira.Enabled = False
    '    End If
    'End Sub
End Class