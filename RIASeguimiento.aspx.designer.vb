﻿'------------------------------------------------------------------------------
' <generado automáticamente>
'     Este código fue generado por una herramienta.
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código. 
' </generado automáticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class RIASeguimiento

    '''<summary>
    '''Control gv_RIA.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gv_RIA As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control gv_Usuarios.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gv_Usuarios As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control PlaceHolderSeguimiento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents PlaceHolderSeguimiento As Global.System.Web.UI.WebControls.PlaceHolder

    '''<summary>
    '''Control lbl_UsuarioA.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_UsuarioA As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control ddl_UsuarioA.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddl_UsuarioA As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control lbl_TipoUsuario.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_TipoUsuario As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control rdb_Principal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rdb_Principal As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control rdb_Secundario.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rdb_Secundario As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control btn_Asignar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btn_Asignar As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control udp_Comentarios.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents udp_Comentarios As Global.System.Web.UI.UpdatePanel

    '''<summary>
    '''Control pnl_Comentarios.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnl_Comentarios As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control lbl_Fecha_Inicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Fecha_Inicio As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control tbx_FechaSeguimiento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_FechaSeguimiento As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control FilteredTextBoxExtender2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents FilteredTextBoxExtender2 As Global.AjaxControlToolkit.FilteredTextBoxExtender

    '''<summary>
    '''Control CalendarExtender1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents CalendarExtender1 As Global.AjaxControlToolkit.CalendarExtender

    '''<summary>
    '''Control lbl_Hora_Seguimiento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Hora_Seguimiento As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control ddl_HoraSeguimiento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddl_HoraSeguimiento As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control ddl_MinSeguimiento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddl_MinSeguimiento As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control lbl_Descripcion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Descripcion As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control tbx_Descripcion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_Descripcion As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control rfv_Descripcion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfv_Descripcion As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control pnl_AgregarI.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnl_AgregarI As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control lbl_Mensaje.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Mensaje As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control Label5.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label5 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lbl_Imagen.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Imagen As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control FileUpload1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents FileUpload1 As Global.System.Web.UI.WebControls.FileUpload

    '''<summary>
    '''Control btn_Subir.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btn_Subir As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control hfd_IDRIA.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfd_IDRIA As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control btn_Finalizar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btn_Finalizar As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control hfd_IDRIAD.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfd_IDRIAD As Global.System.Web.UI.WebControls.HiddenField
End Class
