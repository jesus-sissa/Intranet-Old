<%@ Control Language="VB" AutoEventWireup="false" Inherits="SISSAIntranet.wuc_EnviarMensajes"
    CodeBehind="wuc_EnviarMensajes.ascx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="../Estilos/StyleSheet.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">

<script type="text/javascript" language="javascript">

    function SelectAllCheckboxes(spanChk) {

        // Added as ASPX uses SPAN for checkbox
        var oItem = spanChk.children;
        var theBox = (spanChk.type == "checkbox") ?
        spanChk : spanChk.children.item[0];
        xState = theBox.checked;
        elm = theBox.form.elements;

        for (i = 0; i < elm.length; i++)
            if (elm[i].type == "checkbox" &&
              elm[i].id != theBox.id) {
            //elm[i].click();
            if (elm[i].checked != xState)
                elm[i].click();
            //elm[i].checked=xState;
        }
    }
</script>

<head>
    <title>ENVIAR MENSAJES</title>
</head>
<body>
    <%--<br />
    <div id="divEnca" title="prueba">
        <strong>Enviar Mensajes</strong>
    </div>--%>
    <br />
    <div style="float: left; width: 400px">
        <ajax:TabContainer runat="server" Width="100%" ActiveTabIndex="0" Height="550px"
            AutoPostBack="true" ID="tbc_Destinos">
            <ajax:TabPanel runat="server" ID="tab_Usuarios" HeaderText="Por Usuarios">
                <ContentTemplate>
                    <table style="width: 100%">
                        <tr style="width: 400px">
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" Text="Usuario" Style="font-family: Verdana; font-size: x-small"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList runat="server" ID="ddl_Usuario" Width="100%" CssClass="DropDownList18">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button runat="server" ID="btn_Agregar" Text="Agregar" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView runat="server" ID="gv_Usuarios" Width="100%" CssClass="gridSISSA" Height="100%"
                                    AutoGenerateColumns="False" DataKeyNames="Id_Empleado">
                                    <Columns>
                                        <asp:BoundField DataField="Id_Empleado" HeaderText="Id_Empleado" Visible="False">
                                            <ItemStyle Width="50px" />
                                        </asp:BoundField>
                                        <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Imagenes/EliminarEmpleado.png"
                                            ShowDeleteButton="True">
                                            <ItemStyle Width="30px" />
                                        </asp:CommandField>
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                    </Columns>
                                    <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                                    <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </ajax:TabPanel>
            <ajax:TabPanel runat="server" ID="tab_Modulos" Height="500px" HeaderText="Por Módulos">
                <ContentTemplate>
                    <asp:GridView runat="server" ID="gv_Modulos" Width="100%" CssClass="gridSISSA" Height="100%"
                        AutoGenerateColumns="False" DataKeyNames="Clave">
                        <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                        <Columns>
                            <asp:BoundField DataField="Clave" HeaderText="Clave" Visible="False">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbx_Modulo" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="30px" />
                                <HeaderTemplate>
                                    <input id="chkAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server"
                                        type="checkbox" />
                                </HeaderTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                        </Columns>
                        <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
                    </asp:GridView>
                </ContentTemplate>
            </ajax:TabPanel>
        </ajax:TabContainer>
    </div>
    <div style="float: left; width: 400px; margin-left: 20px">
        <table style="width: 100%; font-family: Verdana; font-size: x-small">
            <tr>
                <td>
                    <asp:Label ID="lbl_Asunto" runat="server" Text="Asunto"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="tbx_Asunto" Width="100%" CssClass="tbx_Mayusculas"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Mensaje"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="tbx_Mensaje" runat="server" TextMode="MultiLine" Style="width: 100%;
                        height: 520px; text-transform: uppercase; font-family: Verdana"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Button runat="server" ID="btn_Enviar" Text="Enviar" Width="100px" />
                </td>
            </tr>
        </table>
        <br />
    </div>
</body>
</html>
