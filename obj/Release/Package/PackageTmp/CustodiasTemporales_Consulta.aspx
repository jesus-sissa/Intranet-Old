<%@ Page Title="CONSULTA DE CUSTODIAS TEMPORALES" Language="vb" AutoEventWireup="false" MasterPageFile="~/MP_Principal.master"
    CodeBehind="CustodiasTemporales_Consulta.aspx.vb" Inherits="IntranetSIAC.CustodiasTemporales_Consulta" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

<script type="text/javascript" language="javascript">

    function MostrarReporte(){
        //        window.open("Reporte.aspx", "Reporte", "fullscreen=no, toolbar=0, location=0,status=0, menubar=0, scrollbars=0, resizable=1, width=1000, height=800, top=0",false)
        window.open("Reporte.aspx", "Reporte", "resizable=1,width=1000, height=800, top=0", false)
        document.title="Despacho de Remisiones";
//        window.print();
    }
    
</script>

    <div class="Div_Principal">
        <br />
        <table class="tablaSISSA2">
            <tr>
                <td align="right" style="width: 100px">
                    <asp:Label ID="lbl_Compania" runat="server" Text="Compañía"></asp:Label>
                </td>
                <td class="celdaMargenDer10" colspan="3">
                    <asp:DropDownList ID="ddl_Cia" runat="server" CssClass="DropDownList18" AutoPostBack="true"
                        Width="320px" DataTextField="Alias" DataValueField="Id_Cia">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <asp:GridView runat="server" ID="gv_Remisiones" CssClass="gv_general" Width="100%"
            DataKeyNames="Id_Bo,Status,Id_Remision,Entidad_Origen,Entidad_Destino" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk_Remision" runat="server" />
                    </ItemTemplate>
                    <ItemStyle Width="15px" />
                </asp:TemplateField>
                <asp:BoundField DataField="Remision" HeaderText="Remision">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Caja" HeaderText="Caja" HtmlEncode="false">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Cliente" HeaderText="Cliente"  HtmlEncode="false">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Importe" HeaderText="Importe">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Envases" HeaderText="Envases">
                    <ItemStyle Width="50px" HorizontalAlign="Right" />
                </asp:BoundField>
            </Columns>

            <RowStyle CssClass="rowHover" />
 <SelectedRowStyle CssClass="FilaSeleccionada_gv" />

  <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
   <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        
        </asp:GridView>
        <br />
        <div>
            <asp:GridView runat="server" ID="gv_Rutas" CssClass="gv_general"
                 Width="100%" DataKeyNames="Id_Punto,IDR,Unidad,Cajero"
                AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select"
                                ImageUrl="~/Imagenes/1rightarrow.png" />
                        </ItemTemplate>
                        <ItemStyle Width="15px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Ruta" HeaderText="Ruta">
                        <ItemStyle Width="80px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Origen" HeaderText="Origen">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Destino" HeaderText="Destino">
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                </Columns>
              <RowStyle CssClass="rowHover" />
 <SelectedRowStyle CssClass="FilaSeleccionada_gv" />

  <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
   <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
            </asp:GridView>
            <br />
            <table width="100%">
                <tr>
                    <td>
                        <asp:Button runat="server" ID="btn_Despachar" Text="Despachar" Width="90px" CssClass="buttonU" />
                    </td>
                    <td align="right">
                        <asp:Button runat="server" ID="btn_CambiarStatus" Text="Finalizar" Width="90px" />
                    </td>
                </tr>
            </table>
        </div>
        
    </div>
</asp:Content>
