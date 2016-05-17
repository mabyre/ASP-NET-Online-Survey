<%@ Page Language="C#" 
    Trace="false"
    MasterPageFile="~/MasterPage.master" 
    AutoEventWireup="true" 
    CodeFile="Manage.aspx.cs" 
    Inherits="Page_MemberManage" 
    Title="Administrer les membres" %>
    
<%@ Register Src="~/UserControl/LoadDocument.ascx" TagName="LoadDocument" TagPrefix="ucld" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="body">
    <div class="DivFullWidth">
        <h3>Intervieweurs</h3>
    </div>
    <table border="0" cellpadding="10px" height="75px" width="80%">
    <tr>
        <td width="16px">
            <asp:ImageButton ID="ImageButtonPrint" runat="server" 
                ImageUrl="~/Images/print.png"
                onclick="ImageButtonPrint_Click" 
                ToolTip="Formulaire d'impression"/>
        </td>
        <td align="right">
            <label for="DropDownListSociete" class="LabelStyle">Société : </label><asp:DropDownList ID="DropDownListSociete" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListSociete_SelectedIndexChanged" />
        </td>
        <td align="right">
            <label for="DropDownListSociete" class="LabelStyle" title="Filtrer les membres dont le Nom commence par la lettre">Lettre : </label><asp:DropDownList ID="DropDownListLettre" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListLettre_SelectedIndexChanged" />
        </td>
    </tr>
    </table>

    <div>
    <asp:UpdatePanel ID="UpdatePanel22" runat="server">
        <ContentTemplate>
        
        <asp:GridView ID="GridViewMembers" runat="server" Width="80%"
            AllowPaging="False"
            AllowSorting="True" 
            PageSize="50"
            AutoGenerateColumns="False" 
            DataKeyNames="MembreGUID" 
            CssClass="GridViewStyle"
            OnRowCreated="GridViewMembers_RowCreated" 
            OnSorted="GridViewMembers_OnSorted" 
            OnSorting="GridViewMembers_OnSorting"
            OnRowCommand="GridViewMembers_RowCommand"
            OnRowUpdated="GridViewMembers_RowUpdated"
            OnRowUpdating="GridViewMembers_RowUpdating" 
            OnLoad="GridViewMembers_OnLoad" 
            OnRowDataBound="GridViewMembers_RowDataBound"
            OnPageIndexChanged="GridViewMembers_PageIndexChanged">
            <HeaderStyle CssClass="GridViewHeaderStyle"/>
            <SelectedRowStyle BackColor="#F1F1F1" />
            <Columns>
            
                <asp:CommandField ItemStyle-Width="50px" ShowSelectButton="True" ShowDeleteButton="False" ShowEditButton="false" ButtonType="Image" DeleteImageUrl="~/Images/Delete.gif" EditImageUrl="~/Images/Edit.gif" UpdateImageUrl="~/Images/Save.gif" CancelImageUrl="~/Images/Annul.gif" SelectImageUrl="~/Images/Select_bleu.gif"/>
                
                <asp:BoundField HeaderText="User" DataField="NomUtilisateur" SortExpression="NomUtilisateur" ReadOnly="true">
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Pass" DataField="MotDePasse" SortExpression="MotDePasse" ReadOnly="true">
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Nom" DataField="Nom" SortExpression="Nom">
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                
                <asp:BoundField HeaderText="Prénom" DataField="Prenom" SortExpression="Prenom">
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>
                
                <asp:BoundField HeaderText="Société" DataField="Societe" SortExpression="Societe">
                    <ItemStyle CssClass="BoundFieldStyle" Width="150px" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Téléphone" DataField="Telephone" SortExpression="Telephone">
                    <ItemStyle CssClass="BoundFieldStyle" Width="100px" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Adresse" DataField="Adresse" SortExpression="Adresse">
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <HeaderStyle Wrap="False" />
                </asp:BoundField>

                <asp:TemplateField HeaderText="Appr" SortExpression="IsApproved">
                    <ItemStyle CssClass="BoundFielCenterStyle" />
                    <ItemTemplate>
                        <asp:Label ID="LabelIsApproved" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Lock" SortExpression="IsLockedOut">
                    <ItemStyle CssClass="BoundFielCenterStyle" />
                    <ItemTemplate>
                        <asp:Label ID="LabelLocked" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="On">
                    <ItemStyle CssClass="BoundFielCenterStyle" />
                    <ItemTemplate>
                        <asp:Label ID="LabelOnline" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Connexion" SortExpression="LastLoginDate">
                    <ItemStyle CssClass="BoundFieldDateStyle" />
                    <ItemTemplate>
                        <asp:Label ID="LabelLastLoginDate" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Mail">
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <ItemTemplate>
                        <asp:Label ID="LabelEmail" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Création" SortExpression="CreateDate">
                    <ItemStyle CssClass="BoundFieldDateStyle" />
                    <ItemTemplate>
                        <asp:Label ID="LabelDateCreation" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Fin" SortExpression="DateFinAbonnement">
                    <ItemStyle CssClass="BoundFieldDateStyle" />
                    <ItemTemplate>
                        <asp:Label ID="LabelDateFinAbonnement" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="@" SortExpression="">
                    <ItemStyle CssClass="BoundFieldEmailStyle" />
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLinkEmail" runat="server" Text="@" />
                    </ItemTemplate>
                    <ControlStyle Width="20px" />
                </asp:TemplateField>
                
                <asp:HyperLinkField DataNavigateUrlFields="MembreGUID" 
                    DataNavigateUrlFormatString="~/Member/Edit.aspx?MembreGUID={0}" 
                    HeaderText="Edit" Text="&#187;&#187;&#187;" 
                    ItemStyle-CssClass="ItemCentreStyle" />
               
            </Columns>
            
            <EmptyDataTemplate>
                <table border="0" cellpadding="10px"><tr><td><b>Pas de contacts pour ce critère</b></td></tr></table>
            </EmptyDataTemplate>
            
            <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" Position="TopAndBottom" />
            <PagerStyle Font-Bold="True" HorizontalAlign="Right" />
            
        </asp:GridView>

        <table>
            <tr>
                <td height="5px">
                </td>
            </tr>
            <tr>
                <td align="left">
                    <i>Page <%=GridViewMembers.PageIndex + 1%> sur <%=GridViewMembers.PageCount%></i>          
                </td>
            </tr>
        </table>
        
        <!-- DataListMembreQuestionnaire -->
        <asp:SqlDataSource ID="SqlDataSourceMembreQuestionnaire" Runat="server" 
            ConnectionString="<%$ ConnectionStrings:QuestionnaireDB %>" />
            
        <asp:DataList ID="DataListMembreQuestionnaire" runat="server" 
            DataSourceID="SqlDataSourceMembreQuestionnaire" 
            OnItemDataBound="DataListMembreQuestionnaire_ItemDataBound">
        <AlternatingItemStyle BackColor="White">
        </AlternatingItemStyle>
        <HeaderTemplate>
            <table>
                <tr height="15px">
                    <td>
                    </td>
                </tr>
                <tr height="15px">
                    <td>
                        <h4>Questionnaires</h4>
                    </td>
                </tr>
            </table>
        </HeaderTemplate>
        <ItemStyle CssClass="LabelNormalStyle" />
        <ItemTemplate>
            <table cellpadding="3">
            <tr>
                <td>
                    <asp:HiddenField ID="HiddenFieldQuestionnaireID" runat="server" 
                        Value='<%# Eval("QuestionnaireID") %>' />
                    <span class="HyperLinkColoredStyle">
                        <asp:HyperLink ID="HyperLink1" runat="server" 
                            NavigateUrl='<%# "~/Poll/List.aspx?QuestionnaireID=" + Eval("QuestionnaireID")%>'
                            ToolTip="Visualiser le Questionnaire" 
                            Text='<%# Eval("Description") %>' />
                    </span>
                    <asp:Label ID="Label1" runat="server"
                        ForeColor="Green"  
                        ToolTip="Code d'Accès"
                        Text='<%# Eval("CodeAcces") %>' />
                    <asp:Label ID="LabelDateCreation" runat="server" 
                        ToolTip="Date de création"
                        Text='<%# Eval("DateCreation") %>' />
                    <asp:Label ID="LabelQuestions" runat="server" 
                        ForeColor="Green" 
                        ToolTip="Nombre de questions" />
                    <asp:Label ID="LabelNombreContacts" runat="server" 
                        ForeColor="Orange" 
                        ToolTip="Nombre d'interviewés" />
                    <asp:Label ID="LabelVotes" runat="server" 
                        ForeColor="Red" 
                        ToolTip="Nombre de votes" />
                    <span class="HyperLinkColoredStyle">
                        <asp:HyperLink ID="HyperLink3" runat="server" 
                            NavigateUrl='<%# "~/Questionnaire/Edit.aspx?QuestionnaireID=" + Eval("QuestionnaireID")%>' 
                            ToolTip="Editer le questionnaire" 
                            Text="Edit" />
                    </span>
                    <span class="HyperLinkColoredStyle">
                        <asp:HyperLink ID="HyperLink4" runat="server" 
                            NavigateUrl='<%# "~/Member/Manage.aspx?QuestionnaireID=" + Eval("QuestionnaireID")%>' 
                            ToolTip="Emails" 
                            Text="@" />
                    </span>
                    <span class="HyperLinkColoredStyle">
                        <asp:HyperLink ID="HyperLink2" runat="server" 
                            NavigateUrl='<%# "~/Poll/QuestionnaireStatAll.aspx?QuestionnaireID=" + Eval("QuestionnaireID")%>' 
                            ToolTip="Satistiques du Questionnaire" 
                            Text="stats &#187;&#187;&#187;" />
                    </span>
                </td>
            </tr>
            </table>
        </ItemTemplate>
        <FooterTemplate>
            <table>
                <tr height="15px">
                    <td></td>
                </tr>
            </table>     
        </FooterTemplate>            
        </asp:DataList>

        </ContentTemplate>
        
    </asp:UpdatePanel>
    
    <asp:UpdateProgress id="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel22">
        <progresstemplate>
            <asp:Image ID="loading2" runat="server" SkinID="ImageLoading" />
        </progresstemplate>
    </asp:UpdateProgress>
 
    
    <table border="0" cellpadding="10px" height="75px" >
    <tr>
        <td align="left">
            <UserControl:RolloverLink ID="RolloverLinkRegister" runat="server" NavigateURL="~/Member/Register.aspx" Text="Nouveau" ToolTip="Enregistrez un nouvel utilisateur" />
        </td>
    </tr>        
    </table>            
    </div>
</div>
</asp:Content>

