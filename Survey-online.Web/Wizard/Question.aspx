<%@ Page Language="C#" Trace="false" MasterPageFile="~/MasterPage.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="Question.aspx.cs" Inherits="Wizard_Question" ValidateRequest="false" Title="Ajouter une Question" %>
<%@ Reference Control="~/UserControl/PopupTextBox.ascx"%>
<%@ Reference Control="~/UserControl/TextBoxDate.ascx"%>

<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<a name="HautDePage"></a>
<div class="DivWizardQuestionStyle">

    <h3><asp:Label ID="LabelTitre" runat="server"/></h3>
    
    <!-- Aide en ligne sur le Formulaire -->
    <asp:Panel ID="PanelAide" runat="server" class="PanelAideStyle" Height="0">
    <table class="TableCollapsePanel">
        <tr>
            <td>
            Vous avez la possibilité :<br />
            <ul class="UlStyle">
                <li>d'ajouter de nouvelles questions à votre Questionnaire.</li>
                <li>de modifier les Questions existantes d'un Questionnaire.</li>
            </ul>
            Choisissez ci-dessous votre Questionnaire.<br />
            En cliquant sur "Visualisez" en bas du formulaire, vous pouvez visualiser le Questionnaire en mode "listing" et effectuer d'autres modifications.<br />
            En bas du formulaire vous visualisez la Question telle qu'elle sera posée à l'interviewé avec les styles appliqués.<br />
            En cliquant sur le "chevron" vous découvrez d'autres options.
            </td>
        </tr>
    </table>
    </asp:Panel>
    
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
            <td align="right" width="180px">
                <label class="LabelStyle" title="Choisir un Questionnaire pour y ajouter des Questions">Questionnaires :</label>
            </td>
            <td align="left">
                <usr:DropDownListQuestionnaires ID="DropDownListQuestionnaire" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListQuestionnaire_SelectedIndexChanged" />
            </td>
            <td align="left" width="75">
                <asp:Button ID="ButtonVoirQuestionaireOk" CssClass="ButtonStyle" runat="server" Text="Visualisez" 
                    ToolTip="Visualisez le Questionnaire" onclick="ButtonVoirQuestionaireOk_Click" 
                    UseSubmitBehavior="true" />
            </td>
        </tr>
    </table>

    <!-- Aide en ligne sur la Question -->
    <asp:Panel ID="PanelAide1" runat="server" class="PanelAideStyle" Height="0">
    <table class="TableCollapsePanel">
        <tr>
            <td>
            Modifier le libellé de votre Question.<br />
            Choisissez un type de votre Question :
            <ul class="UlStyle">
                <li>choix simple, l'interviewé ne peut faire qu'un choix.</li>
                <li>choix multiple, l'interviewé peut faire plusieurs choix, entre Choix Min et Choix Max s'ils sont définis.</li>
            </ul>
            Si la case "Démo" est cochée, vous verrez un exemple de question en fonction du "Type de la question" que vous choisissez.<br />
            </td>
        </tr>
    </table>
    </asp:Panel>

    <table class="TableQuestionStyle" cellpadding="4" width="100%" border="0">
        <tr>
            <td width="16px">
                <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="Server" 
                    AutoCollapse="false"  
                    AutoExpand="false"
                    TargetControlID="PanelAide1"
                    ExpandControlID="PanelControl1"
                    CollapseControlID="PanelControl1" 
                    Collapsed="true"
                    ImageControlID=""    
                    ExpandedImage="~/App_Themes/Sodevlog/Images/help.gif"
                    CollapsedImage="~/App_Themes/Sodevlog/Images/help.gif"
                    SuppressPostBack="true"
                    SkinID="CollapsiblePanel" />  
                <asp:Panel ID="PanelControl1" runat="server" CssClass="CollapsePanelHeader"> 
                    <img src="../App_Themes/Sodevlog/Images/help.gif" border="0" alt ="Cliquez ici pour afficher l'aide"/>
                </asp:Panel>
            </td>
            <td align="right" width="180">
                <label class="LabelStyle" title="Posez ici votre question">
                <label class="LabelStyle">Type de la Question :</label> </label>
            </td>
            <td align="left">
                <asp:DropDownList ID="DropDownListTypeQuestionReponse" runat="server" AutoPostBack="true" 
                    OnSelectedIndexChanged="DropDownListTypeQuestionReponse_SelectedIndexChanged">
                </asp:DropDownList>
                </td>
            <td align="left" >
                 <asp:CheckBox ID="CheckBoxModeDemonstration" runat="server" 
                    oncheckedchanged="CheckBoxModeDemonstration_CheckedChanged" 
                    Text="Démo" ToolTip="En mode démonstration la question et les réponses se remplissent automatiquement"
                    CssClass="LabelStyle" AutoPostBack="True" />
            </td>
        </tr>
        <tr>
            <td width="16px">
                <asp:ImageButton ID="ImageButtonExpandQuestion" runat="server" 
                    ImageUrl="~/Images/expand.jpg" 
                    onclick="ImageButtonExpandQuestion_Click" 
                    ToolTip="Afficher le volet des autres options pour la Question" 
                    Width="13px"/>                
            </td>
            <td align="right">
                <label class="LabelStyle" title="Posez ici votre question">Votre Question :</label>
            </td>
            <td align="left">
                <asp:TextBox ID="TextBoxQuestion" runat="server" Width="330px" 
                    OnTextChanged="TextBoxQuestion_TextChanged"/>
            </td>
            <td align="left">
                <asp:Button ID="ButtonQuestionOk" CssClass="ButtonControlStyle" runat="server" Text="Ok" 
                    ToolTip="Modifier la Question" onclick="ButtonQuestionOk_Click" />
            </td>
        </tr>
        <tr id="TrOptionQuestion01" runat="server" visible="false">
            <td align="right" colspan="2">
                <label class="LabelStyle" title="Nombre de choix minimum pour une question à choix multiple">Choix Min :</label>
            </td>
            <td align="left">
                <asp:TextBox ID="TextBoxChoixMultipleMin" runat="server" Width="45px" /> 
            </td>
            <td align="left">
                &nbsp;</td>
        </tr>
        <tr id="TrOptionQuestion02" runat="server" visible="false">
            <td align="right" colspan="2">
                <label class="LabelStyle" title="Nombre de choix maximum pour une question à choix multiple">Choix Max :</label>
            </td>
            <td align="left">
                <asp:TextBox ID="TextBoxChoixMultipleMax" runat="server" Width="45px" EnableViewState="true" /> 
            </td>
            <td align="left">
                &nbsp;
            </td>
        </tr>        
        <tr id="TrOptionQuestion1" runat="server" visible="false">
            <td>
            </td>
            <td align="right">
                <label class="LabelStyle" title="Obliger l'intervieweur à répondre.">Obligatoire :</label>
            </td>
            <td align="left">
               <asp:CheckBox ID="CheckBoxQuestionObligatoire" runat="server" 
                    oncheckedchanged="CheckBoxQuestionObligatoire_CheckedChanged" Text="" 
                    CssClass="LabelStyle" AutoPostBack="True" />
            </td>
            <td align="left">
                &nbsp;</td>
        </tr>
        <tr id="TrOptionQuestion2" runat="server" visible="false">
            <td>
            </td>
            <td align="right">
                <label class="LabelStyle" title="Question de fin pour la programmation du Questionnaire.">Fin :</label>
            </td>
            <td align="left">
               <asp:CheckBox ID="CheckBoxQuestionFin" runat="server" 
                    oncheckedchanged="CheckBoxQuestionFin_CheckedChanged" Text="" 
                    CssClass="LabelStyle" AutoPostBack="True" />
            </td>
            <td align="left">
                &nbsp;</td>
        </tr>
        <tr id="TrOptionQuestion3" runat="server" visible="false">
            <td>
                &nbsp;</td>
            <td align="right">
                <label class="LabelStyle" 
                    title="Alignement de toute la Question, à gauche, à droite, au centre">Alignement :</label>
            </td>
            <td align="left">
                <asp:DropDownList ID="DropDownListAlignementQuestion" runat="server" 
                    onselectedindexchanged="DropDownListAlignementQuestion_SelectedIndexChanged" 
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
            <td align="left">
                &nbsp;</td>
        </tr>
    </table>

    <!-- Aide en ligne sur la Reponse -->
    <asp:Panel ID="Panel1" runat="server" class="PanelAideStyle" Height="0">
    <table class="TableCollapsePanel">
        <tr>
            <td>
            Ajoutez des Réponses à la Question.<br />
            <ul class="UlStyle">
                <li>Vous pouvez ajouter plusieurs Réponses en les séparant par des point-virgules.</li>
                <li>Vous pouvez choisir des Réponses toutes faites dans la liste "Choix de Réponses".</li>
            </ul>
            </td>
        </tr>
    </table>
    </asp:Panel>

    <table class="TableQuestionStyle" cellpadding="4" width="100%" border="0">
        <tr id="TrVoletReponseChoix" runat="server" visible="true">
            <td colspan="3">
                <!-- Table Reponse choix -->
                <table border="0" width="100%">
                    <tr>
                        <td width="16" style="width: 0" valign="top">
                            <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="Server" 
                                AutoCollapse="false"  
                                AutoExpand="false"
                                TargetControlID="Panel1"
                                ExpandControlID="Panel2"
                                CollapseControlID="Panel2" 
                                Collapsed="true"
                                ImageControlID=""    
                                ExpandedImage="~/App_Themes/Sodevlog/Images/help.gif"
                                CollapsedImage="~/App_Themes/Sodevlog/Images/help.gif"
                                SuppressPostBack="true"
                                SkinID="CollapsiblePanel" />  
                            <asp:Panel ID="Panel2" runat="server" CssClass="CollapsePanelHeader"> 
                                <img src="../App_Themes/Sodevlog/Images/help.gif" border="0" alt ="Cliquez ici pour afficher l'aide"/>
                            </asp:Panel>
                        </td>
                        <td align="right" width="190px" valign="top">
                            <label class="LabelStyle" title="Entrez plusieures réponses de type choix séparées par des points virgules">
                            Réponse choix :</label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBoxReponse" runat="server" Width="330px" Rows="5" TextMode="MultiLine"
                                ontextchanged="TextBoxReponse_TextChanged"/>
                            </td>
                        <td align="left" width="75">
                            <asp:Button ID="ButtonReponseOk" CssClass="ButtonControlStyle" runat="server" Text="Ok" 
                                ToolTip="Ajouter la Réponse" onclick="ButtonReponseOk_Click" 
                                UseSubmitBehavior="true" />
                        </td>
                    </tr>
                    <tr>
                        <td width="16px">
                            <asp:ImageButton ID="ImageButtonExpandReponse" runat="server" 
                                ImageUrl="~/Images/expand.jpg" 
                                onclick="ImageButtonExpandReponse_Click" 
                                ToolTip="Afficher le volet des autres options pour la Réponse" />                
                        </td>
                        <td align="right">
                            <!-- label supprime -->                            
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="DropDownListChoixReponse" runat="server" AutoPostBack="true" 
                                OnSelectedIndexChanged="DropDownListChoixReponse_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td align="left">
                            <asp:ImageButton ID="ImageButtonReponseChoixEffacer" ImageUrl="~/Images/Delete.gif" runat="server"
                                ToolTip="Effacer les Réponses choix" 
                                OnClick="ImageButtonReponseChoixEffacer_Click" />
                        </td>
                    </tr>
                    <tr id="TrVoletReponse1" runat="server" visible="false">
                        <td align="right" colspan="2">
                            <label class="LabelStyle">Direction :</label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="DropDownListDirection" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="DropDownListDirection_SelectedIndexChanged">
                                <asp:ListItem>Vertical</asp:ListItem>
                                <asp:ListItem>Horizontal</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="left">
                            &nbsp;</td>
                    </tr>
                    <tr id="TrVoletReponse2" runat="server" visible="false">
                        <td align="right" colspan="2">
                            <label class="LabelStyle" 
                                title="Alignement des Réponses, à gauche, à droite, au centre">Alignement :</label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="DropDownListAlignementReponse" runat="server" 
                                onselectedindexchanged="DropDownListAlignementReponse_SelectedIndexChanged" 
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td align="left">
                            &nbsp;</td>
                    </tr>
                    <tr id="TrVoletReponse3" runat="server" visible="false">
                        <td align="right" colspan="2">
                            <label class="LabelStyle">Texte à gauche :</label>
                        </td>
                        <td align="left">
                           <asp:CheckBox ID="CheckBoxTextAGauche" runat="server" 
                                oncheckedchanged="CheckBoxTextAGauche_CheckedChanged" Text="" 
                                AutoPostBack="True" CssClass="LabelStyle" />
                        </td>
                        <td align="left">
                            &nbsp;</td>

                </tr>
                </table>    
            </td>
        </tr>

        <tr id="TrReponseTextuelle" runat="server" visible="false">
            <td colspan="3">
                <!-- Table Reponse textuelle -->
                <table border="0" width="100%" class="TableReponseTextuelleStyle" cellpadding="4" >
                    <tr>
                        <td width="16px">
                            <asp:ImageButton ID="ImageButtonExpandReponseTextuelle" runat="server" 
                                ImageUrl="~/Images/expand.jpg" 
                                onclick="ImageButtonExpandReponseTextuelle_Click" 
                                ToolTip="Afficher le volet des autres options pour la Réponse textuelle" />                
                        </td>
                        <td align="right" width="190px" >
                            <asp:Label CssClass="LabelStyle" ID="LabelReponseOuverte" runat="server" 
                                ToolTip="Entrez le libellé de la réponse ou bien laissez le libellé vide"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBoxReponseTextuelle" runat="server" 
                                ontextchanged="TextBoxReponseTextuelle_TextChanged" Width="330px"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:Button ID="ButtonReponseTextuelleOk" CssClass="ButtonControlStyle" runat="server" Text="Ok" 
                                ToolTip="Ajouter la Réponse" onclick="ButtonReponseTextuelleOk_Click" 
                                UseSubmitBehavior="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td align="right">
                            &nbsp;</td>
                        <td align="left">
                            &nbsp;</td>
                        <td align="left">
                            <asp:ImageButton ID="ImageButtonReponseTextuelleEffacer" runat="server"
                                ImageUrl="~/Images/Delete.gif" 
                                ToolTip="Effacer les Réponses" 
                                OnClick="ImageButtonReponseTextuelleEffacer_Click" />
                         </td>
                    </tr>
                    <tr id="TrVoletReponse4" runat="server" visible="false">
                        <td align="right" colspan="2">
                            <label class="LabelStyle" title="Largeur en pixels">Largeur :</label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBoxReponseTextuelleLargeur" runat="server" Width="45px" /> 
                        </td>
                        <td align="left">
                            &nbsp;</td>
                    </tr>
                    <tr id="TrVoletReponse5" runat="server" visible="false">
                        <td align="right" colspan="2">
                            <label class="LabelStyle" title="Hauteur en nombre de lignes">Hauteur :</label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="TextBoxReponseTextuelleHauteur" runat="server" Width="45px" EnableViewState="true" /> 
                        </td>
                        <td align="left">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="TrVoletReponse6" runat="server" visible="false">
                        <td align="right" colspan="2" >
                            <label class="LabelStyle" title="Oblige l'interviewé à donner une réponse">Obligatoire :</label>
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="CheckBoxReponseTextuelleObligatoire" runat="server"
                            OnCheckedChanged="CheckBoxQuestionTextuelleObligatoire_CheckedChanged" AutoPostBack="True"
                             />
                        </td>
                        <td align="left">
                            &nbsp;</td>
                    </tr>
                </table>
           </td>
        </tr>

        <tr id="TrBoutonEffacerReponses" runat="server" visible="false">
            <td width="100%" align="center">
                <asp:Button ID="ButtonReponseToutEffacer" CssClass="ButtonControlStyle" 
                    runat="server" Text="Effacer" 
                    ToolTip="Effacer toutes les Réponses" onclick="ButtonReponseEffacer_Click" />
            </td>
        </tr>     
        
    </table>

    <!-- Aide en ligne sur le Message a l'utilisateur -->
    <asp:Panel ID="Panel3" runat="server" class="PanelAideStyle" Height="0">
    <table class="TableCollapsePanel">
        <tr>
            <td>
            Le message à l'interviewé est un texte supplémentaire que vous ajoutez à la Question afin de renseigner l'interviewé.
            Vous le positionnez en haut ou en bas de la Question.
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <!-- Message a l'utilisateur -->
    <table class="TableQuestionStyle" cellpadding="4" width="100%">
        <tr>
            <td align="right" valign="top" width="16">
                <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="Server" 
                    AutoCollapse="false"  
                    AutoExpand="false"
                    TargetControlID="Panel3"
                    ExpandControlID="Panel4"
                    CollapseControlID="Panel4" 
                    Collapsed="true"
                    ImageControlID=""    
                    ExpandedImage="~/App_Themes/Sodevlog/Images/help.gif"
                    CollapsedImage="~/App_Themes/Sodevlog/Images/help.gif"
                    SuppressPostBack="true"
                    SkinID="CollapsiblePanel" />  
                <asp:Panel ID="Panel4" runat="server" CssClass="CollapsePanelHeader"> 
                    <img src="../App_Themes/Sodevlog/Images/help.gif" border="0" alt ="Cliquez ici pour afficher l'aide"/>
                </asp:Panel>
            </td>
            <td align="right" valign="top" width="180">
                <label class="LabelStyle">Message à l'interviewé :</label>
            </td>
            <td align="left">
                    <asp:TextBox ID="TextBoxMessageUtilisateur" runat="server" Width="330px" 
                        Rows="3" 
                        TextMode="MultiLine" 
                        ontextchanged="ButtonMessageUtilisateurOk_Click"/>
            </td>
            <td width="75" align="left">
                <table border="0" cellpadding="3" cellspacing="0">
                <tr>
                    <td valign="top">
                        <asp:Button ID="ButtonMessageUtilisateurOk" runat="server" 
                            CssClass="ButtonControlStyle" onclick="ButtonMessageUtilisateurOk_Click" Text="Ok" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ImageButton ID="ButtonMessageUtilisateurEffacer" runat="server" 
                            ImageUrl="~/Images/Delete.gif"
                            ToolTip="Effacer le message destiné à l'utilisateur"
                            OnClick="ImageButtonMessageUtilisateurEffacer_Click" />

                    </td>
                </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="right" valign="top" width="16">
                &nbsp;</td>
            <td align="right" valign="top" width="180">
                <label class="LabelStyle">Message en Haut :</label>
            </td>
            <td align="left">
                <asp:CheckBox ID="CheckBoxMessageEnHaut" runat="server" AutoPostBack="True" 
                    oncheckedchanged="CheckBoxMessageEnHaut_CheckedChanged" />
            </td>
        </tr>
    </table>

    <!-- Ici on cree dynamiquement la Question -->
    <table border="0" width="100%" cellpadding="3">
    <tr>
        <td width="15%">&nbsp;</td>
        <td>
            <table border="0" cellpadding="30" cellspacing="0">
                <tr>
                    <td>
                        <asp:Panel ID="PanelQuestion" runat="server" EnableViewState="False" ToolTip="Visualisez la question" />
                    </td>
                </tr>
            </table>
        </td>
        <td align="center">
            <table border="0" cellpadding="5" cellspacing="0" >
                <tr>
                    <td>
                        <asp:TextBox ID="TextBoxRangQuestion" CssClass="TextBoxRangQuestionStyle" runat="server" ToolTip="Modifier le rang de la Question" OnTextChanged="TextBoxRangQuestion_TextChanged" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    </table>
    
    <!-- Boutons Question suivante et Question precedente -->
    <asp:Panel ID="PanelBoutonSuivantePrecedente" runat="server" Visible="false">
    <table border="0" width="100%">
    <tr>
        <td align="center">
            <asp:Button ID="ButtonPrecedente" CssClass="ButtonStyle" runat="server" Text="<<<" 
                ToolTip="Question Précedente" onclick="ButtonPrecedente_Click" />
            <asp:Button ID="ButtonSuivante" CssClass="ButtonStyle" runat="server" Text=">>>" 
                ToolTip="Question Suivante" onclick="ButtonSuivante_Click" />
        </td>
    </tr>
    <tr>
    <td height="30px">
    </td>
    </tr>
    </table>
    </asp:Panel> 

    <table border="0" width="100%">
    <tr>
        <td align="center">
            <UserControl:RolloverButton ID="ButtonModifier" runat="server" 
                OnClick="ButtonModifierQuestion_Click" Visible="false" Text="Modifier"
                ToolTip="Si vous avez modifié les Réponses de la Question, cliquez pour Valider, les votes éventuels seront supprimés." />                
            <UserControl:RolloverButton ID="ButtonAjouter" runat="server" 
                OnClick="ButtonAjouterQuestion_Click" Visible="false" Text="Ajouter"
                ToolTip="Ajouter la Question" />                
            <UserControl:RolloverButton ID="RolloverSupprimer" runat="server" Text="Supprimer" 
                ToolTip="Supprimer la Question du Questionnaire" onclick="ButtonSupprimerQuestion_Click" />
            <UserControl:RolloverButton ID="RolloverVoir" runat="server" Text="Visualisez" 
                ToolTip="Visualisez le Questionnaire" onclick="ButtonVoirQuestionaireOk_Click" />                
        </td>
    </tr>
    <tr>
        <td align="center">
            <asp:Label ID="LabelValidationMessage" CssClass="LabelValidationMessageStyle" runat="server" Text="" Visible="False" />
        </td>
    </tr>
    </table> 
    
</div>       
</asp:Content>

