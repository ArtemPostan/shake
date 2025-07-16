using UnityEngine;

public class SkinChanger : MonoBehaviour
{
	public LayerMask visibleLayers;

	public string skinName;

	public GameObject display;

	public GameObject displayAppearFXPrefab;

	public GameObject displayDisappearFXPrefab;

	private void ChangeSkin(Skin skin)
	{
		skin.SetSkin(skinName);
	}

	private void Awake()
	{
		Skin.onSkinChanged += OnSkinChange;
	}

	private void OnDestroy()
	{
		Skin.onSkinChanged -= OnSkinChange;
	}

	private void Start()
	{
		OnSkinChange(DataManager.GetSkinName());
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!(DataManager.GetSkinName() == skinName) && visibleLayers.Contains(other.gameObject.layer))
		{
			Skin component = other.gameObject.GetComponent<Skin>();
			if (component != null)
			{
				ChangeSkin(component);
			}
		}
	}

	private void OnSkinChange(string skinName)
	{
		if (skinName == this.skinName)
		{
			if ((bool)displayDisappearFXPrefab)
			{
				Object.Instantiate(displayDisappearFXPrefab, base.transform.position, base.transform.rotation, base.transform);
			}
			if ((bool)display)
			{
				display.SetActive(value: false);
			}
			AudioManager.PlaySFXAtPosition("SkinChange", base.transform.position);
		}
		else
		{
			if ((bool)displayAppearFXPrefab)
			{
				Object.Instantiate(displayAppearFXPrefab, base.transform.position, base.transform.rotation, base.transform);
			}
			if ((bool)display)
			{
				display.SetActive(value: true);
			}
		}
	}
}
