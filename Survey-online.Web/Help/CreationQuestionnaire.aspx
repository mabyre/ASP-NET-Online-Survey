<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreationQuestionnaire.aspx.cs" Inherits="Help_CreationQuestionnaire" Title="Questionnaire en ligne - Aide à la création d'un Questionnaire" %>
<%@ Register TagPrefix="uc" TagName="WebContent" Src="~/UserControl/WebContent.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="DivWebContentPageAideStyle">
        <uc:WebContent ID="WebContent1" runat="server" Section="HelpCreationQuestionnaire" /> 
    </div>
</asp:Content>

