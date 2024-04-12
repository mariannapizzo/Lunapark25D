using System;
using UnityEngine; 
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.IO;
using Object = System.Object;


public class LetturaDaTastiera : LetturaDati
{
    public InputActionAsset Azioni;
    public static float F=40; //per il; momento 40Hz,  F = 1 / Time.fixedDeltaTime;
    private static int maxT = 5;
    private static int maxFinestra;

    public int[] ultimeN_presenze;
    public int[] ultimeN_presenzeh;
    public int[] ultimeN_intensità;
    
    
    private InputActionMap keyboardForearm; 
    private InputActionMap KeyboardWrist; 
    private InputActionMap KeyboardIntensity; 
    private bool isHolding = false; 
    private bool isHoldingPolso = false; 
    private bool isHoldingIntensità = false; 
    public int currentAdd=0;
    public int currentAddPolso = 3; 
    public int currentAddIntensità = 0; 
 
  
    
    public override void Start()
    {
        // F = 1 / Time.fixedDeltaTime;
        maxFinestra = (int)F * maxT;
        ultimeN_presenze = new int[maxFinestra];
        ultimeN_presenzeh = new int[maxFinestra];
        ultimeN_intensità = new int[maxFinestra];
        Array.Clear(presenze, 0, presenze.Length);
        Array.Clear(presenzeh, 0, presenzeh.Length);
        Array.Clear(intensità, 0, intensità.Length);
    }


    private void Awake()
    {
       
        keyboardForearm = Azioni.FindActionMap("KeyboardForearm");
        KeyboardWrist = Azioni.FindActionMap("KeyboardWrist");
        KeyboardIntensity = Azioni.FindActionMap("KeyboardIntensity");
         
        KeyboardIntensity["Intensità1"].started += ctx =>
        {
            SetCurrentAddIntensità(1);
           
        };
        KeyboardIntensity["Intensità2"].started += ctx =>
        {
            SetCurrentAddIntensità(2);
        };
        KeyboardIntensity["Intensità3"].started += ctx =>
        {
            SetCurrentAddIntensità(3);
        };
        
        
        keyboardForearm["sO_P"].started += ctx =>
        {
            SetCurrentAdd(DC.sO_P);
        };
        keyboardForearm["sO_PN"].started += ctx =>
        {
            SetCurrentAdd(DC.sO_PN);
        };
        keyboardForearm["sO_N"].started += ctx =>
        {
            SetCurrentAdd(DC.sO_N);
        };
        keyboardForearm["sO_SN"].started += ctx =>
        {
            SetCurrentAdd(DC.sO_SN);
        };
        keyboardForearm["sO_S"].started += ctx =>
        {
            SetCurrentAdd(DC.sO_S);
        };
        keyboardForearm["sD"].started += ctx =>
        {
            SetCurrentAdd(DC.sD);
        };
        keyboardForearm["sU"].started += ctx =>
        {
            SetCurrentAdd(DC.sU);
        };
        keyboardForearm["mU"].started += ctx =>
        {
            SetCurrentAdd(DC.mU);
        };
        keyboardForearm["mD"].started += ctx =>
        {
            SetCurrentAdd(DC.mD);
        };
        keyboardForearm["mR"].started += ctx =>
        {
            SetCurrentAdd(DC.mR);
        };
        keyboardForearm["mL"].started += ctx =>
        {
            SetCurrentAdd(DC.mL);
        };
        keyboardForearm["dU"].started += ctx =>
        {
            SetCurrentAdd(DC.dU);
        };
        keyboardForearm["dD"].started += ctx =>
        {
            SetCurrentAdd(DC.dD);
        };
        keyboardForearm["rA_CW"].started += ctx =>
        {
            SetCurrentAdd(DC.rA_CW);
        };
        keyboardForearm["rA_CCW"].started += ctx =>
        {
            SetCurrentAdd(DC.rA_CCW);
        };
        
        KeyboardWrist["s"].started += ctx =>
        {
            SetCurrentAddPolso(DC.s);
        };
        KeyboardWrist["mE"].started += ctx =>
        {
            SetCurrentAddPolso(DC.mE );
        };
        KeyboardWrist["mF"].started += ctx =>
        {
            SetCurrentAddPolso(DC.mF );
        };
        
        /*----------------------------------------------PERFORMED------------------------------------------------*/
        KeyboardIntensity["Intensità1"].performed += ctx =>
        {
            isHoldingIntensità = true; 
           
            ContinuaAggiungereDatoIntensità();
        };
        KeyboardIntensity["Intensità2"].performed += ctx =>
        {
            isHoldingIntensità = true;
            ContinuaAggiungereDatoIntensità();
        };
        KeyboardIntensity["Intensità3"].performed += ctx =>
        {
            isHoldingIntensità = true; 
            ContinuaAggiungereDatoIntensità();
        };
        
        
        
        keyboardForearm["sO_P"].performed += ctx =>
        {
            isHolding = true;
            ContinuaAggiungereDato();
        };
        keyboardForearm["sO_PN"].performed += ctx =>
        {
           isHolding = true; 
           ContinuaAggiungereDato();
        };
        keyboardForearm["sO_N"].performed += ctx =>
        {
            isHolding = true;
            ContinuaAggiungereDato();
        };
        keyboardForearm["sO_SN"].performed += ctx =>
        {
            isHolding = true;
            ContinuaAggiungereDato();
        };
        keyboardForearm["sO_S"].performed += ctx =>
        {
            isHolding = true; 
            ContinuaAggiungereDato();
        };
        keyboardForearm["sD"].performed += ctx =>
        {
            isHolding = true;
            ContinuaAggiungereDato();
        };
        keyboardForearm["sU"].performed += ctx =>
        {
           isHolding = true;
           ContinuaAggiungereDato();
        };
        keyboardForearm["mU"].performed += ctx =>
        {
            isHolding = true;
            ContinuaAggiungereDato();
        };
        keyboardForearm["mD"].performed += ctx =>
        {
            isHolding = true;
            ContinuaAggiungereDato();
        };
        keyboardForearm["mR"].performed += ctx =>
        {
            isHolding = true;
            ContinuaAggiungereDato();
        };
        keyboardForearm["mL"].performed += ctx =>
        {
           isHolding = true;
           ContinuaAggiungereDato();
        };
        keyboardForearm["dU"].performed += ctx =>
        {
            isHolding = true;
            ContinuaAggiungereDato();
        };
        keyboardForearm["dD"].performed += ctx =>
        {
            isHolding = true;
            ContinuaAggiungereDato();
        };
        keyboardForearm["rA_CW"].performed += ctx =>
        {
            isHolding = true;
            ContinuaAggiungereDato();
        };
        keyboardForearm["rA_CCW"].performed += ctx =>
        {
            isHolding = true;
            ContinuaAggiungereDato();
        };
        //
        KeyboardWrist["s"].performed += ctx =>
        {
            isHoldingPolso = true;
            ContinuaAggiungereDatoPolso();
        };
        KeyboardWrist["mE"].performed += ctx =>
        {
            isHoldingPolso = true;
            ContinuaAggiungereDatoPolso();
        };
        KeyboardWrist["mF"].performed += ctx =>
        {
            isHoldingPolso = true;
            ContinuaAggiungereDatoPolso();
        };
       
        
        KeyboardIntensity["Intensità1"].canceled += ctx => RimossoIntensità();
        KeyboardIntensity["Intensità2"].canceled += ctx => RimossoIntensità();
        KeyboardIntensity["Intensità3"].canceled += ctx => RimossoIntensità();
        keyboardForearm["sO_P"].canceled += ctx => Rimosso();
        keyboardForearm["sO_PN"].canceled += ctx => Rimosso();
        keyboardForearm["sO_N"].canceled += ctx => Rimosso();
        keyboardForearm["sO_SN"].canceled += ctx => Rimosso();
        keyboardForearm["sO_S"].canceled += ctx => Rimosso();
        keyboardForearm["sD"].canceled += ctx => Rimosso();
        keyboardForearm["sU"].canceled += ctx => Rimosso();
        keyboardForearm["mU"].canceled += ctx => Rimosso();
        keyboardForearm["mD"].canceled += ctx => Rimosso();
        keyboardForearm["mR"].canceled += ctx => Rimosso();
        keyboardForearm["mL"].canceled += ctx => Rimosso();
        keyboardForearm["dU"].canceled += ctx => Rimosso();
        keyboardForearm["dD"].canceled += ctx => Rimosso();
        keyboardForearm["rA_CW"].canceled += ctx => Rimosso();
        keyboardForearm["rA_CCW"].canceled += ctx => Rimosso();
        KeyboardWrist["s"].canceled += ctx => RimossoPolso();;
        KeyboardWrist["mE"].canceled += ctx => RimossoPolso();
        KeyboardWrist["mF"].canceled += ctx => RimossoPolso();
    }

    
    
    
    public override void FixedUpdate()
    {
        
    }

    
    private void OnEnable()
    {
        keyboardForearm.Enable();
        KeyboardWrist.Enable();
        KeyboardIntensity.Enable();
    }

    private void Rimosso()
    {
        isHolding = false;
    }
    private void RimossoPolso()
    {
        isHoldingPolso= false;
    }
    private void RimossoIntensità()
    {
        isHoldingIntensità= false;
    }

    void SetCurrentAdd(int classe)
    {
        currentAdd = classe; 
    }
    void SetCurrentAddPolso(int classePolso)
    {
        currentAddPolso = classePolso; 
    }
    void SetCurrentAddIntensità(int classeIntensità)
    {
        currentAddIntensità = classeIntensità; 
    }
    private void ContinuaAggiungereDato()
    {
        if (isHolding)
        { 
            AggiungiDato(ultimeN_presenze, maxFinestra, currentAdd );
            Invoke(nameof(ContinuaAggiungereDato), 1/40f);
        }
    }
    
    private void ContinuaAggiungereDatoPolso()
    {
        if (isHoldingPolso)
        { 
            AggiungiDato(ultimeN_presenzeh, maxFinestra, currentAddPolso);
            Invoke(nameof(ContinuaAggiungereDatoPolso), 1/40f);
        }
    }
    
    private void ContinuaAggiungereDatoIntensità()
    {
        if (isHoldingIntensità)
        { 
            AggiungiDato(ultimeN_intensità, maxFinestra, currentAddIntensità);
            Invoke(nameof(ContinuaAggiungereDatoIntensità), 1/40f);
        }
    }

    private void OnDisable()
    {
        keyboardForearm.Disable();
        KeyboardWrist.Disable();
        KeyboardIntensity.Disable();
    }
    
    public override void statisticheSingolaClasse(int numClass, int finestra)
    {
        singolaPresenza = 0;
        singolaIntensita = 0;
        for (int j = maxFinestra-finestra; j < maxFinestra; j++)
        {
            if (ultimeN_presenze[j] == numClass)
            {
                singolaPresenza++;
                singolaIntensita+=ultimeN_intensità[j];
            }
        }
    }

    public override void statisticaSingolaClassePolso(int numClass, int finestra)
    {
        singolaPresenzah = 0;
        for (int j = maxFinestra-finestra; j < maxFinestra; j++)
        {
            if (ultimeN_presenzeh[j]== numClass)
            {
                singolaPresenzah++;
            }
        }
    }

    public override void statisticheClassi(int finestra)
    {
        Array.Clear(presenze, 0, presenze.Length);
        Array.Clear(presenzeh, 0, presenzeh.Length);
        Array.Clear(intensità, 0, intensità.Length);
     
      
        for (int j = maxFinestra-finestra; j < maxFinestra; j++)
        {
            
            presenze[ultimeN_presenze[j]]++; 
            intensità[ultimeN_presenze[j]]+=ultimeN_intensità[j];
            presenzeh[ultimeN_presenzeh[j]]++;
        }
    }
    
    
   
    public void ClearArray()
    {
        Array.Clear(ultimeN_presenze,0, ultimeN_presenze.Length);
        Array.Clear(ultimeN_presenzeh,0, ultimeN_presenze.Length);
        Array.Clear(ultimeN_intensità,0, ultimeN_presenze.Length);
    }
    
}
