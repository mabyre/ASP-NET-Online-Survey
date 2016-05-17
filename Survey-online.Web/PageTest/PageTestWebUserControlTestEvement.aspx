<%@ Page Language="C#" MasterPageFile="~/PageTest/MasterPageTest.master" AutoEventWireup="true" CodeFile="PageTestWebUserControlTestEvement.aspx.cs" Inherits="PageTest_PageTestWebUserControlTestEvement" Title="Page sans titre" %>
<%@ Register TagPrefix="usr" TagName="WebUserControlTestEvenements" Src="~/PageTest/WebUserControlTestEvenements.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <usr:WebUserControlTestEvenements ID="PopupTextBox1" runat="server" /> 

</asp:Content>

