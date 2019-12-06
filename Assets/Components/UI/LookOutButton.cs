using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookOutButton : ActionButton
{
    protected override void Button_OnClick()
    {
        base.Button_OnClick();

        currentSheep.GetController().LookOut();
    }
}
