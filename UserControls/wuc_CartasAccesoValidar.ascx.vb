
Imports SISSAIntranet.Cn_Soporte
Imports SISSAIntranet.FuncionesGlobales
Imports System.Data
Imports SISSAIntranet.Cn_Mail
Imports SISSAIntranet.Cn_Login

Partial Class UserControls_wuc_CartasAccesoValidar
    Inherits System.Web.UI.UserControl

    Dim Firma As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Page.Title = "VALIDAR MEMO"

        '-----------------------------------------
        'Aquí se valida que el Usuario firmado tenga realmente Privilegios para acceder a esta página
        'o se esta introduciendo la página directo en la barra de direcciones

        Dim arr As String() = Split(Session("CadenaPriv"), ";")

        For x As Integer = 0 To arr.Length - 1
            If arr(x) = "VALIDAR CARTA DE ACCESO" Then
                Exit For
            ElseIf x = (arr.Length - 1) Then
                MostrarAlertAjax("NO TIENE PRIVILEGIOS PARA ACCEDER A ESTA OPCION", Me, Page)
                Response.Redirect("frm_login.aspx")
                Exit Sub
            End If
        Next

        '-------------------------------------------

        MuestraGridsVacios()
        LlenarGridCartasAcceso()
        pnl_Firma.Enabled = False
    End Sub

    Sub MuestraGridsVacios()
        gv_CartasAcceso.SelectedIndex = -1
        gv_CartasAcceso.DataSource = fn_CreaGridVacio("Id_Carta,FechaRegistro,UsuarioRegistro,FechaInicio,FechaFin,Status,Tipo,Observaciones")
        gv_CartasAcceso.DataBind()

        gv_Detalle.DataSource = fn_CreaGridVacio("Id_Carta,Clave,Nombre,Empresa")
        gv_Detalle.DataBind()

        pnl_Firma.Enabled = False
        tbx_Observaciones.Text = ""
        lbl_EncabezadoComentarios.Enabled = False
    End Sub

    Sub LlenarGridCartasAcceso()

        Dim dt As DataTable = fn_CartasAccesoValidar_LlenarLista(Session("SucursalID"), Session("UsuarioID"))

        If dt IsNot Nothing Then
            gv_CartasAcceso.DataSource = dt
            gv_CartasAcceso.DataBind()
        Else
            gv_CartasAcceso.DataSource = fn_CreaGridVacio("Id_Carta,FechaRegistro,UsuarioRegistro,FechaInicio,FechaFin,Status,Tipo,Observaciones")
            gv_CartasAcceso.DataBind()
        End If

    End Sub

    Sub LlenarGridDetalle()
        Dim dt As DataTable = fn_CartasAccesoValidar_LlenarDetalle(Session("SucursalID"), Session("UsuarioID"), gv_CartasAcceso.SelectedDataKey.Values("Id_Carta"))
        If dt IsNot Nothing Then
            gv_Detalle.DataSource = dt
            gv_Detalle.DataBind()
            tbx_Observaciones.Text = gv_CartasAcceso.SelectedDataKey.Values("Observaciones")
        Else
            gv_Detalle.DataSource = fn_CreaGridVacio("Id_Carta,Clave,Nombre,Empresa")
            gv_Detalle.DataBind()
            tbx_Observaciones.Text = ""
        End If
    End Sub

    Protected Sub gv_CartasAcceso_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_CartasAcceso.PageIndexChanging
        MuestraGridsVacios()
        gv_CartasAcceso.PageIndex = e.NewPageIndex
        gv_CartasAcceso.DataSource = fn_CartasAccesoValidar_LlenarLista(Session("SucursalID"), Session("UsuarioID"))
        gv_CartasAcceso.DataBind()
    End Sub

    Protected Sub gv_CartasAcceso_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_CartasAcceso.SelectedIndexChanged
        LlenarGridDetalle()
        pnl_Firma.Enabled = True
        lbl_EncabezadoComentarios.Enabled = True
    End Sub

    Protected Sub gv_CartasAcceso_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_CartasAcceso.RowCreated
        ' En este Sub se agregan a las filas de dgv_Empleados los atributos "onmouseover" y "onmouseout"
        ' para que cuando el puntero del mouse este sobre una fila, se apliquen las propiedades declaradas (backgoundColor)

        ' only apply changes if its DataRow
        If e.Row.RowType = DataControlRowType.DataRow Then

            ' when mouse is over the row, save original color to new attribute, and change it to highlight yellow color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#C0A062'")  '#D0A540'")

            ' when mouse leaves the row, change the bg color to its original value    
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")

        End If
    End Sub

    Protected Sub btn_Validar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Validar.Click

        If tbx_Comentarios.Text = "" Then
            MostrarAlertAjax("Capture los Comentarios de Validación.", btn_Validar, Page)
            tbx_Comentarios.Focus()
            Exit Sub
        End If

        ValidarFirma()

        If Firma Then

            If Not fn_CartasAccesoValidar_Validar(gv_CartasAcceso.SelectedDataKey.Values("Id_Carta"), Session("UsuarioID"), Session("EstacioN"), tbx_Comentarios.Text, Session("SucursalID"), Session("UsuarioID")) Then
                MostrarAlertAjax("Ha ocurrido un error al intentar guardar los datos.", btn_Validar, Page)
                Exit Sub
            Else
                MostrarAlertAjax("Los datos han sido guardados correctamente.", btn_Validar, Page)
            End If

            ''Aquí se inserta la Alerta de Validación de Nuevo Ingreso

            'Dim Detalles As String = "           Nombre: " & tbx_Clave.Text & " - " & tbx_NombreCompleto.Text & Chr(13) _
            '                        & "    Departamento: " & tbx_Departamento.Text & Chr(13) _
            '                        & "          Puesto: " & tbx_Puesto.Text & Chr(13) _
            '                        & "   Fecha Ingreso: " & tbx_FechaIngreso.Text & Chr(13) _
            '                        & "Fecha Validación: " & Now.ToShortDateString & " - " & Now.ToShortTimeString & Chr(13) _
            '                        & "    Validado por: " & Session("UsuarioNombre")
            ''& "Agente de Servicios SIAC " & Today.Year.ToString

            'Dim DetalleHTML As String = "<html><body><table style='border: solid 1px'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
            '                            & "<tr><td colspan='4' align='center'>VALIDACION DE EMPLEADO DE NUEVO INGRESO</td></tr>" _
            '                            & "<tr><td colspan='4'><br><hr /></td></tr>" _
            '                            & "<tr><td align='right'><label><b>Nombre:</b></label></td><td>" & tbx_Clave.Text & " - " & tbx_NombreCompleto.Text & "</td><td></td><td></td></tr>" _
            '                            & "<tr><td align='right'><label><b>Departamento:</b></label></td><td>" & tbx_Departamento.Text & "</td></tr>" _
            '                            & "<tr><td align='right'><label><b>Puesto:</b></label></td><td>" & tbx_Puesto.Text & "</td></tr>" _
            '                            & "<tr><td align='right'><label><b>Fecha Ingreso:</b></label></td><td>" & tbx_FechaIngreso.Text & "</td></tr>" _
            '                            & "<tr><td align='right'><label><b>Fecha Validación:</b></label></td><td>" & Now.ToShortDateString & " - " & Now.ToShortTimeString & "<br></td></tr>" _
            '                            & "<tr><td align='right'><label><b>Validado por:</b></label></td><td>" & Session("UsuarioNombre") & "</td></tr>" _
            '                            & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>Pie</td></tr></table></body></html>"

            'Dim Pie As String = "Agente de Servicios SIAC " & Today.Year.ToString
            'DetalleHTML = Replace(DetalleHTML, "Pie", Pie)

            ''Aquí se guarda la Alerta y se envian los correos

            'If fn_AlertasGeneradas_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), "21", Detalles) Then
            '    'Obtener los Destinos
            '    Dim Dt_Destinos As DataTable = fn_AlertasGeneradas_ObtenerMails("21")
            '    If Dt_Destinos IsNot Nothing Then
            '        For Each renglon As DataRow In Dt_Destinos.Rows
            '            Cn_Mail.fn_Enviar_MailHTML(renglon("Mail"), "VALIDACION DE NUEVO INGRESO", DetalleHTML, "", Session("SucursalID"))
            '            'Cn_Mail.fn_Enviar_MailHTML("jose.nuncio@sissaseguridad.com", "VALIDACION DE NUEVO INGRESO", DetalleHTML, "", Session("SucursalID"))
            '        Next
            '    End If
            'End If

            CerrarFirma()

            'Actualizar el Grid de Cartas de Acceso al Validar la Carta 
            MuestraGridsVacios()
            LlenarGridCartasAcceso()
        Else
            tbx_Contrasena.Focus()
        End If

    End Sub

    Sub ValidarFirma()

        Firma = False

        Dim Contra As String = tbx_Contrasena.Text

        If Contra = "" Then
            MostrarAlertAjax("Indique la Contraseña.", btn_Validar, Page)
            tbx_Contrasena.Focus()
            Exit Sub
        End If
        If Len(Contra) < 4 Then
            MostrarAlertAjax("Contraseña Incorrecta.", btn_Validar, Page)
            tbx_Contrasena.Focus()
            Exit Sub
        End If
        If Not FuncionesGlobales.fn_Valida_Cadena(Contra, FuncionesGlobales.Validar_Cadena.LetrasYnumeros) Then
            'En caso de que el nombre no sea valido
            MostrarAlertAjax("Indique una Contraseña válida.", btn_Validar, Page)
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
                    'FormsAuthentication.RedirectFromLoginPage(Usuario, False)
                    'MsgBox("Solo los usuarios tipo 2 pueden entrar a esta aplicación", Response)
                    Firma = True
                Else
                    'MsgBox("Usuario o Contraseña incorrecta", Response)
                    MostrarAlertAjax("Contraseña incorrecta.", btn_Validar, Page)
                    Exit Sub
                End If

                If tbl.Rows(0)("Dias_Expira") < 1 Then
                    'MsgBox("La Contraseña está expirada.", Response)
                    MostrarAlertAjax("La Contraseña está expirada.", btn_Validar, Page)
                    Exit Sub
                End If

                If tbl.Rows(0)("Status") <> "A" Then
                    'MsgBox("Usuario Bloqueado. Imposible Entrar al SIAC Intranet. Consulte al Administrador.", Response)
                    MostrarAlertAjax("Usuario Bloqueado. Imposible Entrar al SIAC Intranet. Consulte al Administrador.", btn_Validar, Page)
                    Exit Sub
                End If

            Else
                MostrarAlertAjax("Usuario no encontrado.", btn_Validar, Page)
                Exit Sub
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

    End Sub

    Sub CerrarFirma()

        tbx_Contrasena.Text = ""
        tbx_Comentarios.Text = ""
        btn_Validar.Enabled = False

    End Sub

End Class
