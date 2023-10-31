<%@ Page Title="Avisos de Baja" Language="vb" MasterPageFile="~/MP_Principal.master"
    AutoEventWireup="false" CodeBehind="Avisos_Baja.aspx.vb" Inherits="IntranetSIAC.Avisos_Baja" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="ContentPlantilla" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="div_Empleados">
        <asp:GridView ID="gv_Empleados" runat="server" AutoGenerateColumns="False" CssClass="gv_general"
            DataKeyNames="Id_Empleado" Width="100%" AllowPaging="True" PageSize="25">

            <Columns>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select"
                            ImageUrl="~/Imagenes/AvisoBaja.png" Text="Seleccionar" />
                    </ItemTemplate>
                    <ItemStyle Width="15px" />
                </asp:TemplateField>
                <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                <asp:BoundField DataField="Departamento" HeaderText="Departamento" SortExpression="Departamento" />
                <asp:BoundField DataField="Puesto" HeaderText="Puesto" SortExpression="Puesto" />
            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />


        </asp:GridView>
    </div>
    <br />

    <table runat="server" id="tablaComentarios" visible="false" style="Width: 520px;">
        <tr>
            <td style="height: 18px; text-align: right; vertical-align: bottom">
                <asp:Label ID="lbl_Fecha_Inicio" runat="server" Text="Fecha Baja" Font-Size="X-Small"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbx_FechaBaja" runat="server" CssClass="calendarioAjax"
                    AutoPostBack="true" Style="vertical-align: bottom" MaxLength="1"></asp:TextBox>
                <ajax:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="tbx_FechaBaja"
                    FilterType="Custom, Numbers" ValidChars="/">
                </ajax:FilteredTextBoxExtender>

                <ajax:CalendarExtender runat="server" ID="CalendarExtender1" TargetControlID="tbx_FechaBaja"
                    CssClass="calendarioAjax" Format="dd/MM/yyyy"
                    TodaysDateFormat="d MMMM yyyy" DaysModeTitleFormat="MMMM  yyyy">
                </ajax:CalendarExtender>
                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>

        </tr>
        <tr>
            <td style="text-align: right; vertical-align: top">
                <asp:Label ID="lbl_Comentarios" runat="server" Text="Comentarios" Font-Size="X-Small"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="tbx_Comentarios" runat="server" Width="390px" Height="85px" TextMode="MultiLine"
                    CssClass="textbox1" Style="text-transform: uppercase" MaxLength="300"></asp:TextBox>

            </td>

        </tr>
        <tr>
            <td style="text-align: right;">
                <asp:Label ID="Label6" runat="server" Text="Contraseña" Font-Size="X-Small"></asp:Label>
            </td>
            <td style="text-align: left">
                <asp:TextBox ID="tbx_Contrasena" runat="server" Style="margin-left: 0px" TextMode="Password" MaxLength="30"></asp:TextBox>
                <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 33px"></td>
            <td colspan="4" style="height: 33px">
                <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" CssClass="button" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     <asp:Button ID="btn_Cancelar" runat="server" Text="Cancelar" CssClass="button" />
            </td>

        </tr>

        <tr>
            <td></td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Nota: Al firmar electronicamente a traves de INTRANET SIAC, según las políticas de la Empresa, dicha firma cuenta con la misma validez, que una firma manuscrita." Font-Size="X-Small"></asp:Label>
            </td>
        </tr>
    </table>

</asp:Content>
