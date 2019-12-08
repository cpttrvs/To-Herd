using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectWolfToggle : MonoBehaviour
{
    [SerializeField]
    private Toggle wolfToggle1 = null;
    [SerializeField]
    private Toggle wolfToggle2 = null;
    [SerializeField]
    private WolfSelector wolfSelector1 = null;
    [SerializeField]
    private WolfSelector wolfSelector2 = null;

    private void Start()
    {
        wolfToggle1.onValueChanged.AddListener(WolfToggle1_OnValueChanged);
        wolfToggle2.onValueChanged.AddListener(WolfToggle2_OnValueChanged);

        wolfSelector1.OnSelection += OnWolfSelected;
        wolfSelector2.OnSelection += OnWolfSelected;

        wolfSelector1.OnDeselection += OnWolfDeselected;
        wolfSelector2.OnDeselection += OnWolfDeselected;
    }

    private void OnDestroy()
    {
        wolfToggle1.onValueChanged.RemoveListener(WolfToggle1_OnValueChanged);
        wolfToggle2.onValueChanged.RemoveListener(WolfToggle2_OnValueChanged);

        wolfSelector1.OnSelection -= OnWolfSelected;
        wolfSelector2.OnSelection -= OnWolfSelected;

        wolfSelector1.OnDeselection -= OnWolfDeselected;
        wolfSelector2.OnDeselection -= OnWolfDeselected;
    }

    private void WolfToggle1_OnValueChanged(bool value)
    {
        if(value)
        {
            wolfSelector2.Deselect();
            wolfSelector1.Select();
        } else
        {
            wolfSelector1.Deselect();
        }
    }

    private void WolfToggle2_OnValueChanged(bool value)
    {
        if (value)
        {
            wolfSelector1.Deselect();
            wolfSelector2.Select();
        }
        else
        {
            wolfSelector2.Deselect();
        }
    }

    private void OnWolfSelected(WolfSelector w)
    {
        if(w == wolfSelector1)
        {
            wolfToggle1.SetIsOnWithoutNotify(true);
            wolfToggle2.SetIsOnWithoutNotify(false);

            wolfToggle1.interactable = false;
            wolfToggle2.interactable = true;
        }

        if(w == wolfSelector2)
        {
            wolfToggle2.SetIsOnWithoutNotify(true);
            wolfToggle1.SetIsOnWithoutNotify(false);

            wolfToggle2.interactable = false;
            wolfToggle1.interactable = true;
        }
    }

    private void OnWolfDeselected(WolfSelector w)
    {
        if (w == wolfSelector1)
        {
            wolfToggle1.SetIsOnWithoutNotify(false);
            wolfToggle1.interactable = true;
        }

        if (w == wolfSelector2)
        {
            wolfToggle2.SetIsOnWithoutNotify(false);
            wolfToggle2.interactable = true;
        }
    }
}
