using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Services;
using Assignment_Wt1_Oauth.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
{
    // Scope to handle services, setting up services in the IoC-container and adding authentication.
    builder.Services
        .AddSingleton<HttpClient>()
        .AddScoped<IUserService, UserService>()
        .AddScoped<IAuthService, AuthService>()
        .AddScoped<IErrorService, ErrorService>()
        .AddScoped<IJwtHandler, JwtHandler>()
        .AddScoped<ISessionHandler, SessionHandler>()
        .AddScoped<IRequestHandler, RequestHandler>();

    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/";
            options.AccessDeniedPath = "/denied";
            options.ExpireTimeSpan = TimeSpan.FromDays(7);
            options.SlidingExpiration = true;
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
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    builder.Services.AddControllersWithViews();
}

var app = builder.Build();

// Middlewares to run in the request-pipeline.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
} else
{
    app.UseExceptionHandler("/Error");
    // app.UseHttpsRedirection();
}

app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseStaticFiles();
app.MapControllers();

app.Run();
