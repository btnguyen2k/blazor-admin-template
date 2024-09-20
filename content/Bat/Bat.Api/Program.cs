﻿using Bat.Api;

var appBuilder = WebApplication.CreateBuilder(args);
var tasks = AppBootstrapper.Bootstrap(appBuilder, out var app);
await Task.Run(() =>
{
	var logger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger("Program");
	logger.LogInformation("Waiting for background bootstrapping tasks...");
	AppBootstrapper.WaitForBackgroundTasks(tasks);
	Globals.Ready = true; // server is ready to handle requests
	logger.LogInformation("Background bootstrapping completed.");
});
app.Run();
