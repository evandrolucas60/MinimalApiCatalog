using Microsoft.EntityFrameworkCore;
using MinimalApiCatalog.Context;
using MinimalApiCatalog.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

//definir os endpoints
app.MapPost("/categorias", async(Category category, ApplicationDbContext db) =>
{
    db.Categories.Add(category);
    await db.SaveChangesAsync();

    return Results.Created($"/categorias/{category.CategoryId}", category);
});

app.MapGet("/categorias", async (ApplicationDbContext db) => await db.Categories.ToListAsync());

app.MapGet("/categorias{id:int}", async (int id ,ApplicationDbContext db) =>
{
    return await db.Categories.FindAsync(id)
            is Category category
            ?Results.Ok(category)
            : Results.NotFound();
});

app.MapPut("/categoria/{id:int}", async (int id, Category category, ApplicationDbContext db) =>
{
    if (category.CategoryId != id) return Results.BadRequest();

    var categoryDb = await db.Categories.FindAsync(id);

    if (categoryDb is null) return Results.NotFound();

    categoryDb.Name = category.Name;
    categoryDb.Description = category.Description;

    await db.SaveChangesAsync();
    return Results.Ok(categoryDb);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();