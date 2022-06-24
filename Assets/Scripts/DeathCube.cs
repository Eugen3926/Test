using System.Collections;
using UnityEngine;

public class DeathCube : MonoBehaviour
{
    [SerializeField] Transform playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(KillCube());
    }  

    IEnumerator KillCube()
    {
        yield return new WaitForSeconds(2f);
        Instantiate(playerPrefab);
        Destroy(this.gameObject);
    }
}
