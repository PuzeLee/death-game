using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameController_1_4 : MonoBehaviour
{

    [Header("Scene 1-4")]
	public BoxCollider2D _doorBoxCollider2D;

    [Header("Light")]
    public Light2D _globalLight;
    [SerializeField] private float _globalMinIntensity = 0.09f;
    [SerializeField] private float _globalMaxIntensity = 0.11f;
    public Light2D _spotLight;
    [SerializeField] private float _spotMinIntensity = 5.5f;
    [SerializeField] private float _spotMaxIntensity = 4.5f;

    [Header("Role")]
    public GameObject _You;
    public GameObject _Luna;
    public GameObject _Aria;

    private void Start()
    {
		InvokeRepeating("DitheringGlobalLight", 0f, 0.1f);
	}

    public void ClickButton(string _paragraphName)
    {
        switch (_paragraphName)
        {
            case "2-2_Aria":
			case "3_Aria":
				_doorBoxCollider2D.enabled = true;
				InvokeRepeating("DitheringSpotLight", 0f, 0.1f);
                break;

            default:
                break;
        }
    }

    private void DitheringGlobalLight()
    {
        _globalLight.intensity = UnityEngine.Random.Range(_globalMinIntensity, _globalMaxIntensity);
    }
    private void DitheringSpotLight()
    {
        _spotLight.intensity = UnityEngine.Random.Range(_spotMinIntensity, _spotMaxIntensity);
    }
}
