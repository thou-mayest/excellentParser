// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using excellent_parser;

var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddSingleton<IFileService, FileService>()
            .AddSingleton<ICSVService, ParserService>()
            .BuildServiceProvider();

Console.WriteLine("example query (spaces are important in where clause): ");
Console.WriteLine("select deviceVendor,deviceProduct from test.csv WHERE deviceVendor != Microsoft");

ConsoleKey key = ConsoleKey.N;
while(key !=ConsoleKey.E)
{

    Console.WriteLine("Input query: ");
    var queryInput = Console.ReadLine();
    var service = serviceProvider.GetService<ICSVService>();
    var result = service?.Query(queryInput);
    
    Console.WriteLine("json result: " + result);
    Console.WriteLine("press E to exist,any key continue ...");
    key = Console.ReadKey().Key;
}
