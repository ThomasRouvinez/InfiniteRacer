using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstaclesPooling : MonoBehaviour {
	
	// -------------------------------------------------------------------------------------
	// Variables.
	// -------------------------------------------------------------------------------------
	
	public GameObject[] refObjects;
	public int [] poolSizes;
	
	private List<GameObject>[] pools;
	
	// -------------------------------------------------------------------------------------
	// Initialization.
	// -------------------------------------------------------------------------------------
	
	void Start () {
		// Instantiate all objects.
		pools = new List<GameObject>[refObjects.Length];
		
		for(int i = 0 ; i < refObjects.Length ; i++){
			pools[i] = new List<GameObject>();
			
			for(int j = 0 ; j < poolSizes[i]; j++){
				GameObject obj = Instantiate(refObjects[i], new Vector3(0f,0f,0f), Quaternion.identity) as GameObject;
				obj.SetActive(false);
				pools[i].Add(obj);
			}
		}
	}
	
	// -------------------------------------------------------------------------------------
	// Functions.
	// -------------------------------------------------------------------------------------
	
	public GameObject getOrCreate(int reference){
		Debug.Log("CALL");

		if(pools[reference].Count < 1){
			Debug.Log("NEW ASSET REQUIRED FOR OBSTACLE POOL");
			return Instantiate(refObjects[reference], new Vector3(0f,0f,0f), Quaternion.identity) as GameObject;
		}
		
		// Select last in list and give object.
		int lastIndex = pools[reference].Count-1;
		GameObject selected = pools[reference][lastIndex];
		pools[reference].RemoveAt(lastIndex);
		
		selected.SetActive(true);
		return selected;
	}
	
	public void destroy(GameObject pooledObject, int reference){
		pooledObject.SetActive(false);
		pooledObject.transform.parent = null;
		pools[reference].Add(pooledObject);
	}
}
