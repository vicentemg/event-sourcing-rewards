using Marten;
using Marten.Services;
using EventSourcing.Infrastructure.JsonConverters;
using Weasel.Core;

namespace EventSourcing.Infrastructure.Marten.Configuration;

public static class SerializationExtensions
{
    public static StoreOptions AddMartenSerialization(this StoreOptions options)
    {
        var serializer = new SystemTextJsonSerializer
        {
            EnumStorage = EnumStorage.AsString,
            Casing = Casing.CamelCase,
        };

        serializer.Configure(serializerOptions =>
        {
            serializerOptions.Converters.Add(new MoneyJsonConverter());
        });

        options.Serializer(serializer);

        return options;
    }
}
