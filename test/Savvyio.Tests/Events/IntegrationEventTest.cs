﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuemon.Extensions.Reflection;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Domain.Events;
using Savvyio.Assets.Events;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Events
{
    public class IntegrationEventTest : Test
    {
        public IntegrationEventTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void IntegrationEvent_AccountCreated_ShouldHaveMetadata_WithSpecifiedEventId()
        {
            var eventId = Guid.NewGuid().ToString("N");
            var id = 100;
            var fullname = "Michael Mortensen";
            var email = "root@gimlichael.dev";

            var sut = new AccountCreated(100, fullname, email).SetEventId(eventId);

            Assert.Equal(email, sut.EmailAddress);
            Assert.Equal(fullname, sut.FullName);
            Assert.Equal(id, sut.Id);

            Assert.Collection(sut.Metadata, 
                kvp =>
                {
                    Assert.Equal(kvp.Key, MetadataDictionary.EventId);
                    Assert.Equal(kvp.Value, eventId);
                },
                kvp =>
                {
                    Assert.Equal(kvp.Key, MetadataDictionary.Timestamp);
                    Assert.Equal(kvp.Value, sut.GetTimestamp());
                },
                kvp =>
                {
                    Assert.Equal(kvp.Key, MetadataDictionary.MemberType);
                    Assert.Equal(kvp.Value, typeof(AccountCreated).ToFullNameIncludingAssemblyName());
                });
            Assert.True(sut.Metadata.Count == 3, "sut.Metadata.Count == 3");
        }
    }
}
