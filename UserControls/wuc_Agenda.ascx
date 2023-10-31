<%@ Control Language="VB" AutoEventWireup="false" Inherits="SISSAIntranet.wuc_Agenda" Codebehind="wuc_Agenda.ascx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../Estilos/StyleSheet.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <div class="Div_Principal">
        <br />
        <div id="divEnca">
            <strong>Agenda</strong>
        </div>
        <br />
        <div>
            <ajax:CalendarExtender runat="server"></ajax:CalendarExtender>
        </div>
    </div>
</body>
</html>