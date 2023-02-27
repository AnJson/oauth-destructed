using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Repositories;
using Assignment_Wt1_Oauth.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<IUserService, UserService>()
    .AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie")
    .AddOAuth("gitlab", options =>
    {
        options.SignInScheme = "cookie";
        options.ClientId = app.Configuration.GetValue<string>("Oauthconfig:AppId");
        options.ClientSecret = app.Configuration.GetValue<string>("Oauthconfig:AppSecret");
        options.SaveTokens = false;

        options.AuthorizationEndpoint = app.Configuration.GetValue<string>("Oauthconfig:AuthorizationUri");
        options.TokenEndpoint = app.Configuration.GetValue<string>("Oauthconfig:TokenUri");
        options.CallbackPath = app.Configuration.GetValue<string>("Oauthconfig:RedirectUri");
    });

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
} // TODO: Add else-block with exceptionhandling middleware.

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();

app.Run();
