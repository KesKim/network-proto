using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneActionsPlayerOptions : SceneActions
{
    private void Awake()
    {
        sceneActions = new List<TestAction>()
        {
			new TestAction(back, "Back")
        };
    }

	private void back()
	{
		SceneManager.LoadScene("SceneInit");
	}
}
