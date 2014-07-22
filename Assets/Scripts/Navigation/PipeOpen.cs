using UnityEngine;
using System.Collections;

public class PipeOpen : PipeBehaviour {

	// -------------------------------------------------------------------------------------
	// No obstacles in open pipes !
	// -------------------------------------------------------------------------------------

	public void Awake(){
		curved = true;		// For automatic FOV variations.
	}

	/* 
 	 * This pipe is used for the player to rest and lower his/her guard. No obstacles
 	 * are spawned in these pipes.
	 */
}
