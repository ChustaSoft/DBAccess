namespace ChustaSoft.Tools.DBAccess
{
    public class EntityFrameworkSearchParameters<TEntity> : SearchParameters<TEntity>
        where TEntity : class
    {
        public bool TrackingEnabled { get; set; }


        public EntityFrameworkSearchParameters()
            : base()
        {
            TrackingEnabled = false;
        }

    }
}
