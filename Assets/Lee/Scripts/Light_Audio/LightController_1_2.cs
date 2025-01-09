using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightController_1_2 : MonoBehaviour
{
	public GameObject _YouLight;
	public GameObject _LunaLight;
	public GameObject _CelesteLight;
	public GameObject _AriaLight;

    private ParagraphController_1_2 _paragraphController;
	
	private void Awake()
	{
		_paragraphController = GetComponent<ParagraphController_1_2>();
	}

	public void ChangeLight(string _speakerName)
	{

		switch (_speakerName)
		{
			case "You":
				_YouLight.SetActive(true);
				_LunaLight.SetActive(false);
				_CelesteLight.SetActive(false);
				_AriaLight.SetActive(false);
				break;

			case "Luna":
				_YouLight.SetActive(false);
				_LunaLight.SetActive(true);
				_CelesteLight.SetActive(false);
				_AriaLight.SetActive(false);
				break;

			case "Celeste":
				_YouLight.SetActive(false);
				_LunaLight.SetActive(false);
				_CelesteLight.SetActive(true);
				_AriaLight.SetActive(false);
				break;

			case "Aria":
				_YouLight.SetActive(false);
				_LunaLight.SetActive(false);
				_CelesteLight.SetActive(false);
				_AriaLight.SetActive(true);
				break;

			case null:
			default:
				_YouLight.SetActive(false);
				_LunaLight.SetActive(false);
				_CelesteLight.SetActive(false);
				_AriaLight.SetActive(false);
				break;
		}
	}
}
