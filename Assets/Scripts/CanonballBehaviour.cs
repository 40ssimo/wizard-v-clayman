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

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _initialPosition = new Vector3(0.05f, 2.5f, 4.5f);
    }

    void Update()
    {
        if (_isDragging && !isLaunched && isReady)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.ScreenToWorldPoint(transform.position).z;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector3 dragVector = worldPosition - _initialPosition;

            // Adjust the Z axis based on the Y axis movement
            float zAdjustment = dragVector.y * _zAxisFactor;
            transform.position = new Vector3(_initialPosition.x + dragVector.x, _initialPosition.y + dragVector.y, _initialPosition.z + zAdjustment);
        }
    }

    private void OnMouseDown()
    {
        if (isReady)
        {
            _isDragging = true;
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
            float force = direction.magnitude * 10; // Adjust this multiplier to control the force
            _rb.AddForce(direction.normalized * force, ForceMode.Impulse);

            
            GameObject.Find("VirtualCameraOne").GetComponent<CinemachineVirtualCamera>().enabled = false;

            isLaunched = true;
            _isDragging = false;
            isReady = false;

            // Start the coroutine to check the ball's stop condition after a delay
            StartCoroutine(CheckBallStop());
        }
    }

    private IEnumerator CheckBallStop()
    {
        // Wait for 1 second before starting to check the velocity
        yield return new WaitForSeconds(1f);

        while (true)
        {
            if (_rb.velocity.magnitude < 0.5f)
            {
                isStoppedLaunched = true;
                OnBallStop();
                yield break;
            }

            // Check the velocity every 0.1 seconds
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
}
