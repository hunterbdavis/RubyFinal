using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCoin : MonoBehaviour
{
    public AudioClip collectedClip;
    GameObject[] Enemies;
    // Start is called before the first frame update
    void Start()
    {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
            Enemies[i].GetComponent<Enemy>().speed = 0;
        }
        RubyController controller = other.GetComponent<RubyController>();
        if (controller != null)
        {
            Invoke("restoreMovement", 5f);
            controller.PlaySound(collectedClip);
            Destroy(gameObject);
        }
    }

    void restoreMovement()
    {
        for (int i = 0;i < Enemies.Length;i++)
        {
            Enemies[i].GetComponent<Enemy>().speed = 2;
        }
    }
}
