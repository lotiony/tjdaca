using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using tjdaca.Data;
using tjdaca.Services;

namespace tjdaca.Pages
{
    public partial class StudentDetail : ComponentBase
    {
        [Inject] NavigationManager NavManager { get; set; }
        [Inject] MudBlazor.ISnackbar snackBar { get; set; }

        [Parameter]
        public string stu_idx { get; set; }
        [Parameter]
        public string isAdmin { get; set; } = string.Empty;

        private AcaStudents student;
        private bool IsAdmin { get { return this.isAdmin.ToLower() == "a" ?  true : false; } }
        private List<AcaOptions> schTypeList { get; set; }
        private List<AcaOptions> schGradeList { get; set; }
        private List<AcaOptions> cognitivePathwayList { get; set; }
        private List<AcaOptions> subjectList { get; set; }
        private List<string> teacherList { get; set; }
        protected override async Task OnInitializedAsync()
        {
            InitInputForm();

            Int32.TryParse(stu_idx, out int stu_idx_int);

            if (stu_idx_int == 0)
            {
                student = new AcaStudents();
            }
            else
            {
                student = studentService.GetStudentById(stu_idx_int);
            }
        }
    
        private void Save()
        {
            studentService.SaveStudent(student);
            snackBar.Add("학생 저장 완료.", Severity.Success);
            NavManager.NavigateTo("/students");
        }

        private void Cancel()
        {
            JS.InvokeVoidAsync("history.go", -1);
        }

        private void InitInputForm()
        {
            schTypeList = optionService.GetOptions("학교구분");
            schGradeList = optionService.GetOptions("학년구분");
            cognitivePathwayList = optionService.GetOptions("학원 인지경로");
            subjectList = optionService.GetOptions("과목구분");
            teacherList = optionService.GetTeachers();
        }
    }
}