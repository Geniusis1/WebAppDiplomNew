using WebAppDiplomNew.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AngleSharp;
using AngleSharp.Dom;
using c;

namespace WebAppDiplomNew.Pages.Shared
{
	public class ViewModel : PageModel
	{
		private DB db;
		private bool access;

		public string[] TimeLine { get; private set; }
		public string[] DaysLine { get; private set; }
		public Dictionary<int, Dictionary<string, string>> Info { get; private set; }
		public int Week { get; private set; }
		public DateTime FirstDay { get; private set; }
		public Group Group { get; private set; }

		public ViewModel()
		{
			db = new DB();
			access = false;
			TimeLine = Settings_app.TimesLess;
			DaysLine = Settings_app.DaysLess;
			Week = 0;

			Info = new Dictionary<int, Dictionary<string, string>>();
		}

		public void OnGet(string group, int week)
		{
			//Проверка доступа на изменение
			access = Request.Cookies["onLogin"].Equals("true") || Request.Cookies["group"].Equals(group);
            Group = db.Groups.FirstOrDefault(g => g.Name == group);
			Week = week;

			var task = GetInfo();
			task.Wait();

			//Первый день в неделе
			FirstDay = DateTime.Now.AddDays((int)DateTime.Now.DayOfWeek - 6 + (week * 7));
        }

		private async Task GetInfo()
		{
            //Поиск ссылки на расписание группы
            Url url = new Url(@"http://rasp.pgups.ru/search?title=" + Group.Name + @"&by=group");

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            var doc = await context.OpenAsync(url);
			
			//Ссылка на расписание группы
            var link = doc.GetElementsByClassName("kt-section__content kt-section__content--border")[0].GetElementsByTagName("a")[0].GetAttribute("href");

            //Получение расписания
            url = new Url(link);
			doc = await context.OpenAsync(url);

			int countWeek = 0;
			//Блоки с расписанием
			var list = doc.GetElementsByClassName("table m-table mb-5");
            foreach (var i in list)
            {
				var lessWeek = new Dictionary<string, string>();
				//Блоки с пердметами
				var listWeek = i.GetElementsByTagName("tr");
                foreach (var j in listWeek)
                {
					//Время
					var time = j.GetElementsByClassName("text-center kt-shape-font-color-4")[0].GetElementsByTagName("span")[0].InnerHtml;
					//Имя предмета
					var inf = j.GetElementsByClassName("mr-1")[0].InnerHtml;
					lessWeek.Add(time, inf);
                }
				Info.Add(countWeek, lessWeek);
                countWeek++;
            }
        }

		private void GetNotes()
		{

		}
    }
}
