<%@ Page Language="C#" MasterPageFile="~/PageTest/MasterPageTest.master" AutoEventWireup="true" CodeFile="TableauExclusifs.aspx.cs" Inherits="PageTest_Tableau_Exclusifs" Title="Page sans titre" %>

<%@ Register 
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" />
<table cellspacing="20px">
    <tr>
        <td>    
            <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender1" runat="server" TargetControlID="RadioButton1" Key="RadioExclusiveColonne1"/>
            <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender11" runat="server" TargetControlID="RadioButton1" Key="RadioExclusiveLigne1"/>
            <asp:RadioButton runat="server" ID="RadioButton1" Text="radio11" />

            <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender2" runat="server" TargetControlID="RadioButton2" Key="RadioExclusive2"/>
            <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender12" runat="server" TargetControlID="RadioButton2" Key="RadioExclusiveLigne1"/>
            <asp:RadioButton runat="server" ID="RadioButton2" Text="radio21" />

            <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender3" runat="server" TargetControlID="RadioButton3" Key="RadioExclusive3"/>
            <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender13" runat="server" TargetControlID="RadioButton3" Key="RadioExclusiveLigne1"/>
            <asp:RadioButton runat="server" ID="RadioButton3" Text="radio31" />

            <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender15" runat="server" TargetControlID="RadioButton4" Key="RadioExclusiveLigne1"/>
            <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender4" runat="server" TargetControlID="RadioButton4" Key="RadioExclusive4"/>
            <asp:RadioButton runat="server" ID="RadioButton4" Text="radio41" />

            <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender14" runat="server" TargetControlID="RadioButton5" Key="RadioExclusiveLigne1"/>
            <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender5" runat="server" TargetControlID="RadioButton5" Key="RadioExclusive5"/>
            <asp:RadioButton runat="server" ID="RadioButton5" Text="radio51" />
        </td>
    </tr>
    <tr>
        <td>    
            <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender6" runat="server" TargetControlID="RadioButton6" Key="RadioExclusiveColonne1"/>
            <asp:RadioButton runat="server" ID="RadioButton6" Text="radio12" />
            <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender7" runat="server" TargetControlID="RadioButton7" Key="RadioExclusive2"/>
            <asp:RadioButton runat="server" ID="RadioButton7" Text="radio22" />
            <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender8" runat="server" TargetControlID="RadioButton8" Key="RadioExclusive3"/>
            <asp:RadioButton runat="server" ID="RadioButton8" Text="radio32" />
            <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender9" runat="server" TargetControlID="RadioButton9" Key="RadioExclusive4"/>
            <asp:RadioButton runat="server" ID="RadioButton9" Text="radio42" />
            <ajaxToolkit:MutuallyExclusiveCheckBoxExtender ID="MutuallyExclusiveCheckBoxExtender10" runat="server" TargetControlID="RadioButton10" Key="RadioExclusive5"/>
            <asp:RadioButton runat="server" ID="RadioButton10" Text="radio52" />
        </td>
    </tr>
    <tr>
        <td>    
            <asp:CheckBox runat="server" ID="CheckBox1" runat="server"/>
            <asp:CheckBox runat="server" ID="CheckBox4" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox runat="server" ID="CheckBox2" runat="server"/>
            <asp:CheckBox runat="server" ID="CheckBox5" runat="server"/>
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox runat="server" ID="CheckBox3" runat="server"/>
            <asp:CheckBox runat="server" ID="CheckBox6" runat="server"/>
        </td>
    </tr>
</table>
</asp:Content>

