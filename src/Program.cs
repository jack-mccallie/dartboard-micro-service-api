using games_recording_service.Models;
using Microsoft.EntityFrameworkCore;
using games_recording_service.Services;
using Microsoft.Extensions.Configuration;
using src.Dao;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<GameContext>(opt =>
    opt.UseInMemoryDatabase("GameResults"));

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddSingleton<IDatabaseDao>(x => 
    new MongoDatabaseDao(builder.Configuration.GetConnectionString("BigTen")!)
);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.Run();

public partial class Program { }

