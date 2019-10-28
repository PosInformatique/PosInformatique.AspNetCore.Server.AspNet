//-----------------------------------------------------------------------
// <copyright file="AspNetServerOptionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace PosInformatique.AspNetCore.Server.AspNet.Tests.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using FluentAssertions;
    using Xunit;

    public class AspNetServerOptionsTest
    {
        [Fact]
        public void Constructor()
        {
            var options = new AspNetServerOptions();

            options.Routes.Should().BeEmpty();
        }
    }
}