using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace AuthCourseViews.Services.Auth
{
	public class AuthProviderJWT : AuthenticationStateProvider, ILoginService
	{

		public const string TokenKey = "TOKEN_KEY";

		public readonly IJSRuntime JSRuntime;
		private readonly HttpClient httpClient;
		private readonly NavigationManager NavManager;

		private readonly AuthenticationState AnnonState;

		public AuthProviderJWT(IJSRuntime jsRuntime, HttpClient _httpClient, NavigationManager navManager)
		{
			JSRuntime = jsRuntime;
			httpClient = _httpClient;
			NavManager = navManager;
			AnnonState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
		}

		public async Task LogIn(string token)
		{

			await JSRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);

			var authState = BuildAuthState(token);

			NotifyAuthenticationStateChanged(Task.FromResult(authState));

		}

		public async Task LogOut()
		{

			await JSRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);

			httpClient.DefaultRequestHeaders.Authorization = null;

			NotifyAuthenticationStateChanged(Task.FromResult(AnnonState));

		}

		private AuthenticationState BuildAuthState(string token)
		{

			var claims = ParseClaimsFromJWT(token);

			if (claims != null)
			{

				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

				return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));

			}

			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", null);

			return AnnonState;

		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{

			string token = await JSRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);

			if (token is null)
				return AnnonState;

			return BuildAuthState(token);

		}

		private IEnumerable<Claim> ParseClaimsFromJWT(string token)
		{

			var jwtHandler = new JwtSecurityTokenHandler();

			const string secret = "This is a (Hopefully) strong enough secret to generate a JWT.";

			var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

			TokenValidationParameters validationParameters = new TokenValidationParameters
			{

				ValidateIssuer = true,
				ValidIssuer = "https://localhost:7129",
				ValidateAudience = true,
				ValidAudience = "https://localhost:7035",
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = signingKey,
				ClockSkew = TimeSpan.Zero,
			};

			try
			{

				ClaimsPrincipal claimsCollection = jwtHandler.ValidateToken(token, validationParameters, out var validatedToken);

				return claimsCollection.Claims;

			}
			catch (Exception ex)
			{

				NavManager.NavigateTo("/LogIn");

				return null!;

			}

		}

	}
}
