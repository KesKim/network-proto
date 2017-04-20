using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneActions : MonoBehaviour
{
    [SerializeField] protected DynamicButtonGrid buttonGrid;
    [SerializeField] protected bool autoSetupOnStart = true;

    protected List<TestAction> sceneActions;

    // Inherit this class and create the sceneActions array during Awake.
	protected virtual void Start()
    {
        if ( buttonGrid == null )
        {
            buttonGrid = GameObject.FindObjectOfType<DynamicButtonGrid>();
        }

        if ( autoSetupOnStart )
        {
            setupActionButtons();
        }
	}

    protected void setupActionButtons()
    {
        buttonGrid.setActions(sceneActions.ToArray());
    }
}
