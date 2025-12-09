using System.Security.Cryptography;
using System.Text;

namespace AuthAPI.Helpers
{
	public class PasswordHelper
	{

		const int KeySize = 64;
		const int IterationCount = 600000;

		public byte[] GenerateSalt(out string saltString)
		{

			using (var rng = RandomNumberGenerator.Create())
			{

				byte[] salt = new byte[16];

				rng.GetBytes(salt);

				saltString = Convert.ToBase64String(salt);

				return salt;

			}

		}

		public string Hash(string sourceData, byte[] salt)
		{

			byte[] sourceBytes = Encoding.UTF8.GetBytes(sourceData);
			byte[] hashedBytes = Rfc2898DeriveBytes.Pbkdf2(sourceBytes, salt, IterationCount, HashAlgorithmName.SHA256, KeySize);

			return Convert.ToBase64String(hashedBytes);

		}


	}
}
