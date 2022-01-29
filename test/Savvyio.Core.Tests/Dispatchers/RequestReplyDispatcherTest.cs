﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.Collections.Generic;
using Cuemon.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Assets.Commands;
using Savvyio.Assets.Queries;
using Savvyio.Commands;
using Savvyio.Extensions.Microsoft.DependencyInjection;
using Savvyio.Handlers;
using Savvyio.Queries;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Dispatchers
{
    public class RequestReplyDispatcherTest : Test
    {
        public RequestReplyDispatcherTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Dispatch_ShouldFailWithOrphanedHandlerException()
        {
            var sc = new ServiceCollection().AddSavvyIO(o => o.AssembliesToScan = typeof(RequestReplyDispatcherTest).Assembly.Yield());
            var sp = sc.BuildServiceProvider();
            var sut = new QueryDispatcher(sp.GetRequiredService<Func<Type, IEnumerable<object>>>());

            Assert.Throws<OrphanedHandlerException>(() => sut.Query(new FakeQuery()));
        }

        [Fact]
        public void Dispatch_ShouldDispatchCommandToDesignatedHandler()
        {
            var sc = new ServiceCollection()
                .AddSavvyIO(o => o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(RequestReplyDispatcherTest).Assembly, typeof(IQuery).Assembly))
                .AddScoped<ITestStore<IQuery>, InMemUnitTestStore<IQuery>>();
            
            var sp = sc.BuildServiceProvider();
            var sut = new QueryDispatcher(sp.GetRequiredService<Func<Type, IEnumerable<object>>>());
            var ga = new GetAccount(2313);
            var cs = sp.GetRequiredService<ITestStore<IQuery>>();
            
            var result = sut.Query(ga);

            Assert.NotEmpty(result);
            Assert.StartsWith(ga.Id.ToString(), result);
            Assert.Equal(ga.Id, cs.QueryFor<GetAccount>().Single().Id);
        }

        [Fact]
        public async Task DispatchAsync_ShouldFailWithOrphanedHandlerExceptionAsync()
        {
            var sc = new ServiceCollection().AddSavvyIO(o => o.AssembliesToScan = typeof(RequestReplyDispatcherTest).Assembly.Yield());
            var sp = sc.BuildServiceProvider();
            var sut = new QueryDispatcher(sp.GetRequiredService<Func<Type, IEnumerable<object>>>());

            await Assert.ThrowsAsync<OrphanedHandlerException>(async () => await sut.QueryAsync(new FakeQuery()));
        }

        [Fact]
        public async Task DispatchAsync_ShouldDispatchCommandToDesignatedHandlerAsync()
        {
            var sc = new ServiceCollection()
                .AddSavvyIO(o => o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(RequestReplyDispatcherTest).Assembly, typeof(IQuery).Assembly))
                .AddScoped<ITestStore<IQuery>, InMemUnitTestStore<IQuery>>();
            
            var sp = sc.BuildServiceProvider();
            var sut = new QueryDispatcher(sp.GetRequiredService<Func<Type, IEnumerable<object>>>());
            var ga = new GetAccount(74893297432);
            var cs = sp.GetRequiredService<ITestStore<IQuery>>();
            
            var result = await sut.QueryAsync(ga);

            Assert.NotEmpty(result);
            Assert.StartsWith(ga.Id.ToString(), result);
            Assert.Equal(ga.Id, cs.QueryFor<GetAccount>().Single().Id);
        }
    }
}