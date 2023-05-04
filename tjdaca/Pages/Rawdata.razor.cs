using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using tjdaca.Data;
using tjdaca.Services;

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
            if (student.StuName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
            || student.ClassName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
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
    }
}