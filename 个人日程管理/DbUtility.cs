using Google.Protobuf.WellKnownTypes;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Enum = System.Enum;

namespace LiZhiruiToolSet
{
    public enum DbProviderType : byte
    {
        SqlServer,
        MySql,
        SQLite,
        Oracle,
        ODBC,
        OleDb,
        Firebird,
        PostgreSql,
        DB2,
        Informix,
        SqlServerCe
    }

    public class ProviderFactory
    {
        private static Dictionary<DbProviderType,string> providerInvariantNames = new Dictionary<DbProviderType,string>();
        private static Dictionary<DbProviderType,DbProviderFactory> providerFactories = new Dictionary<DbProviderType,DbProviderFactory>();

        static ProviderFactory()
        {
            providerInvariantNames.Add(DbProviderType.SqlServer,"System.Data.SqlClient");
            providerInvariantNames.Add(DbProviderType.OleDb,"System.Data.OleDb");  
            providerInvariantNames.Add(DbProviderType.ODBC,"System.Data.ODBC");  
            providerInvariantNames.Add(DbProviderType.Oracle,"Oracle.DataAccess.Client");  
            providerInvariantNames.Add(DbProviderType.MySql,"MySql.Data.MySqlClient");  
            providerInvariantNames.Add(DbProviderType.SQLite,"System.Data.SQLite");  
            providerInvariantNames.Add(DbProviderType.Firebird,"FirebirdSql.Data.Firebird");  
            providerInvariantNames.Add(DbProviderType.PostgreSql,"Npgsql");  
            providerInvariantNames.Add(DbProviderType.DB2,"IBM.Data.DB2.iSeries");  
            providerInvariantNames.Add(DbProviderType.Informix,"IBM.Data.Informix");  
            providerInvariantNames.Add(DbProviderType.SqlServerCe,"System.Data.SqlServerCe");  
        }

        private static DbProviderFactory ImportDbProviderFactory(DbProviderType providerType)
        {
            var providerName = providerInvariantNames[providerType];
            DbProviderFactory factory = null;

            try
            {
                factory = DbProviderFactories.GetFactory(providerName);
            }
            catch(ArgumentException e)
            {
                factory = null;
            }

            return factory;
        }

        public static string GetProviderInvariantName(DbProviderType providerType)
        {
            return providerInvariantNames[providerType];
        }

        public static DbProviderFactory GetDbProviderFactory(DbProviderType providerType)
        {
            if(!providerFactories.ContainsKey(providerType))
            {
                providerFactories.Add(providerType,ImportDbProviderFactory(providerType));
            }
            
            return providerFactories[providerType];
        }
    }

    class DbUtility
    {
        public string ConnectionString;
        private DbProviderFactory providerFactory;
        private static DbUtility dbUtility = null;
        private DbConnection Connection = null;
        private DbTransaction Transaction = null;

        private DbUtility()
        {
            
        }

        public static DbUtility GetInstance()
        {
            if(dbUtility == null)
            {
                dbUtility = new DbUtility();
            }

            return dbUtility;
        }

        public static DbUtility GetThreadInstance()
        {
            return new DbUtility();
        }

        public void Init(string connectionString,DbProviderType providerType)
        {
            ConnectionString = connectionString;
            providerFactory = ProviderFactory.GetDbProviderFactory(providerType);

            if(providerFactory == null)
            {
                throw new ArgumentException("找不到该数据库提供者类型！");
            }

            Connection = providerFactory.CreateConnection();
        }

        public void Open()
        {
            Connection.ConnectionString = ConnectionString;
            Connection.Open();
        }

        public void Close()
        {
            if(Connection != null)
            {
                Connection.Close();
                Connection = null;
            }
        }

        private DbParameter CreateDbParameter(string name,ParameterDirection parameterDirection,object value)
        {
            var parameter = providerFactory.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            parameter.Direction = parameterDirection;
            return parameter;
        }

        public DbParameter CreateDbParameter(string name,object value)
        {
            return CreateDbParameter(name,ParameterDirection.Input,value);
        }

        private DbCommand CreateDbCommand(string sql,IList<DbParameter> parameters,CommandType commandType)
        {
            var command = providerFactory.CreateCommand();
            command.CommandText = sql;
            command.CommandType = commandType;
            command.Connection = Connection;

            if(!(parameters == null || parameters.Count == 0))
            {
                foreach(var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        public int ExecuteNonQuery(string sql,IList<DbParameter> parameters,CommandType commandType)
        {
            using(var command = CreateDbCommand(sql,parameters,commandType))
            {
                int affectedRows = command.ExecuteNonQuery();
                return affectedRows;
            }
        }

        public int ExecuteNonQuery(string sql,IList<DbParameter> parameters)
        {
            return ExecuteNonQuery(sql,parameters,CommandType.Text);
        }

        public DbDataReader ExecuteReader(string sql,IList<DbParameter> parameters,CommandType commandType)
        {
            var command = CreateDbCommand(sql,parameters,commandType);
            return command.ExecuteReader();
        }

        public DbDataReader ExecuteReader(string sql,IList<DbParameter> parameters)
        {
            return ExecuteReader(sql,parameters,CommandType.Text);
        }

        public DataTable ExecuteDataTable(string sql,IList<DbParameter> parameters,CommandType commandType)
        {
            using(var command = CreateDbCommand(sql,parameters,commandType))
            {
                using(var adapter = providerFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;
                    var data = new DataTable();
                    adapter.Fill(data);
                    return data;
                }
            }
        }

        public DataTable ExecuteDataTable(string sql,IList<DbParameter> parameters)
        {
            return ExecuteDataTable(sql,parameters,CommandType.Text);
        }

        public object ExecuteScalar(string sql,IList<DbParameter> parameters,CommandType commandType)
        {
            using(var command = CreateDbCommand(sql,parameters,commandType))
            {
                return command.ExecuteScalar();
            }
        }

        public object ExecuteScalar(string sql,IList<DbParameter> parameters)
        {
            return ExecuteScalar(sql,parameters,CommandType.Text);
        }

        public List<T> GetEntities<T>(DataTable data) where T : new()
        {
            var type = typeof(T);
            var r = new List<T>();
            var fields = type.GetFields();

            for(var row = 0;row < data.Rows.Count;row++)
            {
                var newObj = Activator.CreateInstance<T>();

                foreach(var field in fields)
                {
                    var name = field.Name;
                    
                    try
                    {
                        if(data.Rows[row].IsNull(name))
                        {
                            field.SetValue(newObj,Activator.CreateInstance(field.FieldType,null));
                        }
                        else if(field.FieldType == typeof(string))
                        {
                            var value = data.Rows[row].Field<string>(name);
                            field.SetValue(newObj,value);
                        }
                        else if(field.FieldType.IsEnum)
                        {
                            var value = data.Rows[row].Field<int>(name);
                            field.SetValue(newObj,value);
                        }
                        else if(field.FieldType == typeof(float))
                        {
                            var value = data.Rows[row].Field<float>(name);
                            field.SetValue(newObj,value);
                        }
                        else if(field.FieldType == typeof(double))
                        {
                            var value = data.Rows[row].Field<double>(name);
                            field.SetValue(newObj,value);
                        }
                        else if(field.FieldType == typeof(bool))
                        {
                            var value = data.Rows[row].Field<bool>(name);
                            field.SetValue(newObj,value);
                        }
                        else if(field.FieldType == typeof(int))
                        {
                            var value = data.Rows[row].Field<int>(name);
                            field.SetValue(newObj,value);
                        }
                        else if(field.FieldType == typeof(long))
                        {
                            var value = data.Rows[row].Field<long>(name);
                            field.SetValue(newObj,value);
                        }
                        else if(field.FieldType == typeof(DateTime))
                        {
                            var value = data.Rows[row].Field<DateTime>(name);
                            field.SetValue(newObj,value);
                        }
                    }
                    catch(Exception e)
                    {
                    
                    }
                }

                r.Add(newObj);
            }

            return r;
        }

        public List<T> QueryForList<T>(string sql,IList<DbParameter> parameters,CommandType commandType) where T : new()
        {
            var data = ExecuteDataTable(sql,parameters,commandType);
            return GetEntities<T>(data);
        }

        public List<T> QueryForList<T>(string sql,IList<DbParameter> parameters) where T : new()
        {
            return QueryForList<T>(sql,parameters,CommandType.Text);
        }

        public T QueryForObject<T>(string sql,IList<DbParameter> parameters,CommandType commandType) where T : new()
        {
            var list = QueryForList<T>(sql,parameters,commandType);

            if(list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return default(T);
            }
        }

        public T QueryForObject<T>(string sql,IList<DbParameter> parameters) where T : new()
        {
            return QueryForObject<T>(sql,parameters,CommandType.Text);
        }

        public string DeleteLastCharacter(string str)
        {
            if(str.Length > 0)
            {
                return str.Substring(0,str.Length - 1);
            }

            return "";
        }

        private object CSharpTypeToDbType(object obj)
        {
            var type = obj.GetType();

            if(obj == null)
            {
                return Activator.CreateInstance(type,null);
            }
            if(type.IsEnum)
            {
                return (int)obj;
            }
            else if(type == typeof(Boolean))
            {
                return ((bool)obj) ? 1 : 0;
            }
            else
            {
                return obj;
            }
        }

        public int Insert<T>(string tableName,T entity) where T : new()
        {
            var fields = typeof(T).GetFields();
            var fieldList = new StringBuilder();
            var valueList = new StringBuilder();
            var pList = new List<DbParameter>();
            FieldInfo idField = null;

            foreach(var field in fields)
            {
                if(field.Name.ToLower() == "id")
                {
                    idField = field;
                    continue;
                }

                fieldList.Append(field.Name);
                fieldList.Append(',');
                valueList.Append('@' + field.Name);
                valueList.Append(',');
                pList.Add(CreateDbParameter(field.Name,CSharpTypeToDbType(field.GetValue(entity))));
            }

            var sql = "insert into " + tableName + "(" + DeleteLastCharacter(fieldList.ToString()) + ") values(" + DeleteLastCharacter(valueList.ToString()) + ")";
            int r = ExecuteNonQuery(sql,pList);

            if(idField != null)
            {
                idField.SetValue(entity,(int)(ulong)ExecuteScalar("select last_insert_id()",new List<DbParameter>()));
            }

            return r;
        }

        public int Update<T>(string tableName,T entity,string condition,IList<DbParameter> conditionParameters) where T : new()
        {
            var fields = typeof(T).GetFields();
            var fieldList = new StringBuilder();
            var pList = new List<DbParameter>();

            foreach(var field in fields)
            {
                if(field.Name.ToLower() == "id")
                {
                    continue;
                }

                fieldList.Append(field.Name);
                fieldList.Append("=@" + field.Name + ",");
                pList.Add(CreateDbParameter(field.Name,CSharpTypeToDbType(field.GetValue(entity))));
            }

            var sql = "update " + tableName + " set " + DeleteLastCharacter(fieldList.ToString());

            if(condition.Length > 0)
            {
                sql += " where " + condition;
                pList.AddRange(conditionParameters);
            }

            return ExecuteNonQuery(sql,pList);
        }

        public int Update<T>(string tableName,T entity,string conditionFieldName,object conditionFieldValue) where T : new()
        {
            var plist = new List<DbParameter>();

            plist.Add(CreateDbParameter("_" + conditionFieldName,conditionFieldValue));
            return Update(tableName,entity,conditionFieldName + " = @_" + conditionFieldName,plist);
        }

        public int Delete(string tableName,string condition,IList<DbParameter> conditionParameter)
        {
            var sql = "delete from " + tableName;
            var pList = new List<DbParameter>();

            if(condition.Length > 0)
            {
                sql += " where " + condition;
                pList.AddRange(conditionParameter);
            }

            return ExecuteNonQuery(sql,pList);
        }

        public int Delete(string tableName,string conditionFieldName,object conditionFieldValue)
        {
            var plist = new List<DbParameter>();

            plist.Add(CreateDbParameter(conditionFieldName,conditionFieldValue));
            return Delete(tableName,conditionFieldName + " = @" + conditionFieldName,plist);
        }

        public List<T> Select<T>(string tableName,IEnumerable<string> resultFields,string condition,IList<DbParameter> conditionParameters) where T : new()
        {
            var resultFieldList = new StringBuilder();
            var pList = new List<DbParameter>();

            foreach(var resultField in resultFields)
            {
                resultFieldList.Append(resultField);
                resultFieldList.Append(",");
            }

            var sql = "select " + DeleteLastCharacter(resultFieldList.ToString()) + " from " + tableName;

            if(condition.Length > 0)
            {
                sql += " where " + condition;
                pList.AddRange(conditionParameters);
            }

            return QueryForList<T>(sql,pList);
        }

        public List<T> Select<T>(string tableName,string condition,IList<DbParameter> conditionParameters) where T : new()
        {
            return Select<T>(tableName,new string[]{"*"},condition,conditionParameters);
        }

        public List<T> Select<T>(string tableName,string conditionFieldName,object conditionFieldValue) where T : new()
        {
            var plist = new List<DbParameter>();

            plist.Add(CreateDbParameter(conditionFieldName,conditionFieldValue));
            return Select<T>(tableName,new string[]{"*"},conditionFieldName + " = @" + conditionFieldName,plist);
        }

        public List<T> Select<T>(string tableName) where T : new()
        {
            return Select<T>(tableName,new string[]{"*"},"",null);
        }

        public List<T> Select<T>(string tableName,IEnumerable<string> resultFields) where T : new()
        {
            return Select<T>(tableName,resultFields,"",null);
        }

        public void BeginTransaction()
        {
            Transaction = Connection.BeginTransaction();
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            Transaction = Connection.BeginTransaction(isolationLevel);
        }

        public void CommitTransaction()
        {
            Transaction.Commit();
        }

        public void RollBackTransaction()
        {
            Transaction.Rollback();
        }

        public void CommitOrRollBackTransaction()
        {
            try
            {
                Transaction.Commit();
            }
            catch(Exception e)
            {
                Transaction.Rollback();
                throw e;
            }
        }
    }
}