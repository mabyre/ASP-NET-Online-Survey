<%@ Page Language="C#" Trace="true" MasterPageFile="~/PageTest/MasterPageTest.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="PageCheckListBoxExclusive.aspx.cs" Inherits="PageTest1" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table border="1" width="100%">
<tr>
    <td width="200">
        <asp:CheckBoxList ID="CheckBoxList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged">
        <asp:ListItem Selected="True">choix 1</asp:ListItem>
        <asp:ListItem>choix 2</asp:ListItem>
        <asp:ListItem>choix 3</asp:ListItem>
        <asp:ListItem>choix 4</asp:ListItem>
        </asp:CheckBoxList>
    </td>
    <td width="200">
        <usr:CheckBoxListExclusive ID="CheckBoxListExclusive1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CheckBoxListExclusive1_SelectedIndexChanged" >
        </usr:CheckBoxListExclusive>
    </td>
    
    <td>
    <asp:Label ID="LabelText" runat="server" Text="Label"></asp:Label>
    </td>
    <td>
    <asp:Label ID="LabelTextExclusif" runat="server" Text="Label"></asp:Label>
    </td>
    
</tr>
</table>
<table border="0" width="100%">
<tr>
    <td>
    <asp:Button ID="ButtonPostBack" runat="server" OnClick="ButtonPostBack_Click" Text="PostBack" />
    </td>
    <td>
    <asp:Button ID="ButtonChoixCheckBoxList1" runat="server" OnClick="ButtonChoixCheckBoxList1_Click" Text="Choix 1 ?" />
    </td>
    <td>
    <asp:Button ID="ButtonCheckBoxListExclusive1" runat="server" OnClick="ButtonCheckBoxListExclusive1_Click" Text="Choix exclusif ?" />
    </td>
</tr>
</table>

</asp:Content>

