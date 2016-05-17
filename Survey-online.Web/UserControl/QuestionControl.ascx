<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QuestionControl.ascx.cs" Inherits="UserControl_QuestionControl" %>
<asp:Table ID="TableQuestionEnchainee" runat="server" CssClass="TableQuestionControlStyle" CellPadding="4" Width="100%">
    <asp:TableRow>
        <asp:TableCell HorizontalAlign="Right" Width="200px">
            <asp:Label ID="LabelQuestionEnchainee" runat="server" CssClass="LabelStyle" ToolTip="Donnez un libellé à votre Question" Text="Libellé de la Question" ></asp:Label>
        </asp:TableCell>
        <asp:TableCell HorizontalAlign="Left">
            <asp:TextBox ID="TextBoxQuestionEnchainee" runat="server" Width="400" />
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell HorizontalAlign="Right" Width="200px">
            <asp:Label ID="LabelReponse" runat="server" CssClass="LabelStyle" ToolTip="Donnez les Réponse à votre Question séparées par des ;" Text="Réponses :" ></asp:Label>
        </asp:TableCell>
        <asp:TableCell HorizontalAlign="Left">
            <asp:TextBox ID="TextBoxReponses" runat="server" Width="400" />
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell HorizontalAlign="Right">
            <label class="LabelStyle">Type de la Question :</label>
        </asp:TableCell>
        <asp:TableCell HorizontalAlign="Left">
            <asp:DropDownList ID="DropDownListTypeQuestionReponse" runat="server">
            </asp:DropDownList>
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell HorizontalAlign="Right">
            <label class="LabelStyle" title="Obliger l'intervieweur à répondre.">Obligatoire :</label>
        </asp:TableCell>
        <asp:TableCell HorizontalAlign="Left">
           <asp:CheckBox ID="CheckBoxQuestionObligatoire" runat="server" 
                CssClass="LabelStyle"/>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
