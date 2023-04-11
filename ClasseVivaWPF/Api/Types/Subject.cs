using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasseVivaWPF.Api.Types
{
    public class Subject : ApiObject
    {
        [JsonProperty(Required = Required.Always)]
        public required int Id { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required string Description { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required int Order { get; init; }

        [JsonProperty(Required = Required.Always)]
        public required SubjectTeacher[] Teachers { get; init; }

        public string ShortName => this.Description.Substring(0, 3).ToUpper();

        public string TeachersString => string.Join(", ", Teachers.Select(x => x.TeacherName));

        public static bool operator == (Subject s1, Subject s2){
            return s1.Id == s2.Id;
        }

        public static bool operator !=(Subject s1, Subject s2)
        {
            return !(s1.Id == s2.Id);
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}
