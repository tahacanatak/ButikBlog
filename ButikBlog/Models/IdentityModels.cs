﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ButikBlog.Models
{

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            //varsayilan deger
            IsEnabled = true;
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [StringLength(100)]
        public string Photo { get; set; }

        //ilk başta bool? 
        public bool IsEnabled { get; set; }


        public virtual ICollection<Post> Posts { get; set; } //her yazarın yazıları vardır
        public virtual ICollection<Comment> Comments { get; set; } 



    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("name = ApplicationDbContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // bire çoklu ilişkilerde baglantılı tabloların silinmesini engelliyor <...> isimli metodu kaldırdık
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>(); // one to manyler kapatıldı

            //Her yorumun zorunlu olarak bir yazısı vardır ve
            //o yazınında yorumları vardır
            //yazı silindiğinde yorumlarını da otomatik sil
            modelBuilder.Entity<Comment>()
                .HasRequired(x => x.Post)
                .WithMany(x => x.Comments)
                .WillCascadeOnDelete();
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

    }
}