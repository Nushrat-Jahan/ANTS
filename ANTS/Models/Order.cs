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

    public partial class Order
    {

        public int orderid { get; set; }
        [Required]
        public int sellerid { get; set; }
        [Required]
        public int customerid { get; set; }
        [Required]
        public string customerphone { get; set; }
        [Required]
        public string customeraddress { get; set; }
        [Required]
        public int packageid { get; set; }
        [Required]
        public string ordername { get; set; }
        [Required]
        public string paytype { get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        public double totalprice { get; set; }
        [Required]
        public System.DateTime createdat { get; set; }
        [Required]
        public string status { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}