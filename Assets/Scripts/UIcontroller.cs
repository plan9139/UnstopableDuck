using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIcontroller : MonoBehaviour
{
    Player player;
    Text distanceText;

    GameObject results;
    Text finalDistanceText;


    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        distanceText = GameObject.Find("DistanceText").GetComponent<Text>();
        
        finalDistanceText = GameObject.Find("FinalDistance").GetComponent<Text>();
        
        results = GameObject.Find("Results");
        results.SetActive(false);
        

    }


    void Start()
    {
        
    }

   
    void Update()
    {
        int distance = Mathf.FloorToInt(player.distance);
       distanceText.text =  distance + " m";
        if (distance>PlayerPrefs.GetInt("HighDistance",0))
        {
           PlayerPrefs.SetInt("HighDistance", distance);
        }
        
       
       if(player.isDead)
       {
           results.SetActive(true);
           finalDistanceText.text = distance + " m";
       }
      
    }

    public void Quit()
    {
        SceneManager.LoadScene("Title");
    }

    public void Retry()
    {
        SceneManager.LoadScene("FirstScene");
    }

}
