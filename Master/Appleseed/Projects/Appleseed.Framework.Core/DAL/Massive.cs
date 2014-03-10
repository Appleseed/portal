// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Massive.cs" company="--">
//   Copyright © -- 2011. All Rights Reserved.
// </copyright>
// <summary>
//   The object extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appleseed.Framework.Massive
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.Common;
    using System.Dynamic;
    using System.Linq;

    /// <summary>
    /// The object extensions.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public static class ObjectExtensions
    {
        #region Public Methods

        /// <summary>
        /// Extension for adding single parameter
        /// </summary>
        /// <param name="cmd">
        /// The CMD.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void AddParam(this DbCommand cmd, object item)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = string.Format("@{0}", cmd.Parameters.Count);
            if (item == null)
            {
                p.Value = DBNull.Value;
            }
            else
            {
                if (item.GetType() == typeof(Guid))
                {
                    p.Value = item.ToString();
                    p.DbType = DbType.String;
                    p.Size = 4000;
                }
                else if (item.GetType() == typeof(ExpandoObject))
                {
                    var d = (IDictionary<string, object>)item;
                    p.Value = d.Values.FirstOrDefault();
                }
                else
                {
                    p.Value = item;
                }

                // from DataChomp
                if (item.GetType() == typeof(string))
                {
                    p.Size = 4000;
                }
            }

            cmd.Parameters.Add(p);
        }

        /// <summary>
        /// Extension method for adding in a bunch of parameters
        /// </summary>
        /// <param name="cmd">
        /// The CMD.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <remarks>
        /// </remarks>
        public static void AddParams(this DbCommand cmd, object[] args)
        {
            foreach (var item in args)
            {
                AddParam(cmd, item);
            }
        }

        /// <summary>
        /// Turns the object into a Dictionary
        /// </summary>
        /// <param name="thingy">
        /// The thingy.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static IDictionary<string, object> ToDictionary(this object thingy)
        {
            return (IDictionary<string, object>)thingy.ToExpando();
        }

        /// <summary>
        /// Turns the object into an ExpandoObject
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        /// <returns>
        /// The to expando.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static dynamic ToExpando(this object o)
        {
            var result = new ExpandoObject();
            var d = result as IDictionary<string, object>; // work with the Expando as a Dictionary
            if (o.GetType() == typeof(ExpandoObject))
            {
                return o; // shouldn't have to... but just in case
            }

            if (o.GetType() == typeof(NameValueCollection))
            {
                var nv = (NameValueCollection)o;
                nv.Cast<string>().Select(key => new KeyValuePair<string, object>(key, nv[key])).ToList().ForEach(d.Add);
            }
            else
            {
                var props = o.GetType().GetProperties();
                foreach (var item in props)
                {
                    d.Add(item.Name, item.GetValue(o, null));
                }
            }

            return result;
        }

        /// <summary>
        /// Turns an IDataReader to a Dynamic list of things
        /// </summary>
        /// <param name="rdr">
        /// The RDR.
        /// </param>
        /// <returns>
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static List<dynamic> ToExpandoList(this IDataReader rdr)
        {
            var result = new List<dynamic>();
            while (rdr.Read())
            {
                dynamic e = new ExpandoObject();
                var d = e as IDictionary<string, object>;
                for (var i = 0; i < rdr.FieldCount; i++)
                {
                    d.Add(rdr.GetName(i), rdr[i]);
                }

                result.Add(e);
            }

            return result;
        }

        #endregion
    }
}