using RepoDb;
using RepoDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbReflector.Databases
{
    public class Tracer : ITrace
    {
        public void AfterAverage(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterAverageAll(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterBatchQuery(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterCount(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterCountAll(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterDelete(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterDeleteAll(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterExecuteNonQuery(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterExecuteQuery(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterExecuteReader(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterExecuteScalar(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterExists(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterInsert(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterInsertAll(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterMax(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterMaxAll(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterMerge(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterMergeAll(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterMin(TraceLog log)
        {
            throw new NotImplementedException();
        }

        public void AfterMinAll(TraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void AfterQuery(TraceLog log)
        {
            // Some implementations here
            var x = log.Statement;
            if(x.Length > 0)
            {

            }
        }

        public void AfterQueryAll(TraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void AfterQueryMultiple(TraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void AfterSum(TraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void AfterSumAll(TraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void AfterTruncate(TraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void AfterUpdate(TraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void AfterUpdateAll(TraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeAverage(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeAverageAll(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeBatchQuery(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeCount(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeCountAll(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeDelete(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeDeleteAll(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeExecuteNonQuery(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeExecuteQuery(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeExecuteReader(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeExecuteScalar(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeExists(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeInsert(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeInsertAll(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeMax(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeMaxAll(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeMerge(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeMergeAll(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeMin(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeMinAll(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeQuery(CancellableTraceLog log)
        {
            // Some implementations here
        }

        public void BeforeQueryAll(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeQueryMultiple(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeSum(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeSumAll(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeTruncate(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeUpdate(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }

        public void BeforeUpdateAll(CancellableTraceLog log)
        {
            //throw new NotImplementedException();
        }
    }
}
