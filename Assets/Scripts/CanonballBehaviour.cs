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
    public bool isLaunched;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _initialPosition = transform.position;
    }

    void Update()
    {
        if (_isDragging)
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
    }

    private void OnMouseUp()
    {
        _isDragging = false;

        _rb.constraints = RigidbodyConstraints.None;

        Vector3 releasePosition = transform.position;
        Vector3 direction = _initialPosition - releasePosition;
        float force = direction.magnitude * 10; // Adjust this multiplier to control the force
        _rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }
}
