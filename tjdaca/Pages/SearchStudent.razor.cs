using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Security.Cryptography;
using tjdaca.Data;
using tjdaca.Services;

namespace tjdaca.Pages
{
    public partial class SearchStudent : ComponentBase
    {
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] NavigationManager NavManager { get; set; }

        private void Edit(int rid)
        {
            NavManager.NavigateTo($"/search/score/{rid}");
        }

    }
}