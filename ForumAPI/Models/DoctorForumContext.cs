using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ForumAPI.Models
{
    public partial class DoctorForumContext : DbContext
    {
        public DoctorForumContext()
        {
        }

        public DoctorForumContext(DbContextOptions<DoctorForumContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Infomation> Infomations { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<TopicComment> TopicComments { get; set; }
        public virtual DbSet<TopicReply> TopicReplies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=DoctorForum;User=sa;Password=1");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CateId)
                    .HasName("PK__Categori__2968AA5EE4215F6F");

                entity.Property(e => e.CateId).HasColumnName("Cate_id");

                entity.Property(e => e.CreatedAt)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Created_at");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Created_by");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CusId)
                    .HasName("PK__Customer__0AC8019F870190B1");

                entity.ToTable("Customer");

                entity.Property(e => e.CusId).HasColumnName("Cus_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(e => e.DocId)
                    .HasName("PK__Doctor__46463FD9B1FAFBFF");

                entity.ToTable("Doctor");

                entity.Property(e => e.DocId).HasColumnName("Doc_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Created_at");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Experience)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Professional)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Qualification)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("Role_id");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Doctor__Role_id__30F848ED");
            });

            modelBuilder.Entity<Infomation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Infomation");

                entity.Property(e => e.Logo)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("Role_id");

                entity.Property(e => e.RoleInfo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Role_info");
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("Topic");

                entity.Property(e => e.TopicId).HasColumnName("Topic_id");

                entity.Property(e => e.CategogiesId).HasColumnName("Categogies_id");

                entity.Property(e => e.Contents).HasColumnType("text");

                entity.Property(e => e.CreatedAt)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Created_at");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Categogies)
                    .WithMany(p => p.Topics)
                    .HasForeignKey(d => d.CategogiesId)
                    .HasConstraintName("FK__Topic__Categogie__31EC6D26");
            });

            modelBuilder.Entity<TopicComment>(entity =>
            {
                entity.HasKey(e => e.CommentId)
                    .HasName("PK__Topic_co__99D3E6C3CDEFB7CC");

                entity.ToTable("Topic_comment");

                entity.Property(e => e.CommentId).HasColumnName("Comment_id");

                entity.Property(e => e.Comment).HasColumnType("text");

                entity.Property(e => e.CreatedAt)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Created_at");

                entity.Property(e => e.TopicId).HasColumnName("Topic_id");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.TopicComments)
                    .HasForeignKey(d => d.TopicId)
                    .HasConstraintName("FK__Topic_com__Topic__32E0915F");
            });

            modelBuilder.Entity<TopicReply>(entity =>
            {
                entity.HasKey(e => e.ReplyId)
                    .HasName("PK__Topic_re__B660369C1811B85C");

                entity.ToTable("Topic_reply");

                entity.Property(e => e.ReplyId).HasColumnName("Reply_id");

                entity.Property(e => e.CommentId).HasColumnName("Comment_id");

                entity.Property(e => e.CreatedAt)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Created_at");

                entity.Property(e => e.Reply).HasColumnType("text");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.TopicReplies)
                    .HasForeignKey(d => d.CommentId)
                    .HasConstraintName("FK__Topic_rep__Comme__33D4B598");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
