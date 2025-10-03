<%@ Page Title="View Students" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewStudent.aspx.cs" Inherits="WebformDemo.ViewStudent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>View Students</h2>
        <hr />

        <asp:GridView ID="GridView1" runat="server"
            AutoGenerateColumns="false"
            AllowPaging="true" PageSize="5"
            CssClass="table table-striped table-bordered"
            OnRowEditing="GridView1_RowEditing"
            OnRowCancelingEdit="GridView1_RowCancelingEdit"
            OnRowUpdating="GridView1_RowUpdating"
            OnRowDeleting="GridView1_RowDeleting"
            OnPageIndexChanging="GridView1_PageIndexChanging"
            DataKeyNames="StudentID"
            OnSelectedIndexChanged="GridView1_SelectedIndexChanged">

            <Columns>
                <asp:BoundField DataField="StudentID" HeaderText="ID" ReadOnly="true" />

                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate><%# Eval("StudentName") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtName" runat="server" Text='<%# Bind("StudentName") %>' CssClass="form-control"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Roll No">
                    <ItemTemplate><%# Eval("RollNo") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtRollNo" runat="server" Text='<%# Bind("RollNo") %>' CssClass="form-control"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Class">
                    <ItemTemplate><%# Eval("Class") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtClass" runat="server" Text='<%# Bind("Class") %>' CssClass="form-control"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Section">
                    <ItemTemplate><%# Eval("Section") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSection" runat="server" Text='<%# Bind("Section") %>' CssClass="form-control"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Email">
                    <ItemTemplate><%# Eval("Email") %></ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="form-control"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Courses with Teachers">
                    <ItemTemplate><%# Eval("CoursesWithTeachers") %></ItemTemplate>
                </asp:TemplateField>

                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ShowSelectButton="true" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
