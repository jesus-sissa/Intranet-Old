Imports IntranetSIAC.Cn_Login
Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.FuncionesGlobales
Imports System.Web.UI.WebControls.GridView

Partial Public Class CustodiasTemporales_Consulta
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Session("SucursalID") = 2
        fn_Log_Create(Session("UsuarioID"), Session("ModuloClave"), "CONSULTA DE CUSTODIAS TEMPORALES", Session("EstacioN"), Session("EstacionIP"), "", Session("ModuloVersion"), Session("LoginID"), Session("SucursalID"))

        Dim dt_Cias As DataTable = fn_CustodiasTemporales_ObtenerCias(Session("SucursalID"), Session("UsuarioID"), 0)
        fn_LlenarDDL(ddl_Cia, dt_Cias, "Nombre", "Id_Cia", "0")

        Call LlenaRemisiones(-1)
        Call LlenaRutas()

        Session("RemisionesDespachadas") = ""
        Session("DatosRuta") = ""

        Dim Columnas As Byte
        Dim Gv_Nombres() As GridView = {gv_Rutas}

        For j As Byte = 0 To Gv_Nombres.Length - 1
            Columnas = Gv_Nombres(j).Columns.Count

            For i As Byte = 0 To Columnas - 1
                Gv_Nombres(j).Columns(i).HeaderStyle.HorizontalAlign = HorizontalAlign.Left
            Next
        Next
    End Sub

    Sub LlenaRemisiones(ByVal cia As Integer)
        Dim dt_Remisiones As DataTable = fn_ConsultaCustTemp_DespachoRutas(Session("SucursalID"), Session("UsuarioID"), cia)
        If dt_Remisiones IsNot Nothing Then
            gv_Remisiones.DataSource = dt_Remisiones
            gv_Remisiones.DataBind()
        Else
            MuestraRemisionesVacio()
        End If
    End Sub

    Sub MuestraRemisionesVacio()
        gv_Remisiones.DataSource = fn_CreaGridVacio("Id_Bo,Remision,Caja,Cliente,Importe,Envases,Status,Id_Remision,Entidad_Origen,Entidad_Destino")
        gv_Remisiones.DataBind()
        gv_Remisiones.SelectedIndex = -1
    End Sub

    Sub LlenaRutas()
        Dim dt_Rutas As DataTable = fn_ConsultaCustTemp_ObtenerRutas(Session("SucursalID"), Session("UsuarioID"))
        If dt_Rutas IsNot Nothing Then
            gv_Rutas.DataSource = dt_Rutas
            gv_Rutas.DataBind()
            gv_Rutas.SelectedIndex = -1
        Else
            MuestraRutasVacio()
        End If
    End Sub

    Sub MuestraRutasVacio()
        gv_Rutas.DataSource = fn_CreaGridVacio("Id_Punto,Ruta,Origen,Destino,IDR,Unidad,Cajero")
        gv_Rutas.DataBind()
        gv_Rutas.SelectedIndex = -1
    End Sub

    Protected Sub ddl_Cia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Cia.SelectedIndexChanged
        If ddl_Cia.SelectedValue = 0 Then
            Call MuestraRemisionesVacio()
        Else
            Call LlenaRemisiones(ddl_Cia.SelectedValue)
        End If
        Call LlenaRutas()
    End Sub

    Protected Sub btn_Despachar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Despachar.Click
        Dim HayMarcadas As Boolean = False
        Dim Remisiones(0) As String
        Dim cant As Integer = 0
        Dim row As GridViewRow
        Dim ischecked As Boolean = False

        'Revisar si hay alguna Ruta seleccionada
        If Val(gv_Rutas.DataKeys(0).Value) = 0 Then
            fn_Alerta("Debe seleccionar la Ruta.")
            Exit Sub
        End If

        If gv_Rutas.SelectedRow Is Nothing Then
            fn_Alerta("Debe seleccionar la Ruta.")
            Exit Sub
        End If

        'Llenar el array Remisiones, que es el que se va a usar en el Reporte,
        'con todas las que hayan sido marcadas
        For c As Integer = 0 To gv_Remisiones.Rows.Count - 1
            'Se recorre el gridview de las Remisiones
            row = gv_Remisiones.Rows(c)
            'Se toman los datos de cada Remision
            ischecked = DirectCast(row.FindControl("chk_Remision"), CheckBox).Checked
            If ischecked Then
                'Si la Remision está marcada
                ReDim Preserve Remisiones(cant)
                'Se agrega los datos de la Remisión al arreglo Remisiones
                Remisiones(cant) = gv_Rutas.SelectedDataKey("Id_Punto") & ";" & gv_Remisiones.DataKeys(row.RowIndex).Values("Id_Remision")

                If Session("RemisionesDespachadas") = "" Then
                    Session("RemisionesDespachadas") = gv_Remisiones.Rows(row.RowIndex).Cells(1).Text & ";" & gv_Remisiones.Rows(row.RowIndex).Cells(2).Text & ";" & gv_Remisiones.Rows(row.RowIndex).Cells(3).Text & ";" & gv_Remisiones.Rows(row.RowIndex).Cells(4).Text & ";" & gv_Remisiones.Rows(row.RowIndex).Cells(5).Text
                Else
                    Session("RemisionesDespachadas") = Session("RemisionesDespachadas") & "/" & gv_Remisiones.Rows(row.RowIndex).Cells(1).Text & ";" & gv_Remisiones.Rows(row.RowIndex).Cells(2).Text & ";" & gv_Remisiones.Rows(row.RowIndex).Cells(3).Text & ";" & gv_Remisiones.Rows(row.RowIndex).Cells(4).Text & ";" & gv_Remisiones.Rows(row.RowIndex).Cells(5).Text
                End If
                'Se incrementa contador de Remisiones marcadas
                'que será la cantidad de elementos del array Remisiones
                cant += 1
            End If
        Next

        If cant > 0 Then
            'Guardar los datos del Despacho
            If Not fn_ConsultaCustTemp_GuardarDespacho(gv_Rutas.SelectedDataKey("Id_Punto"), Remisiones, gv_Rutas.SelectedDataKey("IDR"), Session("SucursalID"), Session("UsuarioID")) Then
                fn_Alerta("Ha ocurrido un error al intentar realizar el Despacho.")
                Exit Sub
            End If

            Session("DatosRuta") = gv_Rutas.SelectedRow.Cells(1).Text & ";" & gv_Rutas.SelectedDataKey("Unidad") & ";" & gv_Rutas.SelectedDataKey("Cajero") & ";" & Session("UsuarioNombre")
            Dim cstype As Type = Page.[GetType]()
            ClientScript.RegisterStartupScript(cstype, "Reporte", "<script type=text/javascript>MostrarReporte()</script>")

            ddl_Cia.SelectedValue = 0
            LlenaRemisiones(-1)
            gv_Rutas.SelectedIndex = -1
            LlenaRutas()
        Else
            fn_Alerta("Debe marcar al menos una Remisión.")
        End If
    End Sub

    Protected Sub btn_CambiarStatus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_CambiarStatus.Click
        If gv_Rutas.SelectedRow Is Nothing Then
            fn_Alerta("Debe seleccionar la Ruta.")
            Exit Sub
        End If

        If Not fn_ConsultaCustTemp_CambiarStatus(Session("SucursalID"), Session("UsuarioID"), gv_Rutas.SelectedDataKey("Id_Punto"), "RU") Then
            fn_Alerta("Ha ocurrido un error al intentar actualizar la información.")
            Exit Sub
        End If

        Call LlenaRutas()
    End Sub
End Class