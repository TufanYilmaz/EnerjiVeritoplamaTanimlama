using SuperFilm.Enerji.CollectData;
using SuperFilm.Enerji.CollectData.Clients;
using SuperFilm.Enerji.CollectData.Clients.Interfaces;
using SuperFilm.Enerji.CollectData.Readers;
using SuperFilm.Enerji.Entites;
using TanvirArjel.EFCore.GenericRepository;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddGenericRepository<EnerjiDbContext>();
builder.Services.AddQueryRepository<EnerjiDbContext>();
builder.Services.AddScoped<IOpcUAReader, OpcUAReader>();
builder.Services.AddScoped<IOpcUAClient, OpcUAClient>();
var host = builder.Build();
host.Run();
