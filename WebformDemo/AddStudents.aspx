<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddStudents.aspx.cs" Inherits="WebformDemo.AddStudents" %>

<!DOCTYPE html>
<style>
    .textbox-gap {
        margin-bottom: 10px; /* vertical gap */
        margin-top: 5px;     /* optional top gap */
    }
</style>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<body>
    <form id="form1" runat="server">
    <asp:Label ID="lblName" runat="server" CssClass="textbox-gap" Text="Name"></asp:Label>
    <asp:TextBox ID="TxtName" runat="server"></asp:TextBox><br /><br />

    <asp:Label ID="lblRollNo" runat="server"  CssClass="textbox-gap" Text="Roll No"></asp:Label>
    <asp:TextBox ID="TxtRollNo" runat="server"></asp:TextBox><br /><br />

    <asp:Label ID="lblClass" runat="server"  CssClass="textbox-gap" Text="Class"></asp:Label>
    <asp:TextBox ID="TxtClass" runat="server"></asp:TextBox><br /><br />

    <asp:Label ID="lblSection" runat="server"  CssClass="textbox-gap" Text="Section"></asp:Label>
    <asp:TextBox ID="TxtSection" runat="server"></asp:TextBox><br /><br />

    <asp:Label ID="lblEmail" runat="server"  CssClass="textbox-gap"  Text="Email"></asp:Label>
    <asp:TextBox ID="TxtEmail" runat="server"></asp:TextBox><br /><br />

    <asp:Button ID="btnAdd" runat="server" Text="Add Student" OnClick="btnAdd_Click" /><br /><br />
    <asp:Button ID="btnView" runat="server" Text="View Students" OnClick="btnView_Click" /><br /><br />


    </form>
</body>
</html>
