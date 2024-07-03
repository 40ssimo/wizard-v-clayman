using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager instance;
    public int ballSequence = 1;
    

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
    }

    void Update()
    {
        // Update your game manager
        ResetBallSequence(ballSequence);
        
    }

    public void ResetBallSequence(int ballSequence)
    {
        if (ballSequence == 4)
        {
            GameManager.Instance.ballSequence = 1;
        }
    }

    public IEnumerator WaitForSecondsCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
