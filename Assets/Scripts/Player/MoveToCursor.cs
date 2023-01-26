using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCursor : MonoBehaviour
{
    private InputHandler _input;
    [SerializeField] private Camera mainCamera;

    

    private void Awake()
    {
        _input = GetComponent<InputHandler>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        //transform.position = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
        RotateTowardMouseVector();

        

    }

    private void RotateTowardMouseVector()
    {
        Ray ray = mainCamera.ScreenPointToRay(_input.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitinfo, maxDistance: 300f))
        {
            var target = hitinfo.point;
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 5);
        }

    }
}
