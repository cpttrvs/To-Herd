using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radius : MonoBehaviour
{
    [SerializeField]
    private GameObject followCircle = null;
    [SerializeField]
    private GameObject lookCircle = null;

    [SerializeField]
    private SheepController sheepController = null;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        if(sheepController != null)
        {
            sheepController.OnSelect += Sheep_OnSelect;
            sheepController.OnDeselect += Sheep_OnDeselect;
            sheepController.OnFollow += Sheep_OnFollow;
            sheepController.OnStopFollow += Sheep_OnStopFollow;
            sheepController.OnLookOut += Sheep_OnLookOut;
            sheepController.OnStopLookOut += Sheep_OnStopLookOut;
        }

        followCircle.SetActive(false);
        lookCircle.SetActive(false);
    }

    private void OnDestroy()
    {
        if (sheepController != null)
        {
            sheepController.OnSelect -= Sheep_OnSelect;
            sheepController.OnDeselect -= Sheep_OnDeselect;
            sheepController.OnFollow -= Sheep_OnFollow;
            sheepController.OnStopFollow -= Sheep_OnStopFollow;
            sheepController.OnLookOut -= Sheep_OnLookOut;
            sheepController.OnStopLookOut -= Sheep_OnStopLookOut;
        }

        followCircle.SetActive(false);
        lookCircle.SetActive(false);
    }

    void Sheep_OnSelect()
    {
        if (sheepController != null)
        {
            if(sheepController.IsFollowing())
            {
                followCircle.SetActive(true);
            }

            if(sheepController.IsLookingOut())
            {
                lookCircle.SetActive(true);
            }
        }
    }

    void Sheep_OnDeselect()
    {
        if(sheepController != null)
        {
            followCircle.SetActive(false);
            lookCircle.SetActive(false);
        }
    }

    void Sheep_OnFollow()
    {
        if(sheepController != null)
        {
            if(sheepController.IsSelected())
            {
                followCircle.SetActive(true);
            }
        }
    }

    void Sheep_OnStopFollow()
    {
        if (sheepController != null)
        {
            if (sheepController.IsSelected())
            {
                followCircle.SetActive(false);
            }
        }
    }

    void Sheep_OnLookOut()
    {
        if (sheepController != null)
        {
            if (sheepController.IsSelected())
            {
                lookCircle.SetActive(true);
            }
        }
    }

    void Sheep_OnStopLookOut()
    {
        if (sheepController != null)
        {
            if (sheepController.IsSelected())
            {
                lookCircle.SetActive(false);
            }
        }
    }
}
