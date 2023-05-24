namespace WebAppDiplomNew.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Group() { }
        public Group(string name)
        {
            Name = name;  
        }
    }
}
