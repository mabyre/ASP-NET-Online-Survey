<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="WebContent_Edit" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<a name="HautDePage"></a>
<div class="DivWebContentStyle">
        <h3>Edition d'une Page</h3>
        <div class="DivEditeurStyle">        
        <asp:Panel ID="PanelCommande" runat="server">

        <table border="0" cellpadding="2" width="100%">
        <tr>
            <td align="left" valign="top" width="16px">
                <a href="http://www.sodevlog.fr/Questionnaire.En.Ligne/page/Questionnaire-en-ligne-Aide.aspx#Pages" title="Aide sur la configuration des pages de l'application" target="_blank">
                    <img src="../App_Themes/Sodevlog/Images/help_rouge.gif" border="0" />
                </a>
            </td>
            <td align="right" width="260px">
                <strong>Section : </strong>
            </td>
            <td align="left">
                <UserControl:DropDownListWebContentSection ID="DropDownListWebContentSection" runat="server" AutoPostBack="false" OnSelectedIndexChanged="DropDownListWebContentSection_SelectedIndexChanged"/>                
            </td>
        </tr>
        </table>
    
        <asp:Panel ID="PanelAdmin" runat="server">
            <table border="0" cellpadding="2" width="100%">
            <tr>
                <td>&nbsp;</td>
                <td class="" align="right" width="260px">
                    <strong>Membre : </strong>
                </td>
                <td align="left">
                    <UserControl:DropDownListMembre ID="DropDownListMembre" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListMembre_SelectedIndexChanged"/>                
                </td>
            </tr>
            </table>
        </asp:Panel>
        
        <table border="0" cellpadding="2" width="100%">
        <tr>
            <td width="16px">&nbsp;</td>
            <td class="" align="right" width="260px">
                <strong>Visualiseur : </strong>
            </td>
            <td align="left">
                <UserControl:DropDownListQuestionnaire ID="DropDownListQuestionnaire" runat="server" AutoPostBack="false" EnableViewState="true"/>                
            </td>
        </tr>
        </table>    
        
        </asp:Panel>
        
        <asp:Panel ID="PanelPageInexistante" runat="server" Visible="false">
            <table border="0" cellpadding="2" width="100%">
            <tr>
                <td align="right" width="260px">
                </td>
                <td align="left">
                    <asp:Label ID="LabelPageInexistante" CssClass="LabelStyle" runat="server" Text="Cette page n'existe pas" />                
                </td>
            </tr>
            </table>    
        </asp:Panel>
        
        <table><tr><td height="10px"></td></tr></table>
        
        <FCKeditorV2:FCKeditor ID="FCKeditor1" runat="server" Height="500px" Width="100%"
            ToolbarSet="Default" SkinPath="skins/office2003/" BasePath="~/FCKeditor/" UseBROnCarriageReturn="true" />

        <table border="0">
        <tr>
              <td class="LabelStyle" width="160px" align="right">Transférer une image : </td>
              <td>
                <asp:FileUpload runat="server" ID="txtUploadImage" Width="400" />
                <asp:Button runat="server" ID="ButtonUploadImage" Text="Upload" OnClick="ButtonUploadImage_Click" />
              </td>
        </tr>
        <tr>
              <td class="LabelStyle"  width="160px" align="right">Transmettre un fichier : </td>
              <td>
                <asp:FileUpload runat="server" ID="txtUploadFile" Width="400" />        
                <asp:Button runat="server" ID="ButtonUploadFile" Text="Upload" OnClick="ButtonUploadFile_Click" />
              </td>
        </tr>
        </table>     
        <table style="border:none" cellpadding="25px" cellspacing="0" width="100%">
            <tr>
                <td height="30px" align="left">
                    <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
                </td>
            </tr>
        </table>        
    </div>
    <table border="0" width="100%" height="80px">
        <tr>
            <td align="center">
                <UserControl:RolloverButton ID="ButtonSauver" runat="server" />
                <UserControl:RolloverButton ID="ButtonSupprimer" runat="server" Text="Supprimer" ToolTip="Attention : Suppression de cette Page sans confirmation" OnClick="ButtonSupprimer_Click"/>                
                <UserControl:RolloverButton ID="ButtonRetour" runat="server" Text="Retour" ToolTip="Retourner à la gestion des pages" OnClick="ButtonRetour_Click"/>
            </td>
        </tr>
    </table>
    </div>
</asp:Content>

