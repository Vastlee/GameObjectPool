using System.Collections;
using UnityEngine;

public class SpawnerExample : MonoBehaviour
{
    [SerializeField] private GameObjectPool cubePool;
    [SerializeField] private GameObjectPool spherePool;
    [SerializeField] private GameObjectPool capsulePool;

    private void Start() {
        StartCoroutine(Demo());
    }

    private IEnumerator Demo() {
        for(int i = 0; i < 10; i++) {
            MoveRandom(cubePool.Get());
            yield return new WaitForEndOfFrame();
            MoveRandom(spherePool.Get());
            yield return new WaitForEndOfFrame();
            MoveRandom(capsulePool.Get());
            yield return new WaitForEndOfFrame();
        }
    }

    private void MoveRandom(GameObject go) {
        Vector3 randDir = Random.insideUnitCircle.normalized;
        go.transform.Translate(randDir * 5f);
    }
}
