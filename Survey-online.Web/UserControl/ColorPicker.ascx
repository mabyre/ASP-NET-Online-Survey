<%@ Control Language="C#" AutoEventWireup="false" CodeFile="ColorPicker.ascx.cs" Inherits="ColorPicker" %>

<table style="border:solid 1px" cellpadding="0" cellspacing="3" bgcolor="#F1F1F1">
<tr>
    <td align="center">
        <table>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Resolution :" />
            </td>
            <td>
                <asp:DropDownList ID="DropDownListResolution" runat="server" 
                    AutoPostBack="true" 
                    onselectedindexchanged="DropDownListResolution_SelectedIndexChanged" >
                    <asp:ListItem>Fine</asp:ListItem>
                    <asp:ListItem>Medium</asp:ListItem>
                    <asp:ListItem>Coarse</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label2" runat="server" Text="Size :" />
            </td>
            <td>
                <asp:DropDownList ID="DropDownListSize" runat="server" 
                AutoPostBack="true" 
                onselectedindexchanged="DropDownListSize_SelectedIndexChanged" >
                    <asp:ListItem>Large</asp:ListItem>
                    <asp:ListItem>Medium</asp:ListItem>
                    <asp:ListItem>Small</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        </table>
        <asp:Table ID="tblPicker" runat="server" CellPadding="0" CellSpacing="0" BorderColor="Black"
            BorderStyle="Solid" BorderWidth="1">
        </asp:Table>
        <asp:Table ID="tblShow" runat="server" CellPadding="0" CellSpacing="0">
            <asp:TableRow ID="rowShow" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1">
                <asp:TableCell ID="cellPreview" runat="server" Text="#FFFFFF" Width="100%" ColumnSpan="5"
                    HorizontalAlign="Center"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="rowPalettes" runat="server" BorderColor="Black" BorderStyle="Solid"
                BorderWidth="1">
                <asp:TableCell ID="cellPal1" runat="server" Width="20%" Text="1" HorizontalAlign="Center"
                    BorderColor="Black" BorderStyle="Dotted" BorderWidth="1" BackColor="White" ForeColor="Black"
                    Style="cursor: hand"></asp:TableCell>
                <asp:TableCell ID="cellPal2" runat="server" Width="20%" Text="2" HorizontalAlign="Center"
                    BorderColor="Black" BorderStyle="Dotted" BorderWidth="1" BackColor="White" ForeColor="Black"
                    Style="cursor: hand"></asp:TableCell>
                <asp:TableCell ID="cellPal3" runat="server" Width="20%" Text="3" HorizontalAlign="Center"
                    BorderColor="Black" BorderStyle="Dotted" BorderWidth="1" BackColor="White" ForeColor="Black"
                    Style="cursor: hand"></asp:TableCell>
                <asp:TableCell ID="cellPal4" runat="server" Width="20%" Text="4" HorizontalAlign="Center"
                    BorderColor="Black" BorderStyle="Dotted" BorderWidth="1" BackColor="White" ForeColor="Black"
                    Style="cursor: hand"></asp:TableCell>
                <asp:TableCell ID="cellPal5" runat="server" Width="20%" Text="5" HorizontalAlign="Center"
                    BorderColor="Black" BorderStyle="Dotted" BorderWidth="1" BackColor="White" ForeColor="Black"
                    Style="cursor: hand"></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </td>
</tr>
</table>
        