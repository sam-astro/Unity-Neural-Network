using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSceneOnButtonPress : MonoBehaviour
{
    public string sceneName;

    public void GoToScene(string name)
	{
		SceneManager.LoadSceneAsync(name);
	}
}
