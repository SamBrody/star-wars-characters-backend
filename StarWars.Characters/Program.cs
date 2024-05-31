using System.Reflection;
using FastEndpoints;
using FastEndpoints.Swagger;
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

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o => {
        o.DocumentSettings = s => {
            s.Title = "StarWars.Characters API";
            s.Version = "v1";
        };
        o.MaxEndpointVersion = 1;

        o.ShortSchemaNames = true;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

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

// Configure Automapper
builder.Services.AddAutoMapper(
    typeof(CharacterMappingProfile),
    typeof(MovieMappingProfile),
    typeof(SpeciesMappingProfile),
    typeof(PlanetMappingProfile)
);

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    var mapper = app.Services.GetService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();
}

app.UseFastEndpoints(cfg => cfg.Versioning.PrependToRoute = true);
app.UseSwaggerGen();

app.UseHttpsRedirection();

app.Run();