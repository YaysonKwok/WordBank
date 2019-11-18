<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="WordBank.Import" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />

    <div>
        <p class="text-success">
            <asp:Literal runat="server" ID="UploadMessage" />
        </p>
        <p class="text-danger">
            <asp:Literal runat="server" ID="UploadFailed" />
        </p>
        <p class="text-primary">
            <asp:Label ID="Label1" runat="server" Text="Expected Input Order: Word, Informal (TRUE or FALSE), Definition, Contextual Sentence, Personal Sentence"></asp:Label>
        </p>
        <p class="text-warning">
            <asp:Label ID="Label2" runat="server" Text="Duplicates will be ignored"></asp:Label>
        </p>
    </div>

    <div>
        <asp:FileUpload ID="Upload" runat="server" />
        <br />
        <asp:CheckBox ID="TitleCheckBox" runat="server" Text="Ignore First Row" />
        <br />
        <br />
        <asp:Button ID="UploadBtn" runat="server" OnClick="UploadBtn_Click" Text="Upload" />
        <asp:RegularExpressionValidator ID="regexValidator" runat="server"
            ControlToValidate="Upload"
            ErrorMessage="Only csv files are allowed"
            ValidationExpression="(.*\.([cC][sS][vV])$)">
        </asp:RegularExpressionValidator>
    </div>

    <br />
    <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="false" CssClass="table table-striped">
        <Columns>
            <asp:BoundField DataField="Word"
                HeaderText="Word"
                SortExpression="Word" />
            <asp:BoundField DataField="Definition"
                HeaderText="Definition" />
            <asp:BoundField DataField="Sentence1"
                HeaderText="Contextual Sentence" />
            <asp:BoundField DataField="Sentence2"
                HeaderText="Personal Sentence" />
            <asp:BoundField DataField="Informal"
                HeaderText="Informal" />
        </Columns>
    </asp:GridView>
</asp:Content>
