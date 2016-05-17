<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Delete.aspx.cs" Inherits="Questionnaire_Edit" Title="Edition d'un Questionnaire" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<a name="HautDePage"></a>
<div id="DivPageStyle">
<div>
    <div>
    <br />
    <h3>Suppression d'un Questionnaire</h3>
    <br />
    </div>
    
    <table style="border:none" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left">
                <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
            </td>
        </tr>
    </table>

    <!-- Boutons de controls -->
    <table cellpadding="2" width="100%">
        <tr>
            <td width="50%">                
            </td>
            <td height="60px">
                <UserControl:RolloverButton ID="ButtonSupprimer" runat="server" Text="Supprimer" ToolTip="Confirmer la suppression du Questionnaire et les objets associés" OnClick="ButtonSupprimer_Click"/>                
            </td>
            <td height="60px">
                <UserControl:RolloverButton ID="ButtonCancel" runat="server" Text="Retour" ToolTip="Retour à la liste des Questionnaires" OnClick="ButtonCancel_Click"/>                
            </td>
            <td width="50%">                
            </td>
        </tr>
    </table>
    
</div>
</div>
</asp:Content>