<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoginBanner.ascx.cs" Inherits="Controls_LoginBanner" %>
<asp:LoginView ID="LoginView1" runat="server">
    <LoggedInTemplate>
        <table border="0" cellpadding="2" cellspacing="3">
            <tr>
                <td>
                    <asp:LoginName ID="LoginName1" runat="server" CssClass="LoginNameStyle" />
                    <asp:Label ID="LabelUserInRoles" runat="server" Text="Label" CssClass="LabelUserInRolesStyle" />
                </td>
            </tr>
            <tr>
                <td valign="middle">
                    <asp:LinkButton ID="LinkButtonLogout" runat="server" Text="Logout" CssClass="ButtonStyle" OnClick="LinkButtonLogout_Click" />
                    <p style="font-size:1px;"></p>
                </td>
            </tr>
        </table>
    </LoggedInTemplate>
    <AnonymousTemplate>
        <table border="0" cellpadding="2" cellspacing="3">
            <tr>
                <td>
                    <!--
                    <asp:HyperLink ID="HyperLinkLogin" runat="server" CssClass="ButtonStyle" Text="Login" NavigateURL="~/Login.aspx?ReturnURL=" />
                    &nbsp;
                    <asp:HyperLink ID="HyperLinkRegister" runat="server" CssClass="ButtonStyle" Text="Register" NavigateURL="~/Member/Register.aspx" />
                    -->
                </td>
            </tr>
        </table>
    </AnonymousTemplate>
</asp:LoginView>
