namespace TrevaliOperationalReport.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Web;
    using System.Xml.Serialization;
    public sealed class ConvertTo
    {
        #region Constructor

        /// <summary>
        /// Prevents a default instance of the ConvertTo class from being created.
        /// </summary>
        private ConvertTo()
        {
        }

        #endregion

        #region Variable/Property Declaration
        #endregion

        #region Methods/Functions

        /// <summary> 
        /// check for given value is null string 
        /// </summary> 
        /// <param name="readField">object to convert</param> 
        /// <returns>if value=string return string else ""</returns> 
        public static string String(object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    return Convert.ToString(readField, CultureInfo.InvariantCulture);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary> 
        /// check for given value is not double 
        /// </summary> 
        /// <param name="readField">object to convert</param> 
        /// <returns>if value=double return double else 0.0</returns> 
        public static double Double(object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    if (readField.ToString().Trim().Length == 0)
                    {
                        return 0.0;
                    }
                    else
                    {
                        return Convert.ToDouble(readField, CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    return 0.0;
                }
            }
            else
            {
                return 0.0;
            }
        }

        /// <summary> 
        /// check given value is boolean or null 
        /// </summary> 
        /// <param name="readField">object to convert</param> 
        /// <returns>return true else false</returns> 
        public static bool Boolean(object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    bool x;
                    if (bool.TryParse(Convert.ToString(readField, CultureInfo.InvariantCulture), out x))
                    {
                        return x;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary> 
        /// check given value is boolean or null 
        /// </summary> 
        /// <param name="readField">object to convert</param> 
        /// <returns>return true else false</returns> 
        public static bool? BoolNull(object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    bool x;
                    if (bool.TryParse(Convert.ToString(readField, CultureInfo.InvariantCulture), out x))
                    {
                        return x;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary> 
        /// check given value is integer or null 
        /// </summary> 
        /// <param name="readField">object to convert</param> 
        /// <returns>return integer else 0</returns> 
        public static int Integer(object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    if (readField.ToString().Trim().Length == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        int toReturn;
                        if (int.TryParse(Convert.ToString(readField, CultureInfo.InvariantCulture), out toReturn))
                        {
                            return toReturn;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary> 
        /// check given value is long or null 
        /// </summary> 
        /// <param name="readField">object to convert</param> 
        /// <returns>return long else 0</returns> 
        public static long Long(object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    if (readField.ToString().Trim().Length == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        long toReturn;
                        if (long.TryParse(Convert.ToString(readField, CultureInfo.InvariantCulture), out toReturn))
                        {
                            return toReturn;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary> 
        /// check given value is short or null 
        /// </summary> 
        /// <param name="readField">object to convert</param> 
        /// <returns>return short else 0</returns> 
        public static short Short(object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    if (readField.ToString().Trim().Length == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        short toReturn = 0;
                        if (short.TryParse(Convert.ToString(readField, CultureInfo.InvariantCulture), out toReturn))
                        {
                            return toReturn;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary> 
        /// check given value of date is date or null 
        /// </summary> 
        /// <param name="readField">date value to check</param> 
        /// <returns>return date if valid format else return nothing</returns> 
        public static DateTime? Date(object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    DateTime dateReturn;
                    if (DateTime.TryParse(Convert.ToString(readField, CultureInfo.CurrentCulture), out dateReturn))
                    {
                        //return Convert.ToDateTime(readField, CultureInfo.InvariantCulture);
                        return dateReturn;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        /// <summary> 
        /// check given value of date is date or null 
        /// </summary> 
        /// <param name="readField">date value to check</param> 
        /// <returns>return date if valid format else return nothing</returns> 
        public static string DateFormat(object readField)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    DateTime dateReturn;
                    if (DateTime.TryParse(Convert.ToString(readField, CultureInfo.CurrentCulture), out dateReturn))
                    {
                        return Convert.ToDateTime(readField, CultureInfo.InvariantCulture).GetDateTimeFormats('d', CultureInfo.InvariantCulture)[5];
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            return string.Empty;
        }

        /// <summary> 
        /// check given value of date is date or null 
        /// </summary> 
        /// <param name="readField">date value to check</param> 
        /// <param name="dateFormat">Date format</param> 
        /// <returns>return date if valid format else return nothing</returns> 
        public static string Date(object readField, string dateFormat)
        {
            if (readField != null)
            {
                if (readField.GetType() != typeof(System.DBNull))
                {
                    if (!string.IsNullOrEmpty(dateFormat))
                    {
                        return Convert.ToDateTime(readField, CultureInfo.CurrentCulture).ToString(dateFormat, CultureInfo.InvariantCulture);
                    }

                    return Convert.ToDateTime(readField, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
                }
            }

            return DateTime.MinValue.ToString(CultureInfo.CurrentCulture);
        }

        /// <summary> 
        /// for save null value in database 
        /// </summary> 
        /// <param name="value">object to convert</param> 
        /// <returns>return DBNull value</returns> 
        public static object DBNullValue(string value)
        {
            if (value == null | string.IsNullOrEmpty(value))
            {
                return System.DBNull.Value;
            }

            return value;
        }

        /// <summary>
        /// To check null value
        /// </summary>
        /// <param name="value">object to check</param>
        /// <returns>if null than returns DBNull.Value else returns object which is passed</returns>
        public static object ToDBNull(object value)
        {
            if (null != value)
            {
                return value;
            }

            return DBNull.Value;
        }

        /// <summary>
        /// Byteses the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] Bytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        /// <summary>
        /// Datas the table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <returns>DataTable.</returns>
        public static DataTable DataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            // Get all the properties
            List<PropertyInfo> props = new List<PropertyInfo>(typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance));
            props.ForEach(prop => dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType));
            items.ForEach(item =>
            {
                DataRow dr = dataTable.NewRow();
                props.ForEach(prop => dr[prop.Name] = prop.GetValue(item, null) ?? DBNull.Value);
                dataTable.Rows.Add(dr);
            });
            // put a breakpoint here and check datatable
            return dataTable;
        }


        /// <summary>
        /// Bytes the array from file.
        /// </summary>
        /// <param name="sFilePathAndName">Name of the s file path and.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ByteArrayFromFile(string sFilePathAndName)
        {
            byte[] buffer = null;
            if (System.IO.File.Exists(sFilePathAndName) == true)
            {
                using (FileStream fs = new FileStream(sFilePathAndName, FileMode.Open, FileAccess.Read))
                {
                    buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, (int)fs.Length);
                }
            }
            return buffer;
        }

        /// <summary>
        /// This function converts plain text to base64 string
        /// </summary>
        /// <param name="plainText">String that has to be converted to base64 string</param>
        /// <returns>base64 string</returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// This function converts base64 string to plain text
        /// </summary>
        /// <param name="base64EncodedData">Base64 Encoded Data</param>
        /// <returns>plain text string</returns>
        public static string Base64Decode(string base64EncodedData)
        {
            try
            {
                if (!string.IsNullOrEmpty(base64EncodedData))
                {
                    var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData.Replace(" ", "+"));
                    return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// This method generates xml data of object
        /// </summary>
        /// <param name="xmlserializer">XML Serializer object</param>
        /// <param name="objectToSerialize">Object to be serialized</param>
        /// <returns>string containing XML data</returns>
        public static string XMLfromObject(XmlSerializer xmlserializer, Object objectToSerialize)
        {
            var stringWriter = new UniCodeStringWriter();
            try
            {
                using (var writer = System.Xml.XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, objectToSerialize);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method generates object from xml string data
        /// </summary>
        /// <param name="xmlserializer">XML Serializer object</param>
        /// <param name="objectToDeSerialize">Object to be deserialized</param>
        /// <returns>string containing XML data</returns>
        public static Object ObjectfromXMLString(XmlSerializer xmlserializer, string stringXMLToDeSerialize)
        {
            var stringWriter = new UniCodeStringWriter();
            try
            {
                using (TextReader reader = new StringReader(stringXMLToDeSerialize))
                {
                    return xmlserializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Hashes the sh a1.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string HashSHA1(string value)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var inputBytes = Encoding.ASCII.GetBytes(value);
            var hash = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        #endregion
    }

    /// <summary>
    /// This class overrides default encoding property to null from UTF-16 of StringWriter
    /// </summary>
    public class UniCodeStringWriter : StringWriter
    {
        public override Encoding Encoding { get { return null; } }
    }
}