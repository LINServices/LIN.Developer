namespace LIN.Developer.Data;


public class Context : DbContext
{


    /// <summary>
    /// Acceso a las Api keys
    /// </summary>
    public DbSet<ApiKeyDataModel> ApiKeys { get; set; }


    /// <summary>
    /// Reglas del firewall
    /// </summary>
    public DbSet<FirewallRuleDataModel> FirewallRule { get; set; }


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
    public DbSet<ApiKeyUsesDataModel> ApikeyUses { get; set; }



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

        modelBuilder.Entity<ApiKeyDataModel>()
           .HasIndex(e => e.Key)
           .IsUnique();

        modelBuilder.Entity<ProfileDataModel>()
           .HasIndex(e => e.UserID)
           .IsUnique();

        modelBuilder.Entity<ProfileDataModel>()
          .HasIndex(e => e.Email)
          .IsUnique();

        modelBuilder.Entity<FirewallRuleDataModel>()
           .HasIndex(e => e.ID)
           .IsUnique();

        modelBuilder.Entity<ProjectDataModel>()
           .HasIndex(e => e.ID);

        modelBuilder.Entity<FirewallBlockLogDataModel>()
          .HasKey(e => new { e.IPv4, e.ProyectoID });

  
        // Nombres de las tablas
        modelBuilder.Entity<ApiKeyDataModel>().ToTable("API_KEYS");
        modelBuilder.Entity<ApiKeyUsesDataModel>().ToTable("API_USAGES");
        modelBuilder.Entity<ProfileDataModel>().ToTable("DEVELOPERS_PROFILES");
        modelBuilder.Entity<TransactionDataModel>().ToTable("TRANSACTIONS");
        modelBuilder.Entity<FirewallRuleDataModel>().ToTable("FIREWALL_RULES");
        modelBuilder.Entity<ProjectDataModel>().ToTable("PROJECTS");
        modelBuilder.Entity<FirewallBlockLogDataModel>().ToTable("FIREWALL_BLOCKS");

    }

}
