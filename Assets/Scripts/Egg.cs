using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Egg : MonoBehaviour
{
    public static Egg Instance { get; private set; }

    Rigidbody rb;
    CapsuleCollider capsule;
    public enum state { DEFAULT, GONE , PICKUP , CHARGING}
    public state _state = state.DEFAULT;

    [Header("DownTime")]
    [SerializeField]float currentTime;
    [SerializeField]float startingTime;

    bool done;
    public TMP_Text countdownText;
    public Color color;

    public Transform Core;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
    }


    void Update()
    {
        //this.gameObject.transform.SetParent(this.transform);

        if (Input.GetKeyDown(KeyCode.R))
        {
            this.gameObject.transform.SetParent(null);
            rb.isKinematic = false;
            _state = state.GONE;
            done = true;
            
        }

        if (_state == state.GONE)
        {
            //PlayerController.Instance.hadEgg = false;
            if (done)
            {
                currentTime = startingTime;
                done = false;
            }
            
            currentTime -= 1 * Time.deltaTime;
            countdownText.text = currentTime.ToString("0");

            if (currentTime <= 0)
            {
                currentTime = 0;
                _state = state.PICKUP;
                
            }
        }
        
        
    }

    public void Damaged()
    {
        if (_state != state.GONE || _state != state.PICKUP)
        {
            backParent(Core);

            rb.isKinematic = false;
            _state = state.GONE;
            done = true;
            capsule.isTrigger = false;
        }

    }
    public void backParent(Transform parent)
    {
        this.gameObject.transform.SetParent(parent);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" && _state == state.PICKUP)
        {
            this.gameObject.transform.SetParent(collision.transform);
            transform.localPosition = new Vector3(0.01f, 0.68f, -1f);
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            rb.isKinematic = true;
            countdownText.text = currentTime.ToString("3");
        }

        if (collision.gameObject.tag == "Dead Wall")
        {
            PhotonNetwork.Destroy(Core.gameObject);
        }

        
    }


}
