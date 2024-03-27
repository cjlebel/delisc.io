using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Deliscio.Tools.IdUpdater;

internal class Program
{
    static void Main(string[] args)
    {
        var client = new MongoClient("");
        var database = client.GetDatabase("deliscio");

        var sourceCollection = database.GetCollection<BsonDocument>("badlinks");
        var targetCollection = database.GetCollection<BsonDocument>("links");

        var documents = sourceCollection.Find(new BsonDocument()).ToList();

        var hash = MD5.HashData(Encoding.Default.GetBytes("some-user-id"));
        var userId = new ObjectId(BitConverter.ToString(hash).Replace("-", string.Empty));

        foreach (var document in documents)
        {
            var newDocument = new BsonDocument
            {
                { "_id", ObjectId.GenerateNewId() }
            };

            foreach (var field in document.Elements)
            {
                if (field.Name != "_id")
                {
                    if (field.Name is "DeletedById" or "CreatedById" or "UpdatedById" or "SubmittedById")
                    {
                        newDocument[field.Name] = userId;
                        continue;
                    }

                    if (field.Name == "Tags")
                    {
                        var tagsList = new BsonArray();
                        foreach (var tag in field.Value.AsBsonArray)
                        {
                            var tagDoc = new BsonDocument
                            {
                                { "_id", ObjectId.GenerateNewId() },
                            };

                            foreach (var tagField in tag.AsBsonDocument.Elements.Where(e => e.Name != "_id"))
                            {
                                if (tagField.Name is "DeletedById" or "CreatedById" or "UpdatedById" or "SubmittedById")
                                {
                                    tagDoc[tagField.Name] = userId;
                                    continue;
                                }

                                tagDoc[tagField.Name] = tagField.Value;
                            }

                            tagsList.Add(tagDoc);
                        }

                        newDocument["Tags"] = tagsList;
                    }
                    else
                    {
                        newDocument[field.Name] = field.Value;
                    }
                }
            }

            targetCollection.InsertOne(newDocument);
        }
    }
}
