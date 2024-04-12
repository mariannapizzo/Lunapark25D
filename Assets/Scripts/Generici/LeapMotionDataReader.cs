using System;
using System.Collections.Generic;
using Leap;
using Leap.Unity;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class LeapMotionDataReader: MonoBehaviour
{

    [SerializeField] private LeapProvider leapProvider;
    private static int finestra = 8;
    private List<float> wristAngles = new List<float>();
    private List<float> elbowAngles = new List<float>();
    private int i = 0;
    private static int maxnumberofclasses=24;
    private static int maxnumberofclassesh = 8;
    int[] ultimeN_presenze = new int[finestra];
    int[] ultimeN_intensità = new int[finestra];
    int[] ultimeN_presenzeh = new int[finestra];
    int[] ultimeN_presenzem = new int[finestra];
    private bool isIdle=false;
    private Animator controllerAnimator;
    private Postazione postazione;
    private Hand hand;
    


    private void Start()
    {
        Debug.Log("leap motion Attivato con successo");
    }

    private void FixedUpdate()
    {
        List<Hand> _allHands = Hands.Provider.CurrentFrame.Hands;
        if (_allHands.Count > 0)
        {
            hand = _allHands[0];
            Vector3 handVelocity = hand.PalmVelocity;
            Vector3 palmNormal = hand.PalmNormal;
            
            float projectionSpeed = Vector3.Dot(handVelocity, palmNormal); 
            // negativa : ESTENSIONE del polso
            // positiva: FLESSIONE del polso
           
            var absProjSpeed = Math.Abs(projectionSpeed);
            //Initial Pos 
            isIdle= CheckInitialPosition(hand);
           //Wrist
           Determine_PresenzeWrist(absProjSpeed, hand);
           //Elbow
           Determine_PresenzeElbow(hand);
           
           Determine_DistalHand(hand);
           
           
            
            i++;
            if (i > finestra-1)
            {
                i = 0;
            }
            
           
        }
    }
    
    bool CheckInitialPosition(Hand hand)
    {
        bool initialPos = false;
        Vector3 handVelocity = hand.PalmVelocity;
        float modHandVel = handVelocity.magnitude;
        float yHand = hand.PalmPosition.y;
        float yElbow = hand.Arm.ElbowPosition.y;
        if (yHand > yElbow && modHandVel < 0.05f)
            initialPos = true;
        return initialPos;
    }

    private void Determine_PresenzeWrist(float absProjSpeed, Hand hand)
    {
        if (isIdle)
        {
            ultimeN_presenzeh[i] = 0;
        }
        if (absProjSpeed > 0.05f)
        {
            float wristAngle = WristExtensionAngle(hand);
            wristAngles.Add(wristAngle);
            if (wristAngles.Count > 1)
            {
                int labelh = DetermineWristState();
                ultimeN_presenzeh[i] = labelh;
            }
        }
        else
        {
            ultimeN_presenzeh[i] = 0;
        }
    }
    
    private void Determine_PresenzeElbow(Hand hand)
    {
        if (isIdle)
        {
            ultimeN_presenze[i] = 10;
        }
        else
        {
            float elbowAngle = ElbowExtensionAngle(hand);
            elbowAngles.Add(elbowAngle);
            if (elbowAngles.Count > 1)
            {
                int label = DetermineElbowState();
                ultimeN_presenze[i] = label;
            }
            ultimeN_intensità[i] = 1;
        }
        
    }
    
    private void Determine_DistalHand(Hand hand)
    {
        if (hand.GrabStrength >= 0.8f) {
            ultimeN_presenzem[i] = 1;
        }
        else
        {
            ultimeN_presenzem[i] = 0;
        }
    }

    float WristExtensionAngle(Hand hand1)
    {
        Vector3 palmDir = hand1.DistalAxis();
        Vector3 forearmDir = hand1.Arm.Direction;
        Vector3 palmNormal = hand1.PalmNormal;
        Vector3 normToWristAnglePlane = hand1.RadialAxis();
        
        Vector3 forearmDirProjected = Vector3.ProjectOnPlane(forearmDir, normToWristAnglePlane);
        Vector3 palmDirProjected = Vector3.ProjectOnPlane(palmDir, normToWristAnglePlane);

        // Calcola l'angolo tra lineDirection proiettato e cameraZ, SignedAngle non ha bisogno di vettori normalizzati
        float theta = Vector3.SignedAngle(forearmDirProjected, palmDirProjected, normToWristAnglePlane);
        return theta;
    }
    
    float ElbowExtensionAngle(Hand hand1)
    {
        Vector3 forearmDir = hand1.Arm.Direction;
        Vector3 leapX = leapProvider.transform.forward;
        Vector3 leapZ = leapProvider.transform.right;
        // Proietto la direzione del braccio sul piano che ha come normale l'asse z del leap
        Vector3 lineDirectionProjected = Vector3.ProjectOnPlane(forearmDir, leapZ);
        // Calcolo l'angolo che la direzione del braccio forma con l'orizzontale per ottenere l'angolo di "elevazione" (o di abbassamento)
        // del gomito
        float alpha = Vector3.SignedAngle(lineDirectionProjected, leapX, leapZ);
        return alpha;
    }
    
    int DetermineWristState()
    {
        if (wristAngles.Count < 2)
        {
            return 0; // Non abbastanza misurazioni per calcolare la velocità angolare
        }

        float angularSpeed = wristAngles[wristAngles.Count - 1] - wristAngles[wristAngles.Count - 2];
       

        if (Mathf.Approximately(angularSpeed, 0f))
        {
            return 0; // Polso fermo
        }
        else if (angularSpeed > 0f)
        {
            return 2; // Polso in estensione
        }
        else if (angularSpeed < 0f)
        {
            return 1; // Polso in flessione
        }
        else
        {
            return 3; // Errore nell'identificazione dello stato
        }
    }
    
    int DetermineElbowState()
    {
        if (elbowAngles.Count < 2)
        {
            return 0; // Non abbastanza misurazioni per calcolare la velocità angolare
        }

        float angularSpeed = elbowAngles[elbowAngles.Count - 1] - elbowAngles[elbowAngles.Count - 2];
        if (angularSpeed < -0.3f)
        {
            return 21;
        }
        else
        {
            return 3;
        }
        
    }
    
    //------------------Da mettere in Leggi schedina quando ci sarà la polsiera---------------
    public int[] GetPresenze()
    {
        return ultimeN_presenze;
    }
    public Hand GetHand()
    {
        return hand;
    }
    public int[] GetPresenzeH()
    {
        return ultimeN_presenzeh;
    }
    public int[] GetPresenzeM()
    {
        return ultimeN_presenzem;
    }
        
    public int[] GetIntensita()
    {
        return ultimeN_intensità;
    }
}