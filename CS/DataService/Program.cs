using DataService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Xml.Serialization;

var builder = WebApplication.CreateBuilder(args);

var publicKey = await GetKeycloakPublicKey(builder.Configuration["Jwt:KeycloakUrl"]!, builder.Configuration["Jwt:Realm"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Development only!!

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"{builder.Configuration["Jwt:KeycloakUrl"]}/realms/{builder.Configuration["Jwt:Realm"]}",
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = publicKey
        };
    });
builder.Services.AddAuthorization();

string? connectionString = builder.Configuration.GetConnectionString("ConnectionString");

builder.Services.AddDbContext<DataServiceDbContext>(o =>
  o.UseSqlServer(connectionString, options =>
  {
      options.EnableRetryOnFailure();
  }));

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

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
}).RequireAuthorization();

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
}).RequireAuthorization();

app.MapGet("/data/OrderItem/{id}", async (DataServiceDbContext dbContext, int id) =>
{
    var orderItem = await dbContext.OrderItems.FindAsync(id);

    if (orderItem is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(orderItem);
}).RequireAuthorization();

app.MapPost("/data/OrderItem", async (DataServiceDbContext dbContext, OrderItem orderItem) =>
{
    dbContext.OrderItems.Add(orderItem);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/data/OrderItem/{orderItem.Id}", orderItem);
}).RequireAuthorization();

app.MapPut("/data/OrderItem/{id}", async (DataServiceDbContext dbContext, int id, OrderItem orderItem) =>
{
    if (id != orderItem.Id)
    {
        return Results.BadRequest("Id mismatch");
    }

    dbContext.Entry(orderItem).State = EntityState.Modified;
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization();

app.MapDelete("/data/OrderItem/{id}", async (DataServiceDbContext dbContext, int id) =>
{
    var orderItem = await dbContext.OrderItems.FindAsync(id);

    if (orderItem is null)
    {
        return Results.NotFound();
    }

    dbContext.OrderItems.Remove(orderItem);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization();

app.Run();



static async Task<SecurityKey> GetKeycloakPublicKey(string keycloakUrl, string realm)
{
    using (var httpClient = new HttpClient())
    {
        var jwksUrl = $"{keycloakUrl}/realms/{realm}/protocol/openid-connect/certs";
        var jwksJson = await httpClient.GetStringAsync(jwksUrl);
        var jwks = new JsonWebKeySet(jwksJson);
        return jwks.Keys[0];
    }
}
