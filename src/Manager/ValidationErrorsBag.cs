using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KnowledgeShare.Manager
{
    public class ValidationErrorsBag : IReadOnlyDictionary<string, List<string>>
    {
        private readonly IDictionary<string, List<string>> _errorsBag =
            new Dictionary<string, List<string>>();

        public List<string> this[string key] => _errorsBag[key];

        public IEnumerable<string> Keys => _errorsBag.Keys;

        public IEnumerable<List<string>> Values => _errorsBag.Values;

        public int Count => _errorsBag.Count;

        public IReadOnlyDictionary<string, string> Flatten()
        {
            return _errorsBag
                .Where(bag => bag.Value.Count > 0)
                .ToDictionary(bag => bag.Key, bag => bag.Value.First());
        }

        public void AddInto(string key, string value)
        {
            if (!ContainsKey(key))
            {
                _errorsBag.Add(key, new List<string>());
            }

            _errorsBag[key].Add(value);
        }

        public bool ContainsKey(string key)
        {
            return _errorsBag.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, List<string>>> GetEnumerator()
        {
            return _errorsBag.GetEnumerator();
        }

        public bool TryGetValue(string key, out List<string> value)
        {
            return _errorsBag.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}