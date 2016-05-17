<%@ Page Language="C#" Trace="false" MasterPageFile="~/PageTest/MasterPageTest.master" AutoEventWireup="true" CodeFile="PageStyleWeb.aspx.cs" Inherits="PageTest_PageStyle" Title="Page sans titre" %>
<%@ Register TagPrefix="usr" TagName="ColorPicker" Src="~/UserControl/ColorPicker.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script>
function showWindow() 
{
    window.open("../StyleWeb/Edit.aspx?Style=question" ,null, "status=yes,toolbar=no,resizable=yes,scrollbars=yes,menubar=no,location=no");
}
</script>
<div>

<table cellpadding="6" border="0">
    <tr>
        <td align="right" valign="top">
            <UserControl:LabelStyleWeb ID="LabelStyleWeb" runat="server" StyleWeb="Question" Text="Un label stylé par le XML" />
        </td>
    </tr>
    <tr>
        <td>
        <button onclick="showWindow()">Show Window</button>
        </td>
    </tr>
</table>

<table cellpadding="6" border="0">
    <tr>
        <td align="right" valign="top">
            <asp:Label ID="Label2" runat="server" 
                AssociatedControlID="DropDownListBorderColor" 
                Text="Border Color:">
            </asp:Label>
        </td>
        <td valign="top">
            <asp:dropdownlist id="DropDownListBorderColor" 
                Runat="server" AutoPostBack="True" 
                OnSelectedIndexChanged="ChangeBorderColor">
            </asp:dropdownlist>
            <br />
            <asp:TextBox ID="TextBoxBorderColor" runat="server"></asp:TextBox>
            <br />
            <usr:ColorPicker ID="ColorPickerBorderColor" runat="server" />
        </td>
        <td rowspan="14" style="border:solid 1px Gray" valign="top">
            <p>
                <asp:label id="Label1" 
                    Text="Border Properties Example" Runat="server">
                    Label Styles
                </asp:label>
            </p>
            <p>
                <asp:button id="Button1" runat="server" 
                    Text="Button Styles">
                </asp:button>
            </p>
            <p>
                <asp:listbox id="ListBox1" Runat="server">
                    <asp:ListItem Value="0" Text="List Item 0">
                    </asp:ListItem>
                    <asp:ListItem Value="1" Text="List Item 1">
                    </asp:ListItem>
                    <asp:ListItem Value="2" Text="List Item 2">
                    </asp:ListItem>
                </asp:listbox>
            </p>
            <p>
                <asp:textbox id="TextBox1" 
                    Text="TextBox Styles" Runat="server">
                </asp:textbox>
            </p>
            <p>
                <asp:table id="Table1" Runat="server">
                    <asp:TableRow>
                        <asp:TableCell Text="(0,0)"></asp:TableCell>
                        <asp:TableCell Text="(0,1)"></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Text="(1,0)"></asp:TableCell>
                        <asp:TableCell Text="(1,1)"></asp:TableCell>
                    </asp:TableRow>
                </asp:table>
            </p>
        </td>
        <td rowspan="11" valign="top">
            &nbsp;</td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="Label3" Runat="server" 
                AssociatedControlID="borderStyleList"
                Text="Border Style:">
            </asp:Label>
        </td>
        <td>
            <asp:dropdownlist id="borderStyleList" 
                Runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="ChangeBorderStyle">
            </asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="Label4" Runat="server" 
                AssociatedControlID="borderWidthList"
                Text="Border Width">
            </asp:Label>
        </td>
        <td>
            <asp:dropdownlist id="borderWidthList" 
                Runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="ChangeBorderWidth">
            </asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td align="right" valign="top">
            <asp:Label ID="Label5" Runat="server" 
                AssociatedControlID="DropDownListBackColor"
                Text="Back Color:">
            </asp:Label>
        </td>
        <td>
            <asp:dropdownlist id="DropDownListBackColor" 
                Runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="ChangeBackColor">
            </asp:dropdownlist>
            <br />
            <asp:TextBox ID="TextBoxBackColor" runat="server"></asp:TextBox>
            <br />
            <usr:ColorPicker ID="ColorPickerBackColor" runat="server" />
        </td>
    </tr>
    <tr>
        <td align="right" valign="top">
            <asp:Label ID="Label6" Runat="server" 
                AssociatedControlID="DropDownListForegroundColor"
                Text="Foreground Color:">
            </asp:Label>
        </td>
        <td>
            <asp:dropdownlist id="DropDownListForegroundColor" 
                Runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="ChangeForeColor">
            </asp:dropdownlist>
            <br />
            <asp:TextBox ID="TextBoxForegroundColor" runat="server"></asp:TextBox>
            <br />
            <usr:ColorPicker ID="ColorPickerForegroundColor" runat="server" />
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="Label7" Runat="server" 
                AssociatedControlID="fontNameList"
                Text="Font Name:">
            </asp:Label>
        </td>
        <td>
            <asp:dropdownlist id="fontNameList" 
                Runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="ChangeFont">
            </asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="Label8" Runat="server" 
                AssociatedControlID="fontSizeList"
                Text="Font Size:">
            </asp:Label>
        </td>
        <td>
            <asp:dropdownlist id="fontSizeList" 
                Runat="server" AutoPostBack="True" 
                OnSelectedIndexChanged="ChangeFontSize">
            </asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td align="right" valign="top">
            Font Style :</td>
        <td>
            <asp:CheckBoxList ID="CheckBoxListFontStyle" runat="server" 
                onselectedindexchanged="CheckBoxListFontStyle_SelectedIndexChanged" AutoPostBack="true">
            </asp:CheckBoxList>
        </td>
    </tr>
    <tr>
        <td align="right">
            Font Style :</td>
        <td>
            <asp:dropdownlist id="DropDownListStyle" 
                Runat="server" AutoPostBack="True" 
                OnSelectedIndexChanged="DropDownListStyle_SelectedIndexChanged">
            </asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Button ID="ButtonPaletteCouleurs" runat="server" onclick="ButtonPaletteCouleurs_Click" 
                Text="Palette Couleurs" />
        </td>
        <td>
            <asp:Label ID="LabelColors" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Table id="TableCouleurs" runat="server"/>
        </td>
    </tr>
</table>

</div>
</asp:Content>

