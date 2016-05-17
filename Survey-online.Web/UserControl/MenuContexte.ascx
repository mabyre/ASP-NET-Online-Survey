<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuContexte.ascx.cs" Inherits="UserControl_MenuContexte" %>

<asp:Panel ID="PanelAccueil" runat="server">
<table border="0" cellpadding="15px" width="60px">
<tr>
    <td valign="top" align="center">
        <asp:ImageButton ID="Image3" runat="server" 
            ImageUrl="~/Images/bureau.gif"
            ToolTip="Retour à la page d'accueil"  
            onclick="ButtonRetourAccueil_Click" />
    </td>
</tr>
</table>
</asp:Panel>

<asp:Panel ID="PanelWizardQuestion" runat="server" Visible="false">
<table border="0" cellpadding="5px">
<tr>
    <td valign="top" align="center">
        <asp:Button ID="ButtonWizardQuestion" CssClass="ButtonMenuWizardStyle" 
            runat="server" Text="Ajoutez des Questions" 
            ToolTip="Ajoutez des Questions à vos Questionnaires" 
            onclick="ButtonWizardQuestion_Click" />
    </td>
</tr>
</table>
</asp:Panel>

<asp:Panel ID="PanelPollList" runat="server" Visible="false">
<table border="0" cellpadding="5px">
<tr>
    <td valign="top" align="center">
        <asp:Button ID="ButtonPollList" CssClass="ButtonMenuWizardStyle" 
            runat="server" Text="Visualisez le Questionnaire" 
            ToolTip="Visualisez le Questionnaire en mode listing" 
            onclick="ButtonPollList_Click" />
    </td>
</tr>
</table>
</asp:Panel>

<asp:Panel ID="PanelPollQuestionnaire" runat="server" Visible="false">
<table border="0" cellpadding="5px">
<tr>
    <td valign="top" align="center">
        <asp:Button ID="ButtonPollQuestionnaire" CssClass="ButtonMenuWizardStyle" 
            runat="server" Text="Testez Questionnaire" 
            ToolTip="Testez le Questionnaire" 
            onclick="ButtonPollQuestionnaire_Click" />
    </td>
</tr>
</table>
</asp:Panel>

<asp:Panel ID="PanelWizardQuestionEnchainee" runat="server" Visible="false">
<table border="0" cellpadding="5px">
<tr>
    <td valign="top" align="center">
        <asp:Button ID="ButtonWizardQuestionEnchainee" CssClass="ButtonMenuWizardStyle" 
            runat="server" Text="Ajouter + Questions"
            ToolTip="Ajouter plusieures Questions à la fois" 
            onclick="ButtonWizardQuestionEnchainee_Click" />
    </td>
</tr>
</table>
</asp:Panel>

<asp:Panel ID="PanelStatistiques" runat="server" Visible="false">
<table border="0" cellpadding="5px">
<tr>
    <td valign="top" align="center">
        <asp:Button ID="Button1" CssClass="ButtonMenuWizardStyle" 
            runat="server" Text="Statistiques" 
            ToolTip="Visualisez les Statistiques" 
            onclick="ButtonStatistiques_Click" />
    </td>
</tr>
</table>
</asp:Panel>
