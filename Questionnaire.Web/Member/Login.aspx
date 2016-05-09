<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Page_Login" Title="Sodevlog - Connexion au Questionnaire en ligne" %>
<%@ Register TagPrefix="ucwc" TagName="WebContent" Src="~/UserControl/WebContent.ascx" %>

<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="DivPageLoginStyle">
    <asp:LoginView ID="LoginViewAuthentification" runat="server" >
        <AnonymousTemplate>
            <ucwc:WebContent ID="WebContent1" runat="server" Section="PageLogin" />
            <!-- compatibilite Chrome text-align:center dans la css n'est pas applique -->
            <div align="center"  >
            <asp:Login ID="LoginControl" runat="server">    
                <LayoutTemplate>
                <asp:Panel ID="Panel1" runat="server" GroupingText="Connexion" CssClass="PanelLogin" >
                    <table border="0" cellpadding="3" cellspacing="3">
                        <tr>
                            <td align="center" colspan="2" height="30px">
                                <asp:Literal runat="server" ID="FailureText" EnableViewState="false"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" AssociatedControlID="UserName" ID="LabelUserName">Nom d'utilisateur : </asp:Label>
                            </td>
                            <td align="left" height="30px">
                                <asp:TextBox runat="server" ID="UserName"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                    ErrorMessage="User Name is required." ToolTip="User Name is required." ID="UserNameRequired">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" height="30px">
                                <asp:Label runat="server" AssociatedControlID="Password" ID="LabelPassword">Mot de passe : </asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox runat="server" TextMode="Password" ID="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                    ErrorMessage="Password is required." ToolTip="Password is required." ID="PasswordRequired">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="left">
                                <asp:CheckBox runat="server" ID="RememberMe" Text="Se souvenir de ma connexion." />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center" style="padding-top:15px;">
                                <UserControl:RolloverButton ID="LoginButton" runat="server" Text="Entrez" CommandName="Login" OnClick="LoginButton_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                </LayoutTemplate>
            </asp:Login>
            </div>
            <table cellspacing="6px">
                <tr>
                    <td align="center">
                        <asp:Label runat="server" ID="LabelMessageUtilisateur" Visible="false" />
                    </td>
                </tr>
            </table>
        </AnonymousTemplate>
        <LoggedInTemplate>
            <ucwc:WebContent ID="WebContent2" runat="server" Section="PageAuthentifie" /> 
            <table border="0" cellpadding="9px" cellspacing="0" >
                <tr>
                    <td class="LoginUserText">
                        <label>Bonjour : </label>
                        <asp:LoginName ID="LoginName1" runat="server" /><br />
                    </td>
                </tr>
                <tr>
                    <td class="LoginUserText">
                        <asp:Label ID="LabelUserInRoles" runat="server" Text="Label" />
                    </td>
                </tr>
            </table>
            <!-- Aide en ligne -->
            <table border="0" cellpadding="35px" cellspacing="0" >
                <tr>
                    <td>
                        <table class="TableCollapsePanel">
                            <tr>
                                <td align="left">
                                <b>Etape 1 :</b> Créez un nouveau Questionnaire ou Créez un Questionnaire à partir d'un exemple en allant à l'étape 1 : "Créez un Questionnaire".<br />
                                <b>Etape 2 :</b> Visualiser vos Questionnaire en mode listing en allant à l'étape 2 : "Visualisez vos Questionnaires".<br />
                                Vous pouvez visualiser la liste de vos Questionnaire en cours en cliquant sur le menu <b>"Questionnaire"</b>.
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </LoggedInTemplate>
    </asp:LoginView>

    <ajaxToolkit:CollapsiblePanelExtender ID="cpe1" runat="Server" 
        AutoCollapse="false"  
        AutoExpand="false"
        TargetControlID="PanelPasswordRecovery"
        ExpandControlID="PanelControl"
        CollapseControlID="PanelControl" 
        Collapsed="true"
        ImageControlID="ImageButtonToggleImage"    
        ExpandedImage="~/App_Themes/Sodevlog/Images/collapse.jpg"
        CollapsedImage="~/App_Themes/Sodevlog/Images/expand.jpg"
        SuppressPostBack="true"
        SkinID="CollapsiblePanel" />  
    <asp:Panel ID="PanelControl" runat="server" CssClass="CollapsePanelHeader"> 
        <div class="CollapsePanelMotPasseOublie">
            <asp:ImageButton ID="ImageButtonToggleImage" CausesValidation="false" runat="server" ImageUrl="~/App_Themes/Sodevlog/Images/expand.jpg" AlternateText="Afficher" />
            Mot de passe oublié ?
        </div>
    </asp:Panel>
    <asp:Panel ID="PanelPasswordRecovery" runat="server" Visible="true">
    <asp:PasswordRecovery ID="PasswordRecovery1" runat="server" 
        CssClass="PasswordRecoveryStyle"
        SubmitButtonStyle-CssClass="ButtonStyle" 
        SuccessTextStyle-CssClass="LabelStyle" 
        SuccessTextStyle-Font-Bold="False" 
        ValidatorTextStyle-CssClass="LabelStyle" 
        LabelStyle-CssClass="LabelStyle">
        <UserNameTemplate>
            <asp:Literal EnableViewState="False" ID="FailureText" runat="server" />
            <table border="0" cellpadding="5" cellspacing="0">
                <tr valign="top">
                    <td colspan="2">
                        <asp:Label ID="Label1" runat="server" Text="Entrez votre Nom d'utilisateur pour retrouver votre Mot de Passe." />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label AssociatedControlID="UserName" ID="UserNameLabel" runat="server" Text="Nom d'utilisateur :" />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="UserName" runat="server" />
                        <asp:RequiredFieldValidator ControlToValidate="UserName" ErrorMessage="User Name is required."
                            ID="UserNameRequired" runat="server" ToolTip="User Name is required." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td height="35px" align="left">
                        <asp:Button CommandName="Submit" ID="SubmitButton" runat="server" Text="Valider" ValidationGroup="PasswordRecovery1" CssClass="ButtonStyle" />
                    </td>
                </tr>
            </table>
        </UserNameTemplate>
    </asp:PasswordRecovery>
    </asp:Panel>    

    <asp:Panel ID="PanelAuthentification" runat="server" Visible="false">
        <table border="0" cellpadding="25px" cellspacing="0">
            <tr>
                <td height="20px" width="380px" align="center">
                    <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>    
</div>
</asp:Content>

