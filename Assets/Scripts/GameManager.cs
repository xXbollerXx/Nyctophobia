using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject Player;
   

    // Start is called before the first frame update
    void Start()
    {
        if (!Instance)
        {
            Instance = this;
        }
        
        //check what level player is on 
        StartLevel();
    }

    void StartLevel()
    {
        DunGen.MapGenerator.Instance.GenerateMap();
       
    }

    public void NextLevel()
    {
        //level++ 
        //unload level load same level 
        PlayerStats.Level++;
        SceneManager.LoadScene("DunGen");

    }
}
