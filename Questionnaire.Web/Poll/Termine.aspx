<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Termine.aspx.cs" Inherits="Poll_Termine" Title="Untitled Page" %>
<%@ Register TagPrefix="uc" TagName="WebContent" Src="~/UserControl/WebContent.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
        <uc:WebContent ID="WebContent1" runat="server" Section="PageTermine" /> 
        <asp:Panel ID="PanelScore" runat="server" Visible="false">
        <table border="0" cellpadding="3" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="LabelResultat" CssClass="LabelStyle" Runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LabelScoreTexte" CssClass="LabelValidationMessageStyle" Runat="server" />
            </td>
        </tr>
        </table>            
        </asp:Panel>
    </div>
</asp:Content>

