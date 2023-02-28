using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<IUserService, UserService>()
    .AddScoped<IAuthService, AuthService>()
    .AddScoped<IErrorService, ErrorService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();

// TODO: Add authentication.

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
app.UseCors();
app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}"); //NOTE: Not working???
app.UseStaticFiles();
app.MapControllers();

app.Run();
