namespace MyBlog.Web.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MyBlogContext : DbContext
    {
        public MyBlogContext()
            : base("name=MyBlogContext")
        {
        }

        public virtual DbSet<artice_type> artice_type { get; set; }

        public virtual DbSet<admin> admin { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<artice_type>()
                .Property(e => e.typename)
                .IsUnicode(false);

            modelBuilder.Entity<artice_type>()
                .Property(e => e.seo_title)
                .IsUnicode(false);

            modelBuilder.Entity<artice_type>()
                .Property(e => e.seo_key)
                .IsUnicode(false);

            modelBuilder.Entity<artice_type>()
                .Property(e => e.seo_description)
                .IsUnicode(false);
        }
    }
}
