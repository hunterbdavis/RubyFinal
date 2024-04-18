using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWinLossScreen : MonoBehaviour
{
    bool oneShotSound;
    public Text winLossMessage;
    public int rubyHP;
    public RubyController ruby;
    public AudioClip collectedClip;
    // Start is called before the first frame update
    void Start()
    {
        RubyController ruby = GameObject.Find("Ruby").GetComponent<RubyController>();
        oneShotSound = true;
    }

    // Update is called once per frame
    void Update()
    {
        rubyHP = ruby.health;
        if (rubyHP <= 0)
        {
            winLossMessage.text = "You Lose! Press R to restart!";
        }
        if (rubyHP > 0)
        {
            winLossMessage.text = "";
        }
        if (ruby.GetComponent<RubyController>().score >= 3)
        {
            winLossMessage.text = "You Win! Created by Group #10";
            if (oneShotSound == true)
            {
                Debug.Log("test");
                ruby.PlaySound(collectedClip);
                oneShotSound= false;
            }
        }
    }
}