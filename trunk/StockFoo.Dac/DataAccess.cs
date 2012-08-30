namespace StockFoo.Dac
{
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;

	/// <summary>
	/// 数据库访问基类.
	/// </summary>
	public abstract class DataAccess
	{
		#region Enums (2)

		/// <summary>
		/// 数据源提供者.
		/// </summary>
		public enum DataProvider
		{
			/// <summary>
			/// OleDb.
			/// </summary>
			OleDb,

			/// <summary>
			/// SqlServer.
			/// </summary>
			SqlServer,

			/// <summary>
			/// Sqlite.
			/// </summary>
			Sqlite,

			/// <summary>
			/// MySql.
			/// </summary>
			MySql
		}

		/// <summary>
		/// 连接管理模式
		/// </summary>
		public enum ConnManagementMode
		{
			/// <summary>
			/// 自动的, 创建一个新的连接, 在方法结束时关闭.
			/// </summary>
			Auto,

			/// <summary>
			/// 手动的, 使用DataAccess.Connection, 需要在外部手动打开或关闭.
			/// </summary>
			Manual
		}

		#endregion Enums

		#region Properties (2)

		/// <summary>
		/// 数据库连接对象.
		/// </summary>
		public DbConnection Connection
		{
			get
			{
				return DbConnection;
			}
		}

		/// <summary>
		/// 数据库连接对象间接变量.
		/// </summary>
		protected DbConnection DbConnection
		{
			get;
			set;
		}

		#endregion Properties

		#region Delegates and Events (1)

		// Delegates (1) 

		/// <summary>
		/// 事务处理回调函数委托.
		/// </summary>
		/// <param name="command">绑定的DbCommand对象.</param>
		/// <param name="paramsDatas">附加的参数数据.</param>
		/// <returns>Object.</returns>
		public delegate object ExecuteHandler(DbCommand command, params object[] paramsDatas);

		#endregion Delegates and Events

		#region Methods (7)

		// Public Methods (7) 

		/// <summary>
		/// 创建DataAccess类的实例.
		/// </summary>
		/// <param name="conn">数据库连接对象.</param>
		/// <returns>新的DataAccess类的实例.</returns>
		public static DataAccess Create(DbConnection conn)
		{
			if (conn is SqlConnection)
				return new SqlClient(conn);
			//if (conn is OleDbConnection)
			//    return new OleDb(conn);
			//if (conn is SQLiteConnection)
			//    return new Sqlite(conn);
			//if (conn is MySqlConnection)
			//    return new MySql(conn);
			return null;
		}

		/// <summary>
		/// 创建一个数据连接对象.
		/// </summary>
		/// <param name="connectionString">连接字符串.</param>
		/// <param name="dataProvider">数据源提供者.</param>
		/// <returns>如果传递了错误的dataProvider参数, 将返回Null.</returns>
		public static DbConnection CreateConnection(string connectionString, DataProvider dataProvider)
		{
			DbConnection conn = null;
			switch (dataProvider) {
				//case DataProvider.OleDb:
				//    conn = new OleDbConnection(connectionString);
				//    break;
				//case DataProvider.Sqlite:
				//    conn = new SQLiteConnection(connectionString);
				//    break;
				case DataProvider.SqlServer:
					conn = new SqlConnection(connectionString);
					break;
				//case DataProvider.MySql:
				//    conn = new MySqlConnection(connectionString);
				//    break;
			}

			return conn;
		}

		/// <summary>
		/// 创建一个命令参数.
		/// </summary>
		/// <param name="parameterName">参数名.</param>
		/// <param name="value">值.</param>
		/// <returns>DbParameter.</returns>
		public abstract DbParameter CreateParameter(string parameterName, object value);

		/// <summary>
		/// 执行命令并返回受影响的行数.
		/// </summary>
		/// <param name="cmMode">连接管理模式.</param>
		/// <param name="isTransaction">是否事务处理?</param>
		/// <param name="cmdText">待执行的命令.</param>
		/// <param name="cmdParams">命令参数.</param>
		/// <returns>受影响的行数.</returns>
		public abstract int ExecuteNonQuery(
			ConnManagementMode cmMode
			, bool isTransaction
			, string cmdText
			, params DbParameter[] cmdParams);

		/// <summary>
		/// 执行命令并返回数据流, 自动模式时调用后必须关闭数据流对象.
		/// </summary>
		/// <param name="cmMode">连接管理模式.</param>
		/// <param name="cmdText">待执行的命令.</param>
		/// <param name="cmdParams">命令参数.</param>
		/// <returns>数据流对象.</returns>
		public abstract IDataReader ExecuteReader(
			ConnManagementMode cmMode
			, string cmdText
			, params DbParameter[] cmdParams);

		/// <summary>
		/// 执行命令并返回查询所返回的结果集中第一行的第一列. 忽略其他列或行.
		/// </summary>
		/// <param name="cmMode">连接管理模式.</param>
		/// <param name="isTransaction">是否事务处理?</param>
		/// <param name="cmdText">待执行的命令.</param>
		/// <param name="cmdParams">命令参数.</param>
		/// <returns>结果集中第一行的第一列或空引用(如果结果集为空).</returns>
		public abstract object ExecuteScalar(
			ConnManagementMode cmMode
			, bool isTransaction
			, string cmdText
			, params DbParameter[] cmdParams);

		/// <summary>
		/// 执行一个自定义的事务处理.
		/// </summary>
		/// <param name="cmMode">连接管理模式.</param>
		/// <param name="handler">事务处理回调函数.</param>
		/// <param name="paramDatas">需要传递给回调函数的参数.</param>
		/// <returns>返回handler函数的返回值.</returns>
		public abstract object ExecuteTranscation(
			ConnManagementMode cmMode
			, ExecuteHandler handler
			, params object[] paramDatas);

		#endregion Methods
	}
}
