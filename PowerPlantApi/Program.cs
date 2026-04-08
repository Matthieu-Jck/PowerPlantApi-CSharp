using PowerPlantApi.Exceptions;
using PowerPlantApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddScoped<ProductionPlanService>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
