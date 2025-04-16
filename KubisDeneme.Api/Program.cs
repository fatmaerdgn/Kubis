using Microsoft.EntityFrameworkCore;
using KubisDeneme.DAL.Data;
using Microsoft.OpenApi.Models;
using KubisDeneme.Service;
using FluentValidation.AspNetCore;
using KubisDeneme.Service.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


//POSTGRESQL BAÐLANTISI
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//KitapService ve IKitapService
builder.Services.AddScoped<IKitapService, KitapService>();

//YazarService ve IYazarService
builder.Services.AddScoped<IYazarService, YazarService>();

//KitapTuruService ve IKitapTuruService
builder.Services.AddScoped<IKitapTuruService, KitapTuruService>();

//UlkeService ve IUlkeService
builder.Services.AddScoped<IUlkeService, UlkeService>();

//SWAGGER SERVÝSÝ
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

//CQRS SERVÝSÝ
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Fluent Validation'ý servis olarak ekleyelim
builder.Services.AddControllers().AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<KitapValidator>();
});


builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Geliþtirme ortamýnda Swagger'ý etkinleþtirin
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

//CQRS Sorunu Ýçin
app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
