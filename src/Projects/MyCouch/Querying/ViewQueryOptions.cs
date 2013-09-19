﻿using System;
using System.Collections.Generic;
using System.Linq;
using MyCouch.Extensions;

namespace MyCouch.Querying
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class ViewQueryOptions
    {
        /// <summary>
        /// Allow the results from a stale view to be used.
        /// </summary>
        public string Stale { get; set; }
        /// <summary>
        /// Include the full content of the documents in the return.
        /// </summary>
        public bool? IncludeDocs { get; set; }
        /// <summary>
        /// Return the documents in descending by key order.
        /// </summary>
        public bool? Descending { get; set; }
        /// <summary>
        /// Return only documents that match the specified key.
        /// </summary>
        public object Key { get; set; }
        /// <summary>
        /// Returns only documents that matches any of the specified keys.
        /// </summary>
        public object[] Keys { get; set; }
        /// <summary>
        /// Indicates if any <see cref="Keys"/> has been specified.
        /// </summary>
        public bool HasKeys
        {
            get { return Keys != null && Keys.Any(); }
        }
        /// <summary>
        /// Return records starting with the specified key.
        /// </summary>
        public object StartKey { get; set; }
        /// <summary>
        /// Return records starting with the specified document ID.
        /// </summary>
        public string StartKeyDocId { get; set; }
        /// <summary>
        /// Stop returning records when the specified key is reached.
        /// </summary>
        public object EndKey { get; set; }
        /// <summary>
        /// Stop returning records when the specified document ID is reached.
        /// </summary>
        public string EndKeyDocId { get; set; }
        /// <summary>
        /// Specifies whether the specified end key should be included in the result.
        /// </summary>
        public bool? InclusiveEnd { get; set; }
        /// <summary>
        /// Skip this number of records before starting to return the results.
        /// </summary>
        public int? Skip { get; set; }
        /// <summary>
        /// Limit the number of the returned documents to the specified number.
        /// </summary>
        public int? Limit { get; set; }
        /// <summary>
        /// Use the reduction function.
        /// </summary>
        public bool? Reduce { get; set; }
        /// <summary>
        /// Include the update sequence in the generated results.
        /// </summary>
        public bool? UpdateSeq { get; set; }
        /// <summary>
        /// The group option controls whether the reduce function reduces to a set of distinct keys or to a single result row.
        /// </summary>
        public bool? Group { get; set; }
        /// <summary>
        /// Specify the group level to be used.
        /// </summary>
        public int? GroupLevel { get; set; }

        /// <summary>
        /// Returns Keys as compatible JSON document for use e.g.
        /// with POST of keys against views.
        /// </summary>
        /// <returns></returns>
        public virtual string GetKeysAsJsonObject()
        {
            if (!HasKeys)
                return "{}";

            return string.Format("{{\"keys\":[{0}]}}",
                string.Join(",", Keys.Select(k => FormatValue(k))));
        }

        /// <summary>
        /// Generats configured values as querystring params.
        /// </summary>
        /// <remarks>Keys are not included in this string.</remarks>
        /// <returns></returns>
        public virtual string GenerateQueryStringParams()
        {
            return string.Join("&", ToJsonKeyValues()
                .Where(kv => kv.Key != KeyValues.Keys)
                .Select(kv => string.Format("{0}={1}", kv.Key, Uri.EscapeDataString(kv.Value))));
        }

        public virtual IDictionary<string, string> ToJsonKeyValues()
        {
            var kvs = new Dictionary<string, string>();

            if (IncludeDocs.HasValue)
                kvs.Add(KeyValues.IncludeDocs, IncludeDocs.Value.ToString().ToLower());

            if (Descending.HasValue)
                kvs.Add(KeyValues.Descending, Descending.Value.ToString().ToLower());

            if(Reduce.HasValue)
                kvs.Add(KeyValues.Reduce, Reduce.Value.ToString().ToLower());

            if (InclusiveEnd.HasValue)
                kvs.Add(KeyValues.InclusiveEnd, InclusiveEnd.Value.ToString().ToLower());

            if (UpdateSeq.HasValue)
                kvs.Add(KeyValues.UpdateSeq, UpdateSeq.Value.ToString().ToLower());

            if (Group.HasValue)
                kvs.Add(KeyValues.Group, Group.Value.ToString().ToLower());

            if (GroupLevel.HasValue)
                kvs.Add(KeyValues.GroupLevel, GroupLevel.Value.ToString(MyCouchRuntime.NumberFormat));

            if (HasValue(Stale))
                kvs.Add(KeyValues.Stale, FormatValue(Stale));

            if (HasValue(Key))
                kvs.Add(KeyValues.Key, FormatValue(Key));

            if (HasValue(Keys))
                kvs.Add(KeyValues.Keys, FormatValue(Keys));

            if(HasValue(StartKey))
                kvs.Add(KeyValues.StartKey, FormatValue(StartKey));

            if (HasValue(StartKeyDocId))
                kvs.Add(KeyValues.StartKeyDocId, FormatValue(StartKeyDocId));
            
            if(HasValue(EndKey))
                kvs.Add(KeyValues.EndKey, FormatValue(EndKey));

            if (HasValue(EndKeyDocId))
                kvs.Add(KeyValues.EndKeyDocId, FormatValue(EndKeyDocId));
            
            if(Limit.HasValue)
                kvs.Add(KeyValues.Limit, Limit.Value.ToString(MyCouchRuntime.NumberFormat));
            
            if(Skip.HasValue)
                kvs.Add(KeyValues.Skip, Skip.Value.ToString(MyCouchRuntime.NumberFormat));

            return kvs;
        }

        protected virtual bool HasValue(object value)
        {
            return value != null;
        }

        protected virtual bool HasValue(string value)
        {
            return value != null;
        }

        protected virtual bool HasValue(IEnumerable<string> value)
        {
            return value != null && value.Any();
        }

        protected virtual string FormatValue(object value)
        {
            if (value is string)
                return FormatValue(value as string);

            var conv = value as IConvertible;
            if (conv == null)
                return value.ToString();

            if (value.IsNumeric())
                return conv.ToString(MyCouchRuntime.NumberFormat);

            if (value.IsDateTime())
                return FormatValue(conv.To<DateTime>().ToString(MyCouchRuntime.DateTimeFormatPattern));

            if (value.IsBool())
                return conv.ToString(MyCouchRuntime.GenericFormat).ToLower();

            return value.ToString();
        }

        protected virtual string FormatValue(string value)
        {
            return string.Format("\"{0}\"", value);
        }

        protected virtual string FormatValue(object[] value)
        {
            return string.Format("[{0}]", string.Join(",", value.Select(v => FormatValue(v))));
        }

        public static class KeyValues
        {
            public const string IncludeDocs = "include_docs";
            public const string Descending = "descending";
            public const string Reduce = "reduce";
            public const string InclusiveEnd = "inclusive_end";
            public const string UpdateSeq = "update_seq";
            public const string Group = "group";
            public const string GroupLevel = "group_level";
            public const string Stale = "stale";
            public const string Key = "key";
            public const string Keys = "keys";
            public const string StartKey = "startkey";
            public const string StartKeyDocId = "startkey_docid";
            public const string EndKey = "endkey";
            public const string EndKeyDocId = "endkey_docid";
            public const string Limit = "limit";
            public const string Skip = "skip";
        }
    }
}