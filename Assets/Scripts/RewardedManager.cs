using UnityEngine;
using System;
using GamePush;

public class RewardedManager : MonoBehaviour
{
    public static RewardedManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        GP_Ads.OnAdsStart += OnRewardedStart;
        GP_Ads.OnAdsClose += OnRewardedClose;
        GP_Ads.OnRewardedReward += OnRewardedReward;
    }

    private void OnDisable()
    {
        GP_Ads.OnAdsStart -= OnRewardedStart;
        GP_Ads.OnAdsClose -= OnRewardedClose;
        GP_Ads.OnRewardedReward -= OnRewardedReward;
    }   
    public void ShowRewardedAdRevive()
    {
        if (GP_Ads.IsRewardedAvailable())
        {
            GP_Ads.ShowRewarded("REVIVE", OnRewardedReward, OnRewardedStart, OnRewardedClose);
        }
        else
        {
            Debug.Log("Rewarded ads not available");
        }
    }

    public void ShowRewardedAdOpenSurviveMode()
    {
        if (GP_Ads.IsRewardedAvailable())
        {
            GP_Ads.ShowRewarded("SURVIVE", OnRewardedReward, OnRewardedStart, OnRewardedClose);
        }
        else
        {
            Debug.Log("Rewarded ads not available");
        }
    }

    private void OnRewardedStart()
    {
        Debug.Log("Rewarded ad started");
    }

    private void OnRewardedReward(string rewardTag)
    {
        Debug.Log($"User earned reward: {rewardTag}");
        if (rewardTag == "REVIVE")
        {
            // воскрешение игрока
            GameManager.Instance.LevelManager.Ressurect();
        }
        if (rewardTag == "SURVIVE")
        {
            // открытие разового достпа к режиму
            DataManager.GrantSurviveModeTempAccess();
        }
    }

    private void OnRewardedClose(bool success)
    {
        Debug.Log("Rewarded ad closed, success = " + success);
        
    }
}
