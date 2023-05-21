using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraAttack : MonoBehaviour
{
    public static PandoraAttack instance { get; private set; }

    [SerializeField]
    public List<GameObject> prefabArrow;
    public List<GameObject> poolArrow;
    public Transform poolManager;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadPrefabArrow(transform.GetChild(0).gameObject);
        LoadArrowToPool(transform.GetChild(1).transform);
    }

    public void LoadPrefabArrow(GameObject arrows)
    {
        foreach (Transform prefabObj in arrows.transform)
        {
            prefabArrow.Add(prefabObj.gameObject);
        }
    }

    public void LoadArrowToPool(Transform attackPoint)
    {
        for (int i = 0; i < prefabArrow.Count; i++)
        {
            GameObject arrow = Instantiate(prefabArrow[i], attackPoint.position, attackPoint.rotation);
            arrow.transform.parent = poolManager;
            arrow.SetActive(false);
            poolArrow.Add(arrow);
        }
    }

    public void IceArrow(Transform attackPoint)
    {
        if (poolArrow[0] != null && !poolArrow[0].activeInHierarchy)
        {
            poolArrow[0].transform.position = attackPoint.position;
            poolArrow[0].SetActive(true);
        }
    }

    public void NormalArrow(Transform attackPoint)
    {
        if (poolArrow[1] != null && !poolArrow[1].activeInHierarchy)
        {
            poolArrow[1].transform.position = attackPoint.position;
            poolArrow[1].SetActive(true);
        }
    }

    public void LightningArrow(Transform attackPoint)
    {
        if (poolArrow[2] != null && !poolArrow[2].activeInHierarchy)
        {
            poolArrow[2].SetActive(true);
        }
    }
}
