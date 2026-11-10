namespace Group1Blackboard.Models
{
    // wrapper class: the plan's course ids live in a cookie for two weeks
    public class BlackboardCookies
    {
        private const string PlanKey = "plancourseids";
        private const string Delimiter = "-";

        private readonly IRequestCookieCollection? requestCookies;
        private readonly IResponseCookies? responseCookies;

        public BlackboardCookies(IRequestCookieCollection cookies) => requestCookies = cookies;
        public BlackboardCookies(IResponseCookies cookies) => responseCookies = cookies;

        public List<int> GetPlanIds()
        {
            string? ids = requestCookies?[PlanKey];
            if (string.IsNullOrEmpty(ids)) return new List<int>();

            // ignore anything in the cookie that is not a number
            return ids.Split(Delimiter)
                .Where(i => int.TryParse(i, out _))
                .Select(int.Parse)
                .ToList();
        }

        public void SetPlanIds(List<Course> courses)
        {
            if (responseCookies == null) return;

            // replace the old cookie; an empty plan simply removes it
            responseCookies.Delete(PlanKey);
            if (courses.Count == 0) return;

            string ids = string.Join(Delimiter, courses.Select(c => c.CourseID));
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(14),
                IsEssential = true
            };
            responseCookies.Append(PlanKey, ids, options);
        }
    }
}
