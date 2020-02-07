<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Words.aspx.cs" Inherits="WordBank.Words" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div style="margin-left: auto; margin-right: auto; text-align: center;">
        <p>
            Hello <%:Session["Username"]%>, here are your personal word stats.
        </p>

        <p>
            <asp:Button Text="Export" OnClick="ExportCSVBtn_Click" runat="server" /></p>
        <br />
        <p>
            <asp:Label ID="Label1" runat="server"></asp:Label></p>
    </div>
    <br />

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
                HeaderText="Edit" />
            <asp:BoundField DataField="Word"
                HeaderText="Word"
                SortExpression="Word" />
            <asp:BoundField DataField="Definition"
                HeaderText="Definition" />
            <asp:BoundField DataField="Sentence1"
                HeaderText="Contextual Sentence" />
            <asp:BoundField DataField="Sentence2"
                HeaderText="Personal Sentence" />
            <asp:BoundField DataField="CorrectWord"
                HeaderText="Corr Word Count" SortExpression="CorrectWord" ReadOnly="true" />
            <asp:BoundField DataField="IncorrectWord"
                HeaderText="Incorr Word Count" SortExpression="IncorrectWord" ReadOnly="true" />
            <asp:BoundField DataField="CorrectDefinition"
                HeaderText="Corr Def Count" SortExpression="CorrectDefinition" ReadOnly="true" />
            <asp:BoundField DataField="IncorrectDefinition"
                HeaderText="Incorr Def Count" SortExpression="IncorrectDefinition" ReadOnly="true" />
            <asp:TemplateField HeaderText="Informal" SortExpression="Informal">
                <ItemTemplate>
                    <asp:CheckBox ID="chkbox1" runat="server" Checked='<%# Eval("Informal") %>' onclick="return false;" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="DateCreated"
                HeaderText="Date Created" SortExpression="DateCreated" ReadOnly="true" />
            <asp:TemplateField HeaderText="Delete">
                <ItemTemplate>
                    <asp:Button ID="deletebtn" runat="server" CommandName="Delete"
                        Text="Delete" OnClientClick="return confirm('Are you sure?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

        <HeaderStyle BackColor="LightCyan"
            ForeColor="MediumBlue" />

    </asp:GridView>
</asp:Content>
