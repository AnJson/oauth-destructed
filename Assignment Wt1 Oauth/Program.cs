using Assignment_Wt1_Oauth.Contracts;
using Assignment_Wt1_Oauth.Repositories;
using Assignment_Wt1_Oauth.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<IUserService, UserService>()
    .AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllersWithViews();


var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
} // TODO: Add else-block with exceptionhandling middleware.

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();

app.Run();
