using CheckMate.BLL.Services;
using DiamondLegends.BLL.Interfaces;
using DiamondLegends.BLL.Services;
using DiamondLegends.DAL.Interfaces;
using DiamondLegends.DAL.Repositories;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<SqlConnection>(c => new SqlConnection(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICountryService, CountryService>();

builder.Services.AddScoped<AuthService>();

builder.Services.AddCors(service =>
{
    service.AddPolicy("Angular_Front", policy =>
    {
        policy.WithOrigins("http://localhost:3000");
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Angular_Front");

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

app.MapControllers();

app.Run();
