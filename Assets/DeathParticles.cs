using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private ParticleSystemRenderer renderer;
    
    public void SetColor(Color color)
    {
        var renderMaterial = renderer.material;
        renderMaterial.color = color;
        renderer.material = renderMaterial;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!particleSystem.isEmitting)
        {
            Destroy(gameObject);
        }
    }
}
