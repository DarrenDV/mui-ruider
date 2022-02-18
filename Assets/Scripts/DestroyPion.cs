using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPion : MonoBehaviour
{

    [SerializeField] private float destructionTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyMySelf());
    }

    IEnumerator DestroyMySelf()
    {
        yield return new WaitForSeconds(destructionTime);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
