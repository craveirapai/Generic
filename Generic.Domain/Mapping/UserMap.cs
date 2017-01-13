using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.Domain.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {

            this.ToTable("User");
            this.HasKey(x => x.Id);

            this.Property(x => x.Id).HasColumnName("Id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(x => x.Name).HasColumnName("Name");
            this.Property(x => x.Email).HasColumnName("Email");
            this.Property(x => x.Password).HasColumnName("Password");
            this.Property(x => x.PicturePath).HasColumnName("PicturePath");
            this.Property(x => x.IsEnabled).HasColumnName("IsEnabled");
            this.Property(x => x.DtRegister).HasColumnName("DtRegister");

            this.HasRequired(x => x.Role).WithMany().HasForeignKey(x => x.RoleId);
            //this.HasMany(x => x.Device).WithRequired().HasForeignKey(x => x.UserId);
        }

    }
}