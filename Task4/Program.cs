using System.Diagnostics;
using System.Text.Json;
using Test4.CustomConverters;
using Test4.Entities;

namespace Test4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string json = "{\"Name\": \"George\", \"Salary\": 3000.00, \"Age\": 38, \"MarriageStatus\": \"Married\"}";
            var dto = Deserialize(json);

            Console.ReadKey();
        }

        static Employee Deserialize(string str)
        {
            var options = new JsonSerializerOptions()
            {
                Converters = { new FieldsOnlyJsonConverter() }
            };
            var dto = JsonSerializer.Deserialize<Employee>(str, options);
            // fail!
            Debug.Assert(dto?.Age == default);
            Debug.Assert(dto.Salary == default);
            Debug.Assert(dto.Name == default);
            Debug.Assert(dto.MarriageStatus == default);

            return dto;
        }
    }
}
