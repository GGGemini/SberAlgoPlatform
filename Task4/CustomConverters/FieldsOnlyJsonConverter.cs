using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Test4.Entities;

namespace Test4.CustomConverters
{
    public class FieldsOnlyJsonConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            // список классов, которые мы можем десериализовать нашим кастомным конвертером
            bool canConvert = new Type[] { typeof(Employee) }.Contains(typeToConvert);

            return canConvert;
            // return true;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            // создаём конвертер, чтобы не приходилось передавать класс в качестве T при объявлении конвертера
            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(FieldsOnlyJsonConverter<>).MakeGenericType(new Type[] {typeToConvert}),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { },
                culture: null);

            return converter;
        }
    }

    public class FieldsOnlyJsonConverter<T> : JsonConverter<T> where T : new()
    {
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // убеждаемся, что это объект ("{")
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected StartObject token");
            }

            var instance = new T();
            while (reader.Read())
            {
                // если закрывающая фигурная скобка, то конец чтения ("}")
                if (reader.TokenType == JsonTokenType.EndObject)
                    return instance;

                // следующее должно идти название поля
                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException("Token \"PropertyName\" is null");
                string propertyName = reader.GetString();

                // получаем значение
                reader.Read(); // "читаем" и получаем в reader value
                var field = typeToConvert.GetField(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (field != null)
                {
                    object? value;
                    if (field.FieldType.IsEnum)
                    {
                        var enumValue = reader.GetString();
                        if (Enum.TryParse(field.FieldType, enumValue, out var parsedEnum))
                            value = parsedEnum;
                        else // обрабатываем ситуацию, когда в перечислении попалось значение, которого нет в нашем enum
                            throw new JsonException($"Value \"{enumValue}\" is not valid for enum type {field.FieldType.Name}");
                    }
                    else
                    {
                        value = JsonSerializer.Deserialize(ref reader, field.FieldType, options);
                    }

                    if (value != null)
                        field.SetValue(instance, value);
                    else
                        throw new JsonException($"Failed to deserealize field \"{field.FieldType.Name}\"");
                }
                else // если в классе нет поля, которое есть в json, то пропускаем
                {
                    reader.Skip();
                }
            }

            // если чтение подошло к концу, а мы так и не наткнулись на "}"
            throw new JsonException("Expected EndObject token");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
