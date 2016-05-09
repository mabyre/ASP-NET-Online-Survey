<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="Contact_Edit" Title="Edition d'un Contact" %>
<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="body">
<div class="DivPageCentreStyle">
    <h3><asp:Label ID="LabelTitre" runat="server"/></h3>
    <!-- Aide en ligne sur le Formulaire -->
    <asp:Panel ID="PanelAide" runat="server" class="PanelAideStyle">
    <table class="TableCollapsePanel">
        <tr align="left">
            <td>
            Créez un nouveau contact à interviewer par, au minimum, son <b>adresse email</b> ou son <b>numéro de téléphone</b>.<br />
            Les autres champs sont facultatifs.<br />
            Vous ne pouvez pas créer deux contacts avec la même adresse email ou le même numéro de téléphone pour le même questionnaire.<br />
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <table class="TableQuestionnaireStyle" width="100%" border="0" cellpadding="5">
        <tr>
            <td colspan="2">
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
        </tr>
        <tr>
            <td class="TdLabelStyle2" align="right" valign="top">
                <strong>Questionnaire : </strong>
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:Label ID="LabelQuestionnaire" runat="server" CssClass="TextBoxStyle" Enabled="false" />
            </td>
        </tr>
        <tr>  
            <td class="TdLabelStyle2" align="right">
                Civilité :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxCivilite" runat="server" CssClass="TextBoxStyle" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle2" align="right">
                Nom :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxNom" runat="server" CssClass="TextBoxStyle" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle2" align="right">
                <strong>Prénom : </strong>
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxPrenom" runat="server" CssClass="TextBoxStyle" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle2" align="right">
                <strong>Société : </strong>
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxSociete" runat="server" CssClass="TextBoxStyle" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle2" align="right">
                <strong>E-mail : </strong>
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxEmail" runat="server" CssClass="TextBoxStyle" />
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle2" align="right">
                <strong>Téléphone : </strong>
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxTelephone" runat="server" CssClass="TextBoxStyle" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
    </table>
        
    <table cellpadding="2">
        <tr>
            <td class="TdControlButtonStyle">
                <UserControl:RolloverButton ID="ButtonSave" runat="server" Text="Sauver" OnClick="ButtonSave_Click"/>                
            </td>
            <td>
                <UserControl:RolloverButton ID="ButtonCancel" runat="server" Text="Retour" OnClick="ButtonCancel_Click"/>                
            </td>
        </tr>
    </table>

    <table style="border:none" cellpadding="0" cellspacing="0">
        <tr>
            <td height="30px">
                <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
            </td>
        </tr>
    </table>
    
</div>
</div>
</asp:Content>