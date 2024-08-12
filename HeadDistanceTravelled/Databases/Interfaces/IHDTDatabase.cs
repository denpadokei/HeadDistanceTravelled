using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HeadDistanceTravelled.Databases.Interfaces
{
    public interface IHDTDatabase
    {
        LiteDatabase RawDatabase { get; }
        bool AnyBeatmapCharacteristic();
        IEnumerable<T> Find<T>(Expression<Func<T, bool>> expression, int skip = 0, int limit = int.MaxValue);
        BsonValue Insert<T>(T entity);
        void InsertBulk<T>(IEnumerable<T> enties);
        void SetDefaultValue();
        void Update<T>(T entity);
    }
}