using Microsoft.EntityFrameworkCore;
using MinimalApiCatalog.Context;
using MinimalApiCatalog.Models;

namespace MinimalApiCatalog.ApiEndpoints
{
    public static class CategoryEndpoints
    {
        public static void MapCategoriesEndpoints(this WebApplication app)
        {
            app.MapPost("/categorias", async (Category category, ApplicationDbContext db) =>
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();

                return Results.Created($"/categorias/{category.CategoryId}", category);
            });

            app.MapGet("/categorias", async (ApplicationDbContext db) => await db.Categories.ToListAsync()).WithTags("Categorias").RequireAuthorization();

            app.MapGet("/categorias{id:int}", async (int id, ApplicationDbContext db) =>
            {
                return await db.Categories.FindAsync(id)
                        is Category category
                        ? Results.Ok(category)
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
        }
    }
}
