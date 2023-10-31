<%@ Page Title="Foro RIA" Language="VB" MasterPageFile="~/MP_Principal.master" AutoEventWireup="false" Inherits="IntranetSIAC.Foro_RIA" Codebehind="Foro_RIA.aspx.vb" %>

<%@ Register Src="~/UserControls/WUC_Foro_RIA.ascx" TagName="ForoRIA" TagPrefix="uc_ForoRIA" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<uc_ForoRIA:ForoRIA runat="server" ID="uc_ForoRIA" />
</asp:Content>

