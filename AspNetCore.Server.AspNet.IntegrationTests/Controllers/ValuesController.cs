//-----------------------------------------------------------------------
// <copyright file="ValuesController.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.AspNetCore.Server.AspNet.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Text;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    /// <summary>
    /// This is a simple ASP .NET Core Web API which is called from the ASP .NET non-core infrastructure.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : Controller
    {
        /// <summary>
        /// Gets an array which contains "value1" and "value2".
        /// </summary>
        /// <returns>An array which contains "value1" and "value2"</returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Gets the value of the specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id">ID to return into a string.</param>
        /// <returns>The <paramref name="id"/> specified in arguments.</returns>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return $"The ID = {id}";
        }

        // GET api/values/json
        [HttpGet("json")]
        public ActionResult GetJson(int q1, string q2)
        {
            return this.Json(new
            {
                a = 1234,
                b = "abcd",
                q1,
                q2
            });
        }

        // GET api/actionWith/{param}/eter
        [HttpGet("actionWith/{param}/eter")]
        public ActionResult<string> GetActionWithParameter(string param)
        {
            return $"The Parameter = {param}";
        }

        // GET api/error404
        [HttpGet("error404")]
        public ActionResult GetError()
        {
            return this.NotFound("Text of the 404");
        }

        // GET api/exception
        [HttpGet("exception")]
        public ActionResult GetException()
        {
            throw new DivideByZeroException("This is to test an exception...");
        }

        // GET api/postData
        [HttpPost("postData")]
        public ActionResult PostData([FromBody]PostData data)
        {
            this.Response.Headers.Add("MyHeader", "The value of MyHeader");

            return this.Json(new
            {
                data.StringValue
            });
        }

        // GET api/request_comparison
        [HttpGet("request_comparison")]
        public ActionResult RequestComparison()
        {
            var requestNonCore = System.Web.HttpContext.Current.Request;

            var result = new StringBuilder();
            result.AppendLine(BuildComparisonLine(string.Empty, "ASP .NET Core", "ASP .NET Non-Core"));
            result.AppendLine();
            result.Append(BuildComparisonLine("Cookies", requestNonCore.Cookies, this.Request.Cookies));
            result.AppendLine(BuildComparisonLine("ContentLength", requestNonCore.ContentLength, this.Request.ContentLength));
            result.AppendLine(BuildComparisonLine("ContentType", requestNonCore.ContentType, this.Request.ContentType));
            result.Append(BuildComparisonLine("Headers", requestNonCore.Headers, this.Request.Headers));
            result.AppendLine(BuildComparisonLine("Host", requestNonCore.ServerVariables["HTTP_HOST"], this.Request.Host));
            result.AppendLine(BuildComparisonLine("Method", requestNonCore.HttpMethod, this.Request.Method));
            result.AppendLine(BuildComparisonLine("QueryString", requestNonCore.QueryString, this.Request.QueryString));
            result.AppendLine(BuildComparisonLine("Scheme", requestNonCore.Url.Scheme, this.Request.Scheme));
            result.AppendLine(BuildComparisonLine("Path", requestNonCore.Path, this.Request.Path));
            result.AppendLine(BuildComparisonLine("PathBase", requestNonCore.ApplicationPath, this.Request.PathBase));

            return this.Content(result.ToString());
        }

        private static string BuildComparisonLine(string name, NameValueCollection a, IHeaderDictionary b)
        {
            var result = new StringBuilder();
            result.AppendLine(BuildComparisonLine(name, a.Count, b.Count));

            foreach (var key in a.AllKeys)
            {
                if (b.ContainsKey(key))
                {
                    result.AppendLine(BuildComparisonLine(" " + key, a[key], b[key]));
                }
                else
                {
                    result.AppendLine(BuildComparisonLine(" " + key, a[key], "null"));
                }
            }

            foreach (var keyValue in b)
            {
                var valueFound = a.Get(keyValue.Key);
                if (valueFound == null)
                {
                    result.AppendLine(BuildComparisonLine(" " + keyValue.Key, "null", keyValue.Value));
                }
            }

            return result.ToString();
        }

        private static string BuildComparisonLine(string name, System.Web.HttpCookieCollection a, IRequestCookieCollection b)
        {
            var result = new StringBuilder();
            result.AppendLine(BuildComparisonLine(name, a.Count, b.Count));

            foreach (var key in a.AllKeys)
            {
                if (b.ContainsKey(key))
                {
                    result.AppendLine(BuildComparisonLine(" " + key, a[key], b[key]));
                }
                else
                {
                    result.AppendLine(BuildComparisonLine(" " + key, a[key], "null"));
                }
            }

            foreach (var keyValue in b)
            {
                var valueFound = a.Get(keyValue.Key);
                if (valueFound == null)
                {
                    result.AppendLine(BuildComparisonLine(" " + keyValue.Key, "null", keyValue.Value));
                }
            }

            return result.ToString();
        }

        private static string BuildComparisonLine(string name, object a, object b)
        {
            return name.PadRight(30) + "| " + ToString(a).PadRight(80) + " | " + ToString(b).PadRight(80);
        }

        private static string ToString(object obj)
        {
            if (obj == null)
            {
                return "null";
            }

            return obj.ToString();
        }
    }

    public class PostData
    {
        [JsonProperty]
        public string StringValue
        {
            get;
            set;
        }
    }
}
