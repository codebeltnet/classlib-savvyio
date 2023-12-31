﻿using System;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.EventDriven;
using Savvyio.EventDriven.Messaging;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Newtonsoft.Json.EventDriven.Messaging
{
    public class IntegrationEventExtensionsTest : Test
    {
        public IntegrationEventExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }
        
        [Fact]
        public void EncloseToSignedMessage_ShouldSerialize_WithoutSignature()
        {
            var utcNow = DateTime.UtcNow;
            var sut1 = new MemberCreated("Jane Doe", "jd@office.com").SetEventId("69bccf3b1117425397c5ed9ed757bb0f").SetTimestamp(utcNow);
            var sut2 = sut1.ToMessage("https://fancy.api/members".ToUri(), o =>
            {
                o.MessageId = "2d4030d32a254ee8a27046e5bafe696a";
                o.Time = utcNow;
            });
            var json = NewtonsoftJsonFormatter.SerializeObject(sut2, o => o.Settings.DateFormatString = "O");
            var jsonString = json.ToEncodedString();
            TestOutput.WriteLine(jsonString);

            Assert.Equal($$"""
                           {
                             "id": "2d4030d32a254ee8a27046e5bafe696a",
                             "source": "https://fancy.api/members",
                             "type": "Savvyio.Assets.EventDriven.MemberCreated, Savvyio.Assets.Tests",
                             "time": "{{utcNow:O}}",
                             "data": {
                               "name": "Jane Doe",
                               "emailAddress": "jd@office.com",
                               "metadata": {
                                 "eventId": "69bccf3b1117425397c5ed9ed757bb0f",
                                 "timestamp": "{{utcNow:O}}"
                               }
                             }
                           }
                           """.ReplaceLineEndings(), jsonString);
        }
    }
}
