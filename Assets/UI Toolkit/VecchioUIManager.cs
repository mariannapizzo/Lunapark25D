using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class VecchioUIManager : MonoBehaviour
{
    public VisualTreeAsset[] settingsScreens;
    private static string nomePaziente;
    private static int eta;
    private static string patologia;
    private static string postazioneScelta;
    private static string livelloScelto;
    private static int tCliente;
    private static int tDipendente;
    private static int tLunaParkStart;
    private static float tollerance;
    private static int tWristManteinance; 
    private static int tBeforeSemplificazione;
    private static int ReducedtWristManteinance; //da interfaccia
    private static int alternativeMovement;//da interfaccia
    
    private VisualElement currentScreen;

    void Start()
    {
        ShowSettingsScreen(0);
        BindButton("FirstNextPage", firstPagebuttonPressed);
    }

    void ShowSettingsScreen(int screenIndex)
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.Clear();
        settingsScreens[screenIndex].CloneTree(root);
    }

    void BindButton([NotNull] string buttonName, [NotNull] Action buttonAction)
    {
        if (buttonName == null) throw new ArgumentNullException(nameof(buttonName));
        if (buttonAction == null) throw new ArgumentNullException(nameof(buttonAction));
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Button button = root.Q<Button>(buttonName);
        button.clicked += buttonAction;
    }

    void firstPagebuttonPressed()
    {
        TextField nomePazienteField = GetCurrentTextField("NomePaz");
        nomePaziente = nomePazienteField.value;

        // Ottenere il riferimento all'IntegerField Età
        IntegerField etaField = GetCurrentIntegerField("Eta");
        eta = etaField.value;

        // Ottenere il riferimento al campo Patologia
        TextField patologiaField = GetCurrentTextField("Patologia");
        patologia = patologiaField.value;

        postazioneScelta = GetSelectedRadioButtonLabel("SelPostazione");
        

        Debug.Log(nomePaziente + " "+ eta + " "+ patologia + " "+ postazioneScelta  );

        ShowSettingsScreen(1);
        BindButton("SecondNextPage", secondPagebuttonPressed);
        BindButton("SecondPreviousPage", secondPrevPagebuttonPressed);
    }

    void secondPagebuttonPressed()
    {
        livelloScelto = GetSelectedRadioButtonLabel("SelLivello");
        

        Debug.Log(livelloScelto );
        ShowSettingsScreen(2);
        BindButton("ThirdNextPage", thirdPagebuttonPressed);
        BindButton("ThirdPreviousPage", thirdPrevPagebuttonPressed);
    }

    void thirdPagebuttonPressed()
    {
        
        IntegerField durataDipendente = GetCurrentIntegerField("TDipendente");
        IntegerField durataCliente = GetCurrentIntegerField("TCliente");
        

        // Ottenere e salvare i valori delle slider
        tCliente = durataCliente.value;
        tDipendente = durataDipendente.value;
        
        
        Debug.Log(tCliente + " "+ tDipendente );
        
        ShowSettingsScreen(3);
        BindButton("FourthNextPage", fourthPagebuttonPressed);
        BindButton("FourthPreviousPage", fourthPrevPagebuttonPressed);
    }

    void fourthPagebuttonPressed()
    {
        FloatField tolleranza = GetCurrentFloatField("Tolleranza");
        tollerance = tolleranza.value;

        // Ottenere il riferimento all'IntegerField Età
        IntegerField Twrist = GetCurrentIntegerField("tWristManteinance");
        tWristManteinance = Twrist.value;

        // Ottenere il riferimento al campo Patologia
        IntegerField TBSempl = GetCurrentIntegerField("tBeforeSemplificazione");
        tBeforeSemplificazione = TBSempl.value;
        
        IntegerField ReducedTWrist = GetCurrentIntegerField("ReducedtWristManteinance");
        ReducedtWristManteinance = ReducedTWrist.value;
        
        IntegerField AltMov = GetCurrentIntegerField("alternativeMovement");
        alternativeMovement = AltMov.value;
        
        
        tLunaParkStart=(int)Math.Round(Time.realtimeSinceStartup);
        SceneManager.LoadScene(1, LoadSceneMode.Single);

     
    }
    void secondPrevPagebuttonPressed()
    {
        ShowSettingsScreen(0);
        BindButton("FirstNextPage", firstPagebuttonPressed);
    }
    void thirdPrevPagebuttonPressed()
    {
        ShowSettingsScreen(1);//se premo il bottone indietro della terza pagina torno alla seconda pagina
        BindButton("SecondNextPage", secondPagebuttonPressed);
        BindButton("SecondPreviousPage", secondPrevPagebuttonPressed);
    }
    void fourthPrevPagebuttonPressed()
    {
        ShowSettingsScreen(2);
        BindButton("ThirdNextPage", thirdPagebuttonPressed);
        BindButton("ThirdPreviousPage", thirdPrevPagebuttonPressed);
    }

   // Metodi per prendere le informazioni dalle schermate, quindi per prendere i dati dai UXML
    private TextField GetCurrentTextField(string fieldName)
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        return root.Q<TextField>(fieldName);
    }

    private IntegerField GetCurrentIntegerField(string fieldName)
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        return root.Q<IntegerField>(fieldName);
    }

    private string GetSelectedRadioButtonLabel(string groupName)
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        GroupBox groupBox = root.Q<GroupBox>(groupName);

        if (groupBox != null)
        {
            RadioButton radioButton = groupBox.Children().OfType<RadioButton>().FirstOrDefault(r => r.value);

            if (radioButton != null)
            {
                return radioButton.label;
            }
        }

        return null;
    }

    private SliderInt GetCurrentSlider(string sliderName)
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        return root.Q<SliderInt>(sliderName);
    }
    
    private FloatField GetCurrentFloatField(string fieldName)
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        return root.Q<FloatField>(fieldName);
    }
    
    // -------------------------------------------------
    
   // Metodi per leggere il dato dalla scena Lunapark
   
   public static string GetNomePaziente()
    {
        return nomePaziente;
    }


    public static int GetEta()
    {
        return eta;
    }

    public static string GetPatologia()
    {
        return patologia;
    }

    public static string GetPostazioneScelta()
    {
        return postazioneScelta;
    }

    public static string GetLivelloScelto()
    {
        return livelloScelto;
    }

    public static int GetTCliente()
    {
        return tCliente;
    }

    public static int GetTDipendente()
    {
        return tDipendente;
    }
    
    public static int GetTLunaparkStart()
    {
        return tLunaParkStart;
    }
    
    public static float GetTolleranza()
    {
        return tollerance;
    }
    public static int GettWristManteinance()
    {
        return tWristManteinance;
    }
  
    public static int GettBeforeSemplificazione()
    {
        return tBeforeSemplificazione;
    }
    public static int GetReducedtWristManteinance()
    {
        return ReducedtWristManteinance;
    }
    
    public static int GetalternativeMovement()
    {
        return alternativeMovement;
    }
    
    
   


}