using System.Text.Json;
using System.Text.Json.Serialization;
using ElectronicBidding.Application;
using ElectronicBidding.Extensions;
using ElectronicBidding.Infastructure;
using ElectronicBidding.Middleware;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services
    .AddPresentation(builder.Configuration)
    .AddApplication(builder.Configuration)
    .AddInfrascture();

builder.Host.UseSerilog((context, services, configuration) =>
    configuration
    .ReadFrom.
    Configuration(context.Configuration));

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication();   
builder.Services.AddAuthorization(); 
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // ✅ Convert enums to strings (camelCase)
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });



var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.MapControllers();

app.Run();

