using ChustaSoft.Common.Contracts;
using System;

namespace ChustaSoft.Tools.DBAccess.Examples.Models
{
    public class Address : IKeyable<Guid>
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public Guid CityId { get; set; }
        public string Line { get; set; }
        public int Sequence { get; set; }


        public City City { get; set; }
        public Person Person { get; set; }

    }
}
