<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="AddReaction.aspx.cs" Inherits="Poll_AddReaction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="body">
        <div class="DivFullWidth">
            <h4>
               Add Reaction</h4>
            <h5>Poll: <asp:Label ID="QuestionLabel" runat="server"></asp:Label></h5>
            <p>
                <asp:TextBox ID="ReactionTextBox" runat="server" TextMode="MultiLine" Rows="12" Columns="96"></asp:TextBox>
                <asp:RequiredFieldValidator ValidationGroup="group1" ID="RequiredFieldValidator1" runat="server"
                ControlToValidate="ReactionTextBox" ErrorMessage="Reaction is required">*</asp:RequiredFieldValidator>
            </p>
            <asp:LinkButton ValidationGroup="group1" ID="UpdateButton" runat="server" CausesValidation="True"
                Text="Add" OnClick="UpdateButton_Click">
            </asp:LinkButton>
            |
            <asp:LinkButton ValidationGroup="group1" ID="UpdateCancelButton" runat="server" CausesValidation="False"
                Text="Cancel" CommandName="Cancel" OnCommand="UpdateCancelButton_Command">
            </asp:LinkButton>
        </div>
    </div>
</asp:Content>