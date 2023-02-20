//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json.Serialization;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace WildlifeMortalities.Data;

//#pragma warning disable CS8603 // Possible null reference return.
//#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
//public class ItemConverterDecorator<TItemConverter> : JsonConverterFactory
//    where TItemConverter : JsonConverter, new()
//{
//    readonly TItemConverter _itemConverter = new();

//    public override bool CanConvert(Type typeToConvert)
//    {
//        var (itemType, _, _) = GetItemType(typeToConvert);
//        if (itemType == null)
//            return false;
//        return _itemConverter.CanConvert(itemType);
//    }

//    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
//    {
//        var (itemType, isArray, isSet) = GetItemType(typeToConvert);
//        if (itemType == null)
//            return null;
//        if (isArray)
//        {
//            return (JsonConverter)
//                Activator.CreateInstance(
//                    typeof(ArrayItemConverterDecorator<>).MakeGenericType(
//                        typeof(TItemConverter),
//                        itemType
//                    ),
//                    new object[] { options, _itemConverter }
//                );
//        }

//        if (
//            !typeToConvert.IsAbstract
//            && !typeToConvert.IsInterface
//            && typeToConvert.GetConstructor(Type.EmptyTypes) != null
//        )
//        {
//            return (JsonConverter)
//                Activator.CreateInstance(
//                    typeof(ConcreteCollectionItemConverterDecorator<,,>).MakeGenericType(
//                        typeof(TItemConverter),
//                        typeToConvert,
//                        typeToConvert,
//                        itemType
//                    ),
//                    new object[] { options, _itemConverter }
//                );
//        }

//        if (isSet)
//        {
//            var setType = typeof(HashSet<>).MakeGenericType(itemType);
//            if (typeToConvert.IsAssignableFrom(setType))
//            {
//                return (JsonConverter)
//                    Activator.CreateInstance(
//                        typeof(ConcreteCollectionItemConverterDecorator<,,>).MakeGenericType(
//                            typeof(TItemConverter),
//                            setType,
//                            typeToConvert,
//                            itemType
//                        ),
//                        new object[] { options, _itemConverter }
//                    );
//            }
//        }
//        else
//        {
//            var listType = typeof(List<>).MakeGenericType(itemType);
//            if (typeToConvert.IsAssignableFrom(listType))
//            {
//                return (JsonConverter)
//                    Activator.CreateInstance(
//                        typeof(ConcreteCollectionItemConverterDecorator<,,>).MakeGenericType(
//                            typeof(TItemConverter),
//                            listType,
//                            typeToConvert,
//                            itemType
//                        ),
//                        new object[] { options, _itemConverter }
//                    );
//            }
//        }
//        // OK it's not an array and we can't find a parameterless constructor for the type.  We can serialize, but not deserialize.
//        return (JsonConverter)
//            Activator.CreateInstance(
//                typeof(EnumerableItemConverterDecorator<,>).MakeGenericType(
//                    typeof(TItemConverter),
//                    typeToConvert,
//                    itemType
//                ),
//                new object[] { options, _itemConverter }
//            );
//    }

//    static (Type Type, bool IsArray, bool isSet) GetItemType(Type type)
//    {
//        // Quick reject for performance
//        // Dictionary is not implemented.
//        if (
//            type.IsPrimitive || type == typeof(string) || typeof(IDictionary).IsAssignableFrom(type)
//        )
//            return (null, false, false);
//        if (type.IsArray)
//        {
//            return type.GetArrayRank() == 1
//                ? (type.GetElementType(), true, false)
//                : (null, false, false);
//        }

//        Type itemType = null;
//        var isSet = false;
//        foreach (var iType in type.GetInterfacesAndSelf())
//        {
//            if (iType.IsGenericType)
//            {
//                var genType = iType.GetGenericTypeDefinition();
//                if (genType == typeof(ISet<>))
//                {
//                    isSet = true;
//                }
//                else if (genType == typeof(IEnumerable<>))
//                {
//                    var thisItemType = iType.GetGenericArguments()[0];
//                    if (itemType != null && itemType != thisItemType)
//                        return (null, false, false); // type implements multiple enumerable types simultaneously.
//                    itemType = thisItemType;
//                }
//                else if (genType == typeof(IDictionary<,>))
//                {
//                    return (null, false, false);
//                }
//            }
//        }
//        return (itemType, false, isSet);
//    }

//    abstract class CollectionItemConverterDecoratorBase<TEnumerable, TItem>
//        : JsonConverter<TEnumerable> where TEnumerable : IEnumerable<TItem>
//    {
//        readonly JsonSerializerOptions _modifiedOptions;

//        public CollectionItemConverterDecoratorBase(
//            JsonSerializerOptions options,
//            TItemConverter converter
//        )
//        {
//            // Clone the incoming options and insert the item converter at the beginning of the clone.
//            // Then if converter is actually a JsonConverterFactory (e.g. JsonStringEnumConverter) then the correct JsonConverter<T> will be manufactured or fetched.
//            _modifiedOptions = new JsonSerializerOptions(options);
//            _modifiedOptions.Converters.Insert(0, converter);
//        }

//        protected TCollection BaseRead<TCollection>(ref Utf8JsonReader reader, Type typeToConvert)
//            where TCollection : ICollection<TItem>, new()
//        {
//            if (reader.TokenType != JsonTokenType.StartArray)
//                throw new JsonException(); // Unexpected token type
//            var list = new TCollection();
//            while (reader.Read())
//            {
//                if (reader.TokenType == JsonTokenType.EndArray)
//                    break;
//                list.Add(JsonSerializer.Deserialize<TItem>(ref reader, _modifiedOptions));
//            }
//            return list;
//        }

//        public sealed override void Write(
//            Utf8JsonWriter writer,
//            TEnumerable value,
//            JsonSerializerOptions options
//        )
//        {
//            writer.WriteStartArray();
//            foreach (var item in value)
//                JsonSerializer.Serialize(writer, item, _modifiedOptions);
//            writer.WriteEndArray();
//        }
//    }

//    sealed class ArrayItemConverterDecorator<TItem>
//        : CollectionItemConverterDecoratorBase<TItem[], TItem>
//    {
//        public ArrayItemConverterDecorator(JsonSerializerOptions options, TItemConverter converter)
//            : base(options, converter) { }

//        public override TItem[] Read(
//            ref Utf8JsonReader reader,
//            Type typeToConvert,
//            JsonSerializerOptions options
//        ) => BaseRead<List<TItem>>(ref reader, typeToConvert)?.ToArray();
//    }

//    sealed class ConcreteCollectionItemConverterDecorator<TCollection, TEnumerable, TItem>
//        : CollectionItemConverterDecoratorBase<TEnumerable, TItem>
//        where TCollection : ICollection<TItem>, TEnumerable, new()
//        where TEnumerable : IEnumerable<TItem>
//    {
//        public ConcreteCollectionItemConverterDecorator(
//            JsonSerializerOptions options,
//            TItemConverter converter
//        ) : base(options, converter) { }

//        public override TEnumerable Read(
//            ref Utf8JsonReader reader,
//            Type typeToConvert,
//            JsonSerializerOptions options
//        ) => BaseRead<TCollection>(ref reader, typeToConvert);
//    }

//    sealed class EnumerableItemConverterDecorator<TEnumerable, TItem>
//        : CollectionItemConverterDecoratorBase<TEnumerable, TItem>
//        where TEnumerable : IEnumerable<TItem>
//    {
//        public EnumerableItemConverterDecorator(
//            JsonSerializerOptions options,
//            TItemConverter converter
//        ) : base(options, converter) { }

//        public override TEnumerable Read(
//            ref Utf8JsonReader reader,
//            Type typeToConvert,
//            JsonSerializerOptions options
//        ) =>
//            throw new NotImplementedException(
//                string.Format(
//                    "Deserialization is not implemented for type {0}",
//                    typeof(TEnumerable)
//                )
//            );
//    }
//}

//public static class TypeExtensions
//{
//    public static IEnumerable<Type> GetInterfacesAndSelf(this Type type) =>
//        (type ?? throw new ArgumentNullException()).IsInterface
//            ? new[] { type }.Concat(type.GetInterfaces())
//            : type.GetInterfaces();
//}

//#pragma warning restore CS8603 // Possible null reference return.
//#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.



