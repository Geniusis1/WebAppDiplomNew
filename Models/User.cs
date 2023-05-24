using System.Text;
using System.Security.Cryptography;

namespace WebAppDiplomNew.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public Group Group { get; set; }

		public User(){}

		public User(string username, string password, Group group)
		{
			Username = username;
			Group = group;

			Password = PassToHashString(password);
		}

		//Хеширование пароля
		public static string PassToHashString(string pass)
		{
			byte[] tmpSource = Encoding.ASCII.GetBytes(pass);
			byte[] byteArr = new MD5CryptoServiceProvider().ComputeHash(tmpSource);

			int i;
			StringBuilder sOutput = new StringBuilder(byteArr.Length);
			for (i = 0; i < byteArr.Length; i++)
			{
				sOutput.Append(byteArr[i].ToString("X2"));
			}
			return sOutput.ToString();
		}

		//Сравнение Хешей
		public bool ComparisonHash(string hash)
		{
			return this.Password == hash;
		}
	}
}
