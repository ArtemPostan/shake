using UnityEngine;

public class Button_QuitGame : ButtonBehaviour
{
	protected override void OnClick()
	{
		Application.Quit();
	}
}
