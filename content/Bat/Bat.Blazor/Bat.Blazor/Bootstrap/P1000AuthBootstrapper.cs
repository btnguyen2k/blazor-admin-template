﻿using Bat.Blazor.App.Services;
using Bat.Shared.Bootstrap;
using Microsoft.AspNetCore.Components.Authorization;

namespace Bat.Blazor.Bootstrap;

/// <summary>
/// Bootstrapper that registers identity/auth services.
/// </summary>
[Bootstrapper]
public class AuthBootstrapper
{
	public static void ConfigureBuilder(WebApplicationBuilder appBuilder)
	{
		//// set up authorization
		//appBuilder.Services.AddAuthorizationCore();

		// register the custom state provider
		appBuilder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
	}
}
