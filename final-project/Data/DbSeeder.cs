using final_project.Models;

namespace final_project.Data;

public static class DbSeeder
{
    public static void Seed(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<FinalCSI402DB>();

        SeedRoles(db);
        SeedUsers(db);
        SeedCategories(db);
        SeedProducts(db);
    }

    private static void SeedRoles(FinalCSI402DB db)
    {
        if (db.Roles.Any()) return;

        db.Roles.AddRange(
            new Role { RoleName = "Admin" },
            new Role { RoleName = "Seller" },
            new Role { RoleName = "Client" }
        );
        db.SaveChanges();
    }

    private static void SeedUsers(FinalCSI402DB db)
    {
        if (db.Users.Count() >= 10) return;

        var adminRoleId = db.Roles.FirstOrDefault(r => r.RoleName == "Admin")?.RoleId ?? 1;
        var sellerRoleId = db.Roles.FirstOrDefault(r => r.RoleName == "Seller")?.RoleId ?? 2;
        var clientRoleId = db.Roles.FirstOrDefault(r => r.RoleName == "Client")?.RoleId ?? 3;

        var seedUsers = new List<User>
        {
            new() { UserName = "admin", Password = "1234", Email = "admin@glamora.com", RoleId = adminRoleId },
            new() { UserName = "seller1", Password = "1234", Email = "seller1@glamora.com", RoleId = sellerRoleId },
            new() { UserName = "seller2", Password = "1234", Email = "seller2@glamora.com", RoleId = sellerRoleId },
            new() { UserName = "client1", Password = "1234", Email = "client1@glamora.com", RoleId = clientRoleId },
            new() { UserName = "client2", Password = "1234", Email = "client2@glamora.com", RoleId = clientRoleId },
            new() { UserName = "client3", Password = "1234", Email = "client3@glamora.com", RoleId = clientRoleId },
            new() { UserName = "client4", Password = "1234", Email = "client4@glamora.com", RoleId = clientRoleId },
            new() { UserName = "client5", Password = "1234", Email = "client5@glamora.com", RoleId = clientRoleId },
            new() { UserName = "client6", Password = "1234", Email = "client6@glamora.com", RoleId = clientRoleId },
            new() { UserName = "client7", Password = "1234", Email = "client7@glamora.com", RoleId = clientRoleId },
        };

        var existingNames = db.Users.Select(u => u.UserName).ToHashSet();
        var toAdd = seedUsers.Where(u => !existingNames.Contains(u.UserName)).ToList();
        if (toAdd.Any())
        {
            db.Users.AddRange(toAdd);
            db.SaveChanges();
        }
    }

    private static void SeedCategories(FinalCSI402DB db)
    {
        if (db.Categories.Count() >= 10) return;

        var seedCategories = new List<Category>
        {
            new() { CategoryName = "เสื้อยืด (T-Shirt)", Description = "เสื้อยืดคอกลม คอวี ผ้าคอตตอน" },
            new() { CategoryName = "เสื้อเชิ้ต (Shirt)", Description = "เสื้อเชิ้ตแขนสั้น แขนยาว" },
            new() { CategoryName = "กางเกง (Pants)", Description = "กางเกงยีนส์ กางเกงสแล็ค กางเกงขาสั้น" },
            new() { CategoryName = "ชุดเดรส / กระโปรง (Dress)", Description = "ชุดเดรสและกระโปรงแฟชั่น" },
            new() { CategoryName = "เสื้อฮู้ด / สเวตเตอร์ (Hoodie)", Description = "เสื้อกันหนาวมีฮู้ดและสเวตเตอร์ผ้าฟลีซ" },
            new() { CategoryName = "แจ็คเก็ต / เสื้อคลุม (Jacket & Coat)", Description = "เสื้อโค้ท เบลเซอร์ และแจ็คเก็ตแฟชั่น" },
            new() { CategoryName = "กางเกงยีนส์ (Jeans)", Description = "กางเกงยีนส์ทรงตรง ทรงกระบอก และทรงหลวม" },
            new() { CategoryName = "ชุดกีฬา (Activewear)", Description = "เสื้อผ้าออกกำลังกายระบายอากาศได้ดี" },
            new() { CategoryName = "ชุดทำงาน (Workwear)", Description = "ชุดสูท กางเกงสแล็ค และเสื้อเชิ้ตทางการ" },
            new() { CategoryName = "แอคเซสซอรี่ (Accessories)", Description = "หมวก เข็มขัด ผ้าพันคอ และกระเป๋าแฟชั่น" },
        };

        var existingNames = db.Categories.Select(c => c.CategoryName).ToHashSet();
        var toAdd = seedCategories.Where(c => !existingNames.Contains(c.CategoryName)).ToList();
        if (toAdd.Any())
        {
            db.Categories.AddRange(toAdd);
            db.SaveChanges();
        }
    }

    private static void SeedProducts(FinalCSI402DB db)
    {
        if (db.Products.Count() >= 10) return;

        var categories = db.Categories.ToList();
        int CatId(int index) => categories.Count > index ? categories[index].CategoryId : categories.First().CategoryId;

        var seedProducts = new List<Product>
        {
            new() { ProductName = "Nadetta Coat Beige", CategoryId = CatId(5), Size = "L", Color = "Beige", Gender = "ผู้หญิง", Description = "เสื้อโค้ทตัวยาวทรงหลวม ผ้าทอเนื้อนุ่ม ใส่สบาย", Price = 2490, Stock = 25 },
            new() { ProductName = "Oversize Cotton T-Shirt", CategoryId = CatId(0), Size = "M", Color = "White", Gender = "Unisex", Description = "เสื้อยืด Oversize ผ้า Cotton 100% ระบายอากาศดี", Price = 490, Stock = 80 },
            new() { ProductName = "Classic Oxford Shirt", CategoryId = CatId(1), Size = "L", Color = "Sky Blue", Gender = "ผู้ชาย", Description = "เสื้อเชิ้ต Oxford แขนยาว ทรงสุภาพ ใส่ทำงานได้", Price = 890, Stock = 45 },
            new() { ProductName = "Wide-Leg Denim Jeans", CategoryId = CatId(6), Size = "M", Color = "Indigo", Gender = "Unisex", Description = "กางเกงยีนส์ขากระบอกใหญ่ ผ้าเดนิมฟอกสีสวย", Price = 1290, Stock = 40 },
            new() { ProductName = "Minimalist Wool Blazer", CategoryId = CatId(8), Size = "M", Color = "Charcoal", Gender = "Unisex", Description = "เบลเซอร์ทรงมินิมอล คัตติ้งเนี้ยบ เหมาะกับทุกโอกาส", Price = 1990, Stock = 20 },
            new() { ProductName = "Linen Summer Dress", CategoryId = CatId(3), Size = "S", Color = "Cream", Gender = "ผู้หญิง", Description = "ชุดเดรสผ้าลินินแท้ โปร่งสบาย เหมาะกับอากาศร้อน", Price = 1190, Stock = 35 },
            new() { ProductName = "Heavyweight Fleece Hoodie", CategoryId = CatId(4), Size = "XL", Color = "Black", Gender = "Unisex", Description = "เสื้อฮู้ดผ้าฟลีซหนา 400gsm ทรงสวย อุ่นสบาย", Price = 1090, Stock = 50 },
            new() { ProductName = "Relaxed Chino Pants", CategoryId = CatId(2), Size = "L", Color = "Khaki", Gender = "ผู้ชาย", Description = "กางเกงชิโน่ทรงหลวม ผ้านุ่มยืดหยุ่น ใส่สบายทั้งวัน", Price = 990, Stock = 60 },
            new() { ProductName = "Dry-Fit Training Tee", CategoryId = CatId(7), Size = "M", Color = "Navy", Gender = "Unisex", Description = "เสื้อออกกำลังกายแห้งไว น้ำหนักเบา ไม่อับชื้น", Price = 590, Stock = 70 },
            new() { ProductName = "Cropped Bomber Jacket", CategoryId = CatId(5), Size = "M", Color = "Brown", Gender = "ผู้หญิง", Description = "แจ็คเก็ตบอมเบอร์ทรงครอป สไตล์สตรีทเรียบหรู", Price = 1590, Stock = 30 },
        };

        var existingNames = db.Products.Select(p => p.ProductName).ToHashSet();
        var toAdd = seedProducts.Where(p => !existingNames.Contains(p.ProductName)).ToList();
        if (toAdd.Any())
        {
            db.Products.AddRange(toAdd);
            db.SaveChanges();
        }
    }
}
