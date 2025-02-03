using CheckMate.BLL.Services;
using DiamondLegends.BLL.Generators;
using DiamondLegends.BLL.Services;
using DiamondLegends.DAL.Factories.Interfaces;
using DiamondLegends.DAL.Factories;
using DiamondLegends.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using DiamondLegends.BLL.Services.Interfaces;
using DiamondLegends.BLL.Generators.Interfaces;
using DiamondLegends.DAL.Repositories.Interfaces;
using DiamondLegends.API.Hubs;
using DiamondLegends.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {

    // c.OperationFilter<SwaggerDefaultValues>();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });

    // Permet d'ajouter le "cadenas" sur les routes
    // - Impl�mentation simple (Cadenas sur toutes les routes)
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    // - Plus d'infos : 
    // https://github.com/domaindrivendev/Swashbuckle.AspNetCore?tab=readme-ov-file#add-security-definitions-and-requirements
});

builder.Services.AddTransient<IDbConnectionFactory>(sp =>
    new DbConnectionFactory(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICountryService, CountryService>();

builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamService, TeamService>();

builder.Services.AddScoped<ILeagueRepository, LeagueRepository>();
builder.Services.AddScoped<ILeagueService, LeagueService>();

builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerService, PlayerService>();

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameService, GameService>();

builder.Services.AddScoped<IOffensiveStatsRepository, OffensiveStatsRepository>();

builder.Services.AddScoped<IPitchingStatsRepository, PitchingStatsRepository>();

builder.Services.AddScoped<IPlayByPlayNotifier, PlayByPlayNotifier>();

builder.Services.AddScoped<LeagueNameGenerator>();
builder.Services.AddScoped<TeamGenerator>();
builder.Services.AddScoped<PlayerGenerator>();
builder.Services.AddScoped<SeasonGenerator>();

builder.Services.AddScoped<ILineUpGenerator, LineUpGenerator>();

builder.Services.AddScoped<AuthService>();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],

        ValidateLifetime = true
    };
});

builder.Services.AddCors(service =>
{
    service.AddPolicy("Angular_Front", policy =>
    {
        policy.WithOrigins("http://localhost:3000");
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
        policy.AllowCredentials();
    });
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Angular_Front");

app.UseAuthentication();
app.UseAuthorization();

// TODO : Outsource in separate file
app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (ArgumentNullException e)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(e.Message);
    }
    catch (UnauthorizedAccessException e)
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsJsonAsync(e.Message);
    }
    // TODO : Catch error 403
    catch (ArgumentException e)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsJsonAsync(e.Message);
    }
    catch (Exception e)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(e.Message);
    }
});

app.UseWebSockets();
app.MapHub<PlayByPlayHub>("playbyplayhub");

app.MapControllers();

app.Run();
