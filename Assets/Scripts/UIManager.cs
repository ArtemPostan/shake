using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	//для ПК
    [SerializeField]
    private GameObject SuccesPC, FailedPC;

    //для Mobile
    [SerializeField]
    private GameObject SuccesMob, FailedMob;

    [SerializeField]
    private Text remainAliveCountText, targetWaveCountText;

    //Для обычного режима
    [SerializeField]
    private Text remainTitle, targetTitle;     

    [SerializeField]
	private Canvas UiCanvas;

	[SerializeField]
	private Image healthBarImage;

    [SerializeField]
    private Image energyBarImage;

	[SerializeField] GameObject EnergyPanel;

	[SerializeField]
	private GameObject ReviveButton;

    private Material healthBarMat;	

	public Color redColor;

	public Color greenColor;

	[SerializeField]
	private CanvasGroup defeatCanvasGroup;

	[SerializeField]
	private Text defeatStringText;

	[SerializeField]
	private CanvasGroup successCanvasGroup;

	[SerializeField]
	private AnimationCurve overShowCurve;

	[SerializeField]
	private float overShowTime;

	[SerializeField]
	private GameObject hudPanel;


    [SerializeField]
    private GameObject MobilePanel;

    [SerializeField]
    private GameObject TimerPanel;

    [SerializeField]
    public Joystick JoystickUI;

    [SerializeField]
    public Joystick FireJoystickUI;

    private float overShowTimer;

	private Color tempColor;

	private bool overIsSuccess;

	private bool over;

	private void Awake()
	{
		healthBarMat = healthBarImage.material;
	}

	private void Start()
	{
		if (GameManager.Instance.isMobile)
		{
            isMobilePanelActive(true);
			SuccessFailedButtonTurnOn();

            return;
        }
        isMobilePanelActive(false);
    }

	public void Init(Camera _camera)
	{
		UiCanvas.worldCamera = _camera;
		UiCanvas.planeDistance = 1f;
		defeatCanvasGroup.alpha = 0f;
		defeatCanvasGroup.gameObject.SetActive(value: false);
		successCanvasGroup.alpha = 0f;
		successCanvasGroup.gameObject.SetActive(value: false);
		hudPanel.SetActive(value: true);
		if (GameManager.Instance.isMobile)
		{
			MobilePanel.SetActive(value: true);
		}
        over = false;
		overShowTimer = 0f;
	}

	public void UpdateCount(int _teamCount, int _remainCount, int _targetCount)
	{
        remainAliveCountText.text = _remainCount.ToString();
		if (GameManager.Instance.LevelManager.gameMode == LevelManager.gameModes.survive)
		{
            targetWaveCountText.text = _teamCount.ToString();
			return;
        }
        targetWaveCountText.text = _teamCount.ToString() + "/" + _targetCount.ToString();
		if (_teamCount >= _targetCount)
		{
            targetWaveCountText.color = greenColor;
		}
		else
		{
            targetWaveCountText.color = redColor;
		}
	}

	private void Update()
	{
		//if (GameManager.Instance.LevelManager.Player != null)
		//{
		//	healthBarMat.SetFloat("_CutValue", GameManager.Instance.LevelManager.Player.Combat.HealthPercent);
		//}
		if (over && overShowTimer < overShowTime)
		{
			overShowTimer += Time.unscaledDeltaTime;
			overShowTimer = Mathf.Min(overShowTimer, overShowTime);
			if (!overIsSuccess)
			{
				defeatCanvasGroup.alpha = overShowCurve.Evaluate(overShowTimer / overShowTime);
			}
			else
			{
				successCanvasGroup.alpha = overShowCurve.Evaluate(overShowTimer / overShowTime);
			}
		}
	}

	public void Defeat(string _defeatString)
	{
		overIsSuccess = false;
		defeatStringText.text = _defeatString;
		hudPanel.SetActive(value: false);
		over = true;
		defeatCanvasGroup.gameObject.SetActive(value: true);
	}

	public void Success()
	{
		overIsSuccess = true;
		hudPanel.SetActive(value: false);
		over = true;
		successCanvasGroup.gameObject.SetActive(value: true);
	}

	public void ChangeTextAlpha(Text _text, float alpha)
	{
		tempColor = _text.color;
		tempColor.a = alpha;
		_text.color = tempColor;
	}

	public void isMobilePanelActive(bool isOn)
	{
		if (GameManager.Instance.isMobile)
		{
			if (isOn)
			{           
                MobilePanel.SetActive(true); 
				return;

			}
			MobilePanel.SetActive(false);
			
        }
    }

	public void SuccessFailedButtonTurnOn()
	{
        SuccesMob.SetActive(true);
        FailedMob.SetActive(true);
		SuccesPC.SetActive(false);
		FailedPC.SetActive(false);
    }
	public void UpdateTitles(bool isSurvive)
	{
		if (isSurvive)
		{
			remainTitle.text = "ALIVE";
			targetTitle.text = "WAVE";
            TimerPanel.SetActive(false);
            EnergyPanel.SetActive(true);
        } else
		{
            remainTitle.text = "HOSTAGES";
            targetTitle.text = "TARGET";
            TimerPanel.SetActive(true);
            EnergyPanel.SetActive(false);
        }
	}

	public void ResetUI()
	{
		energyBarImage.fillAmount = 0;
		healthBarImage.fillAmount = GameManager.Instance.LevelManager.Player.Combat.maxHealth;
    }

	public void UpdateUI()
	{
        energyBarImage.fillAmount = GameManager.Instance.LevelManager.Player.Combat.EnergyPercent;
        healthBarImage.fillAmount = GameManager.Instance.LevelManager.Player.Combat.HealthPercent;

    }

	public void HideDefeatPanel()
	{
        hudPanel.SetActive(value: true);
        over = false;
        defeatCanvasGroup.gameObject.SetActive(value: false);
    }

	public void ShowReviveButton(bool isOn)
	{
		if (isOn)
		{
			ReviveButton.SetActive(true);
			return;
		}
        ReviveButton.SetActive(false);
    }

    public void OnReviveButtonClicked()
    {
        RewardedManager.Instance.ShowRewardedAdRevive();        // для тестирования воскрешения
        ShowReviveButton(false);
    }

    public void OnOpenSurviveModeButtonClicked()
    {
        RewardedManager.Instance.ShowRewardedAdOpenSurviveMode();
        //ShowOpenSurviveModeButton(false);
    }
}
