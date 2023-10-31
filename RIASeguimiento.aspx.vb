Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data
Imports IntranetSIAC.Cn_Mail
Imports System.IO

Partial Class RIASeguimiento
    Inherits BasePage
    Dim Status As String = "I"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Call MuestraGridsVacios()
        Call LlenarGridRIA()

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

        Dim Columnas As Byte
        Dim Gv_Nombres() As GridView = {gv_RIA, gv_Usuarios}

        For j As Byte = 0 To Gv_Nombres.Length - 1
            Columnas = Gv_Nombres(j).Columns.Count

            For i As Byte = 0 To Columnas - 1
                Gv_Nombres(j).Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
            Next
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

        gv_Usuarios.DataSource = fn_CreaGridVacio("Id_Entidad,Nombre")
        gv_Usuarios.DataBind()

        lbl_UsuarioA.Enabled = False
        ddl_UsuarioA.Enabled = False
        btn_Asignar.Enabled = False
        lbl_TipoUsuario.Enabled = False
        rdb_Principal.Enabled = False
        rdb_Secundario.Enabled = False

        Call LimpiarComentarios()
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

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            gv_RIA.DataSource = dt
            gv_RIA.DataBind()
        Else
            gv_RIA.DataSource = fn_CreaGridVacio("Id_RIA,Numero,Sucursal,Fecha,Hora,Tipo,Entidad,Status")
            gv_RIA.DataBind()
        End If

    End Sub

    Sub LlenarGridUsuarios()
        Dim dt As DataTable = fn_ConsultaRIA_ObtenerUsuarios(gv_RIA.SelectedDataKey.Values("Id_RIA"), Session("SucursalID"), Session("UsuarioID"))

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            gv_Usuarios.DataSource = dt
            gv_Usuarios.DataBind()
        Else
            gv_Usuarios.DataSource = fn_CreaGridVacio("Id_Entidad,Nombre")
            gv_Usuarios.DataBind()
        End If

    End Sub

    Sub LlenarGridDetalle()
        Dim dt As DataTable = fn_ConsultaRIA_LlenarDetalle(gv_RIA.SelectedDataKey.Values("Id_RIA"), Session("SucursalID"), Session("UsuarioID"))
        Dim Id_RiaImagen As String = ""
        Dim ID_RIADANTERIOR As Integer = 0
        Dim Cuenta As Byte
        If dt IsNot Nothing Then

            For Each Seg As DataRow In dt.Rows
                If ID_RIADANTERIOR <> Seg.Item("Id_RIAD") Then
                    Cuenta = 1
                    Dim lbl_Encabezado As New System.Web.UI.WebControls.Label()
                    lbl_Encabezado.CssClass = "RiaSeguimiento"
                    lbl_Encabezado.Text = Seg.Item("Fecha") & " - " & Seg.Item("Entidad")
                    
                    PlaceHolderSeguimiento.Controls.Add(lbl_Encabezado)
                    PlaceHolderSeguimiento.Controls.Add(New LiteralControl("<br />"))

                    Dim lbl_Seguimiento As New System.Web.UI.WebControls.Label()
                    lbl_Seguimiento.Text = Seg.Item("Descripcion")
                    lbl_Seguimiento.Font.Size = 8

                    PlaceHolderSeguimiento.Controls.Add(lbl_Seguimiento)
                    PlaceHolderSeguimiento.Controls.Add(New LiteralControl("<br />"))
                End If

                Id_RiaImagen = Seg.Item("Id_RIAI")
                If Not IsDBNull(Seg.Item("Imagen")) Then
                    Dim LigaImagen As New System.Web.UI.WebControls.HyperLink
                    LigaImagen.Text = "Imagen_" & Cuenta

                    LigaImagen.NavigateUrl = "MostrarImagenes.aspx?ID= " + Id_RiaImagen + ""
                    LigaImagen.Attributes.Add("OnClick", "window.open(this.href, 'ViewImage', 'height=450, width=600,top=90,left=100,toolbar=no,menubar=no,location=no,resizable=no,maximized=no,scrollbars=no,status=no'); return false;")

                    PlaceHolderSeguimiento.Controls.Add(LigaImagen)
                    PlaceHolderSeguimiento.Controls.Add(New LiteralControl("<font size=1><br /></font>"))
                End If
                Cuenta += 1
                PlaceHolderSeguimiento.Controls.Add(New LiteralControl("<font size=1><hr /></font>")) 'Escribe Linea Horiz
                'PlaceHolderSeguimiento.Controls.Add(New LiteralControl("<fieldset />"))
                'PlaceHolderSeguimiento.Controls.AddAt(0, lbl_Seguimiento)
                ID_RIADANTERIOR = Seg.Item("Id_RIAD")
            Next
        End If

    End Sub

    Protected Sub gv_RIA_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_RIA.SelectedIndexChanged
        If gv_RIA.Rows(0).Cells(2).Text = "&nbsp;" Then Exit Sub

        pnl_AgregarI.Enabled = True
        pnl_Comentarios.Enabled = True

        btn_Subir.Enabled = True
        'Anteriormente sólo Jefe de Área podía finalizar un RIA (9-Oct-2012)
        btn_Finalizar.Enabled = True
        lbl_UsuarioA.Enabled = True
        ddl_UsuarioA.Enabled = True
        btn_Asignar.Enabled = True
        lbl_TipoUsuario.Enabled = True
        rdb_Principal.Enabled = True
        rdb_Secundario.Enabled = True

        Call LlenarGridDetalle()

        Call LlenarGridUsuarios()

        Session("IndiceAnterior") = gv_RIA.SelectedIndex
    End Sub

    Protected Sub gv_RIA_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_RIA.PageIndexChanging
        Call MuestraGridsVacios()
        gv_RIA.PageIndex = e.NewPageIndex
        gv_RIA.DataSource = fn_SeguimientoRIA_LlenarLista(Session("SucursalID"), Session("UsuarioID"))
        gv_RIA.DataBind()
        Call LimpiarComentarios()
    End Sub

    Protected Sub gv_RIA_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_RIA.PageIndexChanged
        Session("PaginaAnterior") = gv_RIA.PageIndex
    End Sub

    Protected Sub btn_Asignar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Asignar.Click
        Dim TipoUsuario As Integer = 0
        If ddl_UsuarioA.SelectedValue = 0 Then
               fn_Alerta("Seleccione el Usuario a Asignar.")
            ddl_UsuarioA.Focus()
            Call LlenarGridDetalle()
            Exit Sub
        End If

        If Not rdb_Principal.Checked And Not rdb_Secundario.Checked Then
             fn_Alerta("Seleccione el Tipo de Usuario.")
            Call LlenarGridDetalle()
            Exit Sub
        ElseIf rdb_Principal.Checked Then
            TipoUsuario = 1
        Else
            TipoUsuario = 2
        End If

        For Each fila As GridViewRow In gv_Usuarios.Rows
            If gv_Usuarios.DataKeys(fila.RowIndex).Values("Id_Entidad") = ddl_UsuarioA.SelectedValue Then
                fn_Alerta("Elemento seleccionado ya existe en la lista.")
                ddl_UsuarioA.Focus()
                Call LlenarGridDetalle()
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
             Else
            fn_Alerta("Ha ocurrido un error al intentar guardar la información.")
            Exit Sub
        End If

        If Not fn_IncidentesAccidentes_GuardarUsuario(gv_RIA.SelectedDataKey.Values("Id_RIA"), ddl_UsuarioA.SelectedValue, Session("UsuarioID"), Session("EstacioN"), Session("SucursalID"), TipoUsuario) Then
            fn_Alerta("Ha ocurrido un error al intentar Agregar el Usuario.")
            Exit Sub
        End If

        Dim tipo As String = gv_RIA.SelectedRow.Cells(6).Text
        Dim entidad As String = gv_RIA.SelectedRow.Cells(7).Text
        Dim fecha As String = gv_RIA.SelectedRow.Cells(4).Text
        Dim hora As String = gv_RIA.SelectedRow.Cells(5).Text

        'Se consulta los datos del RIA para obtener el Número de RIA
        Dim dr_Reporte As DataRow = fn_IncidentesAccidentes_ObtenerDatos(gv_RIA.SelectedDataKey.Values("Id_RIA"), Session("SucursalID"), Session("UsuarioID"))

        If dr_Reporte Is Nothing Then
            fn_Alerta("Ha ocurrido un error al intentar obtener los datos.")
            Exit Sub
        End If

        Dim Pie As String = "Agente de Servicios SIAC " & Now.Year.ToString
        Dim DetalleHTML As String = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                                & "<tr><td colspan='4' align='center'> REPORTE DE INCIDENTES/ACCIDENTES </td></tr>" _
                                & "<tr><td colspan='4'><hr /></td></tr>" _
                                & "<tr><td align='right'><label><b>Número de Reporte:</b></label></td><td> " & dr_Reporte("Numero_RIA") & " </td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Tipo:</b></label></td><td>" & tipo & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Entidad:</b></label></td><td>" & entidad & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Usuario Registro:</b></label></td><td>" & Session("UsuarioNombre") & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Fecha RIA:</b></label></td><td>" & fecha & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Hora RIA:</b></label></td><td>" & hora & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Fecha Registro:</b></label></td><td>" & Now.Date & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Hora Registro:</b></label></td><td>" & Now.ToShortTimeString & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Descripción:</b></label></td><td>" & DescripcionMail & "</td><td></td><td></td></tr>" _
                                & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>" & Pie & "</td></tr></table><br/><br/></body></html>"


        Dim dr As DataRow = fn_Empleados_ObtenValores(ddl_UsuarioA.SelectedValue)

        If dr("Mail") <> "" Then
            If Not fn_Enviar_MailHTML(dr("Mail"), "REPORTE DE INCIDENTES/ACCIDENTES", DetalleHTML, "", Session("SucursalID")) Then
                fn_Alerta("Ha ocurrido un error al intentar enviar el Correo.")
                Exit Sub
            End If
        End If

        ddl_UsuarioA.SelectedValue = 0
        tbx_Descripcion.Text = ""

        Call LlenarGridRIA()

        gv_RIA.SelectedIndex = Session("IndiceAnterior")
        gv_RIA.PageIndex = Session("PaginaAnterior")
        gv_RIA_SelectedIndexChanged(sender, e)

    End Sub

    Protected Sub btn_Subir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Subir.Click, btn_Finalizar.Click

        If tbx_FechaSeguimiento.Text = "" Then
               fn_Alerta("Capture la Fecha del Evento de Seguimiento.")
            tbx_FechaSeguimiento.Focus()
            Exit Sub
        End If
        Dim FechaSeguimiento As String = fn_Fecha102(tbx_FechaSeguimiento.Text)

        If ddl_HoraSeguimiento.SelectedValue = "00" And ddl_MinSeguimiento.SelectedValue = "00" Then
               fn_Alerta("Seleccione la Hora del Evento de Seguimiento.")
            ddl_HoraSeguimiento.Focus()
            Exit Sub
        End If

        Dim HoraSeguimiento As String = ddl_HoraSeguimiento.SelectedValue & ":" & ddl_MinSeguimiento.SelectedValue

        If tbx_Descripcion.Text = "" Then
             fn_Alerta("Capture la Descripción.")
            tbx_Descripcion.Focus()
            Exit Sub
        End If

        'Aquí se inserta la imagen en caso de que se haya agregado
        If (FileUpload1.HasFile) Then

            ' Obtener el nombre del archivo a subir.
            Dim fileName As String = Server.HtmlEncode(FileUpload1.FileName)

            ' Obtener la extensión de el archivo cargado.
            Dim extension As String = System.IO.Path.GetExtension(fileName)

            If (extension = ".png") Or (extension = ".jpg") Or (extension = ".jpeg") Then

                Dim imageSize As Byte() = New Byte(FileUpload1.PostedFile.ContentLength - 1) {}

                If imageSize.LongLength > 300000 Then
                         fn_Alerta("La Imagen debe ser menor de 300 kb.")
                    FileUpload1.Focus()
                    Exit Sub
                End If

            Else
                fn_Alerta("El Archivo debe tener las extensiones (.jpg o .png).")
                FileUpload1.Focus()
                Exit Sub
            End If

        End If

        If sender Is btn_Finalizar Then
            Status = "V"
        End If

        Dim Id_RIAD As Integer = fn_SeguimientoRIA_Guardar(Session("SucursalID"), Session("UsuarioID"), gv_RIA.SelectedDataKey.Values("Id_RIA"), Session("EstacioN"), tbx_Descripcion.Text.ToUpper, Status, FechaSeguimiento, HoraSeguimiento)

        If Id_RIAD = 0 Then
              fn_Alerta("Ha ocurrido un error al intentar guardar los datos.")
            Exit Sub
        Else
             End If

        hfd_IDRIAD.Value = Id_RIAD
        tbx_Descripcion.Text = ""

        'Aquí se inserta la imagen en caso de que se haya agregado
        If (FileUpload1.HasFile) Then

            Dim imageSize2 As Byte() = New Byte(FileUpload1.PostedFile.ContentLength - 1) {}
            Dim Imagen As HttpPostedFile = FileUpload1.PostedFile

            Imagen.InputStream.Read(imageSize2, 0, CInt(FileUpload1.PostedFile.ContentLength))

            If Not fn_IncidentesAccidentes_GuardarImagenes(Session("SucursalID"), Session("UsuarioID"), gv_RIA.SelectedDataKey.Values("Id_RIA"), imageSize2, "", hfd_IDRIAD.Value) Then
                    fn_Alerta("Ha ocurrido un error al intentar guardar la Imagen.")
                Exit Sub
            End If

        End If

        If sender Is btn_Finalizar Then
            Call MuestraGridsVacios()
            Call LlenarGridRIA()
        Else
            Call LlenarGridRIA()
            gv_RIA.SelectedIndex = Session("IndiceAnterior")
            gv_RIA.PageIndex = Session("PaginaAnterior")
            gv_RIA_SelectedIndexChanged(sender, e)
        End If
    End Sub

End Class
