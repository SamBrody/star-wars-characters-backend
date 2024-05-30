using System.Reflection;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using StarWars.Characters.Configuration.Data;
using StarWars.Characters.Configuration.Services;
using StarWars.Characters.Models.Characters;
using StarWars.Characters.Models.Movies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StarWarsCharactersDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHostedService<PersistenceMigrator<StarWarsCharactersDbContext>>();

builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();

builder.Services.AddMediatR(config => {
    config.RegisterServicesFromAssemblies(
        Assembly.GetExecutingAssembly()
    );
});

builder.Services.AddFastEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseFastEndpoints(config => {
    config.Versioning.PrependToRoute = true;
});
app.Run();