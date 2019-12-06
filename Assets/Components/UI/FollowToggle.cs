using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowToggle : ActionToggle
{
    protected override void Toggle_OnValueChanged(bool value)
    {
        base.Toggle_OnValueChanged(value);

        Debug.Log("ONVALUECHANGED");

        if(value)
        {
            if (currentSheep.IsLookingOut())
            {
                Debug.Log("STOP LOOK OUT");
                currentSheep.StopLookOut();
            }

            currentSheep.Follow();
        } else
        {
            currentSheep.StopFollow();
        }
    }

    protected override void Sheep_OnSelection(SheepSelector s)
    {
        base.Sheep_OnSelection(s);

        if(currentSheep.IsFollowing())
        {
            toggle.isOn = true;
        } else
        {
            toggle.isOn = false;
        }

        currentSheep.OnStopFollow += Sheep_OnStopFollow;
    }

    protected override void Sheep_OnDeselection(SheepSelector s)
    {
        currentSheep.OnStopFollow -= Sheep_OnStopFollow;

        base.Sheep_OnDeselection(s);
    }

    private void Sheep_OnStopFollow()
    {
        Debug.Log("STOP");
        toggle.isOn = false;
    }
}
