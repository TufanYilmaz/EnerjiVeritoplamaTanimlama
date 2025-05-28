using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Hangfire;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.OleDbReader;
using Superfilm.Enerji.VeriToplama.Services;
using TanvirArjel.EFCore.GenericRepository;
using SuperFilm.Enerji.OpcUAReader;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EnerjiDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("EnerjiDbContext")));

builder.Services.AddRazorPages();

/* -----------------------------------------------------------------  */
var EnerjiDbContextString = builder.Configuration.GetConnectionString("EnerjiDbContext");

builder.Services.AddDbContext<EnerjiDbContext>(options =>
{
    options.UseSqlServer(EnerjiDbContextString);
});

builder.Services.AddHangfire(x =>
x.UseInMemoryStorage()
//x.UseSqlServerStorage(EnerjiDbContextString)
    );

builder.Services.AddSingleton<IUAReaderClient>(x=> new UAReaderClient(builder.Configuration.GetValue<string>("opcUrl") ?? ""));
builder.Services.AddScoped<ISayacVeriKaydet,SayacVeriKaydet>();
builder.Services.AddScoped<IOpcVeriKaydet, OpcVeriKaydet>();
builder.Services.AddHangfireServer();
builder.Services.AddGenericRepository<EnerjiDbContext>();
builder.Services.AddQueryRepository<EnerjiDbContext>();

/* -----------------------------------------------------------------  */

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();


/* -----------------------------------------------------------------  */
app.UseHangfireDashboard(); // http://localhost:5000/hangfire ile kontrol edebilirsin


// Baþlangýçta job’u baþlat:
//RecurringJob.AddOrUpdate<ISayacVeriKaydet>(
//    "mdb-okuyucu-job",
//    x => x.Start(),
//    Cron.Hourly // her dakika çalýþýr
//);
RecurringJob.AddOrUpdate<IOpcVeriKaydet>(
    "opc-okuyucu-job",
    x => x.Start(CancellationToken.None),
    Cron.Minutely() // her dakika çalýþýr
);
/* -----------------------------------------------------------------  */

app.Run();
