namespace ChustaSoft.Tools.DBAccess
{
    public interface IDatabaseConfiguration
    {
        string ConnectionString { get; }
        string DatabaseName { get; }
    }
}
