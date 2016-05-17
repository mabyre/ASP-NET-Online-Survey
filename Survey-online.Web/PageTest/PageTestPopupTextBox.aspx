<%@ Page Language="C#" Trace="true" MasterPageFile="~/PageTest/MasterPageTest.master" AutoEventWireup="true" CodeFile="PageTestPopupTextBox.aspx.cs" Inherits="PageTest_PageTestPopupTextBox" Title="Page sans titre" %>
<%@ Register TagPrefix="usr" TagName="PopupTextBox" Src="~/UserControl/PopupTextBox.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <usr:PopupTextBox ID="PopupTextBox1" runat="server" LabelCheckBox="Autre précisez :"  /> 

    <usr:PopupTextBox ID="PopupTextBox2" runat="server" LabelCheckBox="Autre précisez 2 :" TextBoxWidth="110px" TextBoxRows="1" /> 

    <usr:PopupTextBox ID="PopupTextBox3" runat="server" LabelCheckBox="Autre précisez 3 :" TextBoxWidth="210px" TextBoxRows="2" /> 

    <usr:PopupTextBox ID="PopupTextBox4" runat="server" LabelCheckBox="Autre précisez 4 :" TextBoxWidth="510px" TextBoxRows="10" /> 
</asp:Content>

