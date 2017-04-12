using System;
using System.Collections.Generic;

using FishingReports.Client.LocationService;
using FishingReports.Client.Model;

using AccessEntity = FishingReports.Client.LocationService.AccessEntity;
using LocationEntity = FishingReports.Client.LocationService.LocationEntity;
using StateEntity = FishingReports.Client.LocationService.StateEntity;

namespace FishingReports.Client.Repositories
{
	public class LocationRepository : ILocationRepository
	{
		#region Constructors

		internal LocationRepository()
		{
		}

		#endregion Constructors

		#region LocationRepository Members

		public Access[] GetAccesses(Guid locationId)
		{
			using (LocationDataServiceClient client = new LocationDataServiceClient())
			{
				return _TranslateToAccesses(client.GetAccesses(locationId));
			}
		}

		public Location[] GetLocations()
		{
			using (LocationDataServiceClient client = new LocationDataServiceClient())
			{
				return _TranslateToLocations(client.GetAllLocations());
			}
		}

		public Location[] GetLocations(Guid stateId)
		{
			using (LocationDataServiceClient client = new LocationDataServiceClient())
			{
				return _TranslateToLocations(client.GetLocations(stateId));
			}
		}

		public State[] GetStates()
		{
			using (LocationDataServiceClient client = new LocationDataServiceClient())
			{
				return _TranslateToStates(client.GetStates());
			}
		}

		public void SaveAccess(Guid locationId, string access)
		{
			using (LocationDataServiceClient client = new LocationDataServiceClient())
			{
				client.SaveAccess( locationId, access );
			}
		}

		public void SaveLocation(Guid stateId, string location)
		{
			using (LocationDataServiceClient client = new LocationDataServiceClient())
			{
				client.SaveLocation( stateId, location );
			}
		}

		public void SaveState(string state)
		{
			using (LocationDataServiceClient client = new LocationDataServiceClient())
			{
				client.SaveState( state );
			}
		}

		public ValidationResult ValidateNewAccess(Guid locationId, string access)
		{
			using (LocationDataServiceClient client = new LocationDataServiceClient())
			{
				return new ValidationResult(client.ValidateNewAccess(
					new AccessEntity
					{
						LocationID = locationId,
						Access = access
					}));
			}
		}

		public ValidationResult ValidateNewLocation(Guid stateId, string location)
		{
			using (LocationDataServiceClient client = new LocationDataServiceClient())
			{
				return new ValidationResult(client.ValidateNewLocation(new LocationEntity
				{
					Location = location,
					StateID = stateId
				}));
			}
		}

		public ValidationResult ValidateNewState(string state)
		{
			using (LocationDataServiceClient client = new LocationDataServiceClient())
			{
				return new ValidationResult(client.ValidateNewState(new StateEntity
				{
					State = state

				}));
			}
		}

		#endregion LocationRepository Members

		#region Private Members

		private Access[] _TranslateToAccesses(IEnumerable<AccessEntity> accessEntities)
		{
			List<Access> accesses = new List<Access>();

			foreach (var access in accessEntities)
			{
				accesses.Add(new Access
				{
					AccessId = access.AccessID,
					AccessName = access.Access
				});
			}

			return accesses.ToArray();
		}

		private Location[] _TranslateToLocations(IEnumerable<LocationEntity> locationEntities)
		{
			List<Location> locations = new List<Location>();

			foreach (var locationEntity in locationEntities)
			{
				locations.Add(new Location
				{
					LocationId = locationEntity.LocationID,
					LocationName = locationEntity.Location
				});
			}

			return locations.ToArray();
		}

		private State[] _TranslateToStates(IEnumerable<StateEntity> stateEntities)
		{
			List<State> states = new List<State>();

			foreach (var stateEntity in stateEntities)
			{
				states.Add(new State
				{
					StateId = stateEntity.StateID,
					StateName = stateEntity.State
				});
			}

			return states.ToArray();
		}

		#endregion Private Members

	}
}
