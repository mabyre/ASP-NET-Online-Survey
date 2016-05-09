<%@ Control ClassName="PopupTextBox" Language="C#" AutoEventWireup="true" CodeFile="PopupTextBox.ascx.cs" Inherits="PopupTextBox" %>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td id="TdLabelChecbox" runat="server" valign="top" height="25px" >
            <asp:Label ID="LabelChecbox" runat="server" />
            <input id="CheckBoxToggleTextBox" runat="server" type="checkbox" />
        </td>
        <td id="TdTextBox" runat="server" style="display:none" valign="top" width="160px"> 
            <asp:TextBox ID="TextBoxText" runat="server"/>
        </td>
    </tr>
</table>
