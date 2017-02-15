using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using JsonFx.Json;

namespace S3AAD
{
    [Serializable]
    public class RemoteDocument : IDictionary<string, object>
    {
        public string bucketName { get; private set; }
        public string key { get; private set; }
        public Dictionary<string, object> data { get; private set; }
        
        public bool publicRead;

        public object this[string key]
        {
            get
            {
                return data[key];
            }
            set
            {
                data[key] = value;
            }
        }

        internal RemoteDocument(string bucketName, string key)
        {
            this.bucketName = bucketName;
            this.key = key;
            this.data = new Dictionary<string, object>();
        }
        internal RemoteDocument(string json)
        {
            var reader = new JsonReader();
            var _this = reader.Read<Dictionary<string, object>>(json);

            bucketName = ((string)_this["bucketName"]);
            key = ((string)_this["key"]);
            data = new Dictionary<string, object>((System.Dynamic.ExpandoObject)_this["data"]);
        }

        public void Update()
        {
            S3DB.SaveDocument(bucketName, key, this, publicRead);
        }

        public string AsJson()
        {
            var writer = new JsonWriter();
            return writer.Write(new
            {
                bucketName = bucketName,
                key = key,
                data = data
            });
        }

        #region IDictionary<string, obect>
        public ICollection<string> Keys => data.Keys;
        public ICollection<object> Values => data.Values;
        public int Count => data.Count;
        public bool IsReadOnly => false;

        public bool ContainsKey(string key) => data.ContainsKey(key);
        public void Add(string key, object value) => data.Add(key, value);
        bool Remove(string key) => data.Remove(key);
        bool IDictionary<string, object>.Remove(string key) => data.Remove(key);
        public bool TryGetValue(string key, out object value) => data.TryGetValue(key, out value);
        public void Add(KeyValuePair<string, object> item) => data.Add(item.Key, item.Value);
        public void Clear() => data.Clear();
        public bool Contains(KeyValuePair<string, object> item) => data.Any(x => x.Key == item.Key && x.Value == item.Value);
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => data.GetEnumerator();

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        public bool Remove(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException("use `Remove(string key)` instaed");
        }
        #endregion 
    }
}
