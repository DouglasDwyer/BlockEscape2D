using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritParentSpriteColor : MonoBehaviour {

    public ParticleSystem thisSystem;

	public void Awake()
    {
        var main = thisSystem.main;
        main.startColor = transform.parent.GetComponent<SpriteRenderer>().color;
    }
}