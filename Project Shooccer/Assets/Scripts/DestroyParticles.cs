using System.Collections;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    [SerializeField]
    private Transform parent;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().duration);
        Destroy(parent.gameObject);
        Destroy(gameObject);
    }
}