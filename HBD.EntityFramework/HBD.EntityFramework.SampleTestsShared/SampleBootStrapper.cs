using AutoMapper;
using HBD.EntityFramework.DbContexts.Interfaces;
using HBD.EntityFramework.Sample.DbContexts;
using HBD.Mef;
using HBD.Mef.Logging;
using System.Reflection;

#if NETSTANDARD2_0 || NETSTANDARD1_6 || NETCOREAPP1_1 || NETCOREAPP2_0
using System.Composition.Hosting;
using HBD.Mef.Hosting;
#else
using System.ComponentModel.Composition.Hosting;
#endif


namespace HBD.EntityFramework.TestSample
{
    internal class SampleBootStrapper : StandardBootstrapper
    {
        private static SampleBootStrapper _default;

        public static T GetExport<T>()
        {
#if NETSTANDARD2_0 || NETSTANDARD1_6 || NETCOREAPP1_1 || NETCOREAPP2_0
            return SampleBootStrapper.Default.Container.GetExport<T>();
#else
            return SampleBootStrapper.Default.Container.GetExportedValue<T>();
#endif
        }

        internal static SampleBootStrapper Default => HBD.Framework.SingletonManager.GetOrLoad(ref _default, () =>
        {
            var bt = new SampleBootStrapper();
            bt.Run();

#if NETSTANDARD2_0 || NETSTANDARD1_6 || NETCOREAPP1_1 || NETCOREAPP2_0
            bt.Container.GetExport<IDbRepoFactory>().EnsureDbCreated();
#else
            bt.Container.GetExportedValue<IDbRepoFactory>().EnsureDbCreated();
#endif
            return bt;
        });

        protected override ILogger CreateLogger() => new Log4NetLogger();

#if NETSTANDARD2_0 || NETSTANDARD1_6 || NETCOREAPP1_1 || NETCOREAPP2_0
        protected override ExtendedContainerConfiguration CreateContainerConfiguration()
        {
            return base.CreateContainerConfiguration()
                .WithAssembly(typeof(SampleDbContext).GetTypeInfo().Assembly)
                .WithAssembly(typeof(SampleBootStrapper).GetTypeInfo().Assembly)
                as ExtendedContainerConfiguration;
        }
#else
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            AggregateCatalog.Catalogs.Add(new ApplicationCatalog());
        }
#endif

        protected override void DoRun()
        {
            Mapper.Initialize(
                cfg =>
                {
                    cfg.AddProfiles(typeof(SampleDbContext).GetTypeInfo().Assembly);
                });

            base.DoRun();
        }
    }
}