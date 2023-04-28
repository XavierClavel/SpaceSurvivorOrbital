using UnityEngine;
using System.IO;
using System.Text;

public class CsvParser
{
    string pathToCsv = "";


    public string loadText(string pathToText, string defaultText)
    {
        var strBuilder = new StringBuilder();
        if (File.Exists(pathToText))
        {
            StreamReader reader = new StreamReader(pathToText);

            while (!reader.EndOfStream)
            {
                strBuilder.Append(reader.ReadLine() + "\n");
            }

            reader.Close();
        }
        else
        {
            strBuilder.Append(defaultText);
        }
        return strBuilder.ToString();
    }
}

