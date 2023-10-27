namespace LIN.Developer.Data;


public class Context : DbContext
{

    /// <summary>
    /// Acceso a las keys.
    /// </summary>
    public DbSet<KeyModel> Keys { get; set; }


    /// <summary>
    /// Reglas del firewall
    /// </summary>
    public DbSet<FirewallRuleModel> FirewallRules { get; set; }


    /// <summary>
    /// Bloqueos del cortafuegos
    /// </summary>
    public DbSet<FirewallBlockLogDataModel> FirewallBlockLogs { get; set; }


    /// <summary>
    /// IPs aceptadas
    /// </summary>
    public DbSet<ProjectDataModel> Proyectos { get; set; }



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

        modelBuilder.Entity<KeyModel>()
           .HasIndex(e => e.Key)
           .IsUnique();

        modelBuilder.Entity<ProfileDataModel>()
           .HasIndex(e => e.AccountID)
           .IsUnique();

        modelBuilder.Entity<ProfileDataModel>()
          .HasIndex(e => e.Email)
          .IsUnique();

        modelBuilder.Entity<FirewallRuleModel>()
           .HasIndex(e => e.ID)
           .IsUnique();

        modelBuilder.Entity<ProjectDataModel>()
           .HasIndex(e => e.ID);

        modelBuilder.Entity<FirewallBlockLogDataModel>()
          .HasKey(e => new { e.IPv4, e.ProyectoID });



        modelBuilder.Entity<BillingItemModel>()
            .HasOne(T => T.Key)
            .WithMany()
            .HasForeignKey(t => t.KeyId);

        modelBuilder.Entity<BillingItemModel>()
           .HasOne(T => T.Transaction)
           .WithMany()
           .HasForeignKey(t => t.TransactionId);


        modelBuilder.Entity<FirewallRuleModel>()
          .HasOne(T => T.Project)
          .WithMany()
          .HasForeignKey(t => t.ProjectId);

        modelBuilder.Entity<KeyModel>()
          .HasOne(T => T.Project)
          .WithMany()
          .HasForeignKey(t => t.ProjectId);

        modelBuilder.Entity<OTPDataModel>()
          .HasOne(T => T.Profile)
          .WithMany()
          .HasForeignKey(t => t.ProfileId);


        modelBuilder.Entity<ProfileDataModel>()
          .HasMany(T => T.Transactions)
          .WithMany();


        modelBuilder.Entity<ProjectDataModel>()
         .HasMany(T => T.FirewallRules)
         .WithMany();

        modelBuilder.Entity<ProjectDataModel>()
         .HasMany(T => T.Keys)
         .WithMany();


        modelBuilder.Entity<TransactionDataModel>()
         .HasOne(T => T.Billing)
         .WithMany()
         .HasForeignKey(t => t.BillingId);

        modelBuilder.Entity<TransactionDataModel>()
        .HasOne(T => T.Profile)
        .WithMany()
        .HasForeignKey(t => t.ProfileID);




        // Nombres de las tablas.
        modelBuilder.Entity<BillingItemModel>().ToTable("BILLING_ITEMS");
        modelBuilder.Entity<FirewallBlockLogDataModel>().ToTable("FIREWALL_BLOCKS");
        modelBuilder.Entity<FirewallRuleModel>().ToTable("FIREWALL_RULES");
        modelBuilder.Entity<KeyModel>().ToTable("KEYS");
        modelBuilder.Entity<OTPDataModel>().ToTable("OTPS");
        modelBuilder.Entity<ProfileDataModel>().ToTable("PROFILES");
        modelBuilder.Entity<ProjectDataModel>().ToTable("PROJECTS");
        modelBuilder.Entity<TransactionDataModel>().ToTable("TRANSACTIONS");

    }

}