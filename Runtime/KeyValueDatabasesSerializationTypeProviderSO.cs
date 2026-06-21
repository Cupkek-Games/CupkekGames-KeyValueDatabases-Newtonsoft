using System.Collections.Generic;
using CupkekGames.Newtonsoft;
using Newtonsoft.Json;
using UnityEngine;

namespace CupkekGames.KeyValueDatabases.Newtonsoft
{
    /// <summary>
    /// Supplies the <see cref="KeyValueDatabaseConverter"/> to the SerializationManager. Add this asset to
    /// a SerializationManagerRegistrar's providers list so <c>KeyValueDatabase&lt;,&gt;</c> round-trips
    /// through Newtonsoft. Without it, the core type still serializes on Unity's side but reflection
    /// serializers would emit its computed members.
    /// </summary>
    [CreateAssetMenu(menuName = "CupkekGames/Data/Newtonsoft/KeyValueDatabase Type Provider")]
    public class KeyValueDatabasesSerializationTypeProviderSO : SerializationTypeProviderSO
    {
        [SerializeField] private bool _keyValueDatabaseConverter = true;

        public override IList<JsonConverter> GetConverters()
        {
            var converters = new List<JsonConverter>();

            if (_keyValueDatabaseConverter)
                converters.Add(new KeyValueDatabaseConverter());

            return converters;
        }
    }
}
