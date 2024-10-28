using Bat.Blazor.App.Helpers;
using Bat.Blazor.App.Shared;
using Bat.Shared.Api;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Bat.Blazor.App.Pages;

public partial class ApplicationsModify
{
	[Parameter]
	public string Id { get; set; } = string.Empty;

	private string AlertMessage { get; set; } = string.Empty;
	private string AlertType { get; set; } = "info";
	private bool HideUI { get; set; } = false;

	private string AppName { get; set; } = string.Empty;
	private string AppPublicKeyPEM { get; set; } = string.Empty;

	private AppResp? SelectedApp { get; set; }

	private async Task<AppResp?> LoadAppAsync(string id, string authToken)
	{
		HideUI = true;
		ShowAlert("info", "Loading application details. Please wait...");
		var result = await ApiClient.GetAppAsync(id, authToken, NavigationManager.BaseUri);
		if (result.Status == 200)
		{
			return result.Data;
		}
		ShowAlert("danger", result.Message!);
		return null;
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);
		if (firstRender)
		{
			HideUI = true;
			var localStorage = ServiceProvider.GetRequiredService<LocalStorageHelper>();
			var authToken = await localStorage.GetItemAsync<string>(Globals.LOCAL_STORAGE_KEY_AUTH_TOKEN);

			SelectedApp = await LoadAppAsync(Id, authToken ?? "");
			if (SelectedApp == null)
			{
				return;
			}
			AppName = SelectedApp?.DisplayName ?? string.Empty;
			AppPublicKeyPEM = SelectedApp?.PublicKeyPEM ?? string.Empty;

			HideUI = false;
			CloseAlert();
		}
	}

	private void BtnClickCancel()
	{
		NavigationManager.NavigateTo(UIGlobals.ROUTE_APPLICATIONS);
	}

	private void ShowAlert(string type, string message)
	{
		AlertType = type;
		AlertMessage = message;
		StateHasChanged();
	}

	private void CloseAlert()
	{
		AlertType = AlertMessage = string.Empty;
		StateHasChanged();
	}

	private async Task BtnClickSave()
	{
		HideUI = true;
		ShowAlert("info", "Please wait...");
		if (string.IsNullOrWhiteSpace(AppName))
		{
			HideUI = false;
			ShowAlert("warning", "Application name is required.");
			return;
		}
		var req = new CreateOrUpdateAppReq
		{
			DisplayName = AppName.Trim(),
			PublicKeyPEM = AppPublicKeyPEM.Trim(),
		};
		using (var scope = ServiceProvider.CreateScope())
		{
			var localStorage = scope.ServiceProvider.GetRequiredService<LocalStorageHelper>();
			var authToken = await localStorage.GetItemAsync<string>(Globals.LOCAL_STORAGE_KEY_AUTH_TOKEN) ?? string.Empty;
			var resp = await ApiClient.UpdateAppAsync(Id, req, authToken, NavigationManager.BaseUri);
			if (resp.Status != 200)
			{
				HideUI = false;
				ShowAlert("danger", resp.Message!);
				return;
			}
			ShowAlert("success", "Application updated successfully. Navigating to applications list...");
			var passAlertMessage = $"Application '{req.DisplayName}' updated successfully.";
			var passAlertType = "success";
			await Task.Delay(500);
			NavigationManager.NavigateTo($"{UIGlobals.ROUTE_APPLICATIONS}?alertMessage={passAlertMessage}&alertType={passAlertType}");
		}
	}
}