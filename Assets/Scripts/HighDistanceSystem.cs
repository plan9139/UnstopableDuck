using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighDistanceSystem : MonoBehaviour
{
    public Text hightxt;
    void Start()
    {
        hightxt.text = "High Distance :" + PlayerPrefs.GetInt("HighDistance",0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
