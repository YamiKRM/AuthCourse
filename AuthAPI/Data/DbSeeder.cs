using AuthAPI.Helpers;
using eShopModel.Classes;
using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Data
{
	public class DbSeeder
	{

		private readonly EShopContext Context;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly UserManager<Usuario> userManager;

		public DbSeeder(EShopContext context, RoleManager<IdentityRole> roleManager, UserManager<Usuario> userManager)
		{
			Context = context;
			this.roleManager = roleManager;
			this.userManager = userManager;
		}

		public async Task SeedAsync()
		{

			if (Context.Roles.Count() == 0)
				await AddRoles();

			if (Context.Users.Count() == 0)
				await AddAdminUser();

		}

		public async Task AddRoles()
		{

			IdentityRole adminRole = new IdentityRole { Name = "admin" };
			IdentityRole supervisorRole = new IdentityRole { Name = "supervisor" };

			await roleManager.CreateAsync(adminRole);
			await roleManager.CreateAsync(supervisorRole);

		}

		public async Task AddAdminUser()
		{

			PasswordHelper passwordHelper = new PasswordHelper();

			Usuario user = new Usuario
			{

				Email = "superadmin@eShop.com",
				UserName = "superadmin@eShop.com",
				PhoneNumber = "123123123"

			};

			string password = "SuperSecretPassword";

			user.PasswordHash = passwordHelper.Hash(password, passwordHelper.GenerateSalt(out string salt));
			user.Salt = salt;

			await userManager.CreateAsync(user, user.PasswordHash);

			await userManager.AddToRoleAsync(user, "admin");

		}

	}
}
