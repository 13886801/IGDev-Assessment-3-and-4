﻿using System.Collections;
using UnityEngine;

public class JellyfishStates : MonoBehaviour
{
    private TextDisplay textDisplay;
    private Animator anim;
    private BoxCollider2D hitbox;
    private float powerTimer;

    // Start is called before the first frame update
    void Start()
    {
        textDisplay = GameObject.FindGameObjectWithTag("TextManager").GetComponent<TextDisplay>();
        anim = gameObject.GetComponent<Animator>();
        hitbox = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        powerTimer = textDisplay.GetPowerTime();
        anim.SetBool("Scared?", (powerTimer > 0f) ? true : false);
        anim.SetBool("Recovering?", (powerTimer > 0f && powerTimer < 3f) ? true : false);
        gameObject.tag = (powerTimer > 0f) ? "Prey" : "Predator";
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PacStudent") && gameObject.CompareTag("Prey"))
        {
            hitbox.enabled = false;
            anim.SetBool("Dead?", true);
            StartCoroutine("Respawn");
        }
    }

    private IEnumerator Respawn()
    {
        textDisplay.ChangeJellyfishCount(-1);

        while (anim.GetBool("Dead?"))
        {
            yield return null;
        }

        textDisplay.ChangeJellyfishCount(1);
        hitbox.enabled = true;
    }
}
