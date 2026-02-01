using System.Collections.Generic;
using UnityEngine;

namespace Core.Scripts
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private readonly T objectType;
        private readonly Transform parentContainer;
        private readonly Queue<T> pool;
        
        private readonly HashSet<T> activeObjects;
        
        public ObjectPool(T argObjectType, int argInitialSize = 10, Transform argParent = null)
        {
            objectType = argObjectType;
            parentContainer = argParent;
            pool = new Queue<T>(argInitialSize);
            activeObjects = new HashSet<T>();

            // Warm up the pool
            for (int i = 0; i < argInitialSize; i++)
            {
                T obj = CreateNewInstance();
                obj.gameObject.SetActive(false);
                pool.Enqueue(obj);
            }
        }

        private T CreateNewInstance()
        {
            T obj = Object.Instantiate(objectType, parentContainer);
            return obj;
        }

        public T Get()
        {
            T item;

            if (pool.Count > 0)
            {
                item = pool.Dequeue();
            }
            else
            {
                item = CreateNewInstance();
            }

            ActivateObject(item);

            activeObjects.Add(item);

            return item;
        }

        private void ActivateObject(T argObject)
        {
            argObject.gameObject.SetActive(true);
            argObject.OnSpawn();
        }
        
        public void ReturnToPool(T argObject)
        {
            DeactivateObject(argObject);
            
            if (activeObjects.Contains(argObject) == false)
            {
                Debug.LogWarning($"Trying to return {argObject.name} to pool, but it wasn't tracked as active. It might have already been returned.");
                return;
            }

            activeObjects.Remove(argObject);
            pool.Enqueue(argObject);
        }

        private void DeactivateObject(T argObject)
        {
            argObject.OnDespawn();
            argObject.gameObject.SetActive(false);
            argObject.gameObject.transform.SetParent(parentContainer);
        }

        public void Clear()
        {
            pool.Clear();
            activeObjects.Clear();
        }
    }
}