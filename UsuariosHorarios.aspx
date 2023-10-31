<%@ Page Title="HORARIOS DE ACCESO" Language="vb" MasterPageFile="~/MP_Principal.master" AutoEventWireup="false"
    CodeBehind="UsuariosHorarios.aspx.vb" Inherits="IntranetSIAC.UsuariosHorarios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="ContentUPrivilegios" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">
    <div class="Div_Principal">
        <br />
        <table class="tablaSISSA1">
            <tr>
                <td style="width: 100px" align="right">
                    <asp:Label ID="lbl_Usuario" runat="server" Text="Usuario"></asp:Label>
                </td>
                <td class="celdaMargenDer10">
                    <asp:DropDownList runat="server" ID="ddl_Usuarios" CssClass="DropDownList18" Width="300"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <asp:GridView ID="gv_Horarios" runat="server"
            AutoGenerateColumns="False" CssClass="gv_general"
            DataKeyNames="Id_Dia" Width="60%">

            <Columns>
                <asp:BoundField DataField="Dia" HeaderText="Dia" SortExpression="Dia">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="100px" />
                </asp:BoundField>

                <asp:TemplateField HeaderText="0" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_0" runat="server" Text="0" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="0" ToolTip="Pulse el número para seleccionar/deseleccionar todos los dias."></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_0" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="1" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_1" runat="server" Text="1" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="1"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_1" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="2" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_2" runat="server" Text="2" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="2"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_2" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="3" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_3" runat="server" Text="3" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="3"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_3" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="4" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_4" runat="server" Text="4" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="4"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_4" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="5" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_5" runat="server" Text="5" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="5"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_5" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="6" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_6" runat="server" Text="6" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="6"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_6" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="7" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_7" runat="server" Text="7" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="7"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_7" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="8" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_8" runat="server" Text="8" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="8"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_8" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="9" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_9" runat="server" Text="9" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="9"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_9" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="10" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_10" runat="server" Text="10" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="10"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_10" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="11" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_11" runat="server" Text="11" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="11"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_11" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="12" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_12" runat="server" Text="12" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="12"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_12" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="13" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_13" runat="server" Text="13" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="13"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_13" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="14" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_14" runat="server" Text="14" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="14"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_14" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="15" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_15" runat="server" Text="15" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="15"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_15" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="16" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_16" runat="server" Text="16" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="16"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_16" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="17" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_17" runat="server" Text="17" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="17"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_17" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="18" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_18" runat="server" Text="18" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="18"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_18" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="19" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_19" runat="server" Text="19" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="19"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_19" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="20" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_20" runat="server" Text="20" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="20"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_20" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="21" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_21" runat="server" Text="21" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="21"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_21" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="22" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_22" runat="server" Text="22" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="22"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_22" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="23" HeaderStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnk_23" runat="server" Text="23" ForeColor="White" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                            CommandName="23"></asp:LinkButton>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_23" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <ItemStyle Width="25px" HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:Button ID="btn_Dia" runat="server" CausesValidation="false" Text="Botón" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                    </ItemTemplate>
                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                    <ControlStyle Width="30px" />
                </asp:TemplateField>
            </Columns>

            <RowStyle CssClass="rowHover" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>
        <br />
        <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" Width="90px" CssClass="buttonB" />
    </div>
</asp:Content>
