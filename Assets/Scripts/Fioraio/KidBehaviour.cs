using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject flower;
    [SerializeField] private GameManager GameManager;
    private Animator animator;

    private Postazione fioraio;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnAnimationComplete()
    {
        fioraio = GameManager.postazione; 
        animator.SetBool("movementDone", false);
        Animator flowerAnimator = flower.GetComponent<Animator>();
        flowerAnimator.SetBool("animateFlower", false);
        Collider boxCollider = GetComponentInChildren<BoxCollider>();
        if (boxCollider != null)
        {
            // Cambia il tag del BoxCollider
            boxCollider.tag = "Untagged"; // Cambia "NuovoTag" con il tag desiderato
        }
        else
        {
            Debug.LogError("BoxCollider non trovato!");
        }
        GameObject HandR = boxCollider.gameObject;
        int handLayer = LayerMask.NameToLayer("Hand");
        HandR.layer = handLayer;
        if (DataManager.Instance.livelloScelto == 1)
        {
            fioraio.SetState(4);
            
        }
                
        else if (DataManager.Instance.livelloScelto==1)
            fioraio.SetState(7);
        EndFixedJoint();
    }
    
    
    
    private void EndFixedJoint()
    {
        FlowerAttachment flowerAttachment = flower.GetComponent<FlowerAttachment>();
        if (flowerAttachment != null)
        { 
            FixedJoint joint = flowerAttachment.GetFixedJoint();
            if (joint != null)
            {
                Destroy(joint);
                flower.transform.position = flowerAttachment.vasePos;
                flower.transform.rotation = flowerAttachment.vaseRot;
               
            }
            flowerAttachment.isAttached = false;
        }
    }
}
