Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.Cn_Login
Imports IntranetSIAC.Cn_Mail
Imports IntranetSIAC.FuncionesGlobales
Imports System.Data

Partial Class PlantillaLaboral
    Inherits BasePage

    Private Puesto As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        fn_Log_Create(Session("UsuarioID"), Session("ModuloClave"), "ABRIR PAGINA: PLANTILA LABORAL", Session("EstacioN"), Session("EstacionIP"), "", Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

        Dim dr As DataRow = fn_PlantillaLaboral_ObtenerDepto(Session("DepartamentoID"), Session("SucursalID"), Session("UsuarioID"))
        Call LlenarDSPlantilla()
        tbx_Cantidad.Attributes.CssStyle.Add("TEXT-ALIGN", "RIGHT")
    End Sub

    Sub LlenarDSPlantilla()
        If Session("Dpto_Reclutamiento") = Session("DepartamentoID") Then
            gv_Plantilla.DataSource = fn_PlantillaLaboral_LlenarLista(Session("SucursalID"), 0, Session("UsuarioID"))
        Else
            gv_Plantilla.DataSource = fn_PlantillaLaboral_LlenarLista(Session("SucursalID"), Session("DepartamentoID"), Session("UsuarioID"))
        End If
        gv_Plantilla.DataBind()
    End Sub

    Private Function fn_DetalleHTML(ByVal Tipo As String, ByVal Departamento As String, ByVal Jefe As String, _
                                    ByVal Puesto As String, ByVal PlantillaReqAnterior As Integer, _
                                    ByVal PlantillaReqNueva As Integer, ByVal PlantillaActual As Integer, _
                                    ByVal Comentarios As String, ByVal SucursalN As String) As String
        Dim PuestoTxt As String = ""

        If Tipo = "A" Then
            PuestoTxt = "Puesto Actualizado: "
        Else
            PuestoTxt = "Puesto Agregado: "
        End If

        Dim Pie As String = "Agente de Servicios SIAC " & Now.Year.ToString
        Dim DetalleHTML As String = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                                & "<tr><td colspan='4' align='center'> ACTUALIZACION DE PLANTILLA LABORAL </td></tr>" _
                                & "<tr><td colspan='4'><hr /></td></tr>" _
                                & "<tr><td align='right'><label><b>Sucursal:</b></label></td><td> " & SucursalN & " </td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Departamento:</b></label></td><td>" & Departamento & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Jefe de Area:</b></label></td><td>" & Jefe & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>" & PuestoTxt & "</b></label></td><td>" & Puesto & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Plantilla Requerida Anterior:</b></label></td><td>" & PlantillaReqAnterior & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Plantilla Requerida Nueva:</b></label></td><td>" & PlantillaReqNueva & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Plantilla Actual:</b></label></td><td>" & PlantillaActual & "</td><td></td><td></td></tr>" _
                                & "<tr><td align='right'><label><b>Comentarios:</b></label></td><td>" & Comentarios & "</td><td></td><td></td></tr>" _
                                & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>" & Pie & "</td></tr></table><br/><br/></body></html>"

        Return DetalleHTML

    End Function

    Protected Sub btn_Guardar_Click(sender As Object, e As EventArgs) Handles btn_Guardar.Click
       
        If tbx_Cantidad.Text.Trim = "" Then
            fn_Alerta("Debe capturar la cantidad en plantilla requerida.")
            Exit Sub
        End If

        If tbx_Comentarios.Text.Trim = "" Then
            fn_Alerta("Capture los comentarios sobre la modificación de la plantilla.")
            Exit Sub
        End If

        If Session("CantidadAnterior") = CInt(tbx_Cantidad.Text) Then
            fn_Alerta("La Plantilla Requerida no ha sido modificado.")
            tbx_Comentarios.Text = ""
            tbl_Comentarios.Visible = False
            gv_Plantilla.EditIndex = -1
            Call LlenarDSPlantilla()
            Exit Sub
        End If

        Dim Detalles As String = "Plantilla Actualizada :" _
                                & "   Sucursal : " & Session("SucursalN") _
                                & ";  Departamento : " & Session("DeptoNombre") _
                                & ";  Jefe de Area : " & Session("UsuarioNombre") _
                                & ";  Puesto : " & gv_Plantilla.SelectedRow.Cells(1).Text _
                                & ";  De " & Session("CantidadAnterior") _
                                & " A " & tbx_Cantidad.Text

        If fn_PlantillaLaboral_Actualizar(gv_Plantilla.SelectedDataKey(0).ToString(), gv_Plantilla.SelectedDataKey("Did"), _
                                          gv_Plantilla.SelectedDataKey("Pid"), Session("SucursalID"), _
                                          Session("CantidadAnterior"), CInt(tbx_Cantidad.Text), gv_Plantilla.SelectedRow.Cells(3).Text, _
                                          tbx_Comentarios.Text, Session("UsuarioID"), Session("EstacioN")) Then
            fn_AlertasGeneradas_Guardar(Session("SucursalID"), Session("UsuarioID"), Session("EstacioN"), "05", Detalles)
        Else
            fn_Alerta("Ha ocurrido un error al intentar actualizar la Plantilla.")
            Exit Sub
        End If

        Dim Tipo As Char = "A"
        Dim DetalleHTML As String = fn_DetalleHTML(Tipo, Session("DeptoNombre"), Session("UsuarioNombre"), _
                                                   gv_Plantilla.SelectedRow.Cells(1).Text, _
                                                   Session("CantidadAnterior"), tbx_Cantidad.Text, _
                                                   gv_Plantilla.SelectedRow.Cells(3).Text, tbx_Comentarios.Text.ToUpper, Session("SucursalN"))

        Dim tbl As DataTable = fn_AlertasGeneradas_ObtenerMails("05")

        If Not tbl Is Nothing AndAlso tbl.Rows.Count > 0 Then
            For Each renglon As DataRow In tbl.Rows
                fn_Enviar_MailHTML(renglon("Mail"), "ACTUALIZACION DE PLANTILLA", DetalleHTML, "", Session("SucursalID"))
                         Next
        End If
        fn_Alerta("Plantilla actualizada correctamente.")

        tbx_Comentarios.Text = ""
        tbl_Comentarios.Visible = False

        gv_Plantilla.EditIndex = -1
        Call LlenarDSPlantilla()
    End Sub

    Protected Sub gv_Plantilla_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Plantilla.SelectedIndexChanged
        If gv_Plantilla.Rows(0).Cells(2).Text = "&nbsp;" Then Exit Sub

        tbl_Comentarios.Visible = True
        Dim requerida As Integer = gv_Plantilla.SelectedDataKey("Requerida")
        Session("CantidadAnterior") = requerida
    End Sub

    Protected Sub btn_Cancelar_Click(sender As Object, e As EventArgs) Handles btn_Cancelar.Click
        gv_Plantilla.SelectedIndex = -1
        tbl_Comentarios.Visible = False
        tbx_Cantidad.Text = ""
        tbx_Comentarios.Text = ""
        Session("CantidadAnterior") = 0
    End Sub
End Class
