using UnityEngine;

public class GameController : MonoBehaviour
{
	public GameObject _door;
	public BoxCollider2D _doorBoxCollider;
	private ParagraphController _paragraphController;

	private void Awake()
	{
		_paragraphController = GetComponent<ParagraphController>();
	}

	public void ClickButton(string _paragraphName)
	{
		switch (_paragraphName)
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
		if (_paragraphName == "")
		{
			_door.SetActive(true);
		}
	}
}
