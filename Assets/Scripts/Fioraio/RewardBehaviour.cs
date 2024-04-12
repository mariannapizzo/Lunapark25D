using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBehaviour : MonoBehaviour
{
    
    void OnStart()
    {
        SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(true);

        // Disabilita tutti i SkinnedMeshRenderer ottenuti
        foreach (SkinnedMeshRenderer smr in skinnedMeshRenderers)
        {
            smr.enabled = true;
        }
    }
    void OnHide()
    {
        SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(true);

        // Disabilita tutti i SkinnedMeshRenderer ottenuti
        foreach (SkinnedMeshRenderer smr in skinnedMeshRenderers)
        {
            smr.enabled = false;
        }
    }
    void OnAnimationComplete()
    {
        transform.parent.gameObject.SetActive(false);

    }
}