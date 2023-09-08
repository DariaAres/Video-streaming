using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VideoStreaming.Models;

namespace VideoStreaming.Persistence
{
    public class VideoStreamingDbContext : IdentityDbContext<User>
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<FileData> FilesData { get; set; }

        public VideoStreamingDbContext(DbContextOptions<VideoStreamingDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.Property(u => u.Name)
                    .IsRequired();
                entity.Property(u => u.Surname)
                    .IsRequired();
                entity.Ignore(c => c.FullName);

                entity.HasIndex(u => u.UserName);

                entity.HasQueryFilter(u => u.UserName != "admin"); // to not have admin in a selection
                entity.HasQueryFilter(u => u.EmailConfirmed);
            });

            builder.Entity<Participant>(entity =>
            {
                entity.HasKey(p => new { p.UserId, p.RoomId });
                
                entity.Property(p => p.UserId)
                    .IsRequired();
                entity.Property(p => p.RoomId)
                    .IsRequired();
            });

            builder.Entity<Room>(entity =>
            {
                entity.Property(r => r.Title)
                    .IsRequired();
                entity.Property(r => r.OwnerId)
                    .IsRequired();

                entity.HasOne(r => r.File)
                    .WithMany();
            });

            builder.Entity<ChatMessage>(entity =>
            {
                entity.Property(m => m.Date)
                    .IsRequired();
                entity.Property(m => m.UserId)
                    .IsRequired();
                entity.Property(m => m.RoomId)
                    .IsRequired();
                entity.Property(m => m.Text)
                    .IsRequired();
            });

            builder.Entity<FileData>(entity =>
            {
                entity.HasKey(i => i.FileId);
                entity.Property(i => i.FileName)
                    .IsRequired();
                entity.Property(i => i.Description)
                    .IsRequired();
                entity.Property(i => i.Extension)
                    .IsRequired();
            });
        }
    }
}
