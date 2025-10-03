<%@ Page Title="Add Course" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddCourse.aspx.cs" Inherits="WebformDemo.AddCourse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Add New Course</h2>
        <div class="mb-3">
            <asp:TextBox ID="txtCourseName" runat="server" CssClass="form-control" Placeholder="Course Name"></asp:TextBox>
        </div>
        <div class="mb-3">
            <asp:Button ID="btnAddCourse" runat="server" Text="Add Course" CssClass="btn btn-primary" OnClick="btnAddCourse_Click" />
        </div>
        <hr />
        <asp:GridView ID="gvCourses" runat="server" AutoGenerateColumns="False" DataKeyNames="CourseId" CssClass="table table-striped" 
            OnRowDeleting="gvCourses_RowDeleting">
            <Columns>
                <asp:BoundField DataField="CourseId" HeaderText="ID" ReadOnly="True" />
                <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                <asp:ButtonField Text="Delete" CommandName="Delete"  />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
