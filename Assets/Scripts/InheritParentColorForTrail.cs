using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritParentColorForTrail : MonoBehaviour
{
    public TrailRenderer thisSystem;

    public void Awake()
    {
        Color spriteColor = transform.parent.GetComponent<SpriteRenderer>().color;
        Gradient grad = new Gradient();
        GradientColorKey[] keys = thisSystem.colorGradient.colorKeys;
        for (int x = 0; x < keys.Length; x++) {
            keys[x] = new GradientColorKey(spriteColor, keys[x].time);
        }
        grad.alphaKeys = thisSystem.colorGradient.alphaKeys;
        grad.SetKeys(keys, grad.alphaKeys);
        thisSystem.colorGradient = grad;
    }
}