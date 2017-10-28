using AutoMapper;
using HBD.EntityFramework.Sample.DbContexts;
using HBD.Mef;
using HBD.Mef.Logging;
using System.Reflection;
using HBD.EntityFramework.Core;


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

        public static T GetExportOrDefault<T>() => SampleBootStrapper.Default.Container.GetExportedValueOrDefault<T>();

        internal static SampleBootStrapper Default
        {
            get
            {
                if (_default != null) return _default;
                var bt = new SampleBootStrapper();
                bt.Run();

                bt.Container.GetExportedValue<IDbRepositoryFactory>().EnsureDbCreated();
                _default = bt;

                return _default;
            }
        }

        protected override ILogger CreateLogger() => new Log4NetLogger();

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            AggregateCatalog.Catalogs.Add(new ApplicationCatalog());
        }

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