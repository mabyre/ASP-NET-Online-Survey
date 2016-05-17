<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PageTestCodeAcces.aspx.cs" Inherits="PageTestCodeAcces" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Seed (MemberID ) :" />
    <asp:TextBox ID="TextBoxSeed" runat="server"></asp:TextBox>
    <br />
    <asp:Label ID="Label2" runat="server" Text="Nb + 1 : "></asp:Label>
    <asp:TextBox ID="TextBoxNb" runat="server"></asp:TextBox>
    <br />
    <asp:Button ID="ButtonCalculer" runat="server" Text="Calculer" OnClick="ButtonCalculer_Click" />
    <br />
    <asp:Label ID="LabelResultat" runat="server" />
    
</asp:Content>

