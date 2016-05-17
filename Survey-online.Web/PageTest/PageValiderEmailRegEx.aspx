<%@ Page Language="C#" MasterPageFile="~/PageTest/MasterPageTest.master" AutoEventWireup="true" CodeFile="PageValiderEmailRegEx.aspx.cs" Inherits="PageTest_PageValiderEmailRegEx" Title="Page sans titre" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:TextBox ID="TextBoxContactAjouter" runat="server" Height="125px" TextMode="MultiLine" 
        Width="266px"></asp:TextBox>
    <asp:Button ID="ButtonOk" runat="server" onclick="ButtonOk_Click" 
        Text="Button" />
    <br />
    <asp:Label ID="LabelValidation" runat="server"></asp:Label>
    <br />
</asp:Content>

