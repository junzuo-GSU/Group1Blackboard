namespace Group1Blackboard.Models
{
    // carries the data from the Catalog controller to its views
    public class CatalogViewModel
    {
        // filters, bound from the route; "all" means no filter
        public string ActiveMajor { get; set; } = "all";
        public string ActiveLoc { get; set; } = "all";
        public string ActiveSem { get; set; } = "all";

        // data for the result panel / detail page
        public List<Course> Courses { get; set; } = new();
        public Course Course { get; set; } = new();
        public bool InPlan { get; set; }
    }
}
