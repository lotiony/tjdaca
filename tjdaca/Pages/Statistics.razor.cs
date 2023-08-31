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
    public partial class Statistics : ComponentBase
    {
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] NavigationManager NavManager { get; set; }

        private string searchString = "";
        private List<StuStatistics> StudentsList = new List<StuStatistics>();

        private List<AcaOptions> schTypeList { get; set; }
        private List<AcaOptions> schGradeList { get; set; }
        private List<string> teacherList { get; set; }

        protected override async Task OnInitializedAsync()
        {
            InitInputForm();
            //GetStudentList();
        }

        private void InitInputForm()
        {
            schTypeList = optionService.GetOptions("학교구분");
            schGradeList = optionService.GetOptions("학년구분");
            teacherList = optionService.GetTeachers();
        }


        private List<StuStatistics> GetStudentList()
        {
            StudentsList = statisticsService.GetStatisticsList();
            return StudentsList;
        }
     

        private bool _processing = false;
        public async Task DownloadExcel()
        {
            _processing = false;

            var properties = typeof(StuStatistics).GetProperties();

            // 엑셀 템플릿 생성
            var workbook = new XLWorkbook();

            // 워크시트 생성
            var worksheet = workbook.Worksheets.Add("Data");

            // 헤더 행 추가
            var headerRow = worksheet.Row(1);
            int colIdx = 1;
            // 헤더 셀 추가
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                if (property.GetCustomAttribute<IgnoreExcelAttribute>() != null) continue;

                headerRow.Cell(colIdx).Value = property.GetCustomAttribute<DisplayAttribute>()?.Name ?? property.Name;
                colIdx++;
            }

            
            // 데이터 셀 추가
            for (int rowIndex = 0; rowIndex < StudentsList.Count; rowIndex++)
            {
                var data = StudentsList[rowIndex];
                var dataRow = worksheet.Row(rowIndex + 2);
                colIdx = 1;
                for (int columnIndex = 0; columnIndex < properties.Length; columnIndex++)
                {
                    var property = properties[columnIndex];

                    // 특성 검사
                    if (property.GetCustomAttribute<IgnoreExcelAttribute>() != null)
                    {
                        continue; // 필드를 제외하고 다음 속성으로 이동
                    }

                    var propertyValue = property.GetValue(data);
                    dataRow.Cell(colIdx).Value = propertyValue?.ToString() ?? "";
                    colIdx++;
                }
            }

            // 엑셀 파일 저장
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = $"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_특자단_성적현황.xlsx";

                // 파일 다운로드
                await JS.InvokeVoidAsync("downloadFile", Convert.ToBase64String(content), contentType, fileName);
            }

            _processing = false;
        }

    }
}