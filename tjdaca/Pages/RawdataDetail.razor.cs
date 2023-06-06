using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using tjdaca.Data;
using tjdaca.Services;

namespace tjdaca.Pages
{
    public partial class RawdataDetail : ComponentBase
    {
        [Inject] NavigationManager NavManager { get; set; }
        [Inject] MudBlazor.ISnackbar snackBar { get; set; }

        [Parameter]
        public string rid { get; set; } = string.Empty;
        [Parameter]
        public string tid { get; set; } = string.Empty;
        private int ridx_int { get { return !string.IsNullOrEmpty(rid) ? Convert.ToInt32(rid) : 0; } }
        private int tidx_int { get { return !string.IsNullOrEmpty(tid) ? Convert.ToInt32(tid) : 0; } }


        private AcaRawdata rdata;
        private List<AcaStudents> studentList { get; set; } = new List<AcaStudents>();
        private List<AcaOptions> classNameList { get; set; } = new List<AcaOptions>();
        private List<string> classGradeList { get; set; } = new List<string>();
        private List<string> testGradeList { get; set; } = new List<string>();
        private List<string> testSubjectList { get; set; } = new List<string>();
        private List<string> cliniqSubjectList { get; set; } = new List<string>();
        private List<AcaOptions> textbookSourceList { get; set; } = new List<AcaOptions>();
        private List<float?> ratioList { get; set; } = new List<float?>();
        private string selectedTestGrade
        {
            get { return testGradeList?.SingleOrDefault(x => x == rdata.TestGrade) ?? ""; }
            set
            {
                this.rdata.TestGrade = value;
                OnTestGradeChange(value);
            }
        }
        private string selectedCliniqGrade
        {
            get { return testGradeList?.SingleOrDefault(x => x == rdata.CliniqGrade) ?? ""; }
            set
            {
                this.rdata.CliniqGrade = value;
                OnCliniqGradeChange(value);
            }
        }
        private string selectedTestSubject
        {
            get { return testSubjectList?.SingleOrDefault(x => x == rdata.TestSubject) ?? ""; }
            set
            {
                this.rdata.TestSubject = value;
            }
        }
        private string selectedCliniqSubject
        {
            get { return cliniqSubjectList?.SingleOrDefault(x => x == rdata.CliniqSubject) ?? ""; }
            set
            {
                this.rdata.CliniqSubject = value;
            }
        }


        protected override async Task OnInitializedAsync()
        {
            InitInputForm();

            if (ridx_int == 0)
            {
                rdata = new AcaRawdata();
                rdata.TIdx = tidx_int;
                rdata.Attendance = "O";
            }

            else
            {
                rdata = rawdataService.GetRawdataById(ridx_int);
                selectedCliniqGrade = rdata.CliniqGrade;
                selectedTestGrade = rdata.TestSubject;
            }
        }
    
        private void Save()
        {
            rawdataService.SaveData(rdata);
            snackBar.Add("데이터 저장 완료.", Severity.Success);
            NavManager.NavigateTo($"/rawdata/{tid}");
        }

        private void Cancel()
        {
            JS.InvokeVoidAsync("history.go", -1);
        }

        private void InitInputForm()
        {
            studentList = rawdataService.GetStudentByTid(tidx_int);
            textbookSourceList = optionService.GetOptions("교재출처");
            classNameList = optionService.GetOptions("반명");
            classGradeList = GetClassGradeList();
            ratioList = GetRatioList();
            testGradeList = GetTestGradeList();
        }

        private AcaStudents selectedStudent
        {
            get
            {
                return studentService.GetStudentById(rdata?.StuIdx ?? 0);
            }
            set
            {
                rdata.StuIdx = value.StuIdx;
                rdata.StuName = value.StuName;
                rdata.SchType = value.SchType;
                rdata.SchName = value.School;
                rdata.Grade = value.SchGrade;
                rdata.ClassName = value.Class;
                SetLastRawdata();
            }
        }
        
        /// <summary>
        /// 해당학생의 가장 마지막 Rawdata 값이 있으면 불러온다.
        /// </summary>
        private void SetLastRawdata()
        {
            var lastRawdata = rawdataService.GetLastRawdataBySid(rdata.StuIdx ?? 0);
            if (lastRawdata.RIdx > 0)
            {
                snackBar.Add("가장 마지막에 입력된 데이터를 불러오는데 성공했습니다.", Severity.Success);

                rdata.ClassName = lastRawdata.ClassName;
                rdata.Attendance = lastRawdata.Attendance;
                rdata.AbsenceReason = lastRawdata.AbsenceReason;
                //rdata.SchAchievement= lastRawdata.SchAchievement;
                //rdata.ClassSubject = lastRawdata.ClassSubject;
                rdata.ClassGrade = lastRawdata.ClassGrade;
                //rdata.Homework = lastRawdata.Homework;
                //rdata.HomeworkPrev = lastRawdata.HomeworkPrev;
                //rdata.HomeworkPerform = lastRawdata.HomeworkPerform;
                //rdata.HomeworkCorrect = lastRawdata.HomeworkCorrect;
                //rdata.HomeworkProgress = lastRawdata .HomeworkProgress;
                //rdata.HomeworkRatio = lastRawdata.HomeworkRatio;
                //rdata.TestGrade = lastRawdata.TestGrade;
                //selectedTestGrade = lastRawdata.TestGrade;
                //rdata.TestSubject = lastRawdata.TestSubject;
                //rdata.TextbookSource = lastRawdata.TextbookSource;
                //rdata.Difficulty = lastRawdata.Difficulty;
                //rdata.DailyCorrect = lastRawdata.DailyCorrect;
                //rdata.DailyScore = lastRawdata.DailyScore;
                //rdata.DaliyCount = lastRawdata.DaliyCount;
                //rdata.CliniqCorrect = lastRawdata.CliniqCorrect;
                //rdata.CliniqCount = lastRawdata.CliniqCount;
                //rdata.CliniqGrade = lastRawdata.CliniqGrade;
                //selectedCliniqGrade = lastRawdata.CliniqGrade;
                //rdata.CliniqScore = lastRawdata.CliniqScore;
                //rdata.CliniqSubject = lastRawdata.CliniqSubject;
                //rdata.Counsel = lastRawdata.Counsel;
                //rdata.Etc1 = lastRawdata.Etc1;
                //rdata.Etc2 = lastRawdata.Etc2;
                //rdata.Etc3 = lastRawdata.Etc3;
            }
        }

        private float? selectedHomeworkPrev
        { get { return rdata.HomeworkPrev ?? 0; } 
          set { 
                rdata.HomeworkPrev = value;
                if (value > 0 && rdata.HomeworkPerform > 0)
                {
                    /// 과제이행정도 = 과제수행량 / 이전과제량
                    rdata.HomeworkProgress = (float?)(Math.Round((decimal)rdata.HomeworkPerform / (decimal)value * 100, 0));
                }
          }
        }
        private float? selectedHomeworkPerform
        {
            get { return rdata.HomeworkPerform ?? 0; }
            set
            {
                rdata.HomeworkPerform = value;
                if (value > 0 && rdata.HomeworkPrev > 0)
                {
                    /// 과제이행정도 = 과제수행량 / 이전과제량
                    rdata.HomeworkProgress = (float?)(Math.Round((decimal)value / (decimal)rdata.HomeworkPrev * 100, 0));
                }
                if (value > 0 && rdata.HomeworkCorrect > 0)
                {
                    /// 과제정답률 = 과제정답 / 과제수행량
                    rdata.HomeworkRatio = (float?)(Math.Round((decimal)rdata.HomeworkCorrect / (decimal)value * 100, 0));
                }
            }
        }
        private float? selectedHomeworkCorrect
        {
            get { return rdata.HomeworkCorrect ?? 0; }
            set
            {
                rdata.HomeworkCorrect = value;
                if (value > 0 && rdata.HomeworkPerform > 0)
                {
                    /// 과제정답률 = 과제정답 / 과제수행량
                    rdata.HomeworkRatio = (float?)(Math.Round((decimal)value / (decimal)rdata.HomeworkPerform * 100, 0));
                }
            }
        }

        private float? selectedDailyCount
        {
            get { return rdata.DaliyCount ?? 0; }
            set
            {
                rdata.DaliyCount = value;
                if (value > 0 && rdata.DailyCorrect > 0)
                {
                    /// 데일리정답률 = 일일정답수 / 일일문항수
                    rdata.DailyScore = (float?)(Math.Round((decimal)rdata.DailyCorrect / (decimal)value * 100, 0));
                }
            }
        }
        private float? selectedDailyCorrect
        {
            get { return rdata.DailyCorrect ?? 0; }
            set
            {
                rdata.DailyCorrect = value;
                if (value > 0 && rdata.DaliyCount > 0)
                {
                    /// 데일리정답률 = 일일정답수 / 일일문항수
                    rdata.DailyScore = (float?)(Math.Round((decimal)value / (decimal)rdata.DaliyCount * 100, 0));
                }
            }
        }
        private float? selectedCliniqCount
        {
            get { return rdata.CliniqCount ?? 0; }
            set
            {
                rdata.CliniqCount = value;
                if (value > 0 && rdata.CliniqCorrect > 0)
                {
                    /// 클리닉정답률 = 클리닉정답수 / 클리닉문항수
                    rdata.CliniqScore = (float?)(Math.Round((decimal)rdata.CliniqCorrect / (decimal)value * 100, 0));
                }
            }
        }
        private float? selectedCliniqCorrect
        {
            get { return rdata.CliniqCorrect ?? 0; }
            set
            {
                rdata.CliniqCorrect = value;
                if (value > 0 && rdata.CliniqCount > 0)
                {
                    /// 클리닉정답률 = 클리닉정답수 / 클리닉문항수
                    rdata.CliniqScore = (float?)(Math.Round((decimal)value / (decimal)rdata.CliniqCount * 100, 0));
                }
            }
        }

        private List<string> GetClassGradeList()
        {
            List<string> rtn = new List<string>();
            string[] g1 = new string[] { "초4", "초5", "초6", "중1", "중2", "중3", "고1", "고2", "고3" };
            foreach (string g in g1)
            {
                rtn.Add($"{g}-1");
                rtn.Add($"{g}-2");
            }
            return rtn;
        }
        private List<string> GetTestGradeList()
        {
            List<string> rtn = new List<string>();
            rtn = rawdataService.GetTestGradeList("수학");
            return rtn;
        }
        private List<float?> GetRatioList()
        {
            List<float?> rtn = new List<float?>();
            for(float i = 100; i >= 0; i--) 
            {
                rtn.Add(i);
            }
            return rtn;
        }
        private void OnTestGradeChange(string grade)
        {
            if (grade == "") return;
            testSubjectList = rawdataService.GetSubjectListByGrade("수학", grade);
        }

        private void OnCliniqGradeChange(string grade)
        {
            if (grade == "") return;
            cliniqSubjectList = rawdataService.GetSubjectListByGrade("수학", grade);
        }

    }
}