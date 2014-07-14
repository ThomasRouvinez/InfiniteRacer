using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
public class LaserGenerator : MonoBehaviour {

	LineRenderer lineRenderer;
	public Transform source;
	public Transform target;

	void Start () {
		lineRenderer= GetComponent<LineRenderer>();

		transform.Rotate(Vector3.forward, Random.Range(0,360));
	}

	void Update () {
		transform.Rotate(Vector3.forward, Time.deltaTime * 36f);

		lineRenderer.SetPosition(0,source.position);
		
		for(int i=1;i<4;i++)
		{
			Vector3 pos = Vector3.Lerp(source.position,target.position,i/4.0f);

			pos.x += Random.Range(-5.0f,5.0f);
			pos.y += Random.Range(-5f,5.0f);
			
			lineRenderer.SetPosition(i,pos);
		}

		lineRenderer.SetPosition(4,target.position);
	}
}