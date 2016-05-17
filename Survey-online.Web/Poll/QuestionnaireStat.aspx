<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="QuestionnaireStat.aspx.cs" Inherits="Poll_QuestionnaireStat" Title="Statistiques" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" Height="60px">
        Questionnaires :&nbsp;
        <asp:DropDownList ID="DropDownListQuestionnaires" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListQuestionnaires_SelectedIndexChanged"></asp:DropDownList>
        <br />
    </asp:Panel>
    <table cellpadding="10px" cellspacing="0" width="80%">
    <tr>
    <td>
        <asp:Button ID="ButtonStatistiques" CssClass="ButtonStyle" runat="server" Text="Toutes" OnClick="ButtonStatistiques_Click" />
    </td>
    </tr>
    </table>
    <table style="border:solid 1px" cellpadding="10px" cellspacing="0" width="80%">
    <tr>
        <td>
            <table border="0" width="100%">
                <tr>
                    <td valign="top" align="center">
                        <UserControl:QuestionnaireStatControl ID="PollControl1" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    </table>
</asp:Content>
