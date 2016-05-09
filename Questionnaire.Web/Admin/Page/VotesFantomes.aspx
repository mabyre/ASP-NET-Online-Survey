<%@ Page Language="C#" MasterPageFile="~/Admin/MasterAdminPage.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="VotesFantomes.aspx.cs" Inherits="Admin_Page_VotesFantomes" Title="Page sans titre" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">

    <table style="border:none" cellpadding="0" cellspacing="12">
    <thead>
        <tr>
            <td>VoteId</td>
            <td>UserGUID</td>
            <td>CreationDate</td>
            <td>PollQuestionID</td>
        </tr>
    </thead>
    <tr>
        <td>
            <asp:Label ID="Label1" CssClass="LabelValidationMessageStyle" Runat="server" />
        </td>
        <td>
            <asp:Label ID="Label2" CssClass="LabelValidationMessageStyle" Runat="server" />
        </td>
        <td>
            <asp:Label ID="Label3" CssClass="LabelValidationMessageStyle" Runat="server" />
        </td>
        <td>
            <asp:Label ID="Label4" CssClass="LabelValidationMessageStyle" Runat="server" />
        </td>
    </tr>
    </table>
    
    <table style="border:none" cellpadding="0" cellspacing="12">
    <tr>
        <td>
            <asp:Label ID="LabelVotes" CssClass="LabelValidationMessageStyle" Runat="server" />
        </td>
    </tr>
    </table>

    <table style="border:none" cellpadding="0" cellspacing="12">
    <tr>
        <td colspan="2">
            <b>Trouvre une question par son PollQuestionID</b>
        </td>
    </tr>
    <tr>
        <td>
            PollQuestionID : <asp:TextBox ID="TextBoxPollQuestionID" runat="server" ToolTip="" Width="290px" />
        </td>
        <td>
            <asp:Button ID="ButtonGetQuestion" runat="server" OnClick="ButtonGetQuestion_Click" Text="Afficher" ToolTip="Afficher la question" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="LabelQuestion" CssClass="LabelValidationMessageStyle" Runat="server" ToolTip="Description du Questionnaire | Question" Text="" />
        </td>
    </tr>
    </table>

    <table style="border:none" cellpadding="0" cellspacing="12">
    <tr>
        <td colspan="2">
            <b>Supprimer un vote fantôme par son VoteID</b>
        </td>
    </tr>
    <tr>
        <td>
            VoteID : <asp:TextBox ID="TextBoxVoteID" runat="server" ToolTip="" Width="290px" />
        </td>
        <td>
            <asp:Button ID="Button1" runat="server" OnClick="ButtonSupprimerVote_Click" Text="Supprimer" ToolTip="Supprimer le vote"/>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="LabelVote" CssClass="LabelValidationMessageStyle" Runat="server" ToolTip="Nom d'utilisateur/Nom/Prénom | Description du Questionnaire | Question" Text="" />
        </td>
    </tr>
    </table>

    <table style="border:none" cellpadding="0" cellspacing="12">
    <tr>
        <td colspan="2">
            <b>Supprimer des votes fantômes par le UserGUID</b>
        </td>
    </tr>
    <tr>
        <td>
            UserGUID : <asp:TextBox ID="TextBoxUserGUID" runat="server" ToolTip="" Width="290px" />
        </td>
        <td>
            <asp:Button ID="ButtonSupprimerVotesUserGUID" runat="server" OnClick="ButtonSupprimerVotesUserGUID_Click" Text="Supprimer" ToolTip="Supprimer tous les votes d'un UserGUID"/>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Label ID="LabelVotesUserGUID" CssClass="LabelValidationMessageStyle" Runat="server" ToolTip="" Text="" />
        </td>
    </tr>
    </table>
        
    <table style="border:none" cellpadding="0" cellspacing="12" width="100%">
    <tr>
        <td align="center">
            <asp:Button ID="ButtonSupprimer" runat="server" OnClick="ButtonSupprimer_Click" Text="Rafraichir" ToolTip="Ne fait rien pour l'instant on observe" />
        </td>
    </tr>
    </table>

</asp:Content>

