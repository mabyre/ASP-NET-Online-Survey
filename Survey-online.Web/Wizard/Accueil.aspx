<%@ Page Language="C#" 
    MasterPageFile="~/MasterPage.master" 
    MaintainScrollPositionOnPostback="true" 
    AutoEventWireup="true" 
    CodeFile="Accueil.aspx.cs" 
    Inherits="Wizard_Accueil" 
    Title="Accueil" 
    culture="auto" 
    meta:resourcekey="PageResource1" 
    uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <a name="HautDePage"></a>
<div class="DivPageAccueilStyle">

<table border="0" cellpadding="5px" cellspacing="3px" width="550px">
    <tr>
        <td>
            <h3><asp:Label ID="LabelAccueil" runat="server" meta:resourcekey="LabelAccueilResource1" Text="Accueil" /></h3>
        </td>
    </tr>
    <tr>
        <td class="LoginUserText" align="center">
            <asp:Label ID="LabelBonjour" runat="server" meta:resourcekey="LabelBonjourResource1" Text="Bonjour :" />
            <asp:Label ID="LabelUtilisateur" runat="server" />
            (<asp:LoginName ID="LoginName1" runat="server" meta:resourcekey="LoginName1Resource1" />)
        </td>
    </tr>
    <tr>
        <td class="LoginUserText">
            <asp:Label ID="LabelNombreQuestionnaires" runat="server" Text="Vous avez actuellement " meta:resourcekey="LabelNombreQuestionnairesResource1" />
            <span class="SpanHyperLinkAccueilStyle">
            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="HyperLinkAccueilStyle" 
                ToolTip="Accédez à la liste de vos Questionnaires pour sélectionner un questionnaire et travailler avec"
                Text=" questionnaires en cours" 
                NavigateUrl="~/Questionnaire/Manage.aspx" meta:resourcekey="HyperLink1Resource1" />
            </span>
        </td>
    </tr>
</table>

<asp:Panel ID="PanelMembreAbonnement" runat="server" Width="550px" meta:resourcekey="PanelMembreAbonnementResource1">
    <table ID="TableMembreAbonnement" runat="server" border="0" cellpadding="0" cellspacing="0">
    <tr runat="server">
        <td id="TdTableMembreAbonnementHead2" runat="server" colspan="2">
            <asp:Label ID="LabelAbonnementHead2" runat="server" />
        </td>
        <td id="TdTableMembreAbonnementHead1" runat="server">
            <asp:Label ID="LabelAbonnementHead1" runat="server"/>
        </td>
    </tr>
    <tr runat="server">
        <td runat="server">
            <asp:Label ID="Label1" runat="server" Text="Nombre de Questionnaires : " />
        </td>
        <td runat="server">
            <asp:Label ID="LabelQuestionnaires" runat="server" />
        </td>
        <td runat="server">
            <asp:Label ID="LabelLimiteQuestionnaires" runat="server" />
        </td>
    </tr>
    <tr runat="server">
        <td runat="server">
            <asp:Label ID="Label2" runat="server" Text="Nombre de Questions : " />
        </td>
        <td runat="server">
            <asp:Label ID="LabelQuestions" runat="server" />
        </td>
        <td runat="server">
            <asp:Label ID="LabelLimiteQuestions" runat="server" />
        </td>
    </tr>
    <tr runat="server">
        <td runat="server">
            <asp:Label ID="Label3" runat="server" Text="Nombre d'Interviewés : " />
        </td>
        <td runat="server">
            <asp:Label ID="LabelInterviewes" runat="server" />
        </td>
        <td runat="server">
            <asp:Label ID="LabelLimiteInterviewes" runat="server" />
        </td>
    </tr>
    <tr runat="server">
        <td runat="server">
            <asp:Label ID="Label4" runat="server" Text="Nombre de Réponses : "></asp:Label>
        </td>
        <td runat="server">
            <asp:Label ID="LabelReponses" runat="server"></asp:Label>
        </td>
        <td runat="server">
            <asp:Label ID="LabelLimiteReponses" runat="server" />
        </td>
    </tr>
        <tr runat="server">
            <td runat="server"></td>
            <td runat="server"></td>
            <td runat="server">
                <asp:Label ID="LabelDateFinAbonnement" runat="server" ToolTip="Date de fin de votre abonnement"></asp:Label>
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
                ToolTip="Créez un nouveau Questionnaire ou le créer à partir d'un exemple, Ajouter des Questions à un Questionnaire existant" meta:resourcekey="ImageButton1Resource1" />
        </td>
        <td valign="top" align="right" >
            <label class="LabelStyle" title="étape 4"></label>
        </td>
        <td>
            <asp:ImageButton ID="Image4" runat="server" BorderColor="#5282D4" 
                BorderStyle="Solid" BorderWidth="1px"
                ImageUrl="~/Images/Wizard/ajouter_questions.jpg" 
                ToolTip="Ajoutez des Questions à votre Questionnaire" 
                onclick="ButtonAjouterQuestion_Click" meta:resourcekey="Image4Resource1" />
        </td>
        <td>
        </td>
        <td>
            <asp:ImageButton ID="Image8" runat="server" BorderColor="#5282D4" 
                BorderStyle="Solid" BorderWidth="1px"
            ImageUrl="~/Images/Wizard/ajouter questions enchainees.jpg" 
            ToolTip="Ajoutez plusieures Questions à votre Questionnaire" 
            onclick="ButtonAjouterQuestionEnchainee_Click" meta:resourcekey="Image8Resource1" />
        </td>
        <td>
            &nbsp;</td>
        <td>
            <asp:ImageButton ID="Image6" runat="server" BorderColor="#5282D4" 
                BorderStyle="Solid" BorderWidth="1px"
            ImageUrl="~/Images/Wizard/ajouter des questions.jpg" 
            ToolTip="Visualisez, Modifiez vos Questionnaires" 
            onclick="ButtonQuestionnaireList_Click" meta:resourcekey="Image6Resource1" />
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
                onclick="ButtonCreerQuestionnaire_Click" meta:resourcekey="ButtonCreerQuestionnaireResource1" />
        </td>
        <td>
        </td>
        <td class="TableAccueilButtonStyle">
        <asp:Button ID="ButtonAjouterQuestion" CssClass="ButtonAccueilStyle" 
            runat="server" Text="Ajoutez Questions" 
            ToolTip="Ajoutez des Questions à votre Questionnaire" 
            onclick="ButtonAjouterQuestion_Click" meta:resourcekey="ButtonAjouterQuestionResource1" />
        </td>
        <td>
        </td>
        <td class="TableAccueilButtonStyle">
            <asp:Button ID="ButtonAjouterQuestionEnchainee" CssClass="ButtonAccueilStyle" 
                runat="server" Text="Ajouter +Questions" 
                ToolTip="Ajouter plusieures Questions à votre Questionnaire" 
                onclick="ButtonAjouterQuestionEnchainee_Click" meta:resourcekey="ButtonAjouterQuestionEnchaineeResource1"/>
        </td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
        <asp:Button ID="ButtonQuestionnaireList" CssClass="ButtonAccueilStyle" 
            runat="server" Text="Visualisez Questions" 
            ToolTip="Visualisez, Modifier vos Questionnaires" 
            onclick="ButtonQuestionnaireList_Click" meta:resourcekey="ButtonQuestionnaireListResource1"/>
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
            onclick="ButtonEditerPageAccueil_Click" meta:resourcekey="Image5Resource1" />
        </td>
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            <asp:ImageButton ID="Image9" runat="server" BorderColor="#5282D4" 
                BorderStyle="Solid" BorderWidth="1px"
                ImageUrl="~/Images/Wizard/rediger email.jpg" 
                ToolTip="Rédigez l'email aux interviewés" 
                onclick="ButtonRedigerEmail_Click" meta:resourcekey="Image9Resource1" />
        </td>
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            <asp:ImageButton ID="Image10" runat="server" BorderColor="#5282D4" 
                BorderStyle="Solid" BorderWidth="1px"
            ImageUrl="~/Images/Wizard/importer.jpg" 
            ToolTip="Importez les contacts que vous allez interviewer" 
            onclick="ButtonImporter_Click" meta:resourcekey="Image10Resource1" />
        </td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            <asp:ImageButton ID="Image11" runat="server" BorderColor="#5282D4" 
                BorderStyle="Solid" BorderWidth="1px"
            ImageUrl="~/Images/Wizard/exporter.jpg" 
            ToolTip="Gérez l'envoi des emails aux interviewés" 
            onclick="ButtonEvoyerEmail_Click" meta:resourcekey="Image11Resource1" />
        </td>
   </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
        <asp:Button ID="ButtonEditerPageAccueil" CssClass="ButtonAccueilStyle"
            runat="server" Text="Editez Accueil" 
            ToolTip="Editez la page d'accueil des interviewés" 
            onclick="ButtonEditerPageAccueil_Click" meta:resourcekey="ButtonEditerPageAccueilResource1"/>
        </td>
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
        <asp:Button ID="ButtonRedigerEmail" CssClass="ButtonAccueilStyle" 
            runat="server" Text="Editez l'Email" 
            ToolTip="Rédigez l'email envoyé aux interviewés" 
            onclick="ButtonRedigerEmail_Click" meta:resourcekey="ButtonRedigerEmailResource1" />
        </td>
        <td>
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
            <asp:Button ID="ButtonImporter" CssClass="ButtonAccueilStyle" 
                runat="server" Text="Importez Interviewés" 
                ToolTip="Importez les contacts que vous allez interviewer" 
                onclick="ButtonImporter_Click" meta:resourcekey="ButtonImporterResource1"/>
        </td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
        <asp:Button ID="ButtonEvoyerEmail" CssClass="ButtonAccueilStyle" 
            runat="server" Text="Envoyez les emails" 
            ToolTip="Gérez l'envoi des emails aux interviewés" 
            onclick="ButtonEvoyerEmail_Click" meta:resourcekey="ButtonEvoyerEmailResource1"/>
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
                onclick="ButtonStatistiques_Click" meta:resourcekey="Image7Resource1" />
        </td>
   </tr>
    <tr>
        <td class="TableAccueilButtonStyle">
        <asp:Button ID="ButtonStatistiques" CssClass="ButtonAccueilStyle" 
            runat="server" Text="Dépouillez Stats" 
            ToolTip="Statistiques sur les votants" 
            onclick="ButtonStatistiques_Click" meta:resourcekey="ButtonStatistiquesResource1"/>
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
                ToolTip="Afficher le volet de contrôle de la navigation, ajouter ou supprimer des moyens de navigation" meta:resourcekey="ImageButtonExpandNavigationResource1" />                
        </td>
    </tr>
</table>

<asp:Panel ID="PanelControlNavigation" GroupingText="Navigation" runat="server" Visible="False" meta:resourcekey="PanelControlNavigationResource1">
<table border="0" cellpadding="3" cellspacing="0">
    <tr>
        <td align="center" colspan="2" height="35px">
            <label class="LabelStyle" title="Configurer l'interface graphique">Voulez-vous afficher ?</label>
        </td>
    </tr>    
    <tr visible="False" runat="server">
        <td align="right" runat="server">
            <label class="LabelStyle" title="Afficher l'aide en ligne tout au long de votre session">L'aide en ligne</label>
        </td>
        <td align="left" runat="server">
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
            OnCheckedChanged="CheckBoxAideContextuelle_CheckedChanged" meta:resourceKey="CheckBoxAideContextuelleResource1" />
        </td>
    </tr>    
    <tr>
        <td align="right">
            <label class="LabelStyle" title="Afficher le menu expert">Le menu de navigation</label>
        </td>
        <td align="left">
            <asp:CheckBox ID="CheckBoxMenuExpert" runat="server" AutoPostBack="True" 
            oncheckedchanged="CheckBoxMenuExpert_CheckedChanged" meta:resourceKey="CheckBoxMenuExpertResource1" />
        </td>
    </tr>    
    <tr>
        <td align="right">
            <label class="LabelStyle" title="Afficher le menu étape par étape">Le menu en colonne à gauche</label>
        </td>
        <td align="left">
            <asp:CheckBox ID="CheckBoxMenuColonne" runat="server" AutoPostBack="True" 
                oncheckedchanged="CheckBoxMenuColonne_CheckedChanged" meta:resourceKey="CheckBoxMenuColonneResource1"/>
        </td>
    </tr>    
    <tr>
        <td align="right">
            <label class="LabelStyle" title="Barre site map">La barre de navigation</label>
        </td>
        <td align="left">
            <asp:CheckBox ID="CheckBoxBarreNavigation" runat="server" AutoPostBack="True" 
                oncheckedchanged="CheckBoxBarreNavigation_CheckedChanged" meta:resourceKey="CheckBoxBarreNavigationResource1"/>
        </td>
    </tr>    
    <tr>
        <td align="right">
            <label class="LabelStyle" title="Bouton qui suit le déroulement de la page (Haut de page)">Les boutons toujours visibles</label>
        </td>
        <td align="left">
            <asp:CheckBox ID="CheckBoxMenuToujoursVisible" runat="server" AutoPostBack="True" 
                oncheckedchanged="CheckBoxMenuToujoursVisible_CheckedChanged" meta:resourceKey="CheckBoxMenuToujoursVisibleResource1"/>
        </td>
    </tr>    
    <tr>
        <td align="right">
            <label class="LabelStyle">Boutons toujours visibles position :</label>
        </td>
        <td align="left">
            <asp:DropDownList ID="DropDownListMenuToujoursVisiblePosition" runat="server" AutoPostBack="True" 
                OnSelectedIndexChanged="DropDownListMenuToujoursVisiblePosition_SelectedIndexChanged" meta:resourceKey="DropDownListMenuToujoursVisiblePositionResource1">
                <asp:ListItem Text="Defaut" Selected="True"  Value="None" meta:resourceKey="ListItemResource1" />
                <asp:ListItem Text="Haut Gauche" Value="HG" meta:resourceKey="ListItemResource2" />
                <asp:ListItem Text="Haut Centre" Value="HC" meta:resourceKey="ListItemResource3" />
                <asp:ListItem Text="Haut Droite"  Value="HD" meta:resourceKey="ListItemResource4" />
                <asp:ListItem Text="Milieu Gauche" Value="MG" meta:resourceKey="ListItemResource5" />
                <asp:ListItem Text="Milieu Center" Value="MC" meta:resourceKey="ListItemResource6" />
                <asp:ListItem Text="Milieu Droite"  Value="MD" meta:resourceKey="ListItemResource7" />
                <asp:ListItem Text="Bas Gauche" Value="BG" meta:resourceKey="ListItemResource8" />
                <asp:ListItem Text="Bas Centre" Value="BC" meta:resourceKey="ListItemResource9" />
                <asp:ListItem Text="Bas Droite" Value="BD" meta:resourceKey="ListItemResource10" />
            </asp:DropDownList>
        </td>
    </tr>    
    <tr>
        <td align="center" colspan="2" height="55px">
            <asp:Button ID="ButtonParDefaut" runat="server" CssClass="ButtonStyle" ToolTip="Configurer l'interface de navigation par défaut"
                Text="Par défaut" onclick="ButtonParDefaut_Click" meta:resourceKey="ButtonParDefautResource1" />
        </td>
    </tr>    
</table>
</asp:Panel>

</div>
</asp:Content>

