using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Visuals
{
    [ExecuteAlways]
    public class VisualsLocalizationAudio : VisualsLocalizationComponent
    {
        private AudioSource audioSource = null;
        private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

        public override string localizationType { get; } = "sound";

        protected override void Enable()
        {
            audioSource = GetComponent<AudioSource>();
        }
        public override void LocalizationChange(int languageIndex, bool changeValues)
        {
            if (!localizationEnable) return;

            localizationKey = GetKeyIndexByName(localizationKeyName);

            List<string> values = GetValuesByKeyIndex(localizationKey);

            if (values != null)
            {
                if (changeValues)
                {
                    LocalizationLoad();
                }
                SetValues(values[languageIndex]);
            }
        }
        public override async void LocalizationLoad()
        {
            if (!localizationEnable) return;
            localizationKey = GetKeyIndexByName(localizationKeyName);
            
            audioClips.Clear();

            for (int i = 0; i < LocalizationStorage.GetLanguages().Count; i++)
            {
                List<LocalizationStorage.KeyItem> keys = LocalizationStorage.GetLocalization()[localizationCategoryIndex].keys;
                if (localizationKey >= 0 && localizationKey < keys.Count)
                {
                    string fileName = keys[localizationKey].value[i];
                    string path = Application.streamingAssetsPath + "/" + fileName;
                    if (File.Exists(path))
                    {
                        AudioClip audioClip = await WebRequest.GetAudioClip(path);
                        if (!audioClips.ContainsKey(fileName))
                        {
                            audioClips.Add(fileName, audioClip);
                        }
                        // StartCoroutine(Load(path, fileName, i));
                    }
                }
            }
        }
        private IEnumerator Load(string path, string fileName)
        {
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.WAV);
            yield return request.SendWebRequest();

            if (request.isDone)
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
                if(!audioClips.ContainsKey(fileName))
				{
                    audioClips.Add(fileName, audioClip);
                }
            }
        }
        protected override void SetValues(string value)
        {
            if (audioSource != null)
            {
                if (audioClips.Count == 0)
                {
                    LocalizationLoad();
                }
				if (audioClips.ContainsKey(value))
				{
                    if (audioSource.clip != audioClips[value])
					{
                        int position = audioSource.timeSamples;
						bool isPlaying = audioSource.isPlaying;
						audioSource.clip = audioClips[value];
						if (isPlaying && audioClips[value].samples > position)
						{
                            Debug.LogError("audioSource.Play();");
							audioSource.timeSamples = position;
                            audioSource.Play();
                        }
					}
				}
			}
        }
        public void PlayAudioSource()
		{
            audioSource.Play();
        }
        public void StopAudioSource()
		{
            audioSource.Stop();
		}
        public AudioSource GetAudioSource()
		{
            return audioSource;
		}
    }
}