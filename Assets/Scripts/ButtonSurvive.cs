using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSurvive : ButtonBehaviour
{
    [SerializeField] GameObject lockImage;

    private bool isLock = true;

    private void OnEnable()
    {
        base.OnEnable();
        if (DataManager.data.surviveModeUnlocked)
        {
            isLock = false;
            lockImage.SetActive(false);
        }
    }
    protected override void OnClick()
    {       
        if (SceneManager.GetActiveScene().buildIndex != 10)
        {
            if (!isLock)
            {
                GameManager.Instance.LevelManager.gameMode = LevelManager.gameModes.survive;
                SceneManager.LoadScene(10); return;
            } else
            {                
                RewardedManager.Instance.ShowRewardedAdOpenSurviveMode();
            }
           
        } else
        {
            LevelManager.Resume();
        }
        
    }
}

