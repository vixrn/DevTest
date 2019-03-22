namespace DeveloperTest_ThomasParfitt.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Reservations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reservations()
        {
            Returns = new HashSet<Returns>();
        }

        public int Id { get; set; }

        public int BookingNr { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime CustDoB { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RentalDate { get; set; }

        public int VehID { get; set; }

        public int CarMilage { get; set; }

        public int? price { get; set; }

        public virtual Vehicle Vehicle { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Returns> Returns { get; set; }
    }
}
