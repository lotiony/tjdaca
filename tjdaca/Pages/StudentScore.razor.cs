using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Data;
using tjdaca.Data;
using tjdaca.Services;

namespace tjdaca.Pages
{
    public partial class StudentScore : ComponentBase
    {
        [Inject] NavigationManager NavManager { get; set; }
        [Inject] MudBlazor.ISnackbar snackBar { get; set; }
        [Inject] private IDialogService DialogService { get; set; }

        [Parameter]
        public string stu_idx { get; set; }

        private AcaStudents student;
        private AcaStuSchoolScore schoolScore;
        private AcaStuExamScore examScore;
        private List<AcaStuExamScore> examScoreList;

        private List<float?> ratioList { get; set; } = new List<float?>();
        protected override async Task OnInitializedAsync()
        {
            InitInputForm();

            Int32.TryParse(stu_idx, out int stu_idx_int);

            student = studentService.GetStudentById(stu_idx_int);
            schoolScore = studentService.GetSchoolScore(stu_idx_int);
            examScoreList = studentService.GetExamScore(stu_idx_int);
            examScore = new AcaStuExamScore(stu_idx_int);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Int32.TryParse(stu_idx, out int stu_idx_int);

                if (stu_idx_int == 0)
                {
                    await JS.InvokeVoidAsync("alert", "잘못된 접근입니다. 학생이 선택되지 않았습니다.");
                    NavManager.NavigateTo("/search");
                }
                else
                {
                    student = studentService.GetStudentById(stu_idx_int);
                    schoolScore = studentService.GetSchoolScore(stu_idx_int);
                    examScoreList = studentService.GetExamScore(stu_idx_int);
                    examScore = new AcaStuExamScore(stu_idx_int);
                }
            }
        }

        private async void SaveSchoolScore()
        {
            schoolScore.RegDate = DateTime.Now;
            studentService.SaveSchoolScore(schoolScore);
            snackBar.Add("학교 성적이 저장되었습니다.", Severity.Success);
        }

        private async void SaveExamScore()
        {
            examScore.RegDate = DateTime.Now;
            examScore.Idx = 0;
            studentService.SaveExamScore(examScore);
            snackBar.Add("월말모의고사 성적이 등록되었습니다.", Severity.Success);
            examScore = new AcaStuExamScore(student.StuIdx);
            examScoreList = studentService.GetExamScore(student.StuIdx);
            StateHasChanged();
        }

        private async void UpdateExamScore(AcaStuExamScore item)
        {
            item.RegDate = DateTime.Now;
            studentService.SaveExamScore(item);
            snackBar.Add("월말모의고사 성적이 변경되었습니다.", Severity.Success);
        }


        private void Cancel()
        {
            JS.InvokeVoidAsync("history.go", -1);
        }

        private void InitInputForm()
        {
            ratioList = GetRatioList();
        }

        private List<float?> GetRatioList()
        {
            List<float?> rtn = new List<float?>();
            for (float i = 100; i >= 0; i--)
            {
                rtn.Add(i);
            }
            return rtn;
        }
    }
}