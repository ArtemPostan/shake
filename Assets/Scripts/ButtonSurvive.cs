using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSurvive : ButtonBehaviour
{
    protected override void OnClick()
    {
        if (SceneManager.GetActiveScene().name != "LevelSurvive")
        {
            GameManager.Instance.LevelManager.gameMode = LevelManager.gameModes.survive;
            SceneManager.LoadScene("LevelSurvive"); return;
        }
        
        LevelManager.Resume();
    }
}
