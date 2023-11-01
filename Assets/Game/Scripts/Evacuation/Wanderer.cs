using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
    private CustomPath currentPath;

    private void OnEnable()
    {
        //initial position
    }

    //when follow the loop, go from 1 to n
    //0 - initial position out of screen

    public void SetPath(CustomPath targetPath)
    {
        currentPath = targetPath;
        //position to 0
    }
}
