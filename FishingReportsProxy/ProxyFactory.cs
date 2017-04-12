namespace FishingReportsProxy
{
	public static class ProxyFactory
	{
		#region ProxyFactory Members

		public static ILocationProxy CreateLocationProxy()
		{
			return new LocationProxy();
		}

		public static ILoginProxy CreateLoginProxy()
		{
			return new LoginProxy();
		}

		public static IReportProxy CreateReportProxy()
		{
			return new ReportProxy();
		}

		#endregion ProxyFactory Members

	}
}
