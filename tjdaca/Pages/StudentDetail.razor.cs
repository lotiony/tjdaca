using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using tjdaca.Data;
using tjdaca.Services;

namespace tjdaca.Pages
{
    public partial class StudentDetail : ComponentBase
    {
        [Inject] NavigationManager NavManager { get; set; }
        [Inject] IStudentService studentService { get; set; }
        [Inject] MudBlazor.ISnackbar snackBar { get; set; }

        [Parameter]
        public int stu_idx { get; set; }

        private Students student;
        protected override async Task OnInitializedAsync()
        {
            if (stu_idx == 0)
            {
                student = new Students();
            }
            else
            {
                student = studentService.GetStudentById(stu_idx);
            }
        }
    
        private void Save()
        {
            studentService.SaveStudent(student);
            snackBar.Add("학생 저장 완료.", Severity.Success);
            NavManager.NavigateTo("/students");
        }


    }
}