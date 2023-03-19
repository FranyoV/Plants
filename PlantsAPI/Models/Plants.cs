namespace PlantsAPI.Models
{
    public class Plants
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Care {get; set; }
        public string Description { get; set; } 
        public string ImageUrl { get; set; }  


        public virtual User User { get; set; }
    }
}
