using AngleSharp;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppDiplomNew.Models;

namespace WebAppDiplomNew.Pages
{
	[IgnoreAntiforgeryToken]
	public class IndexModel : PageModel
	{
		public string? _MessageLog { get; private set; } = null;
		public string? _MessageGroup { get; private set; } = null;

		private readonly DB db;

		public IndexModel()
		{
			db = new DB();
        }

		public void OnPost(string login, string pass)
		{
            if (Login(login, pass))
			{
				var group = FindGroup(login);

                Response.Cookies.Append("onLogin", "true");
				Response.Cookies.Append("Group", group);

                _ = ParseGroupsAsync();

				Response.Redirect(@"/view?group=" + group);
            }
			else
			{
				_MessageLog = "Ошибка входа";
			}
		}

		public void OnGet(string group)
		{
			if (group == null)
			{
				_MessageGroup = null;
				return;
			}
			if(db.Groups.FirstOrDefault(g => g.Name == group) != null)
			{
                Response.Cookies.Append("onLogin", "false");
                Response.Cookies.Append("Group", group);

				Response.Redirect(@"/view?group=" + System.Net.WebUtility.UrlEncode(group));
            }
			else
			{
				_MessageGroup = "Группа не найдена";
			}
		}

		private bool Login(string username, string password)
		{
			var _OK = false;

            //--Проверка на заполенине всех полей
            if (username == null || password == null)
            {
                return _OK;
            }

			var passHash = Models.User.PassToHashString(password);

			//--Поиск пользователя по логину
			var users = db.Users.First(u => u.Username == username);

			if (users == null)
			{
				return _OK;
			}

			//--Сравнение пароля
			if (!users.ComparisonHash(passHash))
			{
				return _OK;
			}

			_OK = true;
			return _OK;
		}

		private string FindGroup(string username)
		{
			return db.Users.FirstOrDefault(u =>u.Username == username).Group.Name;
		}

		private async Task ParseGroupsAsync()
		{
			var groups = new List<Group>();
            var url = new Url(@"http://rasp.pgups.ru/schedule/group");

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            var doc = await context.OpenAsync(url);

#if DEBUG
			Console.WriteLine("DATABASE CONNECTION:\t " + db.Database.CanConnect());
#endif

			var list = doc.GetElementsByClassName("tab-content")[0].GetElementsByClassName("btn btn-sm btn-secondary btn-pill mr-1 mb-2");
			foreach ( var item in list)
			{
				var f = db.Groups.FirstOrDefault(i => i.Name == item.InnerHtml);
				if (f == null)
				{
                    var group = new Group(item.InnerHtml);
                    db.Add(group);
				}
			}
			db.SaveChanges();

#if DEBUG
			Console.WriteLine("DATABASE:\t Update");
#endif
		}
	}
}