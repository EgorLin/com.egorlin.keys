using System;
using EgorLin.Keys.Ids;
using UnityEngine;

namespace EgorLin.Keys.Selectors.Assets
{
    [Serializable]
    public class KeySelector 
    {
        [HideInInspector] [SerializeField] private KeyId id;
        
        [SerializeField] public bool isSpecificCollection;
        [SerializeField] public UnityEngine.Object specificCollection;

        public KeyId ID => id;

        public void SetKey(KeyId value)
        {
            id = value;
        }
    }
}