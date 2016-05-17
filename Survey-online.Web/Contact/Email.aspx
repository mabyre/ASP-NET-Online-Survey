<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" MaintainScrollPositionOnPostback="true" 
    EnableEventValidation="true" 
    AutoEventWireup="true" 
    CodeFile="Email.aspx.cs" 
    Inherits="Contact_Email" 
    Title="Gérer les emails" 
%>
<%@ Register Src="~/UserControl/LoadDocument.ascx" TagName="LoadDocument" TagPrefix="ucld" %>
<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<a name="HautDePage"></a>
<div id="body">
    <div class="DivPageCentreStyle">
    <h3><asp:Label ID="LabelTitre" runat="server" >Envois des emails aux Interviewés</asp:Label></h3>
    <h4>Gérez la liste des Interviewés</h4>

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

<asp:Panel ID="PanelNonImprime" runat="server" >
<!-- Aide en ligne sur le Formulaire -->
<asp:Panel ID="PanelAide" runat="server" class="PanelAideStyle">
<table class="TableCollapsePanel">
    <tr>
        <td align="left">
        Cochez les cases de la colonne <b>Envoyer</b> puis cliquez sur le bouton Envoyer pour envoyer des emails aux interviewés.<br />
        Entrez le nombre de <b>contacts par pages</b> que vous désirez voir affichés sur la page et tapez sur "Entrée" pour les afficher.<br />
        La liste choix <b>Lettre :</b> vous permet de sélectionner les contacts dont le <b>Nom</b> commence par cette lettre.<br />
        <!--Si vous avez beaucoup d'emails à envoyer, vous pouvez choisir de les envoyer de façon assynchrone, en cochant la case.<br /> -->
        Vous pouvez <b>Editer</b> un contact en cliquant dans la colonne Editer.<br />
        La colonne <b>Icône email envoyé</b> vous indique le nombre d'emails qui ont été envoyés à l'interviewé.<br />
        </td>
    </tr>
</table>
</asp:Panel>
    
    <table class="TableQuestionHautStyle" cellpadding="5" width="100%" border="0">
    <tr>
        <td align="left" class="TdCelluleIcone" >
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
        <td align="left" class="TdCelluleIcone" >
            <asp:ImageButton ID="ImageButtonPrint1" runat="server" 
                ImageUrl="~/Images/print.png"
                onclick="ImageButtonPrint_Click" 
                ToolTip="Formulaire d'impression"/>
        </td>
        <td align="left" class="TdCelluleIcone">
            <asp:ImageButton ID="ImageButtonExcel" runat="server" 
                ImageUrl="~/Images/excel.png"
                onclick="ImageButtonExcel_Click" 
                ToolTip="Fichier Excel"/>
        </td>
        <td>&nbsp;</td>
    </tr>
    </table>
    <table class="TableQuestionBasStyle" cellpadding="5" width="100%" border="0">
    <tr>
        <td align="left">
            <label class="LabelStyle" title="<%=Global.SettingsXml.ContactsParPageMax%> max">Contacts par page : </label>
            <asp:TextBox ID="TextBoxContactsParPage" runat="server" Width="30px" AutoPostBack="true" OnTextChanged="TextBoxContactsParPage_TextChanged" />
        </td>
        <td>
            <label class="LabelStyle">Questionnaire : </label>
            <usr:DropDownListQuestionnaires ID="DropDownListQuestionnaire" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListQuestionnaire_SelectedIndexChanged" />
        </td>
        <td>
            <label class="LabelStyle" title="Filtrer les interviewés dont le Nom commence par la lettre">Lettre : </label>
            <asp:DropDownList ID="DropDownListLettre" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListLettre_SelectedIndexChanged" />
        </td>
        <td align="right">
            <span class="SpanHyperLinkStyle">
            <a href="#BasDePage" class="HyperLinkStyle" title="Aller en bas de la Page">Bas</a>
            </span>
        </td>
    </tr>
    </table>

    <br />

    <table border="0" cellpadding="3" width="100%">
    <tr>
        <td align="left">
            <asp:CheckBox ID="CheckBoxDateVotes" CssClass="LabelStyle" runat="server" AutoPostBack="true" 
                Text="Date"
                ToolTip="Afficher la colonne Date des votes" 
                OnCheckedChanged="CheckBoxDateVotes_CheckedChanged" />
        </td>
    </tr>
    </table>
</asp:Panel>
    
    <div>
        <asp:SqlDataSource ID="SqlDataSourcePersonne" Runat="server" ConnectionString="<%$ ConnectionStrings:QuestionnaireDB %>"
            SelectCommand="SELECT [ID_Personne], [PersonneGUID],[PersonneEmailEnvois], [PersonneCivilite], [PersonneNom], [PersonnePrenom], [PersonneEmailBureau], [PersonneTelephonePerso], [PersonneSociete], [PersonneCode] FROM [Personne]">
        </asp:SqlDataSource>                       

        <asp:UpdatePanel ID="UpdatePanel22" runat="server">
            <ContentTemplate>
    
            <asp:GridView ID="GridViewContacts" runat="server" Width="100%" DataSourceID="SqlDataSourcePersonne"
                AllowPaging="true"
                AllowSorting="True" 
                AutoGenerateColumns="False" 
                DataKeyNames="ID_Personne" 
                OnRowCreated="GridViewContacts_RowCreated" 
                OnSorted="GridViewContacts_OnSorted" 
                OnSorting="GridViewContacts_OnSorting" 
                OnPageIndexChanged="GridViewContacts_PageIndexChanged"
                PagerSettings-Mode="NumericFirstLast" 
                PagerSettings-Position="TopAndBottom" 
                PagerStyle-HorizontalAlign="Right" 
                PagerStyle-Font-Bold="true"
                PagerSettings-PageButtonCount="5"
                CssClass="GridViewStyle"
                GridLines="Both"
                OnRowUpdated="GridViewContacts_RowUpdated" 
                OnLoad="GridViewContacts_Load" 
                OnRowDataBound="GridViewContacts_RowDataBound" 
                OnRowUpdating="GridViewContacts_RowUpdating">
                <HeaderStyle CssClass="GridViewHeaderStyle"/>
                <Columns>
                    <asp:TemplateField HeaderText="Envoyer">
                        <ItemStyle Width="60px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:CheckBox ID="CheckBoxEnvoyerEmail" runat="server"/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="LabelPersonneID" runat="server" Text='<%#Eval("ID_Personne")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="LabelPersonneGUID" runat="server" Text='<%#Eval("PersonneGUID")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="N°">
                        <ItemStyle CssClass="BoundFieldStyle"  />
                        <ItemTemplate>
                            <asp:Label ID="LabelNumero" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderImageUrl="~/App_Themes/Sodevlog/Images/email_envois.gif" FooterText="FooterTexte">
                        <ItemStyle CssClass="BoundFielCenterStyle" />
                        <ItemTemplate>
                            <asp:Label ID="LabelEmailEnvois" runat="server" Text='<%#Eval("PersonneEmailEnvois")%>' Visible='<%#(int)Eval("PersonneEmailEnvois")!=0%>' ToolTip="Nombre d'envois effectués"/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Vote">
                        <ItemStyle CssClass="BoundFieldEmailStyle"  />
                        <ItemTemplate>
                            <asp:Image ID="ImageVote" runat="server" />
                            <asp:Label ID="LabelVote" Text="X" runat="server" Visible="false"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Civilité" SortExpression="PersonneCivilite">
                        <ItemStyle CssClass="BoundFieldStyle" />
                        <ItemTemplate>
                            <asp:Label ID="LabelCivilite" runat="server" Text='<%#Eval("PersonneCivilite")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Nom" SortExpression="PersonneNom">
                        <ItemStyle CssClass="BoundFieldStyle" />
                        <ItemTemplate>
                            <asp:Label ID="LabelNom" runat="server" Text='<%#Eval("PersonneNom")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Prénom" SortExpression="PersonnePrenom">
                        <ItemStyle CssClass="BoundFieldStyle" />
                        <ItemTemplate>
                            <asp:Label ID="LabelPrenom" runat="server" Text='<%#Eval("PersonnePrenom")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Email" SortExpression="PersonneEmailBureau">
                        <ItemStyle CssClass="BoundFieldStyle" />
                        <ItemTemplate>
                            <asp:Label ID="LabelEmail" runat="server" Text='<%#Eval("PersonneEmailBureau")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Tél." SortExpression="PersonneTelephonePerso">
                        <ItemStyle CssClass="BoundFieldStyle" />
                        <ItemTemplate>
                            <asp:Label ID="LabelTelehphone" runat="server" Text='<%#Eval("PersonneTelephonePerso")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Société" SortExpression="PersonneSociete">
                        <ItemStyle CssClass="BoundFieldStyle" />
                        <ItemTemplate>
                            <asp:Label ID="LabelSociete" runat="server" Text='<%#Eval("PersonneSociete")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Code" Visible="false">                        
                        <ItemStyle CssClass="BoundFieldStyle" Width="50px" />
                        <ItemTemplate>
                            <asp:Label ID="LabelCode" runat="server" Text='<%#Eval("PersonneCode")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Date" Visible="true">                        
                        <ItemStyle CssClass="BoundFieldDateStyle" />
                        <ItemTemplate>
                            <asp:Label ID="LabelDateVotes" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Email" SortExpression="">
                        <ItemStyle CssClass="BoundFieldEmailStyle" />
                        <ItemTemplate>
                            <asp:HyperLink ID="Email" runat="server" Text="&#187;&#187;&#187;" NavigateUrl='<%# "mailto:" + Eval("PersonneEmailBureau").ToString() %>' ToolTip="Envoyer un email à l'interviewé" />
                        </ItemTemplate>
                        <ControlStyle Width="50px" />
                    </asp:TemplateField>

                    <asp:HyperLinkField DataNavigateUrlFields="ID_Personne" DataNavigateUrlFormatString="Edit.aspx?PersonneID={0}&Email=1" HeaderText="Editer" Text="&#187;&#187;&#187;" ItemStyle-CssClass="ItemStyle" />

                </Columns>
                <EmptyDataTemplate>
                    <table border="0" cellpadding="10px"><tr><td><b>Pas de contacts pour ce critère</b></td></tr></table>
                </EmptyDataTemplate>
            </asp:GridView>

            <asp:Panel ID="PanelNonImprime2" runat="server" >
            <table border="0" width="100%">
                <tr>
                    <td colspan="2" style="height:5px">
                    </td>
                </tr>
                <tr>
                    <td align="left" style="padding-left:15px;width:80px;" >
                        <asp:CheckBox ID="CheckBoxTousSelectionner" AutoPostBack="true" runat="server" 
                        Text=" Tous" 
                        ToolTip="Sélectionner tous les contacts de la page"
                        OnCheckedChanged="CheckBoxTousSelectionner_CheckedChanged" />
                    </td>
                    <td align="left" style="padding-left:15px;width:80px;" >
                        <asp:CheckBox ID="CheckBoxVotantsSelectionner" AutoPostBack="true" runat="server" 
                        Text=" Votants" 
                        ToolTip="Sélectionner tous les votants de la page"
                        OnCheckedChanged="CheckBoxVotantsSelectionner_CheckedChanged" />
                    </td>
                    <td align="left" style="padding-left:15px;width:110px;" >
                        <asp:CheckBox ID="CheckBoxNonVotantsSelectionner" AutoPostBack="true" runat="server" 
                        Text="Non votants" 
                        ToolTip="Sélectionner tous les non votants de la page"
                        OnCheckedChanged="CheckBoxNonVotantsSelectionner_CheckedChanged" />
                    </td>
                    <td align="left" style="padding-left:15px;width:80px" >
                        <asp:CheckBox ID="CheckBoxInverser" AutoPostBack="true" runat="server" 
                        Text=" Inverser" 
                        ToolTip="Inverser la sélection"
                        OnCheckedChanged="CheckBoxInverser_CheckedChanged" />
                    </td>
                    <td align="left" style="padding-left:100px;">
                        <asp:Button ID="ButtonSupprimer" CssClass="ButtonStyle" runat="server" Text="Supprimer" ToolTip="Attention : Suppression des contacts sélectionnés" OnClick="ButtonSupprimer_Click" />
                    </td>
                    <td align="right">
                        <i>Page <%=GridViewContacts.PageIndex + 1%> sur <%=GridViewContacts.PageCount%></i>          
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            </asp:Panel>
            
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdateProgress id="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel22">
            <progresstemplate>
                <asp:Image ID="loading2" runat="server" SkinID="ImageLoading" />
            </progresstemplate>
        </asp:UpdateProgress>
            
    </div>
    <table border="0" cellpadding="10px" height="80px" width="100%">
        <tr runat="server" id="TrBouton">
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                    <ContentTemplate>

                        <table style="border:none" cellpadding="0" cellspacing="0" >
                            <tr>
                                <td align="center" valign="middle">
                                    <UserControl:RolloverButton ID="ButtonEnvoyer" runat="server" Text="Envoyer" ToolTip="Envoyer un email de recrutement aux contacts sélectionnés" OnClick="ButtonEnvoyer_Click" />
                                </td>
                                <td align="center" valign="middle">
                                    <UserControl:RolloverButton ID="RolloverButtonAjouter" runat="server" Text="Ajouter" ToolTip="Ajouter de nouveaux contacts" PostBackUrl="~/Contact/Manage.aspx"/>
                                </td>
                                <td align="center" valign="middle" visible="false" runat="server"><!--la version assynchrone n'est pas pour tout de suite-->
                                    <asp:CheckBox  ID="CheckBoxEmailAssynchrone" runat="server" Text="Assynchrone" ToolTip="Envoyer les emails de façon assynchrone" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" height="30" align="center">
                                    <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server"/>
                                </td>
                            </tr>
                        </table>    
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>        
        <tr>
            <td align="center" height="35px">
                <asp:UpdateProgress ID="UpdateProgress2" DynamicLayout="true"  runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <progresstemplate>
                        <asp:Image ID="onsenfou1" runat="server" ImageUrl="~/App_Themes/Sodevlog/Images/email.gif"/>
                    </progresstemplate>
                </asp:UpdateProgress>
            </td>
        </tr>        
    </table>  

    <a name="BasDePage" ></a>

    <table style="border:none" cellpadding="0" cellspacing="0" runat="server" id="TableBoutonPageHaut">
        <tr>
            <td height="20px">
                <span class="SpanHyperLinkStyle">
                <a href="#HautDePage" class="HyperLinkStyle">Haut</a>
                </span>
            </td>
        </tr>
    </table>    
    </div>
</div>
</asp:Content>

