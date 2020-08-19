using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour {
    private const int DEFAULT_MAX = 100;
    private const int DEFAULT_BUFFER = 10;

    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform container;
    [SerializeField] private int max = DEFAULT_MAX;
    [SerializeField] private int buffer = DEFAULT_BUFFER;
    [SerializeField] private bool enableOnGet = true;

    private List<GameObject> pool = new List<GameObject>();
    private bool isMaintaining = false;

    #region Properties
    public GameObject Prefab {
        get { return this.prefab; }
        set { this.prefab = value; }
    }

    public Transform Container {
        get { return this.container; }
        set { this.container = value; }
    }

    public int Max {
        get { return this.max; }
        set { this.max = value; }
    }

    public int Buffer {
        get { return this.buffer; }
        set { 
            this.buffer = Math.Max(0, value);
        }
    }

    public bool EnableOnGet {
        get { return this.enableOnGet; }
        set { this.enableOnGet = value; }
    }
    #endregion

    #region MonoBehaviour
    private void Start() {
        MaintainPool();
    }

    private void OnDisable() {
        StopAllCoroutines();
    }
    #endregion

    public GameObject Get() {
        GameObject result = null;

        foreach(var go in pool) {
            if(!go.activeSelf) {
                result = go;
                break;
            }
        }

        if(result == null) {
            result = Add();
        }

        if(enableOnGet) { result.SetActive(true); }

        MaintainPool();
        return result;
    }

    private GameObject Add() {
        if(pool.Count > Max) {
            Debug.LogError($"Warning. {prefab.name} Pool Size ({pool.Count}) is exceeded it's max limit of {Max.ToString()}");
        }

        GameObject go = Instantiate(prefab, container, true);
        go.SetActive(false);
        pool.Add(go);
        return go;
    }

    private void MaintainPool() {
        if(!isMaintaining) {
            StartCoroutine(Fill());
        }
    }

    private IEnumerator Fill() {
        isMaintaining = true;
        int freeCount = 0;

        foreach(var go in pool) {
            if(!go.activeSelf) { freeCount++; }
        }

        int delta = 0;
        if(freeCount < buffer) {
            delta = buffer - freeCount;
            for(int i = 0; i < delta; i++) {   
                Add();
                yield return null;
            }
        }

        isMaintaining = false;
    }
}
