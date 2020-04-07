//-----------------------------------------------------------------------
// <copyright file="GitHubProjectConstants.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.AspNetCore.Server.AspNet
{
    /// <summary>
    /// Contains static constant which describe the project on GitHub.
    /// </summary>
    internal static class GitHubProjectConstants
    {
        /// <summary>
        /// Project name on GitHub.
        /// </summary>
        public const string ProjectName = "PosInformatique/PosInformatique.AspNetCore.Server.AspNet";

        /// <summary>
        /// Project URL on GitHub.
        /// </summary>
#pragma warning disable S1075 // URIs should not be hardcoded
        public const string ProjectWebSiteUrl = "https://github.com/PosInformatique/PosInformatique.AspNetCore.Server.AspNet";
#pragma warning restore S1075 // URIs should not be hardcoded
    }
}
