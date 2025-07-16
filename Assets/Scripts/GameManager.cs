using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;

	private LevelManager levelManager;

	private LayerManager layerManager;

	private ParticleManager particleManager;

	private PhysicsManager physicsManager;

	private PostManager postManager;

	private TimeScaleManager timeScaleManager;

	private BoomManager boomManager;

	private UIManager uiManager;

	private CameraManager cameraManager;

	private bool inited;

	public static GameManager Instance => instance;

	public LevelManager LevelManager => levelManager;

	public LayerManager LayerManager => layerManager;

	public ParticleManager ParticleManager => particleManager;

	public PhysicsManager PhysicsManager => physicsManager;

	public PostManager PostManager => postManager;

	public TimeScaleManager TimeScaleManager => timeScaleManager;

	public BoomManager BoomManager => boomManager;

	public UIManager UIManager => uiManager;

	public CameraManager CameraManager => cameraManager;

	public int currentLevel => levelManager.levelInfo.currentLevelIndex;

	public void Init()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		instance = this;
		DataManager.SetSavesIndex(0);
		DataManager.Load();
		levelManager = GetComponent<LevelManager>();
		layerManager = GetComponent<LayerManager>();
		particleManager = GetComponent<ParticleManager>();
		physicsManager = GetComponent<PhysicsManager>();
		postManager = GetComponent<PostManager>();
		timeScaleManager = GetComponent<TimeScaleManager>();
		boomManager = GetComponent<BoomManager>();
		uiManager = GetComponent<UIManager>();
		cameraManager = GetComponent<CameraManager>();
		inited = true;
		Application.targetFrameRate = 144;
	}

	private void Awake()
	{
		if (!inited)
		{
			Init();
		}
	}

	private void Update()
	{
	}

	public void DeleteDatas()
	{
		DataManager.DeleteSaveData();
	}

	public void Kill100()
	{
		for (int i = 0; i < 100; i++)
		{
			DataManager.CountEnemyKilled();
		}
		DataManager.Save();
	}
}
