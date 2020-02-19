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
        private static SheetsService service = null;
        private static GoogleAPI googleAPI = null;
        private static LocalizationParse localizationParse = null;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialization()
        {
            googleAPI = new GoogleAPI();
            localizationParse = new LocalizationParse();

            service = googleAPI.Initialization();

            Import(service);
        }

        public static void Import()
        {
            if (service == null)
            {
                Initialization();
            }
            else
            {
                Import(service);
            }
        }

        public static void Import(SheetsService service)
        {
            if (LocalizationStorage.GetSpreadsheetId().Length > 0) //https://docs.google.com/spreadsheets/d/1WYJXF0GsAm4OsfTxF9J_WaKqwJ4qs3e1Ifg2FTDL91o/edit#gid=0
            {
                string spreadsheetId = LocalizationStorage.GetSpreadsheetId();

                int indexStart = spreadsheetId.IndexOf("/d/");
                if (indexStart >= 0)
                {
                    spreadsheetId = spreadsheetId.Remove(0, indexStart + 3);
                }

                int indexEnd = spreadsheetId.IndexOf("/");
                if (indexEnd >= 0)
                {
                    spreadsheetId = spreadsheetId.Remove(indexEnd);
                }

                var data = googleAPI.GetData(service, spreadsheetId);
                if (data != null)
                {
                    localizationParse.Run(service, spreadsheetId, data);
                }
            }
        }

        public static void Export()
        {
            Debug.LogError("Export don't work...");
        }
    }
}
