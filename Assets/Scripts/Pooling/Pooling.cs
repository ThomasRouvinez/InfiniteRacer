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
	// Variables.
	// -------------------------------------------------------------------------------------

	public NavigationBehaviour[] refObjects;
	public int [] poolSizes;

	private List<NavigationBehaviour>[] pools;

	// -------------------------------------------------------------------------------------
	// Initialization.
	// -------------------------------------------------------------------------------------

	void Start () {
		// Instantiate all objects.
		pools = new List<NavigationBehaviour>[refObjects.Length];

		for(int i = 0 ; i < refObjects.Length ; i++){
			pools[i] = new List<NavigationBehaviour>();

			for(int j = 0 ; j < poolSizes[i]; j++){
				NavigationBehaviour obj = Instantiate(refObjects[i], new Vector3(0f,0f,0f), Quaternion.identity) as NavigationBehaviour;
				obj.gameObject.SetActive(false);
				pools[i].Add(obj);
			}
		}
	}

	// -------------------------------------------------------------------------------------
	// Functions.
	// -------------------------------------------------------------------------------------

	public NavigationBehaviour getOrCreate(int reference){
		Debug.Log("TAG " + reference);

		if(pools[reference].Count < 0){
			Debug.Log("NEW ASSET CREATED");
			return Instantiate(refObjects[reference], new Vector3(0f,0f,0f), Quaternion.identity) as NavigationBehaviour;
		}

		// Select last in list and give object.
		int lastIndex = pools[reference].Count-1;
		NavigationBehaviour selected = pools[reference][lastIndex];
		pools[reference].RemoveAt(lastIndex);
		
		selected.gameObject.SetActive(true);
		return selected;
	}

	public void destroy(NavigationBehaviour pooledObject, int reference){
		pooledObject.gameObject.SetActive(false);
		pooledObject.transform.parent = null;
		pools[reference].Add(pooledObject);
	}
}
