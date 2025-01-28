using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forceps : MonoBehaviour
{
    [SerializeField]
    private GameObject betadine_stain;

    private Renderer swab_renderer;
    public Material betadine_swab;

    private Material[] mats;

    // Start is called before the first frame update
    void Awake()
    {
        swab_renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        // We check the tag of whatever triggered
        if (collider.gameObject.CompareTag("Betadine"))
        {
            // If the tag was 'Betadine', then the betadine stain will appear
            betadine_stain.SetActive(true);
        }
        else if (collider.gameObject.CompareTag("Betadine_bottle"))
        {
            print("Betadine triggered");
            // If the tag was 'Betadine_bottle', then the swab gets stained with the betadine
            mats = swab_renderer.materials;
            mats[0] = betadine_swab;
            swab_renderer.materials = mats;
        }
    }
}
