<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" Trace="false" AutoEventWireup="true" CodeFile="Answers.aspx.cs" Inherits="Poll_Answers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="DivFullWidth">

    <h3>Administrer les Réponses</h3>

    <table border="0" cellspacing="0" cellpadding="0" width="80%">
        <tr>
            <td height="35" colspan="2">
                <asp:HyperLink ID="HyperLinkQuestionnaire" ToolTip="Retourner à la programmation des questions" runat="server" NavigateUrl="~/Poll/Manage.aspx">Questionnaires</asp:HyperLink>
            </td>
        </tr>
        <tr> 
            <td height="25" colspan="2">
                <span class="SpanHyperLinkReponseModifierStyle">
                    <asp:HyperLink ID="HyperLinkModifierQuestion" runat="server" CssClass="HyperLinkReponseModifierStyle" Width="18px" ToolTip="Modifier la Question" Text="?" />
                </span>
                <span class="SpanHyperLinkReponseStyle">
                    <asp:HyperLink ID="HyperLinkRank" runat="server" CssClass="HyperLinkQuestionEnCoursStyle" ToolTip="Tester le Questionnaire à partir de cette question"/>
                </span>
                <asp:Label ID="LabelQuestion" CssClass="LabelListUserQuestionStyle" runat="server" />
                <asp:Label ID="LabelChoixSimple" CssClass="LabelListUserQuestionChoixSimpleStyle" runat="server" Text=" Choix Simple "/>
                <asp:Label ID="LabelChoixMultiple" CssClass="LabelListUserQuestionChoixMultipleStyle" runat="server" Text=" Choix Multiple "/>
                <asp:Label ID="LabelChoixMultipleMinMax" CssClass="LabelListUserQuestionChoixMultipleStyle" runat="server"  />
                <asp:Label ID="LabelObligatoire" CssClass="LabelListUserQuestionObligatoireStyle" runat="server" Text=" Obligatoire " />
                <asp:Label ID="LabelFin" CssClass="LabelListUserQuestionFinStyle" runat="server" Text=" Fin " />
                <br />
            </td>
        </tr>
        <tr>
            <td height="20" colspan="2">
                <asp:Label ID="LabelInstruction" CssClass="LabelListUserQuestionInstructionStyle" runat="server" /><br />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="LabelMessage" CssClass="LabelListUserQuestionMessageStyle" runat="server" />
            </td>
        </tr>
        <tr>
            <td height="15" colspan="2">
            </td>
        </tr>   
    </table>
    <table border="0" cellspacing="8" cellpadding="2" width="80%">
    <tr>
        <td style="text-align:right;width:50%">
            <label class="LabelStyle" title="Alignement de la Réponse à gauche ou à droite des cases à cocher ou des boutons radios">Texte à :</label>
        </td>
        <td align="left">
            <asp:DropDownList ID="DropDownListTextAlign" runat="server" 
                onselectedindexchanged="DropDownListTextAlign_SelectedIndexChanged" 
                Width="90px" CssClass="DropDownListGridViewStyle" AutoPostBack="True" >
            </asp:DropDownList>
        </td>
    </tr>                    
    <tr>
        <td align="right">
            <label class="LabelStyle" 
                title="Les cases à cocher ou les boutons radio diposés verticalement ou horizontalement">Horizontal/Vertical :</label></td>
        <td align="left">
            <asp:DropDownList ID="DropDownListVerticalHorizontal" runat="server" 
                onselectedindexchanged="DropDownListVerticalHorizontal_SelectedIndexChanged" 
                Width="90px" CssClass="DropDownListGridViewStyle" AutoPostBack="True" >
            </asp:DropDownList>
        </td>
    </tr>                    
    </table>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:QuestionnaireDB %>"
        SelectCommand="SELECT PollAnswerId, PollQuestionId, Answer, TypeReponse, Width, AlignLeft, Horizontal, Obligatoire, Rows, Rank, Score FROM PollAnswers WHERE PollQuestionId = @pollId ORDER BY Rank"
        DeleteCommand="DELETE FROM Bidon"
        InsertCommand="INSERT INTO PollAnswers (PollAnswerId, Answer, PollQuestionId, TypeReponse, Obligatoire, Width, Rows, Rank, Score) VALUES (@pollAnswerId, @answer, @pollQuestionId, @TypeReponse, @Obligatoire, @Width, @Rows, @Rank, @Score)"
        UpdateCommand="UPDATE PollAnswers SET Answer = @Answer, TypeReponse = @TypeReponse, Width = @Width, Rows = @Rows, Obligatoire = @Obligatoire, Rank = @Rank, Score = @Score WHERE PollAnswerId = @original_PollAnswerId"
        OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:QueryStringParameter Name="pollId" QueryStringField="PollId" />
        </SelectParameters>
    </asp:SqlDataSource>

    <table border="0" cellpadding="3px" cellspacing="0" width="80%">
    <tr>
        <td align="left" style="width:90px; height: 26px;">
            <asp:Button ID="ButtonRangPlusUn" runat="server" Text="Rang+1" CssClass="ButtonStyle" ToolTip="Ajouter 1 au rang des Réponses sélectionnées" OnClick="ButtonRangPlusUn_Click" />
        </td>
        <td align="left" style="height: 26px">
            <asp:Button ID="ButtonRangMoinsUn" runat="server" Text="Rang-1" CssClass="ButtonStyle" ToolTip="Retirer 1 au rang des Réponses sélectionnées" OnClick="ButtonRangMoinsUn_Click" />
        </td>
    </tr>
    </table>
            
    <!-- Pour ne pas avoir a retrouver la valeur depuis la DropDownList, on utilise TypeReponse -->
    <asp:GridView ID="GridView1" runat="server" Width="80%"
        AllowPaging="False" 
        AutoGenerateColumns="False"
        DataKeyNames="PollAnswerId" 
        DataSourceID="SqlDataSource1" 
        OnRowUpdating="GridView1_RowUpdating" 
        OnRowCommand="GridView1_RowCommand"
        OnRowDatabound="GridView1_RowDataBound">
        <HeaderStyle CssClass="GridViewHeaderStyle"/>
        <Columns>
            <asp:CommandField ItemStyle-Width="50px" ShowDeleteButton="True" ShowEditButton="True" ButtonType="Image" DeleteImageUrl="~/Images/Delete.gif" EditImageUrl="~/Images/EditBleu.gif" UpdateImageUrl="~/Images/Save.gif" CancelImageUrl="~/Images/Annul.gif"/>

            <asp:BoundField DataField="Answer" HeaderText="Réponse" SortExpression="Answer">
                <ItemStyle CssClass="BoundFieldStyle" />
                <ControlStyle Width="95%"/>
            </asp:BoundField>

            <asp:BoundField DataField="Rank" HeaderText="Rang" SortExpression="Rank">
                <ItemStyle HorizontalAlign="Center" Width="40px"/>
                <ControlStyle Width="40px"/>
            </asp:BoundField>

            <asp:TemplateField HeaderText="&plusmn;">
                <ItemStyle Width="30px" HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBoxRangPlusMoinsUn" runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="TypeReponse" Visible="false">
            </asp:BoundField>

            <asp:TemplateField HeaderText="Type Réponse">
                <ItemStyle Width="30px" HorizontalAlign="Center"/>
                <ItemTemplate>
                    <UserControl:DropDownListGridView ID="DropDownListTypeReponse" CssClass="DropDownListGridViewStyle" EnableViewState="true" ToolTip="Alignement de la Question" AutoPostBack="true"  runat="server" 
                        OnSelectedIndexChanged="DropDownListTypeReponse_SelectedIndexChanged">
                    </UserControl:DropDownListGridView> 
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Obligatoire" HeaderText="Obligatoire" SortExpression="Obligatoire">
                <ItemStyle HorizontalAlign="Center" Width="50px"/>
                <ControlStyle Width="40px"/>
            </asp:BoundField>

            <asp:BoundField DataField="Width" HeaderText="Largeur" SortExpression="Width">
                <ItemStyle HorizontalAlign="Center" Width="60px"/>
                <ControlStyle Width="40px"/>
            </asp:BoundField>

            <asp:BoundField DataField="Rows" HeaderText="Lignes" SortExpression="Rows">
                <ItemStyle HorizontalAlign="Center" Width="50px"/>
                <ControlStyle Width="40px"/>
            </asp:BoundField>

            <asp:BoundField DataField="Score" HeaderText="Score" SortExpression="Score">
                <ItemStyle HorizontalAlign="Center" Width="40px"/>
                <ControlStyle Width="40px"/>
            </asp:BoundField>
            
        </Columns>
    </asp:GridView>
    
    <asp:DetailsView ID="DetailsView1" runat="server" DataSourceID="SqlDataSource1"
        AutoGenerateRows="False" 
        DataKeyNames="PollAnswerId"
        Height="50px" Width="80%"  
        DefaultMode="Insert" 
        OnItemInserting="DetailsView1_ItemInserting">
        <Fields>
            <asp:BoundField DataField="PollAnswerId" HeaderText="Réponse" InsertVisible="False" ReadOnly="True" SortExpression="PollAnswerId" />
            <asp:TemplateField HeaderText="Réponse : " SortExpression="Answer">
                <HeaderStyle Width="120px" HorizontalAlign="Right" />
                <InsertItemTemplate>
                    <table border="0" width="100%">
                    <tr>
                        <td align="left">
                            <asp:TextBox ID="TextBox1" Width="95%" runat="server" Text='<%# Bind("Answer") %>'></asp:TextBox>
                            <!-- BUG20100407
                            <asp:RequiredFieldValidator ControlToValidate="TextBox1" ValidationGroup="Insert"
                                ID="RequiredFieldValidator1" runat="server" ErrorMessage="Answer is required">*</asp:RequiredFieldValidator>
                            -->
                        </td>
                    </tr>
                    </table>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Type de réponse : " SortExpression="topic">
            <HeaderStyle Width="120px" HorizontalAlign="Right" />
                <InsertItemTemplate>
                    <table border="0" width="100%">
                    <tr>
                        <td align="left">
                            <asp:DropDownList ID="DropDownListTypeReponse" CssClass="DropDownListGridViewStyle" DataSource='<%# TypeReponse.List() %>' DataValueField='<%# Bind("TypeReponse") %>' DataTextField='<%# Bind("TypeReponse") %>' runat="server" />
                        </td>
                    </tr>
                    </table>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Obigatoire : " SortExpression="Obigatoire">
            <HeaderStyle Width="120px" HorizontalAlign="Right" />
                <InsertItemTemplate>
                    <table border="0" width="100%">
                    <tr>
                        <td align="left">
                            <asp:CheckBox ID="CheckBoxObigatoire" runat="server" Checked='<%# Bind("Obligatoire") %>'></asp:CheckBox>
                        </td>
                    </tr>
                    </table>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Largeur : " SortExpression="Width">
                <HeaderStyle Width="120px" HorizontalAlign="Right" />
                <InsertItemTemplate>
                    <table border="0" width="100%">
                    <tr>
                        <td align="left">
                            <asp:TextBox ID="TextBoxWidth" Width="45px" runat="server" Text='<%# Bind("Width") %>'></asp:TextBox>
                        </td>
                    </tr>
                    </table>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Lignes : " SortExpression="Rows">
                <HeaderStyle Width="120px" HorizontalAlign="Right" />
                <InsertItemTemplate>
                    <table border="0" width="100%">
                    <tr>
                        <td align="left">
                            <asp:TextBox ID="TextBoxRows" Width="45px" runat="server" Text='<%# Bind("Rows") %>'></asp:TextBox>
                        </td>
                    </tr>
                    </table>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Score : " SortExpression="Score">
                <HeaderStyle Width="120px" HorizontalAlign="Right" />
                <InsertItemTemplate>
                    <table border="0" width="100%">
                    <tr>
                        <td align="left">
                            <asp:TextBox ID="TextBoxScore" Width="45px" runat="server" Text='<%# Bind("Score") %>'></asp:TextBox>
                        </td>
                    </tr>
                    </table>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Rang : " SortExpression="Rang">
                <HeaderStyle Width="120px" HorizontalAlign="Right" />
                <InsertItemTemplate>
                    <table border="0" width="100%">
                    <tr>
                        <td align="left">
                            <asp:TextBox ID="RankTextBox" Width="45px" runat="server" Text='<%# Bind("Rank") %>'></asp:TextBox>
                        </td>
                    </tr>
                    </table>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ButtonType="Link" 
                ControlStyle-CssClass="HyperLinkStyle" ControlStyle-BorderStyle="Solid" 
                ControlStyle-BorderWidth="1px" ShowInsertButton="True" ShowCancelButton="False" 
                InsertText="Ajouter la Réponse" >
            <ControlStyle BorderWidth="1px" BorderStyle="Solid" CssClass="HyperLinkStyle"></ControlStyle>
            </asp:CommandField>
        </Fields>
        </asp:DetailsView>
    <!-- BUG20100407 asp:CommandField : ValidationGroup="Insert" retiré -->
    
    <table style="border:none" cellpadding="0" cellspacing="15px">
        <tr>
            <td>
                <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
            </td>
        </tr>
    </table>
    
</div>
</asp:Content>