using System;
using System.Threading.Tasks;
using Petri.Configs;
using UnityEngine;
using UnityEngine.Networking;

namespace Petri.Services
{
    public class SheetLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadConfigs()
        {
            LoadBacteriasConfig();
        }

        private static async void LoadBacteriasConfig()
        {
            #if UNITY_EDITOR
            try
            {
                var url = Constants.Constants.SHEETS_URL_CSV.Replace("*", Constants.Constants.BACTERIAS_CONFIG_ID);
                var loadedText = await LoadString(url);
                var barterias = CsvToJsonConverter.TryConvert<BacteriaData>(loadedText);
            }
            catch (Exception e)
            {
                throw; // TODO handle exception
            }
            #endif
        }

        private static async Task<string> LoadString(string url)
        {
            var request = UnityWebRequest.Get(url);
            await request.SendWebRequest();

            if (!string.IsNullOrEmpty(request.error))
            {
                throw new NullReferenceException(request.error);
            }

            return request.downloadHandler.text;
        }
    }
}