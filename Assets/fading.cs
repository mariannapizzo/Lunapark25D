using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fading : MonoBehaviour
{
    [SerializeField] private GameObject vaso;
    FixedJoint joint;
    private GameObject fiore;
    private float quotaPartenza;
    private bool canMove = false;
    void FixedUpdate()
    {   
        
       
           
            joint= vaso.transform.GetComponent<FixedJoint>();
            
            if (joint != null)
            {
                fiore= joint.connectedBody.transform.gameObject;
                fiore.GetComponent<Rigidbody>().isKinematic = true; 
                fiore.GetComponent<Rigidbody>().useGravity = false;
                quotaPartenza=fiore.transform.position.y;
                Destroy(joint);
                // Avvio della coroutine per attendere 3 secondi prima di spostare il giunto
                StartCoroutine("WaitAndMoveJoint");
                
            }
           
            // Questo fa la salita, aggiungere la dissolvenza mentre sale
            if((fiore.transform.position.y <= quotaPartenza + 0.8f) &&  canMove)
            {
                Vector3 currentPosition = fiore.transform.position;
                Vector3 targetPosition = currentPosition + Vector3.up * 0.01f; // Modifica 0.1f secondo le tue esigenze
                fiore.transform.position = targetPosition;

                // Calcola l'alpha in base alla distanza verticale rispetto alla quota di partenza
                float alpha = 1.0f - Mathf.Clamp01((targetPosition.y - quotaPartenza) / 0.8f);

                // Imposta l'alpha del materiale del fiore
                Material[] fioreMaterial = fiore.GetComponentInChildren<SkinnedMeshRenderer>().materials;
              
                foreach (var mat in fioreMaterial)
                {
                    if (mat.name == "Ibiscus (Instance)" ||mat.name == "Stem (Instance)" ||mat.name == "Pistil (Instance)")
                    {
                        if (mat.GetFloat("_Mode") == 0)
                        {
                            mat.SetFloat("_Mode", 3);
                        }
                       Color currentColor = mat.color;
                       currentColor.a = alpha;
                       mat.color = currentColor;
                    }
                    
                }
                

                // Disabilita il movimento se il fiore ha superato la quota di dissolvenza
                if (targetPosition.y > quotaPartenza + 0.8f)
                    canMove = false;
            }

           
        
        
    }
    
    IEnumerator WaitAndMoveJoint()
    {
        yield return new WaitForSeconds(0.5f);
        canMove = true; 



    }
}
