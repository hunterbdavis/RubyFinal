using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedCollectible : MonoBehaviour
{
    public AudioClip collectedClip;
    public static event Action<float> OnSpeedCollected;
    public float speedMultiplier = 1.5f;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        //Let RubyController script know
        {
        if (controller != null)
            {
                OnSpeedCollected.Invoke(speedMultiplier);
                controller.PlaySound(collectedClip);
                Destroy(gameObject);
            }
        }
    }
}