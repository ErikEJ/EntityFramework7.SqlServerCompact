﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Specification.Tests.TestModels;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Specification.Tests
{
    public class MonsterFixupSqlCeTest : MonsterFixupTestBase
    {
        private static readonly HashSet<string> _createdDatabases = new HashSet<string>();

        private static readonly ConcurrentDictionary<string, object> _creationLocks
            = new ConcurrentDictionary<string, object>();

        protected override IServiceProvider CreateServiceProvider(bool throwingStateManager = false)
        {
            var serviceCollection = new ServiceCollection()
                .AddEntityFrameworkSqlCe();

            if (throwingStateManager)
            {
                serviceCollection.AddScoped<IStateManager, ThrowingMonsterStateManager>();
            }

            return serviceCollection.BuildServiceProvider();
        }

        protected override DbContextOptions CreateOptions(string databaseName)
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder
                .UseSqlCe(CreateConnectionString(databaseName));

            return optionsBuilder.Options;
        }

        private static string CreateConnectionString(string name)
        {
            return new SqlCeConnectionStringBuilder
            {
                DataSource = name + ".sdf"
            }.ConnectionString;
        }

        protected override void CreateAndSeedDatabase(string databaseName, Func<MonsterContext> createContext, Action<MonsterContext> seed)
        {
            var creationLock = _creationLocks.GetOrAdd(databaseName, n => new object());
            lock (creationLock)
            {
                if (!_createdDatabases.Contains(databaseName))
                {
                    using (var context = createContext())
                    {
                        context.Database.EnsureClean();
                        seed(context);
                        //context.SeedUsingFKs();
                    }

                    _createdDatabases.Add(databaseName);
                    TestSqlLoggerFactory.Reset();
                }
            }
        }

        public override void OnModelCreating<TMessage, TProductPhoto, TProductReview>(ModelBuilder builder)
        {
            base.OnModelCreating<TMessage, TProductPhoto, TProductReview>(builder);

            builder.Entity<TMessage>().HasKey(e => e.MessageId);
            builder.Entity<TProductPhoto>().HasKey(e => e.PhotoId);
            builder.Entity<TProductReview>().HasKey(e => e.ReviewId);
        }
    }
}
