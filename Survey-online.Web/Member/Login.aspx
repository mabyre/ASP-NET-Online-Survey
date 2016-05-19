<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Page_Login" Title="Sodevlog - Connexion au Questionnaire en ligne" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
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
            <asp:Login ID="LoginControl" runat="server" meta:resourcekey="LoginControlResource1">    
                <LayoutTemplate>
                <asp:Panel ID="Panel1" runat="server" GroupingText="Connexion" CssClass="PanelLogin" meta:resourcekey="Panel1Resource1" >
                    <table border="0" cellpadding="3" cellspacing="3">
                        <tr>
                            <td align="center" colspan="2" height="30px">
                                <asp:Literal runat="server" ID="FailureText" EnableViewState="False" meta:resourcekey="FailureTextResource1"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" AssociatedControlID="UserName" ID="LabelUserName" meta:resourcekey="LabelUserNameResource1">Nom d'utilisateur : </asp:Label>
                            </td>
                            <td align="left" height="30px">
                                <asp:TextBox runat="server" ID="UserName" meta:resourcekey="UserNameResource2"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                    ErrorMessage="User Name is required." ToolTip="User Name is required." ID="UserNameRequired" meta:resourcekey="UserNameRequiredResource1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" height="30px">
                                <asp:Label runat="server" AssociatedControlID="Password" ID="LabelPassword" meta:resourcekey="LabelPasswordResource1">Mot de passe : </asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox runat="server" TextMode="Password" ID="Password" meta:resourcekey="PasswordResource2"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                    ErrorMessage="Password is required." ToolTip="Password is required." ID="PasswordRequired" meta:resourcekey="PasswordRequiredResource1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="left">
                                <asp:CheckBox runat="server" ID="RememberMe" Text="Se souvenir de ma connexion." meta:resourcekey="RememberMeResource1" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center" style="padding-top:15px;">
                                <UserControl:RolloverButton ID="LoginButton" runat="server" Text="Entrez" CommandName="Login" OnClick="LoginButton_Click" meta:resourcekey="LoginButtonResource1" />
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
                        <asp:Label runat="server" ID="LabelMessageUtilisateur" Visible="False" meta:resourcekey="LabelMessageUtilisateurResource1" />
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
                        <asp:LoginName ID="LoginName1" runat="server" meta:resourcekey="LoginName1Resource1" /><br />
                    </td>
                </tr>
                <tr>
                    <td class="LoginUserText">
                        <asp:Label ID="LabelUserInRoles" runat="server" Text="Label" meta:resourcekey="LabelUserInRolesResource1" />
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
        TargetControlID="PanelPasswordRecovery"
        ExpandControlID="PanelControl"
        CollapseControlID="PanelControl" 
        Collapsed="True"
        ImageControlID="ImageButtonToggleImage"    
        ExpandedImage="~/App_Themes/Sodevlog/Images/collapse.jpg"
        CollapsedImage="~/App_Themes/Sodevlog/Images/expand.jpg"
        SuppressPostBack="True"
        SkinID="CollapsiblePanel" Enabled="True" />  
    <asp:Panel ID="PanelControl" runat="server" CssClass="CollapsePanelHeader" meta:resourcekey="PanelControlResource1"> 
        <div class="CollapsePanelMotPasseOublie">
            <asp:ImageButton ID="ImageButtonToggleImage" CausesValidation="False" runat="server" ImageUrl="~/App_Themes/Sodevlog/Images/expand.jpg" AlternateText="Afficher" meta:resourcekey="ImageButtonToggleImageResource1" />
            <asp:Label runat="server" ID="LabelPassForgotten" meta:resourcekey="LabelPassForgotten1">Mot de passe oublié ?</asp:Label>
        </div>
    </asp:Panel>
    <asp:Panel ID="PanelPasswordRecovery" runat="server" meta:resourcekey="PanelPasswordRecoveryResource1">
    <asp:PasswordRecovery ID="PasswordRecovery1" runat="server" 
        CssClass="PasswordRecoveryStyle" meta:resourcekey="PasswordRecovery1Resource1">
        <SubmitButtonStyle CssClass="ButtonStyle" />
        <LabelStyle CssClass="LabelStyle" />
        <SuccessTextStyle CssClass="LabelStyle" Font-Bold="False" />
        <UserNameTemplate>
            <asp:Literal ID="FailureText" runat="server" EnableViewState="False" meta:resourcekey="FailureTextResource2"></asp:Literal>
            <table border="0" cellpadding="5" cellspacing="0">
                <tr valign="top">
                    <td colspan="2">
                        <asp:Label ID="Label1" runat="server" meta:resourcekey="Label1Resource1" Text="Entrez votre Nom d'utilisateur pour retrouver votre Mot de Passe."></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" meta:resourcekey="UserNameLabelResource1" Text="Nom d'utilisateur :"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="UserName" runat="server" meta:resourcekey="UserNameResource3"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="User Name is required." meta:resourcekey="UserNameRequiredResource2" ToolTip="User Name is required." ValidationGroup="PasswordRecovery1">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td align="left" height="35px">
                        <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" CssClass="ButtonStyle" meta:resourcekey="SubmitButtonResource1" Text="Valider" ValidationGroup="PasswordRecovery1" />
                    </td>
                </tr>
            </table>
        </UserNameTemplate>
        <ValidatorTextStyle CssClass="LabelStyle" />
    </asp:PasswordRecovery>
    </asp:Panel>    

    <asp:Panel ID="PanelAuthentification" runat="server" Visible="False" meta:resourcekey="PanelAuthentificationResource1">
        <table border="0" cellpadding="25px" cellspacing="0">
            <tr>
                <td height="20px" width="380px" align="center">
                    <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="False" meta:resourcekey="ValidationMessageResource1" />
                </td>
            </tr>
        </table>
    </asp:Panel>    
</div>
</asp:Content>

