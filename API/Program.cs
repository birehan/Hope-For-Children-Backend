using Persistence;
using Application;
using API.Extensions;
using Microsoft.AspNetCore.Identity;
using Domain;
using Microsoft.EntityFrameworkCore;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.ConfigurePersistenceServices(builder.Configuration);
builder.Services.ConfigureApplicationServices();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddFluentEmail(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
   {
       endpoints.MapControllers();
   });

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();

    await context.Database.MigrateAsync();
    // await Seed.SeedData(context, );
    await Seed.SeedData(context, userManager, builder.Configuration);

}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();

}

app.Run();