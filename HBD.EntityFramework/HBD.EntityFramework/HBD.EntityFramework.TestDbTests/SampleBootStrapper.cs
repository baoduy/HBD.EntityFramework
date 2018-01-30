using System.ComponentModel.Composition.Hosting;
using HBD.EntityFramework.Core;
using HBD.Mef;
using HBD.Mef.Logging;

namespace HBD.EntityFramework.TestDbTests
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

                bt.Container.GetExportedValue<IDbFactory>().EnsureDbCreated();
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
            //Mapper.Initialize(
            //    cfg =>
            //    {
            //        cfg.AddProfiles(typeof(SampleDbContext).GetTypeInfo().Assembly);
            //    });

            base.DoRun();
        }
    }
}