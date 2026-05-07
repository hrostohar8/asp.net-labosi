using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace TicketingSystemFightNight.Helpers
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            session.SetString(key, json);
        }

        public static T? GetObject<T>(this ISession session)
        {
            var json = session.GetString(typeof(T).FullName!);
            return json == null ? default : JsonSerializer.Deserialize<T>(json);
        }

        public static T? GetObject<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            return json == null ? default : JsonSerializer.Deserialize<T>(json);
        }
    }
}
