using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPoolExample : MonoBehaviour
{
    [SerializeField] GameObjectPool cubePool;
    [SerializeField] GameObject cubePrefab;
    [SerializeField] Transform container;

    private void Start() {
        cubePool = new GameObjectPool(cubePrefab, container);
    }
}
