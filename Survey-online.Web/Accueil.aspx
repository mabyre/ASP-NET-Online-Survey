<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Accueil.aspx.cs" Inherits="Accueil" Title="Sodevlog - Questionnaire en ligne - Accueil" %>
<%@ Register TagPrefix="ucwc" TagName="WebContent" Src="~/UserControl/WebContent.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ucwc:WebContent ID="WebContentPageAccueil" runat="server" Section="PageAccueil" /> 
    <table cellpadding="2">
        <tr>
            <td class="TdControlButtonStyle" style="padding-left:30px">
                <UserControl:RolloverButton ID="RolloverButtonRepondez" runat="server" Text="Répondre" Visible="false" OnClick="RolloverButtonRepondez_Click" />
            </td>
        </tr>
    </table>
</asp:Content>

