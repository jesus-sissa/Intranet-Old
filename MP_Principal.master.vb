Imports System
Imports System.Collections
Imports System.Web.UI
Imports System.Web.UI.Control
Imports System.Web.UI.ControlBuilder
Imports IntranetSIAC.FuncionesGlobales
Imports IntranetSIAC.Cn_Login

Partial Class MasterPage
    Inherits System.Web.UI.MasterPage

    Public Function GetContentPlaceHolders() As IList
        Return ContentPlaceHolders
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        Dim cn As New Cn_Soporte(Session, Request)

        cn.fn_LoadMenu(mnu_Navegacion, Session("SucursalID"), Session("UsuarioID"), "33")
        lbl_nombreUsuario.Text = Session("UsuarioNombre")

        lbl_NombrePagina.Text = "«" & Page.Title.ToUpper & "»"

        lbl_IntranetSIAC.Text = "INTRANET SIAC"


    End Sub
End Class

