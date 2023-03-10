using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

//
//  Copyright © 2022 Kyo Matias & Nate Florendo. All rights reserved.
//  

public class ObjectPool : MonoBehaviour
{

    #region Instance

    private static ObjectPool _instance;
    public static  ObjectPool Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion
    
    private GameObject _objectToPool;
    private bool _notEnoughObjectsInPool = true;

    public List<GameObject> _objectsPool;
    public List<GameObject> _objectsPoolUI;
    public Transform spawnedObjectsParent;

    private void Start()
    {
        _objectsPool = new List<GameObject>();
        _objectsPoolUI = new List<GameObject>();
    }

    public GameObject GetObject(GameObject objectToPool, Vector3 pos)
    {
        _objectToPool = objectToPool;

        if (_objectsPool.Count > 0)
        {
            for (int i = 0; i < _objectsPool.Count; i++)
            {
                //MUST SET PROPER TAGS PER PREFAB IN UNITY EDITOR
                if (!_objectsPool[i].activeInHierarchy && _objectsPool[i].CompareTag(_objectToPool.tag)) 
                {
                    _objectsPool[i].transform.position = pos;
                    return _objectsPool[i];
                }
            }
        }

        
        if (_notEnoughObjectsInPool)
        {
            CreateObjectParentIfNeeded();
            
            GameObject obj = Instantiate(_objectToPool, pos, Quaternion.identity);
            obj.name = transform.root.name + "_" + _objectToPool.name + "_" + _objectsPool.Count;
            obj.transform.SetParent(spawnedObjectsParent);
            obj.SetActive(false);
            _objectsPool.Add(obj);
            return obj;
        }

        return null;
    }
    
    public GameObject GetObjectUI(GameObject objectToPool, Transform pos)
    {
        _objectToPool = objectToPool;

        if (_objectsPoolUI.Count > 0)
        {
            for (int i = 0; i < _objectsPoolUI.Count; i++)
            {
                //MUST SET PROPER TAGS PER PREFAB IN UNITY EDITOR
                if (!_objectsPoolUI[i].activeInHierarchy && _objectsPoolUI[i].CompareTag(_objectToPool.tag)) 
                {
                    _objectsPoolUI[i].transform.position = transform.position;
                    return _objectsPoolUI[i];
                }
            }
        }

        
        if (_notEnoughObjectsInPool)
        {
            CreateObjectParentIfNeeded();
            
            GameObject obj = Instantiate(_objectToPool, pos);
            _objectsPoolUI.Add(obj);
            return obj;
        }

        return null;
    }
    

    private void CreateObjectParentIfNeeded()
    {
        //creates object to parent pooled objects to avoid messy scene...
        
        if (spawnedObjectsParent == null)
        {
            string name = "ObjectPoolHolder";
            var parentObject = GameObject.Find(name);
            if (parentObject != null)
                spawnedObjectsParent = parentObject.transform;
            else
            {
                spawnedObjectsParent = new GameObject(name).transform;
            }

        }
    }

    public void Dispose(List<GameObject> pool)
    {
        foreach (var t in pool)
        {
            Destroy(t);
        }
    }

}