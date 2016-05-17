<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" ValidateRequest="false" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="List.aspx.cs" Inherits="Poll_List" Title="Liste des Questionnaires" %>

<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<a name="HautDePage"></a>
<div id="DivPageStyle">
<h3>Visualisez et Modifiez vos Questionnaires</h3>

    <!-- Aide en ligne sur le Formulaire -->
    <asp:Panel ID="PanelAide" runat="server" class="PanelAideStyle" Height="0">
    <table class="TableCollapsePanel">
        <tr>
            <td>
            Vous visualisez vos Questionnaires et vous pouvez les modifier.<br />
            <br />
            Les zones de textes sont des zones modifiables que vous pouvez modifier directement.<br />
            Vous devez taper sur <b>"Entrée"</b> à  chaque modification d'un objet pour qu'elle soit prise en compte.<br />
            Pour <b>supprimer</b> une Question, une Réponse, un Message, effacez le libellé et valider en tapant <b>"Entrée"</b>.<br />
            <br />
            Les boutons de contrôle :<br />
            <ul class="UlStyle">
                <li>Le stylet vous permet de modifier la Question et ses Réponses.</li>
                <li>Le saut de Page, vous mettez vos Questions dans des pages.</li>
                <li>Le tableau vous permet de présenter vos Questions sous forme matricielle (en tableau).</li>
                <li>La roue crantée vous permet de visualiser le Questionnaire avec les styles et de tester vos Questions.</li>
                <li>Le point d'interrogation vous permet d'accéder à l'administration des Réponses.</li>
                <li>La case à cocher vous permet de sélectionner la Question, ou la Réponse, pour ajouter ou retirer n rang par Rang+ et Rang-.</li>
            </ul>
            <br />
            Les Questions sont triées par leur <b>Rang</b> (carré bleu avec un nombre).<br />
            Pour réorganiser vos Questions, vous modifiez leur Rang.<br />
            Même chose pour les Réponses, elles sont triées par leur <b>Rang</b>.<br />
            Pour réorganiser ou programmer votre Questionnaire, utilisez le menu : <b>Question -> Programmer</b><br />
            <br />
            Pour présenter vos Questions sous forme <b>tableau</b> (questions matricielles), vous devez les présenter dans des pages et insérer des <b>sauts de page</b>. Sinon votre questionnaire reste en mode Question par Question.<br />
            <br />
            Dans la zone "<b>Réponse :</b>", vous pouvez entrer créer plusieurs Réponses de même type à la fois, en les séparant par des <b>;</b><br />
            <br />
            Pour un Questionnaire long, vous pouvez paginer les Questions en modifiant, menu <b>Administrer</b> onglet <b>Options</b>, "Nombre de questions par page :".
            </td>
        </tr>
    </table>
    </asp:Panel>
    
    <table class="TableQuestionStyle" cellpadding="4" width="100%" border="0" >
        <tr>
            <td width="16px">
                <ajaxToolkit:CollapsiblePanelExtender ID="cpe1" runat="Server" 
                    AutoCollapse="false"  
                    AutoExpand="false"
                    TargetControlID="PanelAide"
                    ExpandControlID="PanelControl"
                    CollapseControlID="PanelControl" 
                    Collapsed="true"
                    ImageControlID="Image1"    
                    ExpandedImage="~/App_Themes/Sodevlog/Images/help.gif"
                    CollapsedImage="~/App_Themes/Sodevlog/Images/help.gif"
                    SuppressPostBack="true"
                    SkinID="CollapsiblePanel" />  
                <asp:Panel ID="PanelControl" runat="server" CssClass="CollapsePanelHeader"> 
                    <img src="../App_Themes/Sodevlog/Images/help.gif" border="0" alt ="Cliquez ici pour afficher l'aide"/>
                </asp:Panel>
            </td>
            <td>
                <asp:ImageButton ID="ImageButtonPrint" runat="server" 
                    ImageUrl="~/Images/print.png"
                    onclick="ImageButtonPrint_Click" 
                    ToolTip="Formulaire d'impression"/>
            </td>                    
            <td width="180px" align="right">
                <label class="LabelStyle">Questionnaires : </label>
            </td>
            <td>
                <usr:DropDownListQuestionnaires ID="DropDownListQuestionnaire" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListQuestionnaire_SelectedIndexChanged" />
            </td>
        </tr>
    </table>
  
    <table border="0" cellpadding="5" cellspacing="0">
        <tr>
            <td style="padding-left:27px">
            <asp:Label ID="LabelValider" ForeColor="blue" runat="server" Text="Valider" ></asp:Label>
            &nbsp;<asp:Label ID="LabelFin" ForeColor="red" runat="server" Text="Fin" ></asp:Label>
            &nbsp;<asp:Label ID="LabelBloque" ForeColor="red" runat="server" Text="Clôturé" ></asp:Label>
            </td>
        </tr>
    </table>     

    <table border="0" cellpadding="10px" cellspacing="0" width="100%">
    <tr>
        <td align="left" style="width:65px;">
            <asp:Button ID="ButtonRangPlusHaut" runat="server" Text="Rang+" CssClass="ButtonStyle" ToolTip="Ajouter n au rang des Questions et des Réponses sélectionnées" OnClick="ButtonRangPlusMoins_Click" />
        </td>
        <td align="left" style="width:65px;">
            <asp:Button ID="ButtonRangMoinsHaut" runat="server" Text="Rang-" CssClass="ButtonStyle" ToolTip="Retirer n au rang des Questions et des Réponses sélectionnées" OnClick="ButtonRangPlusMoins_Click" />
        </td>
        <td align="left">
            <asp:TextBox ID="TextRangMoinsPlusHaut" runat="server" Width="30px" ToolTip="n rang à ajouter ou retirer" Text="1" />
        </td>
        <td align="right">
            <asp:Panel ID="PanelPagerHaut" runat="server" Visible="false" >
                <asp:Button ID="ButtonPagePrecedenteHaut" CssClass="ButtonStyle" runat="server" Text="<<<" 
                    ToolTip="Page de Questions précedente" onclick="ButtonPagePrecedente_Click" />
                <asp:Label ID="LabelPageCouranteHaut" runat="server"></asp:Label>
                <asp:Button ID="ButtonPageSuivanteHaut" CssClass="ButtonStyle" runat="server" Text=">>>" 
                    ToolTip="Page de Questions suivante" onclick="ButtonPageSuivante_Click" />
            </asp:Panel>
        </td>
    </tr>
    </table>

    <asp:DataList ID="DataListQuestion" runat="server" Width="100%">
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
            <asp:Panel ID="PanelSautPageEdit" runat="server" Visible="false" CssClass="PanelSautPage" >
                <table>
                <tr>
                    <td width="55px">
                        <hr class="HrSautPageStyle" />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="TextBoxSautPage" OnTextChanged="TextBoxSautPage_TextChanged" CssClass="TextBoxSautPageStyle" runat="server" Text='<%# Eval("SautPage") %>' />
                    </td>
                    <td>
                        <asp:Button ID="ButtonSautPageOk" CssClass="ButtonControlStyle" runat="server" Text="Ok" 
                            ToolTip="Modifier le libellé du Saut de page" onclick="ButtonSautPageOk_Click" />
                    </td>
                    <td>
                        <span class="SpanHyperLinkReponseStyle">
                            <asp:ImageButton ID="ImageButtonSautPageSupprimer" CssClass="HyperLinkTesterStyle" runat="server" ImageUrl="~/Images/Delete.gif" ToolTip="Supprimer le Saut de page" OnClick="ImageButtonSautPageSupprimer_Click"/>
                        </span>
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
            <asp:Panel ID="PanelTableauEdit" runat="server" Visible="false" CssClass="PanelSautPage" >
                <table>
                <tr>
                    <td width="55px">
                        <hr class="HrTableauStyle" />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="TextBoxTableauEdit" CssClass="TextBoxTableauStyle" runat="server" 
                            Text='<%# Eval("Tableau") %>' 
                            OnTextChanged="TextBoxTableau_TextChanged" />
                    </td>
                    <td>
                        <asp:Button ID="Button1" CssClass="ButtonControlStyle" runat="server" Text="Ok" 
                            ToolTip="Modifier le libellé du Tableau" onclick="ButtonTableauOk_Click" />
                    </td>
                    <td>
                        <asp:Button ID="Button2" CssClass="ButtonControlStyle" runat="server" Text="Fin" 
                            ToolTip="Insérer une fin de tableau" onclick="ButtonTableauFinOk_Click" />
                    </td>
                    <td>
                        <asp:Button ID="Button3" CssClass="ButtonControlStyle" runat="server" Text="Clas" 
                            ToolTip="Définir ou non comme tableau de classement (colonnes mutuellement exclusives)" onclick="ButtonTableauClassementOk_Click" />
                    </td>
                    <td>
                        <span class="SpanHyperLinkReponseStyle">
                            <asp:ImageButton ID="ImageButton1" CssClass="HyperLinkTesterStyle" runat="server" 
                                ImageUrl="~/Images/Delete.gif" 
                                ToolTip="Supprimer le Tableau" 
                                OnClick="ImageButtonTableauSupprimer_Click"/>
                        </span>
                    </td>
                </tr>
                </table>
            </asp:Panel>
            
            <!-- Les boutons de controls, la Question, l'Instruction, le Message -->
            <div class="DivListQuestionStyle">
            <table border="0" width="100%">
            <tr>
                <td class="TdLabelListUserQuestionStyle" width="auto">
                    <table border="0" cellpadding="1" cellspacing="3" width="auto">
                    <tr>
                        <td width="195px">
                            <asp:Panel ID="PanelClient1" runat="server" ToolTip="Boutons de contrôle" Visible='<%# User.IsInRole( "Client" ) == true || User.IsInRole( "Administrateur" ) == true %>'>
                                <span class="SpanHyperLinkTesterStyle">
                                    <asp:HyperLink ID="HyperLink5" runat="server" CssClass="HyperLinkTesterStyle" 
                                    ToolTip="Modifier la Question et ses Réponses"
                                    ImageUrl="~/Images/EditBleu.gif" 
                                    NavigateUrl='<%# "~/Wizard/Question.aspx?PollQuestionId=" + Eval("PollQuestionId")%>' />
                                </span>
                                <span class="SpanHyperLinkTesterStyle">
                                    <asp:ImageButton ID="ImageButtonInsererSautPage" CssClass="HyperLinkTesterStyle" runat="server" 
                                    ImageUrl="~/Images/saut_page.gif" 
                                    ToolTip="Insérez un saut de page" 
                                    OnClick="ImageButtonInsererSautPage_Click"/>
                                </span>
                                <span class="SpanHyperLinkReponseStyle">
                                    <asp:ImageButton ID="ImageButtonInsererTableau" CssClass="HyperLinkTesterStyle" runat="server" 
                                    ImageUrl="~/Images/tableau.gif"
                                    ToolTip="Insérez un tableau" 
                                    OnClick="ImageButtonInsererTableau_Click"/>
                                </span>
                                <span class="SpanHyperLinkTesterStyle">
                                    <asp:HyperLink ID="HyperLink3" 
                                    CssClass="HyperLinkTesterStyle" 
                                    runat="server" ImageUrl="~/Images/tester16.gif"  
                                    ToolTip="Tester le Questionnaire à partir de cette question" 
                                    NavigateUrl='<%# "~/Poll/Questionnaire.aspx?PollQuestionId=" + Eval("PollQuestionId") + "&t=1" %>' 
                                    Text='<%# Eval("Rank") %>' />
                                </span>
                                <span class="SpanHyperLinkTesterStyle">
                                    <asp:HyperLink ID="HyperLink2" runat="server" CssClass="HyperLinkReponseAdministrerStyle" Width="18px" 
                                    ToolTip="Administrer les réponses" 
                                    NavigateUrl='<%# "~/Poll/Answers.aspx?PollId=" + Eval("PollQuestionId") + "&t=1" %>' 
                                    Text="?" />
                                </span>
                                <asp:CheckBox ID="CheckBoxRangPlusMoins" runat="server"
                                    ToolTip="Sélectionner la Question pour ajouter ou retirer n rang"/>
                                <span>
                                    <asp:TextBox ID="TestBoxRank" runat="server" CssClass="TextBoxRankStyle" Width="33px" 
                                        ToolTip="Modifier le rang de la question" 
                                        Text='<%# Eval("Rank") %>' 
                                        OnTextChanged="TestBoxRank_TextChanged" />
                                </span>
                            </asp:Panel>
                            
                            <asp:Panel ID="PanelClientNon2" runat="server"  Visible='<%# User.IsInRole( "Client" ) == false && User.IsInRole( "Administrateur" ) == false %>'>
                                <asp:Label ID="Labelonsenfou1" Width="33px" CssClass="HyperLinkReponseStyle" runat="server" Text='<%# Eval("Rank") %>'/>
                            </asp:Panel>

                        </td>
                        <td class="TdTextBoxQuestionStyle">
                            <!-- Question -->
                            <asp:TextBox ID="TextBoxQuestion" runat="server" 
                                    CssClass="TextBoxListUserQuestionStyle"
                                    Width="100%"
                                    ToolTip="Modifier ou Supprimer la question" 
                                    Text='<%# Eval("Question") %>' 
                                    OnTextChanged="TextBoxQuestion_TextChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="Label1" CssClass="LabelListUserQuestionChoixSimpleStyle" runat="server" Text=" Choix Simple " Visible='<%# (bool)Eval("ChoixMultiple") == false %>'/>
                            <asp:Label ID="Label4" CssClass="LabelListUserQuestionChoixMultipleStyle" runat="server" Text=" Choix Multiple " Visible='<%# (bool)Eval("ChoixMultiple") %>'/>
                            <asp:Label ID="Label2" CssClass="LabelListUserQuestionChoixMultipleStyle" runat="server" Text='<%# "(" + Eval("ChoixMultipleMin") + "/" + Eval("ChoixMultipleMax") + ")" %>' Visible='<%# (bool)Eval("ChoixMultiple") && ((int)Eval("ChoixMultipleMin") > 0) && ((int)Eval("ChoixMultipleMax") > 0)%>'/>
                            <asp:Label ID="Label3" CssClass="LabelListUserQuestionObligatoireStyle" runat="server" Text=" Obligatoire " Visible='<%# (bool)Eval("Obligatoire") %>'/>
                            <asp:Label ID="Labelonsenfou5" CssClass="LabelListUserQuestionFinStyle" runat="server" Text=" Fin " Visible='<%# (bool)Eval("Fin") %>'/>
                        </td>
                    </tr>
                    </table>
                    
                    <!-- BUG GRAPHIQUE -->
                    <table border="0" cellpadding="1" cellspacing="0">
                    <tr>
                        <td valign="middle" style="padding-left:30px;padding-right:10px">
                            <asp:TextBox ID="TextBoxInstruction" runat="server" 
                                CssClass="TextBoxListUserQuestionInstructionStyle" 
                                ToolTip="Modifier l'instruction de la Question" 
                                Width='<%# Eval("Instruction").ToString() != string.Empty ? new Unit("100%") : new Unit(130) %>' 
                                Text='<%# Eval("Instruction") %>' 
                                OnTextChanged="TextBoxInstruction_TextChanged" />
                        </td>
                    </tr>
                    </table>
                    
                    <!-- BUG GRAPHIQUE si la taille est fixee par exemple : width="130px" comme si dessus, et si on met un accent alors ca pete !!-->
                    <!-- malheusement il n'y a pas de solution, au dessus ca marche, car dans l'instruction ya jamais d'accent !!-->
                    <table border="0" cellpadding="1" cellspacing="0">
                    <tr>
                        <td valign="middle" style="padding-left:30px;padding-right:10px">
                            <asp:TextBox ID="TextBoxMessageUtilisateur" runat="server" 
                                CssClass="TextBoxListUserMesssageStyle" 
                                ToolTip="Modifier le message à l'utilisateur"
                                Width='<%# Eval("Message").ToString() != string.Empty ? new Unit("100%") : new Unit(180) %>' 
                                Text='<%# Eval("Message") %>' 
                                OnTextChanged="TextBoxMessageUtilisateur_TextChanged" />
                        </td>
                    </tr>
                    </table>
                    
                </td>
            </tr>
            </table>
            <asp:DataList ID="DataListReponse" runat="server" Width="100%">
                <ItemTemplate>
                    <asp:HiddenField runat="server" ID="PollAnswerId" Value='<%# Eval("PollAnswerId") %>' />
                    <table border="0" width="100%">
                    <tr>
                        <td align="center" width="16px">
                            <asp:Label ID="LabelVote" CssClass="LabelQuestionEnCoursVoteStyle"  runat="server" />
                        </td>
                        <td class="TdLabelListUserReponseStyle">
                            <asp:CheckBox ID="CheckBoxRangPlusMoins" runat="server"
                                ToolTip="Sélectionner la Réponse pour ajouter ou retirer n rang"/>
                            <asp:TextBox ID="TestBoxReponseRank" runat="server" CssClass="TextBoxReponseRankStyle" ToolTip="Modifier le rang de la réponse" Text='<%# Eval("Rank") %>' OnTextChanged="TestBoxReponseRank_TextChanged" />
                            <asp:TextBox ID="TextBoxScore" runat="server" CssClass="TextBoxScoreStyle" 
                            ToolTip="Modifier le score de la réponse" 
                            Text='<%# Eval("Score") == null ? "" : Eval("Score") %>' 
                            Visible='<%# Eval("TypeReponse").ToString() == "Choix" %>'
                            OnTextChanged="TextBoxScore_TextChanged" />
                            &nbsp;-&nbsp;
                            <asp:TextBox ID="TextBoxReponse" runat="server" CssClass="TextBoxReponseStyle" Width="60%" ToolTip="Modifier ou Supprimer la réponse" Text='<%# Eval("Answer") %>' OnTextChanged="TextBoxDataListReponse_TextChanged" />
                            <asp:Label ID="LabelReponseType" CssClass="LabelListBlueStyle" runat="server" Text='<%# Eval("TypeReponse") %>' Visible='<%# Eval("TypeReponse").ToString() != "Choix" %>'/>
                            <asp:Label ID="LabelReponseTextuelleObligatoire" CssClass="LabelListRedStyle" runat="server" Text="Obligatoire" Visible='<%# Eval("Obligatoire") %>'/>
                        </td>
                    </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
            <!-- Ajouter des reponses -->
            <table>
                <tr>
                    <td width="55px">
                        &nbsp;
                    </td>
                    <td>
                        <label title="Ajouter des réponses">Réponses :</label>
                    </td>
                    <td>
                        <asp:TextBox ID="TextBoxReponse" runat="server" CssClass="TextBoxAjouterReponseStyle" Width="350px" 
                            ToolTip="Ajouter une réponse à la Question, entrez plusieures réponses séparées par des ;" 
                            OnTextChanged="TextBoxReponse_TextChanged" />
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownListTypeReponse" CssClass="TextBoxAjouterReponseStyle" runat="server" 
                            ToolTip="Choisir un Type de Réponse" />
                    </td>
                    <td>
                       <asp:CheckBox ID="CheckBoxObligatoire" Text=" Obligatoire" runat="server" />
                    </td>
                </tr>
            </table>
            </div>
        </ItemTemplate>
    </asp:DataList>

    <table border="0" cellpadding="10px" cellspacing="0" width="100%">
    <tr>
        <td align="left" style="width:65px;">
            <asp:Button ID="ButtonRangPlusBas" runat="server" Text="Rang+" CssClass="ButtonStyle" ToolTip="Ajouter n au rang des Questions et des Réponses sélectionnées" OnClick="ButtonRangPlusMoins_Click" />
        </td>
        <td align="left" style="width:65px;">
            <asp:Button ID="ButtonRangMoinsBas" runat="server" Text="Rang-" CssClass="ButtonStyle" ToolTip="Retirer n au rang des Questions et des Réponses sélectionnées" OnClick="ButtonRangPlusMoins_Click" />
        </td>
        <td align="left">
            <asp:TextBox ID="TextRangMoinsPlusBas" runat="server" Width="30px" ToolTip="n rang à ajouter ou retirer" Text="1" />
        </td>
        <td align="right">
            <asp:Panel ID="PanelPagerBas" runat="server" Visible="false" >
                <asp:Button ID="ButtonPagePrecedenteBas" CssClass="ButtonStyle" runat="server" Text="<<<" 
                ToolTip="Page de Questions précedente" onclick="ButtonPagePrecedente_Click" />
                <asp:Label ID="LabelPageCouranteBas" runat="server"></asp:Label>
                <asp:Button ID="ButtonPageSuivanteBas" CssClass="ButtonStyle" runat="server" Text=">>>" 
                ToolTip="Page de Questions suivante" onclick="ButtonPageSuivante_Click" />
            </asp:Panel>
        </td>
    </tr>
    </table>
        
    <a name="BasDePage" id="AncreBasDePage" runat="server"></a>
    
    <table cellpadding="2" width="100%">
        <tr>
            <td height="60px" align="center">
                <UserControl:RolloverButton ID="RolloverButtonTestez" runat="server" Text="Tester" OnClick="RolloverButtonTestez_Click" ToolTip="Testez le Questionnaire" />
            &nbsp;<UserControl:RolloverButton ID="RolloverButtonProgrammer" runat="server" 
                    Text="Programmer" OnClick="RolloverButtonProgrammer_Click" 
                    ToolTip="Programmer le Questionnaire" />
            &nbsp;<UserControl:RolloverButton ID="RolloverButtonAjouterQuestion" runat="server" 
                    Text="Ajouter" OnClick="RolloverButtonAjouterQuestion_Click" 
                    ToolTip="Ajouter des Questions" />
             </td>
        </tr>
    </table>
<asp:Button ID="DefaultButton" runat="server" Visible="true" Width="0" Height="0" OnClick="DefaultButton_Click"/>
</div>
</asp:Content>