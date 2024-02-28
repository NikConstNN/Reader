using System;
using System.Globalization;
using System.Text;

namespace Common
{
	/// <summary>
	/// Дополнительные типы данных
	/// </summary>
	public static class ExtType
	{
		/// <summary>
        /// формат даты из ISO стандарта
		/// </summary>
		public const string DateFormatYYYYMMDD = "yyyyMMdd";

		/// <summary>
		/// Значение Null для int type
		/// </summary>
		public const int INT_NULL = int.MinValue;

		/// <summary>
        /// Значение Null для Int64 type
		/// </summary>
		public const Int64 INT64_NULL = Int64.MinValue;

		/// <summary>
        /// Значение Null для DateTime type
		/// </summary>
		public static DateTime DATE_NULL = DateTime.MinValue;

		/// <summary>
        /// Значение Null для decimal type
		/// </summary>
		public static decimal DECIMAL_NULL = Decimal.MinValue;

		/// <summary>
        /// Значение Null для long type
		/// </summary>
		public static long LONG_NULL = long.MinValue;

		/// <summary>
        /// Значение Null для double type
		/// </summary>
		public static double DOUBLE_NULL = double.NaN;

		/// <summary>
        /// Значение Null для float type
		/// </summary>
		public static float FLOAT_NULL = float.NaN;

		private static CultureInfo cultureInfo = new CultureInfo("en-US");

        /// <summary>
        /// последняя папка открытая folderDialog (в текущем сеансе)
        /// </summary>
        public static string LastOpenFolder = "c:\\";

		/// <summary>
		/// Преобразование строки в Boolean.
		/// </summary>
		/// <param name="aValue">Сртока со значением '1', '0', 'Y', 'N', 'Yes', 'No', 'True', 'False', 'Д', 'Н', 'Да','Нет'. </param>
		/// <returns>true - да, false - нет или пустая стока</returns>
		public static bool StringToBool(string aValue)
		{
			if (!string.IsNullOrWhiteSpace(aValue))
			{
				string val = aValue.ToUpper();
                if (val == "1" || val == "Y" || val == "YES" || val == "TRUE" || val == "Д" || val == "ДА") 
				{
					return true;
				}
				else
				{
                    if (val == "0" || val == "N" || val == "NO" || val == "FALSE" || val == "Н" || val == "НЕТ")
					{
						return false;	
					}
                    throw new FormatException(string.Format("Переданная строка '{0}' не может быть интерпретирована как Boolean.", aValue));
				}
			}
			return false;
		}

		/// <summary>
		/// Overload method. Converts the specified aValue of the string type.
		/// </summary>
		/// <param name="aValue">A object representation of value. </param>
		/// <param name="aInsteadOfNull">A value if aValue is NULL.</param>
		/// <param name="aFormat">A format string.</param>
		/// <returns>The value as string</returns>
		public static string GetObjectValueAsString(object aValue, object aInsteadOfNull, string aFormat)
		{
			string strVal = "";
			string valueOfNull = (aInsteadOfNull == null ? null : aInsteadOfNull.ToString());
			if (aValue == null)
			{
				strVal = valueOfNull;
			}
			else
			{
				if (aValue is DateTime)
				{
					if ((DateTime)aValue == ExtType.DATE_NULL)
					{
					    strVal = valueOfNull;
					}
					else
					{
						strVal = ((DateTime)aValue).ToString(aFormat);
					}
				}
				else if (aValue is int)
				{
					if ((int)aValue == ExtType.INT_NULL)
					{
						strVal = valueOfNull;
					}
					else
					{
						strVal = ((int)aValue).ToString(aFormat);
					}
				}
				else if (aValue is Int64)
				{
					if ((Int64)aValue == ExtType.INT64_NULL)
					{
						strVal = valueOfNull;
					}
					else
					{
						strVal = ((Int64)aValue).ToString(aFormat);
					}
				}
				else if (aValue is decimal)
				{
					if ((decimal)aValue == ExtType.DECIMAL_NULL)
					{
						strVal = valueOfNull;
					}
					else
					{
						if (aFormat == null || aFormat.Equals(string.Empty))
						{
							strVal = ((decimal)aValue).ToString(cultureInfo.NumberFormat);
						}
						else
						{
							strVal = ((decimal)aValue).ToString(aFormat);
						}
					}
				} 
				else if (aValue is long)
				{
					if ((long)aValue == ExtType.LONG_NULL)
					{
						strVal = valueOfNull;
					}
					else
					{
						strVal = ((long)aValue).ToString(aFormat);
					}
				}
				else if (aValue is double)
				{
					if ((double)aValue == ExtType.DOUBLE_NULL)
					{
						strVal = valueOfNull;
					}
					else
					{
						if (aFormat == null || aFormat.Equals(string.Empty))
						{
							strVal = ((double)aValue).ToString(cultureInfo.NumberFormat);
						}
						else
						{
							strVal = ((double)aValue).ToString(aFormat);
						}
					}
				}
				else if (aValue is float)
				{
					if ((float)aValue == ExtType.FLOAT_NULL)
					{
						strVal = valueOfNull;
					}
					else
					{
						if (aFormat == null || aFormat.Equals(string.Empty))
						{
							strVal = ((float)aValue).ToString(cultureInfo.NumberFormat);
						}
						else
						{
							strVal = ((float)aValue).ToString(aFormat);
						}
					}
				}
				else if (aValue is Enum)
				{
					strVal = GetObjectValueAsString((int)aValue, valueOfNull, aFormat);
				}
				else if (aValue is byte[])
				{
					byte[] data = aValue as byte[];
					StringBuilder sBuilder = new StringBuilder();
					sBuilder.Append("0x");
					for (int i = 0; i < data.Length; i++)
					{
						sBuilder.Append(data[i].ToString(aFormat));
					}
					strVal = sBuilder.ToString();
				}
				else
				{
					strVal = aValue.ToString();
				}

			}

			return strVal;
		}

		/// <summary>
		/// Overload method. Converts the specified aValue of the string type.
		/// </summary>
		/// <param name="aValue">A object representation of value. </param>
		/// <param name="aInsteadOfNull">A value if aValue is NULL.</param>
		/// <returns>The value as string</returns>
		public static string GetObjectValueAsString(object aValue, object aInsteadOfNull)
		{
			return GetObjectValueAsString(aValue, aInsteadOfNull, null);
		}

		/// <summary>
		/// Return true if value is nulable.
		/// </summary>
		/// <param name="aValue">A checked value.</param>
		/// <returns>The bool value.</returns>
        public static bool IsNull(this object aValue)
		{
			bool result = aValue == null;
			if (!result)
			{
				if (aValue is DateTime)
				{
					result = (DateTime)aValue == DATE_NULL;
				}
				else if (aValue is int)
				{
					result = (int)aValue == INT_NULL;
				}
				else if (aValue is Int64)
				{
					result = (Int64)aValue == INT64_NULL;
				}
				else if (aValue is decimal)
				{
					result = (decimal)aValue == DECIMAL_NULL;
				}
				else if (aValue is long)
				{
					result = (long)aValue == LONG_NULL;
				}
				else if (aValue is double)
				{
					result = ((double)aValue).Equals(DOUBLE_NULL);
				}
				else if (aValue is float)
				{
					result = ((float)aValue).Equals(FLOAT_NULL);
				}
			}

			return result;
		}

		/// <summary>
		/// Method for compare two object values.
		/// </summary>
		/// <param name="aValue1"></param>
		/// <param name="aValue2"></param>
		/// <returns></returns>
		public static bool IsEquals(object aValue1, object aValue2)
		{
			bool result  = false;
			if (IsNull(aValue1) && IsNull(aValue2))
			{
				result = true;
			}
			else
			{
				if (aValue1 != null && aValue2 != null && aValue1.GetType().Equals(aValue2.GetType()) )
				{
					if (aValue1 is int)
					{
						result = (int)aValue1 == (int)aValue2;
					}
					else if (aValue1 is string)
					{
						result = (string)aValue1 == (string)aValue2;
					}

					// need continue.
				}
			}
			return result;
		}
	}
}
