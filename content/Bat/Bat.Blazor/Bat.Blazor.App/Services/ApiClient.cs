﻿using Bat.Shared.Api;
using System.Net.Http.Json;

namespace Bat.Blazor.App.Services;

public class ApiClient(HttpClient httpClient) : IApiClient
{
	private void UsingBaseUrlAndHttpClient(string? baseUrl, HttpClient? requestHttpClient, out string usingBaseUrl, out HttpClient usingHttpClient)
	{
		usingBaseUrl = string.IsNullOrEmpty(baseUrl) ? Globals.ApiBaseUrl ?? string.Empty : baseUrl;
		usingHttpClient = requestHttpClient ?? httpClient;
	}

	private static HttpRequestMessage BuildRequest(HttpMethod method, Uri endpoint, string? authToken)
	{
		return BuildRequest(method, endpoint, authToken, null);
	}

	private static HttpRequestMessage BuildRequest(HttpMethod method, Uri endpoint, string? authToken, object? requestData)
	{
		var req = new HttpRequestMessage(method, endpoint)
		{
			Headers = {
				{ "Accept", "application/json" }
			}
		};
		if (!string.IsNullOrEmpty(authToken))
		{
			req.Headers.Add("Authorization", $"Bearer {authToken}");
		}
		if (requestData != null)
		{
			req.Content = JsonContent.Create(requestData);
		}
		return req;
	}

	/// <inheritdoc/>
	public async Task<ApiResp<InfoResp>> InfoAsync(string? baseUrl = default, HttpClient? requestHttpClient = default, CancellationToken cancellationToken = default)
	{
		UsingBaseUrlAndHttpClient(baseUrl, requestHttpClient, out var usingBaseUrl, out var usingHttpClient);
		var apiUri = new Uri(new Uri(usingBaseUrl), IApiClient.API_ENDPOINT_INFO);
		// simple GET request, we don't need to build a request message
		var result = await usingHttpClient.GetFromJsonAsync<ApiResp<InfoResp>>(apiUri, cancellationToken);
		return result ?? new ApiResp<InfoResp> { Status = 500, Message = "Invalid response from server." };
	}

	/// <inheritdoc/>
	public async Task<ApiResp<AuthResp>> LoginAsync(LoginReq req, string? baseUrl = null, HttpClient? requestHttpClient = null, CancellationToken cancellationToken = default)
	{
		UsingBaseUrlAndHttpClient(baseUrl, requestHttpClient, out var usingBaseUrl, out var usingHttpClient);
		var apiUri = new Uri(new Uri(usingBaseUrl), IApiClient.API_ENDPOINT_AUTH_SIGNIN);
		var httpReq = BuildRequest(HttpMethod.Post, apiUri, null, req);
		var httpResult = await usingHttpClient.SendAsync(httpReq, cancellationToken);
		var result = await httpResult.Content.ReadFromJsonAsync<ApiResp<AuthResp>>(cancellationToken);
		return result ?? new ApiResp<AuthResp> { Status = 500, Message = "Invalid response from server." };
	}

	/// <inheritdoc/>
	public async Task<ApiResp<AuthResp>> RefreshAsync(string authToken, string? baseUrl = null, HttpClient? requestHttpClient = null, CancellationToken cancellationToken = default)
	{
		UsingBaseUrlAndHttpClient(baseUrl, requestHttpClient, out var usingBaseUrl, out var usingHttpClient);
		var apiUri = new Uri(new Uri(usingBaseUrl), IApiClient.API_ENDPOINT_AUTH_REFRESH);
		var httpReq = BuildRequest(HttpMethod.Post, apiUri, authToken);
		var httpResult = await usingHttpClient.SendAsync(httpReq, cancellationToken);
		var result = await httpResult.Content.ReadFromJsonAsync<ApiResp<AuthResp>>(cancellationToken);
		return result ?? new ApiResp<AuthResp> { Status = 500, Message = "Invalid response from server." };
	}
}