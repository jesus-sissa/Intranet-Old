
Imports SISSAIntranet.Cn_Soporte
Imports SISSAIntranet.Cn_Login
Imports SISSAIntranet.Cn_Mail
Imports SISSAIntranet.FuncionesGlobales
Imports System.Data

Partial Class wuc_PlantillaLaboral
    Inherits System.Web.UI.UserControl

    Dim Puesto As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack Then Exit Sub

        Page.Title = "PLANTILLA LABORAL"

        '-----------------------------------------
        'Aquí se valida que el Usuario firmado tenga realmente Privilegios para acceder a esta página
        'o se esta introduciendo la página directo en la barra de direcciones

        Dim arr As String() = Split(Session("CadenaPriv"), ";")

        For x As Integer = 0 To arr.Length - 1
            If arr(x) = "PLANTILLA LABORAL" Then
                Exit For
            ElseIf x = (arr.Length - 1) Then
                MostrarAlertAjax("NO TIENE PRIVILEGIOS PARA ACCEDER A ESTA OPCION", Me, Page)
                Response.Redirect("frm_login.aspx")
                Exit Sub
            End If
        Next

        '-------------------------------------------

        fn_Log_Create(Session("UsuarioID"), Session("ModuloClave"), "ABRIR PAGINA: PLANTILA LABORAL", Session("EstacioN"), Session("EstacionIP"), "", Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

        Dim dr As DataRow = fn_PlantillaLaboral_ObtenerDepto(Session("DepartamentoID"), Session("SucursalID"), Session("UsuarioID"))

        'If dr IsNot Nothing Then
        '    lbl_Departamento.Text = dr("Descripcion")
        'End If

        LlenarDSPlantilla()

        Dim dt_Puestos As DataTable = fn_PlantillaLaboral_ObtenerPuestos(Session("SucursalID"), Session("UsuarioID"))
        fn_LlenarDDL(ddlPuestos, dt_Puestos, "Descripcion", "Id_Puesto", "")


    End Sub

    Sub LlenarDSPlantilla()
        gv_Plantilla.DataSource = fn_PlantillaLaboral_LlenarLista(Session("SucursalID"), Session("DepartamentoID"), Session("UsuarioID"))
        gv_Plantilla.DataBind()
    End Sub

    Protected Sub gv_Plantilla_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_Plantilla.RowEditing
        ddlPuestos.SelectedIndex = 0
        tbx_PlantillaReq.Text = ""
        tbx_PlantillaAct.Text = ""
        tbx_ComentariosAgregar.Text = ""

        gv_Plantilla.EditRowStyle.ForeColor = Drawing.Color.White
        gv_Plantilla.EditRowStyle.Font.Bold = True
        gv_Plantilla.EditRowStyle.BackColor = System.Drawing.Color.FromName("#C0A062")

        SetEnable()
        tbl_Comentarios.Visible = True
        Dim lbl As Label = gv_Plantilla.Rows(e.NewEditIndex).FindControl("lbl_PlantillaReq")
        Session("CantidadAnterior") = CInt(lbl.Text)

        gv_Plantilla.EditIndex = e.NewEditIndex
        LlenarDSPlantilla()
    End Sub

    Protected Sub gv_Plantilla_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gv_Plantilla.RowCancelingEdit

        SetEnable()
        tbx_Comentarios.Text = ""
        tbl_Comentarios.Visible = False

        gv_Plantilla.EditIndex = -1
        LlenarDSPlantilla()

    End Sub

    Protected Sub gv_Plantilla_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gv_Plantilla.RowUpdating

        Dim tbx As TextBox = gv_Plantilla.Rows(e.RowIndex).FindControl("tbx_GridPlantillaReq")

        If Session("CantidadAnterior") = tbx.Text Then
            MostrarAlertAjax("La Plantilla Requerida no ha sido modificado.", sender, Page)
            SetEnable()
            tbx_Comentarios.Text = ""
            tbl_Comentarios.Visible = False
            gv_Plantilla.EditIndex = -1
            LlenarDSPlantilla()
            Exit Sub
        End If


        Dim Detalles As String = "Plantilla Actualizada :" _
                                & "   Departamento : " & Session("DeptoNombre") _
                                & ";  Jefe de Area : " & Session("UsuarioNombre") _
                                & ";  Puesto : " & gv_Plantilla.Rows(e.RowIndex).Cells(1).Text _
                                & ";  De " & Session("CantidadAnterior") _
                                & " A " & tbx.Text

        If fn_PlantillaLaboral_Actualizar(gv_Plantilla.DataKeys(e.RowIndex).Value.ToString(), gv_Plantilla.DataKeys(e.RowIndex).Values("DeptoId"), gv_Plantilla.DataKeys(e.RowIndex).Values("PuestoID"), Session("SucursalID"), Session("CantidadAnterior"), CInt(tbx.Text), gv_Plantilla.Rows(e.RowIndex).Cells(3).Text, tbx_Comentarios.Text, Session("UsuarioID"), Session("EstacioN")) Then
            fn_AlertasGeneradas_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), "05", Detalles)
        Else
            MostrarAlertAjax("Ha ocurrido un error al intentar actualizar la Plantilla.", btn_Guardar, Page)
            Exit Sub
        End If

        Dim Tipo As Char = "A"
        Dim DetalleMail As String = fn_DetalleMail(Tipo, Session("DeptoNombre"), Session("UsuarioNombre"), gv_Plantilla.Rows(e.RowIndex).Cells(1).Text, Session("CantidadAnterior"), tbx.Text, gv_Plantilla.Rows(e.RowIndex).Cells(3).Text, tbx_Comentarios.Text.ToUpper)

        Dim tbl As DataTable = fn_AlertasGeneradas_ObtenerMails("05")

        If Not tbl Is Nothing Then
            For Each renglon As DataRow In tbl.Rows
                fn_Enviar_Mail(renglon("Mail"), "ACTUALIZACION DE PLANTILLA", DetalleMail, Session("SucursalID"))
                'fn_Enviar_Mail("jfnuncio@hotmail.com", "ACTUALIZACION DE PLANTILLA", DetalleMail, Session("SucursalID"))
            Next
        End If

        SetEnable()
        tbx_Comentarios.Text = ""
        tbl_Comentarios.Visible = False

        gv_Plantilla.EditIndex = -1
        LlenarDSPlantilla()

    End Sub

    Sub SetEnable()

        ddlPuestos.Enabled = gv_Plantilla.EditIndex >= 0
        tbx_PlantillaAct.Enabled = gv_Plantilla.EditIndex >= 0
        tbx_PlantillaReq.Enabled = gv_Plantilla.EditIndex >= 0
        tbx_ComentariosAgregar.Enabled = gv_Plantilla.EditIndex >= 0
        btn_Guardar.Enabled = gv_Plantilla.EditIndex >= 0

    End Sub

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Guardar.Click

        If tbx_PlantillaReq.Text = "0" Then
            MostrarAlertAjax("Plantilla Requerida debe ser mayor que cero.", btn_Guardar, Page)
            tbx_PlantillaReq.Focus()
            Exit Sub
        End If

        For Each fila As GridViewRow In gv_Plantilla.Rows
            If gv_Plantilla.DataKeys(fila.RowIndex).Values("PuestoID") = ddlPuestos.SelectedValue Then
                MostrarAlertAjax("Elemento seleccionado ya existe en la lista", btn_Guardar, Page)
                ddlPuestos.Focus()
                Exit Sub
            End If
        Next

        Dim Detalles As String = "  Plantilla Actualizada :" _
                               & "  Departamento : " & Session("DeptoNombre") _
                               & "; Jefe de Area : " & Session("UsuarioNombre") _
                               & "; Puesto Agregado : " & ddlPuestos.SelectedItem.Text _
                               & ";  De " & "0" & " a " & tbx_PlantillaReq.Text

        If fn_PlantillaLaboral_Guardar(Session("SucursalID"), Session("DepartamentoID"), ddlPuestos.SelectedValue, tbx_PlantillaReq.Text, tbx_PlantillaAct.Text, tbx_ComentariosAgregar.Text, Session("UsuarioID"), Session("EstacioN")) Then
            MostrarAlertAjax("Los datos han sido guardados correctamente.", btn_Guardar, Page)
            fn_AlertasGeneradas_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), "05", Detalles)
        Else
            MostrarAlertAjax("Ha ocurrido un error al intentar actualizar la Plantilla.", btn_Guardar, Page)
            Exit Sub
        End If

        Dim Tipo As Char = "P"
        Dim DetalleMail As String = fn_DetalleMail(Tipo, Session("DeptoNombre"), Session("UsuarioNombre"), ddlPuestos.SelectedItem.Text, 0, tbx_PlantillaReq.Text, tbx_PlantillaAct.Text, tbx_ComentariosAgregar.Text.ToUpper)

        If Not fn_EnviarCorreos("10", DetalleMail, Session("SucursalID"), "MODIFICACION DE PLANTILLA LABORAL") Then
            MostrarAlertAjax("Ha ocurrido un error al intentar enviar los Correos.", btn_Guardar, Page)
            Exit Sub
        End If

        ddlPuestos.SelectedIndex = 0
        tbx_PlantillaReq.Text = ""
        tbx_PlantillaAct.Text = ""
        tbx_ComentariosAgregar.Text = ""

        LlenarDSPlantilla()

    End Sub


    Protected Sub ddlPuestos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPuestos.SelectedIndexChanged

        If ddlPuestos.SelectedValue = "" Then
            Puesto = "0"
        Else
            Puesto = ddlPuestos.SelectedValue
        End If
        Dim PlantillaActual As Integer = fn_PlantillaLaboral_ObtenerEmpleadosXPuesto(Session("SucursalID"), Session("DepartamentoID"), Puesto, Session("UsuarioID"))
        tbx_PlantillaAct.Text = PlantillaActual

    End Sub

    Protected Sub gv_Plantilla_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Plantilla.RowCreated
        ' En este Sub se agregan a las filas de gv_Plantilla los atributos "onmouseover" y "onmouseout"
        ' para que cuando el puntero del mouse este sobre una fila, se apliquen las propiedades declaradas (backgoundColor)

        ' only apply changes if its DataRow
        If e.Row.RowType = DataControlRowType.DataRow Then

            ' when mouse is over the row, save original color to new attribute, and change it to highlight yellow color
            e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#C0A062'")  '#D0A540'")

            ' when mouse leaves the row, change the bg color to its original value    
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")

        End If
    End Sub

    Public Shared Function fn_DetalleMail(ByVal Tipo As String, ByVal Departamento As String, ByVal Jefe As String, ByVal Puesto As String, ByVal PlantillaReqAnterior As Integer, ByVal PlantillaReqNueva As Integer, ByVal PlantillaActual As Integer, ByVal Comentarios As String) As String

        Dim PuestoTxt As String = ""

        If Tipo = "A" Then
            PuestoTxt = "          Puesto Actualizado: "
        Else
            PuestoTxt = "             Puesto Agregado: "
        End If

        Dim Detalle As String = "               ACTUALIZACION DE PLANTILLA LABORAL " & Chr(13) & Chr(13) _
                              & "                Departamento: " & Departamento & Chr(13) _
                              & "                Jefe de Area: " & Jefe & Chr(13) _
                              & PuestoTxt & Puesto & Chr(13) _
                              & "Plantilla Requerida Anterior: " & PlantillaReqAnterior & Chr(13) _
                              & "   Plantilla Requerida Nueva: " & PlantillaReqNueva & Chr(13) _
                              & "            Plantilla Actual: " & PlantillaActual & Chr(13) _
                              & "                 Comentarios: " & Comentarios & Chr(13) & Chr(13) _
                              & "Agente de Servicios SIAC."
        Return Detalle
    End Function

End Class
