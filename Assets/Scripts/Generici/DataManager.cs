using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DataManager : MonoBehaviour
{
    // Dichiarare tutte le variabili che vuoi gestire
    public string nomePaz;
    public string eta;
    public string patologia;
    public int postazioneScelta;
    public int livelloScelto;
    
    public float tolleranza;
    public int tCliente;
    public int tDipendente;
    [FormerlySerializedAs("tWristManteinance")] public int tMantenimento;
    public int tBeforeSemplificazione;
    [FormerlySerializedAs("ReducedtWristManteinance")] public int ReducedTMantenimento;
    public int alternativeMovement;
    [FormerlySerializedAs("tlunaparkstart")] public int tGameStart;
    

    // Singleton pattern
    private static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("DataManager").AddComponent<DataManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // Impedire la creazione di duplicati del singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
