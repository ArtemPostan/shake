using System.Collections.Generic;
using UnityEngine;

public class SimpleSpinBlur : MonoBehaviour
{
	private Quaternion _rotationPrevious = Quaternion.identity;

	private Vector3 rotDelta_Prev = Vector3.one;

	private Mesh SSB_Mesh;

	[Range(1f, 128f)]
	[Tooltip("Motion Blur Amount")]
	public int shutterSpeed = 4;

	[Range(1f, 50f)]
	[Tooltip("Motion Blur Samples")]
	public int Samples = 8;

	private Queue<Quaternion> rotationQueue = new Queue<Quaternion>();

	public Material SSB_Material;

	[Range(-0.1f, 0.1f)]
	[Tooltip("Motion Blur Opacity")]
	public float alphaOffset;

	public AdvancedSettings advancedSettings;

	private void Start()
	{
		SSB_Mesh = GetComponent<MeshFilter>().mesh;
		SSB_Material.enableInstancing = advancedSettings.enableGPUInstancing;
	}

	private void Update()
	{
		if (rotationQueue.Count >= shutterSpeed)
		{
			rotationQueue.Dequeue();
			if (rotationQueue.Count >= shutterSpeed)
			{
				rotationQueue.Dequeue();
			}
		}
		rotationQueue.Enqueue(base.transform.rotation);
		if (Quaternion.Angle(base.transform.rotation, rotationQueue.Peek()) / (float)shutterSpeed >= advancedSettings.AngularVelocityCutoff)
		{
			if (advancedSettings.unitLocalScale)
			{
				for (int i = 0; i <= Samples; i++)
				{
					Graphics.DrawMesh(SSB_Mesh, base.transform.position, Quaternion.Lerp(rotationQueue.Peek(), base.transform.rotation, (float)i / (float)Samples), SSB_Material, 0, null, advancedSettings.subMaterialIndex);
				}
			}
			else
			{
				for (int j = 0; j <= Samples; j++)
				{
					Matrix4x4 matrix = Matrix4x4.TRS(base.transform.position, Quaternion.Lerp(rotationQueue.Peek(), base.transform.rotation, (float)j / (float)Samples), base.transform.localScale);
					Graphics.DrawMesh(SSB_Mesh, matrix, SSB_Material, 0, null, advancedSettings.subMaterialIndex);
				}
			}
			Color color = new Color(SSB_Material.color.r, SSB_Material.color.g, SSB_Material.color.b, Mathf.Abs(2f / (float)Samples + alphaOffset));
			SSB_Material.color = color;
		}
		else if (SSB_Material.color.a < 1f)
		{
			Color color2 = new Color(SSB_Material.color.r, SSB_Material.color.g, SSB_Material.color.b, 1f);
			SSB_Material.color = color2;
		}
	}
}
