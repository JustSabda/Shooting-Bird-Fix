using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class PlayerController : MonoBehaviour
{
    private InputHandler _input;
    public static PlayerController Instance { get; private set; }

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private bool rotateTowardMouse;

    [SerializeField]private Camera mainCamera;

    public BoxCollider corner;
    public GameObject egg;
    public Vector3 eggPlace;

    public Transform meeple;

    public bool hadEgg;

    [Header("Photon")]
    PhotonView view;

    private void Awake()
    {


    }
    private void Start()
    {
        _input = GetComponent<InputHandler>();
        corner = GetComponent<BoxCollider>();
        hadEgg = true;
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (view.IsMine)
        {
            mainCamera = Camera.main;
            meeple = transform.Find("Egg");

            if (meeple == null)
            {
                hadEgg = false;
                corner.enabled = false;
            }
            else
            {
                hadEgg = true;
                corner.enabled = true;
                
            }

            if(corner.GetComponent<DeadWall>())
            {
                PhotonNetwork.Destroy(gameObject);
            }

            var targetVector = new Vector3(_input.InputVector.x, 0, _input.InputVector.y);

            var movementVector = MoveTowardTarget(targetVector);

            if (!rotateTowardMouse)
                RotateTowardMovementVector(movementVector);
            else
                RotateTowardMouseVector();

            if (Input.GetKeyDown(KeyCode.Alpha1))
                Instantiate(egg, transform.position + eggPlace, Quaternion.identity, transform);

            /*
            for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
            {
                // only destroy tagged object
                if (gameObject.transform.GetChild(i).gameObject.tag == "Egg")
                    Destroy(gameObject.transform.GetChild(i).gameObject);
            }
            */

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                gotShoot();
            }


        }
    }

    public void gotShoot()
    {
        for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
        {

        }
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


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Dead Wall"))
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

}
