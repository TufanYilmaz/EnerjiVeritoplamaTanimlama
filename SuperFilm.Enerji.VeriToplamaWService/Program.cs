using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.VeriToplamaWService;
using Hangfire;
using SuperFilm.Enerji.VeriToplamaWService.Services;
using TanvirArjel.EFCore.GenericRepository; 


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<EnerjiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EnerjiDbContext")));


builder.Services.AddGenericRepository<EnerjiDbContext>(ServiceLifetime.Scoped);
builder.Services.AddQueryRepository<EnerjiDbContext>(ServiceLifetime.Scoped);


//builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<OpcVeriKaydetWService>();
builder.Services.AddWindowsService(o=>o.ServiceName="SuperEnerji VeriToplama");
var host = builder.Build();
host.Run();
