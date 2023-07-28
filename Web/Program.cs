using Microsoft.EntityFrameworkCore;
using Model.JobSeekerDb;
using Service.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddNacosV2Configuration(builder.Configuration.GetSection("NacosConfig"));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IJobSeekerService, JobSeekerService>();


builder.Services.AddDbContext<postgresContext>(option =>
           option.UseNpgsql(builder.Configuration.GetConnectionString("NpgsqlConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
