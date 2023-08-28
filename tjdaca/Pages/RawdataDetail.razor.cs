using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using tjdaca.Class;
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
        private List<string> teacherList { get; set; } = new List<string>();
        private List<string> classGradeList { get; set; } = new List<string>();
        private List<string> testGradeList { get; set; } = new List<string>();
        private List<string> testSubjectList { get; set; } = new List<string>();
        private List<string> cliniqSubjectList { get; set; } = new List<string>();
        private List<AcaOptions> textbookSourceList { get; set; } = new List<AcaOptions>();
        private List<string> ratioList { get; set; } = new List<string>();
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
                selectedTestGrade = rdata.TestGrade;
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
            teacherList = optionService.GetTeachers();
            teacherList.Insert(0, "");
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
                rdata.ClassSubject = lastRawdata.ClassSubject;
                rdata.ClassGrade = lastRawdata.ClassGrade;
                rdata.Homework = lastRawdata.Homework;
                //rdata.HomeworkPrev = lastRawdata.HomeworkPrev;
                rdata.HomeworkPrev = lastRawdata.HomeworkPerform;   /// 이전과제량을 불러오는거기땜에 마지막데이터의 과제수행량 값을 바인드한다.
                //rdata.HomeworkPerform = lastRawdata.HomeworkPerform;
                //rdata.HomeworkCorrect = lastRawdata.HomeworkCorrect;
                //rdata.HomeworkProgress = lastRawdata.HomeworkProgress;
                //rdata.HomeworkRatio = lastRawdata.HomeworkRatio;
                rdata.TestGrade = lastRawdata.TestGrade;
                selectedTestGrade = lastRawdata.TestGrade;
                rdata.TestSubject = lastRawdata.TestSubject;
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

        private bool selectedHomeworkYn
        {
            get { return this.rdata.HomeworkYn; }
            set
            {
                selectedHomeworkPerform = value ? selectedHomeworkPrev : "";
                selectedHomeworkCorrect = value ? selectedHomeworkPrev : "";

                this.rdata.HomeworkYn = value;
                this.rdata.HomeworkPrev = selectedHomeworkPrev.ToFloat();
                this.rdata.HomeworkPerform = selectedHomeworkPerform.ToFloat(); ;
                this.rdata.HomeworkCorrect = selectedHomeworkCorrect.ToFloat(); ;

                if ((rdata.HomeworkPrev ?? 0) > 0 && value)
                {
                    /// 과제이행정도 = 과제수행량 / 이전과제량
                    rdata.HomeworkProgress = (float?)(Math.Round((decimal)(rdata.HomeworkPerform ?? 0) / (decimal)rdata.HomeworkPrev * 100, 0));
                }
                else
                    rdata.HomeworkProgress = null;

                if ((rdata.HomeworkPerform ?? 0) > 0 && value)
                {
                    /// 과제정답률 = 과제정답 / 과제수행량
                    rdata.HomeworkRatio = (float?)(Math.Round((decimal)(rdata.HomeworkCorrect ?? 0) / (decimal)rdata.HomeworkPerform * 100, 0));
                }
                else
                    rdata.HomeworkRatio = null;
            }
        }

        private string selectedHomeworkPrev
        { get { return rdata.HomeworkPrev?.ToString() ?? ""; } 
          set { 
                rdata.HomeworkPrev = value.ToFloat(); ;
                if ((rdata.HomeworkPrev ?? 0) > 0)
                {
                    selectedHomeworkPerform = value;
                    selectedHomeworkCorrect = value;
                    /// 과제이행정도 = 과제수행량 / 이전과제량
                    rdata.HomeworkProgress = (float?)(Math.Round((decimal)(rdata.HomeworkPerform ?? 0) / (decimal)rdata.HomeworkPrev * 100, 0));
                }
                else
                    rdata.HomeworkProgress = null;
          }
        }
        private string selectedHomeworkPerform
        {
            get { return rdata.HomeworkPerform?.ToString() ?? ""; }
            set
            {
                rdata.HomeworkPerform = value.ToFloat();
                if ((rdata.HomeworkPrev ?? 0) > 0)
                {
                    /// 과제이행정도 = 과제수행량 / 이전과제량
                    rdata.HomeworkProgress = (float?)(Math.Round((decimal)(rdata.HomeworkPerform ?? 0) / (decimal)rdata.HomeworkPrev * 100, 0));
                }
                else
                    rdata.HomeworkProgress = null ;

                if ((rdata.HomeworkPerform ?? 0) > 0)
                {
                    /// 과제정답률 = 과제정답 / 과제수행량
                    rdata.HomeworkRatio = (float?)(Math.Round((decimal)(rdata.HomeworkCorrect ?? 0) / (decimal)rdata.HomeworkPerform * 100, 0));
                }
                else
                    rdata.HomeworkRatio = null ;
            }
        }
        private string selectedHomeworkCorrect
        {
            get { return rdata.HomeworkCorrect?.ToString() ?? ""; }
            set
            {
                rdata.HomeworkCorrect = value.ToFloat();
                if ((rdata.HomeworkPerform ?? 0) > 0)
                {
                    /// 과제정답률 = 과제정답 / 과제수행량
                    rdata.HomeworkRatio = (float?)(Math.Round((decimal)(rdata.HomeworkCorrect ?? 0) / (decimal)rdata.HomeworkPerform * 100, 0));
                    //rdata.HomeworkYn = true;    // 값을 셋팅하면 yn값을 자동으로 true로 변경한다.
                }
                else
                    rdata.HomeworkRatio = null;
            }
        }

        private bool selectedDailyYn
        {
            get { return this.rdata.DailyYn; }
            set
            {
                selectedDailyCorrect = value ? "20" : "";
                selectedDailyCount = value ? "20" : "";

                this.rdata.DailyYn = value;
                this.rdata.DailyCorrect = selectedDailyCorrect.ToFloat();
                this.rdata.DaliyCount = selectedDailyCount.ToFloat();
                if ((rdata.DaliyCount ?? 0) > 0 && value)
                {
                    /// 데일리정답률 = 일일정답수 / 일일문항수
                    rdata.DailyScore = (float?)(Math.Round((decimal)(rdata.DailyCorrect ?? 0) / (decimal)rdata.DaliyCount * 100, 0));
                }
                else
                    rdata.DailyScore = null;
            }
        }
        private string selectedDailyCount
        {
            get { return rdata.DaliyCount?.ToString() ?? ""; }
            set
            {
                rdata.DaliyCount = value.ToFloat();
                if ((rdata.DaliyCount ?? 0) > 0)
                {
                    selectedDailyCorrect = value;
                    /// 데일리정답률 = 일일정답수 / 일일문항수
                    rdata.DailyScore = (float?)(Math.Round((decimal)(rdata.DailyCorrect ?? 0) / (decimal)rdata.DaliyCount * 100, 0));
                }
                else
                    rdata.DailyScore = null;
            }
        }
        private string selectedDailyCorrect
        {
            get { return rdata.DailyCorrect?.ToString() ?? ""; }
            set
            {
                rdata.DailyCorrect = value.ToFloat();
                if ((rdata.DaliyCount ?? 0) > 0)
                {
                    /// 데일리정답률 = 일일정답수 / 일일문항수
                    rdata.DailyScore = (float?)(Math.Round((decimal)(rdata.DailyCorrect ?? 0) / (decimal)rdata.DaliyCount * 100, 0));
                    //rdata.DailyYn = true;    // 값을 셋팅하면 yn값을 자동으로 true로 변경한다.
                }
                else
                    rdata.DailyScore = null;
            }
        }

        private bool selectedCliniqYn
        {
            get { return this.rdata.CliniqYn; }
            set
            {
                selectedCliniqCorrect = value ? "100" : "";
                selectedCliniqCount = value ? "100" : "";

                this.rdata.CliniqYn = value;
                this.rdata.CliniqCorrect = selectedCliniqCorrect.ToFloat();
                this.rdata.CliniqCount = selectedCliniqCount.ToFloat();
                if ((rdata.CliniqCount ?? 0) > 0 && value)
                {
                    /// 클리닉정답률 = 클리닉정답수 / 클리닉문항수
                    rdata.CliniqScore = (float?)(Math.Round((decimal)(rdata.CliniqCorrect ?? 0) / (decimal)rdata.CliniqCount * 100, 0));
                }
                else
                    rdata.CliniqScore = null;
            }
        }
        private string selectedCliniqCount
        {
            get { return rdata.CliniqCount?.ToString() ?? ""; }
            set
            {
                rdata.CliniqCount = value.ToFloat();
                if ((rdata.CliniqCount ?? 0) > 0)
                {
                    selectedCliniqCorrect = value;
                    /// 클리닉정답률 = 클리닉정답수 / 클리닉문항수
                    rdata.CliniqScore = (float?)(Math.Round((decimal)rdata.CliniqCorrect / (decimal)rdata.CliniqCount * 100, 0));
                }
                else
                    rdata.CliniqScore = null;
            }
        }
        private string selectedCliniqCorrect
        {
            get { return rdata.CliniqCorrect?.ToString() ?? ""; }
            set
            {
                rdata.CliniqCorrect = value.ToFloat();
                if ((rdata.CliniqCount ?? 0) > 0)
                {
                    /// 클리닉정답률 = 클리닉정답수 / 클리닉문항수
                    rdata.CliniqScore = (float?)(Math.Round((decimal)(rdata.CliniqCorrect ?? 0) / (decimal)rdata.CliniqCount * 100, 0));
                    //rdata.CliniqYn = true;    // 값을 셋팅하면 yn값을 자동으로 true로 변경한다.
                }
                else
                    rdata.CliniqScore = 0f;
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
        private List<string> GetRatioList()
        {
            List<string> rtn = new List<string>();
            rtn.Add("");
            for(float i = 130; i >= 0; i--) 
            {
                rtn.Add(i.ToString());
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