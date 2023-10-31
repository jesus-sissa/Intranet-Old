
Imports SISSAIntranet.Cn_Soporte
Imports SISSAIntranet.FuncionesGlobales
Imports System.Data
Imports SISSAIntranet.Cn_Mail

Partial Class UserControls_wuc_Insumos
    Inherits System.Web.UI.UserControl

    Dim valorStatus As Char
    Dim FechaDesde As String
    Dim FechaHasta As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Page.Title = "INSUMOS"

        '-----------------------------------------
        'Aquí se valida que el Usuario firmado tenga realmente Privilegios para acceder a esta página
        'o se esta introduciendo la página directo en la barra de direcciones

        Dim arr As String() = Split(Session("CadenaPriv"), ";")

        For x As Integer = 0 To arr.Length - 1
            If arr(x) = "SOLICITUD DE INSUMOS" Then
                Exit For
            ElseIf x = (arr.Length - 1) Then
                MostrarAlertAjax("NO TIENE PRIVILEGIOS PARA ACCEDER A ESTA OPCION", Me, Page)
                Response.Redirect("frm_login.aspx")
                Exit Sub
            End If
        Next

        '-------------------------------------------
        MuestraGridSubClasesVacio()
        MuestraGridSolicitudVacio()

        MuestraGridConsultaVacio()
        MuestraGridDetalleVacio()

        Session("InsumosAgregados") = ""
    End Sub

    Sub MuestraGridSubClasesVacio()
        gv_Consumibles.SelectedIndex = -1
        gv_Consumibles.DataSource = fn_CreaGridVacio("Id_Consumible,Clave,Descripcion,Elementos,Status")
        gv_Consumibles.DataBind()
    End Sub

    Sub LlenarGridSubClases()
        gv_Consumibles.SelectedIndex = -1
        gv_Consumibles.DataSource = fn_Insumos_ObtenerConsumibles(Session("DepartamentoID"), IIf(rdb_Accesorios.Checked, 1, 2), "A", Session("SucursalID"), Session("UsuarioID"))
        gv_Consumibles.DataBind()
    End Sub

    Sub MuestraGridSolicitudVacio()
        gv_Solicitud.SelectedIndex = -1
        gv_Solicitud.DataSource = fn_CreaGridVacio("Id_SubClase,Clave,Descripcion,Cantidad")
        gv_Solicitud.DataBind()
    End Sub

    Protected Sub rdbs_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdb_Accesorios.CheckedChanged, rdb_Consumibles.CheckedChanged
        Call LlenarGridSubClases()
    End Sub

    Protected Sub btn_Agregar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Agregar.Click
        ' Se valida si el grid de SubClases tiene elementos
        If gv_Consumibles.DataKeys(0).Values("Id_SubClase").ToString = "" Then Exit Sub

        Dim Cantidad As TextBox

        If Session("InsumosAgregados") <> "" Then
            'Se recorre el grid SubClases
            For i As Integer = 0 To gv_Consumibles.Rows.Count - 1
                'Se recorre el grid de la Solicitud
                For Each fila As GridViewRow In gv_Solicitud.Rows
                    'Se obtiene el valor capturado en el textbox de la fila del grid Subclases
                    Cantidad = gv_Consumibles.Rows(i).FindControl("tbx_Cantidad")
                    'Si se ha capturado una cantidad
                    If Cantidad.Text <> "" And Cantidad.Text <> "0" Then
                        If gv_Consumibles.DataKeys(i).Values("Id_SubClase").ToString = gv_Solicitud.DataKeys(fila.RowIndex).Value.ToString Then
                            MostrarAlertAjax("Hay elementos seleccionados que ya existen en la Solicitud. Favor de verificar.", btn_Agregar, Page)
                            Exit Sub
                        End If
                    End If
                Next
            Next
        End If

        For i As Integer = 0 To gv_Consumibles.Rows.Count - 1
            Cantidad = gv_Consumibles.Rows(i).FindControl("tbx_Cantidad")
            If Cantidad.Text <> "" And Cantidad.Text <> "0" Then
                If Session("InsumosAgregados") = "" Then
                    Session("InsumosAgregados") = gv_Consumibles.DataKeys(i).Values("Id_SubClase") & "," & gv_Consumibles.Rows(i).Cells(1).Text & "," & IIf(rdb_Accesorios.Checked, 1, 2) & Server.HtmlDecode(" " & Server.HtmlDecode(gv_Consumibles.Rows(i).Cells(2).Text) & "," & Cantidad.Text)
                Else
                    Session("InsumosAgregados") = Session("InsumosAgregados") & ";" & gv_Consumibles.DataKeys(i).Values("Id_SubClase") & "," & gv_Consumibles.Rows(i).Cells(1).Text & "," & IIf(rdb_Accesorios.Checked, 1, 2) & " " & Server.HtmlDecode(gv_Consumibles.Rows(i).Cells(2).Text) & "," & Cantidad.Text
                End If
            End If
        Next

        If Session("InsumosAgregados") = "" Then
            MuestraGridDetalleVacio()
        Else
            gv_Solicitud.DataSource = fn_AgregarFila("Id_SubClase,Clave,Descripcion,Cantidad", Session("InsumosAgregados"))
            gv_Solicitud.DataBind()
        End If

        LlenarGridSubClases()
    End Sub

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Guardar.Click
        If gv_Solicitud.Rows.Count >= 0 And gv_Solicitud.DataKeys(0).Value.ToString = "" Then
            MostrarAlertAjax("No se ha agregado ningún Insumo.", btn_Guardar, Page)
            Exit Sub
        End If

        Dim SolicitudId As Integer = fn_Insumos_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), Session("InsumosAgregados"))

        If SolicitudId = 0 Then
            MostrarAlertAjax("Ha ocurrido un error al intentar guardar los datos.", btn_Guardar, Page)
            Exit Sub
        End If

        Dim dr_Solicitud As DataRow = fn_Insumos_LeerDatosSolicitud(Session("SucursalID"), Session("UsuarioID"), SolicitudId)
        Dim NumSolicitud As Integer = 0
        If dr_Solicitud IsNot Nothing Then
            NumSolicitud = dr_Solicitud("Numero")
        End If

        Dim dt_Detalle As DataTable = fn_Insumos_LeerDetalleSolicitud(Session("SucursalID"), Session("UsuarioID"), SolicitudId)

        'Aquí se inserta la Alerta de Solicitud de Insumos

        Dim Detalles As String = "Número de Solicitud: " & NumSolicitud & Chr(13) _
                                & "   Usuario Solicita: " & Session("UsuarioNombre") & Chr(13) _
                                & "       Departamento: " & Session("DeptoNombre") & Chr(13) _
                                & "    Fecha Solicitud: " & Now.ToShortDateString & " - " & Now.ToShortTimeString & Chr(13) _
        '& "Agente de Servicios SIAC " & Today.Year.ToString

        Dim DetalleHTML As String = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                                    & "<tr><td colspan='4' align='center'>SOLICITUD DE INSUMOS</td></tr>" _
                                    & "<tr><td colspan='4'><br><hr /></td></tr>" _
                                    & "<tr><td align='right'><label><b>Número de Solicitud:</b></label></td><td>" & NumSolicitud & "</td><td></td><td></td></tr>" _
                                    & "<tr><td align='right'><label><b>Usuario Solicita:</b></label></td><td>" & Session("UsuarioNombre") & "</td></tr>" _
                                    & "<tr><td align='right'><label><b>Departamento:</b></label></td><td>" & Session("DeptoNombre") & "</td></tr>" _
                                    & "<tr><td align='right'><label><b>Fecha Solicitud:</b></label></td><td>" & Now.ToShortDateString & " - " & Now.ToShortTimeString & "<br></td></tr>" _
                                    & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>Pie</td></tr></table><br/><br/></body></html>" _
                                    & fn_DatatableToHTML(dt_Detalle, "DETALLE", 1, 3)

        Dim Pie As String = "Agente de Servicios SIAC " & Today.Year.ToString
        DetalleHTML = Replace(DetalleHTML, "Pie", Pie)

        'Aquí se guarda la Alerta y se envian los correos

        If fn_AlertasGeneradas_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), "22", Detalles) Then
            'Obtener los Destinos
            Dim Dt_Destinos As DataTable = fn_AlertasGeneradas_ObtenerMails("22")
            If Dt_Destinos IsNot Nothing Then
                For Each renglon As DataRow In Dt_Destinos.Rows
                    Cn_Mail.fn_Enviar_MailHTML(renglon("Mail"), "SOLICITUD DE INSUMOS", DetalleHTML, "", Session("SucursalID"))
                    'Cn_Mail.fn_Enviar_MailHTML("jose.nuncio@sissaseguridad.com", "SOLICITUD DE INSUMOS", DetalleHTML, "", Session("SucursalID"))
                    'Cn_Mail.fn_Enviar_MailHTML("raul.coss@sissaseguridad.com", "SOLICITUD DE INSUMOS", DetalleHTML, "", Session("SucursalID"))
                Next
            End If
        End If

        MuestraGridSolicitudVacio()

        Session("InsumosAgregados") = ""
    End Sub

    Protected Sub gv_Solicitud_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_Solicitud.RowEditing
        Dim lbl As Label = gv_Solicitud.Rows(e.NewEditIndex).FindControl("lbl_CantidadSolicitada")
        gv_Solicitud.EditIndex = e.NewEditIndex
        gv_Solicitud.DataSource = fn_AgregarFila("Id_SubClase,Clave,Descripcion,Cantidad", Session("InsumosAgregados"))
        gv_Solicitud.DataBind()
    End Sub

    Protected Sub gv_Solicitud_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gv_Solicitud.RowCancelingEdit
        gv_Solicitud.EditIndex = -1
        gv_Solicitud.DataSource = fn_AgregarFila("Id_SubClase,Clave,Descripcion,Cantidad", Session("InsumosAgregados"))
        gv_Solicitud.DataBind()
    End Sub

    Protected Sub gv_Solicitud_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gv_Solicitud.RowUpdating

        Dim tbx As TextBox = gv_Solicitud.Rows(e.RowIndex).FindControl("tbx_CantidadSolicitada")
        Dim lbl As Label = gv_Solicitud.Rows(e.RowIndex).FindControl("lbl_CantidadSolicitada")

        gv_Solicitud.Rows(e.RowIndex).Cells(4).Text = tbx.Text

        Dim cadena As String = ""
        Dim cant As Integer = 0

        'For i As Integer = 0 To gv_Solicitud.Rows.Count - 1
        '    If i = e.RowIndex Then
        '        lbl = gv_Solicitud.Rows(i).FindControl("lbl_CantidadSolicitada")
        '        cant = CInt(lbl.Text)
        '    Else
        '        tbx = DirectCast(gv_Solicitud.Rows(i).Cells(4).FindControl("tbx_CantidadSolicitada"), TextBox)
        '        cant = CInt(tbx.Text)
        '    End If
        '    If cadena = "" Then
        '        cadena = gv_Solicitud.DataKeys(i).Value & "," & gv_Solicitud.Rows(i).Cells(2).Text & "," & Server.HtmlDecode(gv_Solicitud.Rows(i).Cells(3).Text) & "," & tbx.Text
        '    Else
        '        cadena = cadena & ";" & gv_Solicitud.DataKeys(i).Value & "," & gv_Solicitud.Rows(i).Cells(2).Text & "," & Server.HtmlDecode(gv_Solicitud.Rows(i).Cells(3).Text) & "," & tbx.Text
        '    End If
        'Next


        gv_Solicitud.DataSource = fn_AgregarFila("Id_SubClase,Clave,Descripcion,Cantidad", InsumosAgregadosCadena)
        gv_Solicitud.DataBind()
        gv_Solicitud.EditIndex = -1
        Session("InsumosAgregados") = cadena

    End Sub

    Function InsumosAgregadosCadena() As String
        Dim tbx As TextBox

        Dim cadena As String = ""
        For Each row As GridViewRow In gv_Solicitud.Rows
            tbx = row.Cells(4).FindControl("Id_Subclase")
            If cadena = "" Then
                cadena = gv_Solicitud.DataKeys(row.RowIndex).Value & "," & gv_Solicitud.Rows(row.RowIndex).Cells(2).Text & "," & Server.HtmlDecode(gv_Solicitud.Rows(row.RowIndex).Cells(3).Text) & "," & tbx.Text
            Else
                cadena = cadena & ";" & gv_Solicitud.DataKeys(row.RowIndex).Value & "," & gv_Solicitud.Rows(row.RowIndex).Cells(2).Text & "," & Server.HtmlDecode(gv_Solicitud.Rows(row.RowIndex).Cells(3).Text) & "," & tbx.Text
            End If
        Next

        Return cadena
    End Function

#Region "Consulta"

    Sub MuestraGridConsulta()

    End Sub

    Sub MuestraGridConsultaVacio()
        gv_Solicitudes.SelectedIndex = -1
        gv_Solicitudes.DataSource = fn_CreaGridVacio("Id_Solicitud,Numero,Fecha,Hora,UsuarioSolicita,Status")
        gv_Solicitudes.DataBind()
    End Sub

    Sub MuestraGridDetalleVacio()
        gv_Detalle.SelectedIndex = -1
        gv_Detalle.DataSource = fn_CreaGridVacio("Id_Subclase,Clave,Descripcion,CantidadSolicitada,CantidadValidada,CantidadSurtida")
        gv_Detalle.DataBind()
    End Sub

    Protected Sub cbx_Status_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbx_Status.CheckedChanged
        MuestraGridConsultaVacio()
        MuestraGridDetalleVacio()
        If cbx_Status.Checked Then
            ddl_Status.SelectedValue = 0
            ddl_Status.Enabled = False
        Else
            ddl_Status.Enabled = True
        End If
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Mostrar.Click
        If tbx_FechaIni.Text = "" Then
            MostrarAlertAjax("Seleccione la Fecha Inicio.", btn_Mostrar, Page)
            tbx_FechaIni.Focus()
            Exit Sub
        End If

        If tbx_FechaFin.Text = "" Then
            MostrarAlertAjax("Seleccione la Fecha Fin.", btn_Mostrar, Page)
            tbx_FechaFin.Focus()
            Exit Sub
        End If

        If ddl_Status.SelectedIndex = 0 And Not cbx_Status.Checked Then
            MostrarAlertAjax("Seleccione el Status.", btn_Mostrar, Page)
            ddl_Status.Focus()
            Exit Sub
        Else
            If cbx_Status.Checked Then
                valorStatus = "T"
            Else
                valorStatus = ddl_Status.SelectedValue
            End If
        End If

        Dim dt As DataTable = fn_Insumos_ObtenerSolicitudes(Session("SucursalID"), Session("UsuarioID"), CDate(tbx_FechaIni.Text).ToShortDateString, CDate(tbx_FechaFin.Text).ToShortDateString, valorStatus)

        If dt IsNot Nothing Then
            gv_Solicitudes.DataSource = dt
            gv_Solicitudes.DataBind()
        Else
            gv_Solicitudes.DataSource = fn_CreaGridVacio("Id_Solicitud,Numero,Fecha,Hora,UsuarioSolicita,Status")
            gv_Solicitudes.DataBind()
        End If

    End Sub

    Protected Sub ddl_Status_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Status.SelectedIndexChanged
        MuestraGridConsultaVacio()
        MuestraGridDetalleVacio()
    End Sub

    Protected Sub tbx_FechaIni_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbx_FechaIni.TextChanged
        MuestraGridConsultaVacio()
        MuestraGridDetalleVacio()
    End Sub

    Protected Sub tbx_FechaFin_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbx_FechaFin.TextChanged
        MuestraGridConsultaVacio()
        MuestraGridDetalleVacio()
    End Sub

    Protected Sub gv_Solicitudes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Solicitudes.SelectedIndexChanged
        If gv_Solicitudes.DataKeys(0).Values("Id_Solicitud").ToString = "" Then
            gv_Solicitudes.SelectedIndex = -1
            Exit Sub
        End If
        gv_Detalle.DataSource = fn_Insumos_ObtenerSolicitudesDetalle(Session("SucursalID"), Session("UsuarioID"), gv_Solicitudes.SelectedDataKey("Id_Solicitud"))
        gv_Detalle.DataBind()
    End Sub

    Protected Sub gv_Detalle_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_Detalle.RowEditing
        Dim lbl As Label = gv_Solicitud.Rows(e.NewEditIndex).FindControl("lbl_CantidadSolicitada")
        gv_Solicitudes.EditIndex = e.NewEditIndex
        gv_Solicitudes.DataSource = fn_Insumos_ObtenerSolicitudesDetalle(Session("SucursalID"), Session("UsuarioID"), gv_Solicitudes.SelectedDataKey("Id_Solicitud"))
        gv_Solicitudes.DataBind()
    End Sub

    Protected Sub gv_Detalle_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gv_Detalle.RowCancelingEdit
        gv_Solicitud.EditIndex = -1
        gv_Solicitud.DataSource = fn_Insumos_ObtenerSolicitudesDetalle(Session("SucursalID"), Session("UsuarioID"), gv_Solicitudes.SelectedDataKey("Id_Solicitud"))
        gv_Solicitud.DataBind()
    End Sub

    Protected Sub gv_Detalle_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gv_Detalle.RowUpdating
        Dim tbx As TextBox = gv_Detalle.Rows(e.RowIndex).FindControl("tbx_CantidadSolicitada")
        If Not fn_Insumos_ActualizarCantidad(Session("SucursalID"), Session("UsuarioID"), gv_Detalle.DataKeys("Id_Solicitud").Value, CInt(tbx.Text)) Then
            MostrarAlertAjax("Ha ocurrido un error al intentar modificar los datos.", sender, Page)
            Exit Sub
        End If
        gv_Detalle.EditIndex = -1
        Dim dt As DataTable = fn_Insumos_ObtenerSolicitudes(Session("SucursalID"), Session("UsuarioID"), CDate(tbx_FechaIni.Text).ToShortDateString, CDate(tbx_FechaFin.Text).ToShortDateString, valorStatus)
        gv_Detalle.DataSource = dt
        gv_Detalle.DataBind()
    End Sub

    Protected Sub gv_Solicitudes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Solicitudes.PageIndexChanging
        MuestraGridConsultaVacio()
        MuestraGridDetalleVacio()

        If cbx_Status.Checked Then
            valorStatus = "T"
        Else
            valorStatus = ddl_Status.SelectedValue
        End If

        gv_Solicitudes.PageIndex = e.NewPageIndex
        gv_Solicitudes.DataSource = fn_Insumos_ObtenerSolicitudes(Session("SucursalID"), Session("UsuarioID"), CDate(tbx_FechaIni.Text).ToShortDateString, CDate(tbx_FechaFin.Text).ToShortDateString, valorStatus)
        gv_Solicitudes.DataBind()
    End Sub

#End Region

End Class
