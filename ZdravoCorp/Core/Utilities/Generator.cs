using System;

namespace ZdravoCorp.Core.Utilities
{
    public static class Generator
    {
        public static string GenerateRandomId()
        {
            Random random = new Random();
            return random.Next(1, 100000).ToString();
        }

        public static DateTime GenerateDateForAppointment()
        {
            DateTime now = DateTime.Now.AddDays(10);
            return new DateTime(now.Year, now.Month,now.Day, 8, 0, 0);
        }
    }
}
