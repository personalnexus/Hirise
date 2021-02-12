using StackExchange.Redis;
using System;
using System.Globalization;

namespace HiriseLib.Redis
{
    public static class HashEntryExtensions
    {
        public static DateTime? GetTimestamp(this HashEntry[] entries, string name)
        {
            DateTime? result;
            if (DateTime.TryParseExact(entries.GetString(name), "O", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.RoundtripKind, out DateTime timestamp))
            {
                result = timestamp;
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static string GetString(this HashEntry[] entries, string name) => FirstOrDefault(entries, name, null);

        public static string FirstOrDefault(this HashEntry[] entries, string name, string defaultValue)
        {
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].Name == name)
                {
                    return entries[i].Value;
                }
            }
            return defaultValue;
        }

        public static void Set(this HashEntry[] entries, Index index, string name, DateTime? value) => entries[index] = new HashEntry(name, value.HasValue ? value.Value.ToString("O") : "");
        public static void Set(this HashEntry[] entries, Index index, string name, string value) => entries[index] = new HashEntry(name, value);
    }
}
