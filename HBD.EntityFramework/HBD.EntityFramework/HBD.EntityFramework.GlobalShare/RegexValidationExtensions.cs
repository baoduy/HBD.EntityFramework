using System.Text.RegularExpressions;

namespace HBD.EntityFramework
{
    public static class RegexValidationExtensions
    {
        public static bool IsMatch(this string @this, string regex)
        {
            var re = new Regex(regex, RegexOptions.IgnoreCase);
            return re.IsMatch(@this);
        }

        public static bool IsEmail(this string @this)
            => @this.IsMatch(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
    }
}