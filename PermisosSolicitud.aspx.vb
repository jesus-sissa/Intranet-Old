Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.Cn_Login
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data
Imports IntranetSIAC.BasePage

Partial Public Class PermisosSolicitud
    Inherits BasePage

    Dim Firma As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub
      
        fn_Log_Create(Session("UsuarioID"), Session("ModuloClave"), "ABRIR PAGINA: REGISTRO DE PERMISOS", Session("EstacioN"), Session("EstacionIP"), "", Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

        Call LlenarEmpleados()
        Call LlenarDDLsManual()
    End Sub

    Sub MuestraGridsVacios()
        gv_Empleados.DataSource = fn_CreaGridVacio("Id_Empleado,Clave,Nombre")
        gv_Empleados.DataBind()
        gv_Empleados.SelectedIndex = -1
    End Sub

    Sub LlenarEmpleados()
        Dim dt As DataTable = fn_Default_GetEmpleados(Session("SucursalID"), Session("UsuarioId"), "N")

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            gv_Empleados.DataSource = dt
        Else
            gv_Empleados.DataSource = fn_CreaGridVacio("Id_Empleado,Clave,Nombre")
        End If
        gv_Empleados.DataBind()
    End Sub

    Sub LlenarDDLsManual()
        LlenarMinutos(ddl_Jornada1De, 30)
        ddl_Jornada1De.SelectedValue = 0

        LlenarMinutos(ddl_Jornada1A, 30)
        ddl_Jornada1A.SelectedValue = 0

        LlenarMinutos(ddl_Jornada2De, 30)
        ddl_Jornada2De.SelectedValue = 0

        LlenarMinutos(ddl_Jornada2A, 30)
        ddl_Jornada2A.SelectedValue = 0
    End Sub

    Sub LimpiarObjetos()
        'tbx_Fecha.Text = ""
        tbx_Jornada1.Text = String.Empty
        tbx_Jornada2.Text = String.Empty
        ddl_Jornada1De.SelectedIndex = 0
        ddl_Jornada1A.SelectedIndex = 0
        ddl_Jornada2De.SelectedIndex = 0
        ddl_Jornada2De.Enabled = True
        ddl_Jornada2A.SelectedIndex = 0
        ddl_Jornada2A.Enabled = True
        tbx_Motivos.Text = String.Empty
        udp_DatosPermiso.Update()
    End Sub

    Protected Sub gv_Empleados_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Empleados.SelectedIndexChanged
        If Val(gv_Empleados.SelectedDataKey.Values("Id_Empleado")) = 0 Then
            gv_Empleados.SelectedIndex = -1
            btn_Guardar.Enabled = False
            Exit Sub
        Else
            Call LimpiarObjetos()
            tbx_Fecha.Text = ""
            btn_Guardar.Enabled = True
        End If
    End Sub

    Protected Sub tbx_Fecha_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbx_Fecha.TextChanged
        Call LimpiarObjetos()
        If Len(tbx_Fecha.Text) > 1 Then
            If tbx_Fecha.Text >= Now.Date Then
                Dim dr As DataRow = fn_PermisosRegistro_ObtenerJornada(gv_Empleados.SelectedValue, tbx_Fecha.Text, Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"))
                If dr IsNot Nothing Then
                    tbx_Jornada1.Text = dr.Item("Jornada1")
                    tbx_Jornada2.Text = dr.Item("Jornada2")
                    udp_DatosPermiso.Update()
                    If tbx_Jornada2.Text = "00:00/00:00" Then
                        ddl_Jornada2De.Enabled = False
                        ddl_Jornada2A.Enabled = False
                    End If
                End If
            Else
                  fn_Alerta("La Fecha del Permiso debe ser el día de hoy o uno posterior.")
                tbx_Fecha.Focus()
                tbx_Fecha.Text = ""
                Exit Sub
            End If
        End If
    End Sub

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Guardar.Click
        Dim J2_Anterior As String = ""
        Dim J2_Nueva As String = ""

        If tbx_Fecha.Text.Trim = "" Then
            fn_Alerta("Seleccione la Fecha del Permiso.")
            Exit Sub
        End If
        If ddl_Jornada1De.SelectedIndex = 0 Or ddl_Jornada1A.SelectedIndex = 0 Then
            fn_Alerta("Seleccione el Horario de la Nueva Jornada 1.")
            Exit Sub
        End If
        If ddl_Jornada2De.Enabled And (ddl_Jornada2De.SelectedIndex = 0 Or ddl_Jornada2A.SelectedIndex = 0) Then
            fn_Alerta("Seleccione el Horario de la Nueva Jornada 2.")
            Exit Sub
        ElseIf ddl_Jornada2De.Enabled Then
            J2_Anterior = " - " & tbx_Jornada2.Text
            J2_Nueva = " - " & ddl_Jornada2De.SelectedValue & "/" & ddl_Jornada2A.SelectedValue
        End If
        If tbx_Motivos.Text = "" Then
            fn_Alerta("Capture un Motivo para la solicitud.")
            Exit Sub
        End If

        Call Validar()

        If Firma Then

            Dim TipoIncidencia As Integer = 1   'Cambio de Jornada
            Dim Descripcion As String = "Horario Normal: " & tbx_Jornada1.Text & J2_Anterior & "  -  Horario Nuevo: " & ddl_Jornada1De.SelectedValue & "/" & ddl_Jornada1A.SelectedValue & J2_Nueva

            If Not fn_PermisosRegistro_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), gv_Empleados.SelectedDataKey.Values("Id_Empleado"), tbx_Fecha.Text, TipoIncidencia, Descripcion, tbx_Motivos.Text.ToUpper, "Ninguna") Then
                fn_Alerta("Ha ocurrido un error al intentar guardar los datos.")
                Exit Sub
            Else
                fn_Alerta("Los datos se han guardado correctamente.")
            End If

            'Aquí se inserta la Alerta de Registro de Incidencia (Permiso)

            Dim Detalles As String = "          Empleado: " & gv_Empleados.SelectedRow.Cells(1).Text & " - " & gv_Empleados.SelectedRow.Cells(2).Text & Chr(13) _
                                    & " Fecha Incidencia: " & tbx_Fecha.Text & Chr(13) _
                                    & "           Motivo: " & tbx_Motivos.Text & Chr(13) _
                                    & "      Descripción: " & Descripcion & Chr(13) _
                                    & "Sucursal Autoriza: " & Session("SucursalN") & Chr(13) _
                                    & "         Autoriza: " & Session("UsuarioNombre") & Chr(13) _
                                    & "   Fecha Autoriza: " & Now.ToShortDateString & " - " & Now.ToShortTimeString & Chr(13)

            Dim DetalleHTML As String = "<html><body><table style='border: solid 1px'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                                        & "<tr><td colspan='4' align='center'>INCIDENCIA DE EMPLEADO</td></tr>" _
                                        & "<tr><td colspan='4'><hr /></td></tr>" _
                                        & "<tr><td align='right'><label><b>Empleado:</b></label></td><td>" & gv_Empleados.SelectedRow.Cells(1).Text & " - " & gv_Empleados.SelectedRow.Cells(2).Text & "</td><td></td><td></td></tr>" _
                                        & "<tr><td align='right'><label><b>Fecha Incidencia:</b></label></td><td>" & tbx_Fecha.Text & "</td></tr>" _
                                        & "<tr><td align='right'><label><b>Motivo:</b></label></td><td>" & tbx_Motivos.Text.ToUpper & "</td></tr>" _
                                        & "<tr><td align='right'><label><b>Descripción:</b></label></td><td>" & Descripcion & "</td></tr>" _
                                        & "<tr><td align='right'><label><b>Sucursal Autoriza:</b></label></td><td>" & Session("SucursalN") & "</td></tr>" _
                                        & "<tr><td align='right'><label><b>Autoriza:</b></label></td><td>" & Session("UsuarioNombre") & "</td></tr>" _
                                        & "<tr><td align='right'><label><b>Fecha Autoriza:</b></label></td><td>" & Now.ToShortDateString & " - " & Now.ToShortTimeString & "<br></td></tr>" _
                                        & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>Agente de Servicios SIAC " & Today.Year.ToString & "</td></tr></table></body></html>"

            'Aquí se guarda la Alerta y se envian los correos
            If fn_AlertasGeneradas_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), "41", Detalles) Then
                'Obtener los Destinos
                Dim Dt_Destinos As DataTable = fn_AlertasGeneradas_ObtenerMails("41")
                If Dt_Destinos IsNot Nothing Then
                    For Each renglon As DataRow In Dt_Destinos.Rows
                        Cn_Mail.fn_Enviar_MailHTML(renglon("Mail"), "INCIDENCIA DE EMPLEADO", DetalleHTML, "", Session("SucursalID"))
                                 Next
                End If
            End If

            Call LlenarEmpleados()
            Call LimpiarObjetos()
            tbx_Fecha.Text = ""
            tbx_Contrasena.Text = ""
        End If
    End Sub

    Sub Validar()

        Firma = False
        Dim Contra As String = tbx_Contrasena.Text.Trim

        If Contra = "" Then
              fn_Alerta("Indique la Contraseña.")
            tbx_Contrasena.Focus()
            Exit Sub
        End If
        If Len(Contra) < 4 Then
            MostrarAlertAjax("Contraseña Incorrecta.", btn_Guardar, Page)
            fn_Alerta("Contraseña Incorrecta.")
            tbx_Contrasena.Focus()
            Exit Sub
        End If
        If Not FuncionesGlobales.fn_Valida_Cadena(Contra, FuncionesGlobales.Validar_Cadena.LetrasNumerosYCar) Then
            'En caso de que el nombre no sea valido
               fn_Alerta("Indique una Contraseña válida.")
            tbx_Contrasena.Focus()
            Exit Sub
        End If

        Try
            Dim tbl As DataTable = Usuarios_Login(Session("UsuarioID"))

            If tbl.Rows.Count > 0 Then

                Dim PasswordUsr As String = FormsAuthentication.HashPasswordForStoringInConfigFile(Contra, "SHA1")
                Dim PasswordDb As String = tbl.Rows(0).Item("Password")
                Dim Usuario As String = tbl.Rows(0).Item("Nombre")
                Dim Tipo As Integer = tbl.Rows(0).Item("Tipo")

                If PasswordUsr = PasswordDb Then
                      'MsgBox("Solo los usuarios tipo 2 pueden entrar a esta aplicación", Response)
                    Firma = True
                Else
                       fn_Alerta("Contraseña incorrecta.")
                    Exit Sub
                End If

                If tbl.Rows(0)("Dias_Expira") < 1 Then
                         fn_Alerta("La Contraseña está expirada.")
                    Exit Sub
                End If

                If tbl.Rows(0)("Status") <> "A" Then
                         fn_Alerta("Usuario Bloqueado. Imposible Entrar al SIAC Intranet. Consulte al Administrador.")
                    Exit Sub
                End If

            Else
                     fn_Alerta("Usuario no encontrado.")
                Exit Sub
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub

End Class