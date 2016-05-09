<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Aide.aspx.cs" Inherits="Aide" Title="Questionnaire en ligne - Page d'Aide" %>
<%@ Register TagPrefix="uc" TagName="WebContent" Src="~/UserControl/WebContent.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="DivWebContentPageAideStyle">
        <uc:WebContent ID="WebContent1" runat="server" Section="PageAide" /> 
    </div>
</asp:Content>

