//-----------------------------------------------------------------------
// <copyright file="HttpResponseHeaderDictionary.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.AspNetCore.Server.AspNet
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using Microsoft.Net.Http.Headers;

    /// <summary>
    /// <see cref="IHeaderDictionary"/> implementation which read and sets (wrap) the headers
    /// directly on the <see cref="System.Web.HttpResponse.Headers"/>.
    /// </summary>
    internal sealed class HttpResponseHeaderDictionary : IHeaderDictionary
    {
        /// <summary>
        /// <see cref="System.Web.HttpResponse"/> to wrap.
        /// </summary>
        private readonly System.Web.HttpResponseBase response;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponseHeaderDictionary"/> class.
        /// </summary>
        /// <param name="response"><see cref="System.Web.HttpResponse"/> to wrap.</param>
        public HttpResponseHeaderDictionary(System.Web.HttpResponseBase response)
        {
            this.response = response;
        }

        /// <inheritdoc />
        public long? ContentLength
        {
            get
            {
                var value = this.response.Headers.Get(HeaderNames.ContentLength);

                if (value == null)
                {
                    return null;
                }

                return Convert.ToInt64(value, CultureInfo.InvariantCulture);
            }

            set
            {
                if (value == null)
                {
                    this.response.Headers.Remove(HeaderNames.ContentLength);
                }
                else
                {
                    this.response.Headers[HeaderNames.ContentLength] = Convert.ToString(value, CultureInfo.InvariantCulture);
                }
            }
        }

        /// <inheritdoc />
        public ICollection<string> Keys
        {
            get
            {
                return this.response.Headers.AllKeys;
            }
        }

        /// <inheritdoc />
        public ICollection<StringValues> Values
        {
            get
            {
#pragma warning disable S2365 // Properties should not make collection or array copies
                return this.response.Headers.AllKeys.Select(key => new StringValues(this.response.Headers[key])).ToArray();
#pragma warning restore S2365 // Properties should not make collection or array copies
            }
        }

        /// <inheritdoc />
        public int Count
        {
            get
            {
                return this.response.Headers.Count;
            }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc />
        [SuppressMessage("Critical Vulnerability", "S3330:\"HttpOnly\" should be set on cookies", Justification = "Checked")]
        public StringValues this[string key]
        {
            get
            {
                if (key == HeaderNames.Location)
                {
                    return this.response.RedirectLocation;
                }
                else if (key == HeaderNames.SetCookie)
                {
                    var stringCookies = new string[this.response.Cookies.Count];

                    for (int i = 0; i < stringCookies.Length; i++)
                    {
                        var aspNetCookie = this.response.Cookies[i];

                        var cookie = new SetCookieHeaderValue(aspNetCookie.Name)
                        {
                            Domain = aspNetCookie.Domain,
                            Expires = aspNetCookie.Expires == DateTime.MinValue ? DateTimeOffset.MinValue : aspNetCookie.Expires,
                            HttpOnly = aspNetCookie.HttpOnly,
                            Path = aspNetCookie.Path,
                            SameSite = (Microsoft.Net.Http.Headers.SameSiteMode)aspNetCookie.SameSite,
                            Secure = aspNetCookie.Secure,
                            Value = aspNetCookie.Value,
                        };

                        stringCookies[i] = cookie.ToString();
                    }

                    return new StringValues(stringCookies);
                }
                else
                {
                    return this.response.Headers[key];
                }
            }

            set
            {
                if (key == HeaderNames.Location)
                {
                    this.response.RedirectLocation = value;
                }
                else if (key == HeaderNames.SetCookie)
                {
                    foreach (var stringCookie in value)
                    {
                        var aspNetCoreCookie = SetCookieHeaderValue.Parse(stringCookie);
                        var aspNetCookie = new HttpCookie(aspNetCoreCookie.Name.ToString())
                        {
                            Domain = aspNetCoreCookie.Domain.ToString(),
                            Expires = aspNetCoreCookie.Expires.HasValue ? aspNetCoreCookie.Expires.Value.DateTime : DateTime.MinValue,
                            HttpOnly = aspNetCoreCookie.HttpOnly,
                            Path = aspNetCoreCookie.Path.ToString(),
                            SameSite = (System.Web.SameSiteMode)aspNetCoreCookie.SameSite,
                            Secure = aspNetCoreCookie.Secure,
                            Value = aspNetCoreCookie.Value.ToString(),
                        };

                        this.response.Cookies.Remove(aspNetCookie.Name);

                        this.response.Cookies.Add(aspNetCookie);
                    }
                }
                else
                {
                    this.response.Headers[key] = value;

                    if (key == HeaderNames.ContentType)
                    {
                        this.response.ContentType = value;
                    }
                }
            }
        }

        /// <inheritdoc />
        public void Add(string key, StringValues value)
        {
            this.response.Headers.Add(key, value);
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<string, StringValues> item)
        {
            this.response.Headers.Add(item.Key, item.Value);
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.response.Headers.Clear();
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<string, StringValues> item)
        {
            if (!this.TryGetValue(item.Key, out var valueFound))
            {
                return false;
            }

            if (valueFound != item.Value)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public bool ContainsKey(string key)
        {
            return this.response.Headers.AllKeys.Contains(key);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<string, StringValues>[] array, int arrayIndex)
        {
            for (int i = 0, j = arrayIndex; i < this.response.Headers.Count; i++, j++)
            {
                array[j] = new KeyValuePair<string, StringValues>(this.response.Headers.AllKeys[i], this.response.Headers[i]);
            }
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator()
        {
            return this.response.Headers.AllKeys.Select(key => new KeyValuePair<string, StringValues>(key, this.response.Headers[key])).GetEnumerator();
        }

        /// <inheritdoc />
        public bool Remove(string key)
        {
            bool exists = this.response.Headers.AllKeys.Contains(key);

            this.response.Headers.Remove(key);

            return exists;
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<string, StringValues> item)
        {
            if (!this.TryGetValue(item.Key, out var valueFound))
            {
                return false;
            }

            if (valueFound != item.Value)
            {
                return false;
            }

            this.response.Headers.Remove(item.Key);

            return true;
        }

        /// <inheritdoc />
        public bool TryGetValue(string key, out StringValues value)
        {
            var valueFound = this.response.Headers.Get(key);

            if (valueFound == null)
            {
                return false;
            }

            value = valueFound;

            return true;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}