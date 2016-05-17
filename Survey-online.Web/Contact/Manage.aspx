<%@ Page Language="C#" 
    MasterPageFile="~/MasterPage.master" 
    MaintainScrollPositionOnPostback="true" 
    EnableEventValidation="true" 
    AutoEventWireup="true" 
    CodeFile="Manage.aspx.cs" 
    Inherits="Contact_Manage" 
    Title="Contacts Questionnaires" 
    ValidateRequest="false"
%>
<%@ Register Src="~/UserControl/LoadDocument.ascx" TagName="LoadDocument" TagPrefix="ucld" %>

<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<a name="HautDePage"></a>
<div class="DivPageStyle">
<h3>Importez des Interviewés</h3>
<asp:Panel ID="PanelLoadDocument" runat="server" >
<table class="TableQuestionStyle" cellpadding="4" width="100%" border="0">
    <tr>
        <td align="left" valign="top">
            <a href="http://www.sodevlog.fr/Questionnaire.En.Ligne/page/Questionnaire-en-ligne-Aide.aspx#ImportFichierInterviewe" 
                title="Aide en ligne sur l'import des fichiers pour connaitre le format des fichiers à importer" target="_blank">
                <img src="../App_Themes/Sodevlog/Images/help_rouge.gif" border="0" />
            </a>
        </td>
        <td colspan="2" align="left" height="50px">
            <ucld:LoadDocument ID="LoadDocument" runat="server" />
        </td>
    </tr>
    <tr>
        <td align="left" width="10%">
            <usr:CheckBoxListExclusive ID="CheckBoxListChoixParsing" runat="server" CssClass="CheckBoxListStyle" AutoPostBack="true" >
            </usr:CheckBoxListExclusive>
        </td>
        <td align="left" colspan="2">
            <table border="0" width="100%">
            <tr>
                <td>
                    <table border="0" width="100%">
                        <tr>
                            <td width="140px" align="right">
                                <label>Questionnaire :</label>
                            </td>
                            <td>
                                <asp:Label ID="LabelQuestionnaire" runat="server" CssClass="LabelStyle"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td width="140px" align="right">
                                <label>Code d'accès :</label>
                            </td>
                            <td>
                                <asp:Label ID="LabelCodeAccess" runat="server" CssClass="LabelStyle"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td height="13px">
        </td>
    </tr>
</table>

<!-- Aide en ligne sur le Formulaire -->
<asp:Panel ID="PanelAide" runat="server" class="PanelAideStyle">
<table class="TableCollapsePanel">
    <tr>
        <td>
        Importez vos contacts afin de les interviewer.<br />
        <br />
        <b>Par Email :</b><br />
        Si vous avez le nom et l'adresse email de vos contacts, format de la liste : <b>nom complet (adresse@email.com)</b><br />
        Vous pouvez ajouter autant de contacts séparés par des <b>";"</b><br />
        Exemple:<br />
        nom complet1 (adresse1@email.com);nom complet2 (adresse2@email.com);nom complet3 (adresse3@email.com);<br />
        Si vous n'avez que les adresses emails de vos contacts, format de la liste : <b>adresse@email.com</b><br />
        Vous pouvez ajouter autant de contacts séparés par des <b>"retours chartiots"</b><br />
        Exemple:<br />
        adresse4@email.com<br />
        adresse5@email.com<br />
        adresse6@email.com<br />
        <br />
        <b>Par Téléphone :</b><br />
        Les mots clefs : <b>Téléphone</b> ou <b>Nom[Téléphone]</b> sont insérer en début de liste.<br />
        Formule Regex : <b>^[0-9\s\(\)\+\-\.]+$</b><br />
        Format de la liste, exemple :<br />
        Téléphone<br />
        0102030405<br />
        01 02 03 04 05<br />
        +(33)1-02<br />
        Ou bien, exemple :<br />
        Nom[Téléphone]<br /> 
        nom1 complet1 [0602030405]<br />
        nom2 complet2 [(+33)06.02.03.04.05]<br />
        nom3 complet3 [06-02]<br />
        <br />
        <b>Supprimer une liste d'interviewés :</b><br />
        Entrez une liste d'adresses emails et/ou de numéros de téléphone séparés par des "retours chartiots"<br />
        puis cliquez sur la croix rouge.
        </td>
    </tr>
</table>
</asp:Panel>

<table class="TableQuestionStyle" cellpadding="4" width="100%" border="0" >    
    <tr>
        <td width="16px" valign="top">
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
        <td align="right" width="210px" valign="top">
            <label class="LabelStyle" title="Format de liste des contacts : nom complet (adresse@email.com); ... ou adresse@email.com" >Contacts à importer : </label>
        </td>
        <td align="left">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <asp:TextBox ID="TextBoxContactAjouter" runat="server" Width="395px" TextMode="MultiLine" Rows="5"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;
                        <asp:Button ID="ButtonAjouterContacts" runat="server" 
                            onclick="ButtonAjouterContacts_Click" Text="Ajouter" 
                            ToolTip="Ajouter la liste des contacts" />
                        <br />
                        &nbsp;
                        <asp:ImageButton ID="ImageButtonSupprimerListeContacts" runat="server"
                            ImageUrl="~/Images/Delete.gif" 
                            ToolTip="Supprimer une liste de contacts" 
                            OnClick="ImageButtonSupprimerListeContacts_Click" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td height="15px">
        </td>
    </tr>
</table>

<table style="border:none" cellpadding="15px" cellspacing="0">
    <tr>
        <td>
            <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false"/>
        </td>
    </tr>
</table>

</asp:Panel>

<asp:Panel ID="PanelQuestionnaireBloque" runat="server" Visible="false">
<table style="border:solid 1px" cellpadding="0" cellspacing="0" width="80%">
    <tr>
        <td height="180px">
            <label>Questionnaire bloqué pendant la campagne d'interview.</label>
        </td>
    </tr>
</table>
</asp:Panel>

<asp:Panel ID="PanelValiderContactImport" runat="server" Visible="false">
<table border="0">
<tr>
    <td class="TdControlButtonStyle" align="left">
        <UserControl:RolloverButton ID="ButtonValider" runat="server" Text="Valider" OnClick="ButtonValider_Click" ToolTip="Valider les Contacts importés" />                
    </td>
</tr>        
</table>      
</asp:Panel>

    <div class="DivFullWidth">
    <table border="0" cellpadding="10" height="30">
        <tr>
            <td>
                <label class="LabelListUserStyle">Interviewés : </label>
                <asp:Label ID="LabelNombreContacts" runat="server" ></asp:Label>
            </td>
        </tr>
    </table>     

    <table border="0" class="TableQuestionStyle" cellpadding="4" cellspacing="0" height="55" width="100%">
    <tr>
        <td align="left">
            <label class="LabelStyle" title="<%=Global.SettingsXml.ContactsParPageMax%> max">Contacts par page : </label>
            <asp:TextBox ID="TextBoxContactsParPage" runat="server" Width="30px" CausesValidation="false" OnTextChanged="TextBoxContactsParPage_TextChanged" />
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
    
        <asp:SqlDataSource ID="SqlDataSourcePersonne" Runat="server" ConnectionString="<%$ ConnectionStrings:QuestionnaireDB %>"
            SelectCommand="SELECT [ID_Personne], [Civilite], [PersonneNom], [PersonnePrenom], [PersonneEmailBureau], [PersonneSociete], [PersonneCode] FROM [Personne]"
            DeleteCommand="DELETE FROM [Personne] WHERE [ID_Personne] = @original_ID_Personne" 
            UpdateCommand="UPDATE [Personne] SET PersonneCivilite = @PersonneCivilite, PersonneNom = @PersonneNom, PersonnePrenom = @PersonnePrenom, PersonneEmailBureau = @PersonneEmailBureau, PersonneTelephonePerso = @PersonneTelephonePerso, PersonneSociete = @PersonneSociete WHERE [ID_Personne] = @original_ID_Personne"
            OldValuesParameterFormatString="original_{0}">
        </asp:SqlDataSource>                       

        <asp:UpdatePanel ID="UpdatePanelGridViewContacts" runat="server">
            <ContentTemplate>
            <asp:GridView ID="GridViewContacts" runat="server" Width="100%" DataSourceID="SqlDataSourcePersonne"
                AllowPaging="True"
                AllowSorting="True" 
                AutoGenerateColumns="False" 
                DataKeyNames="ID_Personne" 
                OnRowCreated="GridViewContacts_RowCreated" 
                OnSorted="GridViewContacts_OnSorted" 
                OnSorting="GridViewContacts_OnSorting" 
                OnPageIndexChanged="GridViewContacts_PageIndexChanged"
                OnRowUpdating="GridViewContacts_RowUpdating"
                PagerSettings-Mode="NumericFirstLast" 
                PagerSettings-Position="TopAndBottom" 
                PagerStyle-HorizontalAlign="Right" 
                PagerStyle-Font-Bold="true"
                PagerSettings-PageButtonCount="5"
                CssClass="GridViewStyle"
                GridLines="Both"
                OnRowCommand="GridViewContacts_RowCommand">
                <HeaderStyle CssClass="GridViewHeaderStyle"/>
                <Columns>
                    <asp:CommandField ItemStyle-Width="50px" ShowDeleteButton="True" ShowEditButton="False" CausesValidation="true" ButtonType="Image" DeleteImageUrl="~/Images/Delete.gif" EditImageUrl="~/Images/Edit.gif" UpdateImageUrl="~/Images/Save.gif" CancelImageUrl="~/Images/Annul.gif"/>
                    <asp:BoundField HeaderText="Civilité" DataField="PersonneCivilite" SortExpression="PersonneCivilite">
                        <ItemStyle CssClass="BoundFieldStyle" />
                        <ControlStyle CssClass="BoundFieldControlStyle" Width="90px" />
                        <HeaderStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Nom" DataField="PersonneNom" SortExpression="PersonneNom">
                        <ItemStyle CssClass="BoundFieldStyle" />
                        <ControlStyle CssClass="BoundFieldControlStyle" />
                        <HeaderStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Prénom" DataField="PersonnePrenom" SortExpression="PersonnePrenom">
                        <ItemStyle CssClass="BoundFieldStyle" />
                        <ControlStyle CssClass="BoundFieldControlStyle" />
                        <HeaderStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="E-Mail" DataField="PersonneEmailBureau"  SortExpression="PersonneEmailBureau">
                        <ItemStyle CssClass="BoundFieldStyle" />
                        <HeaderStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Tél." DataField="PersonneTelephonePerso"  SortExpression="PersonneTelephonePerso">
                        <ItemStyle CssClass="BoundFieldStyle" />
                        <HeaderStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Société" DataField="PersonneSociete" SortExpression="PersonneSociete" ReadOnly="false">
                        <ItemStyle CssClass="BoundFieldStyle" />
                        <HeaderStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:HyperLinkField DataNavigateUrlFields="ID_Personne" DataNavigateUrlFormatString="Edit.aspx?PersonneID={0}" HeaderText="Editer" Text="&#187;&#187;&#187;" ItemStyle-CssClass="ItemStyle"/>
                </Columns>
                <EmptyDataTemplate>
                    <table border="0" cellpadding="10px"><tr><td><b>Pas de contacts pour ce critère</b></td></tr></table>
                </EmptyDataTemplate>
                <PagerSettings Mode="NumericFirstLast" PageButtonCount="5" Position="TopAndBottom" />
                <PagerStyle Font-Bold="True" HorizontalAlign="Right" />
            </asp:GridView>

            <table width="100%" cellpadding="5" border="0">
                <tr>
                    <td align="right">
                        <i>Page <%=GridViewContacts.PageIndex + 1%> sur <%=GridViewContacts.PageCount%></i>          
                    </td>
                </tr>
            </table>

            <table style="border:none" cellpadding="15px" cellspacing="0">
                <tr>
                    <td>
                        <asp:Label ID="LabelValidationMassageGridView" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false"/>
                    </td>
                </tr>
            </table>            
            
            </ContentTemplate>
            
        </asp:UpdatePanel>

        <asp:UpdateProgress id="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelGridViewContacts">
            <progresstemplate>
                <asp:Image ID="loading2" runat="server" SkinID="ImageLoading" />
            </progresstemplate>
        </asp:UpdateProgress>
            
    <a name="BasDePage" ></a>
    
    <table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td class="TdControlButtonStyle">
            <UserControl:RolloverLink ID="RolloverLinkEdit" runat="server" NavigateURL="~/Contact/Edit.aspx" Text="Nouveau" ToolTip="Ajouter un nouveau Contact" />
        </td>
        <td class="TdControlButtonStyle">
            <UserControl:RolloverLink ID="RolloverLinkEmail" runat="server" NavigateURL="~/Contact/Email.aspx" Text="Supprimer" ToolTip="Supprimer des contacts" />
        </td>
    </tr>        
    <tr>
        <td height="20px" colspan="2">
            <span class="SpanHyperLinkStyle">
            <a href="#HautDePage" class="HyperLinkStyle">Haut</a>
            </span>
        </td>
    </tr>
    </table>   
       </div>
</div>
</asp:Content>

