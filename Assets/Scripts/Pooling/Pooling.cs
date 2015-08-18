using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Author : Thomas Rouvinez
 * Description : class to handle pooling of objects.
 * 
 */
public class Pooling : MonoBehaviour {

	// -------------------------------------------------------------------------------------
	// Initializes a single instance.
	// -------------------------------------------------------------------------------------

	static Pooling instance = null;
	static readonly object padlock = new object();
	
	Pooling(){}
	
	public static Pooling Instance{
		get{
			if (instance == null){
				lock (padlock){
					if (instance == null){
						instance = new Pooling();
					}
				}
			}
			
			return instance;
		}
	}

	// -------------------------------------------------------------------------------------
	// Variables.
	// -------------------------------------------------------------------------------------

	public GameObject[] refObjects;
	public int [] poolSizes;

	private List<GameObject>[] pools;
	private Dictionary<string, int> index;

	// -------------------------------------------------------------------------------------
	// Initialization.
	// -------------------------------------------------------------------------------------

	void Start () {
		// Instantiate all objects.
		pools = new List<GameObject>[refObjects.Length];

		for(int i = 0 ; i < refObjects.Length ; i++){
			pools[i] = new List<GameObject>();

			for(int j = 0 ; j < poolSizes[i]; j++){
				GameObject obj = (GameObject)Instantiate(refObjects[i]);
				obj.SetActive(false);
				pools[i].Add(obj);
			}
		}

		// Create a hashmap for easy object retrieval.
		index = new Dictionary<string, int>();

		for(int i = 0 ; i < refObjects.Length ; i++){
			index.Add(refObjects[i].tag, i);
		}
	}

	// -------------------------------------------------------------------------------------
	// Functions.
	// -------------------------------------------------------------------------------------

	public GameObject getOrCreate(GameObject reference){
		if(pools[index[reference.tag]].Count > 0){
			// Select last in list and give object.
			int lastIndex = pools[index[reference.tag]].Count-1;
			GameObject selected = pools[index[reference.tag]][lastIndex];
			pools[index[reference.tag]].RemoveAt(lastIndex);

			selected.SetActive(true);
			return selected;
		}
		else{
			// Object not present or available, instantiate a new one.
			return (GameObject)Instantiate(refObjects[index[reference.tag]]);
		}
	}

	public void destroy(GameObject reference){
		reference.SetActive(false);
		pools[index[reference.tag]].Add(reference);
	}
}
