using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using VirtualPet.Infrastructure.Data;
using VirtualPet.Web.Exceptions;
using VirtualPet.Web.Extensions;
using VirtualPet.Web.Options;

var builder = WebApplication.CreateBuilder(args);

#region/*--- Add services to the container. ---*/

//MVC
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

//OpenAPI
builder.Services.AddOpenApi();

//Application
builder.Services.AddVirtualPetApplication();

//Infrastructure
builder.Services.AddVirtualPetInfrastructure(builder.Configuration);

//Identity
builder.Services.AddVirtualPetIdentity();

//Configuration
builder.Services.AddOptions<PetGameOptions>().Bind(builder.Configuration.GetSection(PetGameOptions.SectionName))
    .Validate(options => options.TrainingExperience>0, "PetGame:TrainingExperience must be greater than 0.").ValidateOnStart();

//Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

//Health Check
builder.Services.AddHealthChecks();

#endregion

var app = builder.Build();

//Auto Migration
if (app.Configuration.GetValue<bool>("Database:ApplyMigrations"))
{
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<VirtualPetDbContext>();

    await dbContext.Database.MigrateAsync();
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//Global Exception Handler
app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapHealthChecks("/health");

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

public partial class Program { }