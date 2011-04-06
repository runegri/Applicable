using System;
using SQLite;
using System.Linq;
namespace SQLite
{
	public static class SQLiteConnectionExtensions
	{
		
		public static bool TableExists(this SQLiteConnection conn, string tableName)
		{
			var name = conn.Query<DbName>("SELECT name FROM sqlite_master WHERE type='table' AND name=?", tableName);
			return name.Any();
		}
		
		public static bool TableExists<T>(this SQLiteConnection conn)
		{
			return TableExists(conn, typeof(T).Name);
		}
		
		public static void EnsureTableExists<T>(this SQLiteConnection conn)
		{
			if (!conn.TableExists<T>())
			{
				conn.CreateTable<T>();
			}
		}
	
		internal class DbName
		{
			public string name {get;set;}
		}		
	}

		
}

