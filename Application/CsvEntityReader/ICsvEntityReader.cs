namespace Application.CsvEntityReader
{
    public interface ICsvEntityReader<TEntity> where TEntity : class
    {
        List<TEntity> ReadEntitiesFromCsv(Stream stream);
    }
}
