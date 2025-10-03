<%@ Page Title="Student Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StudentDetails.aspx.cs" Inherits="WebformDemo.StudentDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Student Details</h2>
        <hr />

        <asp:Label ID="lblDetails" runat="server" CssClass="form-control-plaintext"></asp:Label>
    </div>
</asp:Content>
