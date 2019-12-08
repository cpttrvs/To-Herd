using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameConfig : MonoBehaviour
{
    public int nbSheeps = 10;
    public bool showTimer = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
