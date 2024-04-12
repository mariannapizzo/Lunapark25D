using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Collections;
using System.Threading;

using UnityEngine.SceneManagement;

public class LetturaSchedina : LetturaDati
{
	public static SerialPort sp;
	public  Postazione logicaGioco;
	private static float F; //per il; momento 40Hz,  F = 1 / Time.fixedDeltaTime;
	private static int maxT = 5;
	private static int maxFinestra;
	public int label;
	private int labelh;
	public int[] ultimeN_presenze;
	public int[] ultimeN_intensità;
	public int[] ultimeN_presenzeh;
	private int i = 0;
	private byte[] B1;

	public long time1;
	private long time2;
	public long difference;
	public long predifference;
	

	public override void Start()
	{
		F = 1 / Time.fixedDeltaTime;
		maxFinestra = (int)F * maxT;
		ultimeN_presenze = new int[maxFinestra];
		ultimeN_intensità = new int[maxFinestra];
		ultimeN_presenzeh = new int[maxFinestra];
		sp = new SerialPort("COM10", 921600, Parity.None, 8, StopBits.One);
		sp.Open();
		
		Array.Clear(presenze, 0, presenze.Length);//Questo metodo della classe Array in C# imposta gli elementi dell'
                                            //array specificato a un valore predefinito. Prende in input l'array da
                                            //pulire, l'indice da cui iniziare a cancellare e la lunghezza dell'
                                            //intervallo da cancellare.
		Array.Clear(intensità, 0, intensità.Length);
		Array.Clear(presenzeh, 0, presenzeh.Length);
	}

	public override void FixedUpdate()
	{
		// B1 è per l'identificazione del dispositivo
		// B2 è per la lunghezza della parte dati
		// B3 avambraccio senza pronosupinazione 
		// B4 polso 
		// B5 avambraccio pronosupinazione 
		byte[] B1 = BitConverter.GetBytes(0); //BitConverter.GetBytes(0) sta cercando di
                                        //ottenere la rappresentazione in byte del numero intero zero e lo assegna
                                        //a un array di byte chiamato B1.
		byte[] B2 = BitConverter.GetBytes(0);
		byte[] B3 = BitConverter.GetBytes(0);
		byte[] B4 = BitConverter.GetBytes(0);
		byte[] B5 = BitConverter.GetBytes(0);
		byte[] B_useless = BitConverter.GetBytes(0);

		time2 = DateTimeOffset.Now.ToUnixTimeMilliseconds(); // salva il timestamp attuale in millisecondi nella variabile
                                                       // time2. Questo viene utilizzato successivamente nel codice per
                                                       // calcolare differenze di tempo o valutare quanto tempo è trascorso da un certo evento.
		predifference = time2 - time1;

		time1 = DateTimeOffset.Now.ToUnixTimeMilliseconds();

		while (sp.BytesToRead >= 6) //sp.BytesToRead è una proprietà di SerialPort in C# che restituisce il numero di byte
                          //attualmente disponibili per la lettura dal buffer della porta seriale.
		//Quindi, if (sp.BytesToRead > 0) controlla se ci sono byte disponibili per la lettura sulla porta seriale sp.
		//Se il numero di byte disponibili è maggiore di zero, significa che ci sono dati in arrivo sulla porta seriale
		//pronti per essere letti e il codice all'interno del blocco if verrà eseguito per gestirli.
		{		
			sp.Read(B1, 0, 1); //sp.Read(B1, 0, 1) legge byte dalla porta seriale e li memorizza nell'array di byte B1, 1 è il numero di byte letti
			if (BitConverter.ToInt32(B1, 0) == 2) //L'istruzione if (BitConverter.ToInt32(B1, 0) == 1) converte un array
                                         //di byte (B1) in un valore di tipo int e verifica se quel valore convertito è uguale a 2 (seconda versione e' stato cambiato).
										// questo if è sull'identificazione del dispositivo
			{
				sp.Read(B2, 0, 1);
				if (BitConverter.ToInt32(B2, 0) !=
				    0) //se la lunghezza della parte dati è diversa da 0 leggo i byte per lo stato dell'avambraccio
				{
					sp.Read(B3, 0, 1);
					sp.Read(B4, 0, 1);
					sp.Read(B5, 0, 1);
					sp.Read(B_useless, 0, 1);

					if (logicaGioco.annullaLettura)
					{
						AggiungiDato(ultimeN_intensità, maxFinestra, 0);
						AggiungiDato(ultimeN_presenze, maxFinestra,0);
						AggiungiDato(ultimeN_presenzeh, maxFinestra, 3);
					}
					else
					{
						label = BitConverter.ToInt32(B3, 0); //label è per lo stato dell'avambraccio
						labelh = BitConverter.ToInt32(B4, 0); //labelh per lo stato del polso
						if (label >= 16) // ci entra quando l'avambraccio è in movimento
						{
							if (label >= 48) // ci entra se l'avambraccio si muove con velocità veloce
							{
								AggiungiDato(ultimeN_intensità, maxFinestra, 3);
								AggiungiDato(ultimeN_presenze, maxFinestra,
									label - 32); //inserisce la classificazione a meno della velocità
								//32 distanza tra classificazione a velocità veloce e lenta
							}
							else if (label >= 32)
							{
								AggiungiDato(ultimeN_intensità, maxFinestra, 2);
								AggiungiDato(ultimeN_presenze, maxFinestra,
									label - 16); //16 distanza tra classificazione a velocità normale e lenta
							}
							else
							{
								AggiungiDato(ultimeN_intensità, maxFinestra, 1);
								AggiungiDato(ultimeN_presenze, maxFinestra, label);
							}

						}
						// avambraccio
						// in intensità vanno le velocità
						// in presenze le classificazioni 
						else
						{
							AggiungiDato(ultimeN_intensità, maxFinestra, 0);
							AggiungiDato(ultimeN_presenze, maxFinestra, label);
						}

						if (labelh == 7)
						{
							AggiungiDato(ultimeN_presenzeh, maxFinestra, 3);
						}
						else
						{
							AggiungiDato(ultimeN_presenzeh, maxFinestra, labelh);
						}
					}


					time2 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
					difference = time2 - time1;
				}
			}
			else { }
		}
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
	
	
	
		
}