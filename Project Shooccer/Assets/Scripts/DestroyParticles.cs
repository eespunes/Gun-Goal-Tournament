using System.Collections;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().duration);
        Destroy(transform.parent.gameObject);
    }
}