using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightController : MonoBehaviour
{
	public GameObject _YouLight;
	public GameObject _LunaLight;
	public GameObject _CelesteLight;
	public GameObject _AriaLight;

    private ParagraphController _paragraphController;
	
	private void Awake()
	{
		_paragraphController = GetComponent<ParagraphController>();
	}

	private void Update()
	{
		// ChangeLight();
	}

	public void ChangeLight()
	{
		switch (_paragraphController._currentParagraph._speakerName)
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
