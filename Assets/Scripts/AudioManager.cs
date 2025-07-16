using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	[Serializable]
	public struct StringAudioClipPair
	{
		public string name;

		[Range(0f, 1f)]
		public float volume;

		public AudioClip[] audioClip;
	}

	public static AudioManager instance;

	public AudioMixer mixer;

	public string masterVolumeParameter = "volume_Master";

	public string sfxVolumeParameter = "volume_SFX";

	public string bgmVolumeParameter = "volume_BGM";

	public AudioSource bgmSource;

	public AudioSource sfxSource;

	public float bgmAttenuation;

	public float sfxVolumeFactor = 0.75f;

	public Pool<SoundEffectsSource> soundEffectsPool;

	public StringAudioClipPair[] commonSoundEffects;

	private Dictionary<string, StringAudioClipPair> commonSoundEffectsDictionary = new Dictionary<string, StringAudioClipPair>();

	private float cached_PreferredMasterVolume;

	private float cached_PreferredSFXVolume;

	private float cached_PreferredBGMVolume;

	public static float BGMAttenuation
	{
		get
		{
			if ((bool)instance)
			{
				return instance.bgmAttenuation;
			}
			return 0f;
		}
		set
		{
			if ((bool)instance)
			{
				instance.bgmAttenuation = value;
			}
		}
	}

	private void Awake()
	{
		instance = this;
		commonSoundEffectsDictionary = new Dictionary<string, StringAudioClipPair>();
		StringAudioClipPair[] array = commonSoundEffects;
		foreach (StringAudioClipPair stringAudioClipPair in array)
		{
			commonSoundEffectsDictionary.Add(stringAudioClipPair.name, stringAudioClipPair);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		_UpdateMixerValues();
	}

	public float GetPreferredMasterVolume()
	{
		return PlayerPrefs.GetFloat("AudioConfig_" + masterVolumeParameter, 0f);
	}

	public void SetPreferredMasterVolume(float value)
	{
		PlayerPrefs.SetFloat("AudioConfig_" + masterVolumeParameter, value);
	}

	public float GetPreferredSFXVolume()
	{
		return PlayerPrefs.GetFloat("AudioConfig_" + sfxVolumeParameter, 12f);
	}

	public void SetPreferredSFXVolume(float value)
	{
		PlayerPrefs.SetFloat("AudioConfig_" + sfxVolumeParameter, value);
	}

	public float GetPreferredBGMVolume()
	{
		return PlayerPrefs.GetFloat("AudioConfig_" + bgmVolumeParameter, -13f);
	}

	public void SetPreferredBGMVolume(float value)
	{
		PlayerPrefs.SetFloat("AudioConfig_" + bgmVolumeParameter, value);
	}

	public static void PlaySFX(AudioClip clip, float volume = 1f)
	{
		if ((bool)instance)
		{
			instance.sfxSource.PlayOneShot(clip, volume * instance.sfxVolumeFactor);
		}
	}

	public static void PlaySFXAtPosition(AudioClip clip, Vector3 position, float volume = 1f, float randomPitchMin = 0f, float randomPitchMax = 0f)
	{
		if ((bool)instance && !(Time.timeSinceLevelLoad < 0.1f))
		{
			SoundEffectsSource orCreate = instance.soundEffectsPool.GetOrCreate();
			orCreate.transform.position = position;
			orCreate.master = instance.soundEffectsPool;
			orCreate.audioSource.volume = volume * instance.sfxVolumeFactor;
			orCreate.audioSource.pitch = 1f + UnityEngine.Random.Range(randomPitchMin, randomPitchMax);
			orCreate.PlayClip(clip);
		}
	}

	public static void PlaySFXAtPosition(string name, Vector3 position, float volume = 1f, float randomPitchMin = 0f, float randomPitchMax = 0f)
	{
		StringAudioClipPair value;
		if ((bool)instance && instance.commonSoundEffectsDictionary.TryGetValue(name, out value) && value.audioClip.Length != 0)
		{
			PlaySFXAtPosition(value.audioClip[UnityEngine.Random.Range(0, value.audioClip.Length)], position, volume * value.volume, randomPitchMin, randomPitchMax);
		}
	}

	public static void PlaySFX(string name)
	{
		StringAudioClipPair value;
		if ((bool)instance && instance.commonSoundEffectsDictionary.TryGetValue(name, out value) && value.audioClip.Length != 0)
		{
			PlaySFX(value.audioClip[UnityEngine.Random.Range(0, value.audioClip.Length)], value.volume);
		}
	}

	public static void PlayBGM(AudioClip clip)
	{
		if ((bool)instance && (!instance.bgmSource.isPlaying || !(instance.bgmSource.clip == clip)))
		{
			if ((bool)clip)
			{
				instance.bgmSource.clip = clip;
				instance.bgmSource.Play();
			}
			else
			{
				instance.bgmSource.Stop();
			}
		}
	}

	private void _CacheValues()
	{
		cached_PreferredMasterVolume = GetPreferredMasterVolume();
		cached_PreferredSFXVolume = GetPreferredSFXVolume();
		cached_PreferredBGMVolume = GetPreferredBGMVolume();
	}

	private void _UpdateMixerValues()
	{
		mixer.SetFloat(masterVolumeParameter, cached_PreferredMasterVolume);
		mixer.SetFloat(sfxVolumeParameter, cached_PreferredSFXVolume);
		mixer.SetFloat(bgmVolumeParameter, cached_PreferredBGMVolume + bgmAttenuation);
	}

	public static void RefreshValues()
	{
		if ((bool)instance)
		{
			instance._CacheValues();
		}
	}
}
