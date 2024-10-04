using System.Collections.Generic;
using UnityEngine;

namespace BallBlast
{
    /// <summary>
    /// The Pool class represents the pool for a particular prefab.
    /// </summary>
    public class Pool
    {
        private Transform _root;
        private Stack<Component> _inactive;
        private Component _prefab;

        public Pool(Component prefab, Transform root)
        {
            _prefab = prefab;
            if (prefab.gameObject.activeSelf)
                prefab.gameObject.SetActive(false);

            _inactive = new Stack<Component>();
            _root = root;
        }

        public Pool(Component prefab, Transform root, int count) : this(prefab, root)
        {
            for (var i = 0; i < count; i++)
                _inactive.Push(Spawn());
        }

        public void DestroyImmediate()
        {
            while (_inactive.Count > 0)
            {
                var item = _inactive.Pop();
                if (item != null)
                    GameObject.DestroyImmediate(item.gameObject);
            }
        }

        public void Release(Component obj)
        {
            obj.gameObject.SetActive(false);
            _inactive.Push(obj);
        }

        public T Take<T>() where T : Component
        {
            Component obj;

            if (_inactive.Count == 0)
            {
                obj = Spawn();
            }
            else
            {
                // Grab the last object in the inactive array
                obj = _inactive.Pop();

                if (obj == null)
                    return Take<T>();
            }

            return (T)obj;
        }

        private Component Spawn()
        {
            var obj = GameObject.Instantiate(_prefab, _root);
            var member = obj.gameObject.AddComponent<PoolMember>();
            member.MyPool = this;
            member.Obj = obj;

            return obj;
        }
    }
}