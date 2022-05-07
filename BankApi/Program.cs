using BankApi.Filters;
using BankApi.Modules;
using BankApi.Validators;
using FluentValidation.AspNetCore;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(c =>
{
    c.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    c.Filters.Add<ExceptionFilter>();
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
})
.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<EventRequestValidator>());

// Add services to the container.
builder.Services.AddMiddlewareModule();
builder.Services.AddLoggingModule();
builder.Services.AddInfrastructureModule();
builder.Services.AddApplicationModule();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddlewaresModule();

app.Run();

public partial class Program { }
