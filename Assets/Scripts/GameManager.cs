using Cinemachine;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager instance;
    public int ballSequence = 1;
    public bool levelClear = false;


    // Property to access the singleton instance
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

    // Ensure only one instance of the GameManager exists
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

    // Your game management code here
    void Start()
    {
        // Initialize your game manager
        LevelStart();
    }

    void Update()
    {
        // Update your game manager
        
        
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
   
}
