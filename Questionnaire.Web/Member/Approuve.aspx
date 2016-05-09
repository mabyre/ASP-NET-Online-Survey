<%@ Page Language="C#" MasterPageFile="~/Print.master" AutoEventWireup="true" CodeFile="Approuve.aspx.cs" Inherits="Member_Approuve" Title="Page sans titre" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="body">
<div class="DivMemberRegister">
    <h3>Approbation d'un nouvel utilisateur</h3>
    <asp:Panel ID="PanelMessageApprobation" runat="server" Visible="true">
        <div class="DivTableStyle">
            <table border="0" cellpadding="25px" cellspacing="0">
                <tr>
                    <td height="20px" align="center">
                        <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button runat="server" CssClass="ButtonStyle" ID="ButtonLogin" 
                            Text="Connexion" 
                            OnClick="ButtonLogin_Click"
                            Visible="false"/>
                    </td>
                </tr>
            </table>
    </asp:Panel>
</div>
</div>
</asp:Content>

