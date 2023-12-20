using Microsoft.EntityFrameworkCore;
using MinimalApiCatalog.Context;
using MinimalApiCatalog.Models;

namespace MinimalApiCatalog.ApiEndpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductsEndpoints(this WebApplication app)
        {
            app.MapPost("/produtos", async (Product product, ApplicationDbContext db)
 => {
     db.Products.Add(product);
     await db.SaveChangesAsync();

     return Results.Created($"/produtos/{product.ProductId}", product);
 });

            app.MapGet("/produtos", async (ApplicationDbContext db) => await db.Products.ToListAsync()).WithTags("Produtos").RequireAuthorization();

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
        }
    }
}
