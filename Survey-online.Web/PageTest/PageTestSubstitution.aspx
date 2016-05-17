<%@ Page Language="C#" MasterPageFile="~/PageTest/MasterPageTest.master" AutoEventWireup="true" CodeFile="PageTestSubstitution.aspx.cs" Inherits="PageTest_PageTestSubstitution" Title="Page sans titre" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h3>Substitution Class Example</h3>  

    <p>This section of the page is not cached:</p>

    <asp:substitution id="Substitution1"
      methodname="GetCurrentDateTime"
      runat="Server">
    </asp:substitution>

    <br />

    <p>This section of the page is cached:</p>

    <asp:label id="CachedDateLabel"
      runat="Server">
    </asp:label>

    <br /><br />

    <asp:button id="RefreshButton"
      text="Refresh Page"
      runat="Server">
    </asp:button>     

</asp:Content>

