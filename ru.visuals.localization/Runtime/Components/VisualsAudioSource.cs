using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Visuals
{
    [AddComponentMenu("Audio/Visuals Localization/Audio Source")]
    [RequireComponent(typeof(AudioSource))]
    [ExecuteInEditMode]
    public class VisualsAudioSource : MonoBehaviour, IEditor
    {
        private AudioSource audioSource = null;
        private List<AudioClip> audioClips = new List<AudioClip>();

        public bool localizationEnable { get; set; } = true;
        public string localizationType { get; } = "sound";
        public int localizationCategory { get; set; } = 0;
        public int localizationKey { get; set; } = 0;

        public bool saveEnable = true;
        public int saveCategory = 0;
        public int saveKey = 0;

        void Awake()
        {
            localizationEnable = saveEnable;
            localizationCategory = saveCategory;
            localizationKey = saveKey;

            audioSource = this.GetComponent<AudioSource>();

            LocalizationLoad();
        }

        public void LocalizationLoad()
        {
            if (!localizationEnable) return;

            if (audioClips.Count != LocalizationStorage.GetLanguages().Count)
            {
                audioClips.Clear();
                for (int i = 0; i < LocalizationStorage.GetLanguages().Count; i++)
                {
                    audioClips.Add(null);
                }
            }

            for (int i = 0; i < LocalizationStorage.GetLanguages().Count; i++)
            {
                string path = Application.streamingAssetsPath + "/" + LocalizationStorage.GetLocalization()[localizationCategory].keys[localizationKey].item.value[i];
                if (File.Exists(path))
                {
                    StartCoroutine(Load(path, i));
                }
            }
        }

        public void LocalizationChange(int index)
        {
            if (!localizationEnable) return;

            if (audioSource.clip != audioClips[index])
            {
                int position = audioSource.timeSamples;
                bool isPlaying = audioSource.isPlaying;
                audioSource.clip = audioClips[index];
                if (isPlaying && audioClips[index].samples > position)
                {
                    audioSource.timeSamples = position;
                    audioSource.Play();
                }
            }
        }

        private IEnumerator Load(string path, int index)
        {
            UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://"+ path, AudioType.WAV);
            yield return request.SendWebRequest();

            if (request.isDone)
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
                int position = audioSource.timeSamples;
                bool isPlaying = audioSource.isPlaying;
                audioClips[index] = audioClip;
                if (isPlaying && index == LocalizationStorage.GetCurrentLanguages() && audioClips[index].samples > position)
                {
                    LocalizationChange(index);
                    audioSource.timeSamples = position;
                    audioSource.Play();
                }
            }
        }

        private void Update()
        {
            if (!localizationEnable) return;

            if (LocalizationStorage.CheckType(localizationCategory, localizationKey, localizationType))
            {
                LocalizationChange(LocalizationStorage.GetCurrentLanguages());
            }
        }
    }
}
