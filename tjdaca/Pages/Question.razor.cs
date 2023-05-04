using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using tjdaca.Data;
using tjdaca.Services;

namespace tjdaca.Pages
{
    public partial class Question : ComponentBase
    {
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] NavigationManager NavManager { get; set; }

        private string searchString = "";
        private List<AcaQuestions> QuestionList = new List<AcaQuestions>();

        protected override async Task OnInitializedAsync()
        {
            GetQuestionList();
        }
        private List<AcaQuestions> GetQuestionList()
        {
            QuestionList = questionService.GetQuestions();
            return QuestionList;
        }
        private bool Search(AcaQuestions question)
        {
            if (string.IsNullOrWhiteSpace(searchString)) return true;
            if (question.StuName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        private void Edit(int qid)
        {
            NavManager.NavigateTo($"/questions/e/{qid}");
        }

        private async void Delete(int qid)
        {
            bool? result = await DialogService.ShowMessageBox(
                        "삭제 확인",
                        "질문내용을 삭제하시면 복구할 수 없습니다. 삭제하시겠습니까?",
                        yesText: "삭제", cancelText: "취소");
            if (result != null)
            {
                questionService.DeleteQuestion(qid);
                snackBar.Add("질문내용 삭제 완료.", Severity.Success);
            }
            StateHasChanged();
        }

        private void AddQuestion()
        {
            NavManager.NavigateTo("/questions/i/0");
        }
    }
}