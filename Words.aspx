<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Words.aspx.cs" Inherits="WordBank.Words" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <p>
        Hello <%:Session["Username"]%>, here are your personal word stats.
    </p>
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    <asp:GridView ID="GridView"
        runat="server"
        DataKeyNames="ID"
        AutoGenerateColumns="False"
        CssClass="table table-striped"
        AllowSorting="true"
        OnRowDeleting="GridView_RowDeleting"
        OnRowEditing="GridView_RowEditing"
        OnRowUpdating="GridView_RowUpdating"
        OnRowCancelingEdit="GridView_RowCancelingEdit"
        OnSorting="GridView_Sorting">
        <Columns>
            <asp:CommandField ShowEditButton="true"
                EditText="Edit"
                CancelText="Discard"
                UpdateText="Revise"
                HeaderText="Edit" />
            <asp:BoundField DataField="Word"
                HeaderText="Word"
                SortExpression="Word" />
            <asp:BoundField DataField="Definition"
                HeaderText="Definition" />
            <asp:BoundField DataField="CorrectWord"
                HeaderText="Correct Word Count" SortExpression="CorrectWord" />
            <asp:BoundField DataField="WordAttempts"
                HeaderText="Incorrect Word Count" SortExpression="WordAttempts" />
            <asp:BoundField DataField="CorrectDefinition"
                HeaderText="Correct Definition Count" SortExpression="CorrectDefinition" />
            <asp:BoundField DataField="DefinitionAttempts"
                HeaderText="Incorrect Definition Count" SortExpression="DefinitionAttempts" />
            <asp:BoundField DataField="Informal"
                HeaderText="Informal" SortExpression="Informal" />
            <asp:BoundField DataField="DateCreated"
                HeaderText="Date Created" SortExpression="DateCreated" />
            <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" />
        </Columns>
    </asp:GridView>
</asp:Content>
