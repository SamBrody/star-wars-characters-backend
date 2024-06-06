using System.Reflection;
using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
var appAuth = builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

var signingKey = builder.Configuration.GetValue<string>("SigningKey");

var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));

appAuth.AddJwtBearer(opt => {
    opt.TokenValidationParameters.IssuerSigningKey = securityKey;
    opt.TokenValidationParameters.ValidateIssuerSigningKey = true;

    opt.TokenValidationParameters.ValidateLifetime = true;
    opt.TokenValidationParameters.ClockSkew = TimeSpan.Parse("23:59:59.9999");

    opt.TokenValidationParameters.ValidAudience = null;
    opt.TokenValidationParameters.ValidateAudience = false;

    opt.TokenValidationParameters.ValidIssuer = null;
    opt.TokenValidationParameters.ValidateIssuer = false;
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

app.UseFastEndpoints(cfg => cfg.Versioning.PrependToRoute = true);
app.UseSwaggerGen();

app.UseHttpsRedirection();

app.Run();