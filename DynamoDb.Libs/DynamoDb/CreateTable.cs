using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DynamoDb.Libs.DynamoDb
{
    public class CreateTable<T> : ICreateTable<T>
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private const string numberType = "System.Int32";
        public CreateTable(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        public void CreateDynamoDbTable(T obj)
        {
            try
            {
                CreateTempTable(obj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        private void CreateTempTable(T obj)
        {
            Console.WriteLine("Creating Table");

            var attributeDefinitions = new List<AttributeDefinition>();

            var keySchema = new List<KeySchemaElement>();
            
            foreach (var prop in obj.GetType().GetProperties())
            {
                attributeDefinitions.Add(new AttributeDefinition
                {
                    AttributeName = prop.Name,
                    AttributeType =(prop.GetType() == Type.GetType(numberType) ? "N": "S")
                });

                keySchema.Add(new KeySchemaElement
                {
                    AttributeName = prop.Name,
                    KeyType = "HASH"
                });
            }
            
            var request = new CreateTableRequest
            {
                AttributeDefinitions = attributeDefinitions,
                KeySchema = keySchema,
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                TableName = typeof(T).Name
            };

            var response = _dynamoDbClient.CreateTableAsync(request);

            WaitUntilTableReady(typeof(T).Name);
        }

        public void WaitUntilTableReady(string tableName)
        {
            string status = null;
            do
            {
                Thread.Sleep(5000);
                try
                {
                    var res = _dynamoDbClient.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = tableName
                    });
                    status = res.Result.Table.TableStatus;
                }
                catch (ResourceNotFoundException)
                {

                }

            } while (status != "ACTIVE");
            {
                Console.WriteLine("Table Created Successfully");
            }
        }
    }
}
