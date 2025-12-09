using AuthAPI.Data;
using AuthAPI.Helpers;
using eShopModel.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace AuthAPI.Controllers
{

	[ApiController]
	[Route("Auth")]
	[AllowAnonymous]
	public class AuthController : ControllerBase
	{

		private readonly EShopContext Context;
		private readonly UserManager<Usuario> userManager;
		private readonly SignInManager<Usuario> signInManager;

		public AuthController(EShopContext context, UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
		{
			Context = context;
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		[HttpPost("SignUp")]
		public async Task<ActionResult> SignUp(RegisterDTO data)
		{

			try
			{

				PasswordHelper passwordHelper = new PasswordHelper();

				Usuario user = new Usuario
				{

					Email = data.Email,
					UserName = data.Email,
					PhoneNumber = data.Telefono

				};

				user.PasswordHash = passwordHelper.Hash(data.Contraseña, passwordHelper.GenerateSalt(out string salt));
				user.Salt = salt;

				var result = await userManager.CreateAsync(user, user.PasswordHash);

				if (!result.Succeeded)
					return BadRequest();

				result = await userManager.AddToRoleAsync(user, "supervisor");

				if (!result.Succeeded)
					return BadRequest();

				return Ok();

			}
			catch (Exception ex)
			{

				return StatusCode(500, ex.Message);

			}

		}

		[HttpPost("LogIn")]
		public async Task<ActionResult> LogIn(LoginDTO data)
		{

			try
			{

				var user = await userManager.FindByNameAsync(data.Email);

				if (user == null)
					return BadRequest();

				var passwordHelper = new PasswordHelper();

				string hashedPassword = passwordHelper.Hash(data.Contraseña, Convert.FromBase64String(user.Salt));

				var result = await signInManager.PasswordSignInAsync(user , hashedPassword, false, false);

				if (!result.Succeeded)
					return BadRequest();

				return Ok(await CreateJWT(user));

			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

		}

		public async Task<TokenDTO> CreateJWT(Usuario user)
		{

			var roles = await userManager.GetRolesAsync(user);

			var claimsIdentity = new ClaimsIdentity();

			claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, roles.FirstOrDefault()!));
			claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));

			const string secret = "This is a (Hopefully) strong enough secret to generate a JWT.";

			var singingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

			var signingCredentials = new SigningCredentials(singingKey, SecurityAlgorithms.HmacSha256);

			var expiration = DateTime.UtcNow.AddDays(30);

			var tokenDescriptor = new SecurityTokenDescriptor()
			{
				SigningCredentials = signingCredentials,
				Issuer = "https://localhost:7129", //URL de la API.
				Audience = "https://localhost:7035", //URL de la aplicación Blazor.
				Expires = expiration,
				NotBefore = DateTime.UtcNow,
				Subject = claimsIdentity,
			};
			
			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

			return new TokenDTO
			{
				Token = tokenHandler.WriteToken(token),
				ExpirationDate = expiration
			};

		}

	}

}
