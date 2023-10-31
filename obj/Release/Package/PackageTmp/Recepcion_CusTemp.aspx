<%@ Page Title="CUSTODIAS TEMPORALES" Language="vb" AutoEventWireup="false" MasterPageFile="~/MP_Principal.master"
    CodeBehind="Recepcion_CusTemp.aspx.vb" Inherits="IntranetSIAC.Recepcion_CusTemp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="Div_Principal">
        <br />
        <table class="tablaSISSA2">
            <tr>
                <td align="right" style="width: 120px">
                    <asp:Label ID="lbl_Compania" runat="server" Text="Compañía"></asp:Label>
                </td>
                <td class="celdaMargenDer10" colspan="3">
                    <asp:DropDownList ID="ddl_Cia" runat="server" CssClass="DropDownList18" AutoPostBack="true"
                        Width="300px" DataTextField="Alias" DataValueField="Id_Cia">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lbl_Boveda" runat="server" Text="Bóveda"></asp:Label>
                </td>
                <td class="celdaMargenDer10" colspan="3">
                    <asp:DropDownList ID="ddl_Boveda" runat="server" CssClass="DropDownList18" AutoPostBack="true"
                        Width="300px" DataTextField="Descripcion" DataValueField="Id_Boveda">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lbl_Cliente" runat="server" Text="Cliente"></asp:Label>
                </td>
                <td class="celdaMargenDer10" colspan="3">
                    <asp:DropDownList ID="ddl_Cliente" runat="server" CssClass="DropDownList18" AutoPostBack="true"
                        Width="300px" DataTextField="Nombre_Comercial" DataValueField="Id_ClienteP">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lbl_CajaBancaria" runat="server" Text="Caja Bancaria"></asp:Label>
                </td>
                <td class="celdaMargenDer10" colspan="3">
                    <asp:DropDownList ID="ddl_CajaBancaria" runat="server" CssClass="DropDownList18"
                        AutoPostBack="true" Width="300px" DataTextField="Nombre_Comercial" DataValueField="Id_CajaBancaria">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
        <table class="tablaSISSA2">
            <tr>
                <td style="width: 120px">
                    <asp:Label ID="lblNumeroRemision" runat="server" Text="Número Remisión"></asp:Label>
                </td>
                <td class="celdaMargenDer10" colspan="3" style="width: 350px">
                    <asp:TextBox ID="tbx_NumRemision" runat="server" Width="100px" CssClass="Textbox14"
                        CausesValidation="true"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender runat="server" ID="ftb_NumRemision" TargetControlID="tbx_NumRemision"
                        FilterType="Numbers">
                    </ajax:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lbl_Moneda" runat="server" Text="Moneda"></asp:Label>
                </td>
                <td class="celdaMargenDer10">
                    <asp:DropDownList ID="ddl_Monedas" runat="server" CssClass="DropDownList18" AutoPostBack="true"
                        Width="105px" DataTextField="Moneda" DataValueField="Id_Moneda">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbl_TipoCambio" runat="server" Text="Tipo Cambio"></asp:Label>
                </td>
                <td class="celdaMargenDer10" colspan="2">
                    <asp:TextBox ID="tbx_TipoCambio" runat="server" Width="100px" CssClass="Textbox14"
                        ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_ImporteEfectivo" runat="server" Text="Importe Efectivo"></asp:Label>
                </td>
                <td class="celdaMargenDer10" colspan="2">
                    <asp:TextBox ID="tbx_ImporteEfectivo" runat="server" Width="100px" CssClass="Textbox14"
                        CausesValidation="true"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftb_ImporteEfectivo" runat="server" TargetControlID="tbx_ImporteEfectivo"
                        FilterType="Custom" ValidChars="0123456789.">
                    </ajax:FilteredTextBoxExtender>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_ImporteDocumentos" runat="server" Text="Importe Documentos"></asp:Label>
                </td>
                <td class="celdaMargenDer10" colspan="2">
                    <asp:TextBox ID="tbx_ImporteDocumentos" runat="server" Width="100px" CssClass="Textbox14"
                        CausesValidation="true"></asp:TextBox>
                    <ajax:FilteredTextBoxExtender ID="ftb_ImporteDoctos" runat="server" TargetControlID="tbx_ImporteDocumentos"
                        FilterType="Custom" ValidChars="0123456789.">
                    </ajax:FilteredTextBoxExtender>
                </td>
                <td align="left">
                    <asp:Button runat="server" ID="btn_AgregarMoneda" Text="Agregar" Width="90px" CssClass="buttonB" />
                </td>
            </tr>
        </table>
        <asp:GridView runat="server" ID="gv_Monedas" 
            CssClass="gv_general" Width="65%" DataKeyNames="Id_Moneda,TipoCambio"
            AutoGenerateColumns="False">
            <Columns>
                <asp:CommandField ButtonType="Image" ShowDeleteButton="True" DeleteImageUrl="~/Imagenes/HoraNo.png">
                    <ItemStyle Width="30px" />
                </asp:CommandField>
                <asp:BoundField DataField="Moneda" HeaderText="Moneda" >
                <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Importe Efectivo" HeaderText="Importe Efectivo" >
                <HeaderStyle HorizontalAlign="Right" />
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Importe Documentos" HeaderText="Importe Documento" >
                <HeaderStyle HorizontalAlign="Right" />
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="TipoCambio" HeaderText="TipoCambio" 
                    Visible="False" />
            </Columns>
            <RowStyle CssClass="rowHover" />
  <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />


        </asp:GridView>
        <br />
        <fieldset style="width: 65%">
            <table width="80%" class="tablaSISSA3">
                <tr>
                    <td style="width: 100px">
                        <asp:Label ID="lblNumero" runat="server" Text="Número"></asp:Label>
                    </td>
                    <td style="width: 100px">
                        <asp:Label ID="lbl_TipoEnvase" runat="server" Text="Tipo Envase"></asp:Label>
                    </td>
                    <td style="width:100px"></td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="tbx_NumEnvase" runat="server" Width="100px" CssClass="tbx_Mayusculas"></asp:TextBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_TipoEnvase" runat="server" CssClass="DropDownList18" AutoPostBack="true"
                            Width="100px" DataTextField="Descripcion" DataValueField="Id_TipoE">
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:Button runat="server" ID="btn_Agregar" Text="Agregar" Width="90px" CssClass="buttonB" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:GridView ID="gv_Envases" runat="server"
                             AutoGenerateColumns="False" CssClass="gv_general"
                            DataKeyNames="Id_TipoE" Width="100%">
                       

                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" 
                                            CommandName="Delete" ImageUrl="~/Imagenes/HoraNo.png" Text="Eliminar" />
                                    </ItemTemplate>
                                    <ItemStyle Width="30px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Numero" HeaderText="Número">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TipoEnvase" HeaderText="Tipo Envase">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                            </Columns>

                            <RowStyle CssClass="rowHover" />
 <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
  <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
   <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lbl_EnvasesSN" Text="Envases S/N"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbl_TotalEnases" Text="Total Envases"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox runat="server" ID="tbx_EnvasesSN" Width="100px" CssClass="Textbox14" 
                            CausesValidation="true"></asp:TextBox>
                        <ajax:FilteredTextBoxExtender runat="server" ID="ftb_EnvasesSN" TargetControlID="tbx_EnvasesSN"
                            FilterType="Numbers">
                        </ajax:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbl_TotalEnvSuma" Text="0"></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Button runat="server" ID="btn_Guardar" Text="Guardar" Width="90px" CssClass="buttonB" />
                    </td>
                </tr>
                </table>
        </fieldset>
        <br />
    </div>
</asp:Content>
