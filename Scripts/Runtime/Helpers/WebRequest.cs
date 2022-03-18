using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Visuals
{
	public class WebRequest
	{
		public static async Task<byte[]> Get(string url)
		{
			using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
			{
				var asyncOp = unityWebRequest.SendWebRequest();

				while (asyncOp.isDone == false)
				{
					await Task.Delay(1000 / 30);
				}

				if (unityWebRequest.result != UnityWebRequest.Result.Success)
				{
					Debug.Log(unityWebRequest.error);
					return null;
				}
				else
				{
					return unityWebRequest.downloadHandler.data;
				}
			}
		}

		public static async Task<AudioClip> GetAudioClip(string url)
		{
			using (UnityWebRequest unityWebRequest = UnityWebRequestMultimedia.GetAudioClip("file://" + url, AudioType.WAV))
			{
				var asyncOp = unityWebRequest.SendWebRequest();

				while (asyncOp.isDone == false)
				{
					await Task.Delay(1000 / 30);
				}

				if (unityWebRequest.result != UnityWebRequest.Result.Success)
				{
					Debug.Log(unityWebRequest.error);
					return null;
				}
				else
				{
					return DownloadHandlerAudioClip.GetContent(unityWebRequest);
				}
			}
		}
	}
}