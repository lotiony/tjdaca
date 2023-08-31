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

namespace tjdaca.Pages
{
    public partial class Student : ComponentBase
    {
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] NavigationManager NavManager { get; set; }

        private string searchString = "";
        private List<AcaStudents> StudentsList = new List<AcaStudents>();

        protected override async Task OnInitializedAsync()
        {
            string isverified = string.Empty;
            string isadmin = string.Empty;

            try
            {
                isverified = await sessionStorage.GetItemAsync<string>("IsPasswordVerified");
                isadmin = await sessionStorage.GetItemAsync<string>("IsAdministrator");
            }
            catch (Exception)
            {
                isverified = string.Empty;
                isadmin = string.Empty;
            }
            
            DateTime sessionTime = new DateTime();
            if (!string.IsNullOrEmpty(isverified)) DateTime.TryParse(isverified, out sessionTime);
            if (!string.IsNullOrEmpty(isadmin)) bool.TryParse(isadmin, out isAdministrator);

            if (sessionTime.AddMinutes(30) > DateTime.Now)
            {
                isPasswordVerified = true;
                passwordRequired = false;
            }
            else 
            { 
                isPasswordVerified = false;
                passwordRequired = true;
            }

            GetStudentList();
        }

        private List<AcaStudents> GetStudentList()
        {
            StudentsList = studentService.GetStudents();
            return StudentsList;
        }
        private bool Search(AcaStudents student)
        {
            if (string.IsNullOrWhiteSpace(searchString)) return true;
            if ((student.StuName?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? true)
            || (student.StudentPhone?.Contains(searchString, StringComparison.OrdinalIgnoreCase) ?? true)
            || (student.Parent?.Contains(searchString, StringComparison.Ordinal) ?? true)
            )
            {
                return true;
            }
            return false;
        }
        private void Edit(int id)
        {
            NavManager.NavigateTo($"/students/detail/u/{id}");
        }
        private void AdminEdit(int id)
        {
            NavManager.NavigateTo($"/students/detail/a/{id}");
        }

        private async void Delete(int id)
        {
            bool? result = await DialogService.ShowMessageBox(
                        "삭제 확인",
                        "학생을 삭제하시면 복구할 수 없습니다. 삭제하시겠습니까?",
                        yesText: "삭제", cancelText: "취소");
            if (result != null)
            {
                studentService.DeleteStudent(id);
                snackBar.Add("학생 삭제 완료.", Severity.Success);
                GetStudentList();
            }
            StateHasChanged();
        }

        private void AddStudent()
        {
            NavManager.NavigateTo("/students/detail/u/0");
        }

        // 엑셀 다운로드 메서드


        private bool _processing = false;
        public async Task DownloadExcel()
        {
            _processing = false;

            var properties = typeof(AcaStudents).GetProperties();

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
                var fileName = $"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_특자단_학생데이터.xlsx";

                // 파일 다운로드
                await JS.InvokeVoidAsync("downloadFile", Convert.ToBase64String(content), contentType, fileName);
            }

            _processing = false;
        }

    }
}