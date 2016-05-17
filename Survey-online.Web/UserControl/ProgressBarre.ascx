<%@ Control ClassName="ProgessBarre" Language="C#" AutoEventWireup="true" CodeFile="ProgressBarre.ascx.cs" Inherits="ProgessBarre" %>

<table border="0" cellpadding="0" cellspacing="3">
<tr>
    <td>
        <table border="0" cellpadding="0" cellspacing="3">
        <tr>
            <td colspan="2">
                <asp:Label ID="LabelMessages" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Panel ID="PanelBarSide" runat="server"
                    BorderStyle="Solid" 
                    BorderWidth="1px">
                    <asp:Panel ID="PanelProgress" runat="server" />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelProgress" runat="server"></asp:Label>
            </td>
        </tr>
        </table>
    </td>
</tr>
</table>
