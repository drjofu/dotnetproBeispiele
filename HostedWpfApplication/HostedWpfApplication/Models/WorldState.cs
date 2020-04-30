using HostedWpfApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using WorldLib;

namespace HostedWpfApplication.Models
{
	public class WorldState : NotificationObject
	{
		private Continent selectedContinent;

		public Continent SelectedContinent
		{
			get { return selectedContinent; }
			set { selectedContinent = value; OnPropertyChanged(); }
		}

	}
}
