using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Reflection.Emit;
using UdemyReact;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
//.ConfigureApiBehaviorOptions(options =>
//{
//    options.SuppressConsumesConstraintForFormFileParameters = true;
//    options.SuppressInferBindingSourcesForParameters = true;
//    options.SuppressModelStateInvalidFilter = true;               // Bypass automatic model check (difficult to debug)
//    options.SuppressMapClientErrors = true;
//    options.ClientErrorMapping[StatusCodes.Status404NotFound].Link =
//        "https://httpstatuses.com/404";
//})
    ;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowGetPut",
        builder =>
        {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()  //   WithMethods("GET", "PUT") 
            .AllowAnyHeader();
        });
});

// Set up connection string to Ora (defined in appsettings)
builder.Services.AddDbContext<PlaceDbContext>(options => 
{ 
    options.UseOracle(builder.Configuration.GetConnectionString("OracleDb")) //Environment.GetEnvironmentVariable("ASPNETCORE_ConnectionStrings:OracleDb"))
           .ReplaceService<ISqlGenerationHelper, OracleSqlGenerationCasingHelper>(); 
});

// Allow env variables overrides
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles("/api");

app.UseCors("AllowGetPut");
app.UseAuthorization();

app.MapControllers();

app.MapGet("ping", (HttpRequest req) =>
{
    return Results.Ok(new ResponseBody(HttpStatusCode.OK, "Hello World."));
});

app.Run();
