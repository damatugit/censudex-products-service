using TallerDos.ProductsService.src.Data;
using TallerDos.ProductsService.src.Repositories;
using TallerDos.ProductsService.src.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<MongoDBContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddSingleton<CloudinaryService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
