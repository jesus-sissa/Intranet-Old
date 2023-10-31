<%@ Page Language="VB" AutoEventWireup="false" Inherits="IntranetSIAC.MensajesAdmin" Codebehind="MensajesAdmin.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<link href="App_Themes/Negro/Negro.css" rel="Stylesheet" type="text/css" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="600" cellpadding="2" cellspacing="2">
        <tr>
            <td align="right" rowspan="5" style="width: 131px">
                &nbsp;
            </td>
            <td align="right" rowspan="5">
            </td>
            <td align="left" valign="bottom" width="360" rowspan="1">
                <!--<h1 style="color: Black; font-family: Verdana; font-size: x-large;">
                    Intranet SIAC</h1>-->
            </td>
        </tr>
        <tr>
            <td>
                <h1 style="color: Black; font-family: Verdana; font-size: x-large;">
                    Intranet SIAC</h1>
            </td>
        </tr>
        <tr>
            <td align="left" width="500" colspan="2">
                <hr noshade="noshade" style="color: #7088b7; font: verdana;" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="middle">
            </td>
        </tr>
        <tr>
            <td align="left" valign="middle" width="500">
                <%--<h1 style="color: Black; font: 13pt/15pt verdana">
                    Lo sentimos !</h1>--%>
            </td>
        </tr>
        <tr>
            <td align="right" rowspan="5" style="width: 131px">
                &nbsp;
            </td>
            <td align="right" rowspan="5">
            </td>

            <!--<td width="500" colspan="2" style="color: black; font: 8pt/11pt verdana">
                <font style="color: black; font: 8pt/11pt verdana">La operación solicitada no pudo ser
                    completada . Puede </font><a href="Default.aspx" style="color: Red">Volver al Men&uacute; Principal</a>, o Enviar mensaje al Administrador.
            </td>-->
            <%--<td width="500" colspan="2" style="color: black; font: 8pt/11pt verdana">
                <font style="color: black; font: 8pt/11pt verdana">La operación solicitada no pudo ser
                    completada . Puede <a href="Default.aspx" style="color: Red">Volver al Men&uacute; Principal</a>, o <a href="MensajesAdmin.aspx?TipoM=1" style="color: Red">Enviar mensaje al Administrador</a></font>
            </td>--%>
        </tr>
        <tr>
            <td width="500" colspan="2" style="color: black; font: 8pt/11pt verdana">
                <font style="color: black; font: 8pt/11pt verdana">Mensaje para el Administrador</font>
            </td>
        </tr>
        <tr>
            <td>
                <%--<input type="text" size="70" name="Mensaje" style="height:100px" />--%>
                <textarea id="txt_Mensaje" runat="server" rows="10" cols="50" ></textarea>
            </td>
        </tr>
        
        <tr>
            <td width="500" colspan="2">
            </td>
        </tr>
        
        <tr>
            <td width="500" colspan="2" style="color: black; font: 8pt/11pt verdana">
                <font style="color: black; font: 8pt/11pt verdana"></font>
                <%--<input type="submit" value="Enviar Mensaje" />--%>
                <asp:Button runat="server" ID="btn_EnviarMensaje" Text="EnviarMensaje" CssClass="buttonU" />
            </td>
        </tr>

        <tr>
            <td>
            </td>
            <td>
            </td>
            <td width="500" colspan="2">
                <hr noshade="noshade" style="color: #7088b7; font: verdana;" />
                <font style="color: #c0c0c0; font: 8pt/11pt verdana"></font>
            </td>
        </tr>
        
    </table>
       <%-- <table>
            <tr>
                <td style="color: Black; font-family: Verdana; font-size: x-large;">
                    <asp:Label ID="lbl_Titulo" runat="server" Text="Intranet SIAC"></asp:Label>
                </td>
            </tr>
            <tr>
            <td width="500" colspan="2" style="color: black; font: 8pt/11pt verdana">
                <font style="color: black; font: 8pt/11pt verdana">Mensaje para el Administrador</font>
            </td>
        </tr>
        <tr>
            <td>
                <textarea id="txt_Mensaje" runat="server" rows="10" cols="50" ></textarea>
            </td>
        </tr>
        
        <tr>
            <td width="500" colspan="2">
            </td>
        </tr>
        <tr>
            <td width="500" colspan="2" style="color: black; font: 8pt/11pt verdana">
                <font style="color: black; font: 8pt/11pt verdana"></font>
                    <asp:Button runat="server" ID="btn_EnviarMensaje" Text="EnviarMensaje" />
            </td>
        </tr>
        </table>--%>
    </div>
    </form>
</body>
</html>
