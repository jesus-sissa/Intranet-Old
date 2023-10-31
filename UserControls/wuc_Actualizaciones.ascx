<%@ Control Language="VB" AutoEventWireup="false" Inherits="SISSAIntranet.wuc_Actualizaciones"
    CodeBehind="wuc_Actualizaciones.ascx.vb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>ACTUALIZACIONES</title>
    <link href="../Estilos/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="Div_Principal">
        <%--<br />
        <div id="divEnca" title="EncabezadoActualizacion">
            <strong>Listado de Actualizaciones</strong>
        </div>
        <br />--%>
        <asp:GridView ID="gv_Actualizaciones" runat="server" AutoGenerateColumns="False"
            DataKeyNames="Id_Actualizacion,Status,Descripcion" CssClass="gridSISSA" AllowPaging="True"
            Width="100%" Style="color: Black; font-family: Verdana; font-size: x-small" PageSize="15">
            <Columns>
                <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/1rightarrow.png"
                    ShowSelectButton="True">
                    <ItemStyle Width="15px" />
                </asp:CommandField>
                <asp:BoundField DataField="Id_Actualizacion" HeaderText="ACT_ID" Visible="False">
                    <ItemStyle HorizontalAlign="Right" Wrap="True" />
                </asp:BoundField>
                <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                    <ItemStyle Width="50px" />
                </asp:BoundField>
                <asp:BoundField DataField="Modulo" HeaderText="Módulo">
                    <ItemStyle HorizontalAlign="Left" Wrap="True" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Menu" HeaderText="Menú">
                    <ItemStyle HorizontalAlign="Left" Wrap="True" Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Opcion" HeaderText="Opción">
                    <ItemStyle HorizontalAlign="Left" Wrap="True" Width="300px" />
                </asp:BoundField>
                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" Visible="false">
                    <ItemStyle HorizontalAlign="Left" Wrap="True" Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="Status" HeaderText="Status" Visible="false">
                    <ItemStyle HorizontalAlign="Left" Wrap="True" />
                </asp:BoundField>
            </Columns>
            <SelectedRowStyle BackColor="#C0A062" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="Black" ForeColor="White" HorizontalAlign="Left" />
            <PagerStyle BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
        </asp:GridView>
        <br />
        <asp:Label runat="server" Text="Descripción" CssClass="textbox1" ID="lbl_Descripcion"></asp:Label>
        <asp:TextBox runat="server" ID="tbx_Descripcion" Width="100%" Height="150px" CssClass="textbox1"
            TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
    </div>
</body>
</html>
