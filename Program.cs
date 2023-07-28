using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Habilitar al celu conectarse con la API
builder.WebHost.UseUrls("http://localhost:5200", "http://*:5200");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath        = "/Usuarios/Login";
		options.LogoutPath       = "/Usuarios/Logout";
		options.AccessDeniedPath = "/Home/Restringido";
	});
	// .AddJwtBearer(options =>
	// {
	// 	options.TokenValidationParameters = new TokenValidationParameters
	// 	{
	// 		ValidateIssuer = true,
	// 		ValidateAudience = true,
	// 		ValidateLifetime = true,
	// 		ValidateIssuerSigningKey = true,
	// 		ValidIssuer = builder.Configuration["Jwt:Issuer"],
	// 		ValidAudience = builder.Configuration["Jwt:Audience"],
	// 		IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
	// 	};
	// });

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador") );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
