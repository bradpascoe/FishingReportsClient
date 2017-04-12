using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FishingReports.Client.Repositories
{
    public static class RepositoryFactory
    {
        public static ILoginRepository CreateLoginRepository()
        {
            return new LoginRepository();
        }

        public static ILocationRepository CreateLocationRepository()
        {
            return new LocationRepository();
        }

        public static IReportRepository CreateReportRepository()
        {
            return new ReportRepository();
        }

        public static IImageRepository CreateImageRepository()
        {
            return new ImageRepository();
        }
    }
}
