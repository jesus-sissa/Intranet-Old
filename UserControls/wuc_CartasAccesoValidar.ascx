<%@ Control Language="VB" AutoEventWireup="false" Inherits="SISSAIntranet.UserControls_wuc_CartasAccesoValidar"
    CodeBehind="wuc_CartasAccesoValidar.ascx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../Estilos/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="Div_Principal">
        <%--<br />
        <div id="divEnca" title="prueba">
            <strong>Validación de Cartas de Acceso</strong>
        </div>--%>
        <br />
        <div>
            <asp:GridView ID="gv_CartasAcceso" runat="server" AutoGenerateColumns="False" DataKeyNames="Id_Carta,Observaciones"
                AllowPaging="True" Width="100%" Style="color: Black; font-family: Verdana; font-size: x-small">
                <Columns>
                    <asp:BoundField DataField="Id_Carta" HeaderText="CartaID" Visible="False">
                        <ItemStyle HorizontalAlign="Right" Wrap="True" />
                    </asp:BoundField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select"
                                ImageUrl="~/Imagenes/1rightarrow.png" Text="Seleccionar" />
                        </ItemTemplate>
                        <ItemStyle Width="15px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="FechaRegistro" HeaderText="FechaRegistro">
                        <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="UsuarioRegistro" HeaderText="UsuarioRegistro"></asp:BoundField>
                    <asp:BoundField DataField="FechaInicio" HeaderText="FechaInicio">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FechaFin" HeaderText="FechaFin">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Status">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" Visible="False">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" Visible="False" />
                </Columns>
                <SelectedRowStyle CssClass="SelectedRowSISSA" />
                <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
            </asp:GridView>
            <br />
            <table class="tablaSISSA3" width="100%">
                <tr>
                    <td>
                        <b>
                            <asp:Label ID="Label1" runat="server" Text="Observaciones"></asp:Label></b>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;">
                        <asp:TextBox CssClass="textbox1" ID="tbx_Observaciones" runat="server" Width="100%"
                            Height="85px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <asp:GridView runat="server" ID="gv_Detalle" Width="100%" CssClass="gridSISSA" AutoGenerateColumns="False"
                DataKeyNames="Id_Carta">
                <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
                <Columns>
                    <asp:BoundField DataField="Id_Carta" HeaderText="Id_Carta" Visible="False">
                        <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Clave" HeaderText="Clave">
                        <ItemStyle Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="Empresa" HeaderText="Empresa" />
                </Columns>
                <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
            </asp:GridView>
        </div>
        <br />
        <br />
        <div style="float: left; border-style: solid; border-color: Gray; border-width: thin;
            border-top: none">
            <div id="divEncaComentarios" title="prueba">
                <b>
                    <asp:Label ID="lbl_EncabezadoComentarios" runat="server" Text="Capture Comentarios"></asp:Label></b>
            </div>
            <asp:Panel ID="pnl_Firma" runat="server" ForeColor="Black" Visible="True" Width="520px"
                Style="border-style: solid; border-width: 1px">
                <br />
                <table class="tablaSISSA1">
                    <tr>
                        <td valign="top" style="text-align: right; width: 94px; height: 85px">
                            Comentarios
                        </td>
                        <td class="celdaMargenDer10" style="text-align: left;" colspan="2">
                            <asp:TextBox ID="tbx_Comentarios" runat="server" Height="85px" TextMode="MultiLine"
                                Style="text-transform: uppercase" Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            Contraseña
                        </td>
                        <td class="celdaMargenDer10">
                            <asp:TextBox ID="tbx_Contrasena" runat="server" Style="margin-left: 0px" TextMode="Password"
                                CssClass="Textbox14"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv_Contrasena" runat="server" ControlToValidate="tbx_Contrasena"
                                ErrorMessage="Introduzca su Contraseña">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="celdaMargenDer10" style="text-align: left;">
                            <asp:Button ID="btn_Validar" runat="server" Text="Validar" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
        </div>
    </div>
</body>
</html>
