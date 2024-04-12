using System.Collections;
using System.Collections.Generic;
using ReadyPlayerMe.Core;
using UnityEngine;

public class Parla : MonoBehaviour
{
    public VoiceHandler vH;
    private Animator animator;
    [SerializeField] AudioClip ciao;
    [SerializeField] AudioClip IoSono;
    [SerializeField] AudioClip OraTi;
    [SerializeField] AudioClip solleva;
    [SerializeField] AudioClip avambvsbasso;
    [SerializeField] AudioClip bracciooriz;
    [SerializeField] AudioClip ricompensaspec;
    [SerializeField] AudioClip oraprovatu;
    [SerializeField] Animator animatorTutorial;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void AudioCiao()
    {
            vH.PlayAudioClip(ciao);
    }

    void AudioIoSono()
    {
        vH.PlayAudioClip(IoSono);
    }
    void AudioOraTiFaccioVedere()
    {
        vH.PlayAudioClip(OraTi);
    }
    void AudioSolleva()
    {
       animator.SetLayerWeight(1, 1.0f);
       animator.SetLayerWeight(2, 1.0f);
        vH.PlayAudioClip(solleva);
    }
    void AudioAvambVsBasso()
    {
        vH.PlayAudioClip(avambvsbasso);
    }
    void AudioBraccioOriz()
    {
        vH.PlayAudioClip(bracciooriz);
    }
    void AudioRicompensa()
    {
        vH.PlayAudioClip(ricompensaspec);
    }
    void AudioProvaTu()
    {
        vH.PlayAudioClip(oraprovatu);
    }
    void OnFinished()
    {
        animatorTutorial.SetBool("finished", true);
    }
}

