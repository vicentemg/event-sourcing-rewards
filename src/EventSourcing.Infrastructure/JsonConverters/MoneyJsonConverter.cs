using System.Text.Json;
using System.Text.Json.Serialization;
using EventSourcing.Domain.Seedwork;

namespace EventSourcing.Infrastructure.JsonConverters;

public class MoneyJsonConverter : JsonConverter<Money>
{
    public override Money Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var node = System.Text.Json.Nodes.JsonNode.Parse(ref reader);
        if (node == null)
            throw new JsonException();

        // Support both "amount" (camelCase) and "Amount" (PascalCase) or depend on naming policy
        // For simplicity assuming camelCase or explicit check.
        // Node["amount"] is case-sensitive. Use logical OR for robustness if needed,
        // or just match the project standard (camelCase).

        var amountNode = node["amount"] ?? node["Amount"];
        if (amountNode == null)
            throw new JsonException("Property 'amount' not found.");

        var amount = (decimal)amountNode;
        var result = Money.Create(amount);

        return result.IsSuccess ? result.Value : throw new JsonException(result.Error);
    }

    public override void Write(Utf8JsonWriter writer, Money value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber(nameof(Money.Amount), value.Amount);
        writer.WriteEndObject();
    }
}
