using Microsoft.EntityFrameworkCore;
using VirtualPet.Application.Repositories;
using VirtualPet.Application.Services;
using VirtualPet.Domain.Services;
using VirtualPet.Infrastructure.Data;
using VirtualPet.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<VirtualPetDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("VirtualPetDatabase")));
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<PetApplicationService>();
builder.Services.AddSingleton<PetEvolutionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
