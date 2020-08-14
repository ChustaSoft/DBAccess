using ChustaSoft.Common.Builders;
using ChustaSoft.Common.Contracts;
using ChustaSoft.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChustaSoft.Tools.DBAccess.Examples
{
    public class ServiceExample 
    {

        private readonly IRepository<Person> personRepository;
        private readonly IUnitOfWork unitOfWork;


        /// <summary>
        /// It is very important to not inject directly the repository, no repository is registered in DI container and it is provided the first time requested
        /// UnitOfWork is managing internally the creation and reusing if possible the different repositories.
        /// UnitOfControl has the responsibility of comminting the transaction after the different actions performed, so it is also required to have it as a class variable for committing tansactions
        /// </summary>
        /// <param name="unitOfWork"></param>
        public ServiceExample(IUnitOfWork unitOfWork)
        {
            this.personRepository = unitOfWork.GetRepository<Person>();
            this.unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Simple usage for retrieving single entity by Id
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public Person GetEntity(Guid personId) 
        {
            return personRepository.GetSingle(personId);
        }

        /// <summary>
        /// Simple usage for retrieving single entity by a condition
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public Person GetEntity()
        {
            return personRepository.GetSingle(x => x.Name == "John");
        }


        /// <summary>
        /// Example of geting multiple entities, with nested object
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Person> GetFilteredWithDependantProperties() 
        {
            return personRepository.GetMultiple(x => x.Name == "John", 
                orderBy: x => x.OrderByDescending(x => x.BirthDate),
                includedProperties: SelectablePropertiesBuilder<Person>.GetProperties().SelectProperty(x => x.Address)
                )
                .ToList();
        }

        /// <summary>
        /// Example of geting multiple entities, with nested object inside collections
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Person> GetFilteredWithDependantPropertiesInCollection()
        {
            return personRepository.GetMultiple(x => x.Name == "John",
                orderBy: x => x.OrderByDescending(x => x.BirthDate),
                includedProperties: SelectablePropertiesBuilder<Person>.GetProperties()
                    .SelectProperty(x => x.Address)
                    .ThenSelectFromCollection(x => x.Address).ThenSelectProperty(x => x.Country)
                )
                .ToList();
        }

        /// <summary>
        /// Example of geting multiple entities, with pagination
        /// Assuming: We are in the third page, and having batches of 1000 rows
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Person> GetFilteredWithPagination()
        {
            return personRepository.GetMultiple(x => x.Name == "John",
                orderBy: x => x.OrderByDescending(x => x.BirthDate),
                batchSize: 1000, skippedBatches: 2
                )
                .ToList();
        }

        /// <summary>
        /// Example of geting multiple entities, with tracking enabled
        /// By default, if nothing is specified, tracking is disabled
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Person> GetFilteredWithTrackingEnabled()
        {
            return personRepository.GetMultiple(x => x.Name == "John",
                orderBy: x => x.OrderByDescending(x => x.BirthDate),
                trackingEnabled: true
                )
                .ToList();
        }


        /// <summary>
        /// Example Saving single entity
        /// </summary>
        /// <param name="person"></param>
        public void SaveEntity(Person person) 
        {
            this.personRepository.Insert(person);
            this.unitOfWork.CommitTransaction();
        }

        /// <summary>
        /// Example Saving multiple entities
        /// </summary>
        /// <param name="person"></param>
        public void SaveEntity(IEnumerable<Person> persons)
        {
            this.personRepository.Insert(persons);
            this.unitOfWork.CommitTransaction();
        }

    }




    public class Person : IKeyable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }

        public IEnumerable<Address> Address { get; set; }
    }


    public class Address : IKeyable<Guid>
    {
        public Guid Id { get; set; }
        public int PersonId { get; set; }
        public string Line { get; set; }

        public Country Country { get; set; }

    }

    public class Country : IKeyable<Guid>
    {
        public Guid Id { get; set; }
    }

}
