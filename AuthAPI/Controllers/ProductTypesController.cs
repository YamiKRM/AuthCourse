using AuthAPI.Data; //Espacio de nombres donde se encuentra el DbContext.
using eShopModel.Classes; //Espacio de nombres que contiene las clases del modelo de datos.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Controllers
{

	[ApiController]
	[EnableCors(policyName: "Política de CORS")]
	[Route("ProductTypes")]
	[Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
	public class ProductTypesController : ControllerBase
	{

		private readonly EShopContext Context;

		private readonly ILogger<ProductTypesController> Logger;

		public ProductTypesController(EShopContext context, ILogger<ProductTypesController> logger)
		{
			Context = context;
			Logger = logger;
		}

		/// <summary>
		/// Método que busca un tipo de producto según su id.
		/// </summary>
		/// <param name="id">El id del tipo de producto a buscar</param>
		/// <returns>
		/// Si el tipo de producto es encontrado, este se retorna.
		/// En caso contrario, el método retorna null.
		/// </returns>
		[HttpGet("GetById")]
		public async Task<ActionResult> GetById(int id)
		{

			try
			{
				return Ok(await Context.TipoProductos.FirstOrDefaultAsync(x => x.Id == id));
			}
			catch (Exception ex)
			{

				Logger.LogError(ex.Message);

				return StatusCode(500, "No se pudo buscar el tipo de producto.");

			}

		}

		/// <summary>
		/// Método que retorna todos los tipos de producto contenidos en la base de datos.
		/// </summary>
		/// <returns>Una lista que contiene todos los tipos de producto.</returns>
		[HttpGet("GetAll")]
		public async Task<ActionResult> GetAll()
		{

			try
			{

				return Ok(await Context.TipoProductos.ToListAsync());

			}
			catch (Exception ex)
			{

				Logger.LogError(ex.Message);

				return StatusCode(500, "No se pudo consultar los tipos de producto");

			}

		}

		/// <summary>
		/// Metodo que ingresa un nuevo tipo de producto a la base de datos.
		/// </summary>
		/// <param name="data">El tipo de producto a ingresar.</param>
		[HttpPost("Insert")]
		public async Task<ActionResult> Insert(TipoProducto data)
		{

			try
			{

				//Se deja el id del producto en 0 para que EFCore no arroje un error
				//de conflicto la autoincrementación de este atributo.
				data.Id = 0;

				Context.TipoProductos.Add(data);

				await Context.SaveChangesAsync();

				return Ok();

			}
			catch (Exception ex)
			{

				Logger.LogError(ex.Message);

				return StatusCode(500, "No se pudo ingresar el tipo de producto.");

			}
			
		}

		/// <summary>
		/// Método que actualiza un tipo de producto existente en la base de datos.
		/// </summary>
		/// <param name="data">
		/// El información actualizada del tipo de producto. Su id debe ser igual al
		/// del tipo de producto a actualizar.
		/// </param>
		[HttpPut("Update")]
		public async Task<ActionResult> Update(TipoProducto data)
		{

			try
			{

				/*Consultamos la base de datos para revisar si el tipo de producto existe.
				  Si no se encuentra el producto, retornamos una respuesta con el código 404.*/
				if (await Context.TipoProductos.FirstOrDefaultAsync(x => x.Id == data.Id) == null)
					return NotFound();

				Context.TipoProductos.Update(data);

				await Context.SaveChangesAsync();

				return Ok();

			}
			catch (Exception ex)
			{

				Logger.LogError(ex.Message);

				return StatusCode(500, "No se pudo actualizar el tipo de producto.");

			}

		}

		/// <summary>
		/// Método que elimina un tipo de producto de la base de datos según su id.
		/// </summary>
		/// <param name="id">El id del producto a eliminar.</param> 
		[HttpDelete("Delete")]
		public async Task<ActionResult> Delete(int id)
		{

			try
			{

				await Context.TipoProductos.Where(x => x.Id == id).ExecuteDeleteAsync();

				return Ok();

			}
			catch (Exception ex)
			{

				Logger.LogError(ex.Message);

				return StatusCode(500, "No se pudo eliminar el tipo de producto." +
					"\nAsegúrate de que este no esté asociado a algún producto.");

			}

		}

	}
}
