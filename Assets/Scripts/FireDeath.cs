using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDeath : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D colObj)
    {
        PlayerBoxController.instance.KillPlayer();
    }    
}
