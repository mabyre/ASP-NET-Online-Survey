<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DepouillementStat.aspx.cs" Inherits="Help_DepouillementStat" Title="Questionnaire en ligne - Aide au Dépouillement des statistiques" %>
<%@ Register TagPrefix="uc" TagName="WebContent" Src="~/UserControl/WebContent.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="DivWebContentPageAideStyle">
        <uc:WebContent ID="WebContent1" runat="server" Section="HelpDepouillementStat" /> 
    </div>
</asp:Content>

