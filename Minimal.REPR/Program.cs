using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Minimal.REPR;
using Minimal.REPR.Extensions;
using Minimal.REPR.Models;
using Scalar.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ProductsDb"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Register FluentValidation validators
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

var app = builder.Build();

// --- OPTIONAL: Seed default data on startup ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); // Ensure the in-memory DB is ready

    // If you want to add a default product when starting the API:
    if (!db.Products.Any())
    {
        db.Products.Add(new Product
        {
            Id = Guid.NewGuid(),
            Name = "LG Ultrawide Monitor",
            Sku = "MON-9999",
            Price = 350.00m,
            Stock = 10,
            CreatedAtUtc = DateTime.UtcNow
        });
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapEndpoints();
app.Run();
