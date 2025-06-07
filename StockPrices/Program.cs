using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StockPrices.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
string connectionString = "Server=localhost\\SQLEXPRESS;Database=StockPricesDb;Trusted_Connection=True;TrustServerCertificate=True;";

using (SqlConnection conn = new SqlConnection(connectionString))
{
    try
    {
        conn.Open();
        Console.WriteLine("Po³¹czenie udane!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("B³¹d po³¹czenia: " + ex.Message);
    }
}
builder.Services.AddDbContext<StockPricesDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
