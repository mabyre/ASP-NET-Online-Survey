<%@ Page Language="C#" MasterPageFile="~/Admin/MasterAdminPage.master" ValidateRequest="false" AutoEventWireup="true" CodeFile="SettingsOptions.aspx.cs" Inherits="Admin_Pages_SettingsOptions" Title="Settings Options" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" runat="Server">

    <br />
    <br />

    <div class="settings">
    
        <h1>Paramètres de l'Application</h1>
        <asp:Label Width="240px" ID="Label5" runat="server" 
            ToolTip="Recevoir un email si un interviewés répond au Questionnaire">Prévenir sur une nouvelle réponse :</asp:Label>
        <asp:CheckBox runat="server" ID="CheckBoxPrevenirNouvelleReponse"
            OnCheckedChanged="CheckBoxPrevenirNouvelleReponse_CheckedChanged" AutoPostBack="true" />
        <br />                  
        <asp:Label Width="240px" ID="Label3" runat="server" 
            ToolTip="Pagination des Questions dans Visualiser,Modifier - Nombre de Questions par Page : min 1 max 100">Nombre de questions par page :</asp:Label>
        <asp:TextBox ID="TextBoxTaillePage" runat="server" AutoPostBack="true" Width="45px"
            OnTextChanged="TextBoxTaillePage_TextChanged" />
            
    </div>
    
    <div class="settings">
    
        <h1>Bouton question suivante</h1>
        <asp:Label Width="240px" ID="Label1" runat="server" 
            ToolTip="Texte du bouton question suivante dans le questionnaire">Texte du bouton question suivante :</asp:Label>
        <asp:TextBox ID="TextBoxBoutonQuestionSuivanteTexte" runat="server" AutoPostBack="true"
            OnTextChanged="TextBoxBoutonQuestionSuivanteTexte_TextChanged" />
        <br />        
        <asp:Label Width="240px" ID="Label2" runat="server" 
            ToolTip="Titre ou Alt du bouton question suivante dans le questionnaire">Titre du bouton question suivante :</asp:Label>
        <asp:TextBox ID="TextBoxBoutonQuestionSuivanteAlt" runat="server" AutoPostBack="true" Width="250px"
            OnTextChanged="TextBoxBoutonQuestionSuivanteAlt_TextChanged" />
    

    </div>

    <br />
    <br />

</asp:Content>