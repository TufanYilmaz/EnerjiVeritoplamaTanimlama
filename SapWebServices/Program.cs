using Microsoft.EntityFrameworkCore;
using SapWebServices.Helpers;
using SuperFilm.Enerji.Entites;
using TanvirArjel.EFCore.GenericRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<EnerjiDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("EnerjiDbContext")));
builder.Services.AddGenericRepository<EnerjiDbContext>();
builder.Services.AddQueryRepository<EnerjiDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IDataAccess, DataAccess>();
builder.Services.AddScoped<IDB2Helper, DB2Helper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
