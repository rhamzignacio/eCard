﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class eCardEntities : DbContext
    {
        public eCardEntities()
            : base("name=eCardEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<EmailSender> EmailSender { get; set; }
        public virtual DbSet<MotoRequest> MotoRequest { get; set; }
        public virtual DbSet<ServiceFeeFormula> ServiceFeeFormula { get; set; }
        public virtual DbSet<UserAccount> UserAccount { get; set; }
        public virtual DbSet<v_MotoRequest> v_MotoRequest { get; set; }
        public virtual DbSet<vw_Approved> vw_Approved { get; set; }
        public virtual DbSet<vw_Canceled> vw_Canceled { get; set; }
        public virtual DbSet<vw_FORAPPROVAL> vw_FORAPPROVAL { get; set; }
        public virtual DbSet<vw_Void> vw_Void { get; set; }
        public virtual DbSet<vw_Declined> vw_Declined { get; set; }
    }
}
