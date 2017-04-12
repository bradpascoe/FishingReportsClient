using System;
using System.Linq;

using FishingReports.Client.Model;
using FishingReports.Client.Repositories;

namespace FishingReportsProxy
{
	internal class LocationProxy : ILocationProxy
	{
		#region LocationProxy Members

		public void AddAccess(Guid locationId, string newAccess)
		{
			mLocationRepository.SaveAccess(locationId, newAccess);
		}

		public void AddLocation(Guid stateId, string newLocation)
		{
			mLocationRepository.SaveLocation(stateId, newLocation);
		}

		public void AddState(string newState)
		{
			mLocationRepository.SaveState(newState);
		}

		public Access[] GetAccesses(Guid locationId)
		{
			var accessEntities = mLocationRepository.GetAccesses(locationId);

			return accessEntities;
		}

		public string[] GetLocations()
		{
			return mLocationRepository.GetLocations().Select(loc => loc.LocationName).ToArray();
		}

		public Location[] GetLocations(Guid stateId)
		{
			var locationEntities = mLocationRepository.GetLocations(stateId);

			return locationEntities;
		}

		public State[] GetStates()
		{
			var stateEntities = mLocationRepository.GetStates();

			return stateEntities;
		}

		#endregion LocationProxy Members

		#region Fields

		private readonly ILocationRepository mLocationRepository = RepositoryFactory.CreateLocationRepository();

		#endregion Fields

	}
}
