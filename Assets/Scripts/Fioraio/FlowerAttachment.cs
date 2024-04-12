using System;
using UnityEngine;

public class FlowerAttachment : MonoBehaviour
{
    [SerializeField] private Vector3 jointPositionOffset;
    public bool isAttached = false;
    public FixedJoint joint;
    public Vector3 vasePos;
    public Quaternion vaseRot;
    private Animator animator;

    private void Awake()
    {
        vasePos = transform.position;
        vaseRot = transform.rotation;
        animator = GetComponent<Animator>();
    }

    // Metodo per ottenere il riferimento al FixedJoint
    public FixedJoint GetFixedJoint()
    {
        return joint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttached && other.gameObject.CompareTag("Hand"))
        {
            Rigidbody flowerRB = GetComponent<Rigidbody>();
            joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = other.attachedRigidbody;
            joint.enableCollision = false;
            Debug.Log("Collisione fiore mano attivata!");
            isAttached = true;
        }
    }
    
    public void OnAnimationFinished()
    {
        animator.SetBool("animationIsOn", false);
        Debug.Log("Animazione disattivata");
       
    }


    

  
   

    

}

