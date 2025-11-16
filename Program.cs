using TallerDos.ProductsService.src.Data;
using TallerDos.ProductsService.src.Repositories;
using TallerDos.ProductsService.src.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core; 

var builder = WebApplication.CreateBuilder(args);

// Configuración de MongoDB======================================
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<MongoDBContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Servicio de CLoudinary========================================
builder.Services.AddSingleton<CloudinaryService>();

// GRPC==========================================================
builder.Services.AddGrpc();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración Puertos=========================================
builder.WebHost.ConfigureKestrel(options =>
{
    // Puerto HTTP/HTTPS normal (para REST API y Swagger)
    options.ListenAnyIP(3003, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
    
    // Puerto gRPC (solo HTTP/2)
    options.ListenAnyIP(50052, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();
app.MapGrpcService<ProductsGrpcService>();
app.Run();
