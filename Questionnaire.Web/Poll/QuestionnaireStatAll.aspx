<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" 
    Trace="false"
    ValidateRequest="false" 
    MaintainScrollPositionOnPostback="true" 
    AutoEventWireup="true" 
    CodeFile="QuestionnaireStatAll.aspx.cs" 
    Inherits="Poll_QuestionnaireStatAll" 
    Title="Statistiques" %>

<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<a name="HautDePage"></a>
<div class="DivPageStatistiquesStyle">
    <h3>Statistiques par Questionnaire</h3>

    <table border="0" cellpadding="10" height="30">
        <tr>
            <td>
                <label class="LabelListUserStyle">Interviewés : </label>
                <asp:Label ID="LabelNombreContacts" runat="server" ></asp:Label>
            </td>
            <td>
                <label class="LabelListUserStyle">Réponses : </label>
                <asp:Label ID="LabelVotes" runat="server" ></asp:Label>
            </td>
        </tr>
    </table> 
    
    <!-- Aide en ligne sur le Formulaire -->
    <asp:Panel ID="PanelAide" runat="server" class="PanelAideStyle" Height="0">
    <table class="TableCollapsePanel">
        <tr>
            <td align="left">
            Cliquez sur une Question et vous obtenez, en bas du formulaire, toutes les réponses à cette Question.<br />
            Cliquez sur une Réponse et vous obtenez la sous-population des interviewés qui ont répondu cette Réponse.<br />
            Vous pouvez cliquer sur une autre Réponse pour affiner la sous-population.<br />
            Cliquez sur le bouton <b>Afficher</b> qui apparait sous <b>Qui a répondu :</b> pour recalculer les statistiques de cette sous-population.<br />
            Cliquez sur le bouton <b>Effacer</b> qui apparait pour reprendre la totalité des interviewés.<br />
            Cliquez sur le bouton <b>Afficher</b> sous <b>Afficher les votes :</b> pour afficher, en bas du formulaire, toutes les réponses à toutes les questions soit pour tous les interviewés soit pour la sous-population sélectionnée.<br />
            Cliquez sur le lien "Réponse" pour visualiser, en bas du formulaire, toutes les réponses.<br />
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <asp:Panel ID="PanelAfficherVotantEnModePrint" runat="server" Visible="false">
    </asp:Panel>

    <asp:Panel ID="PanelControlSats" runat="server">
    <table class="TableQuestionHautStyle" cellpadding="5" width="100%" border="0">
    <tr>
        <td align="left" class="TdCelluleIcone" >
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
        <td align="left" class="TdCelluleIcone">
            <asp:ImageButton ID="ImageButtonPrint" runat="server" 
                ImageUrl="~/Images/print.png"
                onclick="ImageButtonPrint_Click" 
                ToolTip="Formulaire d'impression"/>
        </td>
        <td align="left" class="TdCelluleIcone">
            <asp:ImageButton ID="ImageButtonExcel" runat="server" 
                ImageUrl="~/Images/excel.png"
                onclick="ImageButtonExcel_Click" 
                ToolTip="Fichier Excel"/>
        </td>
        <td>&nbsp;</td>
    </tr>
    </table>
    
    <table class="TableQuestionBasStyle" cellpadding="5" width="100%" border="0">
    <tr>
        <td align="right">
            <label class="LabelStyle">Questionnaires :</label> 
        </td>
        <td align="left">
            <usr:DropDownListQuestionnaires ID="DropDownListQuestionnaires" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListQuestionnaires_SelectedIndexChanged" />
        </td>
    </tr>
    <tr>
        <td align="right" title="Cliquez sur une réponse pour afficher la liste de ceux qui ont répondu">
            <label class="LabelStyle">Qui a répondu :</label> 
        </td>
        <td align="left">
            <asp:Panel ID="PanelReponses" runat="server" Visible="false">
            </asp:Panel>
            <asp:ListBox ID="ListBoxQui" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListBoxQui_SelectedIndexChanged"></asp:ListBox>
            <asp:Label ID="LabelListBoxQuiCount" runat="server" CssClass="LabelStyle"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td class="TdLabelInvitationStyle" align="left" >
            <asp:Panel ID="PanelSousPopulation" runat="server" Visible="false" Height="23px" >
                <asp:Button ID="ButtonAfficherSousPopulation" CssClass="ButtonStyle" runat="server" Text="Afficher" OnClick="ButtonAfficherSousPopulation_Click" ToolTip="Afficher les satistiques de cette sous-population" />
                <asp:Button ID="ButtonEffacerQuiARepondu" CssClass="ButtonStyle" runat="server" Text="Effacer" OnClick="ButtonEffacerQuiARepondu_Click" ToolTip="Effacer la sous-population et afficher les statistiques complets" />
            </asp:Panel>
        </td>
    </tr>    
    <tr>
        <td align="right" valign="top" title="Choisir un votant pour afficher ses votes">
            <label class="LabelStyle">Afficher les votes :</label>
        </td>
        <td align="left">
            <asp:DropDownList ID="DropDownListQui" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListQui_SelectedIndexChanged"></asp:DropDownList>&nbsp;
        </td>
    </tr>
    <tr>
        <td align="right">
            &nbsp;
        </td>
        <td align="left">
            <asp:Button ID="ButtonAfficherTousLesVotes" CssClass="ButtonStyle" runat="server" OnClick="ButtonAfficherTousLesVotes_Click" ToolTip="Afficher tous les votes en bas du formulaire" Text="Afficher" />
            <asp:Button ID="ButtonEffacerTousLesVotes" CssClass="ButtonStyle" runat="server" OnClick="ButtonEffacerTousLesVotes_Click" ToolTip="Effacer tous les votes en bas du formulaire" Text="Effacer" />
            <span class="SpanHyperLinkStyle"  >
                <a href="#ReponsesInterviewes" class="HyperLinkQuestionEnCoursStyle" title="Voir les réponses en bas du formulaire">Réponses</a>
            </span>
        </td>
    </tr>
    <tr>
        <td align="center" valign="top" title="Afficher ou non les réponses textuelles" colspan="3">
            <label title="Afficher ou non les réponses textuelles">Réponses textuelles :</label> 
            <asp:CheckBox ID="CheckBoxAfficherReponseTextuelle" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBoxAfficherReponseTextuelle_CheckedChanged" />
            &nbsp;&nbsp;
            <label title="Afficher la moyenne pondérée par le score (poid) accordé aux réponses">Moyenne pondérée :</label>
            <asp:CheckBox ID="CheckBoxAfficherMoyennePonderee" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBoxAfficherMoyennePonderee_CheckedChanged" />
            &nbsp;&nbsp;
            <label title="Afficher ou non les dates des votes">Dates des votes :</label>
            <asp:CheckBox ID="CheckBoxAfficherDateVote" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBoxAfficherDateVote_CheckedChanged" />
        </td>
    </tr>
    <tr>
        <td align="right" valign="top" title="">
        </td>
        <td align="left">
        </td>
    </tr>
    <tr>
        <td align="right" valign="top" title="">
        </td>
        <td align="left">
        </td>
    </tr>
    </table>
    </asp:Panel>

    <br />

    <table style="border:solid 1px" cellpadding="10px" cellspacing="0" width="100%">
    <tr>
        <td>
            <table border="0" width="100%">
                <tr>
                    <td valign="top" align="left">
                        <UserControl:QuestionnaireStatAllControl ID="QuestionnaireControlStatAll" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    </table>
    
    <table border="0" cellpadding="5px" cellspacing="0" width="100%">
    <tr>
        <td height="30px">
            <a name="ReponsesInterviewes">Réponses des interviewés</a>
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:Panel ID="PanelReponsesEnBas" runat="server" Visible="true">
            </asp:Panel>
        </td>
    </tr>
    </table>
    
    <asp:Panel ID="PanelBoutonControl" runat="server">
        <table border="0" cellpadding="5px" cellspacing="0" width="100%">
        <tr>
            <td height="20px">
                <span class="SpanHyperLinkStyle">
                <a href="#HautDePage" class="HyperLinkStyle">Haut</a>
                </span>
            </td>
        </tr>
        <tr id="TrBoutonRetour" runat="server" visible="false">
            <td height="20px">
                <span class="SpanHyperLinkStyle">
                <a href="QuestionnaireStatAll.aspx" class="HyperLinkStyle">Retour</a>
                </span>
            </td>
        </tr>
        </table>
    </asp:Panel>
</div>    
</asp:Content>
