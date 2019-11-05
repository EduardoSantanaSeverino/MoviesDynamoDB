using System;
using System.Collections.Generic;
using System.Text;

namespace DynamoDb.Libs.DynamoDb
{
    public interface ICreateTable<T>
    {
        void CreateDynamoDbTable(T obj);
    }
}
