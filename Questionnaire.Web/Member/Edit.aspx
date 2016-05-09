<%@ Page Language="C#" 
    MasterPageFile="~/MasterPage.master" 
    AutoEventWireup="true" 
    CodeFile="Edit.aspx.cs" 
    Inherits="Page_MemberEdit" 
    Title="Edition d'un membre" 
    MaintainScrollPositionOnPostback="true"%>

<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>    
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="body">
<div id="DivPageStyle">
    <h3>Edition d'un membre</h3>
    
    <!-- Nom d'utilisateur Mot de passe Modification -->
    <div class="DivTableStyle">
    <table border="0" cellpadding="3" cellspacing="3">
        <tr>
            <td class="TdLabelStyle" align="right">
                <label title="Non modifiable">Nom d'utilisateur :</label>
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxUserName" runat="server" CssClass="TextBoxRegisterStyle" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                <label title="Votre mot de passe actuel">Mot de Passe :</label> 
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxMotDePasse" runat="server" CssClass="TextBoxRegisterStyle" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                <label title="Entrez un nouveau mot de passe">Nouveau mot de Passe :</label> 
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxNouveauMotDePasse" runat="server" CssClass="TextBoxRegisterStyle" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
            </td>
            <td align="left">
                <asp:Button ID="ButtonChangePassWord" runat="server" CssClass="ButtonStyle"
                    Text="Modifier" ToolTip="Modifier le mot de passe" 
                    OnClick="ButtonChangePassWord_Click"/>
            </td>
        </tr>
    </table>
    </div>
   
    <!-- Informations utilisateur -->
    <br />
    <div class="DivTableStyle">       
    <table border="0" cellpadding="3" cellspacing="3">
        <tr>
            <td class="TdLabelStyle" align="right">
            </td>
            <td align="left">
                <label style="font-weight:bold">Vos informations personnelles</label>
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Nom : 
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxNom" runat="server" CssClass="TextBoxRegisterStyle" />
            </td>
            <td class="TdRequiredFieldValidatorStyle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="TextBoxNom"
                    ErrorMessage="Entrez votre Nom" 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Prénom : 
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxPrenom" runat="server" CssClass="TextBoxRegisterStyle" />
            </td>
            <td class="TdRequiredFieldValidatorStyle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="TextBoxPrenom"
                    ErrorMessage="Entrez votre Prénom" 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right" valign="top">
                Adresse : 
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxAdresse" runat="server" TextMode="MultiLine" Rows="3" CssClass="TextBoxRegisterStyle" />
            </td>
            <td class="TdRequiredFieldValidatorStyle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="TextBoxAdresse"
                    ErrorMessage="Entrez votre Adresse" 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Téléphone : 
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxTelephone" runat="server" CssClass="TextBoxRegisterStyle" />
            </td>
            <td class="TdRequiredFieldValidatorStyle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="TextBoxAdresse"
                    ErrorMessage="Entrez votre numéro de Téléphone" 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Société : 
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxSociete" runat="server" CssClass="TextBoxRegisterStyle" />
            </td>
            <td class="TdRequiredFieldValidatorStyle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ControlToValidate="TextBoxSociete"
                    ErrorMessage="Entrez le nom de votre Société" 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                E-mail :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxEmail" runat="server" CssClass="TextBoxRegisterStyle" />
            </td>
            <td class="TdRequiredFieldValidatorStyle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                    ControlToValidate="TextBoxEmail"
                    ErrorMessage="Merci d'entrer votre adresse email." 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="TextBoxEmail" 
                    ErrorMessage="Merci d'entrer une adresse email valide."
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                    ValidationGroup="reg">*</asp:RegularExpressionValidator>
            </td>
        </tr>
    </table>
    </div>

    <!-- Aide en ligne sur la Gestion de l'abonnement -->
    <asp:Panel ID="PanelAide" runat="server" class="PanelAideStyle">
    <table class="TableCollapsePanel">
        <tr>
            <td>
            Pour gérer votre abonnement vous pouvez aller sur le site du Questionnaire en ligne, afin d'acheter des objets, des Questionnaires, des Questions, des Interviewés (ou Contacts), des Réponses :<br />
            <a href="http://www.sodevlog.fr/questionnaire.en.ligne/" target="_blank">Questionnaire En Ligne</a><br />
            Pour supprimer des objets de votre abonnement, contactez le responsable de votre Compte d'utilisateur ou écrivez à support@sodevlog.com<br />
            </td>
        </tr>
    </table>
    </asp:Panel>

    <!-- Gestion de l'abonnement utilisateur -->
    <div class="DivTableStyle">
    <table border="0" cellpadding="3" cellspacing="3">
        <tr>
            <td valign="top">
                <ajaxToolkit:CollapsiblePanelExtender ID="cpe1" runat="Server" 
                    AutoCollapse="false"  
                    AutoExpand="false"
                    TargetControlID="PanelAide"
                    ExpandControlID="PanelControl"
                    CollapseControlID="PanelControl" 
                    Collapsed="true"
                    ImageControlID="Image1"    
                    ExpandedImage="~/App_Themes/Sodevlog/Images/help.gif"
                    CollapsedImage="~/App_Themes/Sodevlog/Images/help.gif"
                    SuppressPostBack="true"
                    SkinID="CollapsiblePanel" />  
                <asp:Panel ID="PanelControl" runat="server" CssClass="CollapsePanelHeader"> 
                    <img src="../App_Themes/Sodevlog/Images/help.gif" border="0" alt ="Cliquez ici pour afficher l'aide"/>
                </asp:Panel>
            </td>
            <td class="TdLabelStyle" align="left">
                <asp:Label ID="LabelTitreAbonnement" runat="server" Text="Gérez de votre abonnement" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                <label>Nombre de Questionnaires :</label>
            </td>
            <td align="left">
                <asp:TextBox ID="TextBoxAbonneLimiteQuestionnaires" runat="server" CssClass="TextBoxRegisterDisableStyle" Enabled="false" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                <label>Nombres de Questions :</label>
            </td>
            <td align="left">
                <asp:TextBox ID="TextBoxAbonneLimiteQuestions" runat="server" CssClass="TextBoxRegisterDisableStyle" Enabled="false" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                <label>Nombres d'Interviewés :</label>
            </td>
            <td align="left">
                <asp:TextBox ID="TextBoxAbonneLimiteInterviewes" runat="server" CssClass="TextBoxRegisterDisableStyle" Enabled="false" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                <label>Nombres de Réponse :</label>
            </td>
            <td align="left">
                <asp:TextBox ID="TextBoxAbonneLimiteReponses" runat="server" CssClass="TextBoxRegisterDisableStyle" Enabled="false" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                <label title="Date de fin de votre abonnement">Date de fin d'abonnement :</label>
            </td>
            <td align="left">
                <asp:TextBox ID="TextBoxDateFin" runat="server" CssClass="TextBoxRegisterDisableStyle" Enabled="false" />
            </td>
        </tr>
        <tr id="TrBoutonAbonne" runat="server" visible="false">
            <td class="TdLabelStyle" align="right">
            </td>
            <td align="left">
                <asp:Button ID="ButtonAbonner" runat="server" CssClass="ButtonStyle"
                    Text="Abonner" ToolTip="Abonner l'utilisateur" 
                    OnClick="ButtonAbonner_Click"/>
            </td>
        </tr>
    </table>
    </div>

    <asp:Panel ID="PanelMembreLimitePourAdmin" runat="server" Visible="false" 
        GroupingText="Objets du Membre : " 
        CssClass="PanelMemberEditPourAdminStyle" >
    <table class="TableMembreEditLimitationStyle" border="0" cellpadding="5" >
        <tr>
            <td class="TdLabelStyle" align="right">
                Nombre de Questionnaires :
            </td>        
            <td align="left">
                <asp:Label ID="LabelLimiteQuestionnairesPourAdmin" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Nombre de Questions :
            </td>        
            <td align="left">
                <asp:Label ID="LabelLimiteQuestionsPourAdmin" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Nombre d'Interviewés : 
            </td>        
            <td align="left">
                <asp:Label ID="LabelLimiteInterviewesPourAdmin" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Nombre de Réponses : 
            </td>        
            <td align="left">
                <asp:Label ID="LabelLimiteReponsePourAdmin" runat="server" />
            </td>
        </tr>
    </table>
    </asp:Panel>

    <asp:Panel ID="PanelMembreLimite" runat="server" Visible="true" 
        GroupingText="Vos Objets" 
        CssClass="PanelMemberEditStyle" >
    <table class="TableMembreEditLimitationStyle" border="0" cellpadding="5" >
        <tr>
            <td class="TdLabelStyle" align="right">
                Nombre de Questionnaires :
            </td>        
            <td align="left">
                <asp:Label ID="LabelLimiteQuestionnaires" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Nombre de Questions :
            </td>        
            <td align="left">
                <asp:Label ID="LabelLimiteQuestions" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Nombre d'Interviewés : 
            </td>        
            <td align="left">
                <asp:Label ID="LabelLimiteInterviewes" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Nombre de Réponses : 
            </td>        
            <td align="left">
                <asp:Label ID="LabelRepondant" runat="server" />
            </td>
        </tr>
    </table>
    </asp:Panel>

    <asp:Panel ID="PanelLimiteAbonne" runat="server" Visible="false" 
        GroupingText="Limites du compte 'Abonné'" 
        CssClass="PanelMemberEditPourLimiteApplicationStyle" >
    <table class="TableMembreEditLimitationStyle" border="0" cellpadding="5" >
        <tr>
            <td class="TdLabelStyle" align="right">
                Nombre de Questionnaires :
            </td>        
            <td align="left">
                <asp:Label ID="LabelLimiteCompteClientQuestionnaires" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Nombre de Questions :
            </td>        
            <td align="left">
                <asp:Label ID="LabelLimiteCompteClientQuestions" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Nombre d'Interviewés : 
            </td>        
            <td align="left">
                <asp:Label ID="LabelLimiteCompteClientInterviewes" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Nombre de Réponses : 
            </td>        
            <td align="left">
                <asp:Label ID="LabelLimiteCompteClientReponses" runat="server" />
            </td>
        </tr>
    </table>
    </asp:Panel>

    <asp:Panel ID="PanelDebloquerClient" CssClass="PanelMemberEditStyle" runat="server" 
        Visible="false" 
        GroupingText="Rôles Membre" 
        Width="100%">
    <table border="0" cellpadding="3" cellspacing="0">
        <tr>
            <td class="TdLabelStyle" align="right">
                Client :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:CheckBox ID="CheckBoxDebloquerClient" runat="server" CssClass="TextBoxStyle" AutoPostBack="true" OnCheckedChanged="CheckBoxDebloquerClient_CheckedChanged" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Administrateur :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:CheckBox ID="CheckBoxAdministrateur" runat="server" CssClass="TextBoxStyle" AutoPostBack="true" OnCheckedChanged="CheckBoxAdministrateur_CheckedChanged" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Est vérouillé :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:CheckBox ID="CheckBoxClientVerrouille" runat="server" CssClass="TextBoxStyle" AutoPostBack="true" OnCheckedChanged="CheckBoxClientVerrouille_CheckedChanged" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Approuvé :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:CheckBox ID="CheckBoxUserIsApproved" runat="server" CssClass="TextBoxStyle" AutoPostBack="true" OnCheckedChanged="CheckBoxUserIsApproved_CheckedChanged" ToolTip="Un utilisateur non approuvé ne peut plus se connecter" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Membre découverte :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:CheckBox ID="CheckBoxMembreDecouverte" runat="server" CssClass="TextBoxStyle" AutoPostBack="true" OnCheckedChanged="CheckBoxMembreDecouverte_CheckedChanged" ToolTip="Utilisateur limité par les limites fixées par l'administrateur" />
            </td>
        </tr>
    </table>    
    </asp:Panel>

    <asp:Panel ID="PanelMembreLimitation" runat="server" Visible="false" 
        CssClass="PanelMemberEditStyle" 
        GroupingText="Rôles Membre" >
    <table border="0" cellpadding="3" cellspacing="0">
        <tr>
            <td class="TdLabelStyle" align="right">
                Nombres de Questionnaires :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxMembreLimitationQuestionnaires" runat="server" CssClass="TextBoxStyle" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Nombres de Questions :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxLimitationQuestions" runat="server" CssClass="TextBoxStyle" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Nombres d'Interviewés :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxLimitationInterviewes" runat="server" CssClass="TextBoxStyle" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Nombres de Réponses :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxLimitationReponses" runat="server" CssClass="TextBoxStyle" />
            </td>
        </tr>
    </table>    
    </asp:Panel>

    <asp:Panel ID="PanelInfosMembre" runat="server" Visible="false" 
        GroupingText="Infos Membre" 
        CssClass="PanelMemberEditStyle" >
    <table border="0" cellpadding="5" cellspacing="0">
    <tr>
        <td class="TdLabelStyle" align="right">
            Is Approved :
        </td>
        <td align="left">
            <asp:Label ID="LabelIsApproved" runat="server" CssClass="LabelBlueStyle" />
        </td>
    </tr>
    <tr>
        <td class="TdLabelStyle" align="right">
            Is Locked Out :
        </td>
        <td align="left">
            <asp:Label ID="LabelIsLockedOut" runat="server" CssClass="LabelBlueStyle" />
        </td>
    </tr>
    <tr>
        <td class="TdLabelStyle" align="right">
            Is Online :
        </td>
        <td align="left">
            <asp:Label ID="LabelOnline" runat="server" CssClass="LabelNormalStyle" />
        </td>
    </tr>
    <tr>
        <td class="TdLabelStyle" align="right">
            Creation Date :
        </td>
        <td align="left">
            <asp:Label ID="LabelCreationDate" runat="server" CssClass="LabelNormalStyle" />
        </td>
    </tr>
    <tr>
        <td class="TdLabelStyle" align="right">
            Last Login Date :
        </td>
        <td align="left">
            <asp:Label ID="LabelLastLoginDate" runat="server" CssClass="LabelNormalStyle" />
        </td>
    </tr>
    <tr>
        <td class="TdLabelStyle" align="right">
            Last Lockout Date :
        </td>
        <td align="left">
            <asp:Label ID="LastLockoutDate" runat="server" CssClass="LabelNormalStyle" />
        </td>
    </tr>
    <tr>
        <td class="TdLabelStyle" align="right">
            Last Activity Date :
        </td>
        <td align="left">
            <asp:Label ID="LabelActivityDate" runat="server" CssClass="LabelNormalStyle" />
        </td>
    </tr>
    <tr>
        <td class="TdLabelStyle" align="right">
            Last Password Changed Date :
        </td>
        <td align="left">
            <asp:Label ID="LabelLastPasswordChangedDate" runat="server" CssClass="LabelNormalStyle" />
        </td>
    </tr>
    </table>
    </asp:Panel>
    
    <asp:Panel ID="PanelInfosClient" runat="server"
        GroupingText="Infos Membre" 
        CssClass="PanelMemberEditStyle" >
    <table border="0" cellpadding="5" cellspacing="0">
    <tr>
        <td class="TdLabelStyle" align="right">
            Date de création :
        </td>
        <td align="left">
            <asp:Label ID="LabelDateCreation" runat="server" CssClass="LabelNormalStyle" />
        </td>
    </tr>
    <tr>
        <td class="TdLabelStyle" align="right">
            Dernière connexion :
        </td>
        <td align="left">
            <asp:Label ID="LabelDerniereConnexion" runat="server" CssClass="LabelNormalStyle" />
        </td>
    </tr>
    <tr>
        <td class="TdLabelStyle" align="right">
            Changement de mot de passe :
        </td>
        <td align="left">
            <asp:Label ID="LabelChangementMotPasse" runat="server" CssClass="LabelNormalStyle" />
        </td>
    </tr>
    </table>
    </asp:Panel>

    <table border="0" cellpadding="2" align="left" width="100%">
        <tr>
            <td height="80" align="right">
                <UserControl:RolloverButton ID="ButtonSave" runat="server" 
                    Text="Sauver" 
                    ToolTip="Sauver les modifications de votre compte d'utilisateur"
                    OnClick="ButtonSave_Click"
                    ValidationGroup="reg" />                
            </td>
            <td id="TdButtonSupprimer" runat="server" width="10%">
                <UserControl:RolloverButton ID="ButtonSupprimer" runat="server" 
                    Text="Supprimer" 
                    ToolTip="Visualiser les objets associés à ce Membre avant suppression" 
                    OnClick="ButtonSupprimer_Click"/>                
            </td>
            <td align="left">
                <UserControl:RolloverButton ID="ButtonCancel" runat="server" 
                    Text="Retour" 
                    OnClick="ButtonCancel_Click"/>                
            </td>
        </tr>
    </table>
    
    <!-- Bug sur ce formulaire les br sont la pour afficher correctement le message -->
    
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <table border="0" cellpadding="2" align="left" width="100%">
        <tr>
            <td height="30">
                <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
            </td>
        </tr>
        <tr>
            <td class="TdValidationSummaryStyle" colspan="2">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="reg"/>
            </td>
        </tr>
    </table>
    
</div>
</div>
</asp:Content>