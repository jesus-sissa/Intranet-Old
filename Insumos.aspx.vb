Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data
Imports IntranetSIAC.Cn_Mail
Imports System.Threading

Partial Class Insumos
    Inherits BasePage
    Dim valorStatus As Char
    Dim FechaDesde As String
    Dim FechaHasta As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Call MuestraGridConsumiblesVacio()
        Call MuestraGridSolicitudVacio()

        Call MuestraGridConsultaVacio()
        Call MuestraGridDetalleVacio()

        Dim Columnas As Byte
        Dim Gv_Nombres() As GridView = {gv_Solicitudes}

        For j As Byte = 0 To Gv_Nombres.Length - 1
            Columnas = Gv_Nombres(j).Columns.Count

            For i As Byte = 0 To Columnas - 1
                Gv_Nombres(j).Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
            Next
        Next
    End Sub

#Region "Solicitud de Insumos"

    Private Sub MuestraGridConsumiblesVacio()
        gv_Consumibles.SelectedIndex = -1
        gv_Consumibles.DataSource = fn_CreaGridVacio("Id_Consumible,Clave,Descripcion,Cantidad")
        gv_Consumibles.DataBind()
    End Sub

    Private Sub LlenarGridConsumibles()
        gv_Consumibles.SelectedIndex = -1
        Dim dt As DataTable = fn_Insumos_ObtenerConsumibles(Session("DepartamentoID"), IIf(rdb_Accesorios.Checked, 1, 2), "A", Session("SucursalID"), Session("UsuarioID"))
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            gv_Consumibles.DataSource = dt
            gv_Consumibles.DataBind()
        Else
            Call MuestraGridConsumiblesVacio()
        End If
    End Sub

    Private Sub MuestraGridSolicitudVacio()
        gv_Solicitud.SelectedIndex = -1
        gv_Solicitud.DataSource = fn_CreaGridVacio("Id_Consumible,Clave,Descripcion,Cantidad,Cantidad_Ant")
        gv_Solicitud.DataBind()
        btn_Guardar.Enabled = False
    End Sub

    Private Sub CancelarEdicionGridSolicitud()
        For Each Row As GridViewRow In gv_Solicitud.Rows
            If Row.Cells(6).FindControl("tbx_CantidadSolicitada") IsNot Nothing Then
                gv_Solicitud.EditIndex = -1
                gv_Solicitud.DataSource = fn_AgregarFila("Id_Consumible,Clave,Descripcion,Cantidad,Cantidad_Ant", InsumosAgregados_Solicitud(False, Row.RowIndex))
                gv_Solicitud.DataBind()
                Exit For
            End If
        Next
    End Sub

    Protected Sub rdbs_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdb_Accesorios.CheckedChanged, rdb_Consumibles.CheckedChanged
        MuestraGridConsumiblesVacio()
        If sender.Checked Then LlenarGridConsumibles()
    End Sub

    Protected Sub btn_Agregar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Agregar.Click
        ' Se valida si el grid de SubClases tiene elementos
        If gv_Consumibles.DataKeys(0).Values("Id_Consumible").ToString = "" Then Exit Sub

        Dim Cantidad As TextBox
        Dim Con As GridViewRow
        Dim Sol As GridViewRow
        For Each Con In gv_Consumibles.Rows
            For Each Sol In gv_Solicitud.Rows
                'Revisar los que se esten capturando con lo que se tiene ya registrado en la solicitud
                Cantidad = Con.Cells(3).FindControl("tbx_Cantidad")
                If Val(Cantidad.Text) <> 0 Then
                    btn_Guardar.Enabled = True
                    If gv_Consumibles.DataKeys(Con.RowIndex).Value.ToString = gv_Solicitud.DataKeys(Sol.RowIndex).Value.ToString Then
                        fn_Alerta("Existen elementos seleccionados que ya existen en la Solicitud. Favor de verificar.")
                        Con.Cells(3).Focus()
                        Exit Sub
                    End If
                End If
            Next
        Next

        Call CancelarEdicionGridSolicitud()

        Dim InsumosAgregar As String = ""
        Dim Cantidad_Sol As Label
        For Each Sol In gv_Solicitud.Rows
            'Omitir la primer fila que se tiene vacía
            If gv_Solicitud.DataKeys(0).Value = "" Then Continue For

            Cantidad_Sol = Sol.Cells(3).FindControl("lbl_CantidadSolicitada")
            InsumosAgregar &= gv_Solicitud.DataKeys(Sol.RowIndex).Value.ToString & "," & gv_Solicitud.DataKeys(Sol.RowIndex).Values("Clave") _
                              & "," & Server.HtmlDecode(gv_Solicitud.DataKeys(Sol.RowIndex).Values("Descripcion")) & "," & Cantidad_Sol.Text _
                              & "," & Cantidad_Sol.Text & ";"
        Next
        For Each Con In gv_Consumibles.Rows
            'Revisar los que se esten capturando con lo que se tiene ya registrado en la solicitud
            Cantidad = Con.Cells(3).FindControl("tbx_Cantidad")

            If Val(Cantidad.Text) = 0 Then Continue For

            InsumosAgregar &= gv_Consumibles.DataKeys(Con.RowIndex).Value.ToString & "," & gv_Consumibles.DataKeys(Con.RowIndex).Values("Clave") _
                              & "," & Server.HtmlDecode(gv_Consumibles.Rows(Con.RowIndex).Cells(2).Text) & "," & Cantidad.Text _
                              & "," & Cantidad.Text & ";"
        Next

        If InsumosAgregar = "" Then
            Call MuestraGridDetalleVacio()
        Else
            InsumosAgregar = InsumosAgregar.Substring(0, (InsumosAgregar.Length - 1))
            gv_Solicitud.DataSource = fn_AgregarFila("Id_Consumible,Clave,Descripcion,Cantidad,Cantidad_Ant", InsumosAgregar)
            gv_Solicitud.DataBind()
        End If

        Call LlenarGridConsumibles()
    End Sub

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Guardar.Click

        If gv_Solicitud.DataKeys(0).Value.ToString = "" Then
            fn_Alerta("No se ha agregado ningún Insumo.")
            Exit Sub
        End If

        Call CancelarEdicionGridSolicitud()
        Dim InsumosAgregar As String = ""
        Dim Cantidad_Sol As Label
        For Each Sol In gv_Solicitud.Rows
            'Omitir la primer fila que se tiene vacía
            If gv_Solicitud.DataKeys(0).Value = "" Then Continue For

            Cantidad_Sol = Sol.Cells(3).FindControl("lbl_CantidadSolicitada")
            InsumosAgregar &= gv_Solicitud.DataKeys(Sol.RowIndex).Value.ToString & "," & gv_Solicitud.DataKeys(Sol.RowIndex).Values("Clave") _
                              & "," & Server.HtmlDecode(gv_Solicitud.DataKeys(Sol.RowIndex).Values("Descripcion")) & "," & Cantidad_Sol.Text & ";"
        Next
        InsumosAgregar = InsumosAgregar.Substring(0, (InsumosAgregar.Length - 1))

        Dim SolicitudId As Integer = fn_Insumos_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), InsumosAgregar, txt_Observaciones.Text.ToUpper, 1)

        If SolicitudId = 0 Then
            fn_Alerta("Ha ocurrido un error al intentar guardar los datos.")

            Exit Sub
        End If

        Dim dr_Solicitud As DataRow = fn_Insumos_LeerDatosSolicitud(Session("SucursalID"), Session("UsuarioID"), SolicitudId)

        Dim NumSolicitud As Integer = 0
        If dr_Solicitud IsNot Nothing Then NumSolicitud = dr_Solicitud("Numero")

        Dim dt_Detalle As DataTable = fn_Insumos_LeerDetalleSolicitud(Session("SucursalID"), Session("UsuarioID"), SolicitudId)

        'Aquí se inserta la Alerta de Solicitud de Insumos

        Dim Detalles As String = "Número de Solicitud: " & NumSolicitud & Chr(13) _
                                & "   Usuario Solicita: " & Session("UsuarioNombre") & Chr(13) _
                                & "           Sucursal: " & Session("SucursalN") & Chr(13) _
                                & "       Departamento: " & Session("DeptoNombre") & Chr(13) _
                                & "    Fecha Solicitud: " & Now.ToShortDateString & " - " & Now.ToShortTimeString & Chr(13) _
                                & "      Observaciones: " & txt_Observaciones.Text.ToUpper

        Dim DetalleHTML As String
        If dt_Detalle IsNot Nothing Then
            'DetalleHTML = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
            '              & "<tr><td colspan='4' align='center'>SOLICITUD DE INSUMOS</td></tr>" _
            '              & "<tr><td colspan='4'><br><hr /></td></tr>" _
            '              & "<tr><td align='right'><label><b>Número de Solicitud:</b></label></td><td>" & NumSolicitud & "</td><td></td><td></td></tr>" _
            '              & "<tr><td align='right'><label><b>Usuario Solicita:</b></label></td><td>" & Session("UsuarioNombre") & "</td></tr>" _
            '              & "<tr><td align='right'><label><b>Sucursal:</b></label></td><td>" & Session("SucursalN") & "</td></tr>" _
            '              & "<tr><td align='right'><label><b>Departamento:</b></label></td><td>" & Session("DeptoNombre") & "</td></tr>" _
            '              & "<tr><td align='right'><label><b>Fecha Solicitud:</b></label></td><td>" & Now.ToShortDateString & " - " & Now.ToShortTimeString & "<br></td></tr>" _
            '              & "<tr><td align='right'><label><b>Observaciones:</b></label></td><td>" & txt_Observaciones.Text.ToUpper & "<br></td></tr>" _
            '              & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>Agente de Servicios SIAC</td></tr></table><br/><br/></body></html>" _
            '              & fn_DatatableToHTML(dt_Detalle, "DETALLE", 1, 3)
            DetalleHTML = "<html><body style='color: #000; font-family: Lucida Console, Courier New, monospace;'>" _
     & "<table style='border-radius: 25px; background:rgba(178, 186, 187 ); border: solid 0px ;  width:100%;'><tr><td colspan='4' align='center'><b style='color #D68910; font-size: 26PX;>Boletín Informativo</b></td></tr>" _
     & "<tr><td colspan='4' align='center'> <b style='color #D68910; font-size 26PX;'>SOLICITUD DE INSUMOS</b></td></tr>" _
     & "<tr><td colspan='4'><br><hr /></td></tr>" _
     & "<tr><td align='right'><label><b>Número de Solicitud:</b></label></td><td>" & NumSolicitud & "</td><td></td><td></td></tr>" _
     & "<tr><td align='right'><label><b>Usuario Solicita:</b></label></td><td>" & Session("UsuarioNombre") & "</td></tr>" _
     & "<tr><td align='right'><label><b>Sucursal:</b></label></td><td>" & Session("SucursalN") & "</td></tr>" _
     & "<tr><td align='right'><label><b>Departamento:</b></label></td><td>" & Session("DeptoNombre") & "</td></tr>" _
     & "<tr><td align='right'><label><b>Fecha Solicitud:</b></label></td><td>" & Now.ToShortDateString & " - " & Now.ToShortTimeString & "<br></td></tr>" _
     & "<tr><td align='right'><label><b>Observaciones:</b></label></td><td>" & txt_Observaciones.Text.ToUpper & "<br></td></tr>" _
     & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'> <b style='color: #D68910; font-size: 26PX;'>Agente de Servicios SIAC</b></td></tr></table><br/><br/>" _
     & "</body></html>"



        Else
            DetalleHTML = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                          & "<tr><td colspan='4' align='center'>SOLICITUD DE INSUMOS</td></tr>" _
                          & "<tr><td colspan='4'><br><hr /></td></tr>" _
                          & "<tr><td align='right'><label><b>Número de Solicitud:</b></label></td><td>" & NumSolicitud & "</td><td></td><td></td></tr>" _
                          & "<tr><td align='right'><label><b>Usuario Solicita:</b></label></td><td>" & Session("UsuarioNombre") & "</td></tr>" _
                          & "<tr><td align='right'><label><b>Sucursal:</b></label></td><td>" & Session("SucursalN") & "</td></tr>" _
                          & "<tr><td align='right'><label><b>Departamento:</b></label></td><td>" & Session("DeptoNombre") & "</td></tr>" _
                          & "<tr><td align='right'><label><b>Fecha Solicitud:</b></label></td><td>" & Now.ToShortDateString & " - " & Now.ToShortTimeString & "<br></td></tr>" _
                          & "<tr><td align='right'><label><b>Observaciones:</b></label></td><td>" & txt_Observaciones.Text.ToUpper & "<br></td></tr>" _
                          & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>Agente de Servicios SIAC</td></tr></table><br/><br/></body></html>"
        End If

        'Aquí se guarda la Alerta y se envian los correos
        Call ObtenerDestinos(Detalles, DetalleHTML, "SOLICITUD DE INSUMOS")

        Call MuestraGridSolicitudVacio()
        txt_Observaciones.Text = String.Empty
    End Sub

    Private Sub gv_Solicitud_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Solicitud.RowDeleting
        If gv_Solicitud.DataKeys(0).Values("Id_Consumible").ToString = "" Then Exit Sub

        Call CancelarEdicionGridSolicitud()

        Dim InsumosAgregar As String = InsumosAgregados_Solicitud(False, -1, e.RowIndex)
        If InsumosAgregar <> "" Then
            gv_Solicitud.EditIndex = -1
            gv_Solicitud.DataSource = fn_AgregarFila("Id_Consumible,Clave,Descripcion,Cantidad,Cantidad_Ant", InsumosAgregar)
            gv_Solicitud.DataBind()
        Else
            Call MuestraGridSolicitudVacio()
            btn_Guardar.Enabled = False
        End If
    End Sub

    Protected Sub gv_Solicitud_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_Solicitud.RowEditing
        If gv_Solicitud.DataKeys(0).Values("Id_Consumible").ToString = "" Then Exit Sub
        Call CancelarEdicionGridSolicitud()

        gv_Solicitud.EditIndex = e.NewEditIndex
        gv_Solicitud.DataSource = fn_AgregarFila("Id_Consumible,Clave,Descripcion,Cantidad,Cantidad_Ant", InsumosAgregados_Solicitud(False))
        gv_Solicitud.DataBind()
    End Sub

    Protected Sub gv_Solicitud_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gv_Solicitud.RowCancelingEdit
        Call CancelarEdicionGridSolicitud()
    End Sub

    Protected Sub gv_Solicitud_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gv_Solicitud.RowUpdating
        Dim InsumosAgregar As String = InsumosAgregados_Solicitud(True, e.RowIndex)
        If InsumosAgregar <> "" Then
            gv_Solicitud.EditIndex = -1
            gv_Solicitud.DataSource = fn_AgregarFila("Id_Consumible,Clave,Descripcion,Cantidad,Cantidad_Ant", InsumosAgregar)
            gv_Solicitud.DataBind()
        Else
            e.Cancel = True
        End If
    End Sub

    Private Function InsumosAgregados_Solicitud(ByVal Editado As Boolean, Optional ByVal Fila_Edit As Integer = -1, Optional ByVal Fila_Eliminar As Integer = -1) As String
        'Editado = True significa que se deberá de tomar en cuenta el valor del textbox
        'Editado = False significa que se deberá de tomar en cuenta el valor del label
        Dim tbx_Cantidad As TextBox
        Dim lbl_Cantidad As Label
        Dim InsumosAgregar As String = ""

        For Each Row As GridViewRow In gv_Solicitud.Rows
            If Editado Then
                If Fila_Edit = Row.RowIndex Then
                    tbx_Cantidad = Row.Cells(4).FindControl("tbx_CantidadSolicitada")
                    If Val(tbx_Cantidad.Text) = 0 Then
                        fn_Alerta("Debe de Indicar una cantidad para el Insumo o en su defecto eliminarlo.")
                        Return ""
                    End If
                    InsumosAgregar &= gv_Solicitud.DataKeys(Row.RowIndex).Value & "," &
                                  gv_Solicitud.DataKeys(Row.RowIndex).Values("Clave") & "," &
                                  Server.HtmlDecode(gv_Solicitud.DataKeys(Row.RowIndex).Values("Descripcion")) & "," &
                                  tbx_Cantidad.Text & "," & tbx_Cantidad.Text & ";"
                Else
                    InsumosAgregar &= gv_Solicitud.DataKeys(Row.RowIndex).Value & "," &
                                  gv_Solicitud.DataKeys(Row.RowIndex).Values("Clave") & "," &
                                  Server.HtmlDecode(gv_Solicitud.DataKeys(Row.RowIndex).Values("Descripcion")) & "," &
                                  gv_Solicitud.DataKeys(Row.RowIndex).Values("Cantidad_Ant") & "," &
                                  gv_Solicitud.DataKeys(Row.RowIndex).Values("Cantidad_Ant") & ";"
                End If
            Else
                If Fila_Eliminar = Row.RowIndex Then
                    'Cuando se quiere eliminar una fila
                    Continue For
                ElseIf Fila_Edit = Row.RowIndex Then
                    'Cuando se quiere cancelar la edición pero dejar el valor anterior
                    InsumosAgregar &= gv_Solicitud.DataKeys(Row.RowIndex).Value & "," &
                                      gv_Solicitud.DataKeys(Row.RowIndex).Values("Clave") & "," &
                                      Server.HtmlDecode(gv_Solicitud.DataKeys(Row.RowIndex).Values("Descripcion")) & "," &
                                      gv_Solicitud.DataKeys(Row.RowIndex).Values("Cantidad_Ant") & "," &
                                      gv_Solicitud.DataKeys(Row.RowIndex).Values("Cantidad_Ant") & ";"
                Else
                    'Cuando se agrega normalmente
                    lbl_Cantidad = Row.Cells(4).FindControl("lbl_CantidadSolicitada")
                    InsumosAgregar &= gv_Solicitud.DataKeys(Row.RowIndex).Value & "," &
                                      gv_Solicitud.DataKeys(Row.RowIndex).Values("Clave") & "," &
                                      Server.HtmlDecode(gv_Solicitud.DataKeys(Row.RowIndex).Values("Descripcion")) & "," &
                                      lbl_Cantidad.Text & "," &
                                      gv_Solicitud.DataKeys(Row.RowIndex).Values("Cantidad_Ant") & ";"
                End If
            End If
        Next
        If InsumosAgregar <> "" Then InsumosAgregar = InsumosAgregar.Substring(0, (InsumosAgregar.Length - 1))
        Return InsumosAgregar
    End Function

#End Region

#Region "Consulta"

    Private Sub MuestraGridConsultaVacio()
        gv_Solicitudes.SelectedIndex = -1
        gv_Solicitudes.DataSource = fn_CreaGridVacio("Id_Solicitud,Numero,Fecha,Hora,UsuarioSolicita,Status")
        gv_Solicitudes.DataBind()
    End Sub

    Private Sub MuestraGridDetalleVacio()
        gv_Detalle.SelectedIndex = -1
        gv_Detalle.DataSource = fn_CreaGridVacio("Id_Consumible,Clave,Descripcion,CantidadSolicitada,CantidadValidada,CantidadSurtida")
        gv_Detalle.DataBind()
    End Sub

    Protected Sub cbx_Status_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbx_Status.CheckedChanged
        Call MuestraGridConsultaVacio()
        Call MuestraGridDetalleVacio()
        txt_ObservacionesD.Text = ""
        If cbx_Status.Checked Then
            ddl_Status.SelectedValue = 0
            ddl_Status.Enabled = False
        Else
            ddl_Status.Enabled = True
        End If
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Mostrar.Click
        Call LlenaSolicitudes()
    End Sub

    Protected Sub ddl_Status_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Status.SelectedIndexChanged
        Call MuestraGridConsultaVacio()
        Call MuestraGridDetalleVacio()
    End Sub

    Protected Sub tbx_FechaIni_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbx_FechaIni.TextChanged
        Call MuestraGridConsultaVacio()
        Call MuestraGridDetalleVacio()
        txt_ObservacionesD.Text = ""
    End Sub

    Protected Sub tbx_FechaFin_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbx_FechaFin.TextChanged
        Call MuestraGridConsultaVacio()
        Call MuestraGridDetalleVacio()
        txt_ObservacionesD.Text = ""
        btn_Mostrar.Focus()
    End Sub

    Protected Sub gv_Solicitudes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_Solicitudes.SelectedIndexChanged
        If gv_Solicitudes.DataKeys(0).Values("Id_Solicitud").ToString = "" Then
            gv_Solicitudes.SelectedIndex = -1
            Exit Sub
        End If
        gv_Detalle.DataSource = fn_Insumos_ObtenerSolicitudesDetalle(Session("SucursalID"), Session("UsuarioID"), gv_Solicitudes.SelectedDataKey("Id_Solicitud"))
        gv_Detalle.DataBind()
        txt_ObservacionesD.Text = fn_Insumos_Read(gv_Solicitudes.SelectedDataKey("Id_Solicitud"))
    End Sub

    Protected Sub gv_Solicitudes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Solicitudes.PageIndexChanging

        Call MuestraGridConsultaVacio()
        Call MuestraGridDetalleVacio()

        If cbx_Status.Checked Then
            valorStatus = "T"
        Else
            valorStatus = ddl_Status.SelectedValue
        End If

        gv_Solicitudes.PageIndex = e.NewPageIndex
        gv_Solicitudes.DataSource = fn_Insumos_ObtenerSolicitudes(Session("SucursalID"), Session("UsuarioID"), CDate(tbx_FechaIni.Text).ToShortDateString, CDate(tbx_FechaFin.Text).ToShortDateString, valorStatus)
        gv_Solicitudes.DataBind()

    End Sub

    Protected Sub gv_Solicitudes_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Solicitudes.RowDeleting
        If gv_Solicitudes.DataKeys(0).Values("Id_Solicitud").ToString = "" OrElse gv_Solicitudes.Rows(e.RowIndex).Cells(7).Text = "CANCELADA" Then Exit Sub
        Dim Indice As Integer = e.RowIndex
        gv_Solicitudes.SelectedIndex = Indice

        Dim Id_Solicitud As Integer = gv_Solicitudes.DataKeys(Indice).Value

        If Not fn_Insumos_Cancelar(Id_Solicitud, Session("SucursalID"), Session("UsuarioID"), Session("EstacioN")) Then
            fn_Alerta("No se pudo Cancelar la Solicitud.")
            Exit Sub
        End If

        Call LlenaSolicitudes()

        Call EnviaMailCancelacionInsumo(Id_Solicitud, gv_Solicitudes.SelectedRow.Cells(3).Text)

    End Sub

    Private Sub LlenaSolicitudes()

        If tbx_FechaIni.Text = "" Then
            fn_Alerta("Seleccione la Fecha Inicio.")
            Exit Sub
        End If

        If tbx_FechaFin.Text = "" Then
            fn_Alerta("Seleccione la Fecha Fin.")
            Exit Sub
        End If

        If CDate(tbx_FechaIni.Text) > CDate(tbx_FechaFin.Text) Then
            fn_Alerta("La fecha inicial debe se menor a la fecha final.")
            Exit Sub
        End If

        If ddl_Status.SelectedIndex = 0 And Not cbx_Status.Checked Then
            fn_Alerta("Seleccione el Status.")
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
        'editado 22/nov/2012
        If dt.Rows.Count > 0 Then
            gv_Solicitudes.DataSource = dt
            gv_Solicitudes.DataBind()
        Else
            gv_Solicitudes.DataSource = fn_CreaGridVacio("Id_Solicitud,Numero,Fecha,Hora,UsuarioSolicita,Status")
            gv_Solicitudes.DataBind()
        End If
    End Sub

    Private Function EnviaMailCancelacionInsumo(ByVal SolicitudId As Integer, ByVal NumSolicitud As Integer) As Boolean

        Dim dt_Detalle As DataTable = fn_Insumos_LeerDetalleSolicitud(Session("SucursalID"), Session("UsuarioID"), SolicitudId)

        'Aquí se inserta la Alerta de Solicitud de Insumos
        Dim Detalles As String = "Número de Solicitud: " & NumSolicitud & Chr(13) _
                                & "    Usuario Cancela: " & Session("UsuarioNombre") & Chr(13) _
                                & "           Sucursal: " & Session("SucursalN") & Chr(13) _
                                & "       Departamento: " & Session("DeptoNombre") & Chr(13) _
                                & "    Fecha Solicitud: " & Now.ToShortDateString & " - " & Now.ToShortTimeString & Chr(13) _
                                & "      Observaciones: " & txt_Observaciones.Text.ToUpper

        Dim DetalleHTML As String
        If dt_Detalle IsNot Nothing Then
            DetalleHTML = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                          & "<tr><td colspan='4' align='center'>CANCELACION DE INSUMOS</td></tr>" _
                          & "<tr><td colspan='4'><br><hr /></td></tr>" _
                          & "<tr><td align='right'><label><b>Número de Solicitud:</b></label></td><td>" & NumSolicitud & "</td><td></td><td></td></tr>" _
                          & "<tr><td align='right'><label><b>Usuario Cancela:</b></label></td><td>" & Session("UsuarioNombre") & "</td></tr>" _
                          & "<tr><td align='right'><label><b>Sucursal:</b></label></td><td>" & Session("SucursalN") & "</td></tr>" _
                          & "<tr><td align='right'><label><b>Departamento:</b></label></td><td>" & Session("DeptoNombre") & "</td></tr>" _
                          & "<tr><td align='right'><label><b>Fecha Solicitud:</b></label></td><td>" & Now.ToShortDateString & " - " & Now.ToShortTimeString & "<br></td></tr>" _
                          & "<tr><td align='right'><label><b>Observaciones:</b></label></td><td>" & txt_Observaciones.Text.ToUpper & "<br></td></tr>" _
                          & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>Agente de Servicios SIAC</td></tr></table><br/><br/></body></html>" _
                          & fn_DatatableToHTML(dt_Detalle, "DETALLE", 1, 3)
        Else
            DetalleHTML = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                          & "<tr><td colspan='4' align='center'>CANCELACION DE INSUMOS</td></tr>" _
                          & "<tr><td colspan='4'><br><hr /></td></tr>" _
                          & "<tr><td align='right'><label><b>Número de Solicitud:</b></label></td><td>" & NumSolicitud & "</td><td></td><td></td></tr>" _
                          & "<tr><td align='right'><label><b>Usuario Cancela:</b></label></td><td>" & Session("UsuarioNombre") & "</td></tr>" _
                          & "<tr><td align='right'><label><b>Sucursal:</b></label></td><td>" & Session("SucursalN") & "</td></tr>" _
                          & "<tr><td align='right'><label><b>Departamento:</b></label></td><td>" & Session("DeptoNombre") & "</td></tr>" _
                          & "<tr><td align='right'><label><b>Fecha Solicitud:</b></label></td><td>" & Now.ToShortDateString & " - " & Now.ToShortTimeString & "<br></td></tr>" _
                          & "<tr><td align='right'><label><b>Observaciones:</b></label></td><td>" & txt_Observaciones.Text.ToUpper & "<br></td></tr>" _
                          & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>Agente de Servicios SIAC</td></tr></table><br/><br/></body></html>"
        End If

        Call ObtenerDestinos(Detalles, DetalleHTML, "CANCELACION DE INSUMOS")

    End Function

    Private Function ObtenerDestinos(ByVal Detalles As String, ByVal DetalleHTML As String, ByVal Cabezera As String) As Boolean
        If fn_AlertasGeneradas_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), "22", Detalles) Then
            'Obtener los Destinos
            Dim Dt_Destinos As DataTable = fn_AlertasGeneradas_ObtenerMails("22")
            If Dt_Destinos IsNot Nothing Then
                For Each renglon As DataRow In Dt_Destinos.Rows
                    If fn_ValidarMail(renglon("Mail")) Then Cn_Mail.fn_Enviar_MailHTML(renglon("Mail"), Cabezera, DetalleHTML, "", Session("SucursalID"))
                Next
            End If
        End If
    End Function

#End Region

End Class
