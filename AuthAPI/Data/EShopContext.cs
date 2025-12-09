using Microsoft.EntityFrameworkCore;
using eShopModel.Classes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthAPI.Data;

public partial class EShopContext : IdentityDbContext<Usuario>
{
	public EShopContext()
	{
	}

	public EShopContext(DbContextOptions<EShopContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Producto> Productos { get; set; }

	public virtual DbSet<TipoProducto> TipoProductos { get; set; }

	/*La cadena de conexión no debería dejarse en el código fuente. Dado a que esto es un ejemplo, se
    dejará escrita para simplificar el proceso de creación del DbContext.*/
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder.UseSqlServer("Server=LAPTOP-DUPGS2D3\\LESERVER;Initial Catalog=eShop;Encrypt=False;Integrated Security=SSPI");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{

		modelBuilder.Entity<Producto>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC07D5DDF13D");

			entity.ToTable("Producto");

			entity.Property(e => e.Descripcion)
				.HasMaxLength(300)
				.IsUnicode(false);
			entity.Property(e => e.Nombre)
				.HasMaxLength(40)
				.IsUnicode(false);
			entity.Property(e => e.Precio).HasColumnType("money");

			entity.HasOne(d => d.TipoNavigation).WithMany(p => p.Productos)
				.HasForeignKey(d => d.Tipo)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Producto__Tipo__2B0A656D");
		});

		modelBuilder.Entity<TipoProducto>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__TipoProd__3214EC07A5B7A7BF");

			entity.ToTable("TipoProducto");

			entity.Property(e => e.Nombre)
				.HasMaxLength(40)
				.IsUnicode(false);
		});

		OnModelCreatingPartial(modelBuilder);

		/*Llamamos al método base para que el DbContext detecte
          las tablas que se heredan de la clase IdentityDbContext*/
		base.OnModelCreating(modelBuilder);

	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
