using AppointmentData;
using AppointmentData.DataAccessLayer;
using AppointmentData.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ServiceDAL>();
builder.Services.AddScoped<ServiceProviderDAL>();
builder.Services.AddScoped<CustomerDAL>();
builder.Services.AddScoped<SlotDAL>();
builder.Services.AddScoped<AppointmentDAL>();
builder.Services.AddScoped<AccountDAL>();

builder.Services.AddTransient<IServiceProviderDAL, ServiceProviderDAL>();
builder.Services.AddTransient<IServiceDAL, ServiceDAL>();
builder.Services.AddTransient<ICustomerDAL, CustomerDAL>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});
builder.Logging.AddConsole();

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
app.UseAuthentication();
app.UseAuthorization();
app.Use(async (context, next) =>
{
    var user = context.User;
    var requestPath = context.Request.Path;


    if (user?.Identity?.IsAuthenticated != true && !requestPath.StartsWithSegments("/Account/Login"))
    {
        context.Response.Redirect("/Account/Login");
        return;
    }

    await next();
});


app.MapControllerRoute(
    name: "default",
//pattern: "{controller=Service}/{action=GetService}/{id?}");
pattern: "{controller=Home}/{action=Index}/{id?}");
//pattern: "{controller=ServiceProvider}/{action=GetServiceProviders}/{id?}");


app.Run();
