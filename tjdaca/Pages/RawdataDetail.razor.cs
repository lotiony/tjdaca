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
        private List<AcaStudents> studentList { get; set; }
        private List<string> classGradeList { get; set; }
        private List<float?> ratioList { get; set; }
        protected override async Task OnInitializedAsync()
        {
            InitInputForm();

            if (ridx_int == 0)
            {
                rdata = new AcaRawdata();
                rdata.TIdx = tidx_int;
            }

            else
            {
                rdata = rawdataService.GetRawdataById(ridx_int);
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
            classGradeList = GetClassGradeList();
            ratioList = GetRatioList();
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
        private List<float?> GetRatioList()
        {
            List<float?> rtn = new List<float?>();
            for(float i = 100; i >= 0; i--) 
            {
                rtn.Add(i);
            }
            return rtn;
        }
    }
}