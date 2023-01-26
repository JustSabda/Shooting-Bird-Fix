using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputHandler _input;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private bool rotateTowardMouse;

    [SerializeField]private Camera mainCamera;

    private void Awake()
    {
        _input = GetComponent<InputHandler>();

    }

    void Update()
    {
        var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);

        var movementVector = MoveTowardTarget(targetVector);

        if(!rotateTowardMouse)
        RotateTowardMovementVector(movementVector);
        else
            RotateTowardMouseVector();

    }

    private void RotateTowardMouseVector()
    {
        Ray ray = mainCamera.ScreenPointToRay(_input.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitinfo, maxDistance: 300f))
        {
            var target = hitinfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }

    }
    private void RotateTowardMovementVector(Vector3 movementVector)
    {
        if (movementVector.magnitude == 0) { return; }
        var rotation = Quaternion.LookRotation(movementVector);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
    }

    private Vector3 MoveTowardTarget(Vector3 targetVector)
    {
        var speed = moveSpeed * Time.deltaTime;
        targetVector = Quaternion.Euler(0, mainCamera.gameObject.transform.eulerAngles.y, 0) * targetVector;

        var targetPosition = transform.position + targetVector * speed;
        transform.position = targetPosition;

        return targetVector;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Dead Wall")
        {
            Destroy(gameObject);
        }
    }
}
