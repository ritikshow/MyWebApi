﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyWebApi.Models
{
    public class Person
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
    }
}
