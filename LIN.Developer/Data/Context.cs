using LIN.Types.Developer.Models;

namespace LIN.Developer.Data;


public class Context : DbContext
{


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
    public DbSet<ApiKeyUsesDataModel> ApiKeyUses { get; set; }



    /// <summary>
    /// Acceso a los perfiles de desarrollador
    /// </summary>
    public DbSet<ProfileDataModel> Profiles { get; set; }



    /// <summary>
    /// Transacciones de los créditos
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
           .HasIndex(e => e.AccountID)
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



        modelBuilder.Entity<ApiKeyUsesDataModel>()
            .HasOne(apiUse => apiUse.Transaction)
            .WithOne(p=>p.Use)
            .HasForeignKey<TransactionDataModel>(t => t.UseID);


        modelBuilder.Entity<TransactionDataModel>()
            .HasOne(p => p.Profile)
            .WithMany()
            .HasForeignKey(p=>p.ProfileID)
            .OnDelete(DeleteBehavior.Restrict);
        








        // Nombres de las tablas
        modelBuilder.Entity<ApiKeyUsesDataModel>().ToTable("API_USAGES");
        modelBuilder.Entity<ProfileDataModel>().ToTable("DEVELOPERS_PROFILES");
        modelBuilder.Entity<TransactionDataModel>().ToTable("TRANSACTIONS");
        modelBuilder.Entity<FirewallRuleDataModel>().ToTable("FIREWALL_RULES");
        modelBuilder.Entity<ProjectDataModel>().ToTable("PROJECTS");
        modelBuilder.Entity<FirewallBlockLogDataModel>().ToTable("FIREWALL_BLOCKS");

    }

}
