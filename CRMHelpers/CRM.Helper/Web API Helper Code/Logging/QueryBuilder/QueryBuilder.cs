using System;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json;
using CRM.Helper.Web_API_Helper_Code.Logging.Models;

namespace CRM.Helper.Web_API_Helper_Code.Logging.QueryBuilder
{
	public class QueryBuilder : Adapter
	{
		private FilteredRequest request;
		private string SQL;
		private Boolean WhereSet = false;

		public QueryBuilder() : base()
		{

		}

		public QueryBuilder(FilteredRequest request)
		{
			this.request = request;
			this.SQL = QueryStubs.getFilteredResults();
		}


		public static FilteredRequest formatTimestamps(FilteredRequest request)
		{
			if  (!string.IsNullOrEmpty(request.dateFrom))
			{
				if (request.timeFrom != null)
				{
					request.dateFrom = $"{request.dateFrom} {request.timeFrom["hour"]}:{request.timeFrom["minute"]}:{request.timeFrom["second"]}";

					if (request.timeTo == null)
					{
						string defaultTimeTo = "23:59:59";
						request.dateTo = $"{request.dateTo} {defaultTimeTo}";
					}
					else
					{
						request.dateTo = $"{request.dateTo} {request.timeTo["hour"]}:{request.timeTo["minute"]}:{request.timeTo["second"]}";
					}
				}

			}

			return request;
		}

		public QueryBuilder BuildQuery()
		{

			if (NotEmpty(request.selectedItems))
			{
				if (NotEmpty(request.selectedItems.levels))
				{
					SetWhereConditionIn(request.selectedItems.levels, "level");
				}

				if (NotEmpty(request.selectedItems.sources))
				{
					SetWhereConditionIn(request.selectedItems.sources, "source");
				}

				if (NotEmpty(request.selectedItems.users))
				{
					SetWhereConditionIn(request.selectedItems.users, "useropr");
				}

			}

			if (NotEmpty(request.requestContains))
			{
				SetWhereConditionLike("request", request.requestContains);
			}

			if (NotEmpty(request.responseContains))
			{
				SetWhereConditionLike("response", request.responseContains);
			}

			if (NotEmpty(request.dateFrom))
			{
				SetDateCondition(request.dateFrom, request.dateTo);
			}

			return this;
		}

		public string GetFilteredResultSet()
		{
			using (this.connection)
			{
				try
				{
					this.connection.Open();
					SqlCommand cmd = new SqlCommand(this.SQL.Replace("[WHERES]",
						" ORDER BY Date ASC"), this.connection);
					SqlDataReader reader = cmd.ExecuteReader();
					ArrayList rows = new ArrayList();

					while (reader.Read())
					{
						rows.Add(new
						{
							Date = reader["Date"],
							Thread = reader["Thread"],
							Level = reader["Level"],
							Message = reader["Message"],
							UserOpr = reader["UserOpr"],
							Source = reader["Source"],
							Request = reader["Request"],
							Response = reader["Response"],
							Stacktrace = reader["Stacktrace"],
							Exception = reader["Exception"],
							Expanded = 0
						});
					}
					reader.Close();
					return JsonConvert.SerializeObject(rows);
				}
				catch (Exception e)
				{
					return e.Message;
				}
			}
		}

		public List<string> GetDistinctColumnValues(string column)
		{
			List<string> values = new List<string>();
			using (this.connection)
			{
				try
				{
					this.connection.Open();
					SqlCommand cmd = new SqlCommand(QueryStubs.getDistinctColumn(column, this.dbName), this.connection);
					SqlDataReader reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						values.Add(reader[0].ToString());
					}
					reader.Close();
				}
				catch (Exception ex)
				{
				}
			}
			return values;
		}

		public void SetDateCondition(string dateFrom, string dateTo)
		{
			string where = !WhereSet ? "WHERE " : " AND ";
			where += $"Date BETWEEN '{dateFrom}' AND '{dateTo}'";
			this.SQL = this.SQL.Replace("[WHERES]", $"{where}[WHERES]");
			this.WhereSet = true;
		}

		public void SetWhereConditionLike(string condition, string value)
		{
			string where = !WhereSet ? "WHERE " : " AND ";
			where += $"{condition} LIKE '%{value}%'";
			this.SQL = this.SQL.Replace("[WHERES]", $"{where}[WHERES]");
			this.WhereSet = true;

		}

		public void SetWhereConditionIn(List<SelectedItemList> itemList, string condition)
		{
			string items = "";
			string where = !WhereSet ? "WHERE " : " AND ";
			where += $"{condition} in (";
	

			for (var i = 0; i < itemList.Count; i++)
			{

				if (i < itemList.Count - 1)
				{
					items += $"'{itemList[i].val}' ,";
				}
				else
				{
					items += $"'{itemList[i].val}')";
				}

			}

			where += items;
			this.SQL = this.SQL.Replace("[WHERES]", $"{where}[WHERES]");
			this.WhereSet = true;

		}

		private Boolean NotEmpty(object val)
		{
			return (val != null ? true : false);
		}
	}

	public static class QueryStubs
	{
		

		public static string getDistinctColumn(string column, string db)
		{
			return $"SELECT DISTINCT({column}) from {db}";
		}

		public static string getFilteredResults()
		{
			return $"SELECT * FROM CRM_Logs [WHERES]";
		}
	}
}
