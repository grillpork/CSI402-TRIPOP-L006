using Microsoft.EntityFrameworkCore;

namespace final_project.Models;

public partial class FinalCSI402DB : DbContext
{
    public FinalCSI402DB() { }

    public FinalCSI402DB(DbContextOptions<FinalCSI402DB> options)
        : base(options) { }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<ShopSocialLink> ShopSocialLinks { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<ShopWallet> ShopWallets { get; set; }

    public virtual DbSet<WalletTransaction> WalletTransactions { get; set; }

    public virtual DbSet<Media> Media { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "server=localhost;port=3306;database=final_csi402;user=root;password=130946;SslMode=None;AllowPublicKeyRetrieval=True;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
