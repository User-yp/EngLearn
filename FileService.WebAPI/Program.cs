using CommonInitializer;
using FileService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureDbConfiguration();
builder.ConfigureExtraServices(new InitializerOptions
{
    LogFilePath = "E:/EngLearn.Log/FileService.log",
    EventBusQueueName = "FileService.WebAPI",
});

// Add services to the container.
builder.Services//.AddOptions() //asp.net core��Ŀ��AddOptions()��дҲ�У���Ϊ���һ���Զ�ִ����
    .Configure<SMBStorageOptions>(builder.Configuration.GetSection("FileService:SMB"))
    .Configure<UpYunStorageOptions>(builder.Configuration.GetSection("FileService:UpYun"));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FileService.WebAPI", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FileService.WebAPI v1"));
}
app.UseStaticFiles();
app.UseDefault();

app.MapControllers();

app.Run();
