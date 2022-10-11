using Search.Repositories;
using Search.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHotelServices(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();

const string CorsPolicyName = "AllowAll";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName,
        builder =>
        {
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.AllowAnyOrigin();
        });
});

var app = builder.Build();

#if DEBUG
// Avoid running EF migrations in upper environments
await app.Services.ApplyDatabaseMigrationsAsync();
#endif

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(options =>
{
    options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
});

app.UseCors(CorsPolicyName);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");


app.MapFallbackToFile("index.html");

app.Run();
