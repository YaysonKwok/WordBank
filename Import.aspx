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
    </div>

    <p class ="text-primary"><asp:Label ID="Label1" runat="server" Text="Expected Input Order: Word, Informal (TRUE or FALSE), Definition, Contextual Sentence, Personal Sentence"></asp:Label></p>
    <p class ="text-warning"><asp:Label ID="Label2" runat="server" Text="Duplicates will be ignored"></asp:Label></p>
    
    <asp:FileUpload ID="Upload" runat="server" />
    <br />



    <asp:Button ID="UploadBtn" runat="server" OnClick="UploadBtn_Click" Text="Upload" />
    <asp:RegularExpressionValidator ID="regexValidator" runat="server"
        ControlToValidate="Upload"
        ErrorMessage="Only csv files are allowed"
        ValidationExpression="(.*\.([cC][sS][vV])$)">
    </asp:RegularExpressionValidator>



    <div>
        <asp:CheckBox ID="TitleCheckBox" runat="server"  Text="Ignore First Row"/>
    </div>



    <br />
    <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="true" CssClass="table table-striped">
    </asp:GridView>
</asp:Content>
