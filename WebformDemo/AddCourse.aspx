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

        <asp:GridView ID="gvCourses" runat="server" AutoGenerateColumns="False" DataKeyNames="CourseID"
            CssClass="table table-striped"
            SelectMethod="GetCourses"
            UpdateMethod="UpdateCourse"
            DeleteMethod="DeleteCourse">
            
            <Columns>
                <asp:BoundField DataField="CourseID" HeaderText="ID" ReadOnly="True" />

                <asp:TemplateField HeaderText="Course Name">
                    <ItemTemplate>
                        <%# Eval("CourseName") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCourseNameEdit" runat="server" Text='<%# Bind("CourseName") %>' CssClass="form-control"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
