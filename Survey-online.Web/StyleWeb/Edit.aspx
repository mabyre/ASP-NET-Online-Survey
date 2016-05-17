<%@ Page Language="C#" Trace="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="StyleWeb_Edit" Title="Edition d'un Style Web" %>
<%@ Register TagPrefix="usr" TagName="ColorPicker" Src="~/UserControl/ColorPicker.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="DivPageStyle">
<div class="DivStyleWebEdit">
<h3><asp:Label ID="LabelTitre" runat="server"/></h3>
<div style="font-size:12px;margin:6px;font-weight:bold">
<asp:Label ID="LabelApplicable" runat="server" CssClass="LabelListRedStyle" Text="Style non appliqué" />
</div>

<asp:Panel ID="PanelTypeTable" runat="server" Visible="false" Font-Size="Small" GroupingText="Table" Width="220px">
<table cellpadding="6" cellspacing="0" border="0" >
    <tr>
        <td align="right" valign="top">
            <asp:Label ID="Label11" Runat="server" Text="Padding :" ToolTip="" />
        </td>
        <td valign="top">
            <asp:TextBox ID="TextBoxPadding" runat="server" Width="60px" />
            <asp:Button ID="Button1" runat="server" CssClass="ButtonStyle" 
                onclick="ButtonPaddingOk_Click" Text="Ok" />
        </td>
    </tr>
    <tr>
        <td align="right" valign="top">
            <asp:Label ID="Label12" Runat="server" Text="Spacing :" />
        </td>
        <td valign="top">
            <asp:TextBox ID="TextBoxSpacing" runat="server" Width="60px" />
            <asp:Button ID="Button2" runat="server" CssClass="ButtonStyle" 
                onclick="ButtonSpacingOk_Click" Text="Ok" />
        </td>
    </tr>
</table>
</asp:Panel>

<table cellpadding="6" border="0" >
    <tr>
        <td align="right" valign="top">
            <asp:Label ID="Label9" Runat="server" Text="Width :" ToolTip="" />
        </td>
        <td valign="top">
            <asp:TextBox ID="TextBoxWidth" runat="server" 
                ontextchanged="TextBoxWidth_TextChanged" Width="60px"></asp:TextBox>
            <asp:Button ID="ButtonWidthOk" runat="server" CssClass="ButtonStyle" 
                onclick="ButtonWidthOk_Click" Text="Ok" />
        </td>
        <td rowspan="10" style="border:solid 1px Gray" valign="middle" title="L'objet édité">
            <asp:Panel ID="PanelObjet" runat="server" />
        </td>
        <td rowspan="10" valign="top">
            &nbsp;</td>
    </tr>
    <tr>
        <td align="right" valign="top">
            <asp:Label ID="Label10" Runat="server" Text="Height :" />
        </td>
        <td valign="top">
            <asp:TextBox ID="TextBoxHeight" runat="server" 
                ontextchanged="TextBoxHeight_TextChanged" Width="60px"></asp:TextBox>
            <asp:Button ID="ButtonHeightOk" runat="server" CssClass="ButtonStyle" 
                onclick="ButtonHeightOk_Click" Text="Ok" />
        </td>
    </tr>
    <tr>
        <td align="right" valign="top">
            <asp:Label ID="Label3" Runat="server" Text="Border Style :" />
        </td>
        <td valign="top">
            <asp:dropdownlist id="DropDownListBorderStyle" 
                Runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="DropDownListBorderStyle_IndexChanged">
            </asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td align="right" valign="top">
            <asp:Label ID="Label4" Runat="server" Text="Border Width :" />
        </td>
        <td valign="top">
            <asp:dropdownlist id="DropDownListBorderWidthList" 
                Runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="DropDownListBorderWidthList_SelectedIndexChanged">
            </asp:dropdownlist>
            <asp:TextBox ID="TextBoxBorderWidth" runat="server" Width="40px" 
                ontextchanged="TextBoxBorderWidth_TextChanged"></asp:TextBox>
            <asp:Button ID="ButtonBorderWidthOk" runat="server" CssClass="ButtonStyle" 
                Text="Ok" onclick="ButtonBorderWidthOk_Click" />
        </td>
    </tr>
    <tr>
        <td align="right" valign="top">
            <asp:Label ID="Label2" runat="server" Text="Border Color :" />
        </td>
        <td valign="top">
            <asp:dropdownlist id="DropDownListBorderColor" 
                Runat="server" AutoPostBack="True" 
                OnSelectedIndexChanged="DropDownListBorderColor_IndexChanged">
            </asp:dropdownlist>
            <br />
            <asp:TextBox ID="TextBoxBorderColor" runat="server" 
                ontextchanged="TextBoxBorderColor_TextChanged" Width="118px"></asp:TextBox>
            <asp:Button ID="ButtonBorderColorOk" runat="server" CssClass="ButtonStyle" 
                onclick="ButtonBorderColorOk_Click" Text="Ok" />
            <br />
            <usr:ColorPicker ID="ColorPickerBorderColor" runat="server" />
        </td>
    </tr>
    <tr>
        <td align="right" valign="top">
            <asp:Label ID="Label5" Runat="server" Text="Back Color :" />
        </td>
        <td>
            <asp:DropDownList id="DropDownListBackColor" 
                Runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="DropDownListBackColor_IndexChanged" 
                style="height: 22px">
            </asp:DropDownList>
            <br />
            <asp:TextBox ID="TextBoxBackColor" runat="server" 
                OnTextChanged="TextBoxBackColor_TextChanged" Width="118px"></asp:TextBox>
            <asp:Button ID="ButtonBackColorOk" runat="server" CssClass="ButtonStyle" 
                onclick="ButtonBackColorOk_Click" Text="Ok" />
            <br />
            <usr:ColorPicker ID="ColorPickerBackColor" runat="server" />
        </td>
    </tr>
    <tr>
        <td align="right" valign="top">
            <asp:Label ID="Label6" Runat="server" Text="Foreground Color :" />
        </td>
        <td>
            <asp:dropdownlist id="DropDownListForegroundColor" 
                Runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="DropDownListForegroundColor_IndexChanged">
            </asp:dropdownlist>
            <br />
            <asp:TextBox ID="TextBoxForegroundColor" runat="server" 
                ontextchanged="TextBoxForegroundColor_TextChanged" Width="118px"></asp:TextBox>
            <asp:Button ID="ButtonForegroundColorOk" runat="server" 
                onclick="ButtonForegroundColorOk_Click" Text="Ok" CssClass="ButtonStyle" />
            <br />
            <usr:ColorPicker ID="ColorPickerForegroundColor" runat="server" />
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="Label7" Runat="server" Text="Font Name :" />
        </td>
        <td>
            <asp:dropdownlist id="DropDownListFontName" 
                Runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="DropDownListFontName_SelectedIndexChanged">
            </asp:dropdownlist>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Label ID="Label8" Runat="server" Text="Font Size :" />
        </td>
        <td>
            <asp:dropdownlist id="DropDownListFontSize" 
                Runat="server" AutoPostBack="True" 
                OnSelectedIndexChanged="DropDownListFontSize_SelectedIndexChanged">
            </asp:dropdownlist>
            <asp:TextBox ID="TextBoxFontSize" runat="server" Width="40px" 
                ontextchanged="TextBoxFontSize_TextChanged"></asp:TextBox>
            <asp:Button ID="ButtonFontSizeOk" runat="server" CssClass="ButtonStyle" 
                onclick="ButtonFontSizeOk_Click" Text="Ok" />
        </td>
    </tr>
    <tr>
        <td align="right" valign="top">
            <asp:Label ID="Label1" Runat="server" Text="Font Style :" />
        </td>
        <td>
            <asp:CheckBoxList ID="CheckBoxListFontStyle" runat="server" 
                onselectedindexchanged="CheckBoxListFontStyle_SelectedIndexChanged" AutoPostBack="true">
            </asp:CheckBoxList>
        </td>
    </tr>
</table>

<table cellpadding="2" width="100%">
    <tr>
        <td width="30%"></td>
        <td class="TdControlButtonStyle">
            <UserControl:RolloverButton ID="ButtonSauver" runat="server" Text="Sauver" ToolTip="Sauvegarder les modifications" OnClick="ButtonSauver_Click"/>                
        </td>
        <td class="TdControlButtonStyle">
            <UserControl:RolloverButton ID="ButtonRetour" runat="server" Text="Retour" 
                ToolTip="Sans sauvegarder" onclick="ButtonRetour_Click" />                
        <td class="TdControlButtonStyle">
            <UserControl:RolloverButton ID="OnSenFoue5" runat="server" Text="Supprimer" 
                ToolTip="Le style qui ne sera pas appliqué" onclick="ButtonSupprimer_Click" />                
        </td>
        <td width="30%"></td>
    </tr>
</table>

</div>
</div>
</asp:Content>

