using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDb.Libs.DynamoDb
{
    public class GetItem<T> : IGetItem<T>
    {
        private const string numberType = "System.Int32";
        private readonly IAmazonDynamoDB _dynamoClient;

        public GetItem(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }

        public async Task<IEnumerable<T>> GetItems(int? id)
        {
            var queryRequest = RequestBuilder(id);
            var result = await ScanAsync(queryRequest);
            return result.Items.Select(Map).ToList();
        } 

        private T Map(Dictionary<string, AttributeValue> result)
        {
            var retVal = default(T);

            foreach (var prop in retVal.GetType().GetProperties())
            {
                if (prop.GetType() == Type.GetType(numberType))
                {
                    int number = 0;
                    if (int.TryParse(result[prop.Name].N, out number))
                    {
                        prop.SetValue(retVal, number);
                    }
                }
                else
                {
                    prop.SetValue(retVal, result[prop.Name].S);
                }
            }

            return retVal;
        }

        private async Task<ScanResponse> ScanAsync(ScanRequest request)
        {
            var response = await _dynamoClient.ScanAsync(request);
            return response;
        }

        private ScanRequest RequestBuilder(int? id)
        {
            if(id.HasValue == false)
            {
                return new ScanRequest
                {
                    TableName = typeof(T).Name
                };   
            }
            return new ScanRequest
            {
                TableName = typeof(T).Name,
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":v_Id", new AttributeValue{N = id.ToString()}}
                },
                FilterExpression = "Id = :v_Id"
            };
        }
    }
}
