using System.Linq;
using System.Text.RegularExpressions;

namespace ZdravoCorp.Core.Utilities
{
    public static class Validation
    {

        public static bool IsStringAPositiveNumber(string text)
        {
            if (string.IsNullOrEmpty(text) || !(text.All(char.IsDigit)) || (int.Parse(text) < 0))
            {
                return false;
            }
            return true;
        }

        public static bool IsStringAWord(string text)
        {
            Regex regex = new Regex("^[a-zA-Z ]+$");
            if (string.IsNullOrEmpty(text) || !regex.IsMatch(text))
            {
                return false;
            }
            return true;
        }


    }
}
