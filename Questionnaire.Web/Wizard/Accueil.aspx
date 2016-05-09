<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="Accueil.aspx.cs" Inherits="Wizard_Accueil" Title="Accueil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<a name="HautDePage"></a>
<div class="DivPageAccueilStyle">

<table border="0" cellpadding="5px" cellspacing="3px" width="550px">
    <tr>
        <td>
            <h3>Accueil</h3>
        </td>
    </tr>
    <tr>
        <td class="LoginUserText" align="center">
            <label>Bonjour : </label>
            <asp:Label ID="LabelUtilisateur" runat="server"/>
            (<asp:LoginName ID="LoginName1" runat="server" />)
        </td>
    </tr>
    <tr>
        <td class="LoginUserText">
            <asp:Label ID="LabelNombreQuestionnaires" runat="server" Text="Vous avez actuellement " />
            <span class="SpanHyperLinkAccueilStyle">
            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="HyperLinkAccueilStyle" 
                ToolTip="Accédez à la liste de vos Questionnaires pour sélectionner un questionnaire et travailler avec"
                Text=" questionnaires en cours" 
                NavigateUrl="~/Questionnaire/Manage.aspx" />
            </span>
        </td>
    </tr>
</table>

<asp:Panel ID="PanelMembreAbonnement" runat="server" Width="550px">
    <table ID="TableMembreAbonnement" runat="server" border="0" cellpadding="0" cellspacing="0">
    <thead >
        <tr>
            <td id="TdTableMembreAbonnementHead2" runat="server" colspan="2">
            <asp:Label ID="LabelAbonnementHead2" runat="server"></asp:Label>
            </td>
            <td id="TdTableMembreAbonnementHead1" runat="server" >
            <asp:Label ID="LabelAbonnementHead1" runat="server"></asp:Label>
            </td>
        </tr>
    </thead>
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Text="Nombre de Questionnaires : " />
        </td>
        <td>
            <asp:Label ID="LabelQuestionnaires" runat="server"/>
        </td>
        <td>
            <asp:Label ID="LabelLimiteQuestionnaires" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label2" runat="server" Text="Nombre de Questions : " />
        </td>
        <td>
            <asp:Label ID="LabelQuestions" runat="server" />
        </td>
        <td>
            <asp:Label ID="LabelLimiteQuestions" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label3" runat="server" Text="Nombre d'Interviewés : " />
        </td>
        <td>
            <asp:Label ID="LabelInterviewes" runat="server" />
        </td>
        <td>
            <asp:Label ID="LabelLimiteInterviewes" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label4" runat="server" Text="Nombre de Réponses : " />
        </td>
        <td>
            <asp:Label ID="LabelReponses" runat="server" />
        </td>
        <td>
            <asp:Label ID="LabelLimiteReponses" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
        </td>
        <td>
            <asp:Label ID="LabelDateFinAbonnement" runat="server" ToolTip="Date de fin de votre abonnement" />
        </td>
    </tr>
    </table>
</asp:Panel>
    
<table border="0" cellpadding="3" cellspacing="0" >
    <tr>
        <td>
        </td>
        <td>
            <asp:ImageButton ID="ImageButton1" runat="server" BorderColor="#5282D4" 
                BorderStyle="Solid" BorderWidth="1px" 
                ImageUrl="~/Images/Wizard/questionnaire.jpg" 
                OnClick="ButtonCreerQuestionnaire_Click" 
                ToolTip="Créez un nouveau Questionnaire ou le créer à partir d'un exemple, Ajouter des Questions à un Questionnaire existant" />
        </td>
        <td valign="top" align="right" >
            <label class="LabelStyle" title="étape 4"></label>
        </td>
        <td>
            <asp:ImageButton ID="Image4" runat="server" BorderColor="#5282D4" 
                BorderStyle="Solid" BorderWidth="1px"
                ImageUrl="~/Images/Wizard/ajouter_questions.jpg" 
                ToolTip="Ajoutez des Questions à votre Questionnaire" 
                onclick="ButtonAjouterQuestion_Click" />
        </td>
        <td>
        </td>
        <td>
            <asp:ImageButton ID="Image8" runat="server" BorderColor="#5282D4" 
                BorderStyle="Solid" BorderWidth="1px"
            ImageUrl="~/Images/Wizard/ajouter questions enchainees.jpg" 
            ToolTip="Ajoutez plusieures Questions à votre Questionnaire" 
            onclick="ButtonAjouterQuestionEnchainee_Click" />
        </td>
        <td>
            &nbsp;</td>
        <td>
            <asp:ImageButton ID="Image6" runat="server" BorderColor="#5282D4" 
                BorderStyle="Solid" BorderWidth="1px"
            ImageUrl="~/Images/Wizard/ajouter des questions.jpg" 
            ToolTip="Visualisez, Modifiez vos Questionnaires" 
            onclick="ButtonQuestionnaireList_Click" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td class="TableAccueilButtonStyle">
            <asp:Button ID="ButtonCreerQuestionnaire" 
                CssClass="ButtonAccueilStyle"
                runat="server" Text="Créez Questions" 
                ToolTip="Créez un nouveau Questionnaire ou le créer à partir d'un exemple, Ajouter des Questions à un Questionnaire existant"
                onclick="ButtonCreerQuestionnaire_Click" />
        </td>
        <td>
        </td>
        <td class="TableAccueilButtonStyle">
        <asp:Button ID="ButtonAjouterQuestion" CssClass="ButtonAccueilStyle" 
            runat="server" Text="Ajoutez Questions" 
            ToolTip="Ajoutez des Questions à votre Questionnaire" 
            onclick="ButtonAjouterQuestion_Click" />
        </td>
        <td>
        </td>
        <td class="TableAccueilButtonStyle">
            <asp:Button ID="ButtonAjouterQuestionEnchainee" CssClass="ButtonAccueilStyle" 
                runat="server" Text="Ajouter +Questions" 
                ToolTip="Ajouter plusieures Questions à votre Questionnaire" 
                onclick="ButtonAjouterQuestionEnchainee_Click"/>
        </td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
        <asp:Button ID="ButtonQuestionnaireList" CssClass="ButtonAccueilStyle" 
            runat="server" Text="Visualisez Questions" 
            ToolTip="Visualisez, Modifier vos Questionnaires" 
            onclick="ButtonQuestionnaireList_Click"/>
        </td>
   </tr>
    <tr height="10">
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
   </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            <asp:ImageButton ID="Image5" runat="server" BorderColor="#5282D4" 
                BorderStyle="Solid" BorderWidth="1px"
            ImageUrl="~/Images/Wizard/accueil.gif" 
            ToolTip="Rédigez l'email aux interviewés" 
            onclick="ButtonEditerPageAccueil_Click" />
        </td>
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            <asp:ImageButton ID="Image9" runat="server" BorderColor="#5282D4" 
                BorderStyle="Solid" BorderWidth="1px"
                ImageUrl="~/Images/Wizard/rediger email.jpg" 
                ToolTip="Rédigez l'email aux interviewés" 
                onclick="ButtonRedigerEmail_Click" />
        </td>
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            <asp:ImageButton ID="Image10" runat="server" BorderColor="#5282D4" 
                BorderStyle="Solid" BorderWidth="1px"
            ImageUrl="~/Images/Wizard/importer.jpg" 
            ToolTip="Importez les contacts que vous allez interviewer" 
            onclick="ButtonImporter_Click" />
        </td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            <asp:ImageButton ID="Image11" runat="server" BorderColor="#5282D4" 
                BorderStyle="Solid" BorderWidth="1px"
            ImageUrl="~/Images/Wizard/exporter.jpg" 
            ToolTip="Gérez l'envoi des emails aux interviewés" 
            onclick="ButtonEvoyerEmail_Click" />
        </td>
   </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
        <asp:Button ID="ButtonEditerPageAccueil" CssClass="ButtonAccueilStyle"
            runat="server" Text="Editez Accueil" 
            ToolTip="Editez la page d'accueil des interviewés" 
            onclick="ButtonEditerPageAccueil_Click"/>
        </td>
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
        <asp:Button ID="ButtonRedigerEmail" CssClass="ButtonAccueilStyle" 
            runat="server" Text="Editez l'Email" 
            ToolTip="Rédigez l'email envoyé aux interviewés" 
            onclick="ButtonRedigerEmail_Click" />
        </td>
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            <asp:Button ID="ButtonImporter" CssClass="ButtonAccueilStyle" 
                runat="server" Text="Importez Interviewés" 
                ToolTip="Importez les contacts que vous allez interviewer" 
                onclick="ButtonImporter_Click"/>
        </td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
        <asp:Button ID="ButtonEvoyerEmail" CssClass="ButtonAccueilStyle" 
            runat="server" Text="Envoyez les emails" 
            ToolTip="Gérez l'envoi des emails aux interviewés" 
            onclick="ButtonEvoyerEmail_Click"/>
        </td>
   </tr>
    <tr height="10">
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
   </tr>
</table>

<table border="0" cellpadding="3" cellspacing="0" width="550px" >   
    <tr>
        <td class="TableAccueilButtonStyle">
            <asp:ImageButton ID="Image7" runat="server" BorderColor="#5282D4" 
                BorderStyle="Solid" BorderWidth="1px"
                ImageUrl="~/Images/Wizard/Bar-2DRectangles.gif"
                ToolTip="Statistiques sur les votants" 
                onclick="ButtonStatistiques_Click" />
        </td>
   </tr>
    <tr>
        <td class="TableAccueilButtonStyle">
        <asp:Button ID="ButtonStatistiques" CssClass="ButtonAccueilStyle" 
            runat="server" Text="Dépouillez Stats" 
            ToolTip="Statistiques sur les votants" 
            onclick="ButtonStatistiques_Click"/>
        </td>
   </tr>
</table>

<table border="0" cellpadding="3" cellspacing="0">
    <tr>
        <td height="35px">
        </td>
    </tr>
    <tr>
        <td width="16px">
            <asp:ImageButton ID="ImageButtonExpandNavigation" runat="server" 
                ImageUrl="~/Images/expand.jpg" 
                onclick="ImageButtonExpandNavigation_Click" 
                ToolTip="Afficher le volet de contrôle de la navigation, ajouter ou supprimer des moyens de navigation" />                
        </td>
    </tr>
</table>

<asp:Panel ID="PanelControlNavigation" GroupingText="Navigation" runat="server" Visible="false">
<table border="0" cellpadding="3" cellspacing="0">
    <tr>
        <td align="center" colspan="2" height="35px">
            <label class="LabelStyle" title="Configurer l'interface graphique">Voulez-vous afficher ?</label>
        </td>
    </tr>    
    <tr visible="false" runat="server">
        <td align="right">
            <label class="LabelStyle" title="Afficher l'aide en ligne tout au long de votre session">L'aide en ligne</label>
        </td>
        <td align="left">
            <asp:CheckBox ID="CheckBoxAideEnLigne" runat="server" AutoPostBack="True" 
            OnCheckedChanged="CheckBoxAideEnLigne_CheckedChanged" />
        </td>
    </tr>    
    <tr>
        <td align="right">
            <label class="LabelStyle" title="Afficher l'aide contextuelle tout au long de votre session">L'aide contextuelle</label>
        </td>
        <td align="left">
            <asp:CheckBox ID="CheckBoxAideContextuelle" runat="server" AutoPostBack="True" 
            OnCheckedChanged="CheckBoxAideContextuelle_CheckedChanged" />
        </td>
    </tr>    
    <tr>
        <td align="right">
            <label class="LabelStyle" title="Afficher le menu expert">Le menu de navigation</label>
        </td>
        <td align="left">
            <asp:CheckBox ID="CheckBoxMenuExpert" runat="server" AutoPostBack="True" 
            oncheckedchanged="CheckBoxMenuExpert_CheckedChanged" />
        </td>
    </tr>    
    <tr>
        <td align="right">
            <label class="LabelStyle" title="Afficher le menu étape par étape">Le menu en colonne à gauche</label>
        </td>
        <td align="left">
            <asp:CheckBox ID="CheckBoxMenuColonne" runat="server" AutoPostBack="True" 
                oncheckedchanged="CheckBoxMenuColonne_CheckedChanged"/>
        </td>
    </tr>    
    <tr>
        <td align="right">
            <label class="LabelStyle" title="Barre site map">La barre de navigation</label>
        </td>
        <td align="left">
            <asp:CheckBox ID="CheckBoxBarreNavigation" runat="server" AutoPostBack="True" 
                oncheckedchanged="CheckBoxBarreNavigation_CheckedChanged"/>
        </td>
    </tr>    
    <tr>
        <td align="right">
            <label class="LabelStyle" title="Bouton qui suit le déroulement de la page (Haut de page)">Les boutons toujours visibles</label>
        </td>
        <td align="left">
            <asp:CheckBox ID="CheckBoxMenuToujoursVisible" runat="server" AutoPostBack="True" 
                oncheckedchanged="CheckBoxMenuToujoursVisible_CheckedChanged"/>
        </td>
    </tr>    
    <tr>
        <td align="right">
            <label class="LabelStyle">Boutons toujours visibles position :</label>
        </td>
        <td align="left">
            <asp:DropDownList ID="DropDownListMenuToujoursVisiblePosition" runat="server" AutoPostBack="True" 
                OnSelectedIndexChanged="DropDownListMenuToujoursVisiblePosition_SelectedIndexChanged">
                <asp:ListItem Text="Defaut" Selected="true"  Value="None" />
                <asp:ListItem Text="Haut Gauche" Value="HG" />
                <asp:ListItem Text="Haut Centre" Value="HC" />
                <asp:ListItem Text="Haut Droite"  Value="HD" />
                <asp:ListItem Text="Milieu Gauche" Value="MG" />
                <asp:ListItem Text="Milieu Center" Value="MC" />
                <asp:ListItem Text="Milieu Droite"  Value="MD" />
                <asp:ListItem Text="Bas Gauche" Value="BG" />
                <asp:ListItem Text="Bas Centre" Value="BC" />
                <asp:ListItem Text="Bas Droite" Value="BD" />
            </asp:DropDownList>
        </td>
    </tr>    
    <tr>
        <td align="center" colspan="2" height="55px">
            <asp:Button ID="ButtonParDefaut" runat="server" CssClass="ButtonStyle" ToolTip="Configurer l'interface de navigation par défaut"
                Text="Par défaut" onclick="ButtonParDefaut_Click" />
        </td>
    </tr>    
</table>
</asp:Panel>

</div>
</asp:Content>

