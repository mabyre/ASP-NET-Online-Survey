<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false" MaintainScrollPositionOnPostback="false" AutoEventWireup="true" CodeFile="Publication.aspx.cs" Inherits="Page_QuestionnairePublication" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="DivQuestionnairePublication">
    <a name="HautDePage"></a>
    
    <asp:Panel ID="PanelQuestionnairePasDePublication" runat="server" Visible="false">
    <table style="border:solid 1px" cellpadding="15px" cellspacing="0" width="80%">
        <tr>
            <td height="180px">
               <h3>Pas de publication pour ce Questionnaire.</h3>
            </td>
        </tr>
    </table>
    </asp:Panel>
  
    <asp:Panel ID="PanelQuestionnairePublication" runat="server" Visible="true">
    <h3><asp:Label ID="LabelTitre" runat="server"/></h3>

    <table border="0" cellpadding="10" height="30">
        <tr>
            <td>
                <label class="LabelListUserStyle">Interviewés : </label>
                <asp:Label ID="LabelNombreContacts" runat="server" ></asp:Label>
            </td>
            <td>
                <label class="LabelListUserStyle">Réponses : </label>
                <asp:Label ID="LabelVotes" runat="server" ></asp:Label>
            </td>
        </tr>
    </table>     

    <table border="0" cellpadding="5px" width="80%">
    <tr>
        <td align="right" width="150px"></td>
        <td></td>
    </tr>
    <tr>
        <td align="right" valign="top" title="Cliquez sur une réponse pour afficher la liste de ceux qui ont répondu">
            Qui a répondu :&nbsp;
        </td>
        <td align="left">
            <asp:Panel ID="PanelReponses" runat="server" Visible="false">
            </asp:Panel>
            <asp:ListBox ID="ListBoxQui" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListBoxQui_SelectedIndexChanged"></asp:ListBox>
            <asp:Label ID="LabelListBoxQuiCount" runat="server" Font-Bold="true" ForeColor="Green"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right">
        </td>
        <td class="TdLabelInvitationStyle" align="left" >
            <asp:Panel ID="PanelSousPopulation" runat="server" Visible="false" Height="23px" >
                <asp:Button ID="ButtonEffacerQuiARepondu" CssClass="ButtonStyle" runat="server" Text="Effacer" OnClick="ButtonEffacerQuiARepondu_Click" ToolTip="Effacer la sous-population et afficher les statistiques complets" />
                <asp:Button ID="ButtonAfficherSousPopulation" CssClass="ButtonStyle" runat="server" Text="Afficher" OnClick="ButtonAfficherSousPopulation_Click" ToolTip="Afficher les satistiques de cette sous-population" />
                <span class="SpanHyperLinkStyle"  >
                    <a href="#ReponsesInterviewes" class="HyperLinkQuestionEnCoursStyle" title="Voir les réponses">Réponses</a>
                </span>
            </asp:Panel>
        </td>
    </tr>    
    <tr visible="false" runat="server">
        <td align="right" valign="top" title="Choisir un votant pour afficher ses votes">
            Afficher les votes :&nbsp;
        </td>
        <td align="left">
            <asp:DropDownList ID="DropDownListQui" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListQui_SelectedIndexChanged"></asp:DropDownList>
        </td>
    </tr>
    <tr visible="false" runat="server">
        <td align="right" valign="top" title="Afficher ou non les réponses textuelles">
            Réponses textuelles :&nbsp;
        </td>
        <td align="left">
            <asp:CheckBox ID="CheckBoxAfficherReponseTextuelle" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBoxAfficherReponseTextuelle_CheckedChanged" />
        </td>
    </tr>
    <tr visible="false" runat="server">
        <td align="right" valign="top" title="Afficher ou non les dates des votes">
            Dates des votes :&nbsp;
        </td>
        <td align="left">
            <asp:CheckBox ID="CheckBoxAfficherDateVote" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBoxAfficherDateVote_CheckedChanged" />
        </td>
    </tr>
    </table>
    <table style="border:solid 1px" cellpadding="10px" cellspacing="0" width="80%">
    <tr>
        <td>
            <table border="0" width="100%">
                <tr>
                    <td width="20px">
                    </td>
                    <td valign="top" align="left">
                        <UserControl:QuestionnairePublication ID="QuestionnairePublication" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    </table>
    
    <table border="0" cellpadding="5px" cellspacing="0" width="80%">
    <tr>
        <td height="30px">
            <a name="ReponsesInterviewes">Réponses des interviewés</a>
        </td>
    </tr>
    <tr>
        <td align="left">
            <asp:Panel ID="PanelReponsesEnBas" runat="server" Visible="true">
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td height="20px">
            <span class="SpanHyperLinkStyle">
            <a href="#HautDePage" class="HyperLinkStyle">Haut</a>
            </span>
        </td>
    </tr>
    </table>
    </asp:Panel>
</div>    
</asp:Content>
