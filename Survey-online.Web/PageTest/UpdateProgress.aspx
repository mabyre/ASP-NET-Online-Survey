<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UpdateProgress.aspx.cs" Inherits="PageTest_UpdateProgress" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="ScriptManagerManage1" runat="server"  />

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
        <%=DateTime.Now.ToLongTimeString()%><br />
        <asp:Button ID="ButtonWait" runat="server" Text="Wait" OnClick="ButtonWait_Click" />
    </ContentTemplate>
    <Triggers> 
        <asp:AsyncPostBackTrigger ControlID="ButtonWait" /> 
    </Triggers> 
</asp:UpdatePanel>


<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" >
    <ProgressTemplate>
        <br />
        <asp:Label ID="LabelProgress" runat="server" Text="Action en cours ..." />
    </ProgressTemplate>
</asp:UpdateProgress>

<asp:Timer ID="Timer1" runat="server" >
</asp:Timer>

<br />
<asp:Label ID="LabelStatusChecker" runat="server" />

</asp:Content>

