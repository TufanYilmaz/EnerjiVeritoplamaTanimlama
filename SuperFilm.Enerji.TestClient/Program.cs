// See https://aka.ms/new-console-template for more information
using SuperFilm.Enerji.OleDbReader;

Console.WriteLine("Hello, World!");
Client reader = new Client();
reader.ReadFile("29", DateTime.Now.AddDays(-2));
Console.ReadKey();