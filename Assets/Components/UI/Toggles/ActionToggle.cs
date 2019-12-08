using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionToggle : MonoBehaviour
{
    [SerializeField]
    protected Toggle followToggle = null;
    [SerializeField]
    protected Toggle lookToggle = null;

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

        followToggle.onValueChanged.AddListener(FollowToggle_OnValueChanged);
        lookToggle.onValueChanged.AddListener(LookToggle_OnValueChanged);
    }

    private void OnDestroy()
    {
        followToggle.onValueChanged.RemoveListener(FollowToggle_OnValueChanged);
        lookToggle.onValueChanged.RemoveListener(LookToggle_OnValueChanged);

        if (selectors != null)
        {
            for (int i = 0; i < selectors.Length; i++)
            {
                selectors[i].OnSelection -= Sheep_OnSelection;
                selectors[i].OnDeselection -= Sheep_OnDeselection;
            }
        }
    }

    protected virtual void FollowToggle_OnValueChanged(bool value)
    {
        if(currentSheep != null)
        {
            if(value)
            {
                if (currentSheep.IsLookingOut())
                    lookToggle.isOn = false;

                currentSheep.Follow();
            } else
            {
                currentSheep.StopFollow();
            }
        }
    }

    protected virtual void LookToggle_OnValueChanged(bool value)
    {
        if (currentSheep != null)
        {
            if (value)
            {
                if (currentSheep.IsFollowing())
                    followToggle.isOn = false;

                currentSheep.LookOut();
            }
            else
            {
                currentSheep.StopLookOut();
            }
        }
    }

    protected virtual void Sheep_OnSelection(SheepSelector s)
    {
        currentSheep = s.GetController();

        if(currentSheep != null)
        {
            currentSheep.OnMoveOrder += Sheep_OnMoveOrder;

            followToggle.SetIsOnWithoutNotify(currentSheep.IsFollowing());
            lookToggle.SetIsOnWithoutNotify(currentSheep.IsLookingOut());
        }
    }

    protected virtual void Sheep_OnDeselection(SheepSelector s)
    {
        currentSheep.OnMoveOrder -= Sheep_OnMoveOrder;

        currentSheep = null;
    }

    void Sheep_OnMoveOrder()
    {
        followToggle.SetIsOnWithoutNotify(currentSheep.IsFollowing());
        lookToggle.SetIsOnWithoutNotify(currentSheep.IsLookingOut());
    }
}
