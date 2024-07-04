using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _initialPosition = new Vector3(0.05f, 2.5f, 4.5f);

    }

    void Update()
    {
        if (_isDragging && !isLaunched && (isReady == true))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.ScreenToWorldPoint(transform.position).z;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector3 dragVector = (worldPosition - _initialPosition);
            

            // Adjust the Z axis based on the Y axis movement
            float zAdjustment = dragVector.y * _zAxisFactor;
            transform.position = new Vector3(_initialPosition.x + dragVector.x, _initialPosition.y + dragVector.y, _initialPosition.z + zAdjustment);

        }
    }

    private void OnMouseDown()
    {
        if (isReady == true)
        {
            _isDragging = true;
        }
        
    }

    private void OnMouseUp()
    {
        if (!isLaunched && isReady)
        {
            _rb.constraints = RigidbodyConstraints.None;

            Vector3 releasePosition = transform.position;
            Vector3 direction = _initialPosition - releasePosition;
            float force = direction.magnitude * 10; // Adjust this multiplier to control the force
            _rb.AddForce(direction.normalized * force, ForceMode.Impulse);

            if (GameManager.Instance.ballSequence < 4)
            {
                GameManager.Instance.ballSequence += 1;
                ChangeCanonball(GameManager.Instance.ballSequence);
                isLaunched = true;
                _isDragging = false;
                isReady = false;
            } 
            
            if (GameManager.Instance.ballSequence == 4)
            {
                GameManager.Instance.LevelEnd();
            }
        }
    }

    public void ChangeCanonball(int ballSequence)
    {
        StartCoroutine(SetPosition(ballSequence));
    }

    IEnumerator SetPosition(int ballSequence)
    {
        if (ballSequence < 4)
        {
            yield return new WaitForSeconds(3);
            GameObject moveBall = GameObject.Find("Canonball " + GameManager.Instance.ballSequence.ToString());
            moveBall.transform.position = _initialPosition;
            moveBall.GetComponent<CanonballBehaviour>().isReady = true;
        }
    }
}
