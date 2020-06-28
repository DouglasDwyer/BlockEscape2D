using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour {

    public SpriteRenderer blockRenderer;
    public ParticleSystem deathFlakes;

    private float inTime = 0;
    private bool playedSound = false;

	public void Update () {
        inTime += Time.deltaTime;
        blockRenderer.color = Color.Lerp(blockRenderer.color, new Color32(64, 64, 64, 255), 0.5f);
        if (!deathFlakes.gameObject.activeSelf)
        {
            deathFlakes.gameObject.SetActive(true);
            if (!playedSound)
            {
                SoundManager.PlaySoundEffect("Explode");
                playedSound = true;
            }
        }
        else if (inTime > 0.5f && !deathFlakes.isStopped) {
            deathFlakes.Stop();
            PlayerBoxController.instance.DestructTrail();
        }
        blockRenderer.gameObject.transform.localScale *= Mathf.Clamp(1.4f - inTime, 0, 1);
        if (1.35f < inTime) {
            GUIManagement.LoadGUISetupSingular("Death");
            enabled = false;
        }
	}

    public void Reset()
    {
        playedSound = false;
        inTime = 0;
        deathFlakes.gameObject.SetActive(false);
        deathFlakes.Play();
    }
}