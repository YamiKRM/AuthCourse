using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopModel.Classes
{
	public class Usuario : IdentityUser
	{
		public string Salt { get; set; } = null!;

	}
}
