using GR.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace GR.Infrastructure.Extension
{
    public class BookMap
    {
        public BookMap(EntityTypeBuilder <Book> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Name).IsRequired();
            entityBuilder.Property(x => x.ISBN).IsRequired();
            entityBuilder.Property(x => x.Publisher).IsRequired();
            entityBuilder.HasOne(e => e.Author).WithMany(e => e.Books).HasForeignKey(e => e.AuthorId);//.OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}
