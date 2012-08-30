namespace StockFoo.Dac
{
	using System.Data;
	using System.Data.Common;
	using System.Data.SqlClient;

	/// <summary>
	/// SqlServer数据库访问类.
	/// </summary>
	public class SqlClient : DataAccess
	{
		#region Constructors (1)

		/// <summary>
		/// 初始化SqlClient类的新实例.
		/// </summary>
		/// <param name="dbConnection">数据库访问对象.</param>
		public SqlClient(DbConnection dbConnection)
		{
			DbConnection = dbConnection;
		}

		#endregion Constructors

		#region Methods (5)

		// Public Methods (5) 

		/// <summary>
		/// 创建一个命令参数.
		/// </summary>
		/// <param name="parameterName">参数名.</param>
		/// <param name="value">值.</param>
		/// <returns>DbParameter.</returns>
		public override DbParameter CreateParameter(string parameterName, object value)
		{
			return new SqlParameter(parameterName, value);
		}

		/// <summary>
		/// 执行命令并返回受影响的行数.
		/// </summary>
		/// <param name="cmMode">连接管理模式.</param>
		/// <param name="isTransaction">是否事务处理?</param>
		/// <param name="cmdText">待执行的命令.</param>
		/// <param name="cmdParams">命令参数.</param>
		/// <returns>受影响的行数.</returns>
		public override int ExecuteNonQuery(
			ConnManagementMode cmMode
			, bool isTransaction
			, string cmdText
			, params DbParameter[] cmdParams)
		{
			var conn = (cmMode == ConnManagementMode.Auto)
									? new SqlConnection(DbConnection.ConnectionString)
									: DbConnection as SqlConnection;

			try
			{
				if (cmMode == ConnManagementMode.Auto)
					conn.Open();

				var cmd = new SqlCommand(cmdText, conn);
				cmd.Parameters.AddRange(cmdParams);

				int returnValue;
				if (isTransaction)
				{
					cmd.Transaction = conn.BeginTransaction();
					try
					{
						returnValue = cmd.ExecuteNonQuery();
						cmd.Transaction.Commit();
					}
					catch
					{
						cmd.Transaction.Rollback();
						throw;
					}
				}
				else
					returnValue = cmd.ExecuteNonQuery();
				return returnValue;
			}
			finally
			{
				if (cmMode == ConnManagementMode.Auto)
					conn.Close();
			}
		}

		/// <summary>
		/// 执行命令并返回数据流, 自动模式时调用后必须关闭数据流对象.
		/// </summary>
		/// <param name="cmMode">连接管理模式.</param>
		/// <param name="cmdText">待执行的命令.</param>
		/// <param name="cmdParams">命令参数.</param>
		/// <returns>数据流对象.</returns>
		public override IDataReader ExecuteReader(
			ConnManagementMode cmMode
			, string cmdText
			, params DbParameter[] cmdParams)
		{
			var conn = (cmMode == ConnManagementMode.Auto)
									? new SqlConnection(DbConnection.ConnectionString)
									: DbConnection as SqlConnection;

			if (cmMode == ConnManagementMode.Auto)
				conn.Open();

			var cmd = new SqlCommand(cmdText, conn);
			cmd.Parameters.AddRange(cmdParams);
			return (cmMode == ConnManagementMode.Auto)
					? cmd.ExecuteReader(CommandBehavior.CloseConnection)
					: cmd.ExecuteReader();
		}

		/// <summary>
		/// 执行命令并返回查询所返回的结果集中第一行的第一列. 忽略其他列或行.
		/// </summary>
		/// <param name="cmMode">连接管理模式.</param>
		/// <param name="isTransaction">是否事务处理?</param>
		/// <param name="cmdText">待执行的命令.</param>
		/// <param name="cmdParams">命令参数.</param>
		/// <returns>结果集中第一行的第一列或空引用(如果结果集为空).</returns>
		public override object ExecuteScalar(
			ConnManagementMode cmMode
			, bool isTransaction
			, string cmdText
			, params DbParameter[] cmdParams)
		{
			var conn = (cmMode == ConnManagementMode.Auto)
				? new SqlConnection(DbConnection.ConnectionString)
				: DbConnection as SqlConnection;

			try
			{
				if (cmMode == ConnManagementMode.Auto)
					conn.Open();

				var cmd = new SqlCommand(cmdText, conn);
				cmd.Parameters.AddRange(cmdParams);

				object returnValue;
				if (isTransaction)
				{
					cmd.Transaction = conn.BeginTransaction();
					try
					{
						returnValue = cmd.ExecuteScalar();
						cmd.Transaction.Commit();
					}
					catch
					{
						cmd.Transaction.Rollback();
						throw;
					}
				}
				else
					returnValue = cmd.ExecuteScalar();

				return returnValue;
			}
			finally
			{
				if (cmMode == ConnManagementMode.Auto)
					conn.Close();
			}
		}

		/// <summary>
		/// 执行一个自定义的事务处理.
		/// </summary>
		/// <param name="cmMode">连接管理模式.</param>
		/// <param name="handler">事务处理回调函数.</param>
		/// <param name="paramDatas">需要传递给回调函数的参数.</param>
		/// <returns>返回handler函数的返回值.</returns>
		public override object ExecuteTranscation(
			ConnManagementMode cmMode
			, ExecuteHandler handler
			, params object[] paramDatas)
		{
			var conn = (cmMode == ConnManagementMode.Auto)
				? new SqlConnection(DbConnection.ConnectionString)
				: DbConnection as SqlConnection;

			var cmd = new SqlCommand
			{
				Connection = conn
			};
			object returnValue;
			try
			{
				if (cmMode == ConnManagementMode.Auto)
					conn.Open();

				cmd.Transaction = conn.BeginTransaction();
				try
				{
					returnValue = handler(cmd, paramDatas);
					cmd.Transaction.Commit();
				}
				catch
				{
					cmd.Transaction.Rollback();
					throw;
				}
			}
			finally
			{
				if (cmMode == ConnManagementMode.Auto)
					conn.Close();
			}

			return returnValue;
		}

		#endregion Methods
	}
}