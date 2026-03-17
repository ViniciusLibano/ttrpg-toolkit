using TTRPG.Toolkit.Domain.Entities.Identity;
using TTRPG.Toolkit.Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext();
builder.Services.AddIdentityModule();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapIdentityApi<ApplicationUser>();

app.MapGet("/api/protegido", () => "Teste")
    .RequireAuthorization();

app.Run();