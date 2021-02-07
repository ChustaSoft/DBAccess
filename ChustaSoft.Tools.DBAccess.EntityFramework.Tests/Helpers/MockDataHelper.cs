using ChustaSoft.Common.Contracts;
using FizzWare.NBuilder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.DBAccess.EntityFramework.Tests
{
    public class MockDataHelper
    {

        internal static Guid STATIC_COUNTRY_ID => Guid.Parse("da4ae9b5-f65e-4bd2-8645-e6954c913c14");
        internal static Guid STATIC_CITY_ID => Guid.Parse("6a344614-c90d-4995-9d8e-ff9a9d17aee1");
        internal static Guid STATIC_PERSON_ID => Guid.Parse("f79210d7-8be3-4795-a45a-5e46162ac6cc");


        internal IList<Country> Countries { get; private set; } = new List<Country>();
        internal IList<City> Cities { get; private set; } = new List<City>();
        internal IList<Person> Persons { get; private set; } = new List<Person>();
        internal IList<Address> Addresses { get; private set; } = new List<Address>();


        public MockDataHelper()
        {
            Countries = Builder<Country>.CreateListOfSize(10)
                .TheFirst(1).With(x => x.Id = STATIC_COUNTRY_ID)
                .Build();

            Cities = Builder<City>.CreateListOfSize(10)
                .TheFirst(1).With(x => x.Id = STATIC_CITY_ID)
                .All().With(x => x.CountryId = GetRandomId(Countries)).Build();

            Persons = Builder<Person>.CreateListOfSize(2000)
                .TheFirst(1).With(x => x.Id = STATIC_PERSON_ID)
                .Build();

            foreach (var person in Persons) 
            {
                int sequenceNumber = 1;

                ((List<Address>)Addresses).AddRange(Builder<Address>.CreateListOfSize(GetRandom(1, 5))
                    .All()
                        .With(x => x.Id = Guid.NewGuid())
                        .With(x => x.Sequence == sequenceNumber++)
                        .With(x => x.CityId = GetRandomId(Cities))
                        .With(x => x.PersonId = person.Id)
                    .Build());
            }        
        }


        private Guid GetRandomId<T>(IList<T> keyableList)
            where T : IKeyable<Guid>
        {
            var randomId = GetRandom(0, keyableList.Count());

            return keyableList[randomId].Id;
        }

        private int GetRandom(int min, int max) 
        {
            Random rnd = new Random();
            var randomNumber = rnd.Next(min, max);
            
            return randomNumber;
        }

    }
}
