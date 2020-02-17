using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Visuals
{
    class GoogleAPI
    {
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Visuals Localization";

        public SheetsService Initialization()
        {
            UserCredential credential;
            string pathLocalization = Application.streamingAssetsPath + "/Localization";

            using (var stream = new FileStream(pathLocalization + "/credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = pathLocalization;
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
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
    }
}
