<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Words.aspx.cs" Inherits="WordBank.Words" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        
    <p>
        Hello <%:Session["Username"]%>, here are your personal word stats.
    </p>
      <asp:GridView id="GridView" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" > 
        <Columns>
            <asp:CommandField ShowDeleteButton="True" 
                ShowEditButton="True" />
            <asp:BoundField DataField="Word" 
                HeaderText="Word" ReadOnly="True"
                SortExpression="Word" />
            <asp:BoundField DataField="Definition" 
                HeaderText="Definition" InsertVisible="False"
                ReadOnly="True" SortExpression="Definition" />
            <asp:BoundField DataField="CorrectWord" 
                HeaderText="Correct Word Count" SortExpression="CorrectWord" />
                    <asp:BoundField DataField="WordAttempts" 
                HeaderText="Incorrect Word Count" SortExpression="WordAttempts" />
            <asp:BoundField DataField="CorrectDefinition" 
                HeaderText="Correct Definition Count" SortExpression="CorrectDefinition" />
                    <asp:BoundField DataField="DefinitionAttempts" 
                HeaderText="Incorrect Definition Count" SortExpression="DefinitionAttempts" />
        </Columns>
    </asp:GridView>
</asp:Content>
