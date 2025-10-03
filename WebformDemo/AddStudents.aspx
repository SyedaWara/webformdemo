<%@ Page Title="Add Student" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddStudents.aspx.cs" Inherits="WebformDemo.AddStudents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .form-container {
            max-width: 700px;
            margin: 30px auto;
            padding: 20px;
            background-color: #f8f9fa;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        .form-container h2 {
            margin-bottom: 20px;
        }
    </style>

    <div class="form-container">
        <h2>Add New Student</h2>

        <div class="mb-3">
            <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label>
            <asp:TextBox ID="TxtName" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="mb-3">
            <asp:Label ID="lblRollNo" runat="server" Text="Roll No"></asp:Label>
            <asp:TextBox ID="TxtRollNo" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="mb-3">
            <asp:Label ID="lblClass" runat="server" Text="Class"></asp:Label>
            <asp:TextBox ID="TxtClass" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="mb-3">
            <asp:Label ID="lblSection" runat="server" Text="Section"></asp:Label>
            <asp:TextBox ID="TxtSection" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="mb-3">
            <asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>
            <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="mb-3">
            <asp:Label ID="lblCourses" runat="server" Text="Select Courses"></asp:Label>
           <asp:CheckBoxList ID="chkCourses" runat="server" RepeatDirection="Vertical"
    AutoPostBack="true" OnSelectedIndexChanged="chkCourses_SelectedIndexChanged">
</asp:CheckBoxList>
        </div>
       <asp:Repeater ID="rptCourseTeachers" runat="server" OnItemDataBound="rptCourseTeachers_ItemDataBound">
    <ItemTemplate>
        <div class="mb-3">
            <asp:Label runat="server" ID="lblCourseName" Text='<%# Eval("CourseName") %>'></asp:Label>
            <asp:HiddenField ID="hfCourseID" runat="server" Value='<%# Eval("CourseID") %>' />
            <asp:DropDownList ID="ddlTeachers" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
    </ItemTemplate>
</asp:Repeater>

    
        <div class="d-flex flex-column flex-md-row gap-2">
            <asp:Button ID="btnAdd" runat="server" Text="Add Student" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
            
        </div>
    </div>
</asp:Content>
