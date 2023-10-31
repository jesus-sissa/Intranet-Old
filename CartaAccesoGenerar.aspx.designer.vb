'------------------------------------------------------------------------------
' <generado automáticamente>
'     Este código fue generado por una herramienta.
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código. 
' </generado automáticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class CartaAccesoGenerar

    '''<summary>
    '''Control lbl_Tipo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Tipo As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control rdb_Empleado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rdb_Empleado As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control rdb_Visitante.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rdb_Visitante As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control lbl_Empleado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Empleado As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control ddl_Empleado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddl_Empleado As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control lbl_Nombre.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Nombre As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control tbx_Nombre.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_Nombre As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control lbl_Empresa.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Empresa As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control tbx_Empresa.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_Empresa As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control btn_Agregar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btn_Agregar As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control gv_Empleados.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gv_Empleados As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control lbl_Fecha.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Fecha As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control tbx_FechaInicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_FechaInicio As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control ftb_FechaInicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ftb_FechaInicio As Global.AjaxControlToolkit.FilteredTextBoxExtender

    '''<summary>
    '''Control cal_FechaInicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cal_FechaInicio As Global.AjaxControlToolkit.CalendarExtender

    '''<summary>
    '''Control lbl_TipoCarta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_TipoCarta As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control rdb_NuevoIngreso.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rdb_NuevoIngreso As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control rdb_FaltaRetardo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rdb_FaltaRetardo As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control rdb_Externo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rdb_Externo As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control rdb_Otro.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rdb_Otro As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control lbl_Asunto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Asunto As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control tbx_Asunto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_Asunto As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control lbl_Autoriza.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Autoriza As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control ddl_Autoriza.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddl_Autoriza As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control lbl_Dirigida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Dirigida As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control ddl_Dirigida.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddl_Dirigida As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control btn_Guardar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btn_Guardar As Global.System.Web.UI.WebControls.Button
End Class
