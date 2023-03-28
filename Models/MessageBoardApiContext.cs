using Microsoft.EntityFrameworkCore;

namespace MessageBoardApi.Models
{
    public class MessageBoardApiContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }
        
        public MessageBoardApiContext(DbContextOptions<MessageBoardApiContext> options) : base(options)
        {
        }

         // DB SEED:
    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<Message>()
        .HasData(
          new Message { MessageId=1, Name="Matilda", MessageString="Wooly Mammoth" }, 
          new Message { MessageId=2, Name="Rexi", MessageString="Dinosaur" }, 
          new Message { MessageId=3, Name="Matilda", MessageString="Dinosaur" }, 
          new Message { MessageId=4, Name="Pip", MessageString="Shark" }, 
          new Message { MessageId=5, Name="Bartholomew", MessageString="Dinosaur" } 
        );
    }
    }
}