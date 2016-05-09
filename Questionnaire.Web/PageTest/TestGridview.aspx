<%@ Page Language="C#" Trace="true" MasterPageFile="~/MasterPage.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="TestGridview.aspx.cs" Inherits="Test_Gridview" %>
<%@ Register TagPrefix="ucwc" TagName="WebContent" Src="~/UserControl/WebContent.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="body">

    <h3>Administrer les Questions des Questionnaires</h3>

    <table border="0" cellpadding="10" height="75">
        <tr>
            <td>
                <label class="LabelStyle">Questionnaires : </label>
                <usr:DropDownListQuestionnaires ID="DropDownListQuestionnaire" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListQuestionnaire_SelectedIndexChanged" />
            </td>
        </tr>
    </table>        

    <asp:HiddenField ID="HiddenFieldQuestionnaireID" runat="server" Visible="false" />
    <asp:SqlDataSource ID="SqlDataSourceQuestion" runat="server" ConnectionString="<%$ ConnectionStrings:QuestionnaireDB %>"
        DataSourceMode="DataSet"
        EnableCaching="true"
        SelectCommand="SELECT * FROM PollQuestions WHERE QuestionnaireID = @HiddenFieldQuestionnaireID ORDER BY Rank"
        DeleteCommand="DELETE FROM PollQuestions WHERE PollQuestionId = @old_PollQuestionId"
        InsertCommand="INSERT INTO PollQuestions (PollQuestionId, QuestionnaireID, Question, Societe, Rank, Instruction, QuestionObligatoire, QuestionFin, CreationDate, MembreGUID) VALUES (@PollQuestionId, @QuestionnaireID, @Question, @Societe, @Rank, @Instruction, @QuestionObligatoire, @QuestionFin, @CreationDate, @MembreGUID)"
        UpdateCommand="UPDATE PollQuestions SET Question = @question, Rank = @Rank, Societe = @societe, CreationDate = @CreationDate WHERE PollQuestionId = @old_PollQuestionId"
        OldValuesParameterFormatString="old_{0}" 
        OnUpdating="SqlDataSourceQuestion_Updating" >
        <SelectParameters>
            <asp:ControlParameter Name="HiddenFieldQuestionnaireID" ControlID="HiddenFieldQuestionnaireID" PropertyName="Value" /> 
        </SelectParameters>
    </asp:SqlDataSource>

    <div class="DivFullWidth">
        <asp:GridView ID="GridViewQuestion" runat="server" 
            AutoGenerateColumns="false" 
            SelectedRowStyle-BackColor="#F1F1F1" 
            Width="80%" 
            DataKeyNames="PollQuestionId" 
            DataSourceID="SqlDataSourceQuestion" 
            OnRowUpdating="GridViewQuestion_RowUpdating"
            OnRowCommand="GridViewQuestion_RowCommand" 
            OnRowUpdated="GridViewQuestion_RowUpdated" 
            OnSelectedIndexChanged="GridViewQuestion_SelectedIndexChanged">
            <HeaderStyle CssClass="GridViewHeaderStyle"/>
            <SelectedRowStyle BackColor="#F1F1F1" ForeColor="#C1C1C1" />
            <Columns>
            
                <asp:CommandField ShowDeleteButton="True" ShowSelectButton="false" ShowEditButton="True" ButtonType="Image" DeleteImageUrl="~/Images/Delete.gif" EditImageUrl="~/Images/Edit.gif" SelectImageUrl="~/Images/select.gif" UpdateImageUrl="~/Images/Save.gif" CancelImageUrl="~/Images/Annul.gif"/>
                
                <asp:BoundField DataField="Question" HeaderText="Question" SortExpression="Question">
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <ControlStyle Width="250px"/>
                </asp:BoundField>
                
                <asp:BoundField DataField="Rank" HeaderText="Rang" SortExpression="Rank" >
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <ControlStyle Width="40px"/>
                </asp:BoundField>

                <asp:BoundField DataField="QuestionObligatoire" HeaderText="Obligatoire" SortExpression="QuestionObligatoire" >
                    <ItemStyle CssClass="BoundFieldStyle" ForeColor="Blue" Width="85px" />
                    <ControlStyle Width="40px" />
                </asp:BoundField>

                <asp:BoundField DataField="QuestionFin" HeaderText="Fin" SortExpression="QuestionFin" >
                    <ItemStyle CssClass="BoundFieldStyle" ForeColor="Red" />
                    <ControlStyle Width="40px" />
                </asp:BoundField>

                <asp:BoundField DataField="Instruction" HeaderText="Instruction" SortExpression="Instruction">
                    <ItemStyle CssClass="BoundFieldStyle" ForeColor="Green" Width="110px" />
                    <ControlStyle Width="110px"/>
                </asp:BoundField>
                
                <asp:BoundField DataField="Societe" HeaderText="Société" SortExpression="Societe" Visible="false">
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <ControlStyle Width="150px"/>
                </asp:BoundField>
                
                <asp:BoundField DataField="CreationDate" HeaderText="Date" SortExpression="CreationDate" Visible="false">
                    <ItemStyle CssClass="BoundFieldStyle" Width="150px" />
                    <ControlStyle Width="130px"/>
                </asp:BoundField>
                
                <asp:HyperLinkField DataNavigateUrlFields="PollQuestionId" DataNavigateUrlFormatString="~/Poll/Answers.aspx?PollId={0}" HeaderText="Réponses" Text="&#187;&#187;&#187;" ItemStyle-CssClass="ItemStyle" />
            </Columns>
            <EmptyDataTemplate>
                <table border="0" cellpadding="10px"><tr><td><b>Pas de Questions pour ce Questionnaire</b></td></tr></table>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

    <!-- DetailsView -->
    <div id="columnright">
        <div class="rightblock">
            <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" 
                DataKeyNames="PollQuestionId"
                DataSourceID="SqlDataSourceQuestion" 
                Height="60px" Width="80%" 
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
                                        ID="RequiredFieldValidator1" runat="server" ErrorMessage="Question est requise">*</asp:RequiredFieldValidator>
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
                                    <asp:TextBox ID="TextBox2" Width="110px" runat="server" Text='<%# Bind("Instruction") %>'></asp:TextBox>
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
                    <asp:CommandField ValidationGroup="Insert" ShowInsertButton="True" ShowCancelButton="False" ControlStyle-Height="23px" InsertText="Ajouter la Question" />
                </Fields>
            </asp:DetailsView>
            
            
            <table border="0" cellpadding="3px" width="80%" cellspacing="0">
            <tr>
                <td align="left" width="90px">
                    <asp:CheckBox ID="CheckBoxSociete" runat="server" Text="Société" AutoPostBack="true" OnCheckedChanged="CheckBoxSociete_CheckedChanged" />
                </td>
                <td align="left">
                    <asp:CheckBox ID="CheckBoxDate" runat="server" Text="Date" AutoPostBack="true" OnCheckedChanged="CheckBoxDate_CheckedChanged" />
                </td>
            </tr>        
            </table>            
            
        </div>
    </div>
    <br />
    
    <!-- Questionnaire et les réponse -->
    <table style="border:solid 1px"  bordercolor="#808080" cellpadding="25px" cellspacing="0" width="80%">
    <tr>
    <td width="5%">
    </td>
    <td align="center">
    <asp:DataList ID="DataListQuestion" runat="server" Width="100%">
        <HeaderTemplate>
            <h3>Le Questionnaire</h3>
        </HeaderTemplate>
        <ItemTemplate>
            <asp:HiddenField runat="server" ID="PollQuestionId" Value='<%# Eval("PollQuestionId") %>' />
            <table border="0" width="80%">
            <tr>
                <td align="left">
                    <asp:Label ID="LabelQuestion" CssClass="LabelListUserQuestionStyle" runat="server" Text='<%# Eval("Rank") + " - " + Eval("Question") %>'/>
                    <asp:Label ID="Label1" CssClass="LabelListUserQuestionInstructionStyle" runat="server" Text='<%# " " + Eval("Instruction") %>'/>
                    <asp:Label ID="Label2" CssClass="LabelListUserQuestionObligatoireStyle" runat="server" Text='<%# " " + FilterBool((bool)Eval("Obligatoire")) %>'/>
                    <asp:Label ID="Label3" CssClass="LabelListUserQuestionFinStyle" runat="server" Text='<%# " " + FilterBool((bool)Eval("Fin")) %>'/>
                </td>
            </tr>
            </table>
            <asp:DataList ID="DataListReponse" runat="server" Width="100%">
                <ItemTemplate>
                    <table border="0" width="80%">
                    <tr>
                        <td align="left">
                            <asp:Label ID="LabelReponse" CssClass="LabelListUserReponseStyle"  runat="server" Text='<%# Eval("Rank") + " - " + Eval("Answer") %>'/>
                        </td>
                    </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </ItemTemplate>
    </asp:DataList>
    </td>
    <td width="5%">
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

    <table>
        <tr>
            <td height="60px">
                <UserControl:RolloverButton ID="ButtonListe" runat="server" Text="Liste" OnClick="ButtonListe_Click" ToolTip="Liste des Questions et des Réponses associées au Questionnaire"/>                
                <UserControl:RolloverButton ID="RolloverButtonTestez" runat="server" Text="Testez" OnClick="RolloverButtonTestez_Click" ToolTip="Testez le Questionnaire" />
            </td>
        </tr>
    </table>
    
</div>
</asp:Content>