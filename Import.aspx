<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="WordBank.Import" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
         <p class="text-success">
      <asp:Literal runat="server" ID="UploadMessage" />
    </p>
    <p class="text-danger">
      <asp:Literal runat="server" ID="UploadFailed" />
    </p>
    <asp:Label ID="Label1" runat="server" Text="Expected Input Order: No title columns, Word, Definition, Sentence, Informal (TRUE or FALSE)"></asp:Label>
    <asp:FileUpload ID="Upload" runat="server" />
    <br />
    <asp:Button ID="UploadBtn" runat="server" OnClick="UploadBtn_Click" Text="Upload" />
    <asp:RegularExpressionValidator ID="regexValidator" runat="server"
         ControlToValidate="Upload"
         ErrorMessage="Only csv files are allowed" 
         ValidationExpression="(.*\.([cC][sS][vV])$)">
    </asp:RegularExpressionValidator> 
    <asp:CheckBox ID="TitleCheck" runat="server" />
    <br />
    <asp:GridView id="GridView" runat="server" AutoGenerateColumns="true" CssClass="table table-striped" >
    </asp:GridView>
</asp:Content>
