using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PXELDAR
{
    [Serializable]
    public class PoolObject
    {
        //===============================================================================================

        public string objectGroup;
        public string layerName;
        public GameObject[] objectToBePooled;
        public int count;
        public bool expandableList;

        //===============================================================================================

    }

    public class PoolingManager : Singleton<PoolingManager>
    {
        //===============================================================================================

        [SerializeField] private List<PoolObject> _objectsToPool;
        [SerializeField] private List<GameObject> _pooledObjects;

        //===============================================================================================

        private void Start()
        {
            _pooledObjects = new List<GameObject>();

            foreach (PoolObject poolObject in _objectsToPool)
            {
                for (int i = 0; i < poolObject.count; i++)
                {
                    if (poolObject.objectToBePooled != null)
                    {
                        int groupSize = poolObject.objectToBePooled.Length;

                        for (int j = 0; j < groupSize; j++)
                        {
                            int randomIndex = Random.Range(0, groupSize);
                            GameObject poolGameObject = Instantiate(poolObject.objectToBePooled[randomIndex]);

                            if (poolGameObject)
                            {
                                poolGameObject.transform.SetParent(LevelManager.Instance.poolHolder);
                                poolGameObject.SetActive(false);
                                _pooledObjects.Add(poolGameObject);
                            }
                        }
                    }
                }
            }
        }

        //===============================================================================================

        public GameObject GetPooledObject(string layer, string tag = "")
        {
            for (int poolObject = 0; poolObject < _pooledObjects.Count; poolObject++)
            {
                GameObject go = _pooledObjects[poolObject];

                if (!go.activeInHierarchy)
                {
                    if (LayerMask.LayerToName(go.layer) == layer)
                    {
                        _pooledObjects.Remove(go);
                        _pooledObjects.Add(go);
                        return go;
                    }
                }
            }

            return null;
        }

        //===============================================================================================

        public void ReleasePooledObject(GameObject pooledObject, bool scaleDown = false)
        {
            if (pooledObject)
            {
                pooledObject.transform.parent = LevelManager.Instance.poolHolder;

                pooledObject.SetActive(false);

                if (scaleDown)
                {
                    pooledObject.transform.localScale = Vector3.zero;
                }
            }
        }

        //===============================================================================================

        public void ReleaseAllPooledObjects(string layer = null)
        {
            int releasedObjectCount = 0;

            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                GameObject currentGameObject = _pooledObjects[i];

                if (currentGameObject == null)
                {
                    _pooledObjects.Remove(currentGameObject);
                }
                else
                {
                    if (currentGameObject.activeInHierarchy)
                    {
                        if (layer == null)
                        {
                            ReleasePooledObject(currentGameObject);
                        }
                        else
                        {
                            if (LayerMask.LayerToName(currentGameObject.layer) == layer)
                            {
                                releasedObjectCount++;

                                ReleasePooledObject(currentGameObject);
                            }
                        }
                    }
                }
            }
        }

        //===============================================================================================
    }
}
