using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using tjdaca.Data;
using tjdaca.Services;

namespace tjdaca.Pages
{
    public partial class Student : ComponentBase
    {
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] NavigationManager NavManager { get; set; }

        private string searchString = "";
        private List<AcaStudents> StudentsList = new List<AcaStudents>();

        protected override async Task OnInitializedAsync()
        {
            GetStudentList();
        }
        private List<AcaStudents> GetStudentList()
        {
            StudentsList = studentService.GetStudents();
            return StudentsList;
        }
        private bool Search(AcaStudents student)
        {
            if (string.IsNullOrWhiteSpace(searchString)) return true;
            if (student.StuName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
            || student.Class.Contains(searchString, StringComparison.OrdinalIgnoreCase)
            || student.StudentPhone.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        private void Edit(int id)
        {
            NavManager.NavigateTo($"/students/detail/u/{id}");
        }
        private void AdminEdit(int id)
        {
            NavManager.NavigateTo($"/students/detail/a/{id}");
        }

        private async void Delete(int id)
        {
            bool? result = await DialogService.ShowMessageBox(
                        "삭제 확인",
                        "학생을 삭제하시면 복구할 수 없습니다. 삭제하시겠습니까?",
                        yesText: "삭제", cancelText: "취소");
            if (result != null)
            {
                studentService.DeleteStudent(id);
                snackBar.Add("학생 삭제 완료.", Severity.Success);
                GetStudentList();
            }
            StateHasChanged();
        }

        private void AddStudent()
        {
            NavManager.NavigateTo("/students/detail/u/0");
        }
    }
}