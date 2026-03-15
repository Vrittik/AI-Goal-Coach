var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.ConfigureAIClientServices(builder.Configuration);
builder.Services.AddScoped<ClientConfigProvider>();
builder.Services.AddScoped<IGoalDomainLogic, GoalDomainLogic>();
builder.Services.AddSingleton<IGoalRepository, GoalRepository>();
builder.Services.AddScoped<IAIClient, GeminiAIClient>();
builder.Services.AddSingleton<IHttpClientService, HttpClientService>();
builder.Services.AddSingleton<IAITelemetryService, AITelemetryService>();
builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

        if (exceptionFeature != null)
        {
            var error = new
            {
                message = exceptionFeature.Error.Message
            };

            await context.Response.WriteAsJsonAsync(error);
        }
    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("ReactPolicy");
app.Run();
