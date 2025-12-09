using System.ComponentModel.DataAnnotations;

namespace eShopModel.Classes
{
	public class LoginDTO
	{

		[StringLength(80)]
		[Required]
		public string Email { get; set; } = null!;

		[Required]
		public string Contraseña { get; set; } = null!;

	}
}
