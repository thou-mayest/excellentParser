// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using excellent_parser;

Console.WriteLine("Hello, World!");

// HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// builder.Services.AddTransient<IFileService, FileService>();
// builder.Services.AddTransient<ICSVService,ParserService>();

// using IHost host = builder.Build();

// await host.RunAsync();

// var path = Console.ReadLine();
// Whatver(host.Services, path);



// void Whatver (IServiceProvider hostProvider, string args)
// {
//     using IServiceScope serviceScope = hostProvider.CreateScope();
//     var service = serviceScope.ServiceProvider.GetService<ICSVService>();

//     Console.WriteLine(service?.Query(args));
// }


var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddSingleton<IFileService, FileService>()
            .AddSingleton<ICSVService, ParserService>()
            .BuildServiceProvider();

Console.WriteLine("Input query: ");
var queryInput = Console.ReadLine();
var service = serviceProvider.GetService<ICSVService>();
var result = service?.Query(queryInput);
Console.WriteLine("done: " + result);