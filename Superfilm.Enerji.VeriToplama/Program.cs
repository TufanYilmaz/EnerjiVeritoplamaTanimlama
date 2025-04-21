using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Hangfire;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.OleDbReader;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EnerjiDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("EnerjiDbContext")));

builder.Services.AddRazorPages();

/* -----------------------------------------------------------------  */
var EnerjiDbContextString = builder.Configuration.GetConnectionString("EnerjiDbContext");

builder.Services.AddDbContext<EnerjiDbContext>(options =>
    options.UseSqlServer(EnerjiDbContextString));

builder.Services.AddHangfire(x =>
    x.UseSqlServerStorage(EnerjiDbContextString));
builder.Services.AddHangfireServer();

builder.Services.AddScoped<Client>();

/* -----------------------------------------------------------------  */


var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();


/* -----------------------------------------------------------------  */
app.UseHangfireDashboard(); // http://localhost:5000/hangfire ile kontrol edebilirsin

// Baþlangýçta job’u baþlat:
RecurringJob.AddOrUpdate<Client>(
    "mdb-okuyucu-job",
    x => x.Run(),
    Cron.Minutely // her dakika çalýþýr
);
/* -----------------------------------------------------------------  */

app.Run();
