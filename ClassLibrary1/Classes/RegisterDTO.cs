using System.ComponentModel.DataAnnotations;

namespace eShopModel.Classes
{
	public class RegisterDTO
	{

		[Required]
		public string Email { get; set; } = null!;

		[Required]
		public string Telefono { get; set; } = null!;

		[Required]
		public string Contraseña { get; set; } = null!;

	}

}
