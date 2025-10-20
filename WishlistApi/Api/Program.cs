using Application.Interfaces;
using CoreLib.HttpLogic;
using CoreLib.TraceIdLogic;
using Infrastructure;
using Infrastructure.HttpClient;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpRequestService();
builder.Services.TryAddTraceId();
builder.Services.AddScoped<IUserApiClient, UserApiClient>();

//регистрация зависимостей
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogic();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();