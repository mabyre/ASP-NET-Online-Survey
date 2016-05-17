<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoadDocument.ascx.cs" Inherits="Controls_LoadDocument" %>

<table cellspacing="0" cellpadding="3" border="0">
    <tr>
        <td align="right">
            <asp:Label ID="Label5" CssClass="LabelStyle" Runat="server" Text="Fichier à importer :" Width="140px"/>
        </td>
        <td align="left">
            <Input class="BoutonStyle" type="file" ID="DocumentNom" size="50" runat="server" />&nbsp
        </td>
        <td>
			<asp:Button ID="ButtonAdd" runat="server" ToolTip="Ajouter un fichier à sélectionner avec Parcourir..." OnClick="ButtonAdd_OnClick" Text="Ajouter" />
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:Label ID="ValidationMessage" CssClass="LabelValidationMessageStyle" Runat="server" Visible="false" />
        </td>
    </tr>
</table>
