using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BarChart : MonoBehaviour {

    public Bar barPrefab;
    public float[] inputValues;
    public string[] labels;
    public Color[] colors;

    List<Bar> bars = new List<Bar>();

    float chartHeight;

    private CanvasManagerScript canvasManager;

	// Use this for initialization
	void Start () {
        chartHeight = Screen.height + GetComponent<RectTransform>().sizeDelta.y;
        //        float[] values = { 0.1f, 0.2f, 0.7f, 1.0f };
//        DisplayGraph(values);
        DisplayGraph(inputValues);
	}

    private void Awake()
    {
        canvasManager = transform.parent.GetComponent<CanvasManagerScript>();
    }

    void DisplayGraph(float[] vals){
        float maxValue = vals.Max() * 1.05f;
        for (int i = 0; i < vals.Length; i++){
            Bar newBar = Instantiate(barPrefab) as Bar;
            newBar.transform.SetParent(transform);
            RectTransform rt = newBar.bar.GetComponent<RectTransform>();
            float normalizedValue = (float)vals[i] / maxValue;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, chartHeight * normalizedValue);
            newBar.bar.color = colors[i % colors.Length];

            if (labels.Length <= i) {
                newBar.label.text = "UNDEFINED";
            } else {
                newBar.label.text = labels[i];
            }
            newBar.barValue.text = vals[i].ToString();
            // if height is too small, move label to top of bar
            if (rt.sizeDelta.y < 30f){
                newBar.barValue.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                newBar.barValue.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        }
    }

    void ClearGraph()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Bar Holder(Clone)")
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
            inputValues = canvasManager.values;
            DisplayGraph(inputValues);
        }
    }
}
