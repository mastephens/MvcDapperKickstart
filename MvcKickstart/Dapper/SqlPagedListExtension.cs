using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Dapper;

namespace MvcKickstart.Dapper
{
    public static partial class SqlMapperExtensions
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> TypeTableName = new ConcurrentDictionary<RuntimeTypeHandle, string>();

        private static string GetTableName(Type type)
        {
            string name;
            if (!TypeTableName.TryGetValue(type.TypeHandle, out name))
            {
                name = type.Name;// +"s";
                if (type.IsInterface && name.StartsWith("I"))
                    name = name.Substring(1);

                //NOTE: This as dynamic trick should be able to handle both our own Table-attribute as well as the one in EntityFramework 
                var tableattr = type.GetCustomAttributes(false).Where(attr => attr.GetType().Name == "TableAttribute").SingleOrDefault() as
                    dynamic;
                if (tableattr != null)
                    name = tableattr.Name;
                TypeTableName[type.TypeHandle] = name;
            }
            return name;
        }

        public static IEnumerable<T> PagedList<T>(this IDbConnection connection, int page, int pageSize, string where, DynamicParameters parameters, string orderBy)
        {

            int startRow = page * pageSize;
            int maxRow = page * pageSize + pageSize;
            string tableName = GetTableName(typeof(T));
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DECLARE @Temp" + tableName + " TABLE");
            sb.AppendLine("(");
            sb.AppendLine("Id int IDENTITY,");
            sb.AppendLine("FKId int");
            sb.AppendLine(")");

            sb.AppendLine("DECLARE @maxRow int");

            sb.AppendLine("SET @maxRow = " + maxRow);

            sb.AppendLine("SET ROWCOUNT @maxRow");

            sb.AppendLine("INSERT INTO @Temp" + tableName + " (FKId)");
            sb.AppendLine("SELECT Id");
            sb.AppendLine("FROM " + tableName);
            if (!string.IsNullOrWhiteSpace(where))
            {
                sb.AppendLine("WHERE /**where**/");
            }
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                sb.AppendLine("ORDER BY /**orderby**/");
            }
            else
            {
                sb.AppendLine("ORDER BY Id ASC");
            }

            sb.AppendLine("SET ROWCOUNT " + pageSize);

            sb.AppendLine("SELECT s.*");
            sb.AppendLine("FROM @Temp" + tableName + " t");
            sb.AppendLine(" INNER JOIN " + tableName + " s ON");
            sb.AppendLine(" s.Id = t.FKId");
            sb.AppendLine("WHERE t.ID > " + startRow);

            sb.AppendLine("SET ROWCOUNT 0");

            string rawSql = sb.ToString();
            rawSql = rawSql.Replace("/**orderby**/", orderBy);
            rawSql = rawSql.Replace("/**where**/", where);

            var results = connection.Query<T>(rawSql, parameters);

            return results;
        }

    }
}