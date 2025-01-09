using UnityEngine;
using System.Collections;
using UnityEngine.Video;

public class GameController_1_2 : MonoBehaviour
{
	[Header("Scene 1-2")]
	public GameObject _door;
	public BoxCollider2D _doorBoxCollider;

	public void ClickButton(string paragraphName)
	{
		Debug.Log("ClickButton");
		switch (paragraphName)
		{
			case "4-4_You":
			case "5-7_Celeste":
				_door.SetActive(true);
				// _doorBoxCollider.enabled = false;
				break;

			case "OpenTheDoor":
				_doorBoxCollider.enabled = true;
				break;

			default:
				break;
		}
		// if (_paragraphName == "")
		// {
		// 	_door.SetActive(true);
		// }
	}
}
