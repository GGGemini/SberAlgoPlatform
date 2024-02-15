using AutoMapper;
using System.Diagnostics;

namespace Test5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             * Почему вызов функции отображения в Automapper стоит так дешево (наносекунды), если мы знаем,
             * что рефлексия имеет большой оверхэд?
             */

            /*
             * При первой конвертации действительно используется рефлексия и создаётся план маппинга,
             * но результаты этих вычислений кэшируются, поэтому последующий маппинг для сущностей,
             * которые уже были вычислены, происходит гораздо быстрее.
             */

            var config = new MapperConfiguration(c => c.CreateMap<Source, Destination>());
            var mapper = config.CreateMapper();

            var source = new Source { Value = "Hello from AutoMapper!" };

            // замеряем время первой конвертации
            var sw = Stopwatch.StartNew();
            var destination1 = mapper.Map<Destination>(source);
            sw.Stop();
            Console.WriteLine($"Время первой конвертации: {sw.ElapsedTicks} тиков");

            // замеряем время второй конвертации
            sw.Restart();
            var destination2 = mapper.Map<Destination>(source);
            sw.Stop();
            Console.WriteLine($"Время второй конвертации: {sw.ElapsedTicks} тиков");

            // замеряем время третьей конвертации
            sw.Restart();
            var destination3 = mapper.Map<Destination>(source);
            sw.Stop();
            Console.WriteLine($"Время третьей конвертации: {sw.ElapsedTicks} тиков");

            /*
             * В этом примере показано, что первое вычисление маппинга Source в Destination будет всегда медленным,
             * зато последующие маппинги этих сущностей происходят почти мгновенно, засчёт быстрого кэша.
             */
        }
    }

    public class Source
    {
        public string Value { get; set; }
    }

    public class Destination
    {
        public string Value { get; set; }
    }
}
