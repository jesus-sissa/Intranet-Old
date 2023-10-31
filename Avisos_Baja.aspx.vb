Imports IntranetSIAC.FuncionesGlobales
Imports IntranetSIAC.Cn_Soporte

Public Class Avisos_Baja
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If IsPostBack Then Exit Sub
       
        Cn_Login.fn_Log_Create(Session("UsuarioID"), Session("ModuloClave"), "ABRIR PAGINA: AVISOS DE BAJA", Session("EstacioN"), Session("EstacionIP"), "", Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

        Dim dt As DataTable = Cn_Soporte.fn_Default_GetEmpleados(Session("SucursalID"), Session("UsuarioID"), "N")

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            gv_Empleados.DataSource = dt
        Else
            gv_Empleados.DataSource = fn_CreaGridVacio("Id_Empleado,Clave,Nombre,Departamento,Puesto")
        End If
        gv_Empleados.DataBind()

        '-------Cuenta columnas y alinea contenido y encabezado
        Dim Columnas As Byte
        Dim Gv_Nombres() As GridView = {gv_Empleados}

        For j As Byte = 0 To Gv_Nombres.Length - 1
            Columnas = Gv_Nombres(j).Columns.Count

            For i As Byte = 0 To Columnas - 1
                Gv_Nombres(j).Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
            Next
        Next
        tbx_FechaBaja.Text = Now.Date
    End Sub

    Protected Sub btn_Guardar_Click(sender As Object, e As EventArgs) Handles btn_Guardar.Click

        If tbx_FechaBaja.Text.Trim = "" Then
            fn_Alerta("Capture la fecha  de baja del Empleado.")
            Exit Sub
        End If

        Call Validar()
        Dim IdEmpleado As Integer = gv_Empleados.SelectedDataKey("Id_Empleado")

        If Validar() Then
            Dim FechaBaja As Date = CDate(tbx_FechaBaja.Text)

            If fn_Empleados_GuardarAvisodeBaja(IdEmpleado, FechaBaja, Session("EstacioN"), tbx_Comentarios.Text.ToUpper, Session("UsuarioID")) Then
                fn_Alerta("Los datos han sido guardados correctamente.")

                '--enviar la alerta

                Dim Detalle As String = "    Nombre: " & gv_Empleados.SelectedRow.Cells(2).Text & Chr(13) _
                               & "          Sucursal: " & Session("SucursalN") & Chr(13) _
                               & "      Departamento: " & gv_Empleados.SelectedRow.Cells(3).Text & Chr(13) _
                               & "            Puesto: " & gv_Empleados.SelectedRow.Cells(4).Text & Chr(13) _
                               & "  Fecha Aviso Baja: " & Now.ToShortDateString & " - " & Now.ToShortTimeString & Chr(13) _
                               & "Usuario Aviso Baja: " & Session("UsuarioNombre")

                Dim Pie As String = "Agente de Servicios SIAC " & Now.Year.ToString
                Dim DetalleHTML As String = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                                        & "<tr><td colspan=4' align='center'> AVISO DE BAJA DE EMPLEADO </td></tr>" _
                                        & "<tr><td colspan='4'><hr /></td></tr>" _
                                        & "<tr><td align='right'><label><b>Sucursal:</b></label></td><td> " & Session("SucursalN") & " </td><td></td><td></td></tr>" _
                                        & "<tr><td align='right'><label><b>Nombre:</b></label></td><td>" & gv_Empleados.SelectedRow.Cells(2).Text & "</td><td></td><td></td></tr>" _
                                        & "<tr><td align='right'><label><b>Departamento:</b></label></td><td>" & gv_Empleados.SelectedRow.Cells(3).Text & "</td><td></td><td></td></tr>" _
                                        & "<tr><td align='right'><label><b>Puesto:</b></label></td><td>" & gv_Empleados.SelectedRow.Cells(4).Text & "</td><td></td><td></td></tr>" _
                                        & "<tr><td align='right'><label><b>Fecha Aviso Baja:</b></label></td><td>" & Now.Date & "</td><td></td><td></td></tr>" _
                                        & "<tr><td align='right'><label><b>Observaciones:</b></label></td><td>" & tbx_Comentarios.Text.ToUpper & "</td><td></td><td></td></tr>" _
                                        & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='4' align='center'>" & Pie & "</td></tr></table><br/><br/></body></html>"

                'Aquí se guarda la Alerta y se envian los correos
                If fn_AlertasGeneradas_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), "54", Detalle) Then
                    'Obtener los Destinos
                    Dim Dt_Destinos As DataTable = fn_AlertasGeneradas_ObtenerMails("54")
                    If Dt_Destinos IsNot Nothing Then
                        For Each renglon As DataRow In Dt_Destinos.Rows
                            Cn_Mail.fn_Enviar_MailHTML(renglon("Mail"), "AVISO DE BAJA DE EMPLEADO", DetalleHTML, "", Session("SucursalID"))
                        Next
                    End If
                End If
                '------------------

                Call Limpiar()
            Else
                fn_Alerta("Ocurrio un error al guardar los datos.")
            End If
        End If
    End Sub

    Protected Sub btn_Cancelar_Click(sender As Object, e As EventArgs) Handles btn_Cancelar.Click
        Call Limpiar()
    End Sub

    Protected Sub dgv_Empleados_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Empleados.SelectedIndexChanged
        If gv_Empleados.Rows(0).Cells(2).Text = "&nbsp;" Then Exit Sub
        tablaComentarios.Visible = True

    End Sub

    Private Sub Limpiar()
        gv_Empleados.SelectedIndex = -1
        tablaComentarios.Visible = False
        tbx_FechaBaja.Text = String.Empty
        tbx_Comentarios.Text = String.Empty

        tbx_Contrasena.Text = String.Empty
    End Sub

    Private Function Validar() As Boolean
        Dim Contra As String = tbx_Contrasena.Text

        If Contra = "" Then
            fn_Alerta("Indique la Contraseña.")
            tbx_Contrasena.Focus()
            Return False
        End If
        If Len(Contra) < 4 Then
            fn_Alerta("Contraseña Incorrecta.")
            tbx_Contrasena.Focus()
            Return False
        End If
        If Not FuncionesGlobales.fn_Valida_Cadena(Contra, FuncionesGlobales.Validar_Cadena.LetrasYnumeros) Then
            'En caso de que el nombre no sea valido
            fn_Alerta("Indique una Contraseña válida.")
            tbx_Contrasena.Focus()
            Return False
        End If

        Try
            Dim tbl As DataTable = Cn_Login.Usuarios_Login(Session("UsuarioID"))

            If tbl IsNot Nothing AndAlso tbl.Rows.Count > 0 Then
                Dim PasswordUsr As String = FormsAuthentication.HashPasswordForStoringInConfigFile(Contra, "SHA1")
                Dim PasswordDb As String = tbl.Rows(0).Item("Password")
                Dim Usuario As String = tbl.Rows(0).Item("Nombre")
                Dim Tipo As Integer = tbl.Rows(0).Item("Tipo")

                If tbl.Rows(0)("Dias_Expira") < 1 Then
                    fn_Alerta("La Contraseña está expirada.")
                    Return False
                End If

                If PasswordUsr = PasswordDb Then
                    'MsgBox("Solo los usuarios tipo 2 pueden entrar a esta aplicación", Response)
                    Return True
                Else
                    fn_Alerta("Usuario o Contraseña incorrecta.")
                    tbx_Contrasena.Focus()
                    Return False
                End If
            Else
                fn_Alerta("Usuario o Contraseña incorrecta.")
                tbx_Contrasena.Focus()
                Return False
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
            Return False
        End Try
    End Function

    Protected Sub gv_Empleados_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_Empleados.PageIndexChanging
        tablaComentarios.Visible = False

        gv_Empleados.SelectedIndex = -1
        gv_Empleados.PageIndex = e.NewPageIndex

        gv_Empleados.DataSource = fn_Default_GetEmpleados(Session("SucursalID"), Session("UsuarioID"), "N")
        gv_Empleados.DataBind()
    End Sub

End Class