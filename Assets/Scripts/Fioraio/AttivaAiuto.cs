using Cinemachine;
using UnityEngine;

public class AttivaAiuto : MonoBehaviour
{
    public RuntimeAnimatorController nuovoAnimatorController;
    private Animator animator;
    public CinemachineVirtualCamera virtualCameraAiutante;
    private RuntimeAnimatorController vecchioAnimatorController;
    [SerializeField] private AudioSource audioAiuto;

    void Start()
    {
        animator = GetComponent<Animator>();
        vecchioAnimatorController = animator.runtimeAnimatorController; 
    }
    

    public void AiutanteAnimatorController()
    {
        // Ottieni il componente Animator associato all'oggetto corrente
        Animator animator = GetComponent<Animator>();
        
        // Verifica se l'Animator è stato trovato
        if (animator != null)
        {
            // Cambia l'Animator Controller
            animator.runtimeAnimatorController = nuovoAnimatorController;
        }
        
    } 
    public void OnAnimationHelperComplete()
    {
            animator.SetBool("isFinished", true);
            Debug.Log("Porco Spio");
            audioAiuto.mute = false;
            audioAiuto.Play();
    }
    
    public void OriginalAnimatorController()
    {
        // Ottieni il componente Animator associato all'oggetto corrente
        Animator animator = GetComponent<Animator>();
        
        // Verifica se l'Animator è stato trovato
        if (animator != null)
        {
            // Cambia l'Animator Controller
            animator.runtimeAnimatorController = vecchioAnimatorController;
        }
       
    }

  
    
    }

    
   