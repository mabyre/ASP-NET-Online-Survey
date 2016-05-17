<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" Trace="false" AutoEventWireup="true" CodeFile="Questionnaire.aspx.cs" Inherits="Wizard_Questionnaire" Title="Créer un questionnaire" %>
<%@ Register TagPrefix="usrc" TagName="QuestionnaireExempleControl" Src="~/UserControl/QuestionnaireExempleControl.ascx" %>

<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<a name="HautDePage"></a>
<div id="DivPageStyle">

    <h3>Créer un questionnaire, Ajouter des Questions</h3>

    <!-- Aide en ligne sur le Formulaire -->
    <asp:Panel ID="PanelAide" runat="server" class="PanelAideStyle">
    <table class="TableCollapsePanel">
        <tr>
            <td>
            Créez un nouveau Questionnaire en lui donnant un Titre, une Description.<br />
            Choisissez les options de base pour ce nouveau Questionnaire.<br />
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <table class="TableQuestionnaireStyle" width="100%" border="0" cellpadding="3">
        <tr>
            <td>
                <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtenderAide1" runat="Server" 
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
            <td align="left" width="100%">
                <label class="LabelStyle">Créer un nouveau Questionnaire</label>               
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2">
                <table class="ClassDivPageStyle" cellpadding="15px">
                <tr>
                    <td>
                        <UserControl:RolloverButton ID="ButtonCreerQuestionnaire" runat="server" 
                            Text="Nouveau" ToolTip="Créez un nouveau Questionnaire" 
                            OnClick="ButtonCreerQuestionnaire_Click" />
                    </td>
                </tr>
                </table>
            </td>
        </tr>
    </table>

    <asp:Panel ID="PanelAide1" runat="server" class="PanelAideStyle">
    <table class="TableCollapsePanel">
        <tr>
            <td>
            Choisissez un Questionnaire existant.<br />
            En noir apparaissent les questionnaires d'exemples créés par l'administrateur dans l'application.<br />
            En bleu apparaissent vos questionnaires, ceux que vous avez créés et que vous pouvez réutiliser.<br />
            <br />
            Le questionnaire que vous choisissez de créer ou d'ajouter s'affiche sous forme Listing.<br />
            Les boutons vous permettant de "Créer" ou "Ajouter" apparaissent.<br />
            <br />
            Cliquez sur "Créer" pour dupliquer ce Questionnaire existant dans la liste de vos Questionnaires.<br />
            Cliquez sur "Ajouter" pour ajouter, les Questions de ce Questionnaire, à l'un de vos questionnaires déjà existant.<br />
            La liste de vos Questionnaires apparaitra, vous pourrez choisir le Questionnaire dans lequel vous souhaitez ajouter ces nouvelles Questions.<br />
            </td>
        </tr>
    </table>
    </asp:Panel>

    <table class="TableQuestionnaireStyle" width="100%" border="0" cellpadding="3">
        <tr>
            <td>
                <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtenderAide2" runat="Server" 
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
            <td align="left" width="100%">
                <label class="LabelStyle">Créer un Questionnaire partir d'un Questionnaire existant<br />Ajouter des Questions à un Questionnaire existant</label>
            </td>
        </tr>
        <tr>
            <td align="left" colspan="2">
                <usrc:QuestionnaireExempleControl ID="QuestionnaireExempleControl" runat="server" />
            </td>
        </tr>
    </table>
    </div>
</asp:Content>

