using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameController_3_1 : MonoBehaviour
{

    // [Header("Scene 3-1")]

    public void ClickButton(string _paragraphName)
    {
        switch (_paragraphName)
        {
            case "3":
				SceneController._instance.NextScene();
				break;

            default:
                break;
        }
    }
}
