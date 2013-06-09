using MongoDB.Bson;
using System;


public static class StringExtension
{
    public static ObjectId ToObjectId(this string value)
    {
        return ObjectId.Parse(value);
    }
}