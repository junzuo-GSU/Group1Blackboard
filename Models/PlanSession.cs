namespace Group1Blackboard.Models
{
    // wrapper class: keeps the session keys in one place
    public class PlanSession
    {
        private const string PlanKey = "plan";
        private const string CountKey = "plancount";
        private const string MajorKey = "activemajor";
        private const string LocKey = "activeloc";
        private const string SemKey = "activesem";

        private readonly ISession session;
        public PlanSession(ISession session) => this.session = session;

        // the courses the student has added to the plan
        public List<Course> GetPlan() => session.GetObject<List<Course>>(PlanKey) ?? new List<Course>();

        public void SetPlan(List<Course> courses)
        {
            session.SetObject(PlanKey, courses);
            session.SetInt32(CountKey, courses.Count);
        }

        // null means the plan was never loaded into this session
        public int? GetPlanCountOrNull() => session.GetInt32(CountKey);
        public int GetPlanCount() => session.GetInt32(CountKey) ?? 0;

        public void RemovePlan()
        {
            session.Remove(PlanKey);
            session.Remove(CountKey);
        }

        // the catalog filters, retained while the student moves between pages
        public string GetActiveMajor() => session.GetString(MajorKey) ?? "all";
        public string GetActiveLoc() => session.GetString(LocKey) ?? "all";
        public string GetActiveSem() => session.GetString(SemKey) ?? "all";

        public void SetActiveFilters(string major, string loc, string sem)
        {
            session.SetString(MajorKey, major);
            session.SetString(LocKey, loc);
            session.SetString(SemKey, sem);
        }
    }
}
