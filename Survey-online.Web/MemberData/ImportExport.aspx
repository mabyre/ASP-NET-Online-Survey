<%@ Page Language="C#"  
    MasterPageFile="~/MasterPage.master " 
    AutoEventWireup="true" 
    CodeFile="ImportExport.aspx.cs" 
    Inherits="MemberData_ImportExport" 
    MaintainScrollPositionOnPostback="true"
    Title="Importer/Exporter des données Membre" %>

<%@ Register Src="~/UserControl/LoadDocument.ascx" TagName="LoadDocument" TagPrefix="ucld" %>

<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<a name="HautDePage"></a>
<div class="DivPageStyle">

<asp:Panel ID="PanelAdministrateur" runat="server" Visible="false">
<table class="TableQuestionStyleAdministrateur" cellpadding="4" width="100%" border="0">
    <tr>
        <td align="right">
            <label class="LabelStyle" title="L'Administrateur doit choisir un membre pour Importer/Exporter les données">Membre :</label>
        </td>
        <td>
            <UserControl:DropDownListMembre ID="DropDownListMembre" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListMembre_SelectedIndexChanged" />                
        </td>
    </tr>
    <td colspan="2" align="left">
        <asp:Label ID="LabelValidationMessageAdmin" CssClass="LabelValidationMessageErrorStyle" Runat="server" Visible="false" />
    </td>
</table>
<br />
</asp:Panel>

<h3>Importez des Données Membre</h3>
<asp:Panel ID="PanelAide2" runat="server" class="PanelAideStyle">
<table class="TableCollapsePanel">
    <tr>
        <td>
        Choisissez un fichier d'interview au format <b>.xml</b> à importer.<br />
        Importez vos Questionnaires, choisissez d'importer les Interviewés, les Votes, la configuration du Serveur Smtp.<br />
        Analyser votre fichier pour déterminer les objets qu'il contient.<br />
        <br />
        Si votre fichier est trop important pour être traité en une seule passe, en cliquant sur <b>"Analyser"</b> le traitement se terminera par le message <b>"Traitement non terminé."</b><br />
        Pour connaitre le résultat de l'analyse, vous devez alors cliquez sur <b>"Afficher"</b> à la fin du traitement, attendez quelques dizaines de secondes.<br />
        Tant que le traitement n'est pas terminé, le message <b>"Traitement non terminé."</b> s'affichera.<br />
        <br />
        Même chose pour l'importation, cliquez sur <b>"Importer"</b> si le message <b>"Traitement non terminé."</b> s'affiche, attendez quelques dizaines de secondes, cliquez sur <b>"Afficher"</b> pour connaitre le résultat de l'importation.<br />
        <br />
        <b>Patience, certains traitements peuvent prendre plusieurs minutes.</b>
        </td>
    </tr>
</table>
</asp:Panel>

<asp:Panel ID="PanelLoadDocument" runat="server" >
<table class="TableQuestionStyle" cellpadding="4" width="100%" border="0">
    <tr>
        <td align="left" valign="top">
            <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="Server" 
                AutoCollapse="false"  
                AutoExpand="false"
                TargetControlID="PanelAide2"
                ExpandControlID="PanelControl2"
                CollapseControlID="PanelControl2" 
                Collapsed="true"
                ImageControlID="Image1"    
                ExpandedImage="~/App_Themes/Sodevlog/Images/help.gif"
                CollapsedImage="~/App_Themes/Sodevlog/Images/help.gif"
                SuppressPostBack="true"
                SkinID="CollapsiblePanel" />  
            <asp:Panel ID="PanelControl2" runat="server" CssClass="CollapsePanelHeader"> 
                <img src="../App_Themes/Sodevlog/Images/help.gif" border="0" alt ="Cliquez ici pour afficher l'aide"/>
            </asp:Panel>
        </td>
        <td align="left" height="50px">
        <table cellspacing="0" cellpadding="3" border="0">
            <tr>
                <td align="right">
                    <asp:Label ID="Label5" CssClass="LabelStyle" Runat="server" Text="Fichier à importer :" Width="140px"/>
                </td>
                <td align="left">
                    <input class="BoutonStyle" type="file" ID="DocumentNom" size="50" runat="server" />&nbsp
                </td>
            </tr>
        </table>
        <table>
        <tr>
            <td align="right" style="width:200px">
                <label class="LabelStyle" title="Importer les Interveiwés">Interviewés :</label>
            </td>
            <td align="left" class="TdTextBoxStyle">
                <asp:CheckBox ID="CheckBoxImportInterviewes" runat="server" CssClass="TextBoxStyle" Checked="true" />
            </td>
        </tr>
        <tr>
            <td align="right" style="width:200px">
                <label class="LabelStyle" title="Importer les Interveiwés">Votes :</label>
            </td>
            <td align="left" class="TdTextBoxStyle">
                <asp:CheckBox ID="CheckBoxImportVotes" runat="server" CssClass="TextBoxStyle" Checked="true"/>
            </td>
        </tr>
        <tr>
            <td align="right" style="width:200px">
                <label class="LabelStyle" title="Importer la configuration du serveur smtp">Serveur Smtp :</label>
            </td>
            <td align="left" class="TdTextBoxStyle">
                <asp:CheckBox ID="CheckBoxImportServeurSmtp" runat="server" CssClass="TextBoxStyle" Checked="false"/>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
                <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
            </td>
        </tr>
        </table>
        </td>
    </tr>
</table>
</asp:Panel>

<asp:Panel ID="PanelImportAttente" runat="server" Visible="false" >
    <br />
    <asp:Image ID="loading2" runat="server" SkinID="ImageLoading" ToolTip="Le traitement n'est pas terminé, cliquez sur Afficher." />
</asp:Panel>

<table cellpadding="15px" width="100%">
<tr>
    <td align="center">
        <UserControl:RolloverButton ID="ButtonAnalyserImportMemberData" runat="server" ToolTip="Analyser les données membre du fichier sélectionné avec Parcourir..." OnClick="ButtonAnalyserImportMemberData_OnClick" Text="Analyser" />
        <UserControl:RolloverButton ID="ButtonImportMemberData" runat="server" ToolTip="Ajouter les données membre du fichier sélectionné avec Parcourir..." OnClick="ButtonImportMemberData_OnClick" Text="Importer" />
        <UserControl:RolloverButton ID="ButtonAfficherResultats" runat="server" ToolTip="Afficher les résultats des traitements qui prennent du temps" OnClick="ButtonAfficherResultats_OnClick" Text="Afficher" />
    </td>
</tr>
</table>

<h3>Exportez des Données Membre</h3>
<asp:Panel ID="PanelAide3" runat="server" class="PanelAideStyle">
<table class="TableCollapsePanel">
    <tr>
        <td>
        Exportez toutes vos données dans un fichier au format <b>.xml</b> en laissant "Tous les Questionnaires".<br />
        Ou choisissez un Questionnaire pour exporter les données concernant ce Questionnaire.<br />
        <br />
        Cliquez sur <b>"Exporter"</b> pour créer le fichier d'exportation que vous pourrez télécharger.<br />
        Même traitement que pour l'importation, si le message <b>"Traitement non terminé."</b> apparait, vous devez attendre la fin du traitement et cliquer sur <b>"Afficher"</b> pour que le lien de téléchargement de votre fichier apparaisse.<br />
        Cliquez ensuite sur le lien vers votre fichier pour le télécharger.<br />
        <br />
        <b>Remarque importante : </b>Si votre fichier est de taille importante (observez l'information - Taille du fichier), après avoir cliqué sur lien de téléchargement, attendez que tout le fichier soit bien téléchargé dans la page de votre navigateur avant de faire <b>"Fichier" "Enregistrez sous..."</b>.<br />
        Revenez ensuite sur la page "Exportez des Données Membre" pour supprimer votre fichier d'export de données en cliquant sur <b>"Supprimer"</b>, pour ne pas laisser votre fichier sur le serveur.<br />
        <br />
        <b>Patience, certains traitements peuvent prendre plusieurs minutes.</b>
        </td>
    </tr>
</table>
</asp:Panel>

<asp:Panel ID="Panel1" runat="server" >
<table class="TableQuestionStyle" cellpadding="4" width="100%" border="0">
    <tr>
        <td align="left" valign="top" rowspan="4" >
            <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="Server" 
                AutoCollapse="false"  
                AutoExpand="false"
                TargetControlID="PanelAide3"
                ExpandControlID="PanelControl3"
                CollapseControlID="PanelControl3" 
                Collapsed="true"
                ImageControlID="Image1"    
                ExpandedImage="~/App_Themes/Sodevlog/Images/help.gif"
                CollapsedImage="~/App_Themes/Sodevlog/Images/help.gif"
                SuppressPostBack="true"
                SkinID="CollapsiblePanel" />  
            <asp:Panel ID="PanelControl3" runat="server" CssClass="CollapsePanelHeader"> 
                <img src="../App_Themes/Sodevlog/Images/help.gif" border="0" alt ="Cliquez ici pour afficher l'aide"/>
            </asp:Panel>
        </td>
        <td align="right" style="width:200px">
            <label class="LabelStyle" title="Choisir un Questionnaire à exporter">Questionnaires :</label> 
        </td>
        <td align="left" style="width:70%">
            <usr:DropDownListQuestionnaire ID="DropDownListQuestionnaire" runat="server" />
        </td>
    </tr>
    <tr>
        <td align="right" style="width:200px">
            <label class="LabelStyle" title="Importer les Interveiwés">Interviewés :</label>
        </td>
        <td align="left" class="TdTextBoxStyle">
            <asp:CheckBox ID="CheckBoxExportIntervieves" runat="server" CssClass="TextBoxStyle" Checked="true" />
        </td>
    </tr>
    <tr>
        <td align="right" style="width:200px">
            <label class="LabelStyle" title="Importer les Interveiwés">Votes :</label>
        </td>
        <td align="left" class="TdTextBoxStyle">
            <asp:CheckBox ID="CheckBoxExportVotes" runat="server" CssClass="TextBoxStyle" Checked="true"/>
        </td>
    </tr>
    <tr>
        <td align="right" style="width:200px">
            <label class="LabelStyle" title="Importer la configuration du serveur smtp">Serveur Smtp :</label>
        </td>
        <td align="left" class="TdTextBoxStyle">
            <asp:CheckBox ID="CheckBoxExportServeurSmtp" runat="server" CssClass="TextBoxStyle" Checked="false"/>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:Label ID="LabelMessageExport" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
        </td>
    </tr>
</table>
</asp:Panel>

<asp:Panel ID="PanelExportAttente" runat="server" Visible="false" >
    <br />
    <asp:Image ID="onsenfoue45654" runat="server" SkinID="ImageLoading" ToolTip="Le traitement n'est pas terminé, cliquez sur Afficher." />
</asp:Panel>

<table cellpadding="15px" width="100%">
<tr>
    <td align="center">
        <UserControl:RolloverButton ID="ButtonExportMemberData" runat="server" 
            Text="Exporter" 
            ToolTip="Créez vos données pour les exporter" 
            OnClick="ButtonExportMemberData_Click" />
        <UserControl:RolloverButton ID="ButtonAfficherResultExport" runat="server" 
            ToolTip="Afficher les résultats des traitements qui prennent du temps" 
            OnClick="ButtonAfficherResultatsExport_OnClick" Text="Afficher" />
        <UserControl:RolloverLink ID="ButtonSupprimerMemberData" runat="server"
            Text="Supprimer" 
            OnClick="ButtonSupprimerMemberData_Click"
            ToolTip="Supprimer votre fichier de téléchargement et vos données sur le serveur" />
    </td>
</tr>
</table>

</div>

</asp:Content>

