using UnityEngine;
using System.Collections;

public class PipeOpen : PipeBehaviour {

	// -------------------------------------------------------------------------------------
	// No obstacles in open pipes !
	// -------------------------------------------------------------------------------------

	public void Awake(){
		curved = true;		// For automatic FOV variations.
	}
}
