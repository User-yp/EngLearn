using CommonInitializer;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.ConfigureDbConfiguration();
builder.ConfigureExtraServices(new InitializerOptions
{
    EventBusQueueName = "OrderService.WebAPI",
    LogFilePath = "e:/EngLearn.Log/OrderService.log"
});
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "OrderService.WebAPI", Version = "v1" });
    //c.AddAuthenticationHeader();
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderService.WebAPI v1"));
}
app.UseDefault();
/*app.UseHttpsRedirection();

app.UseAuthorization();*/


app.MapControllers();

app.Run();
