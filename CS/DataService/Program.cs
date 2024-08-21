using DataService;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Xml.Serialization;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("ConnectionString");

builder.Services.AddDbContext<DataServiceDbContext>(o =>
  o.UseSqlServer(connectionString, options =>
  {
      options.EnableRetryOnFailure();
  }));

var app = builder.Build();

// Make sure the database exists and is current
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataServiceDbContext>();
    dbContext.Database.Migrate();
}

app.MapGet("/api/populateTestData", async (DataServiceDbContext dbContext) =>
{
    var assembly = Assembly.GetExecutingAssembly();
    Console.WriteLine(String.Join("\n", assembly.GetManifestResourceNames()));
    var resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("order_items.xml"));

    var serializer = new XmlSerializer(typeof(List<OrderItem>));

    using Stream? stream = assembly.GetManifestResourceStream(resourceName);
    if (stream is not null)
    {
        var items = (List<OrderItem>?)serializer.Deserialize(stream);

        if (items is not null)
        {
            dbContext.OrderItems.AddRange(items);
            await dbContext.SaveChangesAsync();
            return Results.Ok("Data populated successfully");
        }
    }

    return Results.NotFound("Error populating data");
});

app.MapGet("/data/OrderItems", async (DataServiceDbContext dbContext, int skip = 0, int take = 20, string sortField = "Id", bool sortAscending = true) =>
{
    var source = dbContext.OrderItems.AsQueryable().OrderBy(sortField + (sortAscending ? " ascending" : " descending"));
    var items = await source.Skip(skip).Take(take).ToListAsync();

    var totalCount = await dbContext.OrderItems.CountAsync();

    return Results.Ok(new
    {
        Items = items,
        TotalCount = totalCount
    });
});

app.Run();

