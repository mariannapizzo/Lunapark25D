using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LetturaDati : MonoBehaviour
{
	private static int maxnumberofclasses=24;
	private static int maxnumberofclassesh = 8;
	public int[] presenze = new int[maxnumberofclasses];
	public int[] intensità = new int[maxnumberofclasses];
	public int[] presenzeh = new int[maxnumberofclassesh];
	public int singolaPresenza=0;
	public int singolaPresenzah=0;
	public int singolaIntensita=0;
	public abstract void Start();
	public abstract void FixedUpdate();


	public abstract void statisticheSingolaClasse(int numClass, int finestra);


	public abstract void statisticaSingolaClassePolso(int numClass, int finestra);

	public abstract void statisticheClassi(int finestra);


	public void AggiungiDato(int[] array, int lungh, int nuovoDato)
	{
		
		for (int i = 0; i < lungh - 1; i++)
		{
			array[i] = array[i + 1];
		}

		// Inserire il nuovo dato nella posizione finale
		array[lungh - 1] = nuovoDato;
	}
}
