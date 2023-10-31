
Partial Class LoginContra
    Inherits BasePage

    Dim Dt_Contras As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

    End Sub

    Protected Sub btn_Aceptar_Click(sender As Object, e As EventArgs) Handles btn_Aceptar.Click
        Call Validar()
    End Sub

    Protected Sub btn_Cancelar_Click(sender As Object, e As EventArgs) Handles btn_Cancelar.Click
        If Session("RequiereCambioContraseña") = "SI" Then
            Id_Usuario = 0
            System.Web.Security.FormsAuthentication.SignOut()
            System.Web.Security.FormsAuthentication.RedirectToLoginPage()
        End If

    End Sub

    Sub Validar()
        Dim BanderA_Local As Boolean
        Dim Fecha_Utilizada As String = ""
        Dim strContra As String = Request.Form("password") ' son los ID's de control
        Dim strConfirmarContra As String = Request.Form("confirmarcontrasena")

        If strContra.Trim = "" Then
            fn_Alerta("Capture la nueva contraseña.")
            Exit Sub
        End If

        If strConfirmarContra.Trim = "" Then
            fn_Alerta("Capture la contraseña de confirmación")
            Exit Sub
        End If

        If strContra <> strConfirmarContra Then
            fn_Alerta("La confirmación no coincide con la nueva Contraseña.")
            Exit Sub
        End If

        Dt_Contras = Cn_Login.UsuariosContra_Consultar10(Session("UsuarioID"))

        Dim HashLocal As String = FormsAuthentication.HashPasswordForStoringInConfigFile(strContra, "SHA1")
        BanderA_Local = FuncionesGlobales.fn_Valida_Contra(strContra)

        If BanderA_Local = False Then
            fn_Alerta("La contraseña es incorrecta.")
            Exit Sub
        End If

        'Validar que no sea igual a las ultimas 10 claves
        Dim Dr_Contra As DataRow
        For Each Dr_Contra In Dt_Contras.Rows
            If HashLocal = Dr_Contra("Contra") Then
                Fecha_Utilizada = Dr_Contra("Fecha")
                BanderA_Local = False
                Exit For
            End If
        Next

        If BanderA_Local = False Then
            fn_Alerta("La contraseña nueva no debe ser igual a una de las últimas 10 utilizadas (Esta la utilizó el " & Fecha_Utilizada & ").")
            Exit Sub
        End If

        'guardar la nueva clave
        If Cn_Login.UsuariosContra_Update(Session("UsuarioID"), Session("ModuloClave"), HashLocal, Session("EstacioN"), Session("EstacionIP"), Session("EstacionMac"), Session("ModuloVersion")) > 0 Then
            Session("RequiereCambioContraseña") = "NO"
            Response.Redirect("~/Default.aspx")
        Else
            fn_Alerta("Ha ocurrido un error al intantar cambiar la Contraseña.")
            Exit Sub
        End If

    End Sub

End Class
