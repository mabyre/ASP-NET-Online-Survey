<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuWizard.ascx.cs" Inherits="UserControl_MenuWizard" %>
<table border="0" class="TableMenuWizardStyle">
    <tr>
        <td valign="top" width="16px" align="right">
            <label class="LabelStyle" title="étape 1">1</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButton1" runat="server" 
                ImageUrl="~/Images/Wizard/questionnaire.jpg" 
                onclick="ButtonCreerQuestionnaire_Click" 
                ToolTip="Créez un Questionnaire à partir d'un exemple ou créer un nouveau Questionnaire" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td class="TableMenuWizardButtonStyle">
            <asp:Button ID="ButtonCreerQuestionnaire" CssClass="ButtonMenuWizardStyle"
                runat="server" Text="Créez Questionnaire" 
                ToolTip="Créez un Questionnaire à partir d'un exemple ou créer un nouveau Questionnaire" 
                onclick="ButtonCreerQuestionnaire_Click" />
        </td>
    </tr>
    <tr>
        <td valign="top" width="16px" align="right">
            <label class="LabelStyle" title="étape 2">2</label>
        </td>
        <td>
            <asp:ImageButton ID="Image2" runat="server" 
                ImageUrl="~/Images/Wizard/ajouter_questions.jpg" 
                onclick="ButtonAjouterQuestion_Click" 
                ToolTip="Ajoutez des Questions à vos Questionnaires" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td class="TableMenuWizardButtonStyle">
            <asp:Button ID="ButtonAjouterQuestion" CssClass="ButtonMenuWizardStyle" 
                runat="server" Text="Ajoutez des Questions" 
                ToolTip="Ajoutez des Questions à vos Questionnaires" 
                onclick="ButtonAjouterQuestion_Click" />
        </td>
    </tr>
    <tr>
        <td valign="top" width="16px" align="right">
            <label class="LabelStyle" title="étape 3">3</label>
        </td>
        <td>
            <asp:ImageButton ID="Image3" runat="server" 
                ImageUrl="~/Images/Wizard/ajouter des questions.jpg"
                ToolTip="Visusalisez, modifier, tester vos questionnaires"  
                onclick="ButtonModifier_Click" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td class="TableMenuWizardButtonStyle">
            <asp:Button ID="ButtonViusaliser" CssClass="ButtonMenuWizardStyle" 
                runat="server" Text="Visualisez Questionnaires" 
                ToolTip="Visusalisez, modifier, tester vos questionnaires" 
                onclick="ButtonModifier_Click"/>
        </td>
    </tr>
    <tr>
        <td valign="top" width="16px" align="right">
            <label class="LabelStyle" title="étape 4">4</label>
        </td>
        <td>
            <asp:ImageButton ID="Image4" runat="server" 
                ImageUrl="~/Images/Wizard/rediger email.jpg" 
                ToolTip="Rédigez l'email aux interviewés" 
                onclick="ButtonRedigerEmail_Click" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td class="TableMenuWizardButtonStyle">
            <asp:Button ID="ButtonRedigerEmail" CssClass="ButtonMenuWizardStyle" 
                runat="server" Text="Editez l'email envoyé" 
                ToolTip="Rédigez l'email qui sera envoyé aux interviewés" 
                onclick="ButtonRedigerEmail_Click" />
        </td>
    </tr>
    <tr>
        <td valign="top" width="16px" align="right">
            <label class="LabelStyle" title="étape 5">5</label>
        </td>
        <td>
            <asp:ImageButton ID="Image5" runat="server" 
            ImageUrl="~/Images/Wizard/accueil.gif" 
            ToolTip="Editez la page d'accueil" 
            onclick="ButtonEditerPageAccueil_Click" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td class="TableMenuWizardButtonStyle">
            <asp:Button ID="ButtonEditerPageAccueil" CssClass="ButtonMenuWizardStyle"
                runat="server" Text="Editez page d'accueil" 
                ToolTip="Editez la page d'accueil des interviewés" 
                onclick="ButtonEditerPageAccueil_Click"/>
        </td>
    </tr>
    <tr>
        <td valign="top" width="16px" align="right">
            <label class="LabelStyle" title="étape 6">6</label>
        </td>
        <td>
            <asp:ImageButton ID="Image8" runat="server" 
            ImageUrl="~/Images/Wizard/importer.jpg" 
            ToolTip="Importez les contacts que vous allez interviewer" 
            onclick="ButtonImporter_Click" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td class="TableMenuWizardButtonStyle">
            <asp:Button ID="ButtonImporter" CssClass="ButtonMenuWizardStyle" 
                runat="server" Text="Importez Interviewés" 
                ToolTip="Importez les contacts que vous allez interviewer" 
                onclick="ButtonImporter_Click"/>
        </td>
    </tr>
    <tr>
        <td valign="top" width="16px" align="right">
            <label class="LabelStyle" title="étape 7">7</label>
        </td>
        <td>
            <asp:ImageButton ID="Image6" runat="server" 
            ImageUrl="~/Images/Wizard/exporter.jpg" 
            ToolTip="Gérez l'envoi des emails aux interviewés" 
            onclick="ButtonEvoyerEmail_Click" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td class="TableMenuWizardButtonStyle">
            <asp:Button ID="ButtonEvoyerEmail" CssClass="ButtonMenuWizardStyle" 
                runat="server" Text="Envoyez Emails" 
                ToolTip="Gérez l'envoi des emails aux interviewés" 
                onclick="ButtonEvoyerEmail_Click"/>
        </td>
    </tr>
    <tr>
        <td valign="top" width="16px" align="right">
            <label class="LabelStyle" title="étape 8">8</label>
        </td>
        <td>
            <asp:ImageButton ID="Image7" runat="server" 
                ImageUrl="~/Images/Wizard/Bar-2DRectangles.gif"
                ToolTip="Statistiques sur les votants" 
                onclick="ButtonStatistiques_Click" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td class="TableMenuWizardButtonStyle">
            <asp:Button ID="ButtonStatistiques" CssClass="ButtonMenuWizardStyle" 
                runat="server" Text="Dépouillez Statistiques" 
                ToolTip="Statistiques sur les votants" 
                onclick="ButtonStatistiques_Click"/>
        </td>
    </tr>
</table>
