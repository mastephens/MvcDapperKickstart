using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcKickstart.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsValidDate(int? birthYear, int? birthMonth, int? birthDay)
        {
            if (birthYear == null || birthMonth == null || birthDay == null) return false;
            DateTime? birthDate = null;
            try
            {
                birthDate = new DateTime(birthYear.Value, birthMonth.Value, birthDay.Value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets someone's age in Years
        /// </summary>
        /// <param name="birthday">The Birth Date of the person</param>
        /// <returns>Age in Years</returns>
        /// <remarks>
        /// http://stackoverflow.com/questions/9/how-do-i-calculate-someones-age
        /// </remarks>
       public static int AgeInYears(this DateTime birthday)
       {
           DateTime now = DateTime.Today;
           int age = now.Year - birthday.Year;
           if (birthday > now.AddYears(-age)) age--;
           return age;
       }

       public static IEnumerable<int> GetBirthMonths()
       {
           return Enumerable.Range(1, 12);
       }

       public static IEnumerable<int> GetBirthDays()
       {
           return Enumerable.Range(1, 31);
       }

       public static IEnumerable<int> GetBirthYears()
       {
           return Enumerable.Range(1906, 95).Reverse();
       }
    }
}