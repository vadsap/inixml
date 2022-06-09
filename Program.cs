using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.PlatformAbstractions;
using System.Diagnostics;

var Configuration = new ConfigurationBuilder()
    .AddIniFile("in.ini", optional: false, reloadOnChange: true)
    .Build();

Console.WriteLine(Configuration["Position:Title_"]);

var dl = new DiagnosticListener("Microsoft.AspNetCore");
var services = new ServiceCollection();
var applicationEnvironment = PlatformServices.Default.Application;
services.AddSingleton(applicationEnvironment);
services.AddLogging();
services.AddMvc();
services.AddSingleton<Cshtml.RRender>();
services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
services.AddSingleton<DiagnosticListener>(dl);
services.AddSingleton<DiagnosticSource>(dl);

var provider = services.BuildServiceProvider();
var razor = provider.GetRequiredService<Cshtml.RRender>();
var view = await razor.Render("view.cshtml", Configuration);

Console.ReadLine();