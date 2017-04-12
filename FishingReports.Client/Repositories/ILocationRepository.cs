using System;

using FishingReports.Client.Model;
using FishingReports.Model;

namespace FishingReports.Client.Repositories
{
	public interface ILocationRepository
	{
		#region ILocationRepository Members

		Access[] GetAccesses(Guid locationId);

		Location[] GetLocations();

		Location[] GetLocations(Guid stateId);

		State[] GetStates();

		void SaveAccess(Guid locationId, string access);

		void SaveLocation(Guid stateId, string location);

		void SaveState(string state);

		ValidationResult ValidateNewAccess(Guid locationId, string access);

		ValidationResult ValidateNewLocation(Guid stateId, string location);

		ValidationResult ValidateNewState(string state);

		#endregion ILocationRepository Members

	}
}
