namespace DeveloperTest_ThomasParfitt.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Returns
    {
        public int Id { get; set; }

        public int BookingNumber { get; set; }

        public DateTime ReturnDate { get; set; }

        public int ReturnCarMilage { get; set; }

        public virtual Reservations Reservations { get; set; }
    }
}
