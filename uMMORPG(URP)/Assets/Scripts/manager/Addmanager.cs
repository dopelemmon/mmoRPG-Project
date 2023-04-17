using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Addmanager : MonoBehaviour   
{
    public static Addmanager Instance;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            DestroyImmediate(gameObject);

        ///Advertisement.Initialize("2685537");

    }

    public void ShowAds()
    {
        //Advertisement.Show();
        Debug.Log("game ads hes show unity ads");
    }

   
}
