using ClosedXML.Excel;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using tjdaca.Data;
using System.Reflection;
using DocumentFormat.OpenXml.CustomProperties;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.JSInterop;
using tjdaca.Class;
using System.ComponentModel.DataAnnotations;
using tjdaca.Services;
using tjdaca.Models;

namespace tjdaca.Pages
{
    public partial class StudentReport : ComponentBase
    {
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] NavigationManager NavManager { get; set; }
        [Inject] IOptionService optionService { get; set; }
        [Inject] IStudentService studentService { get; set; }
        [Inject] IRawdataService rawdataService { get; set; }

        private List<AcaStudents> _studentList = new List<AcaStudents>();
        private int selectedRowNumber = -1;
        private MudTable<AcaStudents> StudentTable;

        private List<AcaRawdata> _dailyList = new List<AcaRawdata>();
        private List<AcaRawdata> _cliniqList = new List<AcaRawdata>();
        private AcaStuSchoolScore _schoolScore = new AcaStuSchoolScore();
        private List<AcaStuExamScore> _examScore = new List<AcaStuExamScore>();
        private List<ChartSeries> _series = new List<ChartSeries>();
        private string[] _xaxisLabels;
        private ChartOptions _chartOption;

        #region ss 
        private AcaStudents _ss; // selectedStudent
        public AcaStudents ss
        {
            get { return this._ss; }
            set
            {
                if (this._ss != value)
                {
                    this._ss = value;
                    GetStudentReport();
                }
            }
        }
        #endregion
        #region setDataCount 
        private int _setDataCount = 10;
        public int setDataCount
        {
            get { return this._setDataCount; }
            set
            {
                if (this._setDataCount != value)
                {
                    this._setDataCount = value;
                    GetStudentReport();
                }
            }
        }
        #endregion

        private List<string> teacherList { get; set; }
        private string _schString = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            InitInputForm();
        }

        private void InitInputForm()
        {
            teacherList = optionService.GetTeachers();
            _chartOption = new ChartOptions();
            _chartOption.YAxisTicks = 10;
            _chartOption.XAxisLines = true;
            _chartOption.MaxNumYAxisTicks = 110;
        }


        private bool Filtering(AcaStudents s)
        {
            if (
                (string.IsNullOrEmpty(_schString) ? true : (s.StuIdx.ToString().Equals(_schString) || s.StuName.Contains(_schString) )))
        {
                return true;
            }
            return false;
        }


        private void Search()
        {
            _studentList = studentService.GetStudents();
        }

        private string SelectStudent(AcaStudents stu, int rowNumber)
        {
            if (selectedRowNumber == rowNumber)
            {
                selectedRowNumber = -1;
                return string.Empty;
            }
            else if (StudentTable.SelectedItem != null && StudentTable.SelectedItem.Equals(stu))
            {
                selectedRowNumber = rowNumber;
                return "selected";
            }
            else
            {
                return string.Empty;
            }
        }

        private void GetStudentReport()
        {
            if (_ss != null)
            {
                var rawList = rawdataService.GetRawdatasBySid(_ss.StuIdx);

                _dailyList = rawList.Where(x => x.DailyScore != null).OrderByDescending(x => x.RIdx).Take(_setDataCount).ToList();
                _cliniqList = rawList.Where(x => x.CliniqScore != null).OrderByDescending(x => x.RIdx).Take(_setDataCount).ToList();
                _schoolScore = studentService.GetSchoolScore(_ss.StuIdx);
                _examScore = studentService.GetExamScore(_ss.StuIdx);


                /// 차트용 데이터
                _series.Clear();
                double[] _chartData = new double[_setDataCount];
                _xaxisLabels = new string[_setDataCount];

                for (int i = 0; i < _setDataCount; i++)
                {
                    _xaxisLabels[i] = string.Empty;
                    if (_dailyList.Count >= (i+1))
                    {
                        _chartData[i] = Convert.ToDouble(_dailyList[i]?.DailyScore ?? 0);
                        _xaxisLabels[i] = _dailyList[i]?.TestSubject ?? "";
                    }
                }
                ChartSeries chartSeries = new ChartSeries()
                {
                    Name = "일일테스트점수",
                    Data = _chartData
                };
                _series.Add(chartSeries);

                
            }
            else
            {
                _dailyList.Clear();
                _cliniqList.Clear();
                _schoolScore = new AcaStuSchoolScore();
                _examScore.Clear();
            }
        }
    }
}