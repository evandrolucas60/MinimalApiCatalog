using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApiCatalog.ApiEndpoints;
using MinimalApiCatalog.AppServicesExtensions;
using MinimalApiCatalog.Context;
using MinimalApiCatalog.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAuthenticationJwt();

var app = builder.Build();

// ------------------------endpoints LogIn ---------------------------------
app.MapAuthenticationEndpoints();

// ------------------------endpoints Category ---------------------------------
app.MapCategoriesEndpoints();

// ------------------------endpoints Product --------------------------------- //
app.MapProductsEndpoints();

// Configure the HTTP request pipeline.
var environment = app.Environment;

app.UseExceptionHandling(environment)
    .UseSwaggerMiddleware()
    .UseAppCors();

app.UseAuthentication();
app.UseAuthorization();

app.Run();