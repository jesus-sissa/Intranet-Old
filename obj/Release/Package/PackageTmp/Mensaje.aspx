<%@ Page Language="VB" AutoEventWireup="false" Inherits="IntranetSIAC._Mensaje" Codebehind="Mensaje.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <link href="App_Themes/Tema1/Mensajes.css" rel="stylesheet" type="text/css" />

    <title>Mensajes Ajax</title>
</head>
<body>
    <form id="form1" runat="server">
    <cc1:ToolkitScriptManager ID="mensajesToolkitScriptManager" runat="server">
    </cc1:ToolkitScriptManager>
    <asp:UpdatePanel ID="mensajesUpdatePanel" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
    <div>
    <%--<center>
        <table>
            <tbody>
                <tr>
                    <td>
                        <asp:Button ID="correctoButton" runat="server" Text="Mensaje Correcto" />
                    </td>
                    <td>
                        <asp:Button ID="advertenciaButton" runat="server" Text="Mensaje Advertencia" />
                    </td>
                    <td>
                        <asp:Button ID="errorButton" runat="server" Text="Mensaje Error" 
                            Height="26px" />
                    </td>
                </tr>
            </tbody>
        </table>
    </center>--%>
    
    <!--
    MENSAJE POPUP
-->           
<asp:Panel ID="pnlMensaje" runat="server" CssClass="CajaDialogo"  Style="display: none;">
                    <asp:Panel ID="mensajeheaderPanel" runat="server">
                    <table id="mensajetable" width="430px" style="margin: 0px; padding: 0px; height:auto;">
                        <tr>
                            <td align="left" valign="middle">
                                <asp:Label ID="Label21" runat="server" Text="MENSAJE DE INFORMACION"  
                                    Font-Bold="true"/>
                            </td>
                            <td valign="middle">
                                <asp:ImageButton ID="cerrarmensajeImageButton" runat="server" 
                                    ImageUrl="~/App_Themes/Tema1/Imagenes/dialog_close.gif" Width="14px"
                                    ToolTip="Cerrar mensaje" />
                            </td>
                        </tr>
                    </table>
                    </asp:Panel>
                    
                    <table align="left" style="margin: 0px 0px 0px 0px;">
                        <tr>
                           <td>
                                <asp:Label ID="mensajeLabel" runat="server" 
                                    Font-Size="13px" Text="Datos de usuario incorrectos" />
                            </td> 
                        </tr>
                    </table>
 </asp:Panel>
                
 <cc1:ModalPopupExtender ID="mpeMensaje" runat="server" 
    TargetControlID="lblOculto"
    PopupControlID="pnlMensaje" 
    BackgroundCssClass="FondoAplicacion" 
    OkControlID="cerrarmensajeImageButton">
 </cc1:ModalPopupExtender>
                   
<asp:Label ID="lblOculto" runat="server" Text="Label" Style="display: none;" > </asp:Label>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>

    </body>
</html>
