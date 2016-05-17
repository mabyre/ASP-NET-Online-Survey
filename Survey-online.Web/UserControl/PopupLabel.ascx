<%@ Control ClassName="PopupLabel" Language="C#" AutoEventWireup="true" CodeFile="PopupLabel.ascx.cs" Inherits="PopupLabel" %>
<table border="0" cellpadding="0" cellspacing="3">
    <tr>
        <td align="right">
            <asp:Label ID="LabelChecbox" runat="server" />
            <input id="CheckBoxToggleLabel" runat="server" type="checkbox" />
        </td>
        <td id="TdPopupLabel" runat="server" align="left"> 
            <asp:Label ID="PopupLabelText" runat="server" style="display:none"/>
        </td>
    </tr>
</table>
