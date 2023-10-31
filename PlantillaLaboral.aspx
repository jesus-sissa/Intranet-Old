<%@ Page Title="Plantilla Laboral" Language="VB" MasterPageFile="~/MP_Principal.master"
    AutoEventWireup="false" Inherits="IntranetSIAC.PlantillaLaboral" CodeBehind="PlantillaLaboral.aspx.vb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="ContentPlantilla" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:GridView ID="gv_Plantilla" runat="server"
        CssClass="gv_general" AutoGenerateColumns="False"
        DataKeyNames="Id_Plantilla,Did,Pid,Requerida">
        <Columns>
            <asp:TemplateField HeaderText="" ItemStyle-Wrap="false"
                ItemStyle-Width="20px">
                <ItemTemplate>
                    <asp:ImageButton ID="EditarPlantilla" CommandName="Select" runat="server"
                        ImageUrl="~/Imagenes/edit.gif" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Puesto" HeaderText="Puesto" ReadOnly="True">
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle Width="300px" />
            </asp:BoundField>

            <asp:BoundField DataField="Requerida" HeaderText="Plantilla Requerida" ReadOnly="True">
                <HeaderStyle HorizontalAlign="Right" Width="80px" />
                <ItemStyle HorizontalAlign="Right" />
            </asp:BoundField>

            <asp:BoundField DataField="Actual" HeaderText="Plantilla Actual" ReadOnly="True">
                <HeaderStyle HorizontalAlign="Right" />
                <ItemStyle HorizontalAlign="Right" Width="80px" />
            </asp:BoundField>

            <asp:BoundField DataField="Did" HeaderText="DeptoId" Visible="False" />
            <asp:BoundField DataField="Pid" HeaderText="PuestoId" Visible="False" />
        </Columns>

        <RowStyle CssClass="rowHover" />
        <SelectedRowStyle CssClass="FilaSeleccionada_gv" />

        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
        <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />

    </asp:GridView>
    <br />
    <table runat="server" id="tbl_Comentarios" visible="false">
        <tr>
            <td align="right">
                <asp:Label ID="lbl_Cantidad" runat="server" Text="Cantidad" Font-Names="Arial" Font-Size="X-Small"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbx_Cantidad" runat="server" MaxLength="2" Width="40px"></asp:TextBox>
                <ajax:FilteredTextBoxExtender ID="tbx_ClaveUsuario_fte" runat="server"
                    Enabled="True" FilterType="Numbers" TargetControlID="tbx_Cantidad">
                </ajax:FilteredTextBoxExtender>
                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top" align="right">
                <asp:Label ID="lbl_Comentarios" runat="server" Font-Names="Arial" Font-Size="X-Small" Text="Comentarios"></asp:Label>
            </td>

            <td>
                <asp:TextBox ID="tbx_Comentarios" runat="server" Width="410px" Height="100px" TextMode="MultiLine"
                    CausesValidation="true" MaxLength="150" Style="text-transform: uppercase" Font-Names="Verdana,Arial" CssClass="textbox1"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" Height="26px" Width="90px" CssClass="button" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_Cancelar" runat="server" Text="Cancelar" Height="26px" Width="90px" CssClass="button" />
            </td>
        </tr>
    </table>

</asp:Content>
