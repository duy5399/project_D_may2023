using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", 1f);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
