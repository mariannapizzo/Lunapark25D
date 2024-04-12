using System.Collections.Generic;
using Leap;
using UnityEngine;


public abstract class Postazione
{
    protected int livelloScelto;
    protected int postazioneScelta;
    protected float presThreshold;
    protected int tCliente;
    protected int tDipendente;
    protected int tManteinance;
    protected float tBeforeSemplificazione;
    protected int ReducedTManteinance;
    protected int alternativeMovement;
    public bool annullaLettura;

    protected Transform AvatarBimbo;
    protected Transform InteractionTarget;
    protected Transform AvatarAiutante;
    protected Transform Reward;
    
    protected Canvas canvas;

    
   public void SetParameters(int livelloScelto, int postazioneScelta, float presThreshold, int tCliente, int tDipendente, int tManteinance, float tBeforeSemplificazione, int ReducedTManteinance, int alternativeMovement)
    {
        this.livelloScelto = livelloScelto;
        this.postazioneScelta = postazioneScelta;
        this.presThreshold = presThreshold;
        this.tCliente = tCliente;
        this.tDipendente = tDipendente;
        this.tManteinance = tManteinance;
        this.tBeforeSemplificazione = tBeforeSemplificazione;
        this.ReducedTManteinance = ReducedTManteinance;
        this.alternativeMovement = alternativeMovement;
    }

    public abstract void Start();
    public abstract void LogicaDiGioco_Liv1(int[] presenze, int[] intensità, int[] presenzeh); // int[] presenzem);
    public abstract void LogicaDiGioco_Liv2(Hand hand, int[] presenze, int[] intensità, int[] presenzeh, int[] presenzem);
    public abstract void LogicaDiGioco_Liv3(Hand hand, int[] presenze, int[] intensità, int[] presenzeh, int[] presenzem);

    public abstract void ActivatorKidAndHelper();
    public abstract void SetState(int newState);

    protected void GameElementSetting(Transform gameObjPostazione)
    {
        //AvatarBimbo setting
        AvatarBimbo = FindChildWithAnimator(gameObjPostazione.Find("KidArm"));

        //Interaction Target setting
        InteractionTarget = gameObjPostazione.Find("InteractionTarget");

        //Avatar Aiutante setting 
        AvatarAiutante = FindChildWithAnimator(gameObjPostazione.Find("Aiutante"));
        //Reward setting 
        Reward = gameObjPostazione.Find("Reward");
        
        canvas = Object.FindObjectOfType<Canvas>();
    }

    protected Transform[] FindAllChildrenWithAnimator(Transform parent)
    {
        List<Transform> OggettiConAnimator = new List<Transform>();

        foreach (Transform child in parent)
        {
            Animator childAnimator = child.GetComponent<Animator>();
            if (childAnimator != null)
            {
                // Aggiungi gli Animator all'elenco
                OggettiConAnimator.Add(child);
            }
        }

        // Restituisci un array di Animator trovati
        return OggettiConAnimator.ToArray();
    }

    // Funzione per trovare il figlio di un oggetto che ha attaccato un componente Animator
    protected Transform FindChildWithAnimator(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.GetComponent<Animator>() != null)
            {
                // Restituisci il primo figlio che ha un componente Animator
                return child;
            }
        }
        // Se nessun figlio ha un componente Animator, restituisci null
        return null;
    }

    
}