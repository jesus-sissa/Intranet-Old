Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.Cn_Mail
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data

Partial Class RIAConsulta
    Inherits BasePage

    Dim FechaI As String
    Dim FechaF As String
    Dim valorTipo As Integer
    Dim valorStatus As Char
    Dim valorUsuario As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Dim ds As Data.DataTable = fn_IncidentesAccidentes_GetEmpleados(Session("SucursalID"), Session("UsuarioID"))
        Dim row As Data.DataRow = ds.NewRow

        ddl_Usuarios.DataTextField = "Nombre"
        ddl_Usuarios.DataValueField = "Id_Empleado"

        row(ddl_Usuarios.DataTextField) = "Seleccione..."
        row(ddl_Usuarios.DataValueField) = 0
        row("Clave_Empleado") = 0
        ds.Rows.InsertAt(row, 0)
        ddl_Usuarios.DataSource = ds
        ddl_Usuarios.DataBind()

        ddl_UsuarioA.DataSource = ds
        ddl_UsuarioA.DataBind()

        gv_RIA.DataSource = fn_CreaGridVacio("Id_RIA,Numero,Sucursal,Fecha,Hora,UsuarioSeg,Tipo,Entidad,Status")
        gv_RIA.DataBind()

        'gv_Detalle.DataSource = fn_CreaGridVacio("Id_RIAD,Id_RIA,Fecha,Tipo,Entidad,Descripcion")
        'gv_Detalle.DataBind() 'esto ya estaba desahuibilitado

        gv_Usuarios.DataSource = fn_CreaGridVacio("Id_Entidad,Nombre,Rol")
        gv_Usuarios.DataBind()

        Session("RIA_Seleccionado") = 0
        Session("PaginaAnterior") = 0
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
        lbl_UsuarioA.Enabled = False
        ddl_UsuarioA.SelectedIndex = 0
        ddl_UsuarioA.Enabled = False
        btn_Asignar.Enabled = False
        lbl_TipoUsuario.Enabled = False
        rdb_Principal.Enabled = False
        rdb_Secundario.Enabled = False
        rdb_Principal.Checked = False
        rdb_Secundario.Checked = False

        gv_RIA.DataSource = fn_CreaGridVacio("Id_RIA,Numero,Sucursal,Fecha,Hora,UsuarioSeg,Tipo,Entidad,Status")
        gv_RIA.DataBind()

        gv_Usuarios.DataSource = fn_CreaGridVacio("Id_Entidad,Nombre,Rol")
        gv_Usuarios.DataBind()
    End Sub

    Protected Sub ddl_Tipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Tipo.SelectedIndexChanged
        Call MuestraGridsVacios()
    End Sub

    Protected Sub cbx_Tipos_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbx_Tipos.CheckedChanged
        MuestraGridsVacios()

        If cbx_Tipos.Checked Then
            ddl_Tipo.SelectedValue = 0
            ddl_Tipo.Enabled = False
        Else
            ddl_Tipo.Enabled = True
        End If
    End Sub

    Protected Sub ddl_Status_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Status.SelectedIndexChanged
        Call MuestraGridsVacios()
    End Sub

    Protected Sub cbx_Status_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbx_Status.CheckedChanged
        Call MuestraGridsVacios()

        If cbx_Status.Checked Then
            ddl_Status.SelectedValue = 0
            ddl_Status.Enabled = False
        Else
            ddl_Status.Enabled = True
        End If
    End Sub

    Protected Sub ddl_Usuarios_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Usuarios.SelectedIndexChanged
        Call MuestraGridsVacios()
    End Sub

    Protected Sub cbx_Usuarios_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbx_Usuarios.CheckedChanged
        Call MuestraGridsVacios()

        If cbx_Usuarios.Checked Then
            ddl_Usuarios.SelectedValue = 0
            ddl_Usuarios.Enabled = False
        Else
            ddl_Usuarios.Enabled = True
        End If
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Mostrar.Click

        If tbx_FechaIni.Text = "" Then
               fn_Alerta("Seleccione la Fecha Inicio.")
            Exit Sub
        End If

        If tbx_FechaFin.Text = "" Then
             fn_Alerta("Seleccione la Fecha Fin.")
            Exit Sub
        End If

        If CDate(tbx_FechaIni.Text) > CDate(tbx_FechaFin.Text) Then
            fn_Alerta("La fecha de inicio debe ser menor que la fecha Fin.")
            Exit Sub
        End If

        Call MuestraGridsVacios()
        Call LlenarGridRIA()

    End Sub

    Sub LlenarGridRIA()

        If ddl_Tipo.SelectedIndex = 0 And Not cbx_Tipos.Checked Then
               fn_Alerta("Seleccione el Tipo.")
            ddl_Tipo.Focus()
            Exit Sub
        Else
            If cbx_Tipos.Checked Then
                valorTipo = 0
            Else
                valorTipo = ddl_Tipo.SelectedValue
            End If
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

        If ddl_Usuarios.SelectedIndex = 0 And Not cbx_Usuarios.Checked Then
                fn_Alerta("Seleccione el Usuario Seguimiento.")
            ddl_Usuarios.Focus()
            Exit Sub
        Else
            If cbx_Usuarios.Checked Then
                valorUsuario = 0
            Else
                valorUsuario = ddl_Usuarios.SelectedValue
            End If
        End If

        'valorTipo = ddl_Tipo.SelectedValue
        FechaI = fn_Fecha102(tbx_FechaIni.Text)
        FechaF = fn_Fecha102(tbx_FechaFin.Text)

        Dim dt As DataTable = fn_ConsultaRIA_LlenarLista(Session("SucursalID"), valorUsuario, valorTipo, FechaI, FechaF, valorStatus, Session("UsuarioID"))

        If dt IsNot Nothing Then
            gv_RIA.DataSource = dt
            gv_RIA.DataBind()
        Else
            gv_RIA.DataSource = fn_CreaGridVacio("Id_RIA,Numero,Sucursal,Fecha,Hora,UsuarioSeg,Tipo,Entidad,Status")
            gv_RIA.DataBind()
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
                

                    PlaceHolderConsulta.Controls.Add(lbl_Encabezado)
                    PlaceHolderConsulta.Controls.Add(New LiteralControl("<br />"))

                    Dim lbl_Seguimiento As New System.Web.UI.WebControls.Label()
                    lbl_Seguimiento.Text = Seg.Item("Descripcion")
                    lbl_Seguimiento.Font.Size = 8

                    PlaceHolderConsulta.Controls.Add(lbl_Seguimiento)
                    PlaceHolderConsulta.Controls.Add(New LiteralControl("<br />"))
                End If

                Id_RiaImagen = Seg.Item("Id_RIAI")
                If Not IsDBNull(Seg.Item("Imagen")) Then
                    Dim LigaImagen As New System.Web.UI.WebControls.HyperLink
                    LigaImagen.Text = "Imagen_" & Cuenta
                    LigaImagen.NavigateUrl = "MostrarImagenes.aspx?ID= " + Id_RiaImagen + ""
                    LigaImagen.Attributes.Add("OnClick", "window.open(this.href, 'ViewImage', 'height=550, width=710,top=90,left=100,toolbar=no,menubar=no,location=no,resizable=no,maximized=no,scrollbars=no,status=no'); return false;")

                    PlaceHolderConsulta.Controls.Add(LigaImagen)
                    PlaceHolderConsulta.Controls.Add(New LiteralControl("<font size=1><br /></font>"))
                End If
                Cuenta += 1
                PlaceHolderConsulta.Controls.Add(New LiteralControl("<font size=1><hr /></font>")) 'Escribe Linea Horiz
                'PlaceHolderSeguimiento.Controls.Add(New LiteralControl("<fieldset />"))
                'PlaceHolderSeguimiento.Controls.AddAt(0, lbl_Seguimiento)
                ID_RIADANTERIOR = Seg.Item("Id_RIAD")
            Next
        End If

    End Sub

    Sub LlenarGridUsuarios()
        Dim dt As DataTable = fn_ConsultaRIA_ObtenerUsuarios(gv_RIA.SelectedDataKey.Values("Id_RIA"), Session("SucursalID"), Session("UsuarioID"))

        If dt IsNot Nothing Then
            gv_Usuarios.DataSource = dt
            gv_Usuarios.DataBind()
        Else
            gv_Usuarios.DataSource = fn_CreaGridVacio("Id_Entidad,Nombre,Rol")
            gv_Usuarios.DataBind()
        End If
    End Sub

    Protected Sub gv_RIA_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_RIA.PageIndexChanged
        Session("PaginaAnterior") = gv_RIA.PageIndex
    End Sub

    Protected Sub gv_RIA_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gv_RIA.SelectedIndexChanged
        If gv_RIA.Rows(0).Cells(2).Text = "&nbsp;" Then Exit Sub

        If gv_RIA.SelectedDataKey.Values("Status") = "PENDIENTE" Or gv_RIA.SelectedDataKey.Values("Status") = "ABIERTO" Then
            lbl_UsuarioA.Enabled = True
            ddl_UsuarioA.Enabled = True
            btn_Asignar.Enabled = True
            lbl_TipoUsuario.Enabled = True
            rdb_Principal.Enabled = True
            rdb_Secundario.Enabled = True
        Else
            lbl_UsuarioA.Enabled = False
            ddl_UsuarioA.Enabled = False
            btn_Asignar.Enabled = False
            lbl_TipoUsuario.Enabled = False
            rdb_Principal.Enabled = False
            rdb_Secundario.Enabled = False
        End If

        Call LlenarGridDetalle()
        Call LlenarGridUsuarios()

        Session("IndiceAnterior") = gv_RIA.SelectedIndex

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
                Call LlenarGridDetalle()
                Exit Sub
            End If
        Next

        Dim Descripcion As String
        Dim DescripcionMail As String

        Descripcion = "NUEVO USUARIO PARA SEGUIMIENTO: " & ddl_UsuarioA.SelectedItem.Text
        DescripcionMail = "ASIGNACION DE RIA PARA SEGUIMIENTO"

        Dim FechaActual As Date = Now.Date
        Dim HoraActual As String = TimeString

        If fn_ConsultaRIA_GuardarD(Session("SucursalID"), gv_RIA.SelectedDataKey.Values("Id_RIA"), Session("UsuarioID"), ddl_UsuarioA.SelectedValue, Session("EstacioN"), Descripcion, FechaActual, HoraActual) Then
                  Else
             fn_Alerta("Ha ocurrido un error al intentar guardar la información.")
            Exit Sub
        End If

        If Not fn_IncidentesAccidentes_GuardarUsuario(gv_RIA.SelectedDataKey.Values("Id_RIA"), ddl_UsuarioA.SelectedValue, Session("UsuarioID"), Session("EstacioN"), Session("SucursalID"), TipoUsuario) Then
             fn_Alerta("Ha ocurrido un error al intentar Agregar el Usuario.")
            Exit Sub
        End If

        Dim tipo As String = gv_RIA.SelectedRow.Cells(7).Text
        Dim entidad As String = gv_RIA.SelectedRow.Cells(8).Text
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

        Call LlenarGridRIA()

        gv_RIA.SelectedIndex = Session("IndiceAnterior")
        gv_RIA_SelectedIndexChanged(sender, e)
        gv_RIA.PageIndex = Session("PaginaAnterior")

        ddl_UsuarioA.SelectedValue = 0
        rdb_Principal.Checked = False
        rdb_Secundario.Checked = False

    End Sub

    Protected Sub gv_RIA_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_RIA.PageIndexChanging
        Call MuestraGridsVacios()

        If cbx_Status.Checked Then
            valorStatus = "T"
        Else
            valorStatus = ddl_Status.SelectedValue
        End If

        valorUsuario = ddl_Usuarios.SelectedValue
        valorTipo = ddl_Tipo.SelectedValue
        FechaI = fn_Fecha102(tbx_FechaIni.Text)
        FechaF = fn_Fecha102(tbx_FechaFin.Text)

        gv_RIA.PageIndex = e.NewPageIndex
        gv_RIA.DataSource = fn_ConsultaRIA_LlenarLista(Session("SucursalID"), valorUsuario, valorTipo, FechaI, FechaF, valorStatus, Session("UsuarioID"))
        gv_RIA.DataBind()
    End Sub

    Protected Sub tbx_FechaIni_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbx_FechaIni.TextChanged
        Call MuestraGridsVacios()
    End Sub

    Protected Sub tbx_FechaFin_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbx_FechaFin.TextChanged
        Call MuestraGridsVacios()
    End Sub

End Class
