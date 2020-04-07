// -----------------------------------------------------------------------
// <copyright file="AspNetServerOptions.cs" company="P.O.S Informatique">
//  Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace PosInformatique.AspNetCore.Server.AspNet
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Options of the ASP .NET non-core hosting.
    /// </summary>
    public sealed class AspNetServerOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AspNetServerOptions"/> class.
        /// </summary>
        public AspNetServerOptions()
        {
            this.Routes = new Collection<string>();
        }

        /// <summary>
        /// Gets the base routes request which will be redirected to the ASP .NET Core hosted application.
        /// </summary>
        public Collection<string> Routes
        {
            get;
        }
    }
}
