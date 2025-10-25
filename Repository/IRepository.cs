using System;

public interface IRepository
{
    public List<Productos> GetAll();

    public Productos GetById(int idProducto);

    public void Create(Productos productos);
    public void Remove(int idProducto);
    public void Update(Productos productos);
}