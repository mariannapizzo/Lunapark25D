using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using TMPro;

public class OldGameManager : MonoBehaviour
{
    public Postazione postazione;
    [SerializeField] GameObject leggiSchedina;
    [SerializeField] CinemachineVirtualCamera cameraTutorial;
    [SerializeField] Animator animatorTutorial;
    [SerializeField] GameObject aiutante;
    [SerializeField] private TMP_Text textCanvasutorial;
    [SerializeField] private RectTransform panelRectTransform;


    void Awake()
    {
        
       /* textCanvasutorial.text = "Obiettivo: tieni il polso fermo per " + DataManager.Instance.tMantenimento + " secondi";

        CambiaVirtualCamera.AttivaCamera(cameraTutorial);
        // Aspetta
        StartCoroutine(DisattivaCameraQuandoFinito());*/
        switch (DataManager.Instance.postazioneScelta)
        {
            case 1: // Fioraio
                postazione = new provaLivello1();
                break;
            case 2: // Cani
                postazione = new Cani();
                break;
            case 3: // Pinza
                postazione = new Pinza();
                break;
            case 4: // Palloncini
                postazione = new Palloncini();
                break;
            case 5: // Bar
                postazione = new Bar();
                break;
            default:
                // Gestire il caso di default o fornire un feedback in base alle esigenze del gioco
                break;
        }
       
        // Passa le informazioni alla postazione
        postazione.SetParameters(
            DataManager.Instance.livelloScelto,
            DataManager.Instance.postazioneScelta,
            DataManager.Instance.tolleranza ,
            DataManager.Instance.tCliente ,
            DataManager.Instance.tDipendente ,
            DataManager.Instance.tMantenimento ,
            DataManager.Instance.tBeforeSemplificazione ,
            DataManager.Instance.ReducedTMantenimento,
            DataManager.Instance.alternativeMovement

        );

        postazione.ActivatorKidAndHelper();
       // LeapMotionDataReader scriptLeggiSchedina = leggiSchedina.GetComponent<LeapMotionDataReader>();

        //Attiva lo script leggi schedina per la lettura dei dati
        leggiSchedina.SetActive(true);
        LetturaSchedina scriptLeggiSchedina = leggiSchedina.GetComponent<LetturaSchedina>();
        scriptLeggiSchedina.logicaGioco=postazione; 
        postazione.Start();

    }

    void FixedUpdate()
    {
        LetturaSchedina scriptLeggiSchedina = leggiSchedina.GetComponent<LetturaSchedina>();
        postazione.LogicaDiGioco_Liv1(scriptLeggiSchedina.ultimeN_presenze,
            scriptLeggiSchedina.ultimeN_intensità, scriptLeggiSchedina.ultimeN_presenzeh);
        
        //LeapMotionDataReader scriptLeggiSchedina = leggiSchedina.GetComponent<LeapMotionDataReader>();
       /*switch (DataManager.Instance.postazioneScelta)
       {
               
           case (1):
               postazione.LogicaDiGioco_Liv1(
                   scriptLeggiSchedina.GetHand(),
                   scriptLeggiSchedina.GetPresenze(),
                   scriptLeggiSchedina.GetIntensita(),
                   scriptLeggiSchedina.GetPresenzeH(),
                   scriptLeggiSchedina.GetPresenzeM()
               );
               break;
           case (2):
               postazione.LogicaDiGioco_Liv2(
                   scriptLeggiSchedina.GetHand(),
                   scriptLeggiSchedina.GetPresenze(),
                   scriptLeggiSchedina.GetIntensita(),
                   scriptLeggiSchedina.GetPresenzeH(),
                   scriptLeggiSchedina.GetPresenzeM()
               );
               break;
           case (3):
               postazione.LogicaDiGioco_Liv3(
                   scriptLeggiSchedina.GetHand(),
                   scriptLeggiSchedina.GetPresenze(),
                   scriptLeggiSchedina.GetIntensita(),
                   scriptLeggiSchedina.GetPresenzeH(),
                   scriptLeggiSchedina.GetPresenzeM()
               );
               break;
               }*/
            
        
        
    }
    
    IEnumerator DisattivaCameraQuandoFinito()
    {
        // Attendi finché la variabile "finished" diventa true
        while (!animatorTutorial.GetBool("finished"))
        {
            yield return null; // Aspetta un frame prima di controllare di nuovo la condizione
        }

        // Una volta che "finished" diventa true, disattiva la camera
        CambiaVirtualCamera.DisattivaCamera(cameraTutorial);
        Transform parentTransform = animatorTutorial.gameObject.transform.parent;
        parentTransform.gameObject.SetActive(false);
        panelRectTransform.anchoredPosition = new Vector2(-577f, -199f);
        
        aiutante.SetActive(true);
    }

}