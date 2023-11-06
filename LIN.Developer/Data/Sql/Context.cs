namespace LIN.Developer.Data.Sql;


public class Context : DbContext
{


    /// <summary>
    /// Acceso a los usos en servicios
    /// </summary>
    public DbSet<BillingItemModel> BillingItems { get; set; }



    /// <summary>
    /// Acceso a los perfiles de desarrollador
    /// </summary>
    public DbSet<ProfileDataModel> Profiles { get; set; }



    /// <summary>
    /// Transacciones de los creditos
    /// </summary>
    public DbSet<TransactionDataModel> Transactions { get; set; }



    /// <summary>
    /// Transacciones de los creditos
    /// </summary>
    public DbSet<OTPDataModel> OTP { get; set; }



    /// <summary>
    /// Nuevo contexto a la base de datos
    /// </summary>
    public Context(DbContextOptions<Context> options) : base(options) { }



    /// <summary>
    /// Cuando se esta creando la BD
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<ProfileDataModel>()
           .HasIndex(e => e.AccountID)
           .IsUnique();

        modelBuilder.Entity<ProfileDataModel>()
          .HasIndex(e => e.Email)
          .IsUnique();




        modelBuilder.Entity<BillingItemModel>()
           .HasOne(T => T.Transaction)
           .WithMany()
           .HasForeignKey(t => t.TransactionId)
           .OnDelete(DeleteBehavior.NoAction);





        modelBuilder.Entity<OTPDataModel>()
          .HasOne(T => T.Profile)
          .WithMany()
          .HasForeignKey(t => t.ProfileId);


        modelBuilder.Entity<ProfileDataModel>()
          .HasMany(T => T.Transactions)
          .WithMany();



        modelBuilder.Entity<TransactionDataModel>()
         .HasOne(T => T.Billing)
         .WithMany()
         .HasForeignKey(t => t.BillingId);

        modelBuilder.Entity<TransactionDataModel>()
        .HasOne(T => T.Profile)
        .WithMany()
        .HasForeignKey(t => t.ProfileID)
        .OnDelete(DeleteBehavior.NoAction);


        // Nombres de las tablas.
        modelBuilder.Entity<BillingItemModel>().ToTable("BILLING_ITEMS");
        modelBuilder.Entity<OTPDataModel>().ToTable("OTPS");
        modelBuilder.Entity<ProfileDataModel>().ToTable("PROFILES");
        modelBuilder.Entity<TransactionDataModel>().ToTable("TRANSACTIONS");

    }

}