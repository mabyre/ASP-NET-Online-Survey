<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QuestionnaireExempleControl.ascx.cs" Inherits="UserControl_QuestionnaireExempleControl" %>

<div id="body">

    <div id="DivPageStyle" class="ClassDivPageStyle" >

    <table border="0" cellpadding="0">
        <tr>
            <td>
                <UserControl:DropDownListQuestionnaire ID="DropDownListQuestionnaire" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListQuestionnaire_SelectedIndexChanged" />
            </td>
        </tr>
    </table>
        
    <asp:Panel ID="PanelQuestionnaireExempleControl" runat="server" >
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td style="padding-left:27px">
                <asp:Label ID="LabelValider" ForeColor="blue" runat="server" Text="Valider" ></asp:Label>
                &nbsp;<asp:Label ID="LabelFin" ForeColor="red" runat="server" Text="Fin" ></asp:Label>
                &nbsp;<asp:Label ID="LabelBloque" ForeColor="red" runat="server" Text="Bloqué" ></asp:Label>
                </td>
            </tr>
        </table>     

        <asp:DataList ID="DataListQuestion" runat="server" Width="100%" >
            <ItemTemplate>
                <asp:HiddenField runat="server" ID="PollQuestionId" Value='<%# Eval("PollQuestionId") %>' />
                <asp:Panel ID="PanelSautPage" runat="server" Visible='<%# Eval("SautPage") != string.Empty %>' CssClass="PanelSautPage" >
                    <table border="0" cellpadding="2" cellspacing="3" width="100%">
                    <tr>
                        <td width="55px">
                            <hr class="HrSautPageStyle" />
                        </td>
                        <td align="left">
                            <asp:Label ID="LabelSautPage" CssClass="LabelSautPageStyle" runat="server" Text='<%# Eval("SautPage") %>' />
                        </td>
                    </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PanelTableau" runat="server" Visible='<%# Eval("Tableau") != string.Empty %>' CssClass="PanelTableau" >
                    <table border="0" cellpadding="2" cellspacing="3" width="100%">
                    <tr>
                        <td width="55px">
                            <hr class="HrTableauStyle" />
                        </td>
                        <td align="left">
                            <asp:Label ID="LabelTableau" CssClass="LabelTableauStyle" runat="server" Text='<%# Eval("Tableau") %>' />
                        </td>
                    </tr>
                    </table>
                </asp:Panel>
                <table border="0" width="100%">
                <tr>
                    <td class="TdLabelListUserQuestionStyle">
                        <table border="0" cellpadding="0" cellspacing="5">
                        <tr>
                            <td valign="top">
                                <asp:Label ID="Labelonsenfou1" Width="33px" CssClass="HyperLinkReponseStyle" runat="server" Text='<%# Eval("Rank") %>'/>
                            </td>
                            <td>
                                <span class="SpanHyperLinkReponseStyle">
                                <asp:HyperLink ID="HyperLink5" runat="server" CssClass="HyperLinkQuestionEnCoursStyle" ToolTip="Tester le Questionnaire à partir de cette question" NavigateUrl='<%# "~/Poll/Questionnaire.aspx?PollQuestionId=" + Eval("PollQuestionId") + "&t=1" %>' Text='<%# Eval("Question") %>' />
                                </span>
                                <asp:Label ID="Label3" CssClass="LabelListUserQuestionObligatoireStyle" runat="server" Text=" Obligatoire " Visible='<%# (bool)Eval("Obligatoire") %>'/>
                                <asp:Label ID="Label4" CssClass="LabelListUserQuestionChoixMultipleStyle" runat="server" Text=" Choix Multiple " Visible='<%# (bool)Eval("ChoixMultiple") %>'/>
                                <asp:Label ID="Labelonsenfou5" CssClass="LabelListUserQuestionFinStyle" runat="server" Text=" Fin " Visible='<%# (bool)Eval("Fin") %>'/>
                            </td>
                        </tr>
                        </table>
                        <asp:Panel ID="PanelInstruction"  BorderColor="#F1F1F1" BorderStyle="Solid" BorderWidth="0px" runat="server" Visible='<%# Eval("Instruction").ToString() != "" %>'> 
                            <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="middle" style="padding-left:55px">
                                    <asp:Label ID="Label1aaaa" CssClass="LabelListUserQuestionInstructionStyle" runat="server" Text='<%# Eval("Instruction") %>' />
                                </td>
                            </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="PanelMessage" runat="server" Visible='<%# Eval("Message").ToString() != "" %>'> 
                            <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="middle" style="padding-left:55px">
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
                                <asp:Label ID="LabelVote" CssClass="LabelQuestionEnCoursVoteStyle"  runat="server" />
                            </td>
                            <td class="TdLabelListUserReponseStyle">
                                <asp:Label ID="LabelReponse" CssClass="LabelListUserReponseStyle"  runat="server" Text='<%# Eval("Rank") + " - " + Eval("Answer") %>'/>
                                <asp:Label ID="LabelReponseType" CssClass="LabelListBlueStyle" runat="server" Text='<%# Eval("TypeReponse") %>' Visible='<%# Eval("TypeReponse").ToString() != "Choix" %>'/>
                                <asp:Label ID="LabelReponseTextuelleObligatoire" CssClass="LabelListRedStyle" runat="server" Text="Obligatoire" Visible='<%# Eval("Obligatoire") %>'/>
                            </td>
                        </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
            </ItemTemplate>
        </asp:DataList>

        <asp:Panel ID="PanelQuestionnaireExistant" runat="server" Visible="false">
        <table width="100%">
            <tr>
                <td height="15px">
                </td>
            </tr>
            <tr>
                <td>
                    <label class="LableStyle">Ajouter à ce questionnaire :</label> 
                </td>
            </tr>
            <tr>
                <td>
                    <usr:DropDownListQuestionnaires ID="DropDownListQuestionnaireExistant" runat="server" DefaultText="Questionnaires existants" />
                </td>
            </tr>
        </table>
        </asp:Panel>
        
        <table border="0" cellpadding="2" width="100%">
            <tr>
                <!--
                <td class="TdControlButtonStyle" height="60px" align="right" >
                    <UserControl:RolloverButton ID="RolloverButtonTestez" runat="server" Text="Testez" OnClick="RolloverButtonTestez_Click" ToolTip="Testez le Questionnaire" />
                </td>
                -->
                <td class="TdControlButtonStyle" align="center" >
                    <UserControl:RolloverButton ID="ButtonCopier" runat="server" Text="Créer" ToolTip="Copier ce Questionnaire existant" OnClick="ButtonCopier_Click"/>                
                    <UserControl:RolloverButton ID="RolloverButtonAjouter" runat="server" Text="Ajouter" ToolTip="Ajouter ces Questions à votre Questionnaire" OnClick="ButtonAjouter_Click"/>                
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    </div>
    
</div>
