using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using tjdaca.Data;
using tjdaca.Services;

namespace tjdaca.Pages
{
    public partial class QuestionDetail : ComponentBase
    {
        [Inject] NavigationManager NavManager { get; set; }
        [Inject] MudBlazor.ISnackbar snackBar { get; set; }

        [Parameter]
        public string q_idx { get; set; }
        [Parameter]
        public string mode { get; set; } = string.Empty;

        private AcaQuestions Question;
        private List<AcaTeachers> teacherList { get; set; } = new List<AcaTeachers>();
        private List<AcaStudents> studentList { get; set; } = new List<AcaStudents>();

        private AcaTeachers selectedTeacher
        {
            get { return teacherList.SingleOrDefault(x=> x.TIdx.Equals(Question?.TIdx ?? 0)); }
            set
            {
                this.Question.TIdx = value.TIdx;
                this.Question.TName = value.TName;
                OnTeacherChange(value.TIdx);
            }
        }

        private AcaStudents selectedStudent
        {
            get { return studentList.SingleOrDefault(x => x.StuIdx.Equals(Question?.StuIdx ?? 0)); }
            set
            {
                this.Question.StuIdx = value.StuIdx;
                this.Question.StuName = value.StuName;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            InitInputForm();

            Int32.TryParse(q_idx, out int QIdx_int);

            if (QIdx_int == 0 && mode == "i")
            {
                Question = new AcaQuestions();
            }
            else
            {
                Question = questionService.GetQuestionById(QIdx_int);
                studentList = questionService.GetStudentByTid(Question.TIdx);
                selectedStudent = studentList.SingleOrDefault(x => x.StuIdx.Equals(Question.StuIdx));
            }
        }
    
        private void Save()
        {
            questionService.SaveQuestion(Question);
            snackBar.Add("학습질문 저장 완료.", Severity.Success);
            NavManager.NavigateTo("/Questions");
        }

        private void Cancel()
        {
            JS.InvokeVoidAsync("history.go", -1);
        }

        private void InitInputForm()
        {
            teacherList = questionService.GetTeachers();
            studentList = new List<AcaStudents>();
        }
        private void OnTeacherChange(int tIdx)
        {
            if (tIdx == 0) return;
            studentList = questionService.GetStudentByTid(Convert.ToInt32(tIdx));
            Question.StuIdx = 0;
            Question.StuName = "";
        }
    }
}