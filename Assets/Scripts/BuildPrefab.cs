using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPrefab : MonoBehaviour
{
    private bool canBuild;
    private float buildMatAlpha;
    private string _canBuildProperty = "_CANBUILD";

    public enum PrefabState {};

    private List<Collider> colliders;

    public Material _buildableMat;

    private void Awake()
    {
        buildMatAlpha = _buildableMat.GetFloat("Alpha");
        colliders = new List<Collider>();
    }

    private void FixedUpdate()
    {
        // canBuild = 
    }

    private void OnCollisionEnter(Collision other)
    {
        colliders.Add(other.collider);
    }
}
