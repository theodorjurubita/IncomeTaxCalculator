using CsvHelper;
using System.Globalization;

namespace Application.CsvEntityReader
{
    public class CsvEntityReader<TEntity> : ICsvEntityReader<TEntity> where TEntity : class
    {
        public virtual List<TEntity> ReadEntitiesFromCsv(Stream stream)
        {
            using var reader = new StreamReader(stream);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csvReader.GetRecords<TEntity>().ToList();
        }
    }
}