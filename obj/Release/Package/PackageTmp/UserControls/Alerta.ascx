<%@ Control Language="VB" AutoEventWireup="false" Inherits="IntranetSIAC.Alerta"
   codeBehind="Alerta.ascx.vb" Classname="Alerta" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"
     TagPrefix="ajax" %>

<style type="text/css">
    .ModalBackground
    {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
        -moz-opacity: 0.7;
    }
        .LogoSIACalerta
    {
        font-family: Stencil;
        font-size: 20px;
        color: #000000;
    }

    .tituloAlerta
    {
        color: #FFFFFF;
        font-weight: bold;
        font-size: large;
    }

</style>

<asp:HiddenField ID="HiddenField1" runat="server" />
<asp:Panel ID="pnl_Alerta" runat="server" Width="350px" BackColor="White" >
        <asp:UpdatePanel runat="server" ID="udp_Alerta" >
        <ContentTemplate>
            <div id="div_contenedorAlerta" runat="server" style="width: 350px; height: 100%;">
                <div id="div_encabezado" style="float: left; width: 93%; height: 27px;">
                    <div id="div_tituloAlerta" runat="server" style="width: 100%; height: 22px; text-align: center; border: 0px; padding-top: 4px;">
                        <asp:Label ID="lblTituloAlerta" runat="server" CssClass="tituloAlerta" Text="Intranet SIAC"></asp:Label>
                    </div>
                </div>

                <div id="div_btnCerrar" runat="server" style="float: right; text-align: center;">
                    <asp:ImageButton ID="btn_Cerrar" runat="server" Text="Cerrar" ImageUrl="~/Imagenes/Cerrar.png" />
                </div>

                <div id="div_mensaje" runat="server" style="word-wrap: break-word; vertical-align: central; margin-top: 10px; width: 99%; height: 100px; float: left; text-align: center;">
                    <br />
                    <asp:Label ID="lbl_Mensaje" runat="server" ForeColor="Black" Font-Bold="True" Font-Size="10pt" />
                </div>
            </div>
      
       </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<ajax:ModalPopupExtender ID="pnl_Alerta_ModalPopupExtender" runat="server" BackgroundCssClass="ModalBackground"
    TargetControlID="HiddenField1" PopupControlID="pnl_Alerta">
</ajax:ModalPopupExtender>
