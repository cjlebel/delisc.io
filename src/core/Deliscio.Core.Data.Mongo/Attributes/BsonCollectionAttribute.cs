// https://medium.com/@marekzyla95/mongo-repository-pattern-700986454a0e

using System;

namespace Deliscio.Core.Data.Mongo.Attributes;
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BsonCollectionAttribute : Attribute
{
    public string CollectionName { get; }

    public BsonCollectionAttribute(string collectionName)
    {
        CollectionName = collectionName;
    }
}
