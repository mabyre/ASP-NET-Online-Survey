<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PageSortGridViewSample.aspx.cs" Inherits="PageSortGridViewSample" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <h3>GridView Sort Example</h3>

      <table align="left">
        <tr>
          <td>
             Sort by:
            <asp:dropdownlist ID="SortList1" runat="server">
                <asp:listitem Selected="true">PersonneNom</asp:listitem>
                <asp:listitem>PersonnePrenom</asp:listitem>
                <asp:listitem>PersonneEmailBureau</asp:listitem>
            </asp:dropdownlist>
          </td>
          <td colspan="2">
            &nbsp;
          </td>
        </tr>
        <tr>
          <td>
            Then by:
              <asp:dropdownlist ID="SortList2" runat="server">
                <asp:listitem Selected="true">PersonneNom</asp:listitem>
                <asp:listitem>PersonnePrenom</asp:listitem>
                <asp:listitem>PersonneEmailBureau</asp:listitem>
              </asp:dropdownlist>
          </td>
          <td>
             Sort order:      
          </td>
          <td>
            <asp:radiobuttonlist id="DirectionList"
              runat="server">
              <asp:listitem selected="true">Ascending</asp:listitem>
              <asp:listitem>Descending</asp:listitem>
            </asp:radiobuttonlist>
          </td>
        </tr>
        <tr>
            <td colspan="3">
                  <asp:button id="SortButton"
                    text="Sort"
                    onclick="SortButton_Click" 
                    runat="Server"/>  
            </td>
        </tr>
      </table>


      <br/>
      <hr/>
      <br/>

    <table>
    <tr>
        <td>
                <asp:gridview id="CustomersGridView" 
                datasourceid="CustomersSource"
                autogeneratecolumns="true"
                emptydatatext="No data available." 
                allowpaging="true" 
                runat="server">
                </asp:gridview>
        </td>
    </tr>
    </table>       
            
      <asp:sqldatasource id="CustomersSource"
        selectcommand="Select [PersonneNom], [PersonnePrenom], [PersonneEmailBureau] From [Personne]"
        connectionstring="<%$ ConnectionStrings:QuestionnaireDB%>" 
        runat="server"/>

</asp:Content>

