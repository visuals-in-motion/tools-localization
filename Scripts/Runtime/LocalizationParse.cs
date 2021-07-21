using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Visuals.LocalizationStorage;

namespace Visuals
{
    class LocalizationParse
    {
#if GOOGLE_LIB
        public void Run(SheetsService service, string spreadsheetId, IList<Sheet> data)
        {
            GetLanguages().Clear();
            GetCategories().Clear();
            GetAllKeys().Clear();
            GetLocalization().Clear();

            for (int i = 0; i < data.Count; i++)
            {
                Localization category = new Localization();

                string range = data[i].Properties.Title + "!A:Z";
                var request = service.Spreadsheets.Values.Get(spreadsheetId, range);

                var response = request.Execute();
                var values = response.Values;
                if (values != null && values.Count > 0)
                {
                    for (int j = 0; j < values.Count; j++)
                    {
                        KeyItem key = new KeyItem();

                        Item item = new Item();

                        for (int k = 0; k < values[j].Count; k++)
                        {
                            if (i == 0 && j == 0 && k >= 2)
                            {
                                GetLanguages().Add(values[j][k].ToString());
                            }
                            else if (j >= 1)
                            {
                                if (k == 0)
                                {
                                    if (i >= GetAllKeys().Count) { Key keys = new Key(); keys.list = new List<string>(); GetAllKeys().Add(keys); }
                                    GetKeys(i).Add(values[j][k].ToString());
                                }
                                else
                                {
                                    if (k == 1)
                                    {
                                        item.type = values[j][k].ToString();
                                    }
                                    else
                                    {
                                        if (item.value == null) item.value = new List<string>();
                                        item.value.Add(values[j][k].ToString());
                                    }
                                }
                            }
                        }

                        if (j >= 1)
                        {
                            if (category.keys == null) category.keys = new List<KeyItem>();

                            key.item = item;
                            category.keys.Add(key);
                        }
                    }

                    GetCategories().Add(data[i].Properties.Title);
                    GetLocalization().Add(category);
                }
                else
                {
                    Debug.Log("No data found.");
                }
            }

            Save();
        }
#endif
    }
}
