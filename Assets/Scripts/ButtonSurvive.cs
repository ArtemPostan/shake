using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSurvive : ButtonBehaviour
{
    protected override void OnClick()
    {
        if (SceneManager.GetActiveScene().buildIndex != 10)
        {
            GameManager.Instance.LevelManager.gameMode = LevelManager.gameModes.survive;
            SceneManager.LoadScene(10); return;
        }
        
        LevelManager.Resume();
    }
}
