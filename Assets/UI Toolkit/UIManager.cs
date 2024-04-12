using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public VisualTreeAsset[] screens; // Un array di VisualTreeAsset per le diverse schermate
    private int choiche;
    private List<VisualTreeAsset> choicheScreens;
    private VisualElement currentScreen;
    private string postazioneScelta;
    private RadioButtonGroup radioButtonGroup;

    // Assicurati di assegnare l'elemento Text nell'editor Unity
    private Label warningNoScelta;

    private void Start()
    {
        // Inizializza il RadioButtonGroup e visualizza la prima schermata
        ShowSettingsScreen(0);
        BindButton("FirstNextPage", OnFirstForwardButtonClick);
        choicheScreens = new List<VisualTreeAsset>();
        HideWarning();
    }

    private void HideWarning()
    {
        warningNoScelta = GetComponent<UIDocument>().rootVisualElement.Q<Label>("warningNoScelta");
        warningNoScelta.style.display = DisplayStyle.None;
    }

    private void OnFirstForwardButtonClick()
    {
        // Ottieni l'indice della scelta corrente nel RadioButtonGroup
        postazioneScelta = GetSelectedRadioButtonLabel("SelPostazione");
        if (string.IsNullOrEmpty(postazioneScelta))
        {
            // L'utente non ha fatto una scelta, mostra l'avviso
            warningNoScelta.style.display = DisplayStyle.Flex;
            return; // Non procedere con l'aggiornamento della schermata
        }

        var nomePazienteField = GetCurrentTextField("NomePaz");
        var nomePaziente = nomePazienteField.value;

        // Ottenere il riferimento all'IntegerField Età
        var etaField = GetCurrentIntegerField("Eta");
        var eta = etaField.value;

        // Ottenere il riferimento al campo Patologia
        var patologiaField = GetCurrentTextField("Patologia");
        var patologia = patologiaField.value;

        postazioneScelta = GetSelectedRadioButtonLabel("SelPostazione");
        Debug.Log(postazioneScelta);


        // Passa alla schermata successiva in base alla scelta
        if (postazioneScelta == "Fiorista")
            choiche = 1;
        else if (postazioneScelta == "Mostra Canina")
            choiche = 2;
        else if (postazioneScelta == "Pinza")
            choiche = 3;
        else if (postazioneScelta == "Palloncini")
            choiche = 4;
        else if (postazioneScelta == "Bar") choiche = 5;
        DataManager.Instance.postazioneScelta = choiche; 

        ShowSettingsScreen(choiche);
        warningNoScelta = GetComponent<UIDocument>().rootVisualElement.Q<Label>("warningNoScelta");
        warningNoScelta.style.display = DisplayStyle.None;
        choicheScreens.Add(screens[choiche]);
        BindButton("SecondNextPage", OnSecondForwardButtonClick);
        BindButton("SecondPreviousPage", OnSecondPrevButtonClick);
    }

    private void OnSecondForwardButtonClick()
    {
        var livelloScelto = GetSelectedRadioButtonLabel("SelLivello");
        if (string.IsNullOrEmpty(livelloScelto))
        {
            // L'utente non ha fatto una scelta, mostra l'avviso
            warningNoScelta.style.display = DisplayStyle.Flex;
            return; // Non procedere con l'aggiornamento della schermata
        }

        switch (livelloScelto)
        {
            case "Livello 1":
                DataManager.Instance.livelloScelto = 1; 
                break;
            case "Livello 2":
                DataManager.Instance.livelloScelto = 2; 
                break;
            case "Livello 3":
                DataManager.Instance.livelloScelto = 3; 
                break;
            
        }
        Debug.Log(livelloScelto);
        ShowSettingsScreen(6);
        choicheScreens.Add(screens[6]);
        BindButton("ThirdNextPage", OnThirdForwardButtonClick);
        BindButton("ThirdPreviousPage", OnThirdPrevButtonClick);
    }

    private void OnSecondPrevButtonClick()
    {
        ShowSettingsScreen(0);
        choicheScreens.RemoveAt(choicheScreens.Count - 1);
        BindButton("FirstNextPage", OnFirstForwardButtonClick);
        HideWarning();
    }

    private void OnThirdForwardButtonClick()
    {
        var durataDipendente = GetCurrentIntegerField("TDipendente");
        var durataCliente = GetCurrentIntegerField("TCliente");


        // Ottenere e salvare i valori delle slider
        
        DataManager.Instance.tCliente = durataCliente.value;
        DataManager.Instance.tDipendente = durataDipendente.value;
        

        var fourthChoiche = choiche + 6;
        // Passa alla schermata successiva in base alla scelta
        /*if (postazioneScelta == "Fiorista")
         {
             choiche = 7;
         }
         else if (postazioneScelta == "Mostra Canina")
         {
             choiche = 8;
         }
         else if (postazioneScelta == "Pinza")
         {
             choiche = 9;
         }
         else if (postazioneScelta == "Palloncini")
         {
             choiche = 10;
         }
         else if (postazioneScelta == "Bar")
         {
            choiche = 11;
         }*/

        Debug.Log(fourthChoiche); //eventualmente aggiungere una dipendenza anche dal livello scelto, si vedrà
        ShowSettingsScreen(fourthChoiche);
        choicheScreens.Add(screens[fourthChoiche]);
        BindButton("FourthNextPage", OnFourthForwardButtonClick);
        BindButton("FourthPreviousPage", OnFourthPrevButtonClick);
    }

    private void OnThirdPrevButtonClick()
    {
        Debug.Log(choiche);
        ShowSettingsScreen(choiche);
        choicheScreens.RemoveAt(choicheScreens.Count - 1);
        BindButton("SecondNextPage", OnSecondForwardButtonClick);
        BindButton("SecondPreviousPage", OnSecondPrevButtonClick);
        HideWarning();
    }

    private void OnFourthForwardButtonClick()
    {
        var tolleranza = GetCurrentFloatField("Tolleranza");
        DataManager.Instance.tolleranza = tolleranza.value;

        // Ottenere il riferimento all'IntegerField Età
        var Twrist = GetCurrentIntegerField("tWristManteinance");
        DataManager.Instance.tMantenimento = Twrist.value;

        // Ottenere il riferimento al campo Patologia
        var TBSempl = GetCurrentIntegerField("tBeforeSemplificazione");
        DataManager.Instance.tBeforeSemplificazione  = TBSempl.value;

        var ReducedTWrist = GetCurrentIntegerField("ReducedtWristManteinance");
        DataManager.Instance.ReducedTMantenimento  = ReducedTWrist.value;

        var AltMov = GetCurrentIntegerField("alternativeMovement");
        DataManager.Instance.alternativeMovement  = AltMov.value;

        var tLunaParkStart = (int)Math.Round(Time.realtimeSinceStartup);
        DataManager.Instance.tGameStart = tLunaParkStart; 
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        //da modificare quando avremo modificato anche l'architettura del gioco
    }

    private void OnFourthPrevButtonClick()
    {
        ShowSettingsScreen(6);
        choicheScreens.RemoveAt(choicheScreens.Count - 1);
        BindButton("ThirdNextPage", OnThirdForwardButtonClick);
        BindButton("ThirdPreviousPage", OnThirdPrevButtonClick);
        HideWarning();
    }

    private void ShowSettingsScreen(int screenIndex)
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.Clear();
        screens[screenIndex].CloneTree(root);
    }

    private void BindButton([NotNull] string buttonName, [NotNull] Action buttonAction)
    {
        if (buttonName == null) throw new ArgumentNullException(nameof(buttonName));
        if (buttonAction == null) throw new ArgumentNullException(nameof(buttonAction));
        var root = GetComponent<UIDocument>().rootVisualElement;
        var button = root.Q<Button>(buttonName);
        button.clicked += buttonAction;
    }

    private string GetSelectedRadioButtonLabel(string groupName)
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var groupBox = root.Q<GroupBox>(groupName);

        if (groupBox != null)
        {
            var radioButton = groupBox.Children().OfType<RadioButton>().FirstOrDefault(r => r.value);

            if (radioButton != null) return radioButton.label;
        }

        return null;
    }

    private TextField GetCurrentTextField(string fieldName)
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        return root.Q<TextField>(fieldName);
    }

    private IntegerField GetCurrentIntegerField(string fieldName)
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        return root.Q<IntegerField>(fieldName);
    }

    private FloatField GetCurrentFloatField(string fieldName)
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        return root.Q<FloatField>(fieldName);
    }
  
}