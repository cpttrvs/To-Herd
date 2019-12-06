using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    [SerializeField]
    private Button button;

    private SheepSelector[] selectors = null;

    protected SheepSelector currentSheep = null;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        selectors = FindObjectsOfType<SheepSelector>();
        for (int i = 0; i < selectors.Length; i++)
        {
            selectors[i].OnSelection += Sheep_OnSelection;
            selectors[i].OnDeselection += Sheep_OnDeselection;
        }

        button.onClick.AddListener(Button_OnClick);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(Button_OnClick);

        if (selectors != null)
        {
            for (int i = 0; i < selectors.Length; i++)
            {
                selectors[i].OnSelection -= Sheep_OnSelection;
                selectors[i].OnDeselection -= Sheep_OnDeselection;
            }
        }
    }

    protected virtual void Button_OnClick()
    {

    }

    void Sheep_OnSelection(SheepSelector s)
    {
        currentSheep = s;
    }

    void Sheep_OnDeselection(SheepSelector s)
    {

    }
}
