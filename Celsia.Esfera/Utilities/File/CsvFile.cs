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

        public IEnumerable<string> Errors { get; set; }

        public CsvFile(ICsvMapping<TEntity> csvMapping)
        {
            this.csvMapping = csvMapping;
        }

        public IEnumerable<TEntity> ParseCSVFile(string ruta)
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ';');
            CsvParser<TEntity> csvParser = new CsvParser<TEntity>(csvParserOptions, this.csvMapping);
            ParallelQuery<CsvMappingResult<TEntity>> results = csvParser.ReadFromFile(ruta, Encoding.UTF8);

            var uploadResult = (from result in results
                                select new { Entity = result.Result, Error = result.Error?.Value }).ToList();

            this.Errors = uploadResult.Where(x => x.Error != null).Select(x => x.Error);

            return uploadResult.Select(x => x.Entity);
        }
    }
}
