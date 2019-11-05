using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDb.Libs.DynamoDb
{
    public interface IPutItem<T>
    {
        Task AddNewEntry(T obj);
    }
}
