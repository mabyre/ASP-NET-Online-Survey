<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AideAdmin.aspx.cs" Inherits="AideAdmin" Title="Untitled Page" %>
<%@ Register TagPrefix="uc" TagName="WebContent" Src="~/UserControl/WebContent.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="DivWebContentPageStyle">
        <uc:WebContent ID="WebContent1" runat="server" Section="PageAideAdmin" /> 
    </div>
</asp:Content>

