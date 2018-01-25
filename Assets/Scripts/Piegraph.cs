using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Piegraph : MonoBehaviour {

    public float[] values;
    public Color[] wedgeColors;
    public Image wedgePrefeb;

    private CanvasManagerScript canvasManager;

    // Use this for initialization
    void Start () {
        MakeGraph();
	}

    private void Awake()
    {
        canvasManager = transform.parent.GetComponent<CanvasManagerScript>();
    }

    void MakeGraph(){
        float total = 0f;
        float zRotation = 0f;
        for (int i = 0; i < values.Length; i++){
            total += values[i];
        }

        for (int i = 0; i < values.Length; i++){
            Image newWedge = Instantiate(wedgePrefeb) as Image;
            newWedge.transform.SetParent(transform, false);
            newWedge.color = wedgeColors[i];
            newWedge.fillAmount = values[i] / total;
            newWedge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));
            zRotation -= newWedge.fillAmount * 360f;
        }
    }

    void ClearGraph()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Wedge(Clone)")
            {
                Destroy(child.gameObject);
            }
        }
    }

    // Ask the canvas manager if there is any new data
    private void Update()
    {
        if (canvasManager.dataChanged)
        {
            ClearGraph();
            values = canvasManager.values;
            MakeGraph();
        }
    }
}
