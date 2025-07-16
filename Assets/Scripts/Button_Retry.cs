public class Button_Retry : ButtonBehaviour
{
	protected override void OnClick()
	{
		LevelManager.ReloadCurrentLevel();
	}
}
