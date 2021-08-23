//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ANTS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Package
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Package()
        {
            this.Ratings = new HashSet<Rating>();
        }
    
        public int packageid { get; set; }
        public int userid { get; set; }
        [Required]
        public string packagename { get; set; }
        [Required]
        public double price { get; set; }
        [Required]
        public string details { get; set; }
        [Required]
        public string category { get; set; }
        [Required]
        public double discount { get; set; }
        public System.DateTime createdat { get; set; }
        [Required]
        public string advertisement { get; set; }
        [Required]
        public string location { get; set; }
        [Required]
        public string approvestatus { get; set; }
    
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}