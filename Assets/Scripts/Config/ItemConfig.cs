using System;
using UnityEngine;

namespace Config
{
    [Serializable]
    public class ItemConfig
    {
        [SerializeField] protected int _id;
        [SerializeField] protected string _name;
        [SerializeField] protected Sprite _avatar;
        [SerializeField] protected GameObject _prefab;
        [SerializeField] protected int _unlockLevel;
        [SerializeField] protected int _price;
        
        public int id => _id;
        public string name => _name;
        public Sprite avatar => _avatar;
        public GameObject prefab => _prefab;
        public int unlockLevel => _unlockLevel;
        public int price => _price;
    }
}