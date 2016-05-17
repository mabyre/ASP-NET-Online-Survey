<%@ Page Language="C#" MasterPageFile="~/Admin/MasterAdminPage.master" ValidateRequest="false" AutoEventWireup="true" CodeFile="SettingsSmtp.aspx.cs" Inherits="Admin_Pages_SettingsSmtp" Title="Settings du serveur SMTP" %>
<%@ Register TagPrefix="uc" TagName="WebContent" Src="~/UserControl/WebContent.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">
    <br />
    <div style="text-align: right">
        <asp:Button runat="server" ID="ButtonSaveTop" Text="Sauver" OnClick="ButtonSave_OnClick" />
    </div>
    <br />
    <div class="settings">
    
        <h1 title="Serveur d'emails sortants">Configuration du Serveur de courrier sortant (SMTP)</h1>
        <asp:Panel ID="PanelAdmin" runat="server" Visible="false">
        <label>Choix utilisateur :</label>
        <UserControl:DropDownListMembre ID="DropDownListMembre" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListMembre_SelectedIndexChanged" />                
        <asp:CheckBox ID="CheckBoxCopier" runat="server" Text="Copier" Width="150px" ToolTip="Conserver les données d'un serveur Smtp déjà configuré" />
        <br />
        </asp:Panel>

        <label title="Cela peut être l'adresse email, comme par exemple chez Google Gmail">Nom d'utilisateur :</label>
        <asp:TextBox runat="server" ID="TextBoxUserName" Width="300" />                
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxUserName" ErrorMessage="Requis" />
        <br />

        <label title="Votre mot de passe">Mot de Passe :</label>
        <asp:TextBox runat="server" ID="TextBoxPassWord" Width="300" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TextBoxPassWord" ErrorMessage="Requis" />
        <br />
        
        <label title="smtp.nomdufournisseur.com .fr ou .net, exemple chez Gmail c'est smtp.gmail.com">Nom du Serveur :</label>
        <asp:TextBox runat="server" ID="TextBoxServerName" Width="300" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="TextBoxServerName" ErrorMessage="Requis" />
        <br />

        <label title="C'est 25 par défaut mais certains fournisseurs utilisent un autre numéro de port">Numéro de Port :</label>
        <asp:TextBox runat="server" ID="TextBoxServerPort" Width="300" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="TextBoxServerPort" ErrorMessage="Requis" />
        <br />

        <label title="Adresse email avec laquelle sera expédié le courrier">Adresse E-mail :</label>
        <asp:TextBox runat="server" ID="TextBoxEmail" Width="300" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
            ControlToValidate="TextBoxEmail" 
            ErrorMessage="Requis" />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="TextBoxEmail" 
            ErrorMessage="Merci d'entrer une adresse email valide."
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
        <br />
        
        <label title="Sujet de l'email, précède le nom du questionnaire">Sujet E-mail :</label>
        <asp:TextBox runat="server" ID="TextBoxEmailSubject" Width="300" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
            ControlToValidate="TextBoxEmailSubject" 
            ErrorMessage="Requis" />
        <br />
        
        <label title="Il est indispensable de cocher cette case avec les comptes Gmail par exemple">Enable SSL : </label>
        <asp:CheckBox runat="Server" ID="CheckBoxEnableSSL" />
        <br />
        <br />
                
        <!-- le corps de l'email est lie au questionnaire et pas au serveur SMTP ! -->
        <asp:Panel ID="PanelNaRienAFoutreIci" runat="server" Visible="false">
        <label>Corps E-mail :</label>
        <br />
        <br />
        <uc:WebContent ID="WebContentCorpsEmail" runat="server" Section="CorpsEmail" /> 
        </asp:Panel>
        
        <asp:Button runat="server" ID="ButtonTestSmtp" CausesValidation="False" Text="Tester paramètres email" ToolTip="Vous devez cliquer sur ce bouton pour valider vos paramêtres et obtenir : Test réussi, en vert sinon vos paramètres sont incorrectes." OnClick="ButtonTestSmtp_Click" />
        <asp:Label runat="Server" ID="LabelSmtpStatus" />          
        
        <asp:Panel ID="PanelMessageValidation" runat="server" >
        <table style="border:none" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td height="30px" align="center">
                    <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" />
                </td>
            </tr>
        </table>        
        </asp:Panel>
        
    </div>
    <div style="width:100%">
        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
        <tr>
            <td style="width:50%;text-align:left">
                <asp:Button runat="server" ID="Button1" Text="Copier" OnClick="ButtonCopier_OnClick" CausesValidation="False" 
                    ToolTip="Copier les coordonées d'un serveur SMTP pour effectuer des tests" />
            </td>
            <td style="width:50%;text-align:right">
                <asp:Button runat="server" ID="ButtonSave" Text="Sauver" OnClick="ButtonSave_OnClick" />
            </td>
        </tr>
        </table>
    </div>
    <br />
</asp:Content>