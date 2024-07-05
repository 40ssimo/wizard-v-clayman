using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CanonballBehaviour : MonoBehaviour
{
    private Vector3 _initialPosition;
    private bool _isDragging = false;
    private Rigidbody _rb;
    private float _zAxisFactor = 1f; // Factor to control how much Z changes based on Y
    public bool isLaunched = false;
    public bool isReady = false;
    public bool isStoppedLaunched = false;
    private Animator animator;
    public GameObject fire;
    public AudioSource audioSource;
    public ParticleSystem explosion;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _initialPosition = new Vector3(0.05f, 3f, 5f);
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        
    }

    void Update()
    {
        if (_isDragging && !isLaunched && isReady)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.ScreenToWorldPoint(transform.position).z;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector3 dragVector = worldPosition - _initialPosition;

            
            float zAdjustment = dragVector.y * _zAxisFactor;
            transform.position = new Vector3(_initialPosition.x + dragVector.x, _initialPosition.y + dragVector.y, _initialPosition.z + zAdjustment);
        }
    }

    private void OnMouseDown()
    {
        if (isReady && GameObject.Find("VirtualCameraOne").GetComponent<CinemachineVirtualCamera>().enabled == true)
        {
            _isDragging = true;
            animator.SetBool("OnClickDown", true);
            GameManager.Instance._redRing.SetActive(true);
            fire.SetActive(true);
            AudioManager.instance.PlaySound(audioSource, AudioManager.instance.audioList[1]);
        }
    }

    private void OnMouseUp()
    {
        if (!isLaunched && isReady)
        {
            Debug.Log("camera asdasdasdasd");
            _rb.constraints = RigidbodyConstraints.None;

            Vector3 releasePosition = transform.position;
            Vector3 direction = _initialPosition - releasePosition;
            float force = direction.magnitude * 7.5f; 
            _rb.AddForce(direction.normalized * force, ForceMode.Impulse);

            
            GameObject.Find("VirtualCameraOne").GetComponent<CinemachineVirtualCamera>().enabled = false;

            isLaunched = true;
            _isDragging = false;
            isReady = false;

            animator.SetBool("OnClickUp", true);
            animator.SetBool("OnClickDown", false);

            GameManager.Instance._redRing.SetActive(false);
            AudioManager.instance.PlaySound(audioSource, AudioManager.instance.audioList[3]);


            StartCoroutine(CheckBallStop());
        }
    }

    private IEnumerator CheckBallStop()
    {
        
        yield return new WaitForSeconds(1f);
        animator.SetBool("OnClickUp", false);
        while (true)
        {
            if (_rb.velocity.magnitude < 0.5f)
            {
                isStoppedLaunched = true;
                OnBallStop();
                yield break;
            }

           
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnBallStop()
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;

        if (GameManager.Instance.ballSequence < 4)
        {
            GameManager.Instance.ballSequence += 1;
            ChangeCanonball(GameManager.Instance.ballSequence);
           
        }
        else
        {
            GameManager.Instance.LevelEnd();
        }
    }

    public void ChangeCanonball(int ballSequence)
    {
        StartCoroutine(SetPosition(ballSequence));
    }

    private IEnumerator SetPosition(int ballSequence)
    {
        if (ballSequence < 4)
        {
            yield return new WaitForSeconds(1);
            GameObject moveBall = GameObject.Find("Canonball " + GameManager.Instance.ballSequence.ToString());
            moveBall.transform.position = _initialPosition;
            moveBall.GetComponent<CanonballBehaviour>().isReady = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            AudioManager.instance.PlaySound(audioSource, AudioManager.instance.audioList[0]);
            explosion.Play();
            
            Destroy(collision.gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            gameObject.transform.localScale = new Vector3(4, 4, 4);
            AudioManager.instance.PlaySound(audioSource, AudioManager.instance.audioList[4]);
            Destroy(other.gameObject);
        }
    }
}
