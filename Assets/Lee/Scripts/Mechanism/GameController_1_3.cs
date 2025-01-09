using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameController_1_3 : MonoBehaviour
{

    [Header("Scene 1-3")]
    public GameObject _dieXian;
    public Light2D _globalLight;

    public void ClickButton(string _paragraphName)
    {
        switch (_paragraphName)
        {
            case "2_Aria":
                _dieXian.SetActive(true);
                _globalLight.intensity = 0.1f;
                break;

            default:
                break;
        }
    }
}
