using CommonInitializer;
using Jwt;
using MediaEncoder.WebAPI.BgServices;
using MediaEncoder.WebAPI.Option;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureDbConfiguration();
builder.ConfigureExtraServices(new InitializerOptions
{
    LogFilePath = "e:/EngLearn.Log/MediaEncoder.log",
    EventBusQueueName = "MediaEncoder.WebAPI"
});
builder.Services.Configure<FileServiceOptions>(builder.Configuration.GetSection("FileService:Endpoint"));
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));
builder.Services.AddHttpClient();
builder.Services.AddHostedService<EncodingBgService>();//后台转码服务
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "MediaEncoder.WebAPI", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MediaEncoder.WebAPI v1"));
}
app.UseDefault();
app.MapControllers();
app.Run();