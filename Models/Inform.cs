namespace DiplomWeb_Service.Models
{
    public class Inform
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string Name_lesson { get; set; }
        public Group Group { get; set; }
        public string? Text { get; set; }
    }
}
