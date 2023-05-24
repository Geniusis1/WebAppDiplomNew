using WebAppDiplomNew.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AngleSharp;
using AngleSharp.Dom;
using c;

namespace WebAppDiplomNew.Pages.Shared
{
	public class ViewModel : PageModel
	{
		private Group group;
		private DB db;
		private bool access;

		public string[] TimeLine { get; private set; }
		public string[] DaysLine { get; private set; }
		public Dictionary<int, Dictionary<string, string>> Info { get; private set; }

		public ViewModel()
		{
			db = new DB();
			access = false;
			TimeLine = Settings_app.TimesLess;
			DaysLine = Settings_app.DaysLess;

			Info = new Dictionary<int, Dictionary<string, string>>();
		}

		public void OnGet(string group, string week)
		{
			//�������� ������� �� ���������
			access = Request.Cookies["onLogin"].Equals("true") || Request.Cookies["group"].Equals(group);
            this.group = db.Groups.FirstOrDefault(g => g.Name == group);

			var task = GetInfo();
			task.Wait();
        }

		private async Task GetInfo()
		{
            //������ ��� ������ ������
            //http://rasp.pgups.ru/search?title=���-911&by=group

            //����� ������ �� ���������� ������
            Url url = new Url(@"http://rasp.pgups.ru/search?title=" + group.Name + @"&by=group");

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            var doc = await context.OpenAsync(url);
			
			//������ �� ���������� ������
            var link = doc.GetElementsByClassName("kt-section__content kt-section__content--border")[0].GetElementsByTagName("a")[0].GetAttribute("href");

            //��������� ����������
            url = new Url(link);
			doc = await context.OpenAsync(url);

			int countWeek = 0;
			//����� � �����������
			var list = doc.GetElementsByClassName("table m-table mb-5");
            foreach (var i in list)
            {
				var lessWeek = new Dictionary<string, string>();
				//����� � ����������
				var listWeek = i.GetElementsByTagName("tr");
                foreach (var j in listWeek)
                {
					//�����
					var time = j.GetElementsByClassName("text-center kt-shape-font-color-4")[0].GetElementsByTagName("span")[0].InnerHtml;
					//��� ��������
					var inf = j.GetElementsByClassName("mr-1")[0].InnerHtml;
					lessWeek.Add(time, inf);
                }
				Info.Add(countWeek, lessWeek);
                countWeek++;
            }
        }
    }
}
