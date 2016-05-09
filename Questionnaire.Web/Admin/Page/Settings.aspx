<%@ Page Language="C#" MasterPageFile="~/Admin/MasterAdminPage.master" ValidateRequest="false" AutoEventWireup="true" CodeFile="Settings.aspx.cs" Inherits="Admin_Pages_Settings" Title="Settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">

    <br />
    <div style="text-align: right">
        <asp:Button runat="server" ID="ButtonSaveTop" Text="Sauver" OnClick="ButtonSave_OnClick" />
    </div>
    <br />
    
    <div class="settings">
    
        <h1>Paramètres du Site</h1>
        <label for="<%=TextBoxSiteNom.ClientID %>">Nom du Site :</label>
        <asp:TextBox runat="server" ID="TextBoxSiteNom" Width="300" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxSiteNom" ErrorMessage="Requis" /><br />
        
        <label for="<%=TextBoxSiteSlogan.ClientID %>">Slogan du Site :</label>
        <asp:TextBox runat="server" ID="TextBoxSiteSlogan" Width="300" /><br />

        <label for="<%=TextBoxAdresse.ClientID %>">Adresse du site :</label>
        <asp:TextBox runat="server" ID="TextBoxAdresse" Width="300px"/>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBoxAdresse" ErrorMessage="Requis" /><br />

        <label for="<%=TextBoxCopyright.ClientID %>">Copyright :</label>
        <asp:TextBox runat="server" ID="TextBoxCopyright" Width="300" Text="Copyright :" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBoxCopyright" ErrorMessage="Requis" /><br />
        
    </div>
    
    <div class="settings">
        <h1>Labels</h1>
        <label>Label du bouton répondre :</label>
        <asp:TextBox runat="server" ID="TextBoxBoutonQuestion" Width="300" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TextBoxBoutonQuestion" ErrorMessage="Requis" /><br />
    </div>
    
    <div class="settings">
        <h1>Paramêtres</h1>
        <asp:Label Width="250px" ID="Label5" runat="server" ToolTip="Pour logguer l'utilisateur savoir d'où il vient">Racine du site web :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxVirtualPath" /><br />
        <asp:Label Width="250px" ID="onsenfou1244" runat="server" ToolTip="Lors de l'interview">Nombre de questions par colonne :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxRepeatColumns" Width="45px" /><br />
        <asp:Label  Width="250px" ID="Label1" runat="server" ToolTip="Ecrire les traces dans le fichier .log">Loguer les utilisateurs :</asp:Label>
        <asp:CheckBox runat="server" ID="CheckBoxLogUser" Width="45px" /><br />
        <asp:Label  Width="250px" ID="Label2" runat="server" ToolTip="Forcer l'enregistrement d'un contact avec un nom de sosicété">Enregistrer Contact avec Société :</asp:Label>
        <asp:CheckBox runat="server" ID="CheckBoxContactAvecSociete" Width="45px" /><br />
        <asp:Label  Width="250px" ID="Label3" runat="server" ToolTip="Donner la possiblité au intervieweur de questionnaires anonymes c.a.d à un contact non identifié de s'enregistrer">Enregistrer Contact Anonyme :</asp:Label>
        <asp:CheckBox runat="server" ID="CheckBoxContactAnonyme" Width="45px" /><br />
    </div>

    <div class="settings">
        <h1>Paramêtres Membre</h1>
        <asp:Label Width="350px" ID="Label4" runat="server" ToolTip="Donner la possiblité à l'administrateur de débloqué les clients : PanelDebloquerClient.Visible = Global.SettingsXml.DebloquerClient">Débloquer les Clients :</asp:Label>
        <asp:CheckBox runat="server" ID="CheckBoxDebloquerClient" Width="45px" /><br />
        <asp:Label Width="350px" ID="Label12" runat="server" ToolTip="Un membre vient de s'inscrire envoyer un email à l'admin">Prévenir sur la création d'un nouveau membre :</asp:Label>
        <asp:CheckBox runat="server" ID="CheckBoxNouveauMembrePrevenir" Width="45px" /><br />
        <asp:Label Width="350px" ID="Label15" runat="server" ToolTip="Un membre vient de se connecter">Prévenir sur la connexion d'un membre :</asp:Label>
        <asp:CheckBox runat="server" ID="CheckBoxConnexionMembrePrevenir" Width="45px" /><br />
        <asp:Label Width="350px" ID="Label13" runat="server" ToolTip="Le membre qui se créé est approuvé, s'il n'a pas besoin de s'approuver par email">Nouveau membre est approuvé :</asp:Label>
        <asp:CheckBox runat="server" ID="CheckBoxNouveauMembreApprouve" Width="45px" /><br />
        <asp:Label Width="350px" ID="Label16" runat="server" ToolTip="Le membre qui se créé s'approuve en cliquant dans un email">Nouveau membre est approuvé par email :</asp:Label>
        <asp:CheckBox runat="server" ID="CheckBoxNouveauMembreApprouveParEmail" Width="45px" /><br />
        <asp:Label Width="250px" ID="Label14" runat="server" ToolTip="Code d'accès du questionnaire de l'Intervieweur à copier pour un nouveau membre enregistré">Code d'accès questionnaire exemple :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxCodeAccesQuestionnaireExemple" /><br />
    </div>

    <div class="settings">
        <h1>Réponses textuelles</h1>
        <asp:Label Width="250px" ID="Label6" runat="server" ToolTip="Largueur min d'une réponse textuelle en pixels">Largeur Min :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxLargeurMin" Width="45px" /><br />
        <asp:Label Width="250px" ID="Label7" runat="server" ToolTip="Largueur max d'une réponse textuelle en pixels">Largeur Max :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxLargeurMax" Width="45px" /><br />
        <asp:Label Width="250px" ID="Label8" runat="server" ToolTip="Nombre de lignes max d'une réponse textuelle">Lignes Max :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxLigneMax" Width="45px" /><br />
    </div>

    <div class="settings">
        <h1>Contacts par page</h1>
        <asp:Label Width="250px" ID="Label9" runat="server" >Min :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxContactsParPageMin" Width="45px" /><br />
        <asp:Label Width="250px" ID="Label10" runat="server" >Max :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxContactsParPageMax" Width="45px" /><br />
        <asp:Label Width="250px" ID="Label11" runat="server" >Courant :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxContactsParPageCourant" Width="45px" /><br />
    </div>
    
    <div class="settings">
        <h1>Limitations de l'utilisateur découverte</h1>
        <asp:Label Width="250px" ID="Label17" runat="server" >Nombre de Questionnaires :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxGratuitLimiteQuestionnaires" Width="45px" /><br />
        <asp:Label Width="250px" ID="Label18" runat="server" >Nombre de Questions :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxGratuitLimiteQuestions" Width="45px" /><br />
        <asp:Label Width="250px" ID="Label19" runat="server" ToolTip="Limite du nombre d'interviewés" >Nombre de Contats :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxGratuitLimiteInterviewes" Width="45px" /><br />
        <asp:Label Width="250px" ID="Label24" runat="server" ToolTip="Limite du nombre de réponses" >Nombre de Réponses :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxGratuitLimiteReponses" Width="45px" /><br />
        <br />
        <h1>Limitations de l'utilisateur abonné</h1>
        <asp:Label Width="250px" ID="Label21" runat="server" >Nombre de Questionnaires :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxAbonneLimiteQuestionnaires" Width="45px" /><br />
        <asp:Label Width="250px" ID="Label22" runat="server" >Nombre de Questions :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxAbonneLimiteQuestions" Width="45px" /><br />
        <asp:Label Width="250px" ID="Label23" runat="server" ToolTip="Limite du nombre d'interviewés" >Nombre de Contats :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxAbonneLimiteInterviewes" Width="45px" /><br />
        <asp:Label Width="250px" ID="Label25" runat="server" ToolTip="Limite du nombre de réponses" >Nombre de Réponses :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxAbonneLimiteReponses" Width="45px" /><br />
    </div>

    <div class="settings">
        <h1>Limitations des Imports dans la liste des interviewés</h1>
        <asp:Label Width="250px" ID="Label20" runat="server" >Max :</asp:Label>
        <asp:TextBox runat="server" ID="TextBoxMaxImportsInterviewes" Width="45px" /><br />
    </div>        
    
    <div class="settings">
        <h1>Informations</h1>
        <table border="0" cellpadding="5px">
        <tr>
            <td>
                <label title="Pour le Répertoire ~/UserFiles des documents téléchargés dans les pages">Taille des fichiers :</label>
                <asp:Label runat="server" ID="LabelTailleUserFiles" Width="300" /><br />
            </td>
        </tr>
        <tr>
            <td>
                <label title="Pour le Répertoire ~/MemberDataFiles des données membre">Taille des données membre :</label>
                <asp:Label runat="server" ID="LabelTailleMemberDataFiles" Width="300" /><br />
            </td>
        </tr>
        <tr>
            <td>
                <label title="Nombre d'utilisateurs connectés actuellement">Utilisateurs connectés :</label>
                <asp:Label runat="server" ID="LabelUtilisateursConnecte" ForeColor="blue" Width="300" />
            </td>
        </tr>
        </table>
    </div>

    <div align="right">
        <asp:Button runat="server" ID="ButtonSave" Text="Sauver" OnClick="ButtonSave_OnClick" />
    </div>
    <br />
</asp:Content>