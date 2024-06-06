using System.Reflection;
using FastEndpoints;
using FastEndpoints.Security;
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
using StarWars.Characters.Models.Users;
using IMapper = AutoMapper.IMapper;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();

builder.Services.AddDbContext<StarWarsCharactersDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHostedService<PersistenceMigrator<StarWarsCharactersDbContext>>();

// Register repositories
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ISpeciesRepository, SpeciesRepository>();
builder.Services.AddScoped<IPlanetRepository, PlanetRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
    cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
});

// Set-up Authentication
builder.Services
    .AddAuthenticationJwtBearer(s => s.SigningKey = "The secret used to sign tokens")
    .AddAuthorization()
    .AddFastEndpoints()
    .SwaggerDocument(o => {
        o.DocumentSettings = s => {
            s.Title = "StarWars.Characters API";
            s.Version = "v1";
        };
        o.MaxEndpointVersion = 1;

        o.ShortSchemaNames = true;
    });
    
// Configure Automapper
builder.Services.AddAutoMapper(
    typeof(CharacterMappingProfile).Assembly,
    typeof(MovieMappingProfile).Assembly,
    typeof(SpeciesMappingProfile).Assembly,
    typeof(PlanetMappingProfile).Assembly,
    typeof(UserMappingProfile).Assembly
);

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    var mapper = app.Services.GetService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();
}

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(o => true));

app.UseAuthentication()
   .UseAuthentication()
   .UseFastEndpoints(cfg => cfg.Versioning.PrependToRoute = true);

app.UseSwaggerGen();

app.UseHttpsRedirection();

app.Run();