<%@ Page Language="VB" AutoEventWireup="false" Inherits="IntranetSIAC.LoginContra"
    CodeBehind="LoginContra.aspx.vb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="App_Themes/Dorado/Dorado.css"/>
</head>
<body>
    <form id="form1" runat="server">
        <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajax:ToolkitScriptManager>

        <div id="Login" class="card card-container">
                  <div>
                      Su Contraseña está expirada. Para continuar capture una Nueva.
                      </div><br />
            <input type="password" class="form-control" runat="server" id="password" title="Nueva Contraseña" name="contrasena" maxlength="14" placeholder="Nueva Contraseña" />
            <input type="password" class="form-control" runat="server" id="confirmarcontrasena" title="Confirmar Contraseña" name="confirmarpassword" maxlength="14" placeholder="Confirmar Contraseña" />
            <asp:Button runat="server" ID="btn_Aceptar" class="btn_aceptar" Text="Entrar" />
            <br />
            <br />
            <asp:Button runat="server" ID="btn_Cancelar" class="btn_aceptar" Text="Cancelar" />
            <br />
            <br />

            <div>
               <asp:Label CssClass="lbl_mensaje" runat="server" ID="lbl_Mensaje" Text="La Contraseña debe cumplir con todas las Reglas siguientes:"></asp:Label>

                <ul class="ul_efect2" style="list-style-image:url('Imagenes/li.jpg')">
                    <li>Las contraseñas solo deben contener Letras, Numeros y los siguientes caracteres adicionales: '.', ',', '-','(',')','@'"</li>
                    <li>Deben ser por lo menos 8 caracteres</li>
                    <li>Deben ser por lo menos 4 Números</li>
                    <li>Deben ser por lo menos 4 Letras</li>
                    <li>Por los menos 1 Mayúscula</li>
                    <li>Po lo menos 1 Minuscula</li>
                </ul>
            </div>

        </div>

   </form>
</body>
</html>
