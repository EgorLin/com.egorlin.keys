using System;
using EgorLin.Keys.Ids;
using EgorLin.Keys.Owners;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace EgorLin.Keys.Selectors.Assets
{
    [Serializable]
    [InlineProperty]
    public class KeySelector 
    {
        [HideInInspector] [SerializeField] private KeyId _keyId;
        
        [SerializeField] public bool isSpecificCollection;
        [SerializeField] public UnityEngine.Object specificCollection;

        public KeyId KeyId => _keyId;

        public void SetKey(KeyId id)
        {
            _keyId = id;
        }
    }
}