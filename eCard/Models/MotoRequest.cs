//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eCard.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MotoRequest
    {
        public System.Guid ID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.Guid> RequestedBy { get; set; }
        public string ClientCode { get; set; }
        public string Company { get; set; }
        public string PaxName { get; set; }
        public string RecordLocator { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Others { get; set; }
        public Nullable<decimal> BCDFee { get; set; }
        public Nullable<decimal> AdminFee { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string OptionTime { get; set; }
        public string ApprovalCode { get; set; }
        public string Remarks { get; set; }
        public string Invoice { get; set; }
        public string Status { get; set; }
    }
}
