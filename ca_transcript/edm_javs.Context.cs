﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ca_transcript
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class db_javsEntities : DbContext
    {
        public db_javsEntities()
            : base("name=db_javsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<BlobType> BlobType { get; set; }
        public virtual DbSet<Email> Email { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<GroupsSecurityRight> GroupsSecurityRight { get; set; }
        public virtual DbSet<GUILayout> GUILayout { get; set; }
        public virtual DbSet<MasterGroup> MasterGroup { get; set; }
        public virtual DbSet<MasterGroupLoggedReason> MasterGroupLoggedReason { get; set; }
        public virtual DbSet<MasterType> MasterType { get; set; }
        public virtual DbSet<MasterTypesMasterGroup> MasterTypesMasterGroup { get; set; }
        public virtual DbSet<MasterTypesProceedingMenu> MasterTypesProceedingMenu { get; set; }
        public virtual DbSet<Pack> Pack { get; set; }
        public virtual DbSet<Party> Party { get; set; }
        public virtual DbSet<PartyBlob> PartyBlob { get; set; }
        public virtual DbSet<PartysType> PartysType { get; set; }
        public virtual DbSet<PartyType> PartyType { get; set; }
        public virtual DbSet<PartyTypeAllowed> PartyTypeAllowed { get; set; }
        public virtual DbSet<Phone> Phone { get; set; }
        public virtual DbSet<PreferenceType> PreferenceType { get; set; }
        public virtual DbSet<Proceeding> Proceeding { get; set; }
        public virtual DbSet<ProceedingMenu> ProceedingMenu { get; set; }
        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<SecurityRight> SecurityRight { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<User_GUILayout> User_GUILayout { get; set; }
        public virtual DbSet<UserPreference> UserPreference { get; set; }
    }
}
