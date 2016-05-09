<%@ Page Language="C#" Trace="false" MasterPageFile="~/MasterPage.master" 
    MaintainScrollPositionOnPostback="false" AutoEventWireup="true" 
    CodeFile="Questionnaire.aspx.cs" Inherits="Poll_Questionnaire" Title="Questionnaire" %>
<%@ Register TagPrefix="ucwc" TagName="WebContent" Src="~/UserControl/WebContent.ascx" %>
<%@ Reference Control="~/UserControl/PopupTextBox.ascx"%>
<%@ Reference Control="~/UserControl/TextBoxDate.ascx"%>
<%@ Register TagPrefix="usr" TagName="ProgressBarre" Src="~/UserControl/ProgressBarre.ascx" %>

<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<style type="text/css">
.LabelProgressStyle
{
    color:Blue
}
</style>
<a name="HautDePage"></a>
<div class="DivPageQuestionnaireStyle">

    <!-- Aide en ligne sur le Formulaire -->
    <asp:Panel ID="PanelAide" runat="server" class="PanelAideStyle" Height="0">
    <table class="TableCollapsePanel">
        <tr>
            <td align="left">
            Choisissez un modèle de style pour votre Questionnaire dans la liste des styles prédéfinis.<br />
            Testez votre Questionnaire, entrez des Réponses qui ne seront pas comptabilisées dans les votes, validez les types de Question.<br />
            Cochez la case Tester pour visualiser les informations de programmation du Questionnaire.
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <!-- Control des styles -->
    <asp:Panel ID="PanelControlStyle" runat="server" Visible="false" >
    <table class="TableQuestionStyle" cellpadding="4" width="100%" border="0" >
        <tr>
            <td width="16px">
                <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" 
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
            <td align="left" rowspan="2" valign="top" >
                <label class="LabelStyle" title="Description du Questionnaire">Description :</label>
                <asp:Label ID="LabelQuestionnaire" ToolTip="Titre du Questionnaire" runat="server" ></asp:Label>
            </td>
            <td align="right" width="110px">
                <label class="LabelStyle" title="Choisir un modèle de Style pour le Questionnaire">Style :</label>
            </td>
            <td align="left">
                <asp:DropDownList ID="DropDownListStyle" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListStyle_SelectedIndexChanged" />
                &nbsp;
                <asp:ImageButton ID="ButtonSupprimerStyle" runat="server" 
                    ImageUrl="~/Images/Delete.gif" AlternateText="Supprimer le Style sélectionné" 
                    ToolTip="Supprimer le Style sélectionné" 
                    OnClick="ButtonSupprimerStyle_Click" />
            </td>
            <td width="240px" align="left">
                 <asp:CheckBox ID="CheckBoxModeTest" runat="server" 
                    oncheckedchanged="CheckBoxModeTest_CheckedChanged" 
                    Text="Tester" ToolTip="Afficher les informations de programmation du Questionnaire."
                    CssClass="LabelStyle" AutoPostBack="True" />
                 <asp:CheckBox ID="CheckBoxModeDebugGraphiqueTable" runat="server" 
                    oncheckedchanged="CheckBoxModeDebugGraphiqueTable_CheckedChanged" 
                    Text="Tables" ToolTip="Afficher les informations de debug graphique des tables du Questionnaire."
                    CssClass="LabelStyle" AutoPostBack="true" EnableViewState="true" />
                 <asp:CheckBox ID="CheckBoxModeDebugGraphiqueCellule" runat="server" 
                    oncheckedchanged="CheckBoxModeDebugGraphiqueCellule_CheckedChanged" 
                    Text="Cellules" ToolTip="Afficher les informations de debug graphique des cellules du Questionnaire."
                    CssClass="LabelStyle" AutoPostBack="true" />
                 <asp:CheckBox ID="CheckBoxModeDebugGraphiqueToolTip" runat="server" 
                    oncheckedchanged="CheckBoxModeDebugGraphiqueToolTip_CheckedChanged" 
                    Text="ToolTip" ToolTip="Afficher les informations ToolTip sur les Objets du Questionnaire."
                    CssClass="LabelStyle" AutoPostBack="true" />
            </td>
        </tr>
        <tr id="TrStyleWebCreation" runat="server">
            <td>
            &nbsp;
            </td>
            <td align="right">
                <label class="LabelStyle" title="Nom du nouveau Style">Nouveau Style :</label>
            </td>
            <td align="left" width="270px">
                <asp:TextBox ID="TextBoxNomNouveauStyle" runat="server" />
                &nbsp;
                <asp:Button ID="Button1" CssClass="ButtonControlStyle" runat="server" Text="Créer" 
                    ToolTip="Créer un nouveau style à  partir du style sélectionné" 
                    onclick="ButtonStyleCreerStyle_Click" 
                    UseSubmitBehavior="true" />
                &nbsp;
                <asp:ImageButton ID="ImageButtonNePasAppliquerStyle" runat="server" 
                    ImageUrl="~/Images/Delete.gif" AlternateText="Ne pas appliquer les Styles" 
                    ToolTip="Supprimer l'application des Styles" 
                    OnClick="ButtonNePasAppliquerStyle_Click" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    </asp:Panel>

    <!-- Cacher tous les Styles -->
    <asp:Panel ID="PanelStyleWeb" runat="server" Visible="false">
    
    <!-- Boutons d'édition des styles pour la Page -->
    <asp:Panel ID="PanelStyleWebTitrePage" runat="server" 
        GroupingText="La Page" Width="95%" CssClass="PanelStyleWeb" >
    <table border="0" cellpadding="3">
    <tr>
        <td>
            <asp:ImageButton ID="ImageButton9" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebPanelPageQuestionnaire_Click" 
                ToolTip="Editer le style du Panel contenant le Questionnaire"/>
            <label class="LabelSmallBlueStyle">Cadre Questionnaire</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButton8" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebPageQuestion_Click" 
                ToolTip="Editer le style de la Table contenant le Questionnaire"/>
            <label class="LabelSmallBlueStyle">Page Question</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButton10" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebTableTitre_Click" 
                ToolTip="Editer le style de la Table contenant le Titre des pages"/>
            <label class="LabelSmallBlueStyle">Table Titre Page</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButtonEditStyleWebTitrePageQuestionnaire2" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebTitrePageQuestionnaire_Click" 
                ToolTip="Editer le style du Label Titre des pages"/>
            <label class="LabelSmallBlueStyle">Titre Page</label>
        </td>
    </tr>
    </table>  
    </asp:Panel>

    <!-- Boutons d'édition des styles pour les Questions -->
    <asp:Panel ID="PanelStyleWebQuestion" runat="server" 
        GroupingText="Les Questions" Width="95%" CssClass="PanelStyleWeb" >
    <table border="0" width="100%" cellpadding="3">
    <tr>
        <td>
            <asp:ImageButton ID="ImageButtonEditStyleWebCelluleQuestion" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebCelluleQuestion_Click" 
                ToolTip="Editer le style de la Cellule qui contient la Question"/>
            <label class="LabelSmallBlueStyle">Cellule Question</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButtonEditStyleWebCadreQuestion" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebCadreQuestion_Click" 
                ToolTip="Editer le style de la Table qui contient la Question"/>
            <label class="LabelSmallBlueStyle">Cadre Question</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButtonEditStyleWebQuestion" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebTableTitreQuestionQuestion_Click" 
                ToolTip="Editer le style de la Table qui contient le Titre de la Question"
                Height="16px" />
            <label class="LabelSmallBlueStyle">Table Titre Question</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButtonEditStyleWebReponseChoixSimple" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebReponseChoixSimple_Click" 
                ToolTip="Editer le style des Réponses choix simples" 
                Width="16px" />
            <label class="LabelSmallBlueStyle">Réponses Choix Simple</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButtonEditStyleWebReponseChoixMultiple" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebReponseChoixMutiple_Click" 
                ToolTip="Editer le style des Réponses choix multiple" 
                Width="16px" />
            <label class="LabelSmallBlueStyle">Réponses Choix Mutiple</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButtonEditStyleWebTableReponseTextuelle" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebTableReponseTextuelle_Click" 
                ToolTip="Editer le style de la Table qui contient les Réponses textuelles" 
                Width="16px"/>
            <label class="LabelSmallBlueStyle">Table Réponses textuelles</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButtonEditStyleWebReponseTextuelleLabel" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebReponseTextuelleLabel_Click" 
                ToolTip="Editer le style du Label des Réponses textuelles" 
                Width="16px"/>
            <label class="LabelSmallBlueStyle">Label Réponses textuelles</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButtonEditStyleWebReponseTextuelleTextBox" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebReponseTextuelleTextBox_Click" 
                ToolTip="Editer le style du Texte des Réponses textuelles" 
                Width="16px"/>
            <label class="LabelSmallBlueStyle">Réponse textuelle</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButton11" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebTableCompteurQuestions_Click" 
                ToolTip="Editer le style de la Table du Compteur de questions"/>
            <label class="LabelSmallBlueStyle">Table Compteur</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButtonEditStyleWebCompteur" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebCompteurQuestions_Click" 
                ToolTip="Editer le style du Label Compteur de questions"/>
            <label class="LabelSmallBlueStyle">Label Compteur</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButtonEditStyleWebMessage" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebTableMessage_Click" 
                ToolTip="Editer le style de la Table qui contient le Message destiné à l'utilisateur"/>
            <label class="LabelSmallBlueStyle">Table Message à l'utilisateur</label>
        </td>
    </tr>
    </table>
    </asp:Panel>

    <!-- Boutons d'édition des styles pour les Tableaux -->
    <asp:Panel ID="Panel1" runat="server" 
        GroupingText="Les Tableaux" Width="95%" CssClass="PanelStyleWeb" >
    <table border="0" width="100%" cellpadding="3">
    <tr>
        <td>
            <asp:ImageButton ID="ImageButton7" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebTableTitreTableau_Click" 
                ToolTip="Editer le style de la Table qui contient le Titre du Tableau"/>
            <label class="LabelSmallBlueStyle">Table Titre Tableau</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButton1" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebTitreTableau_Click" 
                ToolTip="Editer le style du Titre du Tableau"/>
            <label class="LabelSmallBlueStyle">Titre Tableau</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButton2" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebCadreTableau_Click" 
                ToolTip="Editer le style de la Table qui contient le Tableaux"/>
            <label class="LabelSmallBlueStyle">Cadre Tableau</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButton6" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebCelluleQuestionTableau_Click" 
                ToolTip="Editer le style des cellules autours des questions dans les tableaux"/>
            <label class="LabelSmallBlueStyle">Cellule Question Tableau</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButton3" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebQuestionTableau_Click" 
                ToolTip="Editer le style des Questions dans les Tableaux"/>
            <label class="LabelSmallBlueStyle">Question Tableau</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButton5" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebCelluleReponseTableau_Click" 
                ToolTip="Editer le style des cellules autours des réponses dans les tableaux"/>
            <label class="LabelSmallBlueStyle">Cellule Réponse Tableau</label>
        </td>
        <td>
            <asp:ImageButton ID="ImageButton4" runat="server" 
                ImageUrl="~/Images/EditBleu.gif" 
                onclick="ImageButtonEditStyleWebReponseTableau_Click" 
                ToolTip="Editer le style des réponses dans les tableaux"/>
            <label class="LabelSmallBlueStyle">Réponse Tableau</label>
        </td>
    </tr>
    </table>
    </asp:Panel>
    
    </asp:Panel>
       
    <!-- Questionnaire cree dynamiquement -->
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td align="left">
            <ucwc:WebContent ID="WebContent1" runat="server" Section="PageQuestionnaire" /> 
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:Panel ID="PanelQuestionnaire" runat="server" Width="100%" />
        </td>
    </tr>
    <tr>
        <td align="center" style="padding:12px;">
            <asp:HyperLink ID="HyperLinkVosReponses" runat="server" />
        </td>
    </tr>
    <tr>
        <td align="center">
        <usr:ProgressBarre ID="ProgressBarre" PanelProgressBackColor="#5284D4" PanelBarSideHeight="5px" 
        LabelProgressCssClass="LabelProgressStyle" runat="server">
        </usr:ProgressBarre>
        </td>
    </tr>
    <tr>
        <td align="center">
            <UserControl:RolloverButton ID="ButtonSubmit" runat="server" Text="&#187;&#187;&#187;"/>
        </td>
    </tr>
    </table>
    
    <!-- Message de Validation pour la creation des StyleWeb -->
    <table border="0" width="100%">
    <tr>
        <td align="center">
            <asp:Label ID="LabelValidationMessage" runat="server" 
                CssClass="LabelValidationMessageStyle" 
                Text="" 
                Visible="False" />
        </td>
    </tr>
    </table> 

    <asp:Panel runat="server" ID="PanelButtonRetourQuestionnaire" Visible="false">
    <table cellpadding="2">
        <tr>
            <td height="60px" align="center">
                <UserControl:RolloverButton ID="RolloverButtonListQuestions" runat="server" Visible="false" PostBackUrl="~/Poll/Manage.aspx" ToolTip="Programmer les Questions" Text="Questions" />
                <UserControl:RolloverButton ID="ButtonTestez" runat="server" Text="Reset" OnClick="ButtonReset_Click" ToolTip="Remettre le Questionnaire au début" />
            </td>
        </tr>
    </table>
    </asp:Panel>   
</div>    
</asp:Content>
