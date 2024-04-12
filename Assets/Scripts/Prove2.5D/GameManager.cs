using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Sources;
using Cinemachine;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isSchedina; 
    public Postazione postazione; //useless
    [SerializeField]private GameObject goData;
    private LetturaDati dataReader;
    public float presThreshold=0.7f;
    private float F; //per il; momento 40Hz
    public int tManteinance = 3;
    private int framesManteinance;
    private Slider barraCompletamento;
    [SerializeField] Conveyors tapisRoulant;
    [SerializeField] private GameObject vaso;
    private Vector3 posizionePartenzaVaso;
    FixedJoint joint;
    private GameObject fiore;
    private float quotaPartenza;
    private bool canMove = false;
    [SerializeField]private Canvas canvas;
    public string percorsoImmagine = "SpriteOutline"; // Percorso dell'immagine nella sottocartella MySprites
    private Sprite oldSprite1;
    private Sprite oldSprite2;
    private Sprite oldSprite3;
    private Image immagine1;
    private Image immagine2;
    private Image immagine3;
    private bool isCorrect=true;
    [SerializeField]private Sprite fuck; 
    
    void Awake()
    {
        goData.SetActive(true);
        dataReader = goData.GetComponent<LetturaDaTastiera>();
        F = 1 / Time.fixedDeltaTime; //per il; momento 40Hz
        
        framesManteinance = (int)(tManteinance * F);
        barraCompletamento = Object.FindObjectOfType<Slider>();
        barraCompletamento.value = 0;

        posizionePartenzaVaso = vaso.transform.localPosition; 
    }

    void FixedUpdate()
    {   
        dataReader.statisticheClassi(framesManteinance);
        bool meetsReq = (dataReader.presenze[DC.sO_N]+dataReader.presenze[DC.sO_PN]+dataReader.presenze[DC.sO_SN] + dataReader.presenze[DC.sO_S] + dataReader.presenze[DC.sO_P]) > (presThreshold * framesManteinance);
        barraCompletamento.value =Mathf.Clamp01((dataReader.presenze[DC.sO_N]+dataReader.presenze[DC.sO_PN]+dataReader.presenze[DC.sO_SN] + dataReader.presenze[DC.sO_S] + dataReader.presenze[DC.sO_P])/(presThreshold * framesManteinance)) ;
        if (meetsReq)
        {
            tapisRoulant.speed = 0.5f; 
            
        }
        
        
        if (vaso.transform.localPosition.x>0.8f && vaso.transform.localPosition.z>0.15f)
        {
            tapisRoulant.speed = 0; 
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
                            mat.SetFloat("_Mode", 1);
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

            CambiaSpriteFiore();
        }
        
    }
    
    IEnumerator WaitAndMoveJoint()
    {
        yield return new WaitForSeconds(0.5f);
        canMove = true; 



    }

    void CambiaSpriteFiore()
    {
       
        Image[] immaginiFiglie = canvas.GetComponentsInChildren<Image>();
        string name1 = "fiore";
        string name2 = "neutra";
        string name3 = "su";
        // Itera attraverso tutte le immagini trovate
        foreach (Image immagine in immaginiFiglie)
        {
           
            if (immagine != null && immagine.name == name1)
            {
                oldSprite1 = immagine.sprite;
                if (isCorrect)
                {
                   
                    immagine.sprite = fuck;
                }
                else
                {
                    Sprite nuovaImmagine = Resources.Load<Sprite>(percorsoImmagine+"/f_rosso");
                    immagine.sprite = nuovaImmagine;
                }
                
            } else if (immagine != null && immagine.name == name2)
            {
                oldSprite2 = immagine.sprite;
                if (isCorrect)
                {
                    Sprite nuovaImmagine = Resources.Load<Sprite>(percorsoImmagine+"/n_verde");
                    immagine.sprite = nuovaImmagine;
                }
                else
                {
                    Sprite nuovaImmagine = Resources.Load<Sprite>(percorsoImmagine+"/n_rosso");
                    immagine.sprite = nuovaImmagine;
                }
            } else if (immagine != null && immagine.name == name3)
            {
                oldSprite3 = immagine.sprite;
                if (isCorrect)
                {
                    Sprite nuovaImmagine = Resources.Load<Sprite>(percorsoImmagine+"/s_verde");
                    immagine.sprite = nuovaImmagine;
                }
                else
                {
                    Sprite nuovaImmagine = Resources.Load<Sprite>(percorsoImmagine+"/s_rosso");
                    immagine.sprite = nuovaImmagine;
                }
                
            }
        }
        
    }



}