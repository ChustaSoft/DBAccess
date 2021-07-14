using ChustaSoft.Common.Contracts;
using System;

namespace ChustaSoft.Tools.DBAccess.EntityFramework.Tests
{
    public class Post : IKeyable<Guid>
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Text { get; set; }
        public Person Author { get; set; }

    }
}
