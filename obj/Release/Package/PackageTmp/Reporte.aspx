<%@ Page Title="Reportes" Language="vb" AutoEventWireup="false" CodeBehind="Reporte.aspx.vb" Inherits="IntranetSIAC.Reporte" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Estilos/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" language="javascript">
        function Imprimir() {
            window.print();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <table style="font-family: Arial" width="100%">
                <tr>
                    <td style="width: 20%" rowspan="4" align="center">
                        <asp:Image runat="server" ID="img_Logo" ImageUrl="~/Imagenes/LogoSIAC.jpg" Height="37px"
                            Width="95px" />
                    </td>
                    <td align="left" style="width: 80%">
                        <asp:Label runat="server" ID="lbl_NombreEmpresa" Text="EMPRESA"
                            Style="font-weight: bold; font-size:large;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label runat="server" Text="SUCURSAL" ID="lbl_Sucursal" Style="font-weight: bold;
                            font-size: large"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label runat="server" Text="REPORTE DE DESPACHO DE REMISIONES" ID="lbl_Titulo"
                            Style="font-weight: bold; font-size:large"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
            <table style="font-family: Arial" width="100%">
                <tr>
                    <td style="width: 20%">
                    </td>
                    <td align="left" style="width: 80%">
                        <asp:Label runat="server" Text="RUTA    :" ID="lbl_Ruta"></asp:Label>
                        <asp:Label runat="server" Text="Ruta Prueba" ID="lbl_RutaD"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                    </td>
                    <td align="left" style="width: 80%">
                        <asp:Label runat="server" Text="UNIDAD  :" ID="lbl_Unidad"></asp:Label>
                        <asp:Label runat="server" Text="Unidad Prueba" ID="lbl_UnidadD"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <asp:GridView runat="server" ID="gv_Remisiones" 
            CssClass="gv_general" Width="100%"
             AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="Remision" HeaderText="Remision">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="Caja" HeaderText="Caja">
                    <ItemStyle Width="35%" />
                </asp:BoundField>
                <asp:BoundField DataField="Cliente" HeaderText="Cliente">
                    <ItemStyle Width="35%" />
                </asp:BoundField>
                <asp:BoundField DataField="Importe" HeaderText="Importe">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle Width="10%" HorizontalAlign="right" />
                </asp:BoundField>
                <asp:BoundField DataField="Envases" HeaderText="Envases">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                </asp:BoundField>
            </Columns>

  <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

        </asp:GridView>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <%--<fieldset style="width:90%">--%>
        <table style="font-family: Arial; font-size:x-small;" width="100%">
            <tr>
                <td align="center" style="width: 40%; border-bottom: 1px Solid black">
                    <asp:Label runat="server" Text="" ID="lb_DespachaD"></asp:Label>
                </td>
                <td style="width:20%">
                </td>
                <td align="center" style="width: 40%; border-bottom: 1px Solid black">
                    <asp:Label runat="server" Text="" ID="lbl_RecibeD"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 40%" align="center">
                    <asp:Label runat="server" Text="DESPACHO" ID="lbl_Despacha"></asp:Label>
                </td>
                <td>
                </td>
                <td style="width: 40%" align="center">
                    <asp:Label runat="server" Text="RECIBIO" ID="lbl_Recibe"></asp:Label>
                </td>
            </tr>
        </table>
        <%--</fieldset>--%>
        <br />
        <br />
        <br />
        <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" Width="90px" CssClass="buttonB" />
    </div>
    </form>
</body>
</html>
