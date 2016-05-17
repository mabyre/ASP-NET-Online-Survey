<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PageSortGridViewSample2.aspx.cs" Inherits="PageSortGridViewSample2" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <h3>GridView Sorting Example</h3>

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
        selectcommand="Select [PersonneNom], [PersonnePrenom], [PersonneEmailBureau] From [Personne]"
        connectionstring="<%$ ConnectionStrings:QuestionnaireDB%>" 
        runat="server"/>
        
</asp:Content>

