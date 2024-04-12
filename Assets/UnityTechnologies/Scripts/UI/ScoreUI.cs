using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    private RubyController rubyController;
    public Text scoreForUI;

    // Start is called before the first frame update
    void Start()
    {
        GameObject rubyControllerObject = GameObject.FindWithTag("RubyController"); //this line of code finds the RubyController script by looking for a "RubyController" tag on Ruby
        rubyController = rubyControllerObject.GetComponent<RubyController>(); //and this line of code finds the rubyController and then stores it in a variable
    }

    // Update is called once per frame
    void Update()
    {
        int number = rubyController.GetComponent<RubyController>().score;
        scoreForUI.text = "Fixed Robots: " + number.ToString();
    }
}
