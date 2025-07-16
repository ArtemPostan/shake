using UnityEngine;

public class RoomsEventsExample : MonoBehaviour
{
	public void StartedRoomTransition(int currentRoom, int previousRoom)
	{
		UnityEngine.Debug.Log($"Started Room Transition - Current Room:{currentRoom} - Previous Room:{previousRoom}");
	}

	public void FinishedRoomTransition(int currentRoom, int previousRoom)
	{
		UnityEngine.Debug.Log($"Finished Room Transition - Current Room:{currentRoom} - Previous Room:{previousRoom}");
	}
}
