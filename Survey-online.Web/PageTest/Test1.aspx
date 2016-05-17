<%@ Page Language="C#" MasterPageFile="~/PageTest/MasterPageTest.master" AutoEventWireup="true" CodeFile="Test1.aspx.cs" Inherits="PageTest_Test1" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:BulletedList ID="BulletedList1" runat="server" OnClick="BulletedList1_Click" OnTextChanged="BulletedList1_TextChanged" >
        <asp:ListItem Value="132" Selected="True"></asp:ListItem>
        <asp:ListItem Value="456">Hohoho</asp:ListItem>
        <asp:ListItem Value="465">ssss</asp:ListItem>
    </asp:BulletedList>
    <br />
    <asp:RadioButtonList ID="RadioButtonList1" runat="server" OnTextChanged="RadioButtonList1_TextChanged">
        <asp:ListItem>test1</asp:ListItem>
        <asp:ListItem>test2</asp:ListItem>
        <asp:ListItem>test3</asp:ListItem>
    </asp:RadioButtonList>
    <asp:Label ID="Label1" runat="server" Text="Ma texte box :"></asp:Label>
    <asp:TextBox ID="TextBoxMaTextBox" runat="server"></asp:TextBox><br />
</asp:Content>

