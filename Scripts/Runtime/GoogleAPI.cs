﻿#if GOOGLE_LIB
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
#endif
using System.Collections.Generic;
using System.IO; 
using System.Threading;
using UnityEngine;

namespace Visuals
{
    class GoogleAPI
    {
#if GOOGLE_LIB
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Visuals Localization";

        public SheetsService Initialization()
        {
            UserCredential credential;
            string pathLocalization = Application.streamingAssetsPath;

            using (var stream = new FileStream(pathLocalization + "/Localization/credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = pathLocalization;
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Debug.Log("Credential file saved to: " + credPath);
            }

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        public IList<Sheet> GetData(SheetsService service, string spreadsheetId)
        {
            var request = service.Spreadsheets.Get(spreadsheetId);
            var spreadsheet = request.Execute();
            var values = spreadsheet.Sheets;
            if (values != null && values.Count > 0)
            {
                return values;
            }
            else
            {
                Debug.Log("No data found.");
                return null;
            }
        }
#endif
    }
}
