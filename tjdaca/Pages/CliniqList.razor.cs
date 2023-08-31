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
    public partial class CliniqList : ComponentBase
    {
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] NavigationManager NavManager { get; set; }
        [Inject] ICliniqService cliniqService { get; set; }
        [Inject] IOptionService optionService { get; set; }

        private List<StuCliniq> _cliniqList = new List<StuCliniq>();

        private List<string> teacherList { get; set; }
        private string _schTeacher = "0";
        private string _schSchType = "0";
        private string _schSchGrade = "0";
        private string _schScore = "0";

        #region dateRange 
        private DateRange _dateRange = new DateRange(DateTime.Now.AddDays(-7), DateTime.Now);
        public DateRange dateRange
        {
            get { return this._dateRange; }
            set
            {
                if (this._dateRange != value)
                {
                    TimeSpan t = ((value.End ?? DateTime.Now) - (value.Start ?? DateTime.Now));
                    if ( t.TotalDays <= 15 ) 
                    {
                        this._dateRange = value;
                    }
                    else
                    {
                        snackBar.Add("기간범위는 최대 15일 까지 가능합니다.", Severity.Warning);
                        this._dateRange.End = value.End;
                        this._dateRange.Start = value.End?.AddDays(-15) ?? DateTime.Now.AddDays(-15);
                    }
                    _schDays = 0;
                    StateHasChanged();
                }
            }
        }
        #endregion

        #region schDays 
        private int _schDays = 7;
        public int schDays
        {
            get { return this._schDays; }
            set
            {
                if (this._schDays != value)
                {
                    this._schDays = value;
                    this._dateRange.End = DateTime.Now;
                    switch (value)
                    {
                        case 7:
                            this._dateRange.Start = DateTime.Now.AddDays(-7);
                            break;
                        case 15:
                            this._dateRange.Start = DateTime.Now.AddDays(-15);
                            break;
                    }
                    snackBar.Add($"{this._dateRange.Start?.ToShortDateString() ?? ""} ~ {this._dateRange.End?.ToShortDateString() ?? ""}지정", Severity.Info);
                    StateHasChanged();
                }
            }
        }
        #endregion

        private bool _isProcessing = false;

        protected override async Task OnInitializedAsync()
        {
            InitInputForm();
            //GetStudentList();
        }

        private void InitInputForm()
        {
            teacherList = optionService.GetTeachers();
        }


        private bool Filtering(StuCliniq stat)
        {
            if (
                (_schTeacher == "0" ? true : stat.Teacher.Contains(_schTeacher, StringComparison.OrdinalIgnoreCase)) &&
                (_schSchGrade == "0" ? true : CompareGrade(stat, _schSchGrade)) &&
                (_schScore == "0" ? true : CompareScore(stat, _schScore)))
        {
                return true;
            }
            return false;
        }
        private bool CompareGrade(StuCliniq s, string sch)
        {
            string type = $"{sch[0]}등";
            string grade = sch[1].ToString();

            return s.SchType.Equals(type) && s.SchGrade.Equals(grade);
        }
        private bool CompareScore(StuCliniq s, string sch)
        {
            switch (sch)
            {
                case "1":
                    return s.AverageScore >= 90;
                case "2":
                    return s.AverageScore < 90 && s.AverageScore >= 80;
                case "3":
                    return s.AverageScore < 80 && s.AverageScore >= 70;
                case "4":
                    return s.AverageScore < 70;
                default:
                    return false;
            }
        }

        private void Search()
        {
            _isProcessing = true;
            StateHasChanged();
            try
            {
                _cliniqList = cliniqService.GetCliniqList(_dateRange.Start ?? DateTime.Now.AddDays(-7), _dateRange.End ?? DateTime.Now);
            }
            catch (Exception ex)
            {
                snackBar.Add($"오류 => {ex.Message}", Severity.Error);
            }
            finally
            {
                _isProcessing = false;
            }
        }
     

    }
}