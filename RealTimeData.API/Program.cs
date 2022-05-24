using Confluent.Kafka;
using RealTimeData.API.BackgroundServices;
using RealTimeData.API.Hubs;
using RealTimeData.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(opt =>
    {
        opt.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost" || new Uri(origin).Host == "127.0.0.1")
           .AllowAnyMethod()
           .AllowCredentials()
           .AllowAnyHeader();
    });
});

//Data consumption services
builder.Services.AddSingleton<IKafkaConsumer<Ignore,string>, KafkaConsumer>();
builder.Services.AddHostedService<ConsumeDataBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.MapHub<DataHub>("socket/data");

app.Run();

