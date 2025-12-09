using AuthAPI.Data;
using eShopModel.Classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<EShopContext>();

builder.Services.AddIdentity<Usuario, IdentityRole>(options =>
{

	options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;

	options.User.RequireUniqueEmail = true;

	options.SignIn.RequireConfirmedAccount = false;
	options.SignIn.RequireConfirmedEmail = false;
	options.SignIn.RequireConfirmedPhoneNumber = false;

	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true; 
	options.Password.RequireNonAlphanumeric = false; 

	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.AllowedForNewUsers = true;


}).AddEntityFrameworkStores<EShopContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<DbSeeder>();

const string secret = "This is a (Hopefully) strong enough secret to generate a JWT.";
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{

	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = signingKey,
		ValidateAudience = true,
		ValidAudience = "https://localhost:7035",
		ValidateIssuer = true,
		ValidIssuer = "https://localhost:7129",
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero

	};

});

string corsPolicyName = "Política de CORS";

//Configuramos una política de CORS para aceptar el frontend como un origen de peticiones.
builder.Services.AddCors(options =>
{

	options.AddPolicy(name: corsPolicyName, policy =>
	{

		policy.WithOrigins("https://localhost:7035");
		policy.AllowAnyHeader();

	});

});

var app = builder.Build();

SeedDatabase();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(corsPolicyName);

app.MapControllers();

app.Run();

void SeedDatabase()
{

	IServiceScopeFactory? scopeFactory = app.Services.GetService<IServiceScopeFactory>();

	using (IServiceScope scope = scopeFactory!.CreateScope())
	{

		DbSeeder? seeder = scope.ServiceProvider.GetService<DbSeeder>();

		seeder!.SeedAsync().Wait();

	}

}
