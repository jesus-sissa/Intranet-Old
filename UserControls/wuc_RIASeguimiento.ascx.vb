
Imports SISSAIntranet.Cn_Soporte
Imports SISSAIntranet.FuncionesGlobales
Imports System.Data
Imports SISSAIntranet.Cn_Mail

Partial Class UserControls_wuc_RIASeguimiento
    Inherits System.Web.UI.UserControl

    Dim Status As String = "I"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Page.Title = "SEGUIMIENTO DE REPORTES DE INCIDENTES/ACCIDENTES"

        '-----------------------------------------
        'Aquí se valida que el Usuario firmado tenga realmente Privilegios para acceder a esta página
        'o se esta introduciendo la página directo en la barra de direcciones

        Dim arr As String() = Split(Session("CadenaPriv"), ";")

        For x As Integer = 0 To arr.Length - 1
            If arr(x) = "SEGUIMIENTO RIA" Then
                Exit For
            ElseIf x = (arr.Length - 1) Then
                MostrarAlertAjax("NO TIENE PRIVILEGIOS PARA ACCEDER A ESTA OPCION", Me, Page)
                Response.Redirect("frm_login.aspx")
                Exit Sub
            End If
        Next

        '-------------------------------------------

        MuestraGridsVacios()
        LlenarGridRIA()

        Dim ds As Data.DataTable = fn_IncidentesAccidentes_GetEmpleados(Session("SucursalID"), Session("UsuarioID"))
        Dim row As Data.DataRow = ds.NewRow

        ddl_UsuarioA.DataTextField = "Nombre"
        ddl_UsuarioA.DataValueField = "Id_Empleado"

        row(ddl_UsuarioA.DataTextField) = "Seleccione..."
        row(ddl_UsuarioA.DataValueField) = 0
        row("Clave_Empleado") = 0
        ds.Rows.InsertAt(row, 0)
        ddl_UsuarioA.DataSource = ds
        ddl_UsuarioA.DataBind()

        Session("IndiceAnterior") = 0
        Session("PaginaAnterior") = 0

        Dim xs As String

        For x As Integer = 23 To 0 Step -1
            If x < 10 Then
                xs = "0" & x.ToString
            Else
                xs = x.ToString
            End If
            ddl_HoraSeguimiento.Items.Insert(0, xs)
        Next

        For x As Integer = 59 To 0 Step -1
            If x < 10 Then
                xs = "0" & x.ToString
            Else
                xs = x.ToString
            End If
            ddl_MinSeguimiento.Items.Insert(0, xs)
        Next

    End Sub

    Sub MuestraGridsVacios()
        pnl_AgregarI.Enabled = False
        pnl_Comentarios.Enabled = False
        btn_Subir.Enabled = False
        btn_Finalizar.Enabled = False

        gv_RIA.SelectedIndex = -1
        gv_RIA.DataSource = fn_CreaGridVacio("Id_RIA,Numero,Sucursal,Fecha,Hora,Tipo,Entidad,Status")
        gv_RIA.DataBind()

        gv_Imagenes.DataSource = Nothing
        gv_Imagenes.DataBind()

        gv_Usuarios.DataSource = fn_CreaGridVacio("Id_Entidad,Nombre")
        gv_Usuarios.DataBind()

        lbl_UsuarioA.Enabled = False
        ddl_UsuarioA.Enabled = False
        btn_Asignar.Enabled = False
        lbl_TipoUsuario.Enabled = False
        rdb_Principal.Enabled = False
        rdb_Secundario.Enabled = False

        LimpiarComentarios()
    End Sub

    Sub LimpiarComentarios()
        ddl_UsuarioA.SelectedValue = 0
        tbx_FechaSeguimiento.Text = ""
        ddl_HoraSeguimiento.SelectedValue = "00"
        ddl_MinSeguimiento.SelectedValue = "00"
        tbx_Descripcion.Text = ""
        FileUpload1.Dispose()
        rdb_Principal.Checked = False
        rdb_Secundario.Checked = False
    End Sub

    Sub LlenarGridRIA()

        Dim dt As DataTable = fn_SeguimientoRIA_LlenarLista(Session("SucursalID"), Session("UsuarioID"))

        If dt IsNot Nothing Then
            gv_RIA.DataSource = dt
            gv_RIA.DataBind()
        Else
            gv_RIA.DataSource = fn_CreaGridVacio("Id_RIA,Numero,Sucursal,Fecha,Hora,Tipo,Entidad,Status")
            gv_RIA.DataBind()
        End If

    End Sub

    Sub LlenarGridUsuarios()
        Dim dt As DataTable = fn_ConsultaRIA_ObtenerUsuarios(gv_RIA.SelectedDataKey.Values("Id_RIA"), Session("SucursalID"), Session("UsuarioID"))

        If dt IsNot Nothing Then
            gv_Usuarios.DataSource = dt
            gv_Usuarios.DataBind()
        Else
            gv_Usuarios.DataSource = fn_CreaGridVacio("Id_Entidad,Nombre")
            gv_Usuarios.DataBind()
        End If

    End Sub

    Sub LlenarGridDetalle()
        Dim dt As DataTable = fn_ConsultaRIA_LlenarDetalle(gv_RIA.SelectedDataKey.Values("Id_RIA"), Session("SucursalID"), Session("UsuarioID"))

        If dt IsNot Nothing Then
            For Each Seg As DataRow In dt.Rows

                Dim lbl_Encabezado As New System.Web.UI.WebControls.Label()
                lbl_Encabezado.Width = Unit.Percentage(100)
                'lbl_Encabezado.Height = 20
                lbl_Encabezado.BorderStyle = BorderStyle.Solid
                lbl_Encabezado.BorderWidth = 1
                lbl_Encabezado.BorderColor = System.Drawing.Color.FromName("#C0A062")
                lbl_Encabezado.BackColor = System.Drawing.Color.FromName("#C0A062") 'System.Drawing.Color.FromName("#efefef")
                lbl_Encabezado.Text = Seg.Item("Fecha") & " - " & Seg.Item("Entidad")
                lbl_Encabezado.Font.Name = "Verdana"
                lbl_Encabezado.Font.Size = 8
                lbl_Encabezado.Font.Bold = True
                PlaceHolderSeguimiento.Controls.Add(lbl_Encabezado)

                PlaceHolderSeguimiento.Controls.Add(New LiteralControl("<br />"))
                PlaceHolderSeguimiento.Controls.Add(New LiteralControl("<br />"))

                Dim lbl_Seguimiento As New System.Web.UI.WebControls.Label()
                'lbl_Seguimiento.Width = 823
                'lbl_Seguimiento.Height = 100
                'lbl_Seguimiento.BorderWidth = 1
                'lbl_Seguimiento.BorderStyle = BorderStyle.Solid
                'lbl_Seguimiento.BorderColor = System.Drawing.Color.FromName("#C0A062")
                'lbl_Seguimiento.BackColor = Drawing.Color.Cornsilk
                lbl_Seguimiento.Text = Seg.Item("Descripcion")
                lbl_Seguimiento.Font.Size = 8
                PlaceHolderSeguimiento.Controls.Add(lbl_Seguimiento)

                PlaceHolderSeguimiento.Controls.Add(New LiteralControl("<br />"))
                PlaceHolderSeguimiento.Controls.Add(New LiteralControl("<br />"))
                PlaceHolderSeguimiento.Controls.Add(New LiteralControl("<hr />"))
                PlaceHolderSeguimiento.Controls.Add(New LiteralControl("<br />"))

                'PlaceHolderSeguimiento.Controls.Add(New LiteralControl("<fieldset />"))
                'PlaceHolderSeguimiento.Controls.AddAt(0, lbl_Seguimiento)

            Next
        End If

    End Sub

    Protected Sub gv_RIA_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_RIA.SelectedIndexChanged
        If gv_RIA.SelectedDataKey.Values("Status") = "" Then
            gv_RIA.SelectedRowStyle.ForeColor = Drawing.Color.Black
            gv_RIA.SelectedRowStyle.Font.Bold = False
            gv_RIA.SelectedRowStyle.BackColor = System.Drawing.Color.White
            Exit Sub
        End If
        gv_RIA.SelectedRowStyle.BackColor = System.Drawing.Color.FromName("#C0A062")
        gv_RIA.SelectedRowStyle.ForeColor = Drawing.Color.White
        gv_RIA.SelectedRowStyle.Font.Bold = True

        pnl_AgregarI.Enabled = True
        pnl_Comentarios.Enabled = True

        btn_Subir.Enabled = True
        If Session("JefeArea") = "S" Then
            btn_Finalizar.Enabled = True
        End If
        lbl_UsuarioA.Enabled = True
        ddl_UsuarioA.Enabled = True
        btn_Asignar.Enabled = True
        lbl_TipoUsuario.Enabled = True
        rdb_Principal.Enabled = True
        rdb_Secundario.Enabled = True

        LlenarGridDetalle()

        gv_Imagenes.DataSource = fn_IncidentesAccidentes_LeerImagenes(gv_RIA.SelectedDataKey.Values("Id_RIA"), Session("SucursalID"), Session("UsuarioID"))
        gv_Imagenes.DataBind()

        LlenarGridUsuarios()

        Session("IndiceAnterior") = gv_RIA.SelectedIndex

    End Sub

    Protected Sub gv_RIA_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_RIA.PageIndexChanging
        MuestraGridsVacios()
        gv_Imagenes.DataSource = Nothing
        gv_Imagenes.DataBind()
        gv_RIA.PageIndex = e.NewPageIndex
        gv_RIA.DataSource = fn_SeguimientoRIA_LlenarLista(Session("SucursalID"), Session("UsuarioID"))
        gv_RIA.DataBind()
        LimpiarComentarios()
    End Sub

    Protected Sub gv_RIA_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_RIA.PageIndexChanged
        Session("PaginaAnterior") = gv_RIA.PageIndex
    End Sub

    Protected Sub gv_RIA_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_RIA.RowCreated
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

    Protected Sub btn_Asignar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Asignar.Click
        Dim TipoUsuario As Integer = 0
        If ddl_UsuarioA.SelectedValue = 0 Then
            MostrarAlertAjax("Seleccione el Usuario a Asignar.", btn_Asignar, Page)
            ddl_UsuarioA.Focus()
            LlenarGridDetalle()
            Exit Sub
        End If

        If Not rdb_Principal.Checked And Not rdb_Secundario.Checked Then
            MostrarAlertAjax("Seleccione el Tipo de Usuario.", btn_Asignar, Page)
            LlenarGridDetalle()
            Exit Sub
        ElseIf rdb_Principal.Checked Then
            TipoUsuario = 1
        Else
            TipoUsuario = 2
        End If

        For Each fila As GridViewRow In gv_Usuarios.Rows
            If gv_Usuarios.DataKeys(fila.RowIndex).Values("Id_Entidad") = ddl_UsuarioA.SelectedValue Then
                MostrarAlertAjax("Elemento seleccionado ya existe en la lista.", btn_Asignar, Page)
                gv_RIA.SelectedRowStyle.BackColor = System.Drawing.Color.FromName("#C0A062")
                gv_RIA.SelectedRowStyle.ForeColor = Drawing.Color.White
                gv_RIA.SelectedRowStyle.Font.Bold = True
                ddl_UsuarioA.Focus()
                LlenarGridDetalle()
                Exit Sub
            End If
        Next

        Dim Descripcion As String
        Dim DescripcionMail As String

        Dim FechaActual As Date = Now.Date
        Dim HoraActual As String = TimeString

        Descripcion = "NUEVO USUARIO PARA SEGUIMIENTO: " & ddl_UsuarioA.SelectedItem.Text
        DescripcionMail = "ASIGNACION DE RIA PARA SEGUIMIENTO"

        If fn_ConsultaRIA_GuardarD(Session("SucursalID"), gv_RIA.SelectedDataKey.Values("Id_RIA"), Session("UsuarioID"), ddl_UsuarioA.SelectedValue, Session("EstacioN"), Descripcion, FechaActual, HoraActual) Then
            'MostrarAlertAjax("Los datos se han guardado correctamente.", btn_Asignar, Page)
        Else
            MostrarAlertAjax("Ha ocurrido un error al intentar guardar la información.", btn_Asignar, Page)
            Exit Sub
        End If

        If Not fn_IncidentesAccidentes_GuardarUsuario(gv_RIA.SelectedDataKey.Values("Id_RIA"), ddl_UsuarioA.SelectedValue, Session("UsuarioID"), Session("EstacioN"), Session("SucursalID"), TipoUsuario) Then
            MostrarAlertAjax("Ha ocurrido un error al intentar Agregar el Usuario.", btn_Asignar, Page)
            Exit Sub
        End If

        Dim tipo As String = gv_RIA.SelectedRow.Cells(6).Text
        Dim entidad As String = gv_RIA.SelectedRow.Cells(7).Text
        Dim fecha As String = gv_RIA.SelectedRow.Cells(4).Text
        Dim hora As String = gv_RIA.SelectedRow.Cells(5).Text

        'Se consulta los datos del RIA para obtener el Número de RIA
        Dim dr_Reporte As DataRow = fn_IncidentesAccidentes_ObtenerDatos(gv_RIA.SelectedDataKey.Values("Id_RIA"), Session("SucursalID"), Session("UsuarioID"))

        If dr_Reporte Is Nothing Then
            MostrarAlertAjax("Ha ocurrido un error al intentar obtener los datos. ", btn_Asignar, Page)
            Exit Sub
        End If

        Dim DetalleMail As String = "   REPORTE DE INCIDENTES/ACCIDENTES " & Chr(13) & Chr(13) _
                                  & "           Número de Reporte: " & dr_Reporte("Numero_RIA") & Chr(13) _
                                  & "                        Tipo: " & tipo & Chr(13) _
                                  & "                     Entidad: " & entidad & Chr(13) _
                                  & "            Usuario Registro: " & Session("UsuarioNombre") & Chr(13) _
                                  & "                   Fecha RIA: " & fecha & Chr(13) _
                                  & "                    Hora RIA: " & hora & Chr(13) _
                                  & "              Fecha Registro: " & Now.Date & Chr(13) _
                                  & "               Hora Registro: " & Now.ToShortTimeString & Chr(13) _
                                  & "                 Descripción: " & DescripcionMail & Chr(13) & Chr(13) _
                                  & "Agente de Servicios SIAC."

        Dim dr As DataRow = fn_Empleados_ObtenValores(Session("SucursalID"), ddl_UsuarioA.SelectedValue, Session("UsuarioID"))

        If dr("Mail") <> "" Then
            'If Not fn_Enviar_Mail("jfnuncio@hotmail.com", "REPORTE DE INCIDENTES/ACCIDENTES", DetalleMail, Session("SucursalID")) Then
            '    MostrarAlertAjax("Ha ocurrido un error al intentar enviar los Correos.", btn_Asignar, Page)
            '    Exit Sub
            'End If
            If Not fn_Enviar_Mail(dr("Mail"), "REPORTE DE INCIDENTES/ACCIDENTES", DetalleMail, Session("SucursalID")) Then
                MostrarAlertAjax("Ha ocurrido un error al intentar enviar el Correo.", btn_Asignar, Page)
                Exit Sub
            End If
        End If

        ddl_UsuarioA.SelectedValue = 0
        tbx_Descripcion.Text = ""

        'MuestraGridsVacios()
        LlenarGridRIA()

        gv_RIA.SelectedIndex = Session("IndiceAnterior")
        gv_RIA.PageIndex = Session("PaginaAnterior")
        gv_RIA_SelectedIndexChanged(sender, e)

    End Sub

    Protected Sub btn_Subir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Subir.Click, btn_Finalizar.Click

        If tbx_FechaSeguimiento.Text = "" Then
            MostrarAlertAjax("Capture la Fecha del Evento de Seguimiento.", btn_Subir, Page)
            tbx_FechaSeguimiento.Focus()
            Exit Sub
        End If
        Dim FechaSeguimiento As String = fn_Fecha102(tbx_FechaSeguimiento.Text)

        If ddl_HoraSeguimiento.SelectedValue = "00" And ddl_MinSeguimiento.SelectedValue = "00" Then
            MostrarAlertAjax("Seleccione la Hora del Evento de Seguimiento.", btn_Subir, Page)
            ddl_HoraSeguimiento.Focus()
            Exit Sub
        End If

        Dim HoraSeguimiento As String = ddl_HoraSeguimiento.SelectedValue & ":" & ddl_MinSeguimiento.SelectedValue

        If tbx_Descripcion.Text = "" Then
            MostrarAlertAjax("Capture la Descripción.", btn_Subir, Page)
            tbx_Descripcion.Focus()
            gv_RIA.SelectedRowStyle.BackColor = System.Drawing.Color.FromName("#C0A062")
            gv_RIA.SelectedRowStyle.ForeColor = Drawing.Color.White
            gv_RIA.SelectedRowStyle.Font.Bold = True
            Exit Sub
        End If

        'Aquí se inserta la imagen en caso de que se haya agregado
        If (FileUpload1.HasFile) Then

            ' Obtener el nombre del archivo a subir.
            Dim fileName As String = Server.HtmlEncode(FileUpload1.FileName)

            ' Obtener la extensión de el archivo cargado.
            Dim extension As String = System.IO.Path.GetExtension(fileName)

            If (extension = ".jpg") Or (extension = ".jpg") Then

                Dim imageSize As Byte() = New Byte(FileUpload1.PostedFile.ContentLength - 1) {}

                If imageSize.LongLength > 300000 Then
                    MostrarAlertAjax("La Imagen debe ser menor de 300 kb.", btn_Subir, Page)
                    FileUpload1.Focus()
                    gv_RIA.SelectedRowStyle.BackColor = System.Drawing.Color.FromName("#C0A062")
                    gv_RIA.SelectedRowStyle.ForeColor = Drawing.Color.White
                    gv_RIA.SelectedRowStyle.Font.Bold = True
                    Exit Sub
                End If

            Else
                MostrarAlertAjax("El Archivo debe tener las extensiones (.jpg o .png).", btn_Subir, Page)
                FileUpload1.Focus()
                gv_RIA.SelectedRowStyle.BackColor = System.Drawing.Color.FromName("#C0A062")
                gv_RIA.SelectedRowStyle.ForeColor = Drawing.Color.White
                gv_RIA.SelectedRowStyle.Font.Bold = True
                Exit Sub
            End If

        End If

        If sender Is btn_Finalizar Then
            Status = "V"
        End If

        Dim Id_RIAD As Integer = fn_SeguimientoRIA_Guardar(Session("SucursalID"), Session("UsuarioID"), gv_RIA.SelectedDataKey.Values("Id_RIA"), Session("EstacioN"), tbx_Descripcion.Text.ToUpper, Status, FechaSeguimiento, HoraSeguimiento)

        If Id_RIAD = 0 Then
            MostrarAlertAjax("Ha ocurrido un error al intentar guardar los datos.", btn_Subir, Page)
            Exit Sub
        Else
            'MostrarAlertAjax("Los datos han sido guardados correctamente.", btn_Guardar, Page)
        End If

        hfd_IDRIAD.Value = Id_RIAD

        tbx_Descripcion.Text = ""

        gv_RIA.SelectedRowStyle.BackColor = System.Drawing.Color.FromName("#C0A062")
        gv_RIA.SelectedRowStyle.ForeColor = Drawing.Color.White
        gv_RIA.SelectedRowStyle.Font.Bold = True

        'Aquí se inserta la imagen en caso de que se haya agregado
        If (FileUpload1.HasFile) Then

            Dim imageSize2 As Byte() = New Byte(FileUpload1.PostedFile.ContentLength - 1) {}

            Dim Imagen As HttpPostedFile = FileUpload1.PostedFile

            Imagen.InputStream.Read(imageSize2, 0, CInt(FileUpload1.PostedFile.ContentLength))

            If Not fn_IncidentesAccidentes_GuardarImagenes(Session("SucursalID"), Session("UsuarioID"), gv_RIA.SelectedDataKey.Values("Id_RIA"), imageSize2, "", hfd_IDRIAD.Value) Then
                MostrarAlertAjax("Ha ocurrido un error al intentar guardar la Imagen.", btn_Subir, Page)
                Exit Sub
            End If

        End If

        If sender Is btn_Finalizar Then
            MuestraGridsVacios()
            gv_Imagenes.DataSource = Nothing
            gv_Imagenes.DataBind()
            LlenarGridRIA()
        Else
            LlenarGridRIA()
            'gv_Imagenes.DataSource = fn_IncidentesAccidentes_LeerImagenes(gv_RIA.SelectedDataKey.Values("Id_RIA"))
            'gv_Imagenes.DataBind()
            gv_RIA.SelectedIndex = Session("IndiceAnterior")
            gv_RIA.PageIndex = Session("PaginaAnterior")
            gv_RIA_SelectedIndexChanged(sender, e)
            'LlenarGridDetalle()
        End If

    End Sub

End Class
