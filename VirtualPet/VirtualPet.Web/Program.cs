using VirtualPet.Web.Exceptions;
using VirtualPet.Web.Extensions;
using VirtualPet.Web.Options;

var builder = WebApplication.CreateBuilder(args);

#region/*--- Add services to the container. ---*/

//MVC
builder.Services.AddControllersWithViews();

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

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Global Exception Handler
app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
