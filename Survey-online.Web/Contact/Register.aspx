<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Contact_Register" Title="Enregistrement d'un interviewé pour un questionnaire" %>
<%@ Register TagPrefix="uc" TagName="WebContent" Src="~/UserControl/WebContent.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="body">
<div style="text-align:left; padding-left:30%" ><!-- AME011020104 padding-left:30% -->
    <uc:WebContent ID="WebContent1" runat="server" Section="PageEnregistrement" /> 
    <h3><asp:Label ID="LabelTitre" runat="server" /></h3>
    
    <asp:Panel ID="PanelEnregistrement" runat="server" Visible ="false" >
    
    <table border="0" cellpadding="3">
        <tr>
            <td class="TdLabelRegisterStyle" align="right">
                Questionnaire :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:Label ID="LabelQuestionnaire" runat="server" CssClass="TextBoxStyle" Enabled="false" />
            </td>
        </tr>
        <tr runat="server" id="TrCivilite">
            <td class="TdLabelRegisterStyle" align="right">
                Civilité :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxCivilite" runat="server" CssClass="TextBoxStyle" />
            </td>
        </tr>
        <tr  runat="server" id="TrNom">
            <td class="TdLabelRegisterStyle" align="right">
                Nom :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxNom" runat="server" CssClass="TextBoxStyle" />
            </td>
        </tr>
        <tr runat="server" id="TrPrenom">
            <td class="TdLabelRegisterStyle" align="right">
                Prénom :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxPrenom" runat="server" CssClass="TextBoxStyle" />
            </td>
        </tr>
        <tr runat="server" id="TrSociete">
            <td class="TdLabelRegisterStyle" align="right">
                Société :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxSociete" runat="server" CssClass="TextBoxStyle" />
            </td>
        </tr>
        <tr runat="server" id="TrEmail">
            <td class="TdLabelRegisterStyle" align="right">
                E-mail :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxEmail" runat="server" CssClass="TextBoxStyle" />
            </td>
        </tr>
        <tr  runat="server" id="TrNumeroTelephone">
            <td class="TdLabelRegisterStyle" align="right">
                Téléphone :
            </td>
            <td class="TdTextBoxStyle" align="left">
                <asp:TextBox ID="TextBoxTelephone" runat="server" CssClass="TextBoxStyle" />
            </td>
        </tr>
    </table>
    
    </asp:Panel>
            
    <asp:Panel ID="PanelBoutonEnregistrement" runat="server" Visible="false">
    <table cellpadding="2" border="0">
        <tr>
            <td class="TdLabelRegisterStyle" align="right">
                &nbsp;
            </td>
            <td class="TdControlButtonStyle">
                <UserControl:RolloverButton ID="ButtonEnregistrer" runat="server" Text="Enregistrez" OnClick="ButtonEnregistrer_Click" Visible="false" ToolTip="Avec vérification anti-bot CAPTCHA"/>                
                <UserControl:ButtonRepondre ID="ButtonRepondre" runat="server" Text="Répondre" OnClick="ButtonRepondre_Click" Visible="false" ToolTip="Cliquez pour répondre aux questions"/>                
            </td>
        </tr>
        <tr>
            <td class="TdLabelRegisterStyle" align="right">
                &nbsp;
            </td>
            <td>
                <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
            </td>
        </tr>
    </table>
    </asp:Panel>    
    
</div>
</div>
</asp:Content>