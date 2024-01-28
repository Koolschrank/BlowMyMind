using System.Collections;
using Player;
using UnityEngine;

public class AppearanceChange : MonoBehaviour
{
    [SerializeField] private float changeTime = 2;
    [SerializeField] private SkinnedMeshRenderer bodyMesh;
    [SerializeField] private FaceData[] faces;
    
    private WaitForSeconds _changeDelay;
    
    void Start()
    {
        _changeDelay = new WaitForSeconds(changeTime);
        StartCoroutine(ContinousChange());
    }

    private IEnumerator ContinousChange()
    {
        while (true)
        {
            var faceData = faces[Random.Range(0, faces.Length)];
            var materials = bodyMesh.materials;
            materials[0] = faceData.skinMaterial;
            materials[1] = faceData.faceMaterial;
            materials[4] = faceData.hairMaterial;
            bodyMesh.materials = materials;
            yield return _changeDelay;
        }
    }
}
