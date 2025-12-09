using AuthAPI.Data; //Espacio de nombres donde se encuentra el DbContext.
using eShopModel.Classes; //Espacio de nombres que contiene las clases del modelo de datos.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Controllers
{
	[ApiController]
	[EnableCors(policyName: "Política de CORS")]
	[Route("Products")]
	[Authorize(Roles = "supervisor,admin", AuthenticationSchemes = "Bearer")]
	public class ProductsController : ControllerBase
	{

		private readonly EShopContext Context;

		private readonly ILogger<ProductsController> Logger;

		public ProductsController(EShopContext context, ILogger<ProductsController> logger)
		{
			Context = context;
			Logger = logger;
		}

		/// <summary>
		/// Método que busca un producto por su id y lo retorna.
		/// </summary>
		/// <param name="id">El id del producto a buscar.</param>
		/// <returns>
		/// Un producto cuyo id sea igual al ingresado, en caso de que exista.
		/// En caso de que no se encuentre, retorna null.
		/// </returns>
		[HttpGet("GetById")]
		public async Task<ActionResult> GetById(int id)
		{

			try
			{
				return Ok(await Context.Productos.FirstOrDefaultAsync(x => x.Id == id));
			}
			catch (Exception ex)
			{

				Logger.LogError(ex.Message);

				return StatusCode(500, "No se pudo buscar el producto especificado.");

			}

		}

		/// <summary>
		/// Método que retorna todos los productos ingresados en la base de datos.
		/// </summary>
		/// <returns>
		/// Una lista que contiene todos los productos.
		/// </returns>
		[HttpGet("GetAll")]
		public async Task<ActionResult> GetAll()
		{

			try
			{

				return Ok(await Context.Productos.ToListAsync());

			}
			catch (Exception ex)
			{

				Logger.LogError(ex.Message);

				return StatusCode(500, "No se pudo obtener la lista de productos.");

			}

		}

		/// <summary>
		/// Método que inserta un nuevo producto a la base de datos.
		/// </summary>
		/// <param name="data">El producto a ingresar.</param>
		[HttpPost("Insert")]
		public async Task<ActionResult> Insert(Producto data)
		{

			try
			{

				//Se deja el id del producto en 0 para que EFCore no arroje un error
				//de conflicto la autoincrementación de este atributo.
				data.Id = 0;

				Context.Productos.Add(data);

				await Context.SaveChangesAsync();

				return Ok();

			}
			catch (Exception ex)
			{

				Logger.LogError(ex.Message);

				return StatusCode(500, "No se pudo ingresar el producto.");
			}

		}

		/// <summary>
		/// Método para actualizar un producto existente en la base de datos.
		/// </summary>
		/// <param name="data">
		/// La información actualizada del producto. Su id debe ser el del
		/// producto a actualizar.
		/// </param>
		[HttpPut("Update")]
		public async Task<ActionResult> Update(Producto data)
		{

			try
			{

				/*Consultamos la base de datos para revisar si el producto existe.
				 Si no se encuentra el producto, retornamos una respuesta con el código 404.*/
				if (await Context.Productos.FirstOrDefaultAsync(x => x.Id == data.Id) == null)
					return NotFound();

				Context.Productos.Update(data);

				await Context.SaveChangesAsync();

				return Ok();

			}
			catch (Exception ex)
			{

				Logger.LogError(ex.Message);

				return StatusCode(500, "No se pudo actualizar el producto.");

			}

		}

		/// <summary>
		/// Método que elimina un producto de la base de datos según su id.
		/// </summary>
		/// <param name="id">El id del producto a eliminar.</param>
		[HttpDelete("Delete")]
		public async Task<ActionResult> Delete(int id)
		{

			try
			{

				await Context.Productos.Where(x => x.Id == id).ExecuteDeleteAsync();

				return Ok();

			}
			catch (Exception ex)
			{

				Logger.LogError(ex.Message);

				return StatusCode(500, "No se pudo eliminar el producto.");

			}

		}

	}
}
