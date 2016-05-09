<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" Trace="false" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="Manage.aspx.cs" Inherits="Poll_Manage" %>
<%@ Register TagPrefix="ucwc" TagName="WebContent" Src="~/UserControl/WebContent.ascx" %>

<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<a name="HautDePage"></a>
<div class="DivFullWidth">
    <asp:Panel ID="PanelTitre" runat="server">
    <h3><asp:Label ID="LabelTitre" runat="server" Text="Programmer les Questions" /></h3>
    </asp:Panel>
    
    <!-- Aide en ligne sur le Formulaire -->
    <asp:Panel ID="PanelAide" runat="server" class="PanelAideStyle" Height="0">
    <div style="text-align:left;">
    <table class="TableCollapsePanel">
        <tr>
            <td>
            Ce formulaire est <b>complexe</b> à appréhender au début. Pour effectuer des modifications simples de votre Questionnaire, préférez plutôt le formulaire <b>Questionnaire->Visualisez, Modifier</b>.<br />
            <br />
            Ce formulaire vous permet d'avoir une vision synthétique de votre Questionnaire lorsque vous désirez <b>programmer des Instructions</b> pour les Questions d'un Questionnaire programmé.<br />
            <br />
            Il vous permet de <b>réorganiser</b> facilement votre Questionnaire en modifiant le rang des Questions :<br />
            Cochez les Questions dont vous voulez modifier les rangs, cliquez sur le bouton <b>Rang+1</b> si vous désirez ajouter 1 au rang de toutes les Questions cochées.<br />
            Cliquez sur le bouton <b>Rang-1</b> si vous désirez retirer 1 au rang de toutes les Questions cochées.<br />
            <br />
            Si vous avez plusieurs Questions avec de multiples réponses, vous pouvez créer une nouvelle Question en copiant les réponses d'une Question déjà existante en la choisissant par son rang.<br />
            Vous accédez à la programmation des Réponses en cliquant sur les boutons <b>Réponses</b>.<br />
            Vous accédez aux <b>formulaires d'impression</b> de vos Questionnaires en cliquant sur les boutons Imprimantes.<br />
            </td>
        </tr>
    </table>
    </div>
    </asp:Panel>
    
    <asp:Panel ID="PanelDropDownListQuestionnaires" runat="server">
    <table class="TableQuestionStyle" cellpadding="5" width="95%">
        <tr>
            <td width="16px">
                <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" 
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
            <td align="left" class="TdCelluleIcone">
                <asp:ImageButton ID="ImageButtonPrint" runat="server" 
                    ImageUrl="~/Images/print.png"
                    onclick="ImageButtonPrint_Click" 
                    ToolTip="Formulaire d'impression avec la liste"/>
            </td>
            <td align="left" class="TdCelluleIcone">
                <asp:ImageButton ID="ImageButtonPrint2" runat="server" 
                    ImageUrl="~/Images/print.png"
                    onclick="ImageButtonPrint1_Click" 
                    ToolTip="Formulaire d'impression"/>
            </td>
            <td>
                <label class="LabelStyle">Questionnaires : </label>
                <usr:DropDownListQuestionnaires ID="DropDownListQuestionnaire" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListQuestionnaire_SelectedIndexChanged" />
            </td>
        </tr>
    </table>        
    </asp:Panel>
    
    <asp:HiddenField ID="HiddenFieldQuestionnaireID" runat="server" Visible="false" />
    <asp:SqlDataSource ID="SqlDataSourceQuestion" runat="server" ConnectionString="<%$ ConnectionStrings:QuestionnaireDB %>"
        DataSourceMode="DataSet"
        EnableCaching="false"
        DeleteCommand="DELETE FROM Bidon"
        SelectCommand="SELECT * FROM PollQuestions WHERE QuestionnaireID = @HiddenFieldQuestionnaireID ORDER BY Rank"
        InsertCommand="INSERT INTO PollQuestions (PollQuestionId, QuestionnaireID, Question, Societe, Rank, Instruction, Message, MessageHaut, SautPage, Tableau, ChoixMultiple, QuestionObligatoire, QuestionFin, CreationDate, MembreGUID, ChoixMultipleMin, ChoixMultipleMax) VALUES (@PollQuestionId, @QuestionnaireID, @Question, @Societe, @Rank, @Instruction, @Message, @MessageHaut, @SautPAge, @Tableau, @ChoixMultiple, @QuestionObligatoire, @QuestionFin, @CreationDate, @MembreGUID, @ChoixMultipleMin, @ChoixMultipleMax)"
        UpdateCommand="UPDATE PollQuestions SET Question = @Question, Rank = @Rank, Instruction = @Instruction, Message = @Message, MessageHaut = @MessageHaut, SautPage = @SautPage, Tableau = @Tableau, ChoixMultiple = @ChoixMultiple, QuestionObligatoire = @QuestionObligatoire , QuestionFin = @QuestionFin, Societe = @societe, CreationDate = @CreationDate, ChoixMultipleMin = @ChoixMultipleMin, ChoixMultipleMax = @ChoixMultipleMax WHERE [PollQuestionId] = @old_PollQuestionId"
        OldValuesParameterFormatString="old_{0}" 
        OnUpdating="SqlDataSourceQuestion_Updating" >
        <SelectParameters>
            <asp:ControlParameter Name="HiddenFieldQuestionnaireID" ControlID="HiddenFieldQuestionnaireID" PropertyName="Value" /> 
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:Panel ID="PanelGridView" runat="server" >
    <div class="DivFullWidth">
        <asp:Panel ID="PanelBoutonControl" runat="server" >
        <table border="0" cellpadding="3px" width="95%" cellspacing="0">
        <tr>
            <td align="left" width="90px" style="height: 26px">
                <asp:Button ID="ButtonRangPlusUn" runat="server" Text="Rang+1" CssClass="ButtonStyle" ToolTip="Ajouter 1 au rang des Questions sélectionnées" OnClick="ButtonRangPlusUn_Click" />
            </td>
            <td align="left" width="90px" style="height: 26px">
                <asp:Button ID="ButtonRangMoinsUn" runat="server" Text="Rang-1" CssClass="ButtonStyle" ToolTip="Retirer 1 au rang des Questions sélectionnées" OnClick="ButtonRangMoinsUn_Click" />
            </td>
            <td align="left" width="90px" style="height: 26px">
                <asp:CheckBox ID="CheckBoxChoixMultipleMinMax" runat="server" Text="ChoixMinMax" AutoPostBack="true" ToolTip="Afficher les colonnes Min et Max pour les questions multiples" OnCheckedChanged="CheckBoxChoixMultipleMinMax_CheckedChanged" />
            </td>
            <td align="left" width="90px" style="height: 26px">
                <asp:CheckBox ID="CheckBoxInstruction" runat="server" Text="Instruction" AutoPostBack="true" OnCheckedChanged="CheckBoxInstruction_CheckedChanged" />
            </td>
            <td align="left" width="80px" style="height: 26px">
                <asp:CheckBox ID="CheckBoxMessage" runat="server" Text="Message" AutoPostBack="true" OnCheckedChanged="CheckBoxMessage_CheckedChanged" />
            </td>
            <td align="left" width="80px" style="height: 26px">
                <asp:CheckBox ID="CheckBoxAlignement" runat="server" Text="Alignement" AutoPostBack="true" OnCheckedChanged="CheckBoxAlignement_CheckedChanged" />
            </td>
            <td align="left" width="70px" style="height: 26px">
                <asp:CheckBox ID="CheckBoxSociete" runat="server" Text="Société" AutoPostBack="true" OnCheckedChanged="CheckBoxSociete_CheckedChanged" />
            </td>
            <td align="left" width="60px" style="height: 26px">
                <asp:CheckBox ID="CheckBoxDate" runat="server" Text="Date" AutoPostBack="true" OnCheckedChanged="CheckBoxDate_CheckedChanged" />
            </td>
            <td align="right">
                <span class="SpanHyperLinkStyle">
                <a href="#BasDePage" class="HyperLinkStyle" title="Aller en bas de la Page">Bas</a>
                </span>
            </td>
        </tr>        
        </table>  
        </asp:Panel>          

        <div style="width:95%;">
        <asp:GridView ID="GridViewQuestion" runat="server" 
            AutoGenerateColumns="False" 
            SelectedRowStyle-BackColor="#F1F1F1" 
            Width="100%" 
            DataKeyNames="PollQuestionId" 
            DataSourceID="SqlDataSourceQuestion" 
            OnRowUpdating="GridViewQuestion_RowUpdating"
            OnRowCommand="GridViewQuestion_RowCommand" 
            OnRowUpdated="GridViewQuestion_RowUpdated" 
            OnSelectedIndexChanged="GridViewQuestion_SelectedIndexChanged" 
            OnRowDeleted="GridViewQuestion_RowDeleted" 
            OnLoad="GridViewQuestion_Load" 
            OnRowDatabound="GridViewQuestion_RowDataBound">
            <HeaderStyle CssClass="GridViewHeaderStyle"/>
            <SelectedRowStyle BackColor="#F1F1F1" ForeColor="#C1C1C1" />
            <Columns>
            
                <asp:CommandField ItemStyle-Width="60px" ShowDeleteButton="True" 
                    ShowSelectButton="false" ShowEditButton="True" ButtonType="Image" 
                    DeleteImageUrl="~/Images/Delete.gif" 
                    EditImageUrl="~/Images/EditBleu.gif" 
                    SelectImageUrl="~/Images/select.gif" 
                    UpdateImageUrl="~/Images/Save.gif" 
                    CancelImageUrl="~/Images/Annul.gif">
                    <ItemStyle Width="60px" />
                </asp:CommandField>
                
                <asp:BoundField DataField="Question" HeaderText="Question" SortExpression="Question">
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <ControlStyle Width="95%"/>
                </asp:BoundField>
                
                <asp:BoundField DataField="Rank" HeaderText="Rang" SortExpression="Rank" >
                    <ItemStyle CssClass="BoundFieldStyle" Width="40px" />
                    <ControlStyle Width="40px"/>
                </asp:BoundField>

                <asp:TemplateField HeaderText="&plusmn;">
                    <ItemStyle Width="30px" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBoxRangPlusMoinsUn" runat="server"/>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="QuestionObligatoire" HeaderText="Obligatoire" SortExpression="QuestionObligatoire" >
                    <ItemStyle CssClass="BoundFieldStyle" ForeColor="Red" Width="80px" />
                    <ControlStyle Width="40px" />
                </asp:BoundField>

                <asp:BoundField DataField="ChoixMultiple" HeaderText="Multiple" SortExpression="ChoixMultiple" >
                    <ItemStyle CssClass="BoundFieldStyle" ForeColor="Green" Width="50px" />
                    <ControlStyle Width="40px" />
                </asp:BoundField>

                <asp:BoundField DataField="ChoixMultipleMin" HeaderText="Min" SortExpression="ChoixMultipleMin" >
                    <ItemStyle CssClass="BoundFieldStyle" ForeColor="Blue" Width="50px" />
                    <ControlStyle Width="40px" />
                </asp:BoundField>

                <asp:BoundField DataField="ChoixMultipleMax" HeaderText="Max" SortExpression="ChoixMultipleMax" >
                    <ItemStyle CssClass="BoundFieldStyle" ForeColor="Blue" Width="50px" />
                    <ControlStyle Width="40px" />
                </asp:BoundField>

                <asp:BoundField DataField="QuestionFin" HeaderText="Fin" SortExpression="QuestionFin" >
                    <ItemStyle CssClass="BoundFieldStyle" ForeColor="Blue" Width="45px" />
                    <ControlStyle Width="40px" />
                </asp:BoundField>
                
                <asp:BoundField DataField="Instruction" HeaderText="Instruction" SortExpression="Instruction">
                    <ItemStyle CssClass="BoundFieldStyle" ForeColor="Green" />
                    <ControlStyle Width="90%" />
                </asp:BoundField>
                
                <asp:BoundField DataField="Message" HeaderText="Message" SortExpression="Message">
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <ControlStyle Width="90%" />
                </asp:BoundField>
                
                <asp:BoundField DataField="MessageHaut" HeaderText="Haut" SortExpression="MessageHaut">
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <ControlStyle Width="40px" />
                </asp:BoundField>

                <asp:BoundField DataField="SautPage" HeaderText="Page" SortExpression="SautPage">
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <ControlStyle Width="90%" />
                </asp:BoundField>

                <asp:BoundField DataField="Tableau" HeaderText="Tableau" SortExpression="Tableau">
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <ControlStyle Width="90%" />
                </asp:BoundField>

                <asp:BoundField DataField="Societe" HeaderText="Société" SortExpression="Societe" Visible="false">
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <ControlStyle Width="150px"/>
                </asp:BoundField>
                  
                <asp:BoundField DataField="CreationDate" HeaderText="Date" SortExpression="CreationDate" Visible="false">
                    <ItemStyle CssClass="BoundFieldDateStyle" Width="150px" />
                    <ControlStyle Width="130px"/>
                </asp:BoundField>
                
                <asp:TemplateField HeaderText="Question">
                    <ItemStyle Width="30px" HorizontalAlign="Center"/>
                    <ItemTemplate>
                        <UserControl:DropDownListGridView ID="DropDownListGridViewAlignementQuestion" CssClass="DropDownListGridViewStyle" EnableViewState="true" ToolTip="Alignement de la Question" AutoPostBack="true"  runat="server" OnSelectedIndexChanged="DropDownListAlignementQuestion_SelectedIndexChanged">
                        </UserControl:DropDownListGridView> 
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Réponse">
                    <ItemStyle Width="30px" HorizontalAlign="Center"/>
                    <ItemTemplate>
                        <UserControl:DropDownListGridView ID="DropDownListGridViewAlignementReponse" CssClass="DropDownListGridViewStyle" EnableViewState="true" ToolTip="Alignement de la Réponse" AutoPostBack="true"  runat="server" OnSelectedIndexChanged="DropDownListAlignementReponse_SelectedIndexChanged">
                        </UserControl:DropDownListGridView> 
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:HyperLinkField DataNavigateUrlFields="PollQuestionId" 
                    DataNavigateUrlFormatString="~/Poll/Answers.aspx?PollId={0}" 
                    HeaderText="Réponses" Text="&#187;&#187;&#187;" ItemStyle-CssClass="ItemStyle" >
                    <ItemStyle CssClass="ItemStyle" />
                </asp:HyperLinkField>
                
            </Columns>
            <EmptyDataTemplate>
                <table border="0" cellpadding="10px"><tr><td><b>Pas de Questions pour ce Questionnaire</b></td></tr></table>
            </EmptyDataTemplate>
        </asp:GridView>
        </div>
    </div>
    </asp:Panel>

    <!-- DetailsView Ajouter une Question -->
    <asp:Panel ID="PanelDetailsView1" runat="server" >
    <div id="columnright">
        <div style="width:95%;">
            <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" 
                DataKeyNames="PollQuestionId"
                DataSourceID="SqlDataSourceQuestion" 
                Height="60px" Width="100%" 
                DefaultMode="Insert" 
                OnItemInserting="DetailsView1_ItemInserting"  
                OnItemInserted="DetailsView1_ItemInserted">
                <Fields>
                    <asp:BoundField DataField="PollQuestionId" HeaderText="Question" InsertVisible="false" ReadOnly="true" SortExpression="PollQuestionId" />
                    <asp:TemplateField HeaderText="Question : " SortExpression="topic">
                    <HeaderStyle Width="120px" HorizontalAlign="Right" />
                        <InsertItemTemplate>
                            <table border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:TextBox ID="TextBox1" Width="80%" runat="server" Text='<%# Bind("Question") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ControlToValidate="TextBox1" ValidationGroup="Insert"
                                        ID="RequiredFieldValidator1" runat="server" ErrorMessage="Question est requise"><font style="font-weight:bold">***</font></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            </table>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rang : " SortExpression="topic">
                    <HeaderStyle Width="120px" HorizontalAlign="Right" />
                        <InsertItemTemplate>
                            <table border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:TextBox ID="TextBox3" Width="45px" runat="server" Text='<%# Bind("Rank") %>'></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Instruction : " SortExpression="topic">
                    <HeaderStyle Width="120px" HorizontalAlign="Right" />
                        <InsertItemTemplate>
                            <table border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:TextBox ID="TextBox2" Width="60%" runat="server" Text='<%# Bind("Instruction") %>'></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Message : " SortExpression="topic">
                    <HeaderStyle Width="120px" HorizontalAlign="Right" />
                        <InsertItemTemplate>
                            <table border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:TextBox ID="TextBox12" Width="60%" runat="server" Text='<%# Bind("Message") %>'></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="En Haut : " SortExpression="topic">
                    <HeaderStyle Width="120px" HorizontalAlign="Right" />
                        <InsertItemTemplate>
                            <table border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:CheckBox ID="CheckBox123546" runat="server" Checked='<%# Bind("MessageHaut") %>'></asp:CheckBox>
                                </td>
                            </tr>
                            </table>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Saut de Page : " SortExpression="topic">
                    <HeaderStyle Width="120px" HorizontalAlign="Right" />
                        <InsertItemTemplate>
                            <table border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:TextBox ID="TextBox12233" Width="60%" runat="server" Text='<%# Bind("SautPage") %>'></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tableau (fin tableau): " SortExpression="topic">
                    <HeaderStyle Width="120px" HorizontalAlign="Right" />
                        <InsertItemTemplate>
                            <table border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:TextBox ID="TextBox12235" Width="60%" ToolTip="Pour entrer une fin de tableau : fin tableau" runat="server" Text='<%# Bind("Tableau") %>'></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Choix multiple : " SortExpression="topic">
                    <HeaderStyle Width="120px" HorizontalAlign="Right" />
                        <InsertItemTemplate>
                            <table border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:CheckBox ID="CheckBox12354" runat="server" Checked='<%# Bind("ChoixMultiple") %>'></asp:CheckBox>
                                </td>
                            </tr>
                            </table>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Choix Min : " SortExpression="topic">
                    <HeaderStyle Width="120px" HorizontalAlign="Right" />
                        <InsertItemTemplate>
                            <table border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:TextBox ID="TextBox356482" Width="45px" runat="server" Text='<%# Bind("ChoixMultipleMin") %>'></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Choix Max : " SortExpression="topic">
                    <HeaderStyle Width="120px" HorizontalAlign="Right" />
                        <InsertItemTemplate>
                            <table border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:TextBox ID="TextBox1453" Width="45px" runat="server" Text='<%# Bind("ChoixMultipleMax") %>'></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                        </InsertItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Obligatoire : " SortExpression="topic">
                    <HeaderStyle Width="120px" HorizontalAlign="Right" />
                        <InsertItemTemplate>
                            <table border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("QuestionObligatoire") %>'></asp:CheckBox>
                                </td>
                            </tr>
                            </table>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fin : " SortExpression="topic">
                    <HeaderStyle Width="120px" HorizontalAlign="Right" />
                        <InsertItemTemplate>
                            <table border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Bind("QuestionFin") %>'></asp:CheckBox>
                                </td>
                            </tr>
                            </table>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Société : " SortExpression="topic">
                    <HeaderStyle Width="120px" HorizontalAlign="Right" />
                        <InsertItemTemplate>
                            <table border="0" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:TextBox ID="TextBoxOnsenfou112" Width="250px" runat="server" Text='<%# Bind("Societe") %>'></asp:TextBox>
                                </td>
                            </tr>
                            </table>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ValidationGroup="Insert" ButtonType="Link" ControlStyle-CssClass="HyperLinkStyle" ControlStyle-BorderStyle="Solid" ControlStyle-BorderWidth="1px" ShowInsertButton="True" ShowCancelButton="False" InsertText="Ajouter la Question" />
                </Fields>
            </asp:DetailsView>
        </div>
    </div>
    <br />
    </asp:Panel>
    
    <!-- Copier les reponses de la question -->
    <asp:Panel ID="PanelCopierQuestionReponse" runat="server">
    <table style="border:solid 1px"  bordercolor="#808080" border="0" cellpadding="5px" cellspacing="0" width="95%">
    <tr>
        <td align="left" width="350px">
            <label class="LabelStyle" title="Choisir les Réponses qui seront copiées avec la nouvelle Question">Copier les réponse à partir de la Question de Rang :</label>
        </td>
        <td align="left" width="70px">
            <asp:DropDownList ID="DropDownListCopierAPartirDe" Width="45px" runat="server" />
        </td>
        <td align="left">
            <asp:Label ID="LabelValidationMessage" runat="server" CssClass="LabelValidationMessageStyle" />
        </td>
    </tr>
    </table>   
    <br />
    </asp:Panel>     
    
    <!-- Questionnaire et les réponses -->
    <table style="border:solid 1px;border-color:#808080;"  cellpadding="25px" cellspacing="0" width="95%">
    <tr>
        <td style="width:45px">
            <asp:ImageButton ID="ImageButtonPrint1" runat="server" 
                ImageUrl="~/Images/print.png"
                onclick="ImageButtonPrint1_Click" 
                ToolTip="Formulaire d'impression"/>&nbsp;
            <asp:ImageButton ID="ImageButtonExcel" runat="server" 
                ImageUrl="~/Images/excel.png"
                onclick="ImageButtonExcel_Click" 
                ToolTip="Fichier Excel"/>
        </td>
    </tr>
    <tr>
    <td align="center">
    <asp:DataList ID="DataListQuestion" runat="server" Width="95%">
        <HeaderTemplate>
            <h3><%# SessionState.Questionnaire.Description %></h3>
            <asp:Label ID="Label2" ForeColor="blue" runat="server" Text="Valider" Visible='<%# SessionState.Questionnaire.Valider %>'></asp:Label>
            &nbsp;<asp:Label ID="Label1" ForeColor="red" runat="server" Text="Fin" Visible='<%# SessionState.Questionnaire.Fin %>'></asp:Label>
            &nbsp;<asp:Label ID="Label3" ForeColor="red" runat="server" Text="Clôturé" Visible='<%# SessionState.Questionnaire.Bloque %>'></asp:Label>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:HiddenField runat="server" ID="PollQuestionId" Value='<%# Eval("PollQuestionId") %>' />
            <asp:Panel ID="PanelSautPage" runat="server" Visible='<%# Eval("SautPage").ToString() != string.Empty %>' CssClass="PanelSautPage" >
                <table border="0" cellpadding="2" cellspacing="3" width="100%">
                <tr>
                    <td width="55px" runat="server" visible='<%# FormulaireEnModeExcel==false %>' >
                        <hr class="HrSautPageStyle" />
                    </td>
                    <td align="left">
                        <asp:Label ID="LabelSautPage" CssClass="LabelSautPageStyle" runat="server" Text='<%# Eval("SautPage") %>' />
                    </td>
                </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server" Visible='<%# Eval("Tableau").ToString() != string.Empty %>' CssClass="PanelTableau" >
                <table border="0" cellpadding="2" cellspacing="3" width="100%">
                <tr>
                    <td width="55px" runat="server" visible='<%# FormulaireEnModeExcel==false %>'>
                        <hr class="HrTableauStyle" />
                    </td>
                    <td align="left">
                        <asp:Label ID="Label4" CssClass="LabelTableauStyle" runat="server" Text='<%# Eval("Tableau") %>' />
                    </td>
                </tr>
                </table>
            </asp:Panel>
            <table border="0" width="100%" cellpadding="0" cellspacing="5">
            <tr>
                <td align="left" valign="top" runat="server" visible='<%# FormulaireEnModePrint==false %>'>
                    <span class="SpanHyperLinkReponseModifierStyle">
                        <asp:HyperLink ID="HyperLinkModifierQuestion" runat="server" CssClass="HyperLinkReponseModifierStyle" Width="18px" ToolTip="Modifier la Question" Text="?" NavigateUrl='<%# "~/Wizard/Question.aspx?PollQuestionId=" + Eval("PollQuestionId") %>' />
                    </span>
                </td>
                <td align="left" valign="top">
                    <span class="SpanHyperLinkReponseStyle">
                        <asp:HyperLink ID="HyperLink2" runat="server" CssClass="HyperLinkReponseStyle" Width="33px" ToolTip="Tester le Questionnaire à partir de cette question" NavigateUrl='<%# "~/Poll/Questionnaire.aspx?PollQuestionId=" + Eval("PollQuestionId") + "&t=1"%>' Text='<%# Eval("Rank") %>' Visible='<%# FormulaireEnModeExcel==false %>' />
                        <asp:Label ID="Label10" runat="server" CssClass="LabelListUserQuestionStyle" Text='<%# Eval("Rank") %>' Visible='<%# FormulaireEnModeExcel %>' />
                    </span>
                </td>            
                <td align="left" valign="middle" width="90%">
                    <span class="SpanHyperLinkReponseStyle">
                        <asp:HyperLink ID="HyperLink5" runat="server" CssClass="HyperLinkQuestionEnCoursStyle" ToolTip="Ajouter des réponses" NavigateUrl='<%# "~/Poll/Answers.aspx?PollId=" + Eval("PollQuestionId")%>' Text='<%# Eval("Question") %>' Visible='<%# FormulaireEnModeExcel ==false %>'/>
                        <asp:Label ID="onsenfoue123" runat="server" CssClass="LabelListUserQuestionStyle" Text='<%# Eval("Question") %>' Visible='<%# FormulaireEnModeExcel %>' />
                    </span>
                    <asp:Label ID="Label9" CssClass="LabelListUserQuestionChoixSimpleStyle" runat="server" Text=" Choix Simple " Visible='<%# (bool)Eval("ChoixMultiple") == false %>'/>
                    <asp:Label ID="Label7" CssClass="LabelListUserQuestionChoixMultipleStyle" runat="server" Text=" Choix Multiple " Visible='<%# (bool)Eval("ChoixMultiple") %>'/>
                    <asp:Label ID="Label2" CssClass="LabelListUserQuestionChoixMultipleStyle" runat="server" Text='<%# "(" + Eval("ChoixMultipleMin") + "/" + Eval("ChoixMultipleMax") + ")" %>' Visible='<%# (bool)Eval("ChoixMultiple") && ((int)Eval("ChoixMultipleMin") > 0) && ((int)Eval("ChoixMultipleMax") > 0)%>'/>
                    <asp:Label ID="Label6" CssClass="LabelListUserQuestionObligatoireStyle" runat="server" Text=" Obligatoire " Visible='<%# (bool)Eval("Obligatoire") %>'/>
                    <asp:Label ID="Label8" CssClass="LabelListUserQuestionFinStyle" runat="server" Text=" Fin " Visible='<%# (bool)Eval("Fin") %>'/>
                </td>            
            </tr>
            <tr>
                <td align="left" colspan="3">
                    <asp:Panel ID="PanelInstruction"  BorderColor="#F1F1F1" BorderStyle="Solid" BorderWidth="0px" runat="server" Visible='<%# Eval("Instruction").ToString() != "" %>'> 
                        <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="middle" style="padding-left:55px">
                                <asp:Label ID="Label1" CssClass="LabelListUserQuestionInstructionStyle" runat="server" Text='<%# Eval("Instruction") %>' />
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
                        <td align="left" style="padding-left:45px">
                            <asp:Label ID="LabelExcelDeMerde11" runat="server" Text="r : " visible='<%# FormulaireEnModeExcel %>'/>
                            <asp:Label ID="LabelReponse" CssClass="LabelListUserReponseStyle" runat="server" Text='<%# Eval("Rank") + " - " + Eval("Answer") %>'/>
                            <asp:Label ID="LabelReponseType" CssClass="LabelListBlueStyle" runat="server" Text='<%# Eval("TypeReponse") %>' Visible='<%# Eval("TypeReponse").ToString() != "Choix" %>'/>
                            <asp:Label ID="LabelReponseTextuelleObligatoire" CssClass="LabelListRedStyle" runat="server" Text="Obligatoire" Visible='<%# Eval("Obligatoire") %>'/>
                        </td>
                    </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </ItemTemplate>
    </asp:DataList>
    </td>
    </tr>
    </table>
    
    <table style="border:none" cellpadding="0" cellspacing="0">
        <tr>
            <td height="30px">
                <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
            </td>
        </tr>
    </table>

    <a name="BasDePage" ></a>

    <table>
        <tr>
            <td height="60px">
                <UserControl:RolloverButton ID="RolloverButtonTestez" runat="server" Text="Testez" OnClick="RolloverButtonTestez_Click" ToolTip="Testez le Questionnaire" />
            </td>
        </tr>
        <tr>
            <td height="20px">
                <span class="SpanHyperLinkStyle">
                <a href="#HautDePage" class="HyperLinkStyle">Haut</a>
                </span>
            </td>
        </tr>
        <tr>
            <td id="TrBoutonRetour" runat="server" visible="false">
                <span class="SpanHyperLinkStyle">
                    <a href="Manage.aspx" class="HyperLinkStyle">Retour</a>
                </span>
            </td>
        </tr>
    </table>
</div>    
</asp:Content>