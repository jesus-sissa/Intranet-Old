<%@ Control Language="VB" AutoEventWireup="false" Inherits="SISSAIntranet.wuc_PlantillaLaboral"
    CodeBehind="wuc_PlantillaLaboral.ascx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="../Estilos/StyleSheet.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Plantilla Laboral</title>
</head>
<body>
    <%--<br />--%>
    <asp:GridView ID="gv_Plantilla" runat="server" CssClass="gridSISSA2" AutoGenerateColumns="False"
        DataKeyNames="PlantillaId,DeptoId,PuestoID">
        <EmptyDataRowStyle BackColor="LightYellow" />
        <EmptyDataTemplate>
            No Existe Plantilla capturada
        </EmptyDataTemplate>
        <HeaderStyle BackColor="Black" ForeColor="White" />
        <Columns>
            <asp:CommandField ShowEditButton="True" ButtonType="Image" EditImageUrl="~/Imagenes/edit.gif"
                EditText="Editar Plantilla Requerida" ItemStyle-Width="80px" CancelImageUrl="~/Imagenes/cancel.gif"
                UpdateImageUrl="~/Imagenes/save.gif">
                <ItemStyle Width="80px" />
            </asp:CommandField>
            <asp:BoundField DataField="Puesto" HeaderText="Puesto" ReadOnly="True">
                <ItemStyle Width="300px" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Plantilla Requerida">
                <EditItemTemplate>
                    <asp:TextBox ID="tbx_GridPlantillaReq" runat="server" Width="100px" Style="text-align: center"
                        MaxLength="3" Text='<%# Bind("[Plantilla Requerida]") %>' ForeColor="Blue" BorderStyle="None"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender runat="server" ID="ftb_PReq" TargetControlID="tbx_GridPlantillaReq"
                        FilterType="Numbers">
                    </ajax:FilteredTextBoxExtender>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lbl_PlantillaReq" runat="server" Width="80px" Text='<%# Bind("[Plantilla Requerida]") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="Plantilla Actual" HeaderText="Plantilla Actual" ReadOnly="True">
                <ItemStyle HorizontalAlign="Center" Width="80px" />
            </asp:BoundField>
            <asp:BoundField DataField="DeptoId" HeaderText="DeptoId" Visible="False" />
            <asp:BoundField DataField="PuestoId" HeaderText="PuestoId" Visible="False" />
        </Columns>
        <SelectedRowStyle CssClass="SelectedRowSISSA" />
    </asp:GridView>
    <br />
    <table runat="server" id="tbl_Comentarios" visible="false" class="tablaSISSA2">
        <tr>
            <td valign="top">
                Comentarios
            </td>
            <td class="celdaMargenDer10">
                <asp:TextBox ID="tbx_Comentarios" runat="server" Width="410px" Height="100px" TextMode="MultiLine"
                    CausesValidation="true" MaxLength="150" Style="text-transform: uppercase"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_Comentarios" runat="server" ControlToValidate="tbx_Comentarios"
                    ForeColor="Red" ErrorMessage="Capture los Comentarios">*</asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    <div id="div1">
        <asp:UpdatePanel runat="server" ID="udp_AgregarDatos">
            <ContentTemplate>
                <asp:Panel ID="panel1" runat="server">
                    <div id="divEnca" style="width: 100%">
                        <strong>Agregar Puesto a Plantilla</strong>
                    </div>
                    <br />
                    <table id="tbl_AgregarPuesto" runat="server" class="tablaSISSA2" style="border-style: solid;
                        border-color: Black; border-width: 1px">
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Puesto
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:DropDownList ID="ddlPuestos" runat="server" AutoPostBack="true" Width="240px"
                                    CssClass="DropDownList18" CausesValidation="True">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvPuesto" runat="server" ControlToValidate="ddlPuestos"
                                    ValidationGroup="1" ForeColor="Red" ErrorMessage="Seleccione el Puesto a agregar.">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Plantilla Requerida
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:TextBox ID="tbx_PlantillaReq" runat="server" Width="70px" Height="14px" MaxLength="3"
                                    Style="text-align: center" CausesValidation="True"></asp:TextBox>
                                <ajax:FilteredTextBoxExtender ID="ftePlantillaReq" TargetControlID="tbx_PlantillaReq"
                                    runat="server" FilterType="Numbers">
                                </ajax:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="rfvPlantillaReq" runat="server" ControlToValidate="tbx_PlantillaReq"
                                    ValidationGroup="1" ForeColor="Red" ErrorMessage="Capture la Plantilla Requerida.">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Plantilla Actual
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:TextBox ID="tbx_PlantillaAct" runat="server" Width="70px" Height="14px" MaxLength="3"
                                    Style="text-align: center" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Comentarios
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:TextBox ID="tbx_ComentariosAgregar" runat="server" Width="410px" Height="100px"
                                    TextMode="MultiLine" CausesValidation="true" MaxLength="150" Style="text-transform: uppercase"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbx_ComentariosAgregar"
                                    ForeColor="Red" ErrorMessage="Capture los Comentarios" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td class="celdaMargenDer10">
                                <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" ValidationGroup="1" Width="80px" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:ValidationSummary ID="ValidSum" runat="server" ShowMessageBox="true" ShowSummary="false" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="true"
        ShowSummary="false" ValidationGroup="1" />
</body>
</html>
