using System;
using System.Collections.Generic;

using JsonFx.Json;

namespace S3AAD
{
	[Serializable]
	public class RemoteDocument
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
	}
}
