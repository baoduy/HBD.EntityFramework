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
using System.ComponentModel.Composition;
#endif


namespace HBD.EntityFramework.TestSample
{
    internal class SampleBootStrapper : StandardBootstrapper
    {
        private static SampleBootStrapper _default;

        public static T GetExportOrDefault<T>() => Default.Container.TryGetExport<T>(out T v) ? v : default(T);

        internal static SampleBootStrapper Default
        {
            get
            {
                if (_default != null) return _default;

                _default = new SampleBootStrapper();
                _default.Run();

                _default.Container.GetExport<IDbRepositoryFactory>().EnsureDbCreated();
                return _default;
            }
        }

        protected override ILogger CreateLogger() => new Log4NetLogger();

        protected override ExtendedContainerConfiguration CreateContainerConfiguration()
        {
            return base.CreateContainerConfiguration()
                .WithAssembly(typeof(SampleDbContext).GetTypeInfo().Assembly)
                .WithAssembly(typeof(SampleBootStrapper).GetTypeInfo().Assembly)
                as ExtendedContainerConfiguration;
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