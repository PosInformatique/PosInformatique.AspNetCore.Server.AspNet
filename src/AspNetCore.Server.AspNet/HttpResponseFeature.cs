//-----------------------------------------------------------------------
// <copyright file="HttpResponseFeature.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.AspNetCore.Server.AspNet
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;

    /// <summary>
    /// Implementation of the <see cref="IHttpResponseFeature"/> used by ASP .NET Core requests and
    /// which wraps the response to the <see cref="System.Web.HttpResponse"/> for ASP .NET non-core infrastructure.
    /// </summary>
    internal sealed class HttpResponseFeature : IHttpResponseFeature
    {
        /// <summary>
        /// Callbacks methods to be invoked invoked just before the response starts.
        /// </summary>
        private readonly IList<CallbackMethod> startingCallbacks;

        /// <summary>
        /// Callbacks methods to be invoked after a response has fully completed.
        /// </summary>
        private readonly IList<CallbackMethod> completedCallbacks;

        /// <summary>
        /// ASP .NET non-core <see cref="System.Web.HttpContext"/> to wrap.
        /// </summary>
        private readonly System.Web.HttpResponseBase response;

        /// <summary>
        /// The headers of the response.
        /// </summary>
        private readonly HttpResponseHeaderDictionary headers;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpResponseFeature"/> class.
        /// </summary>
        /// <param name="response">ASP .NET non-core <see cref="System.Web.HttpContext"/> to wrap.</param>
        public HttpResponseFeature(System.Web.HttpResponseBase response)
        {
            this.response = response;

            this.startingCallbacks = new List<CallbackMethod>();
            this.completedCallbacks = new List<CallbackMethod>();

            this.headers = new HttpResponseHeaderDictionary(response);
        }

        /// <inheritdoc />
        public int StatusCode
        {
            get => this.response.StatusCode;
            set => this.response.StatusCode = value;
        }

        /// <inheritdoc />
        public string ReasonPhrase
        {
            get => this.response.StatusDescription;
            set => this.response.StatusDescription = value;
        }

        /// <inheritdoc />
        public bool HasStarted
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public IHeaderDictionary Headers
        {
            get => this.headers;
            set => throw new NotSupportedException(string.Format(
                CultureInfo.CurrentUICulture,
                AspNetServerResources.ChangeResponseHeadersNotSupported,
                GitHubProjectConstants.ProjectName,
                GitHubProjectConstants.ProjectWebSiteUrl));
        }

        /// <inheritdoc />
        public Stream Body
        {
            get => this.response.OutputStream;
            set => throw new NotSupportedException(string.Format(
                CultureInfo.CurrentUICulture,
                AspNetServerResources.ChangeResponseBodyStreamNotSupported,
                GitHubProjectConstants.ProjectName,
                GitHubProjectConstants.ProjectWebSiteUrl));
        }

        /// <inheritdoc />
        public void OnStarting(Func<object, Task> callback, object state)
        {
            this.startingCallbacks.Add(new CallbackMethod(callback, state));
        }

        /// <inheritdoc />
        public void OnCompleted(Func<object, Task> callback, object state)
        {
            this.completedCallbacks.Add(new CallbackMethod(callback, state));
        }

        /// <summary>
        /// Executes all the call back method registered when the <see cref="OnStarting(Func{object, Task}, object)"/> has been called.
        /// </summary>
        /// <returns>An <see cref="Task"/> which represents the asynchronous operation.</returns>
        public async Task StartAsync()
        {
            this.HasStarted = true;

            foreach (var callback in this.startingCallbacks)
            {
                await callback.InvokeAsync();
            }
        }

        /// <summary>
        /// Executes all the call back method registered when the <see cref="OnCompleted(Func{object, Task}, object)"/> has been called.
        /// </summary>
        /// <returns>An <see cref="Task"/> which represents the asynchronous operation.</returns>
        public async Task CompleteAsync()
        {
            foreach (var callback in this.completedCallbacks)
            {
                await callback.InvokeAsync();
            }
        }

        /// <summary>
        /// Represents a register callback method when the <see cref="OnStarting(Func{object, Task}, object)"/>
        /// or <see cref="OnCompleted(Func{object, Task}, object)"/> is called.
        /// </summary>
        private class CallbackMethod
        {
            /// <summary>
            /// Callback to invoke.
            /// </summary>
            private readonly Func<object, Task> callback;

            /// <summary>
            /// State argument of the <see cref="callback"/> to invoke.
            /// </summary>
            private readonly object state;

            /// <summary>
            /// Initializes a new instance of the <see cref="CallbackMethod"/> class.
            /// </summary>
            /// <param name="callback">Callback to invoke.</param>
            /// <param name="state">State argument of the <see cref="callback"/> to invoke.</param>
            public CallbackMethod(Func<object, Task> callback, object state)
            {
                this.callback = callback;
                this.state = state;
            }

            /// <summary>
            /// Invokes the callback method.
            /// </summary>
            /// <returns>An <see cref="Task"/> which represents the asynchronous operation.</returns>
            public Task InvokeAsync()
            {
                return this.callback(this.state);
            }
        }
    }
}
