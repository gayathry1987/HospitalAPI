using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HospitalWebApplication.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HospitalWebApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HospitalWebApplicationContext") ?? throw new InvalidOperationException("Connection string 'HospitalWebApplicationContext' not found.")));
//builder.Services.AddDbContext<patientContext>(opt => opt.("patientList"));
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=patients}/{action=Index}/{id?}");

app.Run();
