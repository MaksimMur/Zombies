using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFilter : MonoBehaviour
{
    [SerializeField]
    private Shader _shader;

    protected Material _mat;

    private bool _useFilter = true;

    private void Awake()
    {
        _mat = new Material(_shader);
    }



    protected virtual void OnUpdate() { }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_useFilter)
            UseFilter(source, destination);
        else
            Graphics.Blit(source, destination);
    }

    protected virtual void UseFilter(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _mat);
    }
}