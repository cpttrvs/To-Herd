using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookOutToggle : ActionToggle
{
    protected override void Toggle_OnValueChanged(bool value)
    {
        base.Toggle_OnValueChanged(value);

        if(currentSheep != null)
        {
            if (value)
            {
                if (currentSheep.IsFollowing())
                {
                    currentSheep.StopFollow();
                }

                currentSheep.LookOut();
            }
            else
            {
                currentSheep.StopLookOut();
            }
        }

    }

    protected override void Sheep_OnSelection(SheepSelector s)
    {
        base.Sheep_OnSelection(s);

        if (currentSheep.IsLookingOut())
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }

        currentSheep.OnStopLookOut += Sheep_OnStopLookOut;
    }

    protected override void Sheep_OnDeselection(SheepSelector s)
    {
        currentSheep.OnStopLookOut -= Sheep_OnStopLookOut;

        base.Sheep_OnDeselection(s);
    }

    private void Sheep_OnStopLookOut()
    {
        toggle.isOn = false;
    }
}
