<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" ValidateRequest="false" 
    CodeFile="Edit.aspx.cs" 
    Inherits="Questionnaire_Edit" 
    Title="Edition d'un Questionnaire" %>
    
<%@ Register Src="~/UserControl/PopupLabel.ascx" 
    TagName="PopupLabel" 
    TagPrefix="usrc" %>
<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<style type="text/css">
.TextBoxLienStyle
{
    font-size:10px;
    color:Blue;
}
</style>
<div id="body">
<div id="DivPageStyle">
    <h3><asp:Label ID="LabelTitre" runat="server"/></h3>
    <asp:Panel ID="PanelAdmin" runat="server" Visible="false">
    <table border="0" cellpadding="2" class="TableQuestionStyleAdministrateur" width="100%">
        <tr>
            <td class="TdLabelStyle" align="right" title="Admin : Choisir un membre différent pour copier ce Questionnaire d'un membre à l'autre">
                <strong>Pour le Membre : </strong>
            </td>
            <td class="TdTextBoxStyle" align="left">
                <UserControl:DropDownListMembre ID="DropDownListMembre" runat="server" AutoPostBack="false" />                
            </td>
            <td>
                <asp:Label ID="bourrage1" runat="server" Width="50px" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                <strong>Membre : </strong>
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:Label ID="LabelMembre" runat="server" CssClass="LabelBoxStyle" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                <strong>Date création : </strong>
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:Label ID="LabelDateCreation" runat="server" CssClass="LabelBoxStyle" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                <strong>Nom : </strong>
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:Label ID="LabelNom" runat="server" CssClass="LabelBoxStyle" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                <strong>Prénom : </strong>
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:Label ID="LabePrenom" runat="server" CssClass="LabelBoxStyle"/>
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                <strong>Société : </strong>
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:Label ID="LabelSociete" runat="server" CssClass="LabelBoxStyle" />
            </td>
        </tr>
    </table>    
    <br />    
    </asp:Panel>
    
    <asp:Panel ID="PanelQuestionnaire" runat="server">
    
    <table border="0" cellpadding="2" class="TableQuestionStyle" width="100%">
        <tr>
            <td width="16px">
                <asp:ImageButton ID="ImageButtonExpandQuestion" runat="server" 
                    ImageUrl="~/Images/expand.jpg" onclick="ImageButtonExpandQuestion_Click" 
                    ToolTip="Afficher le volet des autres propriétés de votre Questionnaire" />
            </td>
            <td align="right" width="220px">
                <label class="LabelStyle" title="Description du questionnaire">Titre du Questionnaire :</label>
            </td>
            <td align="left" class="TdTextBoxStyle">
                <asp:TextBox ID="TextBoxDescription" runat="server" CssClass="TextBoxStyle" 
                    Width="350px" />
            </td>
            <td>
                <a href="http://www.sodevlog.fr/Questionnaire.En.Ligne/page/Questionnaire-en-ligne-Aide.aspx#EditezVosQuestionnaires" title="Aide sur l'édition d'un questionnaire" target="_blank">
                <img src="../App_Themes/Sodevlog/Images/help_rouge.gif" border="0" />
                </a>
            </td>
        </tr>
        <tr id="TrVoletCodeAcces" runat="server">
            <td>&nbsp;</td>
            <td align="right">
                <label class="LabelStyle" title="Code d'accès du Questionnaire, permet d'inviter les interviewés">Code d&#39;accès :</label>
            </td>
            <td align="left" class="TdTextBoxStyle">
                <asp:Label ID="LabelCodeAcces" runat="server" CssClass="LabelBoxStyle" />
            </td>
             <td>&nbsp;</td>
        </tr>
    </table>
        
    <!-- Case a cocher Anonymat -->
    <asp:Panel ID="PanelAide1" runat="server" class="PanelAideStyle">
    <table class="TableCollapsePanel">
        <tr>
            <td>
            Choisissez de cocher cette option si vous désirez garantir l'anonymat à vos Interviewés.<br />
            <b>L'anonymat de l'interviewé est respecté</b>, ses informations personnelles ne s'affichent pas.<br />
            L'invitation d'un interviewé en utilisant un lien vers le Formulaire d'enregistrement devient automatique.<br />
            L'interviewé n'a plus besoin de s'enregistrer pour répondre à votre Questionnaire. Vous pouvez alors utiliser n'importe quel lien d'<b>Invitation par formulaire</b>
            </td>
        </tr>
    </table>
    </asp:Panel>
      
    <table border="0" cellspacing="5px" class="TableQuestionStyle" width="100%"
        title="Si vous cochez cette case, l'enregistrement de l'interviewés est automatique">
        <tr runat="server" >
            <td width="16">
                <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtenderAide1" runat="Server" 
                    AutoCollapse="false"  
                    AutoExpand="false"
                    TargetControlID="PanelAide1"
                    ExpandControlID="PanelControl1"
                    CollapseControlID="PanelControl1" 
                    Collapsed="true"
                    ImageControlID="Image1"    
                    ExpandedImage="~/App_Themes/Sodevlog/Images/help.gif"
                    CollapsedImage="~/App_Themes/Sodevlog/Images/help.gif"
                    SuppressPostBack="true"
                    SkinID="CollapsiblePanel" />  
                <asp:Panel ID="PanelControl1" runat="server" CssClass="CollapsePanelHeader"> 
                    <img src="../App_Themes/Sodevlog/Images/help.gif" border="0" alt ="Cliquez ici pour afficher l'aide"/>
                </asp:Panel>
            </td>        
            <td align="left">
                <usrc:PopupLabel ID="PopupLabelAnonymat" 
                    LabelCheckBox="Anonymat :" 
                    CssClassLabelCheckBox="LabelStyle" 
                    LabelChecboxWidth="216px"
                    LabelText="Attention, cette option est définitive" 
                    LabelWidth="250px" 
                    CssClassLabel="LabelRedStyle" 
                    runat="server" />
            </td>
        </tr>
    </table>    

    <!-- Modes d'enregistrement des reponses des interviewes -->
    <asp:Panel ID="PanelModeEnregistrement" runat="server">

        <asp:Panel ID="PanelAide2" runat="server" class="PanelAideStyle">
        <table class="TableCollapsePanel">
            <tr>
                <td>
                Choisissez le mode d'enregistrement des réponses de vos Interviewés :<br />
                <b>Valider :</b> L'interviewé doit valider ses réponses pour qu'elles soient enregistrées dans la base.<br />
                <b>Valider à la Fin :</b> L'interviewé doit répondre à toutes les questions de votre Questionnaire avant de pouvoir valider l'ensemble de ses réponses.<br />
                Si aucune de ces options n'est choisie, les réponses sont enregistrées dans la base, à chaque question.
                </td>
            </tr>
        </table>
        </asp:Panel>

        <table border="0" cellspacing="5px" class="TableQuestionStyle" width="100%" title="Modes d'enregistrement des Interviewés">
        <tr>
            <td width="16" >
                <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="Server" 
                    AutoCollapse="false"  
                    AutoExpand="false"
                    TargetControlID="PanelAide2"
                    ExpandControlID="PanelControl2"
                    CollapseControlID="PanelControl2" 
                    Collapsed="true"
                    ImageControlID="Image1"    
                    ExpandedImage="~/App_Themes/Sodevlog/Images/help.gif"
                    CollapsedImage="~/App_Themes/Sodevlog/Images/help.gif"
                    SuppressPostBack="true"
                    SkinID="CollapsiblePanel" />  
                <asp:Panel ID="PanelControl2" runat="server" CssClass="CollapsePanelHeader"> 
                    <img src="../App_Themes/Sodevlog/Images/help.gif" border="0" alt ="Cliquez ici pour afficher l'aide"/>
                </asp:Panel>
            </td>        
            <td align="right" width="216px">
                <label class="LabelStyle" 
                    title="Donner, à l'interviewé, la possibilité de valider ses réponses">
                Valider :</label>
            </td>
            <td align="left" class="TdTextBoxStyle">
                <asp:CheckBox ID="CheckBoxValider" runat="server" CssClass="TextBoxStyle" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td align="right">
                <label class="LabelStyle" 
                    title="Donner, à l'interviewé, la possibilité de valider ses réponses à la fin du questionnaire">
                Valider à la fin :</label>
            </td>
            <td align="left" class="TdTextBoxStyle">
                <asp:CheckBox ID="CheckBoxFin" runat="server" CssClass="TextBoxStyle" />
            </td>
            <td>&nbsp;</td>
        </tr>
        </table>
        
    </asp:Panel>
    
    <!-- Invitation par lien sur un formulaire d'enregistrement -->
    <asp:Panel ID="PanelInviationFormulaireEnregistrement" runat="server">

        <asp:Panel ID="PanelAide3" runat="server" class="PanelAideStyle">
        <table class="TableCollapsePanel">
            <tr>
                <td>
                Choisissez cette option si vous désirez <b>utiliser un lien</b> vers un des Formulaires d'enregistrement à votre Questionnaire.<br />
                L'Interviewé pourra alors cliquer sur ce lien pour répondre à votre questionnaire.<br />
                <br />
                Un fois votre questionnaire créé, vous pourrez utiliser ces liens qui auront été créés.<br />
                <br />
                <b>Formulaires d'enregistrement de l'Interviewé</b><br />
                L'interviewé doit s'enregistrer pour répondre à votre questionnaire.
                <ul>
                    <li>
                        <b>Formulaire d'Enregistrement Complet Email et Téléphone :</b> Civilité, Nom, Prénom, Société, E-mail, Téléphone<br />
                    </li>
                    <li>
                        <b>Formulaire Enregistrement Complet Email :</b> Civilité, Nom, Prénom, Société, E-mail<br />
                    </li>
                    <li>
                        <b>Formulaire Enregistrement Email :</b> E-mail seulement<br />
                    </li>
                    <li>
                        <b>Formulaire Enregistrement Téléphone :</b> Téléphone seulement<br />
                    </li>
                </ul>
                Si de plus, vous cochez la case <b>Anonymat :</b> vos interviewés n'auront pas à s'enregistrer pour répondre à votre questionnaire, l'enregistrement d'un interviewé se fera alors automatiquement. Quel que soit le lien que vous utilisez.
                </td>
            </tr>
        </table>
        </asp:Panel>
    
        <table border="0" cellspacing="5px" class="TableQuestionStyle" width="100%">
        <tr>
            <td width="16" rowspan="2" valign="top" >
                <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" 
                    AutoCollapse="false"  
                    AutoExpand="false"
                    TargetControlID="PanelAide3"
                    ExpandControlID="PanelControl3"
                    CollapseControlID="PanelControl3" 
                    Collapsed="true"
                    ImageControlID="Image1"    
                    ExpandedImage="~/App_Themes/Sodevlog/Images/help.gif"
                    CollapsedImage="~/App_Themes/Sodevlog/Images/help.gif"
                    SuppressPostBack="true"
                    SkinID="CollapsiblePanel" />  
                <asp:Panel ID="PanelControl3" runat="server" CssClass="CollapsePanelHeader"> 
                    <img src="../App_Themes/Sodevlog/Images/help.gif" border="0" alt ="Cliquez ici pour afficher l'aide"/>
                </asp:Panel>
            </td>        
            <td align="right" width="216px">
                <label class="LabelStyle" title="Donner la possibilité d'interviewer des contacts par un lien vers l'un des Formulaires d'enregistrement">
                Invitation par formulaire :</label>
            </td>
            <td align="left">
                <asp:CheckBox ID="CheckBoxAnonyme" AutoPostBack="true" runat="server" CssClass="TextBoxStyle" 
                    Width="215px" OnCheckedChanged="CheckBoxAnonyme_CheckedChanged" />
            </td>
            <td rowspan="2">
            &nbsp;
            </td>
        </tr>
        <tr>
            <td align="left" class="TdLabelInvitationStyle" colspan="2">
                <table runat="server" id="TableLienEnregistrement" border="0" cellpadding="0" cellspacing="20" style="padding-left:40px;">
                <tr>
                    <td>
                        <label style="color:Blue;font-weight:bold;height:20px">Formulaires d'enregistrement de l'Interviewé</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <!-- Complet -->
                        <span class="SpanHyperLinkStyle">
                        <asp:HyperLink ID="HyperLinkInvitationEnregistrementComplet" runat="server" 
                            CssClass="HyperLinkStyle" Text="Invitation Formulaire Enregistrement Complet Email et Téléphone" 
                            ToolTip="Invitez des contacts à s'enregistrer pour ce questionnaire, voir l'invitation enregistrement" />
                        </span>
                        <br />
                        <asp:TextBox ID="TextBoxInvitationEnregistrementComplet" 
                        CssClass="TextBoxLienStyle" 
                        ReadOnly="true" 
                        runat="server"  
                        Width="480px" 
                        TextMode="MultiLine" 
                        Rows="3"
                        ToolTip="Copier ce code Html pour créer un lien vers votre questionnaire dans votre page web." />
                    </td>
                </tr>
                <tr>
                    <td>
                        <!-- Complet Email (pas de telephone)-->
                        <span class="SpanHyperLinkStyle">
                        <asp:HyperLink ID="HyperLinkInvitationEnregistrementCompletEmail" runat="server" 
                            CssClass="HyperLinkStyle" Text="Invitation Formulaire Enregistrement Complet Email" 
                            ToolTip="Invitez des contacts à s'enregistrer pour ce questionnaire, voir l'invitation enregistrement" />
                        </span>
                        <br />
                        <asp:TextBox ID="TextBoxInvitationEnregistrementCompletEmail" 
                        CssClass="TextBoxLienStyle" 
                        ReadOnly="true" 
                        runat="server"  
                        Width="480px" 
                        TextMode="MultiLine" 
                        Rows="3"
                        ToolTip="Copier ce code Html pour créer un lien vers votre questionnaire dans votre page web." />
                    </td>
                </tr>
                <tr>
                    <td>
                        <!-- Email -->
                        <span class="SpanHyperLinkStyle">
                        <asp:HyperLink ID="HyperLinkInvitationEnregistrementEmail" runat="server" 
                            CssClass="HyperLinkStyle" Text="Invitation Formulaire Enregistrement Email" 
                            ToolTip="Invitez des contacts par email pour ce questionnaire, voir l'invitation" />
                        </span>
                        <br />
                        <asp:TextBox ID="TextBoxInvitationEnregistrementEmail" 
                        CssClass="TextBoxLienStyle" 
                        ReadOnly="true" 
                        runat="server"  
                        Width="480px" 
                        TextMode="MultiLine" 
                        Rows="3"
                        ToolTip="Copier ce code Html pour créer un lien vers votre questionnaire dans votre page web." />               
                    </td>
                </tr>
                <tr>
                    <td>
                        <!-- Telephone -->
                        <span class="SpanHyperLinkStyle">
                        <asp:HyperLink ID="HyperLinkInvitationEnregistrementTelephone" runat="server" 
                            CssClass="HyperLinkStyle" Text="Invitation Formulaire Enregistrement Telephone" 
                            ToolTip="Invitez des contacts par téléphone pour ce questionnaire, voir l'invitation" />
                        </span>
                        <br />
                        <asp:TextBox ID="TextBoxInvitationEnregistrementTelephone" 
                        CssClass="TextBoxLienStyle" 
                        ReadOnly="true" 
                        runat="server"  
                        Width="480px" 
                        TextMode="MultiLine" 
                        Rows="3"
                        ToolTip="Copier ce code Html pour créer un lien vers votre questionnaire dans votre page web." />               
                    </td>
                </tr>
                </table>
            </td>
        </tr>
        </table>

    </asp:Panel> <!-- PanelInviationFormulaire -->

    
    <!-- Invitation par lien sur un formulaire d'authentification  -->
    <asp:Panel ID="PanelInviationFormulaireAuthentification" runat="server">
    
        <asp:Panel ID="PanelAide4" runat="server" class="PanelAideStyle">
        <table class="TableCollapsePanel">
            <tr>
                <td>
                <b>Formulaires d'authentification de l'Interviewé</b><br />
                L'interviewés est enregistré dans la base des contacts à interviewer, il doit s'authentifier pour répondre à votre questionnaire.<br />
                <ul>
                    <li>
                        <b>Invitation Formulaire d'authentification Email et Code d'accès :</b> l'interviewé s'authentifie par son email et son code d'accès.<br />
                        Vous délivré ces informations à vos interviewés en plaçant dans l'email que vous envoyez, les méta-mots <b>%%ADRESSE_EMAIL%%</b>, <b>%%CODE_ACCES%%</b> et <b>%%LOG%%</b>.<br />
                        <br />
                    </li>
                    <li>
                        <b>Invitation Formulaire d'authentification Téléphone et Code d'accès :</b> l'enquêteur authentifie l'interviewé au moment de recueillir ses réponses par téléphone.<br />
                    </li>
                </ul>   
                </td>
            </tr>
        </table>
        </asp:Panel>

        <table border="0" cellspacing="5px" class="TableQuestionStyle" width="100%">
        <tr>
            <td width="16" rowspan="2" valign="top" >
                <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="Server" 
                    AutoCollapse="false"  
                    AutoExpand="false"
                    TargetControlID="PanelAide4"
                    ExpandControlID="PanelControl4"
                    CollapseControlID="PanelControl4" 
                    Collapsed="true"
                    ImageControlID="Image1"    
                    ExpandedImage="~/App_Themes/Sodevlog/Images/help.gif"
                    CollapsedImage="~/App_Themes/Sodevlog/Images/help.gif"
                    SuppressPostBack="true"
                    SkinID="CollapsiblePanel" />  
                <asp:Panel ID="PanelControl4" runat="server" CssClass="CollapsePanelHeader"> 
                    <img src="../App_Themes/Sodevlog/Images/help.gif" border="0" alt ="Cliquez ici pour afficher l'aide"/></asp:Panel>
            </td>        
            <td align="right" width="240px">
                <label class="LabelStyle" title="Utiliser les liens vers l'un des Formulaires d'authentification">
                Formulaires d'authentifaction :</label>
                &nbsp;
                <asp:ImageButton ID="ImageButtonLinkAuthentification" runat="server" 
                    ImageUrl="~/Images/expand.jpg" onclick="ImageButtonLinkAuthentification_Click" 
                    ToolTip="Afficher le volet des liens vers les formulaires d'authentification" />
            </td>
            <td rowspan="2">
            &nbsp;
            </td>
        </tr>
        <tr>
            <td align="left" class="TdLabelInvitationStyle" colspan="2">
                <table runat="server" id="TableTableLienAuthentification" border="0" cellpadding="0" cellspacing="20" style="padding-left:40px;" visible="false">
                <tr>
                    <td>
                        <label style="color:Blue;font-weight:bold;height:20px">Formulaires d'authentification de l'Interviewé</label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <!-- Adresse Email et Code d'Acces -->
                        <span class="SpanHyperLinkStyle">
                        <asp:HyperLink ID="HyperLinkInvitationEmailCodeAcces" runat="server" 
                            CssClass="HyperLinkStyle" Text="Invitation Formulaire d'authentification Email et Code d'accès" 
                            ToolTip="Invitez des contacts à s'authentifier pour ce questionnaire, format codes d'accès et adresse email, voir la page d'invitation" />
                        </span>
                        <br />
                        <asp:TextBox ID="TextBoxInvitationEmailCodeAcces" 
                        CssClass="TextBoxLienStyle" 
                        ReadOnly="true" 
                        runat="server"  
                        Width="480px" 
                        TextMode="MultiLine" 
                        Rows="3"
                        ToolTip="Copier ce code Html pour créer un lien vers votre questionnaire dans votre page web." /> 
                    </td>
                </tr>
                <tr>
                    <td>
                        <!-- Téléphone et Code d'Acces -->
                        <span class="SpanHyperLinkStyle">
                        <asp:HyperLink ID="HyperLinkInvitationTelephoneCodeAcces" runat="server" 
                            CssClass="HyperLinkStyle" Text="Invitation Formulaire d'authentification Téléphone et Code d'accès" 
                            ToolTip="Invitez des contacts à s'authentifier pour ce questionnaire, format codes d'accès et adresse email, voir la page d'invitation" />
                        </span>
                        <br />
                        <asp:TextBox ID="TextBoxInvitationTelehponeCodeAcces" 
                        CssClass="TextBoxLienStyle" 
                        ReadOnly="true" 
                        runat="server"  
                        Width="480px" 
                        TextMode="MultiLine" 
                        Rows="3"
                        ToolTip="Copier ce code Html pour créer un lien vers votre questionnaire dans votre page web." /> 
                    </td>
                </tr>
                </table>
            </td>
        </tr>
        </table>
        
    </asp:Panel> <!-- PanelInviationFormulaire -->
        
        
    <!-- Publication -->
    <asp:Panel ID="PanelPublication" runat="server">
    <br />
    <table border="0" cellspacing="5px" class="TableQuestionStyle" width="100%">
        <tr>
            <td align="right" width="16px" rowspan="2">
            &nbsp;
            </td>
            <td align="right" width="216px" title="Publier les résultats du Questionnaire en utilisant un lien web.">
                <label class="LabelStyle">Publier :</label>
            </td>
            <td align="left">
                <asp:CheckBox ID="CheckBoxPublierQuestionnaire" AutoPostBack="true" runat="server" Width="215px" OnCheckedChanged="CheckBoxPublierQuestionnaire_CheckedChanged" />
            </td>
            <td rowspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td align="left" class="TdLabelInvitationStyle" colspan="2">
                <table runat="server" id="TableLienPublication" border="0" cellpadding="0" cellspacing="20" style="padding-left:40px;">
                <tr>
                    <td>
                        <span class="SpanHyperLinkStyle">
                        <asp:HyperLink ID="HyperLinkPublication" runat="server" 
                            CssClass="HyperLinkStyle" Text="Lien Formulaire de Publication" 
                            ToolTip="Voir la page de publication des résultats, des statistiques du Questionnaire" />
                        </span>
                        <br />
                        <asp:TextBox ID="TextBoxPublication"
                        CssClass="TextBoxLienStyle" 
                        ReadOnly="true" 
                        runat="server"  
                        Width="480px" 
                        TextMode="MultiLine" 
                        Rows="3"
                        ToolTip="Copier ce code Html pour créer un lien vers votre les statistiques de votre questionnaire dans votre page web." />                               
                    </td>
                </tr>
                </table>
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <!-- Cloture -->
    <asp:Panel ID="PanelCloture" runat="server">
    <br />
    <table border="0" cellspacing="5px" class="TableQuestionStyle" width="100%">
        <tr>
            <td align="right" width="236px" title="Indiquer aux interviewés que le Questionnaire est clôturé, qu'ils ne peuvent plus y répondre">
                <asp:Label ID="LabelBloque" runat="server" class="LabelStyle" 
                    Text="Clôturé :" />
            </td>
            <td align="left" class="TdTextBoxStyle">
                <asp:CheckBox ID="CheckBoxBloque" runat="server" CssClass="TextBoxStyle" />
            </td>
            <td>&nbsp;</td>
        </tr>
    </table>
    </asp:Panel>
    
    <!-- Compteur de question -->
    <asp:Panel ID="PanelCompteurQuestion" runat="server">
    <br />
    <table border="0" cellspacing="5px" class="TableQuestionStyle" width="100%">
        <tr>
            <td align="right" width="236px" title="Afficher le compteur de Questions pour chaque question">
                <asp:Label ID="Label1" runat="server" class="LabelStyle" 
                    Text="Afficher le compteur :" />
            </td>
            <td align="left" class="TdTextBoxStyle">
                <asp:CheckBox ID="CheckBoxAfficherCompteur" runat="server" CssClass="TextBoxStyle" />
            </td>
            <td>&nbsp;</td>
        </tr>
    </table>
    </asp:Panel>

    </asp:Panel><!-- PanelQuestionnaire -->

    <asp:Panel ID="PanelQuestionnaireBloque" runat="server" Visible="false">
    <table style="border:solid 1px" cellpadding="0" cellspacing="0" width="80%">
        <tr>
            <td height="180px">
                <label>Questionnaire bloqué pendant la campagne d'interview.</label>
            </td>
        </tr>
    </table>
    </asp:Panel>

    <table border="0" cellpadding="10" cellspacing="0" width="100%">
        <tr>
            <td align="left">
                <asp:ImageButton ID="ImageButtonExpandListQuestionnaire" runat="server" 
                    ImageUrl="~/Images/expand.jpg" onclick="ImageButtonExpandListQuestionnaire_Click" 
                    ToolTip="Afficher la liste de vos Questionnaires" />
            </td>
        </tr>
        <tr id="TrVoletListQuestionnaire" runat="server">
            <td align="center">
                <asp:HiddenField ID="HiddenFieldMembreGUID" runat="server" />
                <asp:SqlDataSource ID="SqlDataSourceMembreQuestionnaire" Runat="server" ConnectionString="<%$ ConnectionStrings:QuestionnaireDB %>" />                   
                <asp:DataList ID="DataListMembreQuestionnaire" runat="server" DataSourceID="SqlDataSourceMembreQuestionnaire">
                    <ItemStyle CssClass="LabelBlueStyle" />
                    <ItemTemplate>
                        <asp:Label ID="onsenfou1" runat="server" Text='<%# Eval("Description") + ":" + Eval("CodeAcces") %>' ></asp:Label>
                    </ItemTemplate>
                </asp:DataList>
            </td>
        </tr>
    </table>
        
    <table cellpadding="2" width="100%" border="0">
        <tr>
            <td width="30%">
            </td>
            <td class="TdControlButtonStyle">
                <UserControl:RolloverButton ID="ButtonSave" runat="server" OnClick="ButtonSave_Click"/>                
            </td>
            <td>
                <UserControl:RolloverButton ID="ButtonCopier" runat="server" Text="Copier" ToolTip="Copier un Questionnaire" OnClick="ButtonCopier_Click"/>                
            </td>
            <td>
                <UserControl:RolloverButton ID="ButtonSupprimer" runat="server" Text="Supprimer" ToolTip="Supprimer le Questionnaire et les objets associés" OnClick="ButtonSupprimer_Click"/>                
            </td>
            <td>
                <UserControl:RolloverButton ID="ButtonAjouterQuestion" runat="server" Text="Ajouter" ToolTip="Ajoutez des Questions à vos questionnaires" OnClick="ButtonAjouterQuestion_Click"/>                
            </td>
            <td width="30%">
            </td>
        </tr>
    </table>

    <table style="border:none" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td height="30px" align="center">
                <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
            </td>
        </tr>
    </table>
    
</div>
</div>
</asp:Content>