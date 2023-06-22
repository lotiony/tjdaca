using ClosedXML.Excel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using tjdaca.Class;
using tjdaca.Data;

namespace tjdaca.Pages
{
    public partial class Rawdata : ComponentBase
    {
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] NavigationManager NavManager { get; set; }

        [Parameter]
        public string tid { get; set; } = string.Empty;

        private string searchString = "";
        private List<Data.AcaRawdata> RawdataList = new List<Data.AcaRawdata>();

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(tid))
            {
                GetRawdataList();
            }
        }
        private List<Data.AcaRawdata> GetRawdataList()
        {
            Int32.TryParse(tid, out var tid_int);
            RawdataList = rawdataService.GetRawdatasByTid(tid_int);
            return RawdataList;
        }
        private bool Search(AcaRawdata student)
        {
            if (string.IsNullOrWhiteSpace(searchString)) return true;
            if (student.StuName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        private void Edit(int rid)
        {
            NavManager.NavigateTo($"/rawdata/{tid}/detail/{rid}");
        }

        private async void Delete(int rid)
        {
            bool? result = await DialogService.ShowMessageBox(
                        "삭제 확인",
                        "데이터를 삭제하시면 복구할 수 없습니다. 삭제하시겠습니까?",
                        yesText: "삭제", cancelText: "취소");
            if (result != null)
            {
                rawdataService.DeleteRawdata(rid);
                snackBar.Add("데이터 삭제 완료.", Severity.Success);
            }
            StateHasChanged();
        }

        private void AddRawdata()
        {
            NavManager.NavigateTo($"/rawdata/{tid}/detail/0");
        }

        // 엑셀 다운로드 메서드
        public async Task DownloadExcel()
        {

            var properties = typeof(AcaRawdata).GetProperties();

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
            for (int rowIndex = 0; rowIndex < RawdataList.Count; rowIndex++)
            {
                var data = RawdataList[rowIndex];
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
                Int32.TryParse(tid, out var tid_int);
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = $"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}_{rawdataService.GetTeacherNameByTid(tid_int)}_RAWDATA.xlsx";

                // 파일 다운로드
                await JS.InvokeVoidAsync("downloadFile", Convert.ToBase64String(content), contentType, fileName);
            }

        }

    }
}