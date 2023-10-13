using Microsoft.EntityFrameworkCore;
using SmartShopAPI;
using SmartShopAPI.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SmartShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SmartShopDbConnection")));
// Add services to the container.
builder.Services.AddScoped<SmartShopSeeder>();
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(SmartShopMappingProfile));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<SmartShopSeeder>();
    seeder.Seed();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
