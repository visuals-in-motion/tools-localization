#if GOOGLE_LIB
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
#endif
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;
using File = System.IO.File;

namespace Visuals
{
    public static class VisualsLocalization
    {
        public static event Action loadLocalization;
#if GOOGLE_LIB && UNITY_EDITOR_WIN
        private static SheetsService service = null;

        private static GoogleAPI googleAPI = null;
#endif
        private static LocalizationParse localizationParse = null;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialization()
        {
#if GOOGLE_LIB && UNITY_EDITOR_WIN
            googleAPI = new GoogleAPI();
            service = googleAPI.Initialization();
#endif
            localizationParse = new LocalizationParse();

#if GOOGLE_LIB && UNITY_EDITOR_WIN
            Import(service);
#else
            ImportSimple();
#endif

        }
        public static void Open()
		{
            Application.OpenURL(LocalizationStorage.GetSpreadsheetURL());
        }
        public static void Import()
        {
#if GOOGLE_LIB && UNITY_EDITOR_WIN
            if (service == null)
            {
                Initialization();
            }
            else
            {
                Import(service);
            }
#else
            ImportSimple();
#endif

        }
#if GOOGLE_LIB && UNITY_EDITOR_WIN
        public static async void Import(SheetsService service)
        {
            if (LocalizationStorage.GetSpreadsheetURL().Length > 0)
            {

                string spreadsheetId = GetIdFromURL(LocalizationStorage.GetSpreadsheetURL());
                LocalizationStorage.SetSpreadsheetId(spreadsheetId);

                IList<Sheet> data = googleAPI.GetData(service, spreadsheetId);
                List<SheetsInfo> items = data.Select(x => new SheetsInfo(x.Properties.SheetId, x.Properties.Title)).ToList();

                await Downloading(items);

                LoadLocalization();
                loadLocalization?.Invoke();
            }
        }
#else

public static void ImportSimple()
        {
            if (LocalizationStorage.GetSpreadsheetURL().Length > 0)
            {
                LoadLocalization();
                loadLocalization?.Invoke();
            }
        }

#endif

        public static async void UpdateExistingSheets()
		{
            await Downloading(LocalizationStorage.GetCategoriesInfo());
            UpdateLocalization();
        }
        public static void UpdateLocalization()
        {
            localizationParse = new LocalizationParse();

            List<string> files = GetLocalizationFiles();

            localizationParse.Update(files);
        }
        public static void LoadLocalization()
        {
            localizationParse = new LocalizationParse();
            localizationParse.Clear();
            List<string> files = GetLocalizationFiles();
            
            localizationParse.Run(files);
        }

#region Private methods
        private static async Task Downloading(List<SheetsInfo> states)
        {
            List<SheetsInfo> list = new List<SheetsInfo>(states);
            LocalizationStorage.SetCategoriesInfo(list);

            List<byte[]> taskList = await DowloadFiles(list);

            for (int i = 0; i < taskList.Count; i++)
            {
                await ChangeLocalizationFilesInStreamingAssets(taskList[i], list[i].name);
            }
        }
        private static async Task<List<byte[]>> DowloadFiles(List<SheetsInfo> list)
		{
            List<Task<byte[]>> taskList = new List<Task<byte[]>>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].id != null)
                {
                    string request = GetSpreadsheetsRequest(list[i].id);
                    Task<byte[]> downloadAndCreateFilesTask = WebRequest.Get(request);
                    taskList.Add(downloadAndCreateFilesTask);
                }
            }
            await Task.WhenAll(taskList.ToArray());

            List<byte[]> result = taskList.Select(x => x.Result).ToList();
            return result;
        }
        private static async Task ChangeLocalizationFilesInStreamingAssets(byte[] bytesArray, string name)
		{
            string str = Encoding.Default.GetString(bytesArray);
            if (str.IndexOf("<!DOCTYPE html>") == -1)
            {
                string finalPath = Application.streamingAssetsPath + $"/Localization/{name}.csv";
                await File.WriteAllBytesAsync(finalPath, bytesArray);
            }
            else
            {
                Debug.LogError("Ошибка. Проверьте настройки доступа файла.");
            }
        }
        private static string GetIdFromURL(string url)
        {
            string id = string.Empty;
            int indexStart = url.IndexOf("/d/");
            if (indexStart >= 0)
            {
                url = url.Remove(0, indexStart + 3);
            }

            int indexEnd = url.IndexOf("/");
            if (indexEnd >= 0)
            {
                id = url.Remove(indexEnd);
            }
            return id;
        }
        private static List<string> GetAllFiles(string rootDirectory, string fileExtension)
        {
            List<string> files = new List<string>();
            files.AddRange(Directory.GetFiles(rootDirectory, fileExtension));
            return files;
        }
        private static List<string> GetLocalizationFiles()
        {
            return GetAllFiles($"{Application.streamingAssetsPath}/Localization", "*.csv");
        }
        private static string GetSpreadsheetsRequest(int? sheetsId)
        {
            return $"https://docs.google.com/spreadsheets/d/{LocalizationStorage.GetSpreadsheetId()}/export?format=csv&gid={sheetsId}";
        }
        private static void ClearLocalization(LocalizationParse localizationParse)
        {
            localizationParse.Clear();
        }
#endregion
    }
}