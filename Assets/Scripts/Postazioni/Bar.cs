using Leap;
using UnityEngine;

public class Bar : Postazione
{
    public override void Start()
    {
    }
    public override void  LogicaDiGioco_Liv1(int[] presenze, int[] intensità, int[] presenzeh)
    {
       
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
        string postazioneSceltaStr = "Bar"; 
        // Trova il figlio corrispondente alla postazione scelta
        Transform gameObjPostazione = PostazioniGiochi.transform.Find(postazioneSceltaStr);

        // Attiva il gameobject "Aiutante"
        gameObjPostazione.Find("Aiutante").gameObject.SetActive(true);

        // Attiva il gameobject "Bimbo"
        gameObjPostazione.Find("KidArm").gameObject.SetActive(true);
        GameElementSetting(gameObjPostazione);
    }
    
    public override void SetState(int newState)
    {
        //state = newState;
    }
}