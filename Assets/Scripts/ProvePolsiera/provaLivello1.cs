using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Cinemachine;
using Leap;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class provaLivello1 : Postazione
{

    private Animator kidArmAnimator;
    private bool IsUserInCorrectPosition = false;
    private bool movementDone = false;
    private float tStartCase1;//da modificare in tStartCase sO
    private int state = -1; // Stato iniziale: 0 - Nessun dato rilevante ancora trovato, 1 movimento fatto,
    private int statePostAiutante = -1;
    private float timeInState1 = 0f;
    private List<bool> meetsThresholdPresenzeHistory = new List<bool>();
    private Animator flowerAnimator;
    private float F = 1 / Time.fixedDeltaTime; //per il; momento 40Hz
    private LetturaSchedina scriptLetturaDati;
    private int framesManteinance;
    private int reducedFramesManteinance;
    private float reducedTManteinance = 0.2f;
    private Slider barraCompletamento;
    private int deltaTManoChiusa=1;
     
    
    
    public override void Start()
    {
        SkinnedMeshRenderer meshRendererArms = AvatarBimbo.GetComponentInChildren<SkinnedMeshRenderer>();//potrebbe non andare
        meshRendererArms.enabled = false;
        
        barraCompletamento = Object.FindObjectOfType<Slider>();
        
        kidArmAnimator = AvatarBimbo.GetComponent<Animator>();
        Transform[] oggettiDiInterazione= FindAllChildrenWithAnimator(InteractionTarget);
        if(oggettiDiInterazione.Length ==1 )
            flowerAnimator = oggettiDiInterazione[0].GetComponent<Animator>();
        GameObject goSchedina = GameObject.Find("DataReader");
        scriptLetturaDati  = goSchedina.GetComponent<LetturaSchedina>();
        framesManteinance = (int)(tManteinance * F);
        reducedFramesManteinance = (int)( reducedTManteinance * F);
        
    }

    // Update is called once per frame
    public override void LogicaDiGioco_Liv1(int[] ultimeN_presenze, int[] ultimeN_intensita, int[] ultimeN_presenzeh) //, int[] presenzem)
    {
        if (tCliente - (Time.realtimeSinceStartup - DataManager.Instance.tGameStart) <= 0)
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        Debug.Log(state);
        
        
        switch (state)
        {
            case -1:
                
                scriptLetturaDati.statisticheSingolaClasse(DC.sU, framesManteinance);
                bool meetsReq = scriptLetturaDati.singolaPresenza > (presThreshold * framesManteinance);
                barraCompletamento.value =Mathf.Clamp01(scriptLetturaDati.singolaPresenza/(presThreshold * framesManteinance)) ;
                if (meetsReq)
                {
                    state = 0;
                    Debug.Log("Posizionato correttamente");
                }
                else
                {
                    Debug.Log("NON Posizionato correttamente");
                }
                break;
                
            case 0:
                scriptLetturaDati.statisticheSingolaClasse(DC.dU, reducedFramesManteinance);
                meetsReq = scriptLetturaDati.singolaPresenza > (0.5f * reducedFramesManteinance);
                barraCompletamento.value =Mathf.Clamp01(scriptLetturaDati.singolaPresenza/(0.5f * reducedFramesManteinance)) ;
                if (meetsReq)
                {
                    state = 1; // Cambia lo stato
                }
                break;
            
            case 1:
                scriptLetturaDati.statisticheSingolaClasse(DC.mD, reducedFramesManteinance);
                meetsReq = scriptLetturaDati.singolaPresenza > (0.5f * reducedFramesManteinance);
                barraCompletamento.value =Mathf.Clamp01(scriptLetturaDati.singolaPresenza/(0.5f * reducedFramesManteinance)) ;
                // Supponiamo di richiedere per dU l'intesita' 1
               
                if (meetsReq)
                {
                    state = 2; // Cambia lo stato
                    tStartCase1 = Time.realtimeSinceStartup;
                }
                break;
            case 2:
                SkinnedMeshRenderer meshRendererArms = AvatarBimbo.GetComponentInChildren<SkinnedMeshRenderer>();//potrebbe non andare
                meshRendererArms.enabled = true;
                if (Time.realtimeSinceStartup - tStartCase1 >= tBeforeSemplificazione)
                {
                    tStartCase1 = Time.realtimeSinceStartup;
                    state= 500;
                    statePostAiutante = 101;
                }
                scriptLetturaDati.statisticheSingolaClasse(DC.sO_N, framesManteinance);
                meetsReq = scriptLetturaDati.singolaPresenza > (presThreshold * framesManteinance);
                barraCompletamento.value =Mathf.Clamp01(scriptLetturaDati.singolaPresenza/(presThreshold * framesManteinance)) ;
                
                if (meetsReq)
                {
                    scriptLetturaDati.statisticaSingolaClassePolso(DC.mF, (int)(deltaTManoChiusa *F));
                    bool meetsReqPolso = scriptLetturaDati.singolaPresenzah > (0.3f * F);//0.3f * F è il tempo per cui la flessione della mano durante la chiusura viene eseguita
                    if (meetsReqPolso)
                    {
                        Debug.Log("flessione polso");
                        if (flowerAnimator.GetBool("animationIsOn") == true)
                        {
                            Reward.gameObject.SetActive(true);
                        }
                    }
                    state = 3;
                }
                break;
            case 3: 
                AvvioAzionePresa();
                break;
            case 4: 
                flowerAnimator.SetBool("animateFlower", false);
                state = -1;
                break;
            case 500:
                MostraAiutante();
                break;
            case 101:
                cambiaCanvas(DataManager.Instance.ReducedTMantenimento);
                
                scriptLetturaDati.statisticheClassi(framesManteinance);
               
                
                meetsReq = (scriptLetturaDati.presenze[DC.sO_N]+scriptLetturaDati.presenze[DC.sO_PN]+scriptLetturaDati.presenze[DC.sO_SN]) > (presThreshold * framesManteinance);
                barraCompletamento.value =Mathf.Clamp01((scriptLetturaDati.presenze[DC.sO_N]+scriptLetturaDati.presenze[DC.sO_PN]+scriptLetturaDati.presenze[DC.sO_SN])/(presThreshold * framesManteinance)) ;
                if (meetsReq)
                    state = 3;
                break;
       }
        
    }
  
 
    
    private void AvvioAzionePresa()
    {
        
        Collider boxColliderMano = AvatarBimbo.GetComponentInChildren<BoxCollider>();
        if (boxColliderMano != null)
        {
            boxColliderMano.tag = "Hand"; 
        }
        else
        {
            Debug.LogError("BoxCollider non trovato!");
        }
        boxColliderMano.gameObject.layer = LayerMask.NameToLayer("Default");
        
        if (flowerAnimator.GetBool("animateFlower") == false)
        {
            flowerAnimator.SetBool("animateFlower", true);
            flowerAnimator.SetBool("animationIsOn", true);
        }
        kidArmAnimator.SetBool("movementDone", true);
    }

    private void MostraAiutante()
    {
        annullaLettura = true;
        Animator animatorAiutante = AvatarAiutante.GetComponentInChildren<Animator>();
        AttivaAiuto gestioneAnimator = animatorAiutante.gameObject.GetComponent<AttivaAiuto>(); 
        gestioneAnimator.AiutanteAnimatorController();
        if (gestioneAnimator != null && animatorAiutante.GetBool("AnimationIsOn") == false)
        {
            animatorAiutante.SetBool("AnimationIsOn", true);
            SkinnedMeshRenderer meshRendererArms = AvatarBimbo.GetComponentInChildren<SkinnedMeshRenderer>();//potrebbe non andare
            meshRendererArms.enabled = false;
            canvas.enabled = false;
            CambiaVirtualCamera.AttivaCamera(AvatarAiutante.parent.GetComponentInChildren<CinemachineVirtualCamera>());//potrebbe non andare
        }

        if (animatorAiutante.GetBool("isFinished") == true)
        {
            Debug.Log("esco da aiutante");
            CambiaVirtualCamera.DisattivaCamera(AvatarAiutante.parent.GetComponentInChildren<CinemachineVirtualCamera>());//potrebbe non andare
            SkinnedMeshRenderer meshRendererArms = AvatarBimbo.GetComponentInChildren<SkinnedMeshRenderer>();//potrebbe non andare
            meshRendererArms.enabled = true;
            gestioneAnimator.OriginalAnimatorController();
            animatorAiutante.SetBool("AnimationIsOn", false);
            animatorAiutante.SetBool("isFinished", false);
            state = statePostAiutante;
            annullaLettura = false;
        }
    }

   private void cambiaCanvas(int tR)
    {
        canvas.enabled = true;
        TMP_Text TMP = canvas.GetComponentInChildren<TextMeshProUGUI>();
        TMP.text = "Obiettivo: tieni il polso fermo per " + tR + " secondi";
    }
    
    public override void LogicaDiGioco_Liv2(Hand hand, int[] presenze, int[] intensità, int[] presenzeh, int[] presenzem)
    {
        
    }
    public override void LogicaDiGioco_Liv3(Hand hand, int[] presenze, int[] intensità, int[] presenzeh, int[] presenzem)
    {
        
    }
    
    public override void ActivatorKidAndHelper()
    {
        GameObject PostazioniGiochi = GameObject.Find("PostazioniGiochi");
        string postazioneSceltaStr = "FlowerStall";
        Transform gameObjPostazione = PostazioniGiochi.transform.Find(postazioneSceltaStr);

        // Attiva il gameobject "Aiutante"
        gameObjPostazione.Find("Aiutante").gameObject.SetActive(true);

        // Attiva il gameobject "Bimbo"
        gameObjPostazione.Find("KidArm").gameObject.SetActive(true);
        
        GameElementSetting(gameObjPostazione);
        //se poi volessi avere queso metodo GameElementSetting diverso per ogni classe
        //in postazione lo metto abstract e qui lo faccio override in modo che ogni classe figlia abbia
        //la propria implementazione
    }
    
    public override void SetState(int newState)
    {
        state = newState;
    }
}
