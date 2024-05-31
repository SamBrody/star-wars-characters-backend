using System.Reflection;
using AutoMapper;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using StarWars.Characters.Configuration.Data;
using StarWars.Characters.Configuration.MappingProfiles;
using StarWars.Characters.Configuration.Pipeline;
using StarWars.Characters.Configuration.Services;
using StarWars.Characters.Models.Characters;
using StarWars.Characters.Models.Movies;
using StarWars.Characters.Models.Planets;
using StarWars.Characters.Models.Species;
using IMapper = AutoMapper.IMapper;

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
builder.Services.AddScoped<ISpeciesRepository, SpeciesRepository>();
builder.Services.AddScoped<IPlanetRepository, PlanetRepository>();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
});

builder.Services.AddFastEndpoints();

// Configure Automapper
builder.Services.AddAutoMapper(
    typeof(CharacterMappingProfile),
    typeof(MovieMappingProfile),
    typeof(SpeciesMappingProfile),
    typeof(PlanetMappingProfile)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();

    var mapper = app.Services.GetService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();
}

app.UseHttpsRedirection();

app.UseFastEndpoints(config => {
    config.Versioning.PrependToRoute = true;
});
app.Run();