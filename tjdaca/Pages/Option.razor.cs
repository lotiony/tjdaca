using Microsoft.AspNetCore.Components;
using MudBlazor;
using tjdaca.Data;
using tjdaca.Services;

namespace tjdaca.Pages
{
    public partial class Option : ComponentBase
    {
        private string searchString = "";
        private Options option = new Options();
        private List<Options> OptionsList = new List<Options>();
        protected override async Task OnInitializedAsync()
        {
            GetOptionList();
        }
        private List<Options> GetOptionList()
        {
            OptionsList = optionService.GetOptions();
            return OptionsList;
        }
        private bool Search(Options option)
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
            option = new Options();
            snackBar.Add("옵션 저장 완료.", Severity.Success);
            GetOptionList();
        }
        private void Edit(int id)
        {
            option = OptionsList.FirstOrDefault(c => c.OIdx == id);
        }
        private void Delete(int id)
        {
            optionService.DeleteOption(id);
            snackBar.Add("옵션 삭제 완료.", Severity.Success);
            GetOptionList();
        }
    }
}