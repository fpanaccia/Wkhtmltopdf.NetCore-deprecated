using Microsoft.CodeAnalysis;
using Wkhtmltopdf.NetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WebHost.UseKestrel(kestrel =>
{
    kestrel.ListenAnyIP(80);
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddWkhtmltopdf("./Binaries");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
