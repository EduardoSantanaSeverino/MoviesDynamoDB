using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDb.Libs.DynamoDb
{
    public interface IGetItem<T>
    {
        Task<IEnumerable<T>> GetItems(int? id);
    }
}
