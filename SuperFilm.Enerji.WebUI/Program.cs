using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperFilm.Enerji.Entites;
using SuperFilm.Enerji.Repository;
using SuperFilm.Enerji.WebUI.Services.Identity;
using TanvirArjel.EFCore.GenericRepository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EnerjiDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("EnerjiDbContext")));

builder.Services.AddGenericRepository<EnerjiDbContext>();
builder.Services.AddQueryRepository<EnerjiDbContext>();
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("EnerjiDbContext")));
builder.Services.AddScoped(typeof(EnerjiVeriRepository<>));
//builder.Services.AddScoped(IEnerjiVeriRepository, EnerjiVeriRepository);


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequiredLength = 8;
	options.Password.RequireUppercase = false;
	options.Password.RequireLowercase = false;
	options.User.RequireUniqueEmail = true;
	options.SignIn.RequireConfirmedEmail = false;
	options.SignIn.RequireConfirmedAccount = false;
	options.SignIn.RequireConfirmedPhoneNumber = false;

}).AddEntityFrameworkStores<AppIdentityDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddRazorPages();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
