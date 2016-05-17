<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Delete.aspx.cs" Inherits="Contact_MemberDelete" Title="Supprimer un membre" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="body">
<div>
    <div>
    <h3>Suppression d'un Membre</h3>
    <h5>Voici les objets de votre compte d'utilisateur</h5>
    <p style="color:Red;font-weight:bold;">Confirmez la suppression de votre compte en cliquant sur le bouton "Supprimer" en bas du formulaire.</p>
    </div>
    
    <table style="border:none" cellpadding="0" cellspacing="0">
        <tr>
            <td align="left">
                <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
            </td>
        </tr>
    </table>

    <table cellpadding="2">
        <tr>
            <td height="60px">
                <UserControl:RolloverButton ID="ButtonSupprimer" runat="server" Text="Supprimer" ToolTip="Confirmer la suppression de ce Membre et des objets associés" OnClick="ButtonSupprimer_Click"/>                
            </td>
            <td height="60px">
                <UserControl:RolloverButton ID="ButtonCancel" runat="server" Text="Retour" ToolTip="Retour à la liste des Membres" OnClick="ButtonCancel_Click"/>                
            </td>
        </tr>
    </table>
    
</div>
</div></asp:Content>

