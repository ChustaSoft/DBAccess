using ChustaSoft.Tools.DBAccess.EntityFramework.IntegrationTests.Context;
using System;
using TestSupport.EfHelpers;

namespace ChustaSoft.Tools.DBAccess.EntityFramework.IntegrationTests.Base
{
    public class IntegrationTestBase : IDisposable
    {

        protected readonly IUnitOfWork _unitOfWork;


        public IntegrationTestBase()
        {
            var options = SqliteInMemory.CreateOptions<TestContext>();
            var testContext = new TestContext(options);

            testContext.Database.EnsureCreated();

            _unitOfWork = new UnitOfWork<TestContext>(testContext);
        }


        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

    }
}
