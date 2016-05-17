<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ScoreControl.ascx.cs" Inherits="UserControl_ScoreControl" %>
<asp:HiddenField runat="server" ID="HiddenFieldScoreID"/>
<asp:Table ID="TableScore" runat="server" CssClass="TableQuestionControlStyle" CellPadding="4" Width="100%">
    <asp:TableRow>
        <asp:TableCell HorizontalAlign="Right" Width="200px">
            <asp:Label ID="LabelScoreMin" runat="server" CssClass="LabelStyle" 
                ToolTip="Donnez le score minimum" Text="Score minimum :" ></asp:Label>
        </asp:TableCell>
        <asp:TableCell HorizontalAlign="Left">
            <asp:TextBox ID="TextBoxScoreMin" runat="server" Width="400" />
        </asp:TableCell>
        <asp:TableCell HorizontalAlign="Right">
            <asp:ImageButton ID="ImageButtonSauverScore" CssClass="HyperLinkTesterStyle" runat="server" 
                ImageUrl="~/Images/Save.gif" ToolTip="Sauver le Score" />
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell HorizontalAlign="Right" Width="200px">
            <asp:Label ID="LabelScoreMax" runat="server" CssClass="LabelStyle" 
                ToolTip="Donnez le score maximum" Text="Score maximum :" ></asp:Label>
        </asp:TableCell>
        <asp:TableCell HorizontalAlign="Left">
            <asp:TextBox ID="TextBoxScoreMax" runat="server" Width="400" />
        </asp:TableCell>
        <asp:TableCell HorizontalAlign="Right">
            <asp:ImageButton ID="ImageButtonSupprimerScore" CssClass="HyperLinkTesterStyle" runat="server" 
                ImageUrl="~/Images/Delete.gif" ToolTip="Supprimer le Score" />
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell HorizontalAlign="Right" VerticalAlign="Top" Width="200px">
            <asp:Label ID="LabelScoreTexte" runat="server" CssClass="LabelStyle" 
                ToolTip="Donnez le texte du score" Text="Texte :" ></asp:Label>
        </asp:TableCell>
        <asp:TableCell HorizontalAlign="Left">
            <asp:TextBox ID="TextBoxScoreTexte" runat="server" Width="400" TextMode="MultiLine" Rows="3" />
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
