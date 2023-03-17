using System;
using System.Text.RegularExpressions;


namespace ClasseVivaWPF.Api.Types
{
    public record class Me(string Ident, string FirstName, string LastName, bool ShowPwdChangeReminder, string Token, DateTime Release, DateTime Expire) : ApiObject
    {
        private int? _id = null;
        public int Id => _id ??= int.Parse(new Regex("\\d+").Match(Ident).Value);
    };
}
