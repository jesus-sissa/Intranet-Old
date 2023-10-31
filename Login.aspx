<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="IntranetSIAC.Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Login IntranetSIAC</title>
       <link rel="icon" href="Imagenes/fav.png" />
         <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="App_Themes/Dorado/Dorado.css" rel="stylesheet" type="text/css" />
    <script src="https://www.google.com/recaptcha/api.js?hl=es"></script>
</head>

<body>

    <form id="form1" runat="server">
        <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajax:ToolkitScriptManager>

        <div id="Login" class="card card-container">

            <div class="profile-img-card">
                <asp:Label runat="server" ID="titulosiac" Text="Intranet " CssClass="fuenteLogin"></asp:Label>
                <asp:Label runat="server" ID="Label1" Text="SIAC"></asp:Label>
            </div>

            <asp:UpdatePanel ID="udp_Sucursal" runat="server">
                <ContentTemplate>
                    <asp:DropDownList ID="ddl_Sucursal" CssClass="form-control"
                        Height="34px" runat="server"
                        DataTextField="display" DataValueField="value">
                    </asp:DropDownList>
                </ContentTemplate>
            </asp:UpdatePanel>

            <input type="text" class="form-control" runat="server" id="usuarioID" title="ID Usuario" name="usuarioID" maxlength="5" placeholder="ID Usuario" />

            <input type="password" class="form-control" runat="server" id="password" title="Contraseña" name="password" maxlength="14" placeholder="Contraseña" />

            <div style="max-width: 260px; text-align: center; display: inline-block; max-height: 575px; transform: scale(0.86); -webkit-transform: scale(0.86); transform-origin: 0 0; -webkit-transform-origin: 0 0" class="g-recaptcha" data-theme="light" data-sitekey="6LdtDQwUAAAAAJ67OzCXRXm8SKt3MX8tRtMsCRif"></div>

            <asp:Button runat="server" ID="btn_Aceptar" class="btn_aceptar" Text="Entrar" />

            <hr style="border-color: black; height: 2px; background-color: black" />

            <asp:Label ID="lbl_Declaracion" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11pt" Text="Declaración de Privacidad"></asp:Label>
            <br />
            <asp:Label ID="lbl_PrivacidadContenido" runat="server"
                Text="Al entrar a este sitio usted está aceptando que la información mostrada es confidencial. Queda prohibido su uso parcial o total para fines diferentes a los que se establecen en el contrato celebrado entre usted y la Empresa." Font-Names="arial" Font-Size="11pt"></asp:Label>
            <asp:LinkButton ID="LinkButton1" runat="server" Font-Names="Arial" Font-Size="11pt">Aviso de Privacidad</asp:LinkButton>

            <hr style="border-color: black; height: 2px; background-color: black" />
                     
        </div>

        <asp:HiddenField ID="HiddenField1" runat="server" />

        <ajax:ModalPopupExtender ID="mpeEditOrder" runat="server"
            PopupControlID="panelEditOrder" TargetControlID="HiddenField1"
            BackgroundCssClass="backgroundColor">
        </ajax:ModalPopupExtender>

        <asp:Panel ID="panelEditOrder" runat="server" BackColor="White">

            <asp:UpdatePanel ID="upEditOrder" runat="server">
                <ContentTemplate>

                    <div id="Avisoprivacidad" style="background-color: whitesmoke; width: 718px; font-size: 11px; text-align: justify; font-family: Verdana">

                        <asp:PlaceHolder ID="PlaceHolder1" runat="server">

                            <b>AVISO DE PRIVACIDAD </b>
                            <br />
                            <br />

                            <b>Identificación.</b>
                            <br />
                            <br />

                            De conformidad con lo establecido en la Ley Federal de Protección de Datos Personales en Posesión de los Particulares le
informamos que Servicio Integral de Seguridad, S.A. de C.V. , con domicilio en Juan Alvarez 209, Col. Centro, C.P. 64000
Monterrey, Nuevo León (la "Dirección de Contacto") tratará los datos personales que recabe de Usted en los términos del
presente aviso de privacidad. 
                            <br />
                            <br />

                            <b>Finalidad de la obtención de datos:</b>
                            <br />
                            <br />

                            Los datos personales que usted libre y voluntariamente proporcione a través de una solicitud y/o a través de otros medios
distintos, podrán incluir de manera enunciativa más no limitativa su: nombre, domicilio, dirección de correo electrónico (email),
números telefónicos, fecha de nacimiento, nacionalidad; y para los procedimientos de atracción de talento,
podremos incluir también, intereses personales, formación profesional y académica, experiencia laboral, información
sobre sus dependientes económicos, así como referencias personales para fines de consulta por parte de Servicio Integral
de Seguridad S.A. de C.V., e inclusive datos personales sensibles, caso en el cual se obtendrá previamente su
consentimiento expreso y por escrito.
                            <br />
                            <br />

                            Los datos personales que recopilamos los destinamos únicamente a los siguientes propósitos: (a) fines de identificación,
(b) fines estadísticos y de análisis interno, (c) información a clientes, (e) reclutamiento y selección de personal, y/o, (f)para
eventualmente contactarlo vía correo electrónico en relación a los fines antes mencionados. En la recolección y
tratamiento de datos personales que usted nos proporcione, cumplimos todos los principios que marca la Ley (artículo 6):
licitud, calidad, consentimiento, información, finalidad, lealtad, proporcionalidad y responsabilidad. 
                            <br />
                            <br />

                            Compartimos su información con Instituciones en donde la empresa Servicio Integral de Seguridad S.A. de C.V. esta
registrada para cumplir sus funciones como empresa de seguridad y traslado de valores.
El área responsable del manejo y la administración de los datos personales de esta empresa será del departamento de
RECURSOS HUMANOS.
                            <br />
                            <br />

                            Los usuarios titulares de datos personales podrán ejercitar los derechos ARCO (acceso, cancelación, rectificación y
oposición al tratamiento de sus datos personales), enviando directamente su solicitud por escrito al departamento de
RECURSOS HUMANOS. Dicha solicitud deberá contener por lo menos: (a) nombre y domicilio u otro medio para
comunicarle la respuesta a su solicitud; (b) los documentos que acrediten su identidad o, en su caso, la representación
legal; (c) la descripción clara y precisa de los datos personales respecto de los que se solicita ejercer alguno de los derechos
ARCO; y (d) cualquier otro elemento que facilite la localización de los datos personales. 
                            <br />
                            <br />

                            <b>Modificaciones al Aviso de Privacidad.</b> Nos reservamos el derecho de cambiar este Aviso de Privacidad en cualquier
momento. En caso de que exista algún cambio en este Aviso de Privacidad, se le comunicará a los usuarios publicando una
nota visible en nuestro departamento y en lugares accesibles. Por su seguridad, consulte en todo momento que así lo
desee el contenido de este aviso de privacidad en nuestro Portal. 
                            <br />
                            <br />

                            <b>Consentimiento.</b> Doy mi consentimiento para el tratamiento de mis datos personales sensibles y personales financieros o
patrimoniales para las finalidades necesarias para la relación jurídica con el Responsable. (Negarse al tratamiento de sus
datos personales tendrá como consecuencia la imposibilidad de establecer una relación jurídica con la empresa Servicio
Integral de Seguridad S.A. de C.V.). 
                            <br />
                            <br />

                            <b>Álvarez 209 Norte, Centro, Monterrey, N.L. C.P. 64000 Tel. (81) 8047-4545</b>

                        </asp:PlaceHolder>

                    </div>
                    <div id="botoncerrar" style="width: 718px;">
                        <asp:ImageButton ID="imgbtn_Salir" runat="server" CssClass="Botoncerrar" ImageAlign="Top" ImageUrl="~/Imagenes/Cerrar.png" />

                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>

        </asp:Panel>

    </form>
</body>
</html>
