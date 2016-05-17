<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="QuestionnaireEnCours.aspx.cs" Inherits="Poll_QuestionnaireEnCours" Title="Liste des Questions en cours" %>
<%@ Register TagPrefix="ucwc" TagName="WebContent" Src="~/UserControl/WebContent.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
        <div class=DivPageQuestionnaireEnCoursStyle>
            <h3>Vos Questions et vos Réponses</h3>
            <ucwc:WebContent ID="WebContent1" runat="server" Section="PageQuestionnaireEnCours" /> 
            <asp:Label ID="LabelDataListVide" runat="server" Visible="false" CssClass="LabelStyle" Text="Il n'y a pas de Réponse en cours pour ce Questionnaire."></asp:Label>
            <asp:DataList ID="DataListQuestion" runat="server" Width="100%">
                <ItemTemplate>
                    <asp:HiddenField runat="server" ID="PollQuestionId" Value='<%# Eval("PollQuestionId") %>' />
                    <table border="0" width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="LabelPageID" CssClass="LabelSautPageStyle" runat="server" Text='<%# Eval("SautPage") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" CssClass="LabelTableauStyle" runat="server" Text='<%# Eval("Tableau") %>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="TdLabelListUserQuestionStyle">
                            <span class="SpanHyperLinkQuestionEnCoursStyle">
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("Question") %>' Visible='<%# SessionState.Questionnaire.Valider == false %>' />
                            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="HyperLinkQuestionEnCoursStyle" 
                                Visible='<%# SessionState.Questionnaire.Valider == true %>' 
                                ToolTip="Retourner à la Question" 
                                NavigateUrl='<%# "~/Poll/Questionnaire.aspx?PollQuestionId=" + Eval("PollQuestionId")%>' 
                                Text='<%# Eval("Question") %>' />
                            </span>
                            <asp:Panel ID="PanelMessage" runat="server" Visible='<%# Eval("Message").ToString() != "" %>'> 
                                <table border="0" cellpadding="3px" cellspacing="0">
                                <tr>
                                    <td valign="middle" style="padding-left:30px; text-align:center" height="20px">
                                        <asp:Label ID="Label5" CssClass="LabelListUserQuestionMessageStyle" runat="server" Text='<%# Eval("Message") %>' />
                                    </td>
                                </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    </table>
                    <asp:DataList ID="DataListReponse" runat="server" Width="100%">
                        <ItemTemplate>
                            <table border="0" width="100%">
                            <tr>
                                <td align="center" width="16px">
                                    <asp:Label ID="LabelVote" CssClass="LabelQuestionEnCoursVoteStyle" runat="server" />
                                </td>
                                <td class="TdLabelListUserReponseStyle">
                                    <asp:Label ID="LabelReponse" CssClass="LabelListUserReponseStyle" runat="server" Text='<%# Eval("Answer") %>'/>
                                    <asp:Label ID="LabelVoteTexte" runat="server" />
                                </td>
                            </tr>
                            </table>
                        </ItemTemplate>
                    </asp:DataList>
                </ItemTemplate>
            </asp:DataList>
            
            <table cellpadding="2" width="100%">
                <tr>
                    <td height="60" align="center">
                        <UserControl:RolloverButton ID="ButtonValiderQuestionnaire" runat="server" OnClick="ButtonValiderQuestionnaire_Click" Visible="false" Text="Valider" ToolTip="Enregistrez vos réponses" />
                        <UserControl:RolloverLink ID="RolloverLinkRetour" runat="server" NavigateURL="~/Poll/Questionnaire.aspx" Text="Retour" ToolTip="Retournez au Questionnaire" />
                    </td>
                </tr>
            </table>
            
        </div>

        <table style="border:none" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="center" height="40">
                    <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
                </td>
            </tr>
        </table>

    </div>
</asp:Content>