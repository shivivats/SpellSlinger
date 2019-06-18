using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys the stone spell when it exits the bounds
/// </summary>
public class StoneDestroy : MonoBehaviour
{

    /// <summary>
    /// The stone magic object that we will destroy
    /// </summary>
    public GameObject stoneMagic;

    /// <summary>
    /// Destroys the stone magic upon the trigger being entered
    /// </summary>
    /// <param name="other">The ball that we collide with</param>
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Ball")) {

            Destroy(stoneMagic);
        }
    }
}

