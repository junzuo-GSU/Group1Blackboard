using System.Text.Json;

namespace Group1Blackboard.Models
{
    // lets a session store objects: they are saved as JSON strings
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value) =>
            session.SetString(key, JsonSerializer.Serialize(value));

        public static T? GetObject<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            return json == null ? default : JsonSerializer.Deserialize<T>(json);
        }
    }
}
