using src.Models;
using Microsoft.EntityFrameworkCore;
using src.Services;
using src.Dao;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_myAllowSpecificOrigins",
                      policy  =>
                      {
                          policy.WithOrigins(
                            "http://192.168.1.75:3000",
                            "http://localhost:3000",
                            "http://0.0.0.0:3000",
                            "http://*")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});
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

app.UseSwagger();
app.UseSwaggerUI();


app.UseCors("_myAllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.Run();

public partial class Program { }

