<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Copier.aspx.cs" Inherits="Questionnaire_Copier" Title="Copier un Questionnaire" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="DivPageStyle">
<div>
    <div>
    <br />
    <h3>Copier un Questionnaire</h3>
    <br />
    </div>
    
    <table style="border:none" cellpadding="0" cellspacing="0">
        <tr>
            <td align="left">
                <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
            </td>
        </tr>
    </table>

    <table cellpadding="2" width="100%" border="0">
        <tr>
            <td width="30%">
            </td>
            <td height="60px">
                <UserControl:RolloverButton ID="ButtonCopier" runat="server" Text="Copier" ToolTip="Confirmer la copie du Questionnaire" OnClick="ButtonCopier_Click"/>                
            </td>
            <td>
                <UserControl:RolloverButton ID="ButtonCancel" runat="server" Text="Retour" ToolTip="Retour à la liste des Questionnaires" OnClick="ButtonCancel_Click"/>                
            </td>
            <td>
                <UserControl:RolloverButton ID="ButtonAjouterQuestion" runat="server" Text="Ajouter" ToolTip="Ajoutez des Questions à vos questionnaires" OnClick="ButtonAjouterQuestion_Click" Visible="false"/>                
            </td>
            <td width="30%">
            </td>
        </tr>
    </table>
    
</div>
</div>
</asp:Content>