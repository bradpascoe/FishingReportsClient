using System;

using FishingReports.Client.Model;

namespace FishingReportsProxy
{
	public interface ILocationProxy
	{
		#region ILocationProxy Members

		void AddAccess(Guid locationId, string newAccess);

		void AddLocation(Guid stateId, string newLocation);

		void AddState(string newState);

		Access[] GetAccesses(Guid parse);

		string[] GetLocations();

		Location[] GetLocations(Guid stateId);

		State[] GetStates();

		#endregion ILocationProxy Members

	}
}
