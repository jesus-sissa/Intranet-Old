
Imports SISSAIntranet.Cn_Soporte
Imports SISSAIntranet.Cn_Mail
Imports SISSAIntranet.FuncionesGlobales
Imports System.Data

Partial Class UserControls_wuc_RIAConsulta
    Inherits System.Web.UI.UserControl

    Dim FechaI As String
    Dim FechaF As String
    Dim valorTipo As Integer
    Dim valorStatus As Char
    Dim valorUsuario As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Page.Title = "CONSULTA DE REPORTES DE INCIDENTES/ACCIDENTES"

        '-----------------------------------------
        'Aquí se valida que el Usuario firmado tenga realmente Privilegios para acceder a esta página
        'o se esta introduciendo la página directo en la barra de direcciones

        Dim arr As String() = Split(Session("CadenaPriv"), ";")

        For x As Integer = 0 To arr.Length - 1
            If arr(x) = "CONSULTA RIA" Then
                Exit For
            ElseIf x = (arr.Length - 1) Then
                MostrarAlertAjax("NO TIENE PRIVILEGIOS PARA ACCEDER A ESTA OPCION", Me, Page)
                Response.Redirect("frm_login.aspx")
                Exit Sub
            End If
        Next

        '-------------------------------------------

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
        'gv_Detalle.DataBind()

        gv_Usuarios.DataSource = fn_CreaGridVacio("Id_Entidad,Nombre,Rol")
        gv_Usuarios.DataBind()

        Session("RIA_Seleccionado") = 0
        Session("PaginaAnterior") = 0

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

        gv_RIA.SelectedRowStyle.BackColor = Drawing.Color.White
        gv_RIA.SelectedRowStyle.ForeColor = Drawing.Color.Black
        gv_RIA.SelectedRowStyle.Font.Bold = False
        gv_RIA.DataSource = fn_CreaGridVacio("Id_RIA,Numero,Sucursal,Fecha,Hora,UsuarioSeg,Tipo,Entidad,Status")
        gv_RIA.DataBind()

        gv_Imagenes.DataSource = Nothing
        gv_Imagenes.DataBind()

        gv_Usuarios.DataSource = fn_CreaGridVacio("Id_Entidad,Nombre,Rol")
        gv_Usuarios.DataBind()
    End Sub

    Protected Sub ddl_Tipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Tipo.SelectedIndexChanged
        MuestraGridsVacios()
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
        MuestraGridsVacios()
    End Sub

    Protected Sub cbx_Status_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbx_Status.CheckedChanged
        MuestraGridsVacios()

        If cbx_Status.Checked Then
            ddl_Status.SelectedValue = 0
            ddl_Status.Enabled = False
        Else
            ddl_Status.Enabled = True
        End If
    End Sub

    Protected Sub ddl_Usuarios_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Usuarios.SelectedIndexChanged
        MuestraGridsVacios()
    End Sub

    Protected Sub cbx_Usuarios_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbx_Usuarios.CheckedChanged
        MuestraGridsVacios()

        If cbx_Usuarios.Checked Then
            ddl_Usuarios.SelectedValue = 0
            ddl_Usuarios.Enabled = False
        Else
            ddl_Usuarios.Enabled = True
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

        'If ddl_Status.SelectedIndex = 0 And Not cbx_Status.Checked Then
        '    MostrarAlertAjax("Seleccione el Status.", btn_Mostrar, Page)
        '    ddl_Status.Focus()
        '    Exit Sub
        '    If cbx_Status.Checked Then
        '        valorStatus = "T"
        '    Else
        '        valorStatus = ddl_Status.SelectedValue
        '    End If
        'End If

        'If ddl_Status.SelectedIndex = 0 Then

        '    If cbx_Status.Checked Then
        '        valorStatus = "T"
        '        valorUsuario = 0
        '    Else
        '        MostrarAlertAjax("Seleccione el Status.", btn_Mostrar, Page)
        '        ddl_Status.Focus()
        '        Exit Sub
        '    End If
        'Else
        '    If ddl_Status.SelectedValue = "I" Or ddl_Status.SelectedValue = "V" Then
        '        If ddl_Usuarios.SelectedIndex = 0 And Not cbx_Usuarios.Checked Then
        '            MostrarAlertAjax("Seleccione el Usuario Seguimiento.", btn_Mostrar, Page)
        '            ddl_Usuarios.Focus()
        '            Exit Sub
        '        End If
        '    End If
        '    valorStatus = ddl_Status.SelectedValue
        '    valorUsuario = ddl_Usuarios.SelectedValue
        'End If

        MuestraGridsVacios()
        LlenarGridRIA()

    End Sub

    Sub LlenarGridRIA()

        gv_RIA.SelectedRowStyle.BackColor = Drawing.Color.White
        gv_RIA.SelectedRowStyle.ForeColor = Drawing.Color.Black
        gv_RIA.SelectedRowStyle.Font.Bold = False

        If ddl_Tipo.SelectedIndex = 0 And Not cbx_Tipos.Checked Then
            MostrarAlertAjax("Seleccione el Tipo.", btn_Mostrar, Page)
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

        If ddl_Usuarios.SelectedIndex = 0 And Not cbx_Usuarios.Checked Then
            MostrarAlertAjax("Seleccione el Usuario Seguimiento.", btn_Mostrar, Page)
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

        If dt IsNot Nothing Then
            For Each Seg As DataRow In dt.Rows

                Dim lbl_Encabezado As New System.Web.UI.WebControls.Label()
                'lbl_Encabezado.Width = 823
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
                PlaceHolderConsulta.Controls.Add(lbl_Encabezado)

                PlaceHolderConsulta.Controls.Add(New LiteralControl("<br />"))
                PlaceHolderConsulta.Controls.Add(New LiteralControl("<br />"))

                Dim lbl_Seguimiento As New System.Web.UI.WebControls.Label()
                lbl_Seguimiento.Width = 823
                'lbl_Seguimiento.Height = 100
                'lbl_Seguimiento.BorderWidth = 1
                'lbl_Seguimiento.BorderStyle = BorderStyle.Solid
                'lbl_Seguimiento.BorderColor = System.Drawing.Color.FromName("#C0A062")
                'lbl_Seguimiento.BackColor = Drawing.Color.Cornsilk
                lbl_Seguimiento.Text = Seg.Item("Descripcion")
                lbl_Seguimiento.Font.Size = 8
                PlaceHolderConsulta.Controls.Add(lbl_Seguimiento)

                PlaceHolderConsulta.Controls.Add(New LiteralControl("<br />"))
                PlaceHolderConsulta.Controls.Add(New LiteralControl("<br />"))
                PlaceHolderConsulta.Controls.Add(New LiteralControl("<hr />"))
                PlaceHolderConsulta.Controls.Add(New LiteralControl("<br />"))

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

        If gv_RIA.SelectedDataKey.Values("Status") = "" Then
            gv_RIA.SelectedRowStyle.ForeColor = Drawing.Color.Black
            gv_RIA.SelectedRowStyle.Font.Bold = False
            gv_RIA.SelectedRowStyle.BackColor = System.Drawing.Color.White
            Exit Sub
        End If

        gv_RIA.SelectedRowStyle.BackColor = System.Drawing.Color.FromName("#C0A062")
        gv_RIA.SelectedRowStyle.ForeColor = Drawing.Color.White
        gv_RIA.SelectedRowStyle.Font.Bold = True

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

        LlenarGridDetalle()

        gv_Imagenes.DataSource = fn_IncidentesAccidentes_LeerImagenes(gv_RIA.SelectedDataKey.Values("Id_RIA"), Session("SucursalID"), Session("UsuarioID"))
        gv_Imagenes.DataBind()

        LlenarGridUsuarios()

        Session("IndiceAnterior") = gv_RIA.SelectedIndex

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
                LlenarGridDetalle()
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
            'MostrarAlertAjax("Los datos se han guardado correctamente.", btn_Asignar, Page)
        Else
            MostrarAlertAjax("Ha ocurrido un error al intentar guardar la información.", btn_Asignar, Page)
            Exit Sub
        End If

        If Not fn_IncidentesAccidentes_GuardarUsuario(gv_RIA.SelectedDataKey.Values("Id_RIA"), ddl_UsuarioA.SelectedValue, Session("UsuarioID"), Session("EstacioN"), Session("SucursalID"), TipoUsuario) Then
            MostrarAlertAjax("Ha ocurrido un error al intentar Agregar el Usuario.", btn_Asignar, Page)
            Exit Sub
        End If

        Dim tipo As String = gv_RIA.SelectedRow.Cells(7).Text
        Dim entidad As String = gv_RIA.SelectedRow.Cells(8).Text
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

        'MuestraGridsVacios()
        LlenarGridRIA()

        gv_RIA.SelectedIndex = Session("IndiceAnterior")
        gv_RIA_SelectedIndexChanged(sender, e)
        gv_RIA.PageIndex = Session("PaginaAnterior")
        'LlenarGridUsuarios()

        ddl_UsuarioA.SelectedValue = 0
        rdb_Principal.Checked = False
        rdb_Secundario.Checked = False

    End Sub

    Protected Sub gv_RIA_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_RIA.PageIndexChanging
        MuestraGridsVacios()

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
        MuestraGridsVacios()
    End Sub

    Protected Sub tbx_FechaFin_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbx_FechaFin.TextChanged
        MuestraGridsVacios()
    End Sub

End Class
