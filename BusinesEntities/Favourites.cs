using System;
using System.Collections.Generic;


namespace BusinesEntities
{
	public partial class Favourites
	{
		public int Id { get; set; }

		public int CustomerID { get; set; }
        public int EventID { get; set; }

    }
}
