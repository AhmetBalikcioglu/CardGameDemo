using System.Collections.Generic;
using UnityEngine;

namespace Lib.Pool
{
    public abstract class ObjectPoolBehaviour : MonoBehaviour
    {
        #region Childs

        [SerializeField]
        private ObjectPoolData _data;
        
        #endregion

        #region Field

        protected List<GameObject> _pooledObjects;
        public List<GameObject> PooledObjects => _pooledObjects;

        #endregion

        #region Unity: Awake

        private void Awake()
        {
            CreatePool();
        }
        
        #endregion

        
        #region Pool: Create

        private void CreatePool()
        {
            _pooledObjects = new List<GameObject>();

            for(int i = 0; i < _data.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(_data.objectToPool, transform);
                obj.SetActive(false);
                _pooledObjects.Add(obj);
            }

            OnCreatePool();
        }

        protected abstract void OnCreatePool();

        #endregion

        #region Pool: GetObject

        public GameObject GetPooledObject()
        {
            for(int i = 0; i < _pooledObjects.Count; i++)
            {
                if(!_pooledObjects[i].activeInHierarchy)
                {
                    OnGetPooledObject(_pooledObjects[i]);
                    return _pooledObjects[i];
                }
            }

            if(_data.shouldExpand)
            {
                GameObject obj = (GameObject)Instantiate(_data.objectToPool);
                obj.SetActive(false);
                _pooledObjects.Add(obj);
                OnGetPooledObject(obj);
                return obj;
            }
       
            return null;
        }
        
        protected abstract void OnGetPooledObject(GameObject gameObject);

        #endregion
    }
}