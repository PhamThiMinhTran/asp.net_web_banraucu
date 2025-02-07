using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Web_banThucPhamSach.Data;
using Web_banThucPhamSach.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WebBanThucPhamSachContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("CHBTPS"));
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "WebBanThucPhamSach";
    options.IdleTimeout = TimeSpan.FromMinutes(3600); // Thay đổi thời gian timeout theo ý bạn
    options.Cookie.HttpOnly = true; // Cài đặt cho cookie
    options.Cookie.IsEssential = true; // Đảm bảo cookie luôn có mặt
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.Cookie.IsEssential = true;
        //options.AccessDeniedPath = "/Account/Denied";
        //options.LoginPath = "/Account/Login";
        //options.LogoutPath = "/Account/Logout";
    });
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("http://localhost:5072/")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseSession();

app.UseStaticFiles();

app.MapHub<ChatHub>("/chathub");

app.UseCors("AllowAll");

app.UseDeveloperExceptionPage();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TrangChu}/{action=Index}/{id?}");

app.Run();
