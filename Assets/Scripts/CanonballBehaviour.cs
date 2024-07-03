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
    public bool isSpawned = false;
    

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _initialPosition = new Vector3(0.05f, 2.5f, 4.5f);

    }

    void Update()
    {
        if (_isDragging && !isLaunched)
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
        _isDragging = true;
        isSpawned = true;
    }

    private void OnMouseUp()
    {
        if (!isLaunched)
        {
            _rb.constraints = RigidbodyConstraints.None;

            Vector3 releasePosition = transform.position;
            Vector3 direction = _initialPosition - releasePosition;
            float force = direction.magnitude * 10; // Adjust this multiplier to control the force
            _rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        }
        
        isLaunched = true;
        _isDragging = false;

        GameManager.Instance.ballSequence += 1;

        ChangeCanonball();
    }

    public void ChangeCanonball()
    {
        StartCoroutine(SetPosition());
    }

    IEnumerator SetPosition()
    {
        yield return new WaitForSeconds(3);
        GameObject moveBall = GameObject.Find("Canonball " + GameManager.Instance.ballSequence.ToString());
        moveBall.transform.position = _initialPosition;
    }
}
