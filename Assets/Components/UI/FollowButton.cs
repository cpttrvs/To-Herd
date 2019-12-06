using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowButton : ActionButton
{

    protected override void Button_OnClick()
    {
        base.Button_OnClick();

        currentSheep.GetController().Follow();
    }
}
