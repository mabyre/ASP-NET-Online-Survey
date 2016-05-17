<%@ Page Language="C#" Trace="false" MasterPageFile="~/MasterPage.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="Manage.aspx.cs" Inherits="Score_Manage" Title="Administrer les Scores" %>
<%@ Reference Control="~/UserControl/QuestionControl.ascx" %>

<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="DivWizardQuestionEnchaineeStyle" >
    <a name="HautDePage"></a>
    <h3><asp:Label ID="LabelTitre" runat="server" Text="Gérez les scores du Questionnaire"/></h3>
    
    <!-- Aide en ligne sur le Formulaire -->
    <asp:Panel ID="PanelAide" runat="server" class="PanelAideStyle">
    <table class="TableCollapsePanel">
        <tr>
            <td>
            Ajouter des plages de Scores à votre Questionnaire.<br />
            Vous pouvez ajouter plusieurs plages à la fois en modifiant "Combien de scores à ajouter".<br />
            Vous attribuez un score pour chaque réponse dans "Visualisez, Modifiez" vos questionnaires.<br />
            "Score minimum :" désigne le score minimum pour que le "Texte :" soit délivré à l'interviewé.<br />
            "Score maximum :" désigne le score maximum pour que le "Texte :" soit délivré à l'interviewé.<br />
            "Texte :" c'est le message délivré à l'interviewé si son score est entre minimum et maximum.<br />
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <!-- Choix du Questionnaire -->
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
            <td align="right">
                <label class="LabelStyle" title="Choisir un Questionnaire pour y ajouter des Scores">Questionnaires :</label>
            </td>
            <td align="left">
                <usr:DropDownListQuestionnaires ID="DropDownListQuestionnaire" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListQuestionnaire_SelectedIndexChanged" />
            </td>
            <td align="left" width="75">
                <!-- Supprimer les Scores -->
            </td>
        </tr>
    </table>

    <br />
    
    <!-- Nombre de Plages de Score -->
    <table class="TableQuestionStyle" cellpadding="4" width="100%" border="0" >
        <tr>
            <td align="right" width="250px">
                <label class="LabelStyle" title="Entrez le nombre de plages de Scores pour ce Questionnaire">Combien de scores à ajouter :</label>
            </td>
            <td align="left">
                <asp:TextBox ID="TextBoxCombienDeScores" Width="45px" runat="server" AutoPostBack="true" 
                    OnTextChanged="TextBoxCombienDeScores_TextChanged" />
            </td>
            <td align="left" width="75">
                <asp:Button ID="ButtonCombienDeScoresOk" CssClass="ButtonControlStyle" runat="server" Text="Ok" 
                    ToolTip="Combien de scores à ajouter" Width="40px" onclick="ButtonCombienDeScoresOk_Click"/>
            </td>
        </tr>
    </table>
    
    <br />
    
    <!-- Creation dynamique des Scores-->
    <table cellpadding="4" width="100%" border="0">
        <tr>
            <td>
                <asp:Panel ID="PanelScores" runat="server" EnableViewState="true">
                </asp:Panel>
            </td>
        </tr>
    </table>

    <table border="0" width="100%">
    <tr>
        <td align="center">
            <UserControl:RolloverButton ID="ButtonCreer" runat="server" Text="Ajouter"
                OnClick="ButtonAjouterScore_Click" ToolTip="Créer ces scores au Questionnaire" />
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

