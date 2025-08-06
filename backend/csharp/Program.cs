using csharp.Services;
using Materialise.Candidate.Backend;
using Microsoft.AspNetCore.Authentication;
using csharp.Authentication;
using csharp.Interface;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, new PlainTextInputFormatter());
});
builder.Services.AddSingleton<IItemService, ItemService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication("FakeAuth")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("FakeAuth", null);

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
await app.RunAsync();
