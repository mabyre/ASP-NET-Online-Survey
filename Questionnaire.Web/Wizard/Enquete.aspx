<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Enquete.aspx.cs" Inherits="Wizard_Enquete" Title="Gérer vos enquêtes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="DivPageAccueilStyle">
<h3>Gérez vos enquêtes</h3>

<table border="0" cellpadding="9px" cellspacing="10">
    <tr>
        <td class="LoginUserText">
            <asp:Label ID="LabelNombreQuestionnaires" runat="server" Text="Vous avez actuellement " />
            <span class="SpanHyperLinkAccueilStyle">
            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="HyperLinkAccueilStyle" 
                ToolTip="Accédez à la liste de vos Questionnaires"
                Text=" questionnaires en cours" 
                NavigateUrl="~/Questionnaire/Manage.aspx" />
            </span>
        </td>
    </tr>
    <tr>
        <td height="35px">
        </td>
    </tr>
</table>

<table border="0" cellpadding="3" cellspacing="0" >
    <tr>
        <td valign="top" align="right" >
            <label class="LabelStyle" title="étape 5"></label>
        </td>
        <td>
            <asp:ImageButton ID="Image4" runat="server" BorderColor="#5282D4" BorderStyle="Solid" BorderWidth="1px"
                ImageUrl="~/Images/Wizard/rediger email.jpg" 
                ToolTip="Rédigez l'email aux interviewés" 
                onclick="ButtonRedigerEmail_Click" />
        </td>
        <td valign="top" align="right" >
            <label class="LabelStyle" title="étape 6"></label>
        </td>
        <td>
            <asp:ImageButton ID="Image8" runat="server" BorderColor="#5282D4" BorderStyle="Solid" BorderWidth="1px"
            ImageUrl="~/Images/Wizard/importer.jpg" 
            ToolTip="Importez les contacts que vous allez interviewer" 
            onclick="ButtonImporter_Click" />
        </td>
        <td>
        </td>
        <td>
            <asp:ImageButton ID="Image6" runat="server" BorderColor="#5282D4" BorderStyle="Solid" BorderWidth="1px"
            ImageUrl="~/Images/Wizard/exporter.jpg" 
            ToolTip="Gérez l'envoi des emails aux interviewés" 
            onclick="ButtonEvoyerEmail_Click" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td class="TableAccueilButtonStyle">
        <asp:Button ID="ButtonRedigerEmail" CssClass="ButtonAccueilStyle" 
            runat="server" Text="Editez l'email envoyé aux interviewés" 
            ToolTip="Rédigez l'email qui sera envoyé aux interviewés" 
            onclick="ButtonRedigerEmail_Click" />
        </td>
        <td >
        </td>
        <td class="TableAccueilButtonStyle">
            <asp:Button ID="ButtonImporter" CssClass="ButtonAccueilStyle" 
                runat="server" Text="Importez la liste des Interviewés" 
                ToolTip="Importez les contacts que vous allez interviewer" 
                onclick="ButtonImporter_Click"/>
        </td>
        <td>
        </td>
        <td class="TableAccueilButtonStyle">
        <asp:Button ID="ButtonEvoyerEmail" CssClass="ButtonAccueilStyle" 
            runat="server" Text="Envoyez les emails aux Interviewés" 
            ToolTip="Gérez l'envoi des emails aux interviewés" 
            onclick="ButtonEvoyerEmail_Click"/>
        </td>
    </tr>
    <tr>
    <tr height="25px">
        <td>
        </td>
        <td>
        </td>
        <td>
        </td>
        <td>
        </td>
        <td>
        </td>
        <td>
        </td>
        <td>
        </td>
    </tr>
</table>
<table border="0" cellpadding="3" cellspacing="0" >    
    <tr>
        <td>
        </td>
        <td>
            <asp:ImageButton ID="Image5" runat="server" BorderColor="#5282D4" BorderStyle="Solid" BorderWidth="1px"
            ImageUrl="~/Images/Wizard/accueil.gif" 
            ToolTip="Rédigez l'email aux interviewés" 
            onclick="ButtonEditerPageAccueil_Click" />
        </td>
        <td>
        </td>
        <td>
        </td>
        <td>
        </td>
        <td>
            <asp:ImageButton ID="Image7" runat="server" BorderColor="#5282D4" BorderStyle="Solid" BorderWidth="1px"
                ImageUrl="~/Images/Wizard/Bar-2DRectangles.gif"
                ToolTip="Statistiques sur les votants" 
                onclick="ButtonStatistiques_Click" />
        </td>
    </tr>
    <tr>
        <td class="TableAccueilButtonStyle">
        </td>
        <td class="TableAccueilButtonStyle">
        <asp:Button ID="ButtonEditerPageAccueil" CssClass="ButtonAccueilStyle"
            runat="server" Text="Editez la page d'accueil de vos interviewés" 
            ToolTip="Editez la page d'accueil des interviewés" 
            onclick="ButtonEditerPageAccueil_Click"/>
        </td>
        <td>
            &nbsp;
        </td>
        <td class="TableAccueilButtonStyle">
        </td>
        <td class="TableAccueilButtonStyle">
            &nbsp;</td>
        <td class="TableAccueilButtonStyle">
        <asp:Button ID="ButtonStatistiques" CssClass="ButtonAccueilStyle" 
            runat="server" Text="Dépouillez les statistiques" 
            ToolTip="Statistiques sur les votants" 
            onclick="ButtonStatistiques_Click"/>
        </td>
    </tr>
</table>
</div>
</asp:Content>

