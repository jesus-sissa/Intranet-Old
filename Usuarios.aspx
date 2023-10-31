<%@ Page Title="Registro de Usuarios" Language="vb" MasterPageFile="~/MP_Principal.master" AutoEventWireup="false"
    CodeBehind="Usuarios.aspx.vb" Inherits="IntranetSIAC.Usuarios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="ContentUsuarios" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="Div_Principal">

        <div>
            <table class="tablaSISSA1">
                <tr>
                    <td>
                        <asp:Image ID="img_ReiniciarPass" runat="server" ImageUrl="~/Imagenes/ReiniciarClave16x16.png" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_ReiniciarPass" runat="server" Text="Reiniciar Contraseña"></asp:Label>
                    </td>
                    <td style="width: 30px"></td>
                    <td>
                        <asp:Image ID="img_UserBloqDesbl" runat="server" ImageUrl="~/Imagenes/unlock.png" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_UserBloqDesbl" runat="server" Text="Bloquear/Desbloquear Usuario"></asp:Label>
                    </td>
                    <td style="width: 30px"></td>
                    <td style="width: 30px">
                        <asp:Image ID="img_ModoClave" runat="server" ImageUrl="~/Imagenes/claveExpira.png" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_ModoClave" runat="server" Text="Clave Expira/No Expira"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>

        <div id="div_Usuarios" style="width: 100%">

            <asp:UpdatePanel ID="udp_Usuario" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gv_Usuarios" runat="server"
                        CssClass="gv_general" AutoGenerateColumns="False"
                        DataKeyNames="Id_Empleado" AllowPaging="True"
                        PageSize="35" OnRowCommand="gv_Usuarios_RowCommand" Width="100%">

                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />

                            <asp:BoundField DataField="Clave" HeaderText="Clave">
                                <ItemStyle Width="80px" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" ReadOnly="True">
                                <ItemStyle Width="300px" />
                            </asp:BoundField>

                            <asp:BoundField DataField="VenceClave" HeaderText="VenceClave" HeaderStyle-Width="70px" />
                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                            <asp:BoundField DataField="Departamento" HeaderText="Departamento" />
                            <asp:BoundField DataField="Puesto" HeaderText="Puesto" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="ClaveExpira" HeaderText="ClaveExp" />

                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="Ibt_Reiniciar" runat="server" CausesValidation="False" CommandName="Reiniciar" ToolTip="Reiniciar Contraseña"
                                        ImageUrl="~/Imagenes/ReiniciarClave16x16.png" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" />
                                </ItemTemplate>
                                <ItemStyle Width="15px" />
                            </asp:TemplateField>

                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="Ibt_Bloquear" runat="server" CausesValidation="False" CommandName="Bloq_Desbloq"
                                        ImageUrl="~/Imagenes/unlock.png" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" ToolTip="Bloquear/Desbloquear Usuario" />
                                </ItemTemplate>
                                <ItemStyle Width="15px" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="Ibt_ClaveExpira" runat="server" CausesValidation="False" CommandName="ClaveExpira"
                                        ImageUrl="~/Imagenes/claveExpira.png" CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>" ToolTip="Clave Expira/No Expira" />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <RowStyle CssClass="rowHover" />
                        <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                        <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />

                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </div>
</asp:Content>
