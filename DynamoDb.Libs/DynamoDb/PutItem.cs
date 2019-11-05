using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDb.Libs.DynamoDb
{
    public class PutItem<T> : IPutItem<T>
    {
        private readonly IAmazonDynamoDB _dynamoClient;
        private const string numberType = "System.Int32";

        public PutItem(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        public async Task AddNewEntry(T obj)
        {
            var queryRequest = RequestBuilder(obj);

            await PutItemAsync(queryRequest);
        }
        
        private PutItemRequest RequestBuilder(T obj)
        {
            var item = new Dictionary<string, AttributeValue>();

            foreach (var prop in obj.GetType().GetProperties())
            {
                item.Add(prop.Name,
                   (prop.GetType() == Type.GetType(numberType) ? 
                        new AttributeValue { N = prop.GetValue(obj).ToString() } :
                        new AttributeValue { S = prop.GetValue(obj).ToString() }));
               
            }

            return new PutItemRequest
            {
                TableName = typeof(T).Name,
                Item = item
            };
        }

        private async Task PutItemAsync(PutItemRequest request)
        {
            await _dynamoClient.PutItemAsync(request);
        }
    }
}
