using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionToggle : MonoBehaviour
{
    [SerializeField]
    protected Toggle toggle = null;

    private SheepSelector[] selectors = null;

    protected SheepController currentSheep = null;
    

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

        toggle.onValueChanged.AddListener(Toggle_OnValueChanged);
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(Toggle_OnValueChanged);

        if (selectors != null)
        {
            for (int i = 0; i < selectors.Length; i++)
            {
                selectors[i].OnSelection -= Sheep_OnSelection;
                selectors[i].OnDeselection -= Sheep_OnDeselection;
            }
        }
    }

    protected virtual void Toggle_OnValueChanged(bool value)
    {
    }

    protected virtual void Sheep_OnSelection(SheepSelector s)
    {
        currentSheep = s.GetController();
    }

    protected virtual void Sheep_OnDeselection(SheepSelector s)
    {
        currentSheep = null;
    }
}
