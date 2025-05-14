using Spestqnko.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
new Startup(builder.Configuration).ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
new Startup(builder.Configuration).Configure(app, builder.Environment);

app.Run();
