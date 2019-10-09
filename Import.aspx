<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="WordBank.Import" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <asp:FileUpload ID="Upload" runat="server" />
    <asp:Button ID="UploadBtn" runat="server" OnClick="UploadBtn_Click" Text="Upload" />
    <asp:RegularExpressionValidator ID="regexValidator" runat="server"
         ControlToValidate="Upload"
         ErrorMessage="Only csv files are allowed" 
         ValidationExpression="(.*\.([cC][sS][vV])$)">
    </asp:RegularExpressionValidator> 
    <br />
    <asp:GridView id="GridView" runat="server" AutoGenerateColumns="true" CssClass="table table-striped" >
    </asp:GridView>
    <asp:Label ID="Label1" runat="server" Text="test"></asp:Label>
</asp:Content>
