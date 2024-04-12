using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AvatarManager : MonoBehaviour
{
    [SerializeField] public AudioSource Source;
    [SerializeField] public AudioClip Speech;
    [SerializeField] public int SampleLength = 1024;

    private SkinnedMeshRenderer[] SMRlist;
    private SkinnedMeshRenderer SMRenderer, SMRHead, SMRTeeth;
    private float[] SpeechSampleData;
    private int BlendshapeIndex_mouthOpen, BlendshapeIndex_TeethOpen, count;

    void Start() {
        SMRlist = GetComponentsInChildren<SkinnedMeshRenderer>();
        SMRHead = SMRlist.FirstOrDefault(smr => smr.name == "Wolf3D_Head");
        SMRTeeth = SMRlist.FirstOrDefault(smr => smr.name == "Wolf3D_Teeth");
        BlendshapeIndex_mouthOpen = SMRHead.sharedMesh.GetBlendShapeIndex("mouthOpen");
        BlendshapeIndex_TeethOpen = SMRTeeth.sharedMesh.GetBlendShapeIndex("mouthOpen");
        SpeechSampleData = new float[SampleLength];
        SMRHead.SetBlendShapeWeight (BlendshapeIndex_mouthOpen, 0f);
        SMRTeeth.SetBlendShapeWeight (BlendshapeIndex_TeethOpen, 0f);
    }

    void Update( ){
        if (Source != null && Source.isPlaying)
        {
            var v = ComputeAmplitude();
            SMRHead.SetBlendShapeWeight(BlendshapeIndex_mouthOpen, v);
            SMRTeeth.SetBlendShapeWeight(BlendshapeIndex_TeethOpen, v);
        }
    }

    public void Speak()
    {
        Source.Play();
        Debug.Log(Source.gameObject.name);
    }
    
    private float ComputeAmplitude() {
        float clipLoudness = 0f;
        
        Source.clip.GetData(SpeechSampleData, Source.timeSamples);
        foreach (float sample in SpeechSampleData) {
            clipLoudness += Mathf.Abs(sample);
        }
        clipLoudness /= SampleLength;
        
        return clipLoudness * 500;
    }
}
