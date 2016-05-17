<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PageSortGridViewSample3.aspx.cs" Inherits="PageSortGridViewSample3" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <h3>GridView Sorting Example</h3>
    
        Choix : 
        <asp:dropdownlist ID="DropDownListChoix" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListChoix_SelectedIndexChanged">
        <asp:listitem>Nom Prénom</asp:listitem>
        <asp:listitem>Nom</asp:listitem>
        </asp:dropdownlist>


      <asp:label id="Message"
        forecolor="Red"
        runat="server"/>
        
      <br/>
        
      <asp:label id="SortInformationLabel"
        forecolor="Navy"
        runat="server"/>
                
      <br/>  

      <asp:gridview id="CustomersGridView" 
        datasourceid="CustomersSource" 
        autogeneratecolumns="true"
        allowpaging="true"
        emptydatatext="No data available." 
        allowsorting="true"
        onsorting="CustomersGridView_Sorting"
        onsorted="CustomersGridView_Sorted"  
        runat="server">
                
      </asp:gridview>
            
      <asp:sqldatasource id="CustomersSource"
        connectionstring="<%$ ConnectionStrings:QuestionnaireDB%>" 
        runat="server"/>
        
</asp:Content>

