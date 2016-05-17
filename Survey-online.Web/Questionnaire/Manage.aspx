<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="Manage.aspx.cs" Inherits="Questionnaire_Manage" %>
<%@ Register TagPrefix="ucwc" TagName="WebContent" Src="~/UserControl/WebContent.ascx" %>

<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:SqlDataSource ID="SqlDataSourceQuestionnaire" runat="server" ConnectionString="<%$ ConnectionStrings:QuestionnaireDB %>"
    UpdateCommand="UPDATE [Questionnaire] SET Description = @Description, Valider = @Valider, Fin = @Fin, Anonyme = @Anonyme, Anonymat = @Anonymat, Bloque = @Bloque, Publier = @Publier, DateCreation = @DateCreation WHERE [QuestionnaireID] = @original_QuestionnaireID"
    OldValuesParameterFormatString="original_{0}" >
</asp:SqlDataSource>

<div class="DivFullWidth">
    <h3>Administrer les Questionnaires</h3>
    <h4>Sélectionnez un Questionnaire</h4>
    <!-- Aide en ligne sur le Formulaire -->
    <asp:Panel ID="PanelAide" runat="server" class="PanelAideStyle" Width="90%">
    <div style="text-align:left;">
    <table class="TableCollapsePanel">
        <tr>
            <td>
            Sélectionnez un Questionnaire pour travailler avec.<br />
            Toutes les fonctionnalités s'appliqueront alors à ce Questionnaire.<br />
            <br />
            Vous avez la possibilité de modifier certaines propriétés de votre Questionnaire :<br />
            <ul class="UlStyle">
                <li>Description : le Titre du Questionnaire.</li>
                <li>Valider et Fin : modes d'enregistrement des réponses des interviewés.</li>
                <li>Invitation : pour utiliser la possibilité d'inviter les Interviewés par un lien web.</li>
                <li>Publier : pour utiliser la possibilité d'un lien web sur le formulaire de publication.</li>
                <li>Anonymat : propriété en lecture seule.</li>
            </ul>
            Ces propriétés et les autres propriétés de votre Questionnaire, sont accessibles par le bouton <b>Editer</b>.<br />
            Cliquez sur <b>Editer</b> pour obtenir plus d'informations sur chacune de ces propriétés.<br />
            Pour modifier une propriété vous pouvez mettre ; vrai, faux, true, false, ou 0 et 1 comme valeur.<br />
            Sauvegarder les modifications en cliquant sur la disquette.<br />
            </td>
        </tr>
    </table>
    </div>
    </asp:Panel>
    
    <table border="0" cellpadding="10px" width="80%">
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
        <td align="left" width="28px">
            <a href="http://www.sodevlog.fr/Questionnaire.En.Ligne/page/Questionnaire-en-ligne-Aide.aspx#ProgrammezVosQuestionnaires" title="Aide sur la programmation de vos Questionnaires" target="_blank">
                <img src="../App_Themes/Sodevlog/Images/help_rouge.gif" border="0" />
            </a>
        </td>
        <td align="left">
            <asp:CheckBox ID="CheckBoxDate" runat="server" Text="Date" AutoPostBack="true" OnCheckedChanged="CheckBoxDate_CheckedChanged" />
        </td>
    </tr>        
    </table>             
    
    <div>
    <asp:UpdatePanel ID="UpdatePanel22" runat="server">
        <ContentTemplate>
        
        <!-- Ne cherchons pas a numeroter les BoundField ce n'est pas autorisé -->
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
            DataKeyNames="QuestionnaireID" 
            Width="80%" 
            DataSourceID="SqlDataSourceQuestionnaire" 
            OnRowUpdating="GridView1_RowUpdating" OnRowCommand="GridView1_RowCommand" OnRowUpdated="GridView1_RowUpdated">
            <HeaderStyle CssClass="GridViewHeaderStyle"/>
            <SelectedRowStyle BackColor="#F1F1F1" />
            <Columns>
            
                <asp:CommandField ItemStyle-Width="50px"
                    ShowDeleteButton="False" 
                    ShowEditButton="True" 
                    ShowSelectButton="True" 
                    SelectText="Sélectionner un Questionnaire pour choisir de l'éditer, le modifier ou le visualiser"
                    ButtonType="Image" 
                    DeleteImageUrl="~/Images/Delete.gif" 
                    EditImageUrl="~/Images/Edit.gif" 
                    UpdateImageUrl="~/Images/Save.gif" 
                    CancelImageUrl="~/Images/Annul.gif" 
                    SelectImageUrl="~/Images/Select_bleu.gif" />

                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description">
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <ControlStyle Width="95%"/>
                </asp:BoundField>

                <asp:BoundField DataField="Style" HeaderText="Style" SortExpression="Style" ReadOnly="true"/>

                <asp:BoundField DataField="Nom" HeaderText="Nom" SortExpression="Nom" ReadOnly="true"/>

                <asp:BoundField DataField="Prenom" HeaderText="Prenom" SortExpression="Prenom" ReadOnly="true"/>

                <asp:BoundField DataField="Societe" HeaderText="Societe" SortExpression="Societe" ReadOnly="true" />

                <asp:BoundField HeaderText="Code" DataField="CodeAcces" SortExpression="CodeAcces" ReadOnly="true">
                    <ItemStyle CssClass="BoundFieldStyle" />
                    <ControlStyle Width="50px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Valider" DataField="Valider" SortExpression="Valider" >
                    <ItemStyle CssClass="BoundFieldStyle" ForeColor="Blue"  />
                    <ControlStyle Width="50px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Fin" DataField="Fin" SortExpression="Fin" >
                    <ItemStyle CssClass="BoundFieldStyle" ForeColor="Red" />
                    <ControlStyle Width="50px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Invitation" DataField="Anonyme" SortExpression="Anonyme" >
                    <ItemStyle CssClass="BoundFieldStyle" ForeColor="Green"  />
                    <ControlStyle Width="50px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Publier" DataField="Publier" SortExpression="Publier" >
                    <ItemStyle CssClass="BoundFieldStyle" ForeColor="Blue"  />
                    <ControlStyle Width="50px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Anonymat" DataField="Anonymat" ReadOnly="true">
                    <ItemStyle CssClass="BoundFieldStyle" ForeColor="Red"  />
                    <ControlStyle Width="50px" />
                </asp:BoundField>

                <asp:BoundField HeaderText="Bloqué" DataField="Bloque" SortExpression="Bloque" Visible="false">
                    <ItemStyle CssClass="BoundFieldStyle" ForeColor="Red"  />
                    <ControlStyle Width="50px" />
                </asp:BoundField>

                <asp:BoundField DataField="DateCreation" HeaderText="Date" SortExpression="DateCreation" Visible="false">
                    <ItemStyle CssClass="BoundFieldDateStyle" Width="150px" />
                    <ControlStyle Width="130px"/>
                </asp:BoundField>

                <asp:HyperLinkField DataNavigateUrlFields="QuestionnaireID" DataNavigateUrlFormatString="~/Questionnaire/Edit.aspx?QuestionnaireID={0}" HeaderText="Editer" Text="&#187;&#187;&#187;" ItemStyle-CssClass="ItemStyle" />
            </Columns>
            <EmptyDataTemplate>
                <table border="0" cellpadding="10px"><tr><td><b>Pas de Questionnaires</b></td></tr></table>
            </EmptyDataTemplate>
        </asp:GridView>

        <!-- DataListMembreQuestionnaire -->
        <asp:SqlDataSource ID="SqlDataSourceQuestionnaireInfos" Runat="server" 
            ConnectionString="<%$ ConnectionStrings:QuestionnaireDB %>" />
            
        <asp:DataList ID="DataListQuestionnaireInfos" runat="server" 
            DataSourceID="SqlDataSourceQuestionnaireInfos" 
            OnItemDataBound="DataListQuestionnaireInfos_ItemDataBound">
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
                        <h4>Questionnaire Informations</h4>
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
                        ToolTip="Nombre de votants" />
                    <span class="HyperLinkColoredStyle">
                        <asp:HyperLink ID="HyperLink4" runat="server" 
                            NavigateUrl='<%# "~/Contact/Email.aspx" %>' 
                            ToolTip="Enquête Emails" 
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
            <UserControl:RolloverLink ID="RolloverLinkEdit" runat="server" NavigateURL="~/Questionnaire/Edit.aspx" Text="Nouveau" ToolTip="Créer un nouveau Questionnaire" />
        </td>
    </tr>        
    </table>             
    </div>
</div>
</asp:Content>