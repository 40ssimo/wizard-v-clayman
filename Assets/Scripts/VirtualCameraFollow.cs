using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    private CinemachineVirtualCamera vCamFollow;
    void Start()
    {
        vCamFollow = GetComponent<CinemachineVirtualCamera>();
        vCamFollow.Follow = GameObject.Find("Canonball 1").transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckBallStopped();
    }

    public void CheckBallStopped()
    {
        GameObject stoppedCanonball;
        if (!(GameManager.Instance.ballSequence - 1 == 0))
        {
            stoppedCanonball = GameObject.Find("Canonball " + (GameManager.Instance.ballSequence - 1).ToString());
            if (stoppedCanonball.GetComponent<CanonballBehaviour>().isStoppedLaunched == true)
            {
                StartCoroutine(DelayChangeBall(stoppedCanonball));
            }
        }
    }

    IEnumerator DelayChangeBall(GameObject canonball)
    {
        
        yield return new WaitForSeconds(3);
        Debug.Log("change camera follow");
        if (canonball.GetComponent<CanonballBehaviour>().isReady == false)
        {
            GameObject.Find("VirtualCameraOne").GetComponent<CinemachineVirtualCamera>().enabled = true;
            canonball.GetComponent<CanonballBehaviour>().isReady = true;
        }
        
        if (!(GameManager.Instance.ballSequence > 3))
        {
            vCamFollow.Follow = GameObject.Find("Canonball " + (GameManager.Instance.ballSequence).ToString()).transform;
            yield break;
        }
        
    }

}
