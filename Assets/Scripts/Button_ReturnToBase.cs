using UnityEngine.SceneManagement;

public class Button_ReturnToBase : ButtonBehaviour
{
	protected override void OnClick()
	{
        GameManager.Instance.LevelManager.gameMode = LevelManager.gameModes.single;
        LevelManager.instance.TryLoadLevel(0);
	}
}
