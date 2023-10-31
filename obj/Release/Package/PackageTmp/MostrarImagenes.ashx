<%@ WebHandler Language="VB" Class="MostrarImagenes" %>

Imports System
Imports System.Web
Imports System.Data
Imports System.Data.SqlClient
Imports IntranetSIAC.Cn_Datos

Public Class MostrarImagenes : Implements IHttpHandler
        
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        
        Dim cnn As SqlConnection = Crea_ConexionSTD()
        cnn.Open()
        
        Dim sql As String = "Select Id_RIAI, ID_RIA, Imagen from SIACI..Cat_RIAI where Id_RIAI=@Id_RIAI"
        Dim cmd As New SqlCommand(sql, cnn)
        
        cmd.Parameters.Add("@Id_RIAI", SqlDbType.Int).Value = context.Request.QueryString("id")
        cmd.Prepare()
        
        Dim dr As SqlDataReader = cmd.ExecuteReader()
        dr.Read()
        context.Response.ContentType = dr("Id_RIAI").ToString()
        context.Response.BinaryWrite(DirectCast(dr("Imagen"), Byte()))
        dr.Close()
        cnn.Close()

    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class