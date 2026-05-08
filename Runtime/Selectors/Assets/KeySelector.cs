using System;
using EgorLin.Keys.Ids;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EgorLin.Keys.Selectors.Assets
{
    [Serializable]
    [InlineProperty]
    public class KeySelector 
    {
        [HideInInspector] [SerializeField] private KeyId _keyId;

        public KeyId KeyId => _keyId;

        public void SetKey(KeyId id)
        {
            _keyId = id;
        }
    }
}