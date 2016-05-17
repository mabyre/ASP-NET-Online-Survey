<%@ Page Language="C#" Trace="false" MasterPageFile="~/MasterPage.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="QuestionEnchainee.aspx.cs" Inherits="Wizard_QuestionEnchainee" Title="Ajouter plusieures Questions" %>
<%@ Reference Control="~/UserControl/QuestionControl.ascx" %>

<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="DivWizardQuestionEnchaineeStyle" >
    <a name="HautDePage"></a>
    <h3><asp:Label ID="LabelTitre" runat="server" Text="Enchainez vos Questions"/></h3>
    
    <!-- Aide en ligne sur le Formulaire -->
    <asp:Panel ID="PanelAide" runat="server" class="PanelAideStyle">
    <table class="TableCollapsePanel">
        <tr>
            <td>
            Choisissez ci-dessous le Questionnaire pour y ajouter des Questions.<br />
            Choisissez le nombre de Questions que vous désirez ajouter.<br />
            Vous pouvez créer une page en donnant son titre dans "Page :".<br />
            Vous pouvez créer un tableau (questions sous forme matricielle) en donnant son titre dans "Tableau :".
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

    <br />
    
    <!-- Nombre de Questions -->
    <table class="TableQuestionStyle" cellpadding="4" width="100%" border="0" >
        <tr>
            <td align="right" width="250px">
                <label class="LabelStyle" title="Entrez le nombre de Questions que désirez poser">Combien de questions :</label>
            </td>
            <td align="left">
                <asp:TextBox ID="TextBoxCombienDeQuestions" runat="server" AutoPostBack="true" OnTextChanged="TextBoxCombienDeQuestions_TextChanged" />
            </td>
            <td align="left" width="75">
                <asp:Button ID="ButtonCombienDeQuestionsOk" CssClass="ButtonControlStyle" runat="server" Text="Ok" 
                    ToolTip="Combien de questions à ajouter" Width="40px" onclick="ButtonCombienDeQuestionsOk_Click"/>
            </td>
        </tr>
        <tr>
            <td align="right" width="250px">
                <label class="LabelStyle" title="Page dans laquelle sera le tableau">Page :</label>
            </td>
            <td align="left">
                <asp:TextBox ID="TextBoxPageTableau" runat="server" />
            </td>
            <td align="left" width="75">
            </td>
        </tr>
        <tr>
            <td align="right" width="250px">
                <label class="LabelStyle" title="Titre du tableau">Tableau :</label>
            </td>
            <td align="left">
                <asp:TextBox ID="TextBoxTitreTableau" runat="server" />
            </td>
            <td align="left" width="75">
            </td>
        </tr>
    </table>
    
    <br />
    
    <!-- Creation dynamique des Questions Chainees-->
    <table cellpadding="4" width="100%" border="0">
        <tr>
            <td>
                <asp:Panel ID="PanelQuestions" runat="server" EnableViewState="true">
                </asp:Panel>
            </td>
        </tr>
    </table>

    <table border="0" width="100%">
    <tr>
        <td align="center">
            <UserControl:RolloverButton ID="ButtonAjouterModifier" runat="server" Text="Ajouter"
                OnClick="ButtonAjouterQuestion_Click" ToolTip="Ajouter cette série de Questions" />
            <UserControl:RolloverButton ID="RolloverVoir" runat="server" Text="Visualisez" 
                ToolTip="Visualisez le Questionnaire" OnClick="ButtonVoirQuestionaireOk_Click" />                
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

