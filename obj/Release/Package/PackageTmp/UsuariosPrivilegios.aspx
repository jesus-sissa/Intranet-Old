<%@ Page Title="PRIVILEGIOS DE USUARIOS" Language="vb" MasterPageFile="~/MP_Principal.master" AutoEventWireup="false"
    CodeBehind="UsuariosPrivilegios.aspx.vb" Inherits="IntranetSIAC.UsuariosPrivilegios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="ContentUPrivilegios" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">

    <%--    <div class="Div_Principal">--%>
    <br />
    <table class="555">
        <tr>
            <td style="width: 50px" align="right">
                <asp:Label ID="lbl_Usuario" runat="server" Text="Usuario"></asp:Label>
            </td>
            <td class="celdaMargenDer10">
                <%--  <asp:UpdatePanel ID="udp_Usuario" runat="server">
                        <ContentTemplate>--%>
                <asp:DropDownList runat="server" ID="ddl_Usuarios" Width="300"
                    AutoPostBack="true">
                </asp:DropDownList>
                <%--     </ContentTemplate>
                    </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr>
            <td style="width: 50px" align="right">
                <asp:Label ID="lbl_Modulo" runat="server" Text="Módulo"></asp:Label>
            </td>
            <td class="celdaMargenDer10">
                <%--  <asp:UpdatePanel ID="udp_Modulo" runat="server">
                        <ContentTemplate>--%>
                <asp:DropDownList runat="server" ID="ddl_Modulos" CssClass="DropDownList18" Width="300"
                    AutoPostBack="true">
                </asp:DropDownList>
                <%--     </ContentTemplate>
                    </asp:UpdatePanel>--%>
            </td>
        </tr>
    </table>
    <br />
    <div style="width: 50%">
        <table style="width: 100%">
            <tr>
                <td style="text-align: center; font-size: small; font-family: Verdana; font-weight: bold;">
                    <asp:Label runat="server" ID="lbl_Menus" Text="Menus"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%--<asp:UpdatePanel ID="udp_Menu" runat="server">
                            <ContentTemplate>--%>
                    <asp:GridView ID="gv_Menus" runat="server"
                        CssClass="gv_general"
                        AutoGenerateColumns="False"
                        DataKeyNames="Id_Menu" Width="100%">

                        <Columns>
                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/1rightarrow.png"
                                ShowSelectButton="True">
                                <ItemStyle Width="15px" />
                            </asp:CommandField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción">
                                <ItemStyle Width="300px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status" Visible="False" />
                            <asp:BoundField DataField="Orden" HeaderText="Orden" Visible="False" />
                        </Columns>

                        <RowStyle CssClass="rowHover" />
                        <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                        <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                    </asp:GridView>
                    <%--  </ContentTemplate>
                        </asp:UpdatePanel>--%>
                </td>
            </tr>
        </table>

    </div>
    <br />

    <div style="float: left; width: 50%">

        <table style="width: 100%">
            <tr>
                <td style="text-align: center; font-size: small; font-family: Verdana; font-weight: bold; height: 20px;">
                    <asp:Label runat="server" ID="lbl_Opciones" Text="Opciones">
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%--  <asp:UpdatePanel ID="udp_Opciones" runat="server">
                            <ContentTemplate>--%>
                    <asp:GridView ID="gv_Opciones" runat="server"
                        CssClass="gv_general" Width="100%" AutoGenerateColumns="False" DataKeyNames="Id_Opcion">

                        <Columns>
                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/1rightarrow.png"
                                ShowSelectButton="True">
                                <ItemStyle Width="15px" />
                            </asp:CommandField>

                            <asp:TemplateField HeaderStyle-Width="15px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSeleccionaTodo" runat="server" AutoPostBack="true"
                                        OnCheckedChanged="chkSeleccionaTodo_CheckedChanged" Width="15px" />
                                </HeaderTemplate>
                                <ItemStyle VerticalAlign="Middle" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkOpc" runat="server" Width="15px"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción">
                                <ItemStyle Width="300px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status" Visible="False" />
                        </Columns>

                        <RowStyle CssClass="rowHover" />
                        <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                        <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                    </asp:GridView>

                    <%--     </ContentTemplate>
                        </asp:UpdatePanel>--%>
                </td>
            </tr>
            <tr>
                <%-- <asp:UpdatePanel ID="udp_btnOpciones" runat="server">
                        <ContentTemplate>--%>
                <td align="right">
                    <asp:Button ID="btn_GuardarOpciones" runat="server" Text="Guardar" CssClass="buttonB" />
                </td>
                <%--      </ContentTemplate>
                    </asp:UpdatePanel>--%>
            </tr>
        </table>
    </div>

    <div style="float: left; width: 48%; margin-left: 10px">
        <table style="width: 100%">
            <tr>
                <td style="text-align: center; font-size: small; font-family: Verdana; font-weight: bold;">
                    <asp:Label runat="server" ID="lbl_Controles" Text="Controles"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <%--  <asp:UpdatePanel ID="udp_Controles" runat="server">
                            <ContentTemplate>--%>
                    <asp:GridView ID="gv_Controles" runat="server"
                        CssClass="gv_general"
                        AutoGenerateColumns="False"
                        DataKeyNames="Id_Control" Width="100%">

                        <Columns>

                            <asp:TemplateField HeaderStyle-Width="15px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSeleccionaTodoControl" runat="server" AutoPostBack="true"
                                        OnCheckedChanged="chkSeleccionaTodoControl_CheckedChanged" Width="15px" />
                                </HeaderTemplate>
                                <ItemStyle VerticalAlign="Middle" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkControl" runat="server" Width="15px"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción">
                                <ItemStyle Width="300px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status" HeaderText="Status" Visible="False" />
                        </Columns>

                        <RowStyle CssClass="rowHover" />
                        <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                        <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                    </asp:GridView>
                    <%--     </ContentTemplate>
                        </asp:UpdatePanel>--%>
                </td>
            </tr>
            <tr>
                <%--<asp:UpdatePanel ID="udp_btnControles" runat="server">
                        <ContentTemplate>--%>
                <td align="right">
                    <asp:Button ID="btn_GuardarControles" runat="server" Text="Guardar" CssClass="buttonB" />
                </td>
                <%--    </ContentTemplate>
                    </asp:UpdatePanel>--%>
            </tr>
        </table>

    </div>
    <%-- </div>--%>
</asp:Content>
