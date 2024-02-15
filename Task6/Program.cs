using System.Text.RegularExpressions;

namespace Test6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * Cуществуют ли проблемы с вызовом такого кода, где db – DbContext EFCore?:
             * 
             * var set = await db.AlgoTypes
             *  .GroupBy(x => x.AlgoHostCode)
             *  .Select(x => new {x.Key, Value = x.Select(s => s.Code).ToArray()})
             *  .ToArrayAsync();
             */

            /*
             * - Во-первых, нужно убедиться в том, что SQL, который будет в итоге сгенерирован, будет работать в нашей СУБД.
             * - Во-вторых, GroupBy и последующий вызов метода Select может создать сложный и неоптимизированный запрос.
             * 
             * Эти два пункта можно проверить, залогировав генерируемый EF-ом SQL. Это можно сделать следующими способами:
             * 1)
             * public void ConfigureServices(IServiceCollection services)
             * {
             *  services.AddDbContext<YourDbContext>(options =>
             *      options.UseSqlServer("ВашаСтрокаПодключения")
             *          .LogTo(Console.WriteLine, LogLevel.Information));
             * }
             * 2)
             * var query = db.AlgoTypes...();
             * Console.WriteLine(query.ToQueryString());
             * 
             * - В-третьих, необходимо оценить количество данных, для которых мы применяем это выражение и,
             * если данных много, то задуматься о пагинации (выгрузке данных по кускам).
             * 
             * - В-четвёртых нужно посмотреть скорость работы базы данных. Если база данных работает медленно, то, возможно,
             * стоит часть логики обработки данных переложить на приложение:
             */

            //public void ConfigureServices(IServiceCollection services)
            //{
            //    services.AddDbContext<AppDbContext>(options =>
            //        options.UseSqlServer("СтрокаПодключения")
            //            .LogTo(Console.WriteLine, LogLevel.Information));
            //}
            
            //var query = db.AlgoTypes...();
            //Console.WriteLine(query.ToQueryString());

            //// предполагая, что нам нужны только коды и мы можем обработать их после извлечения
            //var algoTypes = await db.AlgoTypes
            //    .Select(x => new { x.AlgoHostCode, x.Code })
            //    .ToListAsync();
            //// обработка данных на стороне клиента для уменьшения нагрузки на БД
            //var grouped = algoTypes
            //    .GroupBy(x => x.AlgoHostCode)
            //    .Select(g => new { Key = g.Key, Value = g.Select(x => x.Code).ToArray() })
            //    .ToArray();

        }
    }
}
