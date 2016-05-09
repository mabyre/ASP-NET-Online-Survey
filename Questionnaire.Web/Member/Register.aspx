<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Member_Register" Title="Enregistrement d'un nouveau membre" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="body">
<div class="DivMemberRegister">
    <h3>Enregistrez un nouvel utilisateur</h3>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <!-- Infos Membre -->
    <!-- Pour ne pas focer l'utisateur ajouter Enabled="false" dans le Validator -->
    <div class="DivTableStyle">
    <table border="0" cellpadding="3" cellspacing="3">
        <tr>
            <td class="TdLabelStyle" align="right">
                Nom d'utilisateur : 
            </td>
            <td class="TdRegisterTextBoxStyle">
                <asp:TextBox ID="TextBoxNomUtilisateur" runat="server" CssClass="TextBoxRegisterStyle" />
            </td>
            <td class="TdRequiredFieldValidatorStyle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="TextBoxNomUtilisateur"
                    ErrorMessage="Entrez un nom d'utilisateur." 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Mot de Passe : 
            </td>
            <td class="TdRegisterTextBoxStyle">
                <asp:TextBox ID="TextBoxPassWord" TextMode="Password" runat="server" 
                    CssClass="TextBoxRegisterStyle" />
            </td>
            <td class="TdRequiredFieldValidatorStyle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorPassWord" runat="server" 
                    ControlToValidate="TextBoxPassWord"
                    ErrorMessage="Entrez un mot de Passe." 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
                <asp:CustomValidator ID="CustomValidatorPassWord" runat="server" 
                    ControlToValidate = "TextBoxPassWord"
                    ClientValidationFunction="validateLengthPassWord" >
                </asp:CustomValidator>
            </td>
        </tr>
        
<script type="text/javascript">
function validateLengthPassWord( oSrc, args )
{
args.IsValid = ( args.Value.length >= '<%=Membership.MinRequiredPasswordLength%>' );
}
</script>

        <tr>
            <td class="TdLabelStyle" align="right">
                Confirmez le Mot de Passe : 
            </td>
            <td class="TdRegisterTextBoxStyle">
                <asp:TextBox ID="TextBoxConfirmPassword" TextMode="Password" runat="server" 
                    CssClass="TextBoxRegisterStyle"></asp:TextBox>
            </td>
            <td class="TdRequiredFieldValidatorStyle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="TextBoxConfirmPassword"
                    ErrorMessage="Merci de confirmer votre Mot de Passe." 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToCompare="TextBoxPassWord" 
                    ControlToValidate="TextBoxConfirmPassword"
                    ErrorMessage="La confirmation du Mot de Passe ne correspond pas." 
                    ValidationGroup="reg">*</asp:CompareValidator></td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Question de Sécurité : 
            </td>
            <td class="TdRegisterTextBoxStyle">
                <asp:TextBox ID="TextBoxQuestion" runat="server" CssClass="TextBoxRegisterStyle" />
            </td>
            <td class="TdRequiredFieldValidatorStyle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ControlToValidate="TextBoxQuestion"
                    ErrorMessage="Entrez une question de sécurité." 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
                </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Réponse de Sécurité : 
            </td>
            <td class="TdRegisterTextBoxStyle">
                <asp:TextBox ID="TextBoxAnswer" runat="server" CssClass="TextBoxRegisterStyle" />
            </td>
            <td class="TdRequiredFieldValidatorStyle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                    ControlToValidate="TextBoxAnswer"
                    ErrorMessage="Merci d'entrer une réponse de sécurité." 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
            </td>
        </tr>
</table>
</div>
<!-- Information sur l'utilisateur -->
<br />
<div class="DivTableStyle">
<table border="0" cellpadding="3" cellspacing="3">
        <tr>
            <td class="TdLabelStyle" align="right">
                Nom : 
            </td>
            <td class="TdRegisterTextBoxStyle">
                <asp:TextBox ID="TextBoxLastName" runat="server" CssClass="TextBoxRegisterStyle" />
            </td>
            <td class="TdRequiredFieldValidatorStyle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                    ControlToValidate="TextBoxLastName"
                    ErrorMessage="Merci d'entrer votre nom." 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Prénom : 
            </td>
            <td class="TdRegisterTextBoxStyle">
                <asp:TextBox ID="TextBoxFisrtName" runat="server" CssClass="TextBoxRegisterStyle" />
            </td>
            <td class="TdRequiredFieldValidatorStyle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                    ControlToValidate="TextBoxFisrtName"
                    ErrorMessage="Merci d'entrer votre prénom." 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Email : 
            </td>
            <td class="TdRegisterTextBoxStyle">
                <asp:TextBox ID="TextBoxEmail" runat="server" CssClass="TextBoxRegisterStyle" />
            </td>
            <td class="TdRequiredFieldValidatorStyle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="TextBoxEmail"
                    ErrorMessage="Merci d'entrer votre adresse email." 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="TextBoxEmail" 
                    ErrorMessage="Merci d'entrer une adresse email valide."
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                    ValidationGroup="reg">*</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Société : 
            </td>
            <td class="TdRegisterTextBoxStyle">
                <asp:TextBox ID="TextBoxSociete" runat="server" CssClass="TextBoxRegisterStyle" />
            </td>
            <td class="TdRequiredFieldValidatorStyle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                    ControlToValidate="TextBoxSociete"
                    ErrorMessage="Merci d'entrer votre société." 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right" valign="top">
                Adresse : 
            </td>
            <td class="TdRegisterTextBoxStyle">
                <asp:TextBox ID="TextBoxAdresse" runat="server" CssClass="TextBoxRegisterStyle" TextMode="MultiLine" Rows="3"/>
            </td>
            <td class="TdRequiredFieldValidatorStyle" valign="top">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                    ControlToValidate="TextBoxAdresse"
                    ErrorMessage="Merci d'entrer votre adresse." 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="TdLabelStyle" align="right">
                Téléphone : 
            </td>
            <td class="TdRegisterTextBoxStyle">
                <asp:TextBox ID="TextBoxTelephone" runat="server" CssClass="TextBoxRegisterStyle"></asp:TextBox>
            </td>
            <td class="TdRequiredFieldValidatorStyle">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" 
                    ControlToValidate="TextBoxTelephone"
                    ErrorMessage="Merci d'entrer votre numéro de téléphone." 
                    ValidationGroup="reg">*</asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>    
    </div>     

    <br />
    <div class="DivTableStyle">
    <table border="0" cellpadding="3" cellspacing="3">
        <tr>
            <td class="TdRegisterCheckBoxConditionStyle" colspan="3">
                <asp:CheckBox ID="CheckBoxConditionGenerales" runat="server" 
                    CssClass="CheckBoxRegisterStyle"
                    Text="J'accepte les conditions générales d'utilisation" />
                <asp:CustomValidator ID="CustomValidator2" runat="server"
                   Display="Static"
                   ErrorMessage="Veuillez accepter les conditions générales d'utilisation."
                   ForeColor="red"
                   Font-Names="verdana" 
                   Font-Size="10pt"
                   ClientValidationFunction="validateCheckBox"
                   ValidationGroup="reg">*</asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td class="TdRegisterStyle">
            </td>
            <td class="TdValidationSummaryStyle" colspan="2">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="reg"/>
            </td>
        </tr>
    </table>
    </div>
   
<script type="text/javascript">
function validateCheckBox(oSrc, args)
{
args.IsValid = document.getElementById('<%=CheckBoxConditionGenerales.ClientID%>').checked;
}
</script>
    
    <table border="0" width="100%" height="60">
    <tr>
        <td align="center">
            <UserControl:RolloverButton ID="ButtonEnregistrer" runat="server" 
                Text="Enregistrer" 
                ValidationGroup="reg" 
                OnClick="ButtonEnregistrer_Click" 
                ToolTip="Avec vérification anti-bot CAPTCHA" />
            <UserControl:RolloverButton ID="ButtonRetour" runat="server"
                Visible="false" 
                Text="Retour" 
                ToolTip="Retourner vers l'application" 
                OnClick="ButtonRetour_Click" />
        </td>
    </tr>
    </table>
    
    <table style="border:none" cellpadding="0" cellspacing="0">
        <tr>
            <td height="30">
                <asp:Label ID="ValidationMessage" Runat="server"
                    CssClass="LabelValidationMessageStyle"
                    Visible="false"/>
            </td>
        </tr>
    </table>    
    
    </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
        AssociatedUpdatePanelID="UpdatePanel1" 
        DisplayAfter="10">
        <ProgressTemplate>
            <asp:Image ID="loadingimg" runat="server" SkinID="ImageLoading" />
        </ProgressTemplate>
    </asp:UpdateProgress>
</div>
</div>
</asp:Content>

