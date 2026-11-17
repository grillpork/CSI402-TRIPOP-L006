using Microsoft.EntityFrameworkCore;

namespace tripop_8021_lab006.Models;

public partial class CSI402DB : DbContext
{
    public CSI402DB() { }

    public CSI402DB(DbContextOptions<CSI402DB> options)
        : base(options) { }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<LabStudent> LabStudents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "server=localhost;port=3306;database=CSI402DB;user=root;password=130946;SslMode=None;AllowPublicKeyRetrieval=True;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
