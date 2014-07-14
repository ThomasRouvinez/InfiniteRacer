using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavigationBehaviour))]
public class OpenPipeGenerator : MonoBehaviour{

	// -------------------------------------------------------------------------------------
	// Variables.
	// -------------------------------------------------------------------------------------

	public float splineRad = 200f;
	public float ringRad = 25f;
	private Transform[] lines;	
	private Transform splineParent;
	public Transform ringPrefab;
	public int ringDistanceFactor = 3;
	public Material lineMaterial;
	public int segments = 16;
	public int lineCount = 12;
	
	// -------------------------------------------------------------------------------------
	// Functions.
	// -------------------------------------------------------------------------------------

	private void GenerateSplineNodes () {
		float arc = Mathf.PI/2;

		for (int i = 0 ; i < segments; i++){
			float bias = (i/(float)segments) * arc;
			GameObject node = GetComponent<NavigationBehaviour>().spline.AddSplineNode();
			node.name = "" + i;
			
			node.transform.parent = splineParent;
			node.transform.localPosition = new Vector3(Mathf.Cos(bias)*splineRad-splineRad,0f,Mathf.Sin(bias)*splineRad);
			node.transform.localRotation = Quaternion.Euler(new Vector3(0f,bias*-Mathf.Rad2Deg/*-90*/,0f));
			
		}

		GetComponent<NavigationBehaviour>().spline.UpdateSpline();
	}
	
	private Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle){
    	return angle * (point - pivot) + pivot;
    }

	void Awake () {
		splineParent = new GameObject("Spline").transform;
		splineParent.parent=transform;
		splineParent.localPosition=Vector3.zero;
		splineParent.localRotation=Quaternion.identity;
		
		GenerateSplineNodes();
	}

	// -------------------------------------------------------------------------------------
	// Initialization.
	// -------------------------------------------------------------------------------------

	void Start () {
		for(int i = 0 ; i < (segments-1)/ringDistanceFactor +1 ; i++){
			SplineNode node = GetComponent<NavigationBehaviour>().spline.SplineNodes[i * ringDistanceFactor];
			((Transform) Instantiate(ringPrefab,node.Position,node.Rotation)).parent = transform;
		}
		
		lines = new Transform[lineCount];
		
		for (int i = 0 ; i < lineCount ; i++){
			Transform line = new GameObject("Line").transform;
			line.parent = transform;
			line.localPosition = Vector3.zero;
			line.localRotation = Quaternion.identity;
			line.gameObject.AddComponent<LineRenderer>().material=lineMaterial;
			lines[i]=line;
			float rBias = (i / (float)lineCount) * (2 * Mathf.PI);
			lines[i].gameObject.GetComponent<LineRenderer>().SetVertexCount(segments);

			for (int j=0; j<segments; j++){
				SplineNode node=GetComponent<NavigationBehaviour>().spline.SplineNodes[j];
				lines[i].gameObject.GetComponent<LineRenderer>().useWorldSpace=false;
				lines[i].gameObject.GetComponent<LineRenderer>().SetPosition(j,
					node.transform.localPosition+RotateAroundPoint(new Vector3(Mathf.Cos(rBias)*ringRad,Mathf.Sin(rBias)*ringRad, 0f), Vector3.zero, node.transform.localRotation));
			}
		}
	}
}