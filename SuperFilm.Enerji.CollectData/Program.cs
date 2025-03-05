using SuperFilm.Enerji.CollectData;
using SuperFilm.Enerji.Entites;
using TanvirArjel.EFCore.GenericRepository;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddGenericRepository<EnerjiDbContext>();
builder.Services.AddQueryRepository<EnerjiDbContext>();
var host = builder.Build();
host.Run();
