using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class TestAction
{
    public UnityAction action;
    public string text;

    public TestAction(UnityAction _action, string _text)
    {
        action = _action;
        text = _text;
    }
}

public class DynamicButtonGrid : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    private TestAction[] actions;

    [SerializeField] private RectTransform buttonParent;

    public void setActions(TestAction[] _actions)
    {
        destroyButtons();

        int count = _actions.Length;

        for ( int i = 0; i < count; i++ )
        {
            createButton(_actions[i]);
        }
    }

    private void destroyButtons()
    {
        int count = buttonParent.GetChildCount();

        for ( int i = count - 1; i > -1; i-- )
        {
            Destroy(buttonParent.GetChild(i).gameObject);
        }
    }

    private void createButton(TestAction _action)
    {
        GameObject buttonObject = (GameObject)Instantiate(buttonPrefab, buttonParent);

        Button button = buttonObject.GetComponent<Button>();
        button.onClick.AddListener(_action.action);

        Text text = buttonObject.GetComponentInChildren<Text>();
        text.text = _action.text;
    }
}
