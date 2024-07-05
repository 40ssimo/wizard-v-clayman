using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager instance;
    public int ballSequence = 1;
    public bool levelClear = false;
    public GameObject _redRing;
    public AudioSource audioSource;


    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";

                    DontDestroyOnLoad(singletonObject);
                }
            }

            return instance;
        }
    }

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        LevelStart();
        
    }

    void Update()
    {
        if (ballSequence == 4)
        {
            LevelEnd();
        }
        
        
    }

    

    public void LevelStart()
    {
        levelClear = false;
        ballSequence = 1;
        StartCoroutine(CameraMovement());
    }

    public void LevelEnd()
    {
        levelClear = true;
    }

    IEnumerator CameraMovement()
    {
        var virtualCameraThree = GameObject.Find("VirtualCameraThree").GetComponent<CinemachineVirtualCamera>().enabled = false;
        yield return new WaitForSeconds(3);
        var virtualCameraTwo = GameObject.Find("VirtualCameraTwo").GetComponent<CinemachineVirtualCamera>().enabled = false;
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
   
}
