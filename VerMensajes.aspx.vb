Imports IntranetSIAC.Cn_Soporte
Imports System.Web
Imports System.Web.UI.WebControls.Unit
Imports System.Data
Imports IntranetSIAC.FuncionesGlobales

Partial Class VerMensajes
    Inherits BasePage

    Dim Desde As Date = #1/1/2010#
    Dim Hasta As Date = Today.AddDays(1)
    Dim valorStatus As Char
    Dim dt_Usuarios As DataTable
    Dim dt_Recibidos As DataTable
    Dim dt_Enviados As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub
     
        dt_Usuarios = fn_EnviarMensajes_LlenarListaU(Session("SucursalID"), Session("UsuarioID"))
        fn_LlenarDDL(ddl_Usuario, dt_Usuarios, "Nombre", "Id_Empleado", "0")

        'dt_Recibidos = fn_VerMensajes_LlenarLista(Session("ModuloClave"), "A", Session("UsuarioID"), Desde, Hasta, ddl_Usuario.SelectedValue, Session("SucursalID"))

        'If dt_Recibidos IsNot Nothing And dt_Recibidos.Rows.Count > 0 Then
        'gv_Recibidos.DataSource = dt_Recibidos
        ' gv_Recibidos.DataBind()
        'Else
        gv_Recibidos.DataSource = fn_CreaGridVacio("Id_Mensaje,Fecha de Registro,Asunto,Remitente,Tipo,Status")
        gv_Recibidos.DataBind()
        ' End If

        gv_Enviados.DataSource = fn_CreaGridVacio("Id_Mensaje,Fecha de Registro,Asunto,Destinatario,Tipo")
        gv_Enviados.DataBind()

    End Sub

    Sub MostrarGridVacio()
        If tbc_Mensajes.ActiveTab Is tab_Recibidos Then
            gv_Recibidos.DataSource = fn_CreaGridVacio("Id_Mensaje,Fecha de Registro,Asunto,Remitente,Tipo,Status")
            gv_Recibidos.DataBind()
            gv_Recibidos.SelectedIndex = -1
            btn_Atendido.Enabled = False
        Else
            gv_Enviados.DataSource = fn_CreaGridVacio("Id_Mensaje,Fecha de Registro,Asunto,Destinatario,Tipo")
            gv_Enviados.DataBind()
            gv_Enviados.SelectedIndex = -1
        End If
    End Sub

    Sub ActualizarGrid()
        Desde = #1/1/2010#
        Hasta = Today.AddDays(1)

        If tbc_Mensajes.ActiveTab Is tab_Recibidos Then
            chk_Status.Checked = True
            dt_Recibidos = fn_VerMensajes_LlenarLista(Session("ModuloClave"), "A", Session("UsuarioID"), Desde, Hasta, ddl_Usuario.SelectedValue, Session("SucursalID"))
            If dt_Recibidos IsNot Nothing AndAlso dt_Recibidos.Rows.Count > 0 Then
                gv_Recibidos.DataSource = dt_Recibidos
                gv_Recibidos.DataBind()
                gv_Recibidos.SelectedIndex = -1
            Else
                Call MostrarGridVacio()
            End If
        Else
            dt_Enviados = fn_VerMensajes_LlenarListaEnviados(Session("UsuarioID"), Desde, Hasta, ddl_Usuario.SelectedValue, Session("SucursalID"))
            If dt_Enviados IsNot Nothing AndAlso dt_Enviados.Rows.Count > 0 Then
                gv_Enviados.DataSource = dt_Enviados
                gv_Enviados.DataBind()
                gv_Enviados.SelectedIndex = -1
            Else
                Call MostrarGridVacio()
            End If
    
        End If
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Mostrar.Click

        If tbx_FechaIni.Text = String.Empty And Not chk_TodasF.Checked Then
                  fn_Alerta("Seleccione la Fecha Inicio.")
            Exit Sub
        End If

        If tbx_FechaFin.Text = "" And Not chk_TodasF.Checked Then
             fn_Alerta("Seleccione la Fecha Fin.")
            Exit Sub
        End If

        If chk_TodasF.Checked Then
            Desde = #1/1/2010#
            Hasta = Today.AddDays(1)
        Else
            Desde = CDate(tbx_FechaIni.Text)
            Hasta = CDate(tbx_FechaFin.Text)
        End If

        If ddl_Usuario.SelectedIndex = 0 And Not chk_Usuario.Checked Then
            If tbc_Mensajes.ActiveTab Is tab_Recibidos Then
                    fn_Alerta("Seleccione el Remitente.")
            Else
                     fn_Alerta("Seleccione el Destinatario.")
            End If
            Exit Sub
        End If

        Dim Status = ""
        If tbc_Mensajes.ActiveTab Is tab_Recibidos Then
            gv_Recibidos.SelectedIndex = -1
            LimpiarDetalle()
            If chk_Status.Checked Then
                Status = "A"
            Else
                Status = "T"
            End If
            dt_Recibidos = fn_VerMensajes_LlenarLista(Session("ModuloClave"), Status, Session("UsuarioID"), Desde, Hasta, ddl_Usuario.SelectedValue, Session("SucursalID"))
            If dt_Recibidos IsNot Nothing AndAlso dt_Recibidos.Rows.Count > 0 Then
                gv_Recibidos.DataSource = dt_Recibidos
                gv_Recibidos.DataBind()
            Else
                Call MostrarGridVacio()
            End If
        Else
            dt_Enviados = fn_VerMensajes_LlenarListaEnviados(Session("UsuarioID"), Desde, Hasta, ddl_Usuario.SelectedValue, Session("SucursalID"))

            If dt_Enviados IsNot Nothing AndAlso dt_Enviados.Rows.Count > 0 Then
                gv_Enviados.DataSource = dt_Enviados
                gv_Enviados.DataBind()
            Else
                Call MostrarGridVacio()
            End If
        End If

    End Sub

    Protected Sub tbc_Mensajes_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbc_Mensajes.ActiveTabChanged
        If tbc_Mensajes.ActiveTab Is tab_Recibidos Then
            lbl_Usuario.Text = "Remitente"
            ddl_Usuario.Width = Pixel(245)
        Else
            lbl_Usuario.Text = "Destinatario"
            ddl_Usuario.Width = Pixel(245)
            btn_Atendido.Enabled = False
        End If
    End Sub

    Sub LimpiarDetalle()
        tbx_FechaReg.Text = ""
        tbx_Hora.Text = ""
        tbx_Asunto.Text = ""
        tbx_Mensaje.Text = ""
        tbx_MensajeR.Text = ""
        btn_Responder.Enabled = False
    End Sub

    Sub ValidarFechas()

        If chk_TodasF.Checked Then
            tbx_FechaIni.Text = ""
            tbx_FechaFin.Text = ""
            tbx_FechaIni.Enabled = False
            tbx_FechaFin.Enabled = False
        Else
            tbx_FechaIni.Text = ""
            tbx_FechaFin.Text = ""
            tbx_FechaIni.Enabled = True
            tbx_FechaFin.Enabled = True
        End If

    End Sub

    Protected Sub chk_TodasF_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_TodasF.CheckedChanged
        MostrarGridVacio()
        LimpiarDetalle()
        ValidarFechas()
    End Sub

    Protected Sub chk_Usuario_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Usuario.CheckedChanged
        MostrarGridVacio()
        LimpiarDetalle()
        If chk_Usuario.Checked Then
            ddl_Usuario.SelectedValue = 0
            ddl_Usuario.Enabled = False
        Else
            ddl_Usuario.Enabled = True
        End If
    End Sub

    Protected Sub chk_Status_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk_Status.CheckedChanged
        Call MostrarGridVacio()
        Call LimpiarDetalle()
    End Sub

    Protected Sub gv_Recibidos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Recibidos.SelectedIndexChanged
        Dim resultado As Integer
        Integer.TryParse(gv_Recibidos.SelectedDataKey.Values("Id_Mensaje"), resultado)
        If resultado = 0 Then
            gv_Recibidos.SelectedIndex = -1
            Exit Sub
        End If
        Call LimpiarDetalle()

        Dim dr As DataRow = fn_VerMensajes_LlenarDetalle(gv_Recibidos.SelectedDataKey.Values("Id_Mensaje"), Session("SucursalID"), Session("UsuarioID"))
        If dr IsNot Nothing Then
            tbx_FechaReg.Text = dr("Fecha")
            tbx_Hora.Text = dr("Hora")
            tbx_Asunto.Text = dr("Asunto")
            tbx_Mensaje.Text = dr("Mensaje")
            Session("Usuario_Reg") = dr("Usuario_Registro")
            If gv_Recibidos.SelectedRow.Cells(6).Text = "PENDIENTE" Then
                btn_Responder.Enabled = True
                btn_Atendido.Enabled = True
            Else
                btn_Responder.Enabled = False
                btn_Atendido.Enabled = False
            End If
        Else
            fn_Alerta("Ha ocurrido un error al intentar consultar el Mensaje.")
        End If
    End Sub

    Private Sub gv_Enviados_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Enviados.PageIndexChanging
        If chk_TodasF.Checked Then
            Desde = #1/1/2010#
            Hasta = Today.AddDays(1)
        Else
            Desde = tbx_FechaIni.Text
            Hasta = tbx_FechaFin.Text
        End If
        If chk_Status.Checked Then
            valorStatus = "A"
        Else
            valorStatus = "T"
        End If
        gv_Enviados.SelectedIndex = -1
        LimpiarDetalle()
        gv_Enviados.PageIndex = e.NewPageIndex

        dt_Enviados = fn_VerMensajes_LlenarListaEnviados(Session("UsuarioID"), Desde, Hasta, ddl_Usuario.SelectedValue, Session("SucursalID"))

        If dt_Enviados IsNot Nothing Then
            gv_Enviados.DataSource = dt_Enviados
            gv_Enviados.DataBind()
        Else
            MostrarGridVacio()
        End If
    End Sub

    Protected Sub gv_Enviados_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Enviados.PreRender
        If tbc_Mensajes.ActiveTab Is tab_Recibidos Then Exit Sub
    End Sub

    Protected Sub gv_Enviados_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Enviados.SelectedIndexChanged
        Dim resultado As Integer
        Integer.TryParse(gv_Enviados.SelectedDataKey.Values("Id_Mensaje"), resultado)
        If resultado = 0 Then
            gv_Enviados.SelectedIndex = -1
            Exit Sub
        End If

        Dim dr As DataRow = fn_VerMensajes_LlenarDetalle(gv_Enviados.SelectedDataKey.Values("Id_Mensaje"), Session("SucursalID"), Session("UsuarioID"))
        If dr IsNot Nothing Then
            tbx_FechaReg.Text = dr("Fecha")
            tbx_Hora.Text = dr("Hora")
            tbx_Asunto.Text = dr("Asunto")
            tbx_Mensaje.Text = dr("Mensaje")
        Else
              fn_Alerta("Ha ocurrido un error al intentar consultar el Mensaje.")
        End If
    End Sub

    Protected Sub btn_Responder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Responder.Click

        If tbx_MensajeR.Text.Length < 2 Then
            fn_Alerta("Por favor indique un mensaje de Respuesta.")
            Exit Sub
        End If

        Dim ModuloD As String = ""
        Dim Respuesta As String = tbx_MensajeR.Text & Chr(13) & Chr(13) & "<-- Mensaje Original -->" & Chr(13) & tbx_Mensaje.Text

        If Not fn_VerMensajes_Responder(Session("UsuarioID"), Session("EstacioN"), Session("ModuloClave"), "RE : " & tbx_Asunto.Text, Respuesta, ModuloD, Session("Usuario_Reg"), Session("SucursalID")) Then
               fn_Alerta("Ha ocurrido un error al intentar enviar el mensaje.")
            Exit Sub
        Else
            ' MostrarAlertAjax("El mensaje se ha enviado correctamente.", btn_Responder, Page)
            fn_Alerta("El mensaje se ha enviado correctamente.")
        End If

        Call AtenderMensaje()
    End Sub

    Protected Sub gv_Recibidos_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Recibidos.PreRender
        If tbc_Mensajes.ActiveTab Is tab_Enviados Then Exit Sub
    End Sub

    Protected Sub btn_Atendido_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Atendido.Click
        Call AtenderMensaje()
    End Sub

    Sub AtenderMensaje()
        If gv_Recibidos.Rows(0).Cells(2).Text = "&nbsp;" Then
            fn_Alerta("No ha seleccionado el mensaje.")
            Exit Sub
        End If

        If Not fn_VerMensajes_Status(gv_Recibidos.SelectedDataKey.Values("Id_Mensaje"), "V", Session("UsuarioID"), Session("EstacioN"), Session("SucursalID")) Then
            fn_Alerta("Ha ocurrido un error al intentar atender el Mensaje.")
            Exit Sub
        End If
        tbx_FechaIni.Text = ""
        tbx_FechaFin.Text = ""
        chk_TodasF.Checked = True
        ddl_Usuario.SelectedValue = 0
        ddl_Usuario.Enabled = False
        chk_Usuario.Checked = True
        Call LimpiarDetalle()
        Call ActualizarGrid()
    End Sub

    Protected Sub gv_Recibidos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Recibidos.PageIndexChanging

        If chk_TodasF.Checked Then
            Desde = #1/1/2010#
            Hasta = Today.AddDays(1)
        Else
            Desde = tbx_FechaIni.Text
            Hasta = tbx_FechaFin.Text
        End If
        If chk_Status.Checked Then
            valorStatus = "A"
        Else
            valorStatus = "T"
        End If
        gv_Recibidos.SelectedIndex = -1
        LimpiarDetalle()
        gv_Recibidos.PageIndex = e.NewPageIndex

        dt_Recibidos = fn_VerMensajes_LlenarLista(Session("ModuloClave"), valorStatus, Session("UsuarioID"), Desde, Hasta, ddl_Usuario.SelectedValue, Session("SucursalID"))
        If dt_Recibidos IsNot Nothing Then
            gv_Recibidos.DataSource = dt_Recibidos
            gv_Recibidos.DataBind()
        Else
            MostrarGridVacio()
        End If
    End Sub

    Protected Sub tbx_FechaIni_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbx_FechaIni.TextChanged, tbx_FechaFin.TextChanged
        Call MostrarGridVacio()
        Call LimpiarDetalle()
    End Sub

End Class
