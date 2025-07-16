using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Skin : MonoBehaviour
{
	[Serializable]
	public struct SkinInfo
	{
		public string name;

		public SkinnedMeshRenderer skinnedMeshRenderer;

		public Gun gun;

		public bool ability_Floating;

		public bool ability_IgnoreExplosion;

		public bool ability_Dash;
	}

	public delegate void SkinEventHandler(string skinName);

	private Combat combat;

	private PlayerControl playerControl;

	public SkinInfo[] skins;

	private SkinInfo activeSkin;

	private int activeSkinIndex;

	private Transform[] bones;

	private Transform rootBone;

	public GameObject changeSkinParticle;

	public static event SkinEventHandler onSkinChanged;

	private void Awake()
	{
		Initialize();
	}

	private void Initialize()
	{
		combat = GetComponent<Combat>();
		playerControl = GetComponent<PlayerControl>();
		if (skins.Length != 0)
		{
			bones = skins[0].skinnedMeshRenderer.bones;
			rootBone = skins[0].skinnedMeshRenderer.rootBone;
			for (int i = 0; i < skins.Length; i++)
			{
				skins[i].skinnedMeshRenderer?.gameObject.SetActive(value: false);
			}
			SetSkin(DataManager.GetSkinName());
		}
	}

	public void SetSkin(int index)
	{
		if (skins.Length != 0)
		{
			index = Mathf.Clamp(index, 0, skins.Length - 1);
			activeSkin.skinnedMeshRenderer?.gameObject.SetActive(value: false);
			activeSkin = skins[index];
			if (GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps)
			{
				activeSkin.skinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
			}
			else
			{
				activeSkin.skinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.On;
			}
			if ((bool)activeSkin.skinnedMeshRenderer)
			{
				activeSkin.skinnedMeshRenderer.gameObject.SetActive(value: true);
				activeSkin.skinnedMeshRenderer.bones = bones;
				activeSkin.skinnedMeshRenderer.rootBone = rootBone;
			}
			if ((bool)activeSkin.gun)
			{
				combat.ChangeGun(activeSkin.gun);
			}
			playerControl.floatingMode = activeSkin.ability_Floating;
			playerControl.dashMode = activeSkin.ability_Dash;
			combat.ignoreExplosion = activeSkin.ability_IgnoreExplosion;
			DataManager.SetSkinName(activeSkin.name);
			activeSkinIndex = index;
			Skin.onSkinChanged?.Invoke(activeSkin.name);
			if ((bool)changeSkinParticle)
			{
				UnityEngine.Object.Instantiate(changeSkinParticle, base.transform.position, Quaternion.Euler(0f, 0f, 0f)).transform.SetParent(base.transform);
			}
		}
	}

	public void SetSkin(string name)
	{
		for (int i = 0; i < skins.Length; i++)
		{
			if (skins[i].name == name)
			{
				SetSkin(i);
				return;
			}
		}
		SetSkin(0);
	}
}
