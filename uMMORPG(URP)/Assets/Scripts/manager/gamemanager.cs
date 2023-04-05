using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gamemanager : MonoBehaviour
{
    public static gamemanager Instance;
    public Text leveltext;
    public int levelno,levelshow;
    public int collected;
    [HideInInspector]
    public Transform cans;
    GameObject levelcompletepanel;
    public GameObject popeffect;
    bool levelcomplete=false,nextlevel=false;
    public Transform ballparent;
    public bool checkwin = false;
    bool complete = false;
    public GameObject trailprefab,Knife;
    float TrailZ = 3f;
    static int loadCount = 0;

    private void Awake()
    {
        Instance = this;
        levelno = SceneManager.GetActiveScene().buildIndex;
    }

    void Start()
    {
        trailprefab.SetActive(false);
        ballparent = GameObject.Find("ballparent").GetComponent<Transform>();
        levelcompletepanel = GameObject.Find("levelcomplete_panel");
        cans =  GameObject.Find("cans").GetComponent<Transform>();
        leveltext = GameObject.Find("leveltext").GetComponent<Text>();
        levelshow = levelno-1;
        Dispalyinfo();
        levelcompletepanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 trialpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mousepos = new Vector3(trialpos.x, trialpos.y, TrailZ);
            trailprefab.transform.position = mousepos;
            trailprefab.SetActive(true);
            Knife.transform.position = trialpos;
        }else
        {
            trailprefab.SetActive(false);
        }
        if(levelcomplete==false)
        {
         
            levelcompleted();
        }
        if(nextlevel)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Nextlevel();
            }
        }
        if(checkwin)
        {
            checkwin = false;
            levelcheck();
        }
    }

     void levelcompleted()
    {
        if(collected >= cans.childCount&&complete == false)
        {
            complete = true;
            levelcomplete = true;
            nextlevel = true;
            print("level completed");
            Invoke("Popefffectmethod", 1f);
           
        }
     
    }

    public void levelcheck()
    {
        if (collected >= cans.childCount&&ballparent.childCount==0&&complete==false)
        {
            complete = true;
            print("level completed");
            Invoke("Popefffectmethod", 1f);
        }
        else if (collected < cans.childCount && ballparent.childCount == 0)
        {
            retry();
        }

    }

    void Popefffectmethod()
    {
        GameObject pop = Instantiate(popeffect, popeffect.transform.position, popeffect.transform.rotation);
        levelcompletepanel.SetActive(true);
    }

    void Dispalyinfo()
    {
        leveltext.text = "L E V E L  " + levelshow;
    }

    public void Retrybutton()
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void retry()
    {
        if (complete == false)
        {
            complete = true;
            SceneManager.LoadScene(levelno);
            Addmanager.Instance.ShowAds();
            

        }
    }

    public void Nextlevel()
    {
        levelno++;
        PlayerPrefs.SetInt("levels", levelno);
        SceneManager.LoadScene(levelno);
    }

    public void Menu_Next()
    {
        levelno = PlayerPrefs.GetInt("levels",2);
        SceneManager.LoadScene(levelno);
    }
}
