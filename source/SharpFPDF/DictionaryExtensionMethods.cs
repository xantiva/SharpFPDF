using System;
using System.Collections.Generic;

namespace SharpFPDF
{
    public static class DictionaryExtensionMethods
    {
        /// <summary>
        /// Adds the key value pair or updates the entry, if the key still exists.
        /// </summary>
        /// <param name="keyValuePairs">The <see cref="Dictionary{string, string}"/>, which should be modified.</param>
        /// <param name="key">The key <see cref="string "/>.</param>
        /// <param name="value">The value <see cref="string"/>.</param>
        public static void AddOrUpdate(this Dictionary<string, string> keyValuePairs, string key, string value)
        {
            if (keyValuePairs == null) throw new ArgumentNullException(nameof(keyValuePairs), "The dictionary must not be null");

            if (keyValuePairs.ContainsKey(key))
            {
                keyValuePairs[key] = value;
            }
            else
            {
                keyValuePairs.Add(key, value);
            }
        }
    }
}
