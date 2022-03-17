using System;
using System.Collections.Generic;
using System.IO;
using static Visuals.LocalizationStorage;
using Debug = UnityEngine.Debug;

namespace Visuals
{
    class LocalizationParse
    {
        public void Clear()
		{
            GetLanguages().Clear();
            GetCategories().Clear();
            GetAllKeys().Clear();
            for (int i = 0; i < GetAllKeys().Count; i++)
            { 
                GetKeys(i).Clear(); 
            }

            GetLocalization().Clear();
            ClearAllTypes();

            Save();
        }
        public void Run(List<String> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(data[i]);
                int indexOf = GetCategories().IndexOf(fileNameWithoutExtension);
                if (indexOf == -1)
                {
                    Localization category = new Localization();
                    var values = Parse(data[i]);
                    if (values != null && values.Count > 0)
                    {
                        category = Calculate(values, GetCategories().Count, category);
                        category.categoryName = fileNameWithoutExtension;
                        GetCategories().Add(fileNameWithoutExtension);
                        GetLocalization().Add(category);
                    }
                    else
                    {
                        Debug.Log("No data found.");
                    }
                }
			}
            SetCategoriesByType();
            Save();
        }
        private string FindFileWithName(List<String> data, string name)
        {
            for (int i = 0; i < data.Count; i++)
            { 
                if(Path.GetFileNameWithoutExtension(data[i]) == name)
                {
                    return data[i];
                }
            }
            return null;
        }
        private Localization Calculate(List<List<string>> values, int i, Localization category)
        {
            for (int j = 0; j < values.Count; j++)
            {
                KeyItem key = new KeyItem();

                for (int k = 0; k < values[j].Count; k++)
                {
                    if (i == 0 && j == 0 && k >= 2)
                    {
                        string languageValue = values[j][k].ToString();
                        if (!GetLanguages().Contains(languageValue))
                        {
                            GetLanguages().Add(languageValue);
                        }
                    }
                    else if (j >= 1)
                    {
                        if (k == 0)
                        {
                            if (i >= GetAllKeys().Count)
                            {
                                Key keys = new Key();
                                keys.list = new List<string>();
                                if (!GetAllKeys().Contains(keys))
                                {
                                    GetAllKeys().Add(keys);
                                }
                            }
                            string keyName = values[j][k].ToString();
                            if (!GetKeys(i).Contains(keyName))
                            {
                                GetKeys(i).Add(keyName);
                                key.name = keyName;
                            }
                        }
                        else
                        {
                            if (k == 1)
                            {
                                key.type = values[j][k].ToString();
                                AddTypes(key.type);
                            }
                            else
                            {
                                if (key.value == null) key.value = new List<string>();
                                string itemValue = values[j][k].ToString();
                                key.value.Add(itemValue);
                                
                            }
                        }
                    }
                }

                if (j >= 1)
                {
                    if (category.keys == null)
                    {
                        category.keys = new List<KeyItem>();
                    }
                    if (!category.keys.Contains(key))
                    {
                        category.keys.Add(key);
                    }
                }
            }

            return category;
        }
        public void Update(List<String> data)
        {
            List<string> allCategories = GetCategories();
            
            for (int i = 0; i < allCategories.Count; i++)
            {
                var currentData = FindFileWithName(data, allCategories[i]);
                if(currentData != null)
                {
                    Localization category = new Localization();
                    List<List<string>> values = Parse(currentData);
                    if (values != null && values.Count > 0)
                    {
                        GetKeys(i).Clear();
                        category = Calculate(values, i, category);
                        category.categoryName = Path.GetFileNameWithoutExtension(currentData); 
                        GetLocalization()[i].keys.Clear();
                        GetLocalization()[i] = category;
                    }
                    else
                    {
                        Debug.Log("No data found.");
                    }
                }
            }
            SetCategoriesByType();

            Save();
        }

        private static bool lineBreak = false;
        public static List<List<string>> values = new List<List<string>>();
        private static string lastCells;

        private static List<List<string>> Parse(string dataPath)
        {
            string[] text = File.ReadAllLines(dataPath);
            values.Clear();
            lineBreak = false;
            for (int i = 0; i < text.Length; i++)
            {
                List<String> cells = new List<string>();
                if (lineBreak)
                {
                    var x = ParseLine(string.Concat(lastCells, text[i]));
                    cells = x.Item1;
                    lineBreak = x.Item2;
                }
                else
                {
                    var x = ParseLine(text[i]);
                    cells = x.Item1;
                    lineBreak = x.Item2;
                }

                if (lineBreak)
                {
                    text[i] += "\n";
                    lastCells = string.Concat(lastCells, text[i]);
                }

                if (!lineBreak)
                {
                    values.Add(cells);
                    lastCells = "";
                }
            }

            for (int i = 0; i < values.Count; i++)
            {
                for (int j = 0; j < values[i].Count; j++)
                {
                    values[i][j] = values[i][j].Replace("\"", "").Replace("~", "\"");
                }
            }

            return values;
        }

        private static (List<string>, bool) ParseLine(string line)
        {
            List<string> cells = new List<string>();
            bool lineBreak = false;
            bool quotationMarks = false;
            var result = (cells, lineBreak);

            line = line.Replace("\"\"", "~");
            string newCell = "";

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '"')
                {
                    quotationMarks = !quotationMarks;
                }

                if (quotationMarks || line[i] != ',')
                {
                    newCell += line[i];
                }
                else
                {
                    cells.Add(newCell);
                    newCell = "";
                }
            }

            if (newCell != "" && !quotationMarks)
            {
                cells.Add(newCell);
            }
            else
            {
                result.lineBreak = quotationMarks;
            }

            result.cells = cells;

            return result;
        }
    }
}