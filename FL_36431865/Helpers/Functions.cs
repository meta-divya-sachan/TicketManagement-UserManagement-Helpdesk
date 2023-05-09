using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TicketManagement.Helpers
{
    public class Functions
    {
        // Depurar carpeta
        public static void DepurarCarpeta(string p_folder, int p_minutos)
        {
            var folder = new DirectoryInfo(p_folder);
            foreach (var file in folder.GetFiles())
            {
                if ((DateTime.Now - file.CreationTime).TotalMinutes > p_minutos)
                {
                    file.Delete();
                }
            }
        }

        // Convert object to datatable
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor descriptor in properties)
            {
                table.Columns.Add(descriptor.Name, Nullable.GetUnderlyingType(descriptor.PropertyType) ?? descriptor.PropertyType);
            }
            foreach (T local in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor descriptor2 in properties)
                {
                    row[descriptor2.Name] = descriptor2.GetValue(local) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }

        // Convertir tabla a lista
        public static List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                    .Select(c => c.ColumnName)
                    .ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                    {
                        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                        pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], pI.PropertyType));
                    }
                }
                return objT;
            }).ToList();
        }

        // Copiar objetos
        public class PropertyCopier<TParent, TChild> where TParent : class where TChild : class
        {
            public static void Copy(TParent parent, TChild child)
            {
                var parentProperties = parent.GetType().GetProperties();
                var childProperties = child.GetType().GetProperties();

                foreach (var parentProperty in parentProperties)
                {
                    foreach (var childProperty in childProperties)
                    {
                        if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
                        {
                            childProperty.SetValue(child, parentProperty.GetValue(parent));
                            break;
                        }
                    }
                }
            }
        }

        // Deserialize object
        public static T Deserialize<T>(string p_jsonData)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<T>(p_jsonData, settings);
        }

        // Checkbox to bool
        public static bool CheckBoxToBool(string cbVal)
        {

            if (string.Compare(cbVal, "false") == 0)
                return false;
            if (string.Compare(cbVal, "true,false") == 0)
                return true;
            else
                throw new ArgumentNullException(cbVal);
        }

        // Return datetime
        public static DateTime ToDateTime(object value)
        {
            if (value == System.DBNull.Value)
                return new DateTime(1900, 1, 1);
            else if (value == null)
                return new DateTime(1900, 1, 1);
            else
            {
                if (value is DateTime time)
                    return time;
                else
                    return StringToDateTime(value);
            }
        }

        // Return time
        public static DateTime ToTime(object value)
        {
            if (value == System.DBNull.Value)
                return new DateTime(1900, 1, 1);
            else if (value == null)
                return new DateTime(1900, 1, 1);
            else
            {
                var lista = value.ToString().Split(':');
                if (lista.Length > 0)
                {
                    int hours = 0; if (lista.Length >= 1) hours = Functions.ToInt(lista[0]);
                    int minutes = 0; if (lista.Length >= 2) minutes = Functions.ToInt(lista[1]);
                    int seconds = 0; if (lista.Length >= 3) seconds = Functions.ToInt(lista[2]);
                    var time = new DateTime(1900, 1, 1, hours, minutes, seconds); 
                    return time;
                }
                else
                    return StringToDateTime(value);
            }
        }

        // To cvs datetime
        public static DateTime ToCsvDateTime(object value)
        {
            if (value == System.DBNull.Value)
                return new DateTime(1900, 1, 1);
            else if (value == null)
                return new DateTime(1900, 1, 1);
            else
            {
                if (value is DateTime time)
                    return time;
                else
                {
                    string input = value.ToString().Trim();
                    if (input != "")
                    {
                        if (input.Contains("/") && input.Contains(":") && input.Contains(" "))
                        {
                            var xdate = input.Split(' ')[0].Split('/');
                            var xtime = input.Split(' ')[1].Split(':');

                            int xyear = int.Parse(xdate[2]) + 2000;
                            int xmonth = xdate.Length > 1 ? int.Parse(xdate[1]) : 0;
                            int xday = xdate.Length > 2 ? int.Parse(xdate[0]) : 0;

                            int xhours = int.Parse(xtime[0]);
                            int xminutes = xtime.Length > 1 ? int.Parse(xtime[1]) : 0;
                            int xseconds = xtime.Length > 2 ? int.Parse(xtime[2]) : 0;

                            return new DateTime(xyear, xmonth, xday, xhours, xminutes, xseconds);
                        } 
                        else
                            return new DateTime(1900, 1, 1);
                    }
                    else
                        return new DateTime(1900, 1, 1);
                }
            }
        }

        // Return datetime
        public static DateTime StringToDateTime(object value)
        {
            if (value == null)
                return new DateTime(1900, 1, 1);
            else
            {
                try
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-PE");
                    string input = value.ToString().Trim();
                    if (input.Contains("/"))
                    {
                        var list = input.Split(' ')[0].Split('/');
                        if (list.Length == 3)
                        {
                            if (list[0].Length == 4)
                            {
                                return new DateTime(
                                    int.Parse(list[0]),
                                    int.Parse(list[1]),
                                    int.Parse(list[2]));
                            }
                            else
                            {
                                return new DateTime(
                                    int.Parse(list[2]),
                                    int.Parse(list[1]),
                                    int.Parse(list[0]));
                            }
                        }
                        else
                            return new DateTime(1900, 1, 1);
                    }
                    else
                    if (input.Contains("-"))
                    {
                        var list = input.Split(' ')[0].Split('-');
                        if (list.Length == 3)
                        {
                            if (list[0].Length == 4)
                            {
                                return new DateTime(
                                    int.Parse(list[0]),
                                    int.Parse(list[1]),
                                    int.Parse(list[2]));
                            }
                            else
                            {
                                return new DateTime(
                                    int.Parse(list[2]),
                                    int.Parse(list[1]),
                                    int.Parse(list[0]));
                            }
                        }
                        else
                            return new DateTime(1900, 1, 1);
                    }
                    else if (input.Length == 8)
                    {
                        return new DateTime(
                            int.Parse(input.Substring(0, 4)),
                            int.Parse(input.Substring(4, 2)),
                            int.Parse(input.Substring(6, 2)));
                    }
                    else if (input.Length == 16)
                    {
                        return new DateTime(
                            int.Parse(input.Substring(0, 4)),
                            int.Parse(input.Substring(5, 2)),
                            int.Parse(input.Substring(8, 2)));
                    }
                    else if (string.IsNullOrEmpty(input))
                    {
                        return new DateTime(1900, 1, 1);
                    }
                    else
                    {
                        return new DateTime(1900, 1, 1);
                    }
                }
                catch
                {
                    return new DateTime(1900, 1, 1);
                }
            }
        }

        // Get Key
        public static string GetKey(object p_value, char p_char)
        {
            string svalue = p_value.ToString();
            if (svalue == null)
                return string.Empty;
            else
                return p_value.ToString().Trim().Split(p_char)[0];
        }

        // Mid
        public static string Mid(string data, int start, int length)
        {
            string output;
            if (string.IsNullOrEmpty(data))
                output = "";
            else if (start > data.Length)
                output = "";
            else
            {
                if (start + length > data.Length)
                    length = data.Length - start;

                output = data.Substring(start, length);
            }
            return output;
        }

        // Left
        public static string Left(string input, int count)
        {
            return input.Substring(0, Math.Min(input.Length, count));
        }

        // Right
        public static string Right(string input, int count)
        {
            return input.Substring(Math.Max(input.Length - count, 0), Math.Min(count, input.Length));
        }

        // Return string
        public static string ToString(object value)
        {
            if (value == null)
                return string.Empty;
            else
                return value.ToString().Trim();
        }

        // Return decimal
        public static decimal ToDecimal(object value)
        {
            if (value == null)
                return 0;
            else
            {
                if (decimal.TryParse(value.ToString().Trim().Replace(",", ""), out decimal outResult))
                    return outResult;
                else
                    return 0;
            }
        }

        // Return Int
        public static int ToInt(object value, int default_ = 0)
        {
            int result = 0;
            if (default_ != 0)
                result = default_;

            if (value == null)
                return result;
            else
            {
                if (int.TryParse(value.ToString().Trim().Replace(",", ""), out int outResult))
                    return outResult;
                else
                    return result;
            }
        }

        // Return Long
        public static long ToLong(object value)
        {
            if (value == null)
                return 0;
            else
            {
                if (long.TryParse(value.ToString().Trim().Replace(",", ""), out long outResult))
                    return outResult;
                else
                    return 0;
            }
        }

        // Return boolean
        public static bool ToBool(object value)
        {
            if (value == null)
                return false;
            else
            {
                if (bool.TryParse(value.ToString().Trim(), out bool outResult))
                    return outResult;
                else
                {
                    if (value.ToString() == "1" || value.ToString().ToUpper() == "TRUE")
                        return true;
                    else
                        return false;
                }
            }
        }
    }
}
