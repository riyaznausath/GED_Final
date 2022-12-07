using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // Making cursor invisible when in play mode
        Cursor.lockState = CursorLockMode.Locked;
    }
}
