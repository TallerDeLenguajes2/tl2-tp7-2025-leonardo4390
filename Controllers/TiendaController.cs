using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class TiendaController : ControllerBase
{
    private readonly IRepository _repository;

    public TiendaController()
    {
        _repository = new ProductoRepository();
    }

    [HttpGet("productos")]
    public ActionResult<List<Productos>> GetAll()
    {
        var productos = _repository.GetAll();
        return Ok(productos);
    }

    [HttpGet("producto")]
    public ActionResult<List<Productos>> GetById(int idProducto)
    {
        var producto = _repository.GetById(idProducto);

        if (producto == null)
            return NotFound("Producto no encontrado");

        return Ok(producto);

    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] Productos producto)
    {
        _repository.Create(producto);

        return Ok("Producto creado");
    }

    [HttpPut("update")]
    public IActionResult Update(int id, [FromBody] Productos producto)
    {
        var existeProducto = _repository.GetById(id);

        if (existeProducto == null)
            return NotFound("Producto no encontrado");

        producto.IdProducto = id;

        _repository.Update(producto);
        return Ok("Producto actualizado");
    }

    [HttpPut("Remove")]
    public IActionResult Remove(int idProducto)
    {
        var productoExistente = _repository.GetById(idProducto);
        if (productoExistente == null)
            return NotFound("Producto no encontrado");

        _repository.Remove(idProducto);

        return Ok("Producto eliminado");
    }
    
}
