<%@ Page Language="C#" MasterPageFile="~/PageTest/MasterPageTest.master" Trace="false" AutoEventWireup="true" CodeFile="RadioButtonList.aspx.cs" Inherits="PageTest_RadioButtonList" Title="Page sans titre" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<br />
<asp:CheckBox ID="CheckBoxTextAligne" runat="server" Text=""TextAligne"></asp:CheckBox>
<br />
    <input type="radio" size="24px" style="background:#00FF00; border:solid 3px #FF0000" />BBB
    <br />
    <font style="font-family:Arial; font-size: 18px; font-weight:bold" color="green">du texte avec un style</font>
    <br />
    <br />
    <br />
    <table style="width:100%;">
        <tr style="font-family:Arial Black">
            <td width="140">
                Repeat Colums :</td>
            <td>
                <asp:TextBox ID="TextBoxRepeatColums" runat="server" 
                    ontextchanged="TextBoxRepeatColums_TextChanged"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Repeat Direction :</td>
            <td>
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="DropDownList1_SelectedIndexChanged">
                    <asp:ListItem>Horizontal</asp:ListItem>
                    <asp:ListItem>Vertical</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Font :</td>
            <td>
                <asp:DropDownList ID="DropDownListFont" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="DropDownListFont_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Layout :</td>
            <td>
                <asp:DropDownList ID="DropDownListLayout" runat="server" 
                    onselectedindexchanged="DropDownListLayout_SelectedIndexChanged" 
                    AutoPostBack="True">
                    <asp:ListItem>Table</asp:ListItem>
                    <asp:ListItem>Flow</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Underline :</td>
            <td>
                <asp:CheckBox ID="CheckBoxUnderline" runat="server" AutoPostBack="True" 
                    oncheckedchanged="CheckBoxUnderline_CheckedChanged" />
            </td>
        </tr>
    </table>

<div id="DivRadioButtonList" runat="server">
<asp:RadioButtonList ID="RadioButtonList1" runat="server" Font-Names="Garamond"  >
    <asp:ListItem>Article 1</asp:ListItem>
    <asp:ListItem>Article 2</asp:ListItem>
    <asp:ListItem>Article 3</asp:ListItem>
    <asp:ListItem>Article 4</asp:ListItem>
    <asp:ListItem>Article 5</asp:ListItem>
    <asp:ListItem>Article 6</asp:ListItem>
</asp:RadioButtonList>
</div>

<br />
<br />

<UserControl:RadioButtonListStyle ID="RadioButtonListStyle1" runat="server" ForeColor="AliceBlue" Font-Bold="true" Font-Size="24px">
    <asp:ListItem>Article 1</asp:ListItem>
    <asp:ListItem>Article 2</asp:ListItem>
    <asp:ListItem>Article 3</asp:ListItem>
    <asp:ListItem>Article 4</asp:ListItem>
    <asp:ListItem>Article 5</asp:ListItem>
    <asp:ListItem>Article 6</asp:ListItem>
</UserControl:RadioButtonListStyle>

<br />
<br />
<table style="width: 10%; font-family:Cursive">
    <tr>
        <td>
            texte pour changer la font 11
        </td>
        <td>
            A12
        </td>
        <td>
            A13
        </td>
    </tr>
    <tr>
        <td>
            B1
        </td>
        <td>
            b21
        </td>
        <td>
            b3
        </td>
    </tr>
</table>

<asp:Label ID="Label1" runat="server" Text="du texte pour changer la font" />

</asp:Content>
