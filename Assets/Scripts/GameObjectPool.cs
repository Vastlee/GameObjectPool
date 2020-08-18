using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class GameObjectPool {
    public GameObject Prefab { get; private set; }
    public Transform Container { get; private set; }
    public int Max { get; private set; } = int.MaxValue - 1;
    public int Min { get; private set; }
    public int Buffer { get; set; }
    public List<GameObject> Pool { get; } = new List<GameObject>();

    private bool isBeingMaintained = false;

    public GameObjectPool(GameObject prefab, Transform container = null, int max = 100, int min = 1, int buffer = 10) {
        Prefab = prefab;
        if(container == null) {
            container = new GameObject($"{prefab.name} Pool").transform;
        }
        Container = container;
        Max = max;
        Min = min;
        Buffer = buffer;

        _ = FillAsync();
    }

    public GameObject Get() {
        foreach(var go in Pool) {
            if(!go.activeSelf) {
                return go;
            }
        }

        return Add().Result;
    }

    public async Task<GameObject> Add() {
        if(Pool.Count < Max) {
            var go = MonoBehaviour.Instantiate(Prefab, Container);
            go.SetActive(false);
            Pool.Add(go);
            return go;
        }

        Debug.LogError($"Unable to Add to {Prefab.name} pool. Max [{Max}] exceeded");
        return null;
    }

    private async Task FillAsync() {
        int freeCount = 0;

        foreach(var go in Pool) {
            if(!go.activeSelf) { freeCount++; }
        }

        int delta = 0;
        if(freeCount < (Min + Buffer)) {
            delta = (Min + Buffer) - freeCount;
            for(int i = 0; i < delta; i++) {
                await Add();
            }
        }
    }
}
