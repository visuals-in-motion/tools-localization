using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Visuals
{
    public class VisualsLocalization
    {
        private static string spreadsheetId = string.Empty;
        private static SheetsService service = null;
        private static GoogleAPI googleAPI = null;
        private static LocalizationParse localizationParse = null;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialization()
        {
            spreadsheetId = LocalizationStorage.GetSpreadsheetId(); //"11i_6WnjQmM1hSNBFvWMZe3QAontokRoZD8h4cekLUOc";

            googleAPI = new GoogleAPI();
            localizationParse = new LocalizationParse();

            service = googleAPI.Initialization();

            Import(service, spreadsheetId);
        }

        public static void Import()
        {
            if (service == null)
            {
                Initialization();
            }
            else
            {
                Import(service, spreadsheetId);
            }
        }

        public static void Import(SheetsService service, string spreadsheetId)
        {
            var data = googleAPI.GetData(service, spreadsheetId);
            if (data != null)
            {
                localizationParse.Run(service, spreadsheetId, data);
            }
        }

        public static void Export()
        {
            Debug.LogError("Export don't work...");
        }
    }
}
