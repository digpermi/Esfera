using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Utilities.File
{
    public class CsvFile<TEntity>
    {
        private readonly ICsvMapping<TEntity> csvMapping;

        public CsvFile(ICsvMapping<TEntity> csvMapping)
        {
            this.csvMapping = csvMapping;
        }

        public IEnumerable<TEntity> ParseCSVFile(string ruta)
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ';');
            CsvParser<TEntity> csvParser = new CsvParser<TEntity>(csvParserOptions, this.csvMapping);
            ParallelQuery<CsvMappingResult<TEntity>> results = csvParser.ReadFromFile(ruta, Encoding.ASCII);

            return results.Select(x => x.Result);
        }
    }
}
