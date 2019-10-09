<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Words.aspx.cs" Inherits="WordBank.Words" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        
    <p>
        Hello <%:Session["Username"]%>, here are your personal word stats.
    </p>
      <asp:GridView id="GridView" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" > 
    <Columns>
<%--        <asp:CommandField ShowDeleteButton="True" 
            ShowEditButton="True" />--%>
        <asp:BoundField DataField="Word" 
            HeaderText="Word" ReadOnly="True"
            SortExpression="Word" />
        <asp:BoundField DataField="Definition" 
            HeaderText="Definition" InsertVisible="False"
            ReadOnly="True" SortExpression="Definition" />
        <asp:BoundField DataField="CorrectWord" 
            HeaderText="Correct Word Attempts" SortExpression="CorrectWord" />
                <asp:BoundField DataField="WordAttempts" 
            HeaderText="Total Word Attempts" SortExpression="WordAttempts" />
        <asp:BoundField DataField="CorrectDefinition" 
            HeaderText="Correct Definition Attempts" SortExpression="CorrectDefinition" />
                <asp:BoundField DataField="DefinitionAttempts" 
            HeaderText="Total Definition Attempts" SortExpression="DefinitionAttempts" />
    </Columns>


      </asp:GridView>
</asp:Content>
