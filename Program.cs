// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using excellent_parser;

Console.WriteLine("Hello, World!");

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