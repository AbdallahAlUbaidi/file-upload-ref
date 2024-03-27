using FileUpload.Contracts;
using FileUpload.Storage;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IStorage, LocalStorage>();

builder.Services.AddHttpLogging(options =>
{

    options.LoggingFields = HttpLoggingFields.All;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();

app.MapControllers();

app.UseHttpsRedirection();


app.Run();
