<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Contact_Login" Title="Sodevlog - Connexion au Questionnaire en ligne" %>
<%@ Register TagPrefix="ucwc" TagName="WebContent" Src="~/UserControl/WebContent.ascx" %>

<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="DivPageLoginStyle">
    <ucwc:WebContent ID="WebContentLoginContact" runat="server" Section="PageLoginContact" />
    
    <asp:Panel ID="PanelLoginContact" runat="server" GroupingText="Connexion" CssClass="PanelLogin" Width="350px" >
        <table border="0" cellpadding="3" cellspacing="6">
            <tr>
                <td align="center" colspan="2" height="30px">
                    <asp:Literal runat="server" ID="FailureText" EnableViewState="false"></asp:Literal>
                </td>
            </tr>
            <tr runat="server" id="TrAdresseCourielle">
                <td>
                    <asp:Label runat="server" AssociatedControlID="TextBoxAdresseCourrielle" 
                    ID="LabelUserName" Width="150px">Adresse courrielle :</asp:Label>
                </td>
                <td align="left" height="30px">
                    <asp:TextBox runat="server" ID="TextBoxAdresseCourrielle"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TextBoxAdresseCourrielle"
                        ErrorMessage="Adresse courrielle requise." 
                        ToolTip="Votre courrielle est requise" 
                        ID="RequiredFieldValidatorAdresse">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr runat="server" id="TrNumeroTelephone">
                <td>
                    <asp:Label runat="server" AssociatedControlID="TextBoxAdresseCourrielle" 
                    ID="Label1" Width="150px">Numéro de téléphone :</asp:Label>
                </td>
                <td align="left" height="30px">
                    <asp:TextBox runat="server" ID="TextBoxTelephone"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TextBoxTelephone"
                        ErrorMessage="Numéro de téléphone requis." 
                        ToolTip="Votre numéro de téléphone est requis" 
                        ID="RequiredFieldValidator1">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" height="30px">
                    <asp:Label runat="server" AssociatedControlID="TextBoxCodeAcces" 
                    ID="LabelPassword">Code d'accès : </asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox runat="server" TextMode="Password" ID="TextBoxCodeAcces"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TextBoxCodeAcces"
                        ErrorMessage="Le code d'accès est requis." 
                        ToolTip="Le code d'accès est requis." 
                        ID="RequiredFieldValidatorCodeAcces">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" height="30px">
                </td>
                <td align="left">
                    <UserControl:RolloverButton ID="LoginButton" runat="server" 
                    Text="Entrez" 
                    OnClick="LoginButton_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
            
    <table cellspacing="6px">
        <tr>
            <td align="center">
                <asp:Label runat="server" ID="LabelMessageUtilisateur" Visible="false" />
            </td>
        </tr>
    </table>

    <table border="0" cellpadding="25px" cellspacing="0">
        <tr>
            <td align="center">
                <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
            </td>
        </tr>
    </table>
</div>    
</asp:Content>

