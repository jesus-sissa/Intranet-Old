Imports IntranetSIAC
Imports System.Data

Public Class BasePage
    Inherits Page

#Region "Variables Privadas"
    Public cn As Cn_Soporte
    Private _Alerta As Alerta
    Private _Form As Control
#End Region

#Region "Eventos"

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad
        cn = New Cn_Soporte(Session, Request)
        _Form = Me.Controls(0).FindControl("Form1")
        _Alerta = Page.LoadControl("~/UserControls/Alerta.ascx")
        _Form.Controls.Add(_Alerta)

        If Id_Usuario = 0 Then
            If Request.Url.AbsolutePath.EndsWith("/Login.aspx") Then
                Exit Sub
            Else
                System.Web.Security.FormsAuthentication.SignOut()
                System.Web.Security.FormsAuthentication.RedirectToLoginPage()
            End If
        End If

        If Request.Url.AbsolutePath.Contains("/LoginContra.aspx") Then
            Exit Sub
        End If

        If Not cn.fn_ValidaPermisos() Then
            System.Web.Security.FormsAuthentication.SignOut()
            Session.Clear() 'pendiente aplicar esta validacion
            'Session.Abandon() '
            Response.Redirect("Login.aspx")
        End If

    End Sub

#End Region

#Region "Propiedades"

    Public Property Id_Usuario() As Integer
        Get
            Dim res As Integer = 0
            Integer.TryParse(Session("UsuarioID"), res)
            Return res
        End Get
        Set(ByVal value As Integer)
            Session("UsuarioID") = value
        End Set
    End Property

    Public Property Id_Sucursal() As Integer
        Get
            Dim res As Integer = 0
            Integer.TryParse(Session("SucursalID"), res)
            Return res
        End Get
        Set(ByVal value As Integer)
            Session("SucursalID") = value
        End Set
    End Property

    Public Property NombreUsuario() As String
        Get
            Return Session("UsuarioNombre")
        End Get
        Set(ByVal value As String)
            Session("UsuarioNombre") = value
        End Set
    End Property

    Public Property Tabla(ByVal Clave As String) As DataTable
        'new 20/mayo/2015
        Get
            Dim Ds As New DataSet
            If ViewState("TablaXML") = Nothing Then Return Nothing
            Ds.ReadXml(New System.IO.StringReader(ViewState("TablaXML")))
            Return Ds.Tables(Clave)
        End Get
        Set(ByVal value As DataTable)

            Dim Ds As New DataSet
            If (ViewState("TablaXML") <> "") Then Ds.ReadXml(New System.IO.StringReader(ViewState("TablaXML")))
            If Ds.Tables(Clave) IsNot Nothing Then Ds.Tables.Remove(Clave)
            If value Is Nothing Then Exit Property

            value.TableName = Clave
            Ds.Tables.Add(value.Copy)

            ViewState("TablaXML") = Ds.GetXml

        End Set
    End Property

#End Region

#Region "Constantes"

    'Public Const ClaveModulo As String = "33"

#End Region

#Region "Metodos"

    Public Sub fn_Alerta(ByVal Mensaje As String)
        _Alerta.Alerta(Mensaje, Theme.ToUpper)
    End Sub

    Public Function fn_MostrarSiempre(ByVal Tabla As DataTable) As DataTable
        For Each col As DataColumn In Tabla.Columns
            col.AllowDBNull = True
        Next

        If Tabla.Rows.Count = 0 Then Tabla.Rows.InsertAt(Tabla.NewRow(), 0)
        Return Tabla
    End Function

    Public Function fn_LlenarDropDown(ByRef ddl As DropDownList, ByVal Tbl As DataTable) As DataTable
        Dim RowSeleccione As DataRow = Tbl.NewRow()

        For Each c As DataColumn In Tbl.Columns
            If c.ColumnName = ddl.DataTextField And c.DataType Is GetType(String) Then
                If c.MaxLength > 0 Then
                    RowSeleccione(c.ColumnName) = Left("Seleccione...", c.MaxLength)
                Else
                    RowSeleccione(c.ColumnName) = "Seleccione..."
                End If
            ElseIf c.ColumnName = ddl.DataValueField And c.DataType IsNot GetType(Date) Then
                RowSeleccione(c.ColumnName) = 0
            ElseIf (Not c.AllowDBNull) And c.DataType Is GetType(String) Then
                RowSeleccione(c.ColumnName) = String.Empty
            ElseIf (Not c.AllowDBNull) And c.DataType Is GetType(Decimal) Then
                RowSeleccione(c.ColumnName) = 0
            ElseIf (Not c.AllowDBNull) And c.DataType Is GetType(Date) Then
                RowSeleccione(c.ColumnName) = Today
            Else
                RowSeleccione(c.ColumnName) = DBNull.Value
            End If
        Next

        Tbl.Rows.InsertAt(RowSeleccione, 0)

        ddl.DataSource = Tbl
        ddl.DataBind()

        Return Tbl
    End Function

#End Region

#Region "Encripta y Desencripta 17/12/2014, 03/01/2017"
    Public Shared Function fn_Encode(ByVal data As String) As String
        Try
            Dim encyrpt(0 To data.Length - 1) As Byte
            encyrpt = System.Text.Encoding.UTF8.GetBytes(data)
            Dim encodedata As String = Convert.ToBase64String(encyrpt)
            Return encodedata
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Shared Function fn_Decode(ByVal data As String) As String
        Try


            Dim encoder As New UTF8Encoding()
            Dim decode As Decoder = encoder.GetDecoder()
            Dim bytes As Byte() = Convert.FromBase64String(data)
            Dim count As Integer = decode.GetCharCount(bytes, 0, bytes.Length)
            Dim decodechar(0 To count - 1) As Char
            decode.GetChars(bytes, 0, bytes.Length, decodechar, 0)
            Dim result As New String(decodechar)
            Return result
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Shared Function fn_EncryptToSHA1(ByVal password As String) As String
        Try
            Dim sha As New System.Security.Cryptography.SHA1CryptoServiceProvider
            Dim bytesToHash() As Byte
            bytesToHash = System.Text.Encoding.ASCII.GetBytes(password)
            bytesToHash = sha.ComputeHash(bytesToHash)

            Dim encPassword As String = ""

            For Each b As Byte In bytesToHash
                encPassword += b.ToString("X2")
            Next
            Return encPassword
        Catch ex As Exception
            Return ""
        End Try
    End Function


#End Region

End Class
