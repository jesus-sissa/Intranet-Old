
Imports System.Data
Imports System.Net.Mail
Imports System.Security.Cryptography

Public Class Cn_Mail

    Public Shared Function fn_Sucursales_Read(ByVal SucursalId As Integer) As DataTable
        Dim Cnn As SqlClient.SqlConnection = Cn_Datos.Crea_ConexionSTD
        Dim Cmd As SqlClient.SqlCommand = Cn_Datos.Crea_Comando("Cat_ParametrosL_Read", CommandType.StoredProcedure, Cnn)
        Cn_Datos.Crea_Parametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
        Dim Tbl As DataTable = Cn_Datos.EjecutaConsulta(Cmd)
        Cmd.Dispose()
        Return Tbl
    End Function

    Public Shared Function fn_Enviar_Mail(ByVal Destino As String, ByVal Asunto As String, ByVal Texto As String, ByVal SucursalId As Integer) As Boolean
        Dim MailServer As String
        Dim MailRemitente As String
        Dim MailRemitenteNombre As String
        Dim MailUser As String
        Dim MailClaveEnc As String
        Dim MailClave As String
        Dim Hubo_Destinos As Boolean = False
        Dim SMTP As New System.Net.Mail.SmtpClient 'Variable con la que se envia el correo
        Dim CORREO As New System.Net.Mail.MailMessage 'Variable que amlmacena los Attachment

        Dim Tabla As DataTable
        Tabla = fn_Sucursales_Read(SucursalId)
        If Tabla.Rows.Count = 0 Then
            CORREO.Dispose()
            fn_Enviar_Mail = False
            Exit Function
        End If
        If Tabla.Rows(0)("Mail_Server") = "" Or Tabla.Rows(0)("Mail_User") = "" Or Tabla.Rows(0)("Mail_Clave") = "" Or Tabla.Rows(0)("Mail_Remitente") = "" Or Tabla.Rows(0)("Mail_RemitenteNombre") = "" Then
            CORREO.Dispose()
            fn_Enviar_Mail = False
            Exit Function
        End If
        MailServer = Tabla.Rows(0)("Mail_Server")
        MailUser = Tabla.Rows(0)("Mail_User")
        MailClaveEnc = Tabla.Rows(0)("Mail_Clave")
        MailRemitente = Tabla.Rows(0)("Mail_Remitente")
        MailRemitenteNombre = Tabla.Rows(0)("Mail_RemitenteNombre")
        MailClave = Cn_Encripta.fn_Decode(MailClaveEnc)

        'Configuracion del Mensaje
        CORREO.From = New System.Net.Mail.MailAddress(MailRemitente, MailRemitenteNombre, System.Text.Encoding.UTF8)
        Dim Destinos = Split(Destino, ",")
        For Ilocal As Integer = 0 To Destinos.Length - 1
            'CORREO.[To].Add(Destino)
            If FuncionesGlobales.fn_ValidarMail(Destinos(Ilocal).Trim) Then
                CORREO.[To].Add(Destinos(Ilocal).Trim)
                Hubo_Destinos = True
            End If
        Next Ilocal
        CORREO.Subject = Asunto
        CORREO.IsBodyHtml = False
        CORREO.Body = Texto


        SMTP.Host = MailServer
        SMTP.UseDefaultCredentials = False
        SMTP.Credentials = New System.Net.NetworkCredential(MailUser, MailClave)

        If Not Hubo_Destinos Then
            fn_Enviar_Mail = False
            CORREO.Dispose()
            'FuncionesGlobales.fn_GuardaError("Cn_Mail - SMTP.Send", 0, "NO SE ESPECIFICO NINGUN DESTINATARIO. " & Asunto)
            Exit Function
        End If

        Try
            SMTP.Send(CORREO)
            fn_Enviar_Mail = True
        Catch ex As System.Net.Mail.SmtpException
            fn_Enviar_Mail = False
            'FuncionesGlobales.fn_GuardaError("Cn_Mail - SMTP.Send", 0, ex.Message, False)
        Finally
            CORREO.Dispose()
        End Try
    End Function

    Public Shared Function fn_Enviar_MailFallas(ByVal Asunto As String, ByVal Texto As String, ByVal Adjunto As String, ByVal SucursalId As Integer) As Boolean
        Dim MailServer As String
        Dim MailRemitente As String
        Dim MailRemitenteNombre As String
        Dim MailUser As String
        Dim MailClaveEnc As String
        Dim MailClave As String
        Dim Hubo_Destinos As Boolean = False
        Dim Destino As String = ""
        Dim SMTP As New System.Net.Mail.SmtpClient 'Variable con la que se envia el correo
        Dim CORREO As New System.Net.Mail.MailMessage 'Variable que amlmacena los Attachment
        Dim Tabla As DataTable

        Tabla = fn_Sucursales_Read(SucursalId)
        If Tabla.Rows.Count = 0 Then
            CORREO.Dispose()
            fn_Enviar_MailFallas = False
            Exit Function
        End If
        If Tabla.Rows(0)("Mail_Server") = "" Or Tabla.Rows(0)("Mail_User") = "" Or Tabla.Rows(0)("Mail_Clave") = "" Or Tabla.Rows(0)("Mail_Remitente") = "" Or Tabla.Rows(0)("Mail_RemitenteNombre") = "" Or Tabla.Rows(0)("Mail_ReporteFallas") = "" Then
            CORREO.Dispose()
            fn_Enviar_MailFallas = False
            Exit Function
        End If
        MailServer = Tabla.Rows(0)("Mail_Server")
        MailUser = Tabla.Rows(0)("Mail_User")
        MailClaveEnc = Tabla.Rows(0)("Mail_Clave")
        MailRemitente = Tabla.Rows(0)("Mail_Remitente")
        MailRemitenteNombre = Tabla.Rows(0)("Mail_RemitenteNombre")
        MailClave = Cn_Encripta.fn_Decode(MailClaveEnc)
        Destino = Tabla.Rows(0)("Mail_ReporteFallas")

        'Configuracion del Mensaje
        CORREO.From = New System.Net.Mail.MailAddress(MailRemitente, MailRemitenteNombre, System.Text.Encoding.UTF8)
        'Agregar los Destinatarios uno a uno
        Dim Destinos = Split(Destino, ",")
        For Ilocal As Integer = 0 To Destinos.Length - 1
            'CORREO.[To].Add(Destino)
            If FuncionesGlobales.fn_ValidarMail(Destinos(Ilocal).Trim) Then
                CORREO.[To].Add(Destinos(Ilocal).Trim)
                Hubo_Destinos = True
            End If
        Next Ilocal
        CORREO.Subject = Asunto
        CORREO.IsBodyHtml = False
        CORREO.Body = Texto
        If Adjunto <> "" Then
            CORREO.Attachments.Add(New System.Net.Mail.Attachment(Adjunto))
        End If

        SMTP.Host = MailServer
        SMTP.UseDefaultCredentials = False
        SMTP.Credentials = New System.Net.NetworkCredential(MailUser, MailClave)

        'Si no tiene destino se sale sin intentar el envío
        If Not Hubo_Destinos Then
            fn_Enviar_MailFallas = False
            CORREO.Dispose()
            'FuncionesGlobales.fn_GuardaError("Cn_Mail - SMTP.Send", 0, "NO SE ESPECIFICO NINGUN DESTINATARIO. " & Asunto, False)
            Exit Function
        End If

        Try
            SMTP.Send(CORREO)
            fn_Enviar_MailFallas = True
        Catch ex As System.Net.Mail.SmtpException
            fn_Enviar_MailFallas = False
            'FuncionesGlobales.fn_GuardaError("Cn_Mail - SMTP.Send", 0, ex.Message, False)
        Finally
            CORREO.Dispose()
        End Try
    End Function

    Public Shared Function fn_Enviar_MailHTML(ByVal Destino As String, ByVal Asunto As String, ByVal Texto As String, ByVal Adjunto As String, ByVal SucursalId As Integer) As Boolean
        Dim MailServer As String
        Dim MailRemitente As String
        Dim MailRemitenteNombre As String
        Dim MailUser As String
        Dim MailClaveEnc As String
        Dim MailClave As String
        Dim Hubo_Destinos As Boolean = False
        Dim SMTP As New System.Net.Mail.SmtpClient 'Variable con la que se envia el correo
        Dim CORREO As New System.Net.Mail.MailMessage 'Variable que amlmacena los Attachment

        Dim Tabla As DataTable
        Tabla = fn_Sucursales_Read(SucursalId)
        If Tabla.Rows.Count = 0 Then
            CORREO.Dispose()
            fn_Enviar_MailHTML = False
            Exit Function
        End If
        If Tabla.Rows(0)("Mail_Server") = "" Or Tabla.Rows(0)("Mail_User") = "" Or Tabla.Rows(0)("Mail_Clave") = "" Or Tabla.Rows(0)("Mail_Remitente") = "" Or Tabla.Rows(0)("Mail_RemitenteNombre") = "" Then
            CORREO.Dispose()
            fn_Enviar_MailHTML = False
            Exit Function
        End If
        MailServer = Tabla.Rows(0)("Mail_Server")
        MailUser = Tabla.Rows(0)("Mail_User")
        MailClaveEnc = Tabla.Rows(0)("Mail_Clave")
        MailRemitente = Tabla.Rows(0)("Mail_Remitente")
        MailRemitenteNombre = Tabla.Rows(0)("Mail_RemitenteNombre")
        MailClave = Cn_Encripta.fn_Decode(MailClaveEnc)

        'Configuracion del Mensaje
        CORREO.From = New System.Net.Mail.MailAddress(MailRemitente, MailRemitenteNombre, System.Text.Encoding.UTF8)
        'CORREO.[To].Add(Destino)
        Dim Destinos = Split(Destino, ",")
        For Ilocal As Integer = 0 To Destinos.Length - 1
            'CORREO.[To].Add(Destino)
            If Destinos(Ilocal).Trim <> "" Then
                CORREO.[To].Add(Destinos(Ilocal).Trim)
                Hubo_Destinos = True
            End If
        Next Ilocal
        CORREO.Subject = Asunto
        CORREO.IsBodyHtml = True
        CORREO.Body = Texto

        Dim Att As System.Net.Mail.Attachment

        If Adjunto <> "" Then
            'CORREO.Attachments.Add(New System.Net.Mail.Attachment(Adjunto))
            Att = New System.Net.Mail.Attachment(Adjunto)
            Att.ContentId = 1
            CORREO.Attachments.Add(Att)
        End If

        SMTP.Host = MailServer
        SMTP.UseDefaultCredentials = False
        SMTP.Credentials = New System.Net.NetworkCredential(MailUser, MailClave)

        If Not Hubo_Destinos Then
            fn_Enviar_MailHTML = False
            CORREO.Dispose()
            'FuncionesGlobales.fn_GuardaError("Cn_Mail(fn_Enviar_MailHTML) - SMTP.Send", 0, "NO SE ESPECIFICO NINGUN DESTINATARIO. " & Asunto, False)
            Exit Function
        End If

        Try
            SMTP.Timeout = 20000
            SMTP.Port = 587
            SMTP.Send(CORREO)
            fn_Enviar_MailHTML = True
        Catch ex As System.Net.Mail.SmtpException
            fn_Enviar_MailHTML = False
            'FuncionesGlobales.fn_GuardaError("Cn_Mail(fn_Enviar_MailHTML) - SMTP.Send", 0, ex.Message, False)
            CORREO.Dispose()
        End Try
    End Function

End Class
