using Microsoft.AspNetCore.Components;
using MudBlazor;
using tjdaca.Data;
using tjdaca.Services;

namespace tjdaca.Pages
{
    public partial class Option : ComponentBase
    {
        private string searchString = "";
        private AcaOptions option = new AcaOptions();
        private List<AcaOptions> AcaOptionsList = new List<AcaOptions>();
        protected override async Task OnInitializedAsync()
        {
            GetOptionList();
        }
        private List<AcaOptions> GetOptionList()
        {
            AcaOptionsList = optionService.GetOptions();
            return AcaOptionsList;
        }
        private bool Search(AcaOptions option)
        {
            if (string.IsNullOrWhiteSpace(searchString)) return true;
            if (option.OptValue.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        private void Save()
        {
            optionService.SaveOption(option);
            option = new AcaOptions();
            snackBar.Add("옵션 저장 완료.", Severity.Success);
            GetOptionList();
        }
        private void Edit(int id)
        {
            option = AcaOptionsList.FirstOrDefault(c => c.OIdx == id);
        }
        private async void Delete(int id)
        {
            bool? result = await DialogService.ShowMessageBox(
                   "삭제 확인",
                   "데이터를 삭제하시면 복구할 수 없습니다. 삭제하시겠습니까?",
                   yesText: "삭제", cancelText: "취소");
            if (result != null)
            {
                optionService.DeleteOption(id);
                snackBar.Add("옵션 삭제 완료.", Severity.Success);
            }
            StateHasChanged();
        }
    }
}