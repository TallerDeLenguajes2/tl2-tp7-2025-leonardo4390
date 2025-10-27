using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class TiendaController : ControllerBase
{
    private readonly IRepository<Productos> _repository;
    private readonly IRepository<Presupuestos> _repositoryPresupuesto;

    public TiendaController()
    {
        _repository = new ProductoRepository();
        _repositoryPresupuesto = new PresupuestosRepository();
    }

    [HttpGet("listar/productos")]
    public ActionResult<List<Productos>> GetAllProductos()
    {
        var productos = _repository.GetAll();
        return Ok(productos);
    }

    [HttpGet("listar/presupuestos")]
    public ActionResult<List<PresupuestosDetalles>> GetAllPresupuestos()
    {
        var presupuestos = _repositoryPresupuesto.GetAll();
        return Ok(presupuestos);
    }

    [HttpGet("producto/id")]
    public ActionResult<List<Productos>> GetById(int idProducto)
    {
        var producto = _repository.GetById(idProducto);

        if (producto == null)
            return NotFound("Producto no encontrado");

        return Ok(producto);

    }

    [HttpGet("presupuesto/id")]
    public ActionResult<List<Presupuestos>> GetByIdPresopuesto(int idPresupuesto)
    {
        var presupuesto = _repositoryPresupuesto.GetById(idPresupuesto);
        if(presupuesto == null)
            return NotFound("Presupuesto no encontrado");
        
        return Ok(presupuesto);
    }

    [HttpPost("create/producto")]
    public IActionResult Create([FromBody] Productos producto)
    {
        _repository.Create(producto);

        return Ok("Producto creado");
    }

    [HttpPost("create/presupuesto")]
    public IActionResult CreatePresupuesto([FromBody] Presupuestos presupuesto)
    {
        _repositoryPresupuesto.Create(presupuesto);
        return Ok("Presupuesto creado");
    }

    [HttpPut("update/producto")]
    public IActionResult Update(int id, [FromBody] Productos producto)
    {
        var existeProducto = _repository.GetById(id);

        if (existeProducto == null)
            return NotFound("Producto no encontrado");

        producto.IdProducto = id;

        _repository.Update(producto);
        return Ok("Producto actualizado");
    }

    [HttpPut("Remove/producto")]
    public IActionResult Remove(int idProducto)
    {
        var productoExistente = _repository.GetById(idProducto);
        if (productoExistente == null)
            return NotFound("Producto no encontrado");

        _repository.Remove(idProducto);

        return Ok("Producto eliminado");
    }

    [HttpPut("Remove/presupuesto")]
    public IActionResult RemovePresupuesto(int idPresupuesto)
    {
        var presupuestoExistente = _repositoryPresupuesto.GetById(idPresupuesto);
        if(presupuestoExistente == null)
            return NotFound("Presupuesto no encontrado");
        
        _repositoryPresupuesto.Remove(idPresupuesto);
        return Ok("Presupuesto eliminado");
    }    

    [HttpPost("presupuestos/detalles")]
    public IActionResult AgregarDetalle(int idPresupuesto, [FromQuery] int idProducto, [FromQuery] int cantidad)
    {
        if (cantidad <= 0)
            return BadRequest("La cantidad debe ser mayor a cero.");

        var presupuesto = _repositoryPresupuesto.GetById(idPresupuesto);
        if (presupuesto == null)
            return NotFound("No se encontró un presupuesto.");

        var producto = _repository.GetById(idProducto);
        if (producto == null)
            return NotFound("No existe el producto.");

        if (presupuesto.Detalle.Any(d => d.Producto?.IdProducto == idProducto))
            return Conflict("Ese producto ya está agregado al presupuesto.");

        _repositoryPresupuesto.AgregarDetalle(idPresupuesto, idProducto, cantidad);

        return Ok("Producto agregado al presupuesto.");
    }

}
