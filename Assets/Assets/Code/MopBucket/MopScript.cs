using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MopScript : MonoBehaviour
{
    public bool isClean = true;

    public Material cleanMaterial;
    public Material dirtMaterial;

    private Renderer mopRenderer;

    private void Start()
    {
        mopRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (isClean)
        {
            mopRenderer.material = cleanMaterial;
        }
        else
        {
            mopRenderer.material = dirtMaterial;
        }
    }

}