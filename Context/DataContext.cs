using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using vennAPI.Models;

namespace vennAPI.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<RoomModel> Rooms { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<RoomMember> RoomMembers { get; set; }
        public DbSet<UserAvailability> UserAvailability { get; set; }

        // configure our entities or queries how we specify it
        //This is used to override default conventions / configure relationships manually
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friend>()
            .HasOne(f => f.Requester)
            .WithMany()
            .HasForeignKey(f => f.RequesterId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
            .HasOne(f => f.Receiver)
            .WithMany()
            .HasForeignKey(f => f.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserAvailability>()
            .HasIndex(x => new { x.UserId, x.Day, x.Hour })
            .IsUnique();

            // modelBuilder.Entity<RoomModel>()
            // .HasOne(r => r.UserModel)
            // .WithMany()
            // .HasForeignKey(r => r.UserId)
            // .OnDelete(DeleteBehavior.Restrict); // or NoAction

            // RoomMember → User (member)
            modelBuilder.Entity<RoomMember>()
                .HasOne(user => user.MemberInfo)
                .WithMany()
                .HasForeignKey(user => user.UserModelId)
                .OnDelete(DeleteBehavior.Restrict); // THIS is the important one

            // RoomMember → Room
            // modelBuilder.Entity<RoomMember>()
            //     .HasOne(rm => rm.Room)
            //     .WithMany(r => r.Members)
            //     .HasForeignKey(rm => rm.RoomModelId)
            //     .OnDelete(DeleteBehavior.Cascade); // safe to keep cascade here
        }


    }
}