using Microsoft.EntityFrameworkCore;
using MinimalApiCatalog.Context;
using MinimalApiCatalog.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// ------------------------endpoints Category ---------------------------------
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

app.MapDelete("/categoria/{id:int}", async (int id, ApplicationDbContext db) =>
{
    var category = await db.Categories.FindAsync(id);

    if (category is null)
    {
        return Results.NotFound();
    }

    db.Categories.Remove(category);
    await db.SaveChangesAsync();

    return Results.NoContent();
});


// ------------------------endpoints Product ---------------------------------
app.MapPost("/produtos", async (Product product, ApplicationDbContext db)
 => {
     db.Products.Add(product);
await db.SaveChangesAsync();

return Results.Created($"/produtos/{product.ProductId}", product);
 });

app.MapGet("/produtos", async (ApplicationDbContext db) => await db.Products.ToListAsync());

app.MapGet("/produtos/{id:int}", async (int id, ApplicationDbContext db)
    => {
        return await db.Products.FindAsync(id)
                     is Product product
                     ? Results.Ok(product)
                     : Results.NotFound();
    });

app.MapPut("/produtos/{id:int}", async (int id, Product product, ApplicationDbContext db) =>
{

    if (product.ProductId != id)
    {
        return Results.BadRequest();
    }

    var productDB = await db.Products.FindAsync(id);

    if (productDB is null) return Results.NotFound();

    productDB.Name = product.Name;
    productDB.Description = product.Description;
    productDB.Price = product.Price;
    productDB.Image = product.Image;
    productDB.PurchaseDate = product.PurchaseDate;
    productDB.Stock = product.Stock;
    productDB.CategoryId = product.CategoryId;

    await db.SaveChangesAsync();

    return Results.Ok(productDB);
});

app.MapDelete("/produtos/{id:int}", async (int id, ApplicationDbContext db) =>
{
    var product = await db.Products.FindAsync(id);

    if (product is null)
    {
        return Results.NotFound();
    }

    db.Products.Remove(product);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();