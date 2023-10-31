Imports IntranetSIAC.Cn_Login
Imports IntranetSIAC.Cn_Soporte
Imports IntranetSIAC.FuncionesGlobales

Partial Public Class Recepcion_CusTemp
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Session("SucursalID") = 2

        Dim dt_Cias As DataTable = fn_CustodiasTemporales_ObtenerCias(Session("SucursalID"), Session("UsuarioID"), 0)
        fn_LlenarDDL(ddl_Cia, dt_Cias, "Nombre", "Id_Cia", "0")

        Dim dt_Bovedas As DataTable = fn_CustodiasTemporales_ObtenerBovedas(Session("SucursalID"), Session("UsuarioID"), "P", "A")
        fn_LlenarDDL(ddl_Boveda, dt_Bovedas, "Descripcion", "Id_Boveda", "0")

        Dim dt_Clientes As DataTable = fn_CustodiasTemporales_ObtenerClientes(Session("SucursalID"), Session("UsuarioID"), "0", "A")
        fn_LlenarDDL(ddl_Cliente, dt_Clientes, "Nombre_Comercial", "Id_ClienteP", "0")

        Dim dt_CajasBancarias As DataTable = fn_CustodiasTemporales_ObtenerCajasBancarias(Session("SucursalID"), Session("UsuarioID"), "-1")
        fn_LlenarDDL(ddl_CajaBancaria, dt_CajasBancarias, "Nombre_Comercial", "Id_CajaBancaria", "0")

        Dim dt_Monedas As DataTable = fn_CustodiasTemporales_Monedas(Session("SucursalID"), Session("UsuarioID"))
        fn_LlenarDDL(ddl_Monedas, dt_Monedas, "Moneda", "Id_Moneda", "0")

        Dim dt_Envases As DataTable = fn_CustodiasTemporales_Envases(Session("SucursalID"), Session("UsuarioID"))
        fn_LlenarDDL(ddl_TipoEnvase, dt_Envases, "Descripcion", "Id_TipoE", "0")

        MuestraMonedasVacio()

        Call MuestraEnvasesVacio()

        Session("MonedasAgregadasGrid") = ""
        Session("MonedasAgregadasGuardar") = ""
        Session("EnvasesAgregados") = ""
        Session("ClienteDestino") = 0
    End Sub

    Sub MuestraMonedasVacio()
        gv_Monedas.DataSource = fn_CreaGridVacio("Id_Moneda,Moneda,Importe Efectivo,Importe Documentos,TipoCambio")
        gv_Monedas.DataBind()
        gv_Monedas.SelectedIndex = -1
    End Sub

    Sub MuestraEnvasesVacio()
        gv_Envases.DataSource = fn_CreaGridVacio("Id_TipoE,Numero,TipoEnvase")
        gv_Envases.DataBind()
    End Sub

    Protected Sub btn_AgregarMoneda_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_AgregarMoneda.Click
        If Val(ddl_Monedas.SelectedValue) = 0 Then
            fn_Alerta("Seleccione la Moneda.")
            ddl_Monedas.Focus()
            Exit Sub
        End If
        If Val(tbx_ImporteEfectivo.Text) = 0 Then
            If Val(tbx_ImporteDocumentos.Text) = 0 Then
                fn_Alerta("Capture el Importe Efectivo o Importe Documentos.")
                tbx_ImporteEfectivo.Focus()
                Exit Sub
            End If
        End If
        For Each fila As GridViewRow In gv_Monedas.Rows
            If gv_Monedas.DataKeys(fila.RowIndex).Values("Id_Moneda") = ddl_Monedas.SelectedValue Then
                fn_Alerta("Elemento seleccionado ya existe en la lista.")
                Exit Sub
            End If
        Next

        If Session("MonedasAgregadasGrid") = "" Then
            Session("MonedasAgregadasGrid") = ddl_Monedas.SelectedValue & ";" & ddl_Monedas.SelectedItem.Text & ";" & Val(tbx_ImporteEfectivo.Text) & ";" & Val(tbx_ImporteDocumentos.Text) & ";" & tbx_TipoCambio.Text
            Session("MonedasAgregadasGuardar") = ddl_Monedas.SelectedValue & ";" & Val(tbx_ImporteEfectivo.Text) & ";" & Val(tbx_ImporteDocumentos.Text)
        Else
            Session("MonedasAgregadasGrid") = Session("MonedasAgregadasGrid") & "/" & ddl_Monedas.SelectedValue & ";" & ddl_Monedas.SelectedItem.Text & ";" & Val(tbx_ImporteEfectivo.Text) & ";" & Val(tbx_ImporteDocumentos.Text) & ";" & tbx_TipoCambio.Text
            Session("MonedasAgregadasGuardar") = Session("MonedasAgregadasGuardar") & "/" & ddl_Monedas.SelectedValue & ";" & Val(tbx_ImporteEfectivo.Text) & ";" & Val(tbx_ImporteDocumentos.Text)
        End If

        gv_Monedas.DataSource = fn_AgregarFilaGrid("Id_Moneda,Moneda,Importe Efectivo,Importe Documentos,TipoCambio", Session("MonedasAgregadasGrid"))
        gv_Monedas.DataBind()

        Call LimpiarMonedas()
    End Sub

    Sub LimpiarMonedas()
        ddl_Monedas.SelectedValue = 0
        tbx_TipoCambio.Text = ""
        tbx_ImporteEfectivo.Text = ""
        tbx_ImporteDocumentos.Text = ""
    End Sub

    Sub LimpiarDatosEnvases()
        tbx_NumEnvase.Text = ""
    End Sub

    Protected Sub ddl_Monedas_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Monedas.SelectedIndexChanged
        If ddl_Monedas.SelectedValue = 0 Then
            tbx_TipoCambio.Text = ""
            Exit Sub
        End If
        Dim TC As Decimal = fn_CustodiasTemporales_ObtenerTipoCambio(Session("SucursalID"), Session("UsuarioID"), ddl_Monedas.SelectedValue)
        If TC = 0 Then
            fn_Alerta("No existe Tipo de Cambio registrado para el día de hoy.")
        Else
            tbx_TipoCambio.Text = Format(TC, "n2")
        End If
    End Sub

    Protected Sub btn_Agregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Agregar.Click
        If tbx_NumEnvase.Text.Trim = "" Then
            fn_Alerta("Capture el Número de Envase.")
            tbx_NumEnvase.Focus()
            Exit Sub
        End If
        If ddl_TipoEnvase.SelectedValue = 0 Then
            fn_Alerta("Seleccione el Tipo de Envase.")
            ddl_TipoEnvase.Focus()
            Exit Sub
        End If

        For Each fila As GridViewRow In gv_Envases.Rows
            If gv_Envases.Rows(fila.RowIndex).Cells(1).Text = tbx_NumEnvase.Text.ToUpper Then
                fn_Alerta("Número de Envase ya existe en la lista.")
                Exit Sub
            End If
        Next

        If Session("EnvasesAgregados") = "" Then
            Session("EnvasesAgregados") = ddl_TipoEnvase.SelectedValue & ";" & tbx_NumEnvase.Text.ToUpper & ";" & ddl_TipoEnvase.SelectedItem.Text
        Else
            Session("EnvasesAgregados") = Session("EnvasesAgregados") & "/" & ddl_TipoEnvase.SelectedValue & ";" & tbx_NumEnvase.Text.ToUpper & ";" & ddl_TipoEnvase.SelectedItem.Text
        End If

        gv_Envases.DataSource = fn_AgregarFilaGrid("Id_TipoE,Numero,TipoEnvase", Session("EnvasesAgregados"))
        gv_Envases.DataBind()

        Call LimpiarDatosEnvases()
        SumarEnvases()
        tbx_NumEnvase.Focus()
    End Sub

    Protected Sub gv_Envases_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Envases.RowDeleting
        If gv_Envases.DataKeys(e.RowIndex).Value = "" Then Exit Sub

        ActualizarEnvases(gv_Envases.Rows(e.RowIndex).Cells(1).Text)

        If Session("EnvasesAgregados") = "" Then
            MuestraEnvasesVacio()
            lbl_TotalEnvSuma.Text = 0
        Else
            gv_Envases.DataSource = fn_AgregarFilaGrid("Id_TipoE,Numero,TipoEnvase", Session("EnvasesAgregados"))
            gv_Envases.DataBind()
            SumarEnvases()
        End If
    End Sub

    Sub ActualizarEnvases(ByVal EnvEliminar As String)
        Dim EnvasesActualizados As String = ""
        Dim Envases() As String = Split(Session("EnvasesAgregados"), "/")

        For x As Integer = 0 To Envases.Length - 1
            Dim div() As String = Split(Envases(x), ";")
            If div(1) <> EnvEliminar Then
                If EnvasesActualizados = "" Then
                    EnvasesActualizados = Envases(x)
                Else
                    EnvasesActualizados = EnvasesActualizados & "/" & Envases(x)
                End If
            End If
        Next
        Session("EnvasesAgregados") = EnvasesActualizados
    End Sub

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Guardar.Click
        If ddl_Cia.SelectedValue = 0 Then
            fn_Alerta("Seleccione la Compañía.")
            ddl_Cia.Focus()
            Exit Sub
        End If
        If ddl_Boveda.SelectedValue = 0 Then
            fn_Alerta("Seleccione la Bóveda.")
            ddl_Boveda.Focus()
            Exit Sub
        End If
        If ddl_Cliente.SelectedValue = 0 Then
            fn_Alerta("Seleccione el Cliente.")
            ddl_Cliente.Focus()
            Exit Sub
        End If
        If ddl_CajaBancaria.SelectedValue = 0 Then
            fn_Alerta("Seleccione la Caja Bancaria.")
            ddl_CajaBancaria.Focus()
            Exit Sub
        End If
        If Val(tbx_NumRemision.Text) = 0 Then
            fn_Alerta("Capture el Número de Remisión.")
            tbx_NumRemision.Focus()
            Exit Sub
        End If
        If fn_CustodiasTemporales_ExisteRemision(Session("SucursalID"), Session("UsuarioID"), tbx_NumRemision.Text, ddl_Cia.SelectedValue) Then
            fn_Alerta("El Número de Remisión ya existe.")
            tbx_NumRemision.Focus()
            Exit Sub
        End If
        If Session("MonedasAgregadasGrid") = "" Then
            fn_Alerta("Debe agregar al menos un Importe.")
            Exit Sub
        End If

        If Session("EnvasesAgregados") = "" And Val(tbx_EnvasesSN.Text) = 0 Then
            fn_Alerta("Debe agregar al menos un Envase.")
            Exit Sub
        End If

        Dim Envases() As String
        Dim EnvasesCN As Integer = 0

        If Session("EnvasesAgregados") <> "" Then
            EnvasesCN = gv_Envases.Rows.Count
            Envases = Split(Session("EnvasesAgregados"), "/")
        End If

        Dim Monedas() As String = ValidarMonedas()

        Dim ImporteRemision As Decimal = ObtenerImporteTotal()
        Dim Proceso As Boolean = True


        If Not fn_CustodiasTemporales_Guardar(tbx_NumRemision.Text, EnvasesCN, Val(tbx_EnvasesSN.Text), ImporteRemision, ddl_Cia.SelectedValue, ddl_Boveda.SelectedValue, ddl_Cliente.SelectedValue, ddl_CajaBancaria.SelectedValue, Envases, Monedas, Proceso, Session("ClienteDestino"), ddl_Cliente.SelectedValue, Session("SucursalID"), Session("UsuarioID"), Session("MonedaId"), Session("EstacioN"), Session("TurnoId")) Then
            fn_Alerta("Ha ocurrido un error al intentar Guardar la Remisión.")
            Exit Sub
        End If

        Session("MonedasAgregadasGrid") =""
        Session("MonedasAgregadasGuardar")=""
        Session("EnvasesAgregados")=""

        ddl_Cliente.SelectedValue = 0
        ddl_CajaBancaria.SelectedValue=0
        tbx_NumRemision.Text = ""
        Call LimpiarMonedas()
        MuestraMonedasVacio()

        tbx_NumEnvase.Text = ""
        ddl_TipoEnvase.SelectedValue = 0
        MuestraEnvasesVacio()

        tbx_EnvasesSN.Text = ""
        lbl_TotalEnvSuma.Text = "0"
    End Sub

    Private Function ObtenerImporteTotal() As Decimal
        Dim Total As Decimal = 0
        For Each row As GridViewRow In gv_Monedas.Rows
            If Total = 0 Then
                Total = (CDec(row.Cells(2).Text) + CDec(row.Cells(3).Text)) * CDec(gv_Monedas.DataKeys(row.RowIndex).Values("TipoCambio"))
            Else
                Total = Total + (CDec(row.Cells(2).Text) + CDec(row.Cells(3).Text)) * CDec(gv_Monedas.DataKeys(row.RowIndex).Values("TipoCambio"))
            End If

        Next
        Return Total
    End Function

    Function ValidarMonedas() As String()
        Dim arrMonedas(0) As String
        Dim cant As Integer = 0

        Dim MonedaID As Integer = 0
        Dim ImpEfe As Decimal = 0.0
        Dim ImpDoc As Decimal = 0.0
        Dim SiExisteMoneda As Boolean = False
        Dim TipoMonedas() As String = Split(Session("MonedasAgregadasGuardar"), "/")
        Dim Monedas() As String

        For Each item As ListItem In ddl_Monedas.Items
            If item.Value = 0 then Continue For
            ReDim Preserve arrMonedas(cant)
            SiExisteMoneda = False
            MonedaID = item.Value
            ImpEfe = 0
            ImpDoc = 0
            For i As Integer = 0 To TipoMonedas.Length - 1
                Monedas = Split(TipoMonedas(i), ";")

                If item.Value = Monedas(0) Then
                    SiExisteMoneda = True
                    ImpEfe = Monedas(1)
                    ImpDoc = Monedas(2)
                    Exit For
                End If
            Next
            If SiExisteMoneda Then
                arrMonedas(cant) = MonedaID & ";" & ImpEfe.ToString & ";" & ImpDoc.ToString
            Else
                arrMonedas(cant) = MonedaID & ";" & ImpEfe.ToString & ";" & ImpDoc.ToString
            End If
            cant += 1
        Next

        Return arrMonedas
    End Function

    Protected Sub ddl_Cia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Cia.SelectedIndexChanged
        If ddl_Cia.SelectedValue = 0 Then
            Dim dt_Clientes As DataTable = fn_CustodiasTemporales_ObtenerClientes(Session("SucursalID"), Session("UsuarioID"), "0", "A")
            fn_LlenarDDL(ddl_Cliente, dt_Clientes, "Nombre_Comercial", "Id_ClienteP", "0")
        Else
            Dim dt_Clientes As DataTable = fn_CustodiasTemporales_ObtenerClientes(Session("SucursalID"), Session("UsuarioID"), ddl_Cia.SelectedValue, "A")
            fn_LlenarDDL(ddl_Cliente, dt_Clientes, "Nombre_Comercial", "Id_ClienteP", "0")
        End If
    End Sub

    Protected Sub ddl_Cliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Cliente.SelectedIndexChanged
        If ddl_Cliente.SelectedValue = 0 Then
            Dim dt_CajasBancarias As DataTable = fn_CustodiasTemporales_ObtenerCajasBancarias(Session("SucursalID"), Session("UsuarioID"), "-1")
            fn_LlenarDDL(ddl_CajaBancaria, dt_CajasBancarias, "Nombre_Comercial", "Id_CajaBancaria", "0")
        Else
            Dim dt_CajasBancarias As DataTable = fn_CustodiasTemporales_ObtenerCajasBancarias(Session("SucursalID"), Session("UsuarioID"), ddl_Cliente.selectedvalue )
            fn_LlenarDDL(ddl_CajaBancaria, dt_CajasBancarias, "Nombre_Comercial", "Id_CajaBancaria", "0")
        End If
    End Sub

    Protected Sub ddl_CajaBancaria_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_CajaBancaria.SelectedIndexChanged
        If ddl_CajaBancaria.SelectedValue = 0 Then
            Session("ClienteDestino") = 0
        Else
            Dim dr_CB As DataRow = fn_CustodiasTemporales_ObtenClienteDestino(Session("SucursalID"), Session("UsuarioID"), ddl_CajaBancaria.SelectedValue)
            If Not dr_CB Is Nothing Then
                Session("ClienteDestino") = dr_CB("Id_Cliente")
            End If
        End If
    End Sub

    Private Sub gv_Monedas_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gv_Monedas.RowDeleting
        If gv_Monedas.DataKeys(e.RowIndex).Value = "" Then Exit Sub

        ActualizarMonedas(gv_Monedas.DataKeys(e.RowIndex).Value)

        If Session("MonedasAgregadasGrid") = "" Then
            MuestraMonedasVacio()
        Else
            gv_Monedas.DataSource = fn_AgregarFilaGrid("Id_Moneda,Moneda,Importe Efectivo,Importe Documentos,TipoCambio", Session("MonedasAgregadasGrid"))
            gv_Monedas.DataBind()
        End If
    End Sub

    Sub ActualizarMonedas(ByVal MonEliminar As String)
        Dim MonedasActualizados As String = ""
        Dim Monedas() As String = Split(Session("MonedasAgregadasGrid"), "/")

        For x As Integer = 0 To Monedas.Length - 1
            Dim div() As String = Split(Monedas(x), ";")
            If div(0) <> MonEliminar Then
                If MonedasActualizados = "" Then
                    MonedasActualizados = Monedas(x)
                Else
                    MonedasActualizados = MonedasActualizados & "/" & Monedas(x)
                End If
            End If
        Next
        Session("MonedasAgregadasGrid") = MonedasActualizados
    End Sub

    Protected Sub tbx_EnvasesSN_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbx_EnvasesSN.TextChanged
        If tbx_EnvasesSN.Text.Length > 0 Then
            SumarEnvases()
        End If
    End Sub

    Sub SumarEnvases()
        lbl_TotalEnvSuma.Text = gv_Envases.Rows.Count + Val(tbx_EnvasesSN.Text)
    End Sub

End Class