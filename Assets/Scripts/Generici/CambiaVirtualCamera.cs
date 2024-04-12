using UnityEngine;
using Cinemachine;

public class CambiaVirtualCamera : MonoBehaviour
{
    public CinemachineBrain cinemachineBrain;

    void Start()
    {
        // Assicurati di avere un riferimento al CinemachineBrain
        if (cinemachineBrain == null)
        {
            cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
            if (cinemachineBrain == null)
            {
                Debug.LogError("CinemachineBrain non trovato sulla camera principale.");
                return;
            }
        }
    }

    public static void AttivaCamera(CinemachineVirtualCamera nuovaVirtualCamera)
    {
        // Assicurati di avere un riferimento alla nuova virtual camera
        if (nuovaVirtualCamera == null)
        {
            Debug.LogError("La nuova virtual camera non può essere nulla.");
            return;
        }

        // Ottieni il CinemachineBrain
        
        nuovaVirtualCamera.Priority = 10; 
    }
    
    
    public static void DisattivaCamera(CinemachineVirtualCamera nuovaVirtualCamera)
    {
        // Assicurati di avere un riferimento alla nuova virtual camera
        if (nuovaVirtualCamera == null)
        {
            Debug.LogError("La nuova virtual camera non può essere nulla.");
            return;
        }

        // Ottieni il CinemachineBrain
        
        nuovaVirtualCamera.Priority = 0; 
    }
}