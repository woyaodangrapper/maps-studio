using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using GrandSql.PGsql.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace GrandSql.PGsql.Data
{
    public partial class taoistContext : DbContext
    {
        public taoistContext()
        {
        }

        public taoistContext(DbContextOptions<taoistContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Architecture> Architecture { get; set; }
        public virtual DbSet<ArchitectureRecord> ArchitectureRecord { get; set; }
        public virtual DbSet<Basemap> Basemap { get; set; }
        public virtual DbSet<Commandplan> Commandplan { get; set; }
        public virtual DbSet<CommandplanRecord> CommandplanRecord { get; set; }
        public virtual DbSet<CommonLocations> CommonLocations { get; set; }
        public virtual DbSet<CommonLocationsRecord> CommonLocationsRecord { get; set; }
        public virtual DbSet<Config> Config { get; set; }
        public virtual DbSet<Equipmentdata> Equipmentdata { get; set; }
        public virtual DbSet<EquipmentdataRecord> EquipmentdataRecord { get; set; }
        public virtual DbSet<EventProcess> EventProcess { get; set; }
        public virtual DbSet<EventReporting> EventReporting { get; set; }
        public virtual DbSet<EventsAlarm> EventsAlarm { get; set; }
        public virtual DbSet<EventsData> EventsData { get; set; }
        public virtual DbSet<Facecomputing> Facecomputing { get; set; }
        public virtual DbSet<Facedata> Facedata { get; set; }
        public virtual DbSet<Keyareas> Keyareas { get; set; }
        public virtual DbSet<Marker> Marker { get; set; }
        public virtual DbSet<MarkerRecord> MarkerRecord { get; set; }
        public virtual DbSet<PersonnelPositioning> PersonnelPositioning { get; set; }
        public virtual DbSet<PersonnelPositioningdata> PersonnelPositioningdata { get; set; }
        public virtual DbSet<Roaming> Roaming { get; set; }
        public virtual DbSet<RoamingRecord> RoamingRecord { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<RolesPermissions> RolesPermissions { get; set; }
        public virtual DbSet<RouteEquipment> RouteEquipment { get; set; }
        public virtual DbSet<RouteUser> RouteUser { get; set; }
        public virtual DbSet<Routedata> Routedata { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersRoles> UsersRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql(@"Server=192.168.2.15;Port=5432;User Id=postgres;
Password=123qwe=-;Database=taoist;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Architecture>(entity =>
            {
                entity.ToTable("architecture");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.ModelUrl)
                    .HasColumnName("model_url")
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.Pid)
                    .HasColumnName("pid")
                    .HasMaxLength(36);

                entity.Property(e => e.RolseId)
                    .HasColumnName("rolse_id")
                    .HasMaxLength(36);

                entity.Property(e => e.Selftest).HasColumnName("selftest");
            });

            modelBuilder.Entity<ArchitectureRecord>(entity =>
            {
                entity.ToTable("architecture_record");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.ArchitectureId)
                    .HasColumnName("architecture_id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Data).HasColumnName("data");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(255);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(36);
            });

            modelBuilder.Entity<Basemap>(entity =>
            {
                entity.ToTable("basemap");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Icon).HasColumnName("icon");

                entity.Property(e => e.OnOff).HasColumnName("on_off");

                entity.Property(e => e.Sontitle)
                    .HasColumnName("sontitle")
                    .HasMaxLength(255);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(255);

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Commandplan>(entity =>
            {
                entity.ToTable("commandplan");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Data).HasColumnName("data");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.Timer).HasColumnName("timer");
            });

            modelBuilder.Entity<CommandplanRecord>(entity =>
            {
                entity.ToTable("commandplan_record");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Dataurl)
                    .HasColumnName("dataurl")
                    .HasMaxLength(255);

                entity.Property(e => e.EventProcessId)
                    .HasColumnName("event_process_id")
                    .HasMaxLength(36);

                entity.Property(e => e.OnOff).HasColumnName("on_off");
            });

            modelBuilder.Entity<CommonLocations>(entity =>
            {
                entity.ToTable("common_locations");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Point).HasColumnName("point");

                entity.Property(e => e.Sontitle)
                    .HasColumnName("sontitle")
                    .HasMaxLength(255);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<CommonLocationsRecord>(entity =>
            {
                entity.ToTable("common_locations_record");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Data)
                    .HasColumnName("data")
                    .HasMaxLength(255);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(255);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(36);
            });

            modelBuilder.Entity<Config>(entity =>
            {
                entity.ToTable("config");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Embedded).HasColumnName("embedded");

                entity.Property(e => e.IconBase64).HasColumnName("icon_base64");

                entity.Property(e => e.InitLanLong).HasColumnName("init_lan_long");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.TitleList)
                    .HasColumnName("title_list")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Equipmentdata>(entity =>
            {
                entity.HasKey(e => e.Equipmentcode)
                    .HasName("equipmentdata_pkey");

                entity.ToTable("equipmentdata");

                entity.Property(e => e.Equipmentcode)
                    .HasColumnName("equipmentcode")
                    .HasMaxLength(36);

                entity.Property(e => e.ArchitectureId)
                    .HasColumnName("architecture_id")
                    .HasMaxLength(36);

                entity.Property(e => e.Config).HasColumnName("config");

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Model)
                    .HasColumnName("model")
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.Size).HasColumnName("size");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<EquipmentdataRecord>(entity =>
            {
                entity.ToTable("equipmentdata_record");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Configlist).HasColumnName("configlist");

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Equipmentcode)
                    .HasColumnName("equipmentcode")
                    .HasMaxLength(36);

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(255);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(36);
            });

            modelBuilder.Entity<EventProcess>(entity =>
            {
                entity.ToTable("event_process");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.ArchitectureId)
                    .HasColumnName("architecture_id")
                    .HasMaxLength(36);

                entity.Property(e => e.Attachdata).HasColumnName("attachdata");

                entity.Property(e => e.CommandPlanId)
                    .HasColumnName("command_plan_id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.EventId)
                    .HasColumnName("event_id")
                    .HasMaxLength(36);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(36);
            });

            modelBuilder.Entity<EventReporting>(entity =>
            {
                entity.ToTable("event_reporting");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Attachdata).HasColumnName("attachdata");

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Eventlevel).HasColumnName("eventlevel");

                entity.Property(e => e.RoamingId)
                    .HasColumnName("roaming_id")
                    .HasMaxLength(36);
            });

            modelBuilder.Entity<EventsAlarm>(entity =>
            {
                entity.ToTable("events_alarm");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.CommandPlanId)
                    .HasColumnName("command_plan_id")
                    .HasMaxLength(36);

                entity.Property(e => e.EventCode)
                    .HasColumnName("event_code")
                    .HasMaxLength(36);

                entity.Property(e => e.EventLevel)
                    .HasColumnName("event_level")
                    .HasMaxLength(36);

                entity.Property(e => e.EventType)
                    .HasColumnName("event_type")
                    .HasMaxLength(36);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.OnOff).HasColumnName("on_off");
            });

            modelBuilder.Entity<EventsData>(entity =>
            {
                entity.ToTable("events_data");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Equipmentcode)
                    .HasColumnName("equipmentcode")
                    .HasMaxLength(36);

                entity.Property(e => e.EventId)
                    .HasColumnName("event_id")
                    .HasMaxLength(36);

                entity.Property(e => e.EventsData1).HasColumnName("events_data");
            });

            modelBuilder.Entity<Facecomputing>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("facecomputing");

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Equipmentcode)
                    .HasColumnName("equipmentcode")
                    .HasMaxLength(255);

                entity.Property(e => e.EventsId)
                    .HasColumnName("events_id")
                    .HasMaxLength(36);

                entity.Property(e => e.Picturepath)
                    .HasColumnName("picturepath")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Facedata>(entity =>
            {
                entity.ToTable("facedata");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Picturepath)
                    .HasColumnName("picturepath")
                    .HasMaxLength(255);

                entity.Property(e => e.Usercode)
                    .HasColumnName("usercode")
                    .HasMaxLength(255);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Keyareas>(entity =>
            {
                entity.ToTable("keyareas");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Electronicfence).HasColumnName("electronicfence");

                entity.Property(e => e.Fencecolor).HasColumnName("fencecolor");

                entity.Property(e => e.Fencestyle).HasColumnName("fencestyle");

                entity.Property(e => e.Matrixcolor).HasColumnName("matrixcolor");

                entity.Property(e => e.Matrixcoordinates).HasColumnName("matrixcoordinates");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(36);
            });

            modelBuilder.Entity<Marker>(entity =>
            {
                entity.ToTable("marker");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Picture)
                    .HasColumnName("picture")
                    .HasMaxLength(255);

                entity.Property(e => e.Pointlist)
                    .HasColumnName("pointlist")
                    .HasMaxLength(255);

                entity.Property(e => e.Special)
                    .HasColumnName("special")
                    .HasMaxLength(255);

                entity.Property(e => e.Style)
                    .HasColumnName("style")
                    .HasMaxLength(255);

                entity.Property(e => e.Text).HasColumnName("text");
            });

            modelBuilder.Entity<MarkerRecord>(entity =>
            {
                entity.ToTable("marker_record");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Data).HasColumnName("data");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(36);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(36);
            });

            modelBuilder.Entity<PersonnelPositioning>(entity =>
            {
                entity.ToTable("personnel_positioning");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Coordinatetype)
                    .HasColumnName("coordinatetype")
                    .HasMaxLength(255);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.KeyareasId)
                    .HasColumnName("keyareas_id")
                    .HasMaxLength(36);

                entity.Property(e => e.Offsetpoint).HasColumnName("offsetpoint");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<PersonnelPositioningdata>(entity =>
            {
                entity.ToTable("personnel_positioningdata");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.BasemapId)
                    .HasColumnName("basemap_id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Data).HasColumnName("data");

                entity.Property(e => e.Latitude).HasColumnName("latitude");

                entity.Property(e => e.Longitude).HasColumnName("longitude");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(255);

                entity.Property(e => e.Usercode)
                    .HasColumnName("usercode")
                    .HasMaxLength(36);
            });

            modelBuilder.Entity<Roaming>(entity =>
            {
                entity.ToTable("roaming");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Isend).HasColumnName("isend");

                entity.Property(e => e.Rate).HasColumnName("rate");

                entity.Property(e => e.Thumbnail).HasColumnName("thumbnail");

                entity.Property(e => e.UserRoutedataId)
                    .HasColumnName("user_routedata_id")
                    .HasMaxLength(36);
            });

            modelBuilder.Entity<RoamingRecord>(entity =>
            {
                entity.ToTable("roaming_record");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Position).HasColumnName("position");

                entity.Property(e => e.RoamingId)
                    .HasColumnName("roaming_id")
                    .HasMaxLength(36);

                entity.Property(e => e.Servertime).HasColumnName("servertime");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Permission).HasColumnName("permission");

                entity.Property(e => e.Rolename)
                    .HasColumnName("rolename")
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<RolesPermissions>(entity =>
            {
                entity.ToTable("roles_permissions");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.Pid)
                    .HasColumnName("pid")
                    .HasMaxLength(36);
            });

            modelBuilder.Entity<RouteEquipment>(entity =>
            {
                entity.ToTable("route_equipment");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Equipmentcode)
                    .HasColumnName("equipmentcode")
                    .HasMaxLength(36);

                entity.Property(e => e.Important).HasColumnName("important");

                entity.Property(e => e.RouteId)
                    .HasColumnName("route_id")
                    .HasMaxLength(36);
            });

            modelBuilder.Entity<RouteUser>(entity =>
            {
                entity.ToTable("route_user");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(32);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.RouteId)
                    .HasColumnName("route_id")
                    .HasMaxLength(32);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<Routedata>(entity =>
            {
                entity.ToTable("routedata");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Data).HasColumnName("data");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.Isadministrator).HasColumnName("isadministrator");

                entity.Property(e => e.Nickname)
                    .HasColumnName("nickname")
                    .HasMaxLength(32);

                entity.Property(e => e.Portrait).HasColumnName("portrait");

                entity.Property(e => e.Psd)
                    .HasColumnName("psd")
                    .HasMaxLength(255);

                entity.Property(e => e.Salt)
                    .HasColumnName("salt")
                    .HasMaxLength(255);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(16);
            });

            modelBuilder.Entity<UsersRoles>(entity =>
            {
                entity.ToTable("users_roles");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(36);

                entity.Property(e => e.Createtime).HasColumnName("createtime");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasMaxLength(36);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasMaxLength(36);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
