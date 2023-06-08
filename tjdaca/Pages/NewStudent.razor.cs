using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using tjdaca.Data;
using tjdaca.Services;

namespace tjdaca.Pages
{
    public partial class NewStudent : ComponentBase
    {
        [Inject] NavigationManager NavManager { get; set; }
        [Inject] MudBlazor.ISnackbar snackBar { get; set; }
        [Inject] private IDialogService DialogService { get; set; }


        private AcaStudents student;
        private List<AcaOptions> schTypeList { get; set; }
        private List<AcaOptions> schGradeList { get; set; }
        private List<AcaOptions> cognitivePathwayList { get; set; }
        private List<AcaOptions> subjectList { get; set; }
        private List<string> teacherList { get; set; }
        protected override async Task OnInitializedAsync()
        {
            InitInputForm();
            student = new AcaStudents();
        }
    
        private async void Save()
        {
            bool? result = await DialogService.ShowMessageBox(
                         "등록 확인",
                         "입력하신 정보로 학생을 등록하시겠습니까?",
                         yesText: "등록", cancelText: "취소");
            if (result != null)
            {
                if (student.StuIdx == 0) student.Regdate = DateTime.Now;
                student.Lastdate = DateTime.Now;
                studentService.SaveStudent(student);
                snackBar.Add("학생 등록이 완료되었습니다.", Severity.Success);
                student = new AcaStudents();
                StateHasChanged();
            }
        }

        private async void Cancel()
        {
            bool? result = await DialogService.ShowMessageBox(
                         "초기화",
                         "입력하신 정보를 모두 초기화 합니다. 진행하시겠습니까?",
                         yesText: "초기화", cancelText: "취소");
            if (result != null)
            {
                student = new AcaStudents();
                StateHasChanged();
            }
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