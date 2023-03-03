using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
{
    // Services
    builder.Services
        .AddSingleton<HttpClient>()
        .AddScoped<IUserService, UserService>()
        .AddScoped<IAuthService, AuthService>()
        .AddScoped<IErrorService, ErrorService>();

    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/";
            options.AccessDeniedPath = "/denied";
            options.Cookie.Name = "wt1_1dv027";
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Events = new CookieAuthenticationEvents()
            {
                OnRedirectToLogin = (ctx) =>
                {
                    ctx.Response.StatusCode = 403;
                    return Task.CompletedTask;
                }
            };
        });

    builder.Services.AddAuthorization();

    builder.Services.AddSession(options =>
    {
        options.Cookie.Name = "wt1_1dv027";
    });

    builder.Services.AddControllersWithViews();
}

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
} else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseStaticFiles();
app.MapControllers();

app.Run();
