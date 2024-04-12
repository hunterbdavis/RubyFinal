using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWinLossScreen : MonoBehaviour
{
    public Text winLossMessage;
    public int rubyHP;
    public RubyController ruby;
    // Start is called before the first frame update
    void Start()
    {
        RubyController ruby = GameObject.Find("Ruby").GetComponent<RubyController>();
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
        if(ruby.GetComponent<RubyController>().score >= 3)
        {
            winLossMessage.text = "You Win! Created by Group #10";
            
        }

    }
}
