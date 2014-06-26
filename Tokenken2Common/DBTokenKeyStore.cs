using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Nancy.Authentication.Token.Storage;
using Dapper;


namespace Tokenken2Common
{
    public class DBTokenKeyStore : ITokenKeyStore
    {
        private IDbConnection con;

        public DBTokenKeyStore(string connectionString)
        {
            this.con = new System.Data.SqlClient.SqlConnection(connectionString);
        }

        /// <summary>
        /// パージ
        /// </summary>
        public void Purge()
        {
            con.Open();
            IDbTransaction tran = con.BeginTransaction();
            try
            {
                con.Execute("delete from token_key_store", null, tran);
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        /// <summary>
        /// 取り出し
        /// </summary>
        /// <returns></returns>
        public IDictionary<DateTime, byte[]> Retrieve()
        {
            con.Open();
            try
            {
                return con
                    .Query("select ticks, ekey from token_key_store order by ticks")
                    .Select<dynamic, Tuple<long, byte[]>>((src) =>
                    {
                        return new Tuple<long, byte[]>(
                            Convert.ToInt64(src.ticks),
                            (byte[])src.ekey);
                    })
                    .Aggregate(new Dictionary<DateTime, byte[]>(), (memo, item) =>
                    {
                        memo[DateTime.FromBinary(item.Item1)] = item.Item2;
                        return memo;
                    });
            }
            finally
            {
                con.Close();
            }
        }

        /// <summary>
        /// 格納
        /// </summary>
        /// <param name="keys"></param>
        public void Store(IDictionary<DateTime, byte[]> keys)
        {
            con.Open();
            IDbTransaction tran = con.BeginTransaction();
            try
            {
                con.Execute("insert into token_key_store(ticks, ekey)values(@TICKS, @EKEY)", keys.Select((i) => new { TICKS = i.Key.Ticks, EKEY = i.Value }), tran);
                tran.Commit();

                //foreach (var k in keys)
                //{
                //    if (1 <= con.Execute("select count(*) from token_key_store where ticks = @TICKS", new { TICKS = k.Key.Ticks }, tran))
                //        continue;
                //    con.Execute("insert into token_key_store(ticks, ekey)values(@TICKS, @EKEY)", new { TICKS = k.Key.Ticks, EKEY = k.Value }, tran);
                //}

                //tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
            finally
            {
                con.Close();
            }
        }
    }
}
