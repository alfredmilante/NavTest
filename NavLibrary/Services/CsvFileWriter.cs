using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NavLibrary
{

    /// <summary>
    /// Class to write data to a CSV file
    /// </summary>
    public class CsvFileWriter
    {
        string filePath;
        public CsvFileWriter(string filename)
        {
            this.filePath = filename;
        }

        /// <summary>
        /// Writes a single row to a CSV file.
        /// </summary>
        /// <param name="row">The row to be written</param>
        public void WriteRow(List<string> row)
        {
            StringBuilder builder = new StringBuilder();
            bool firstColumn = true;
            foreach (string value in row)
            {
                // Add separator if this isn't the firsCt value
                if (!firstColumn)
                    builder.Append(',');
                // Implement special handling for values that contain comma or quote
                // Enclose in quotes and double up any double quotes
                if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                    builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                else
                    builder.Append(value);
                firstColumn = false;
            }
            var rowText = builder.ToString();
            File.AppendAllText(this.filePath, "\n" + rowText);
        }
    }

    public class CsvFileReader
    {
        string filePath;
        public CsvFileReader(string filename)
        {
            this.filePath = filename;
        }

        /// <summary>
        /// Writes a single row to a CSV file.
        /// </summary>
        /// <param name="row">The row to be written</param>
        public List<string> ReadCsv()
        {
            using (var reader = new StreamReader(this.filePath))
            {
                List<string> list = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    list.Add(line);
                }

                return list;
            }
        }
    }
}