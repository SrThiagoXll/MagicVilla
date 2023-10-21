using MagicVilla_API.Modelos;

namespace MagicVilla_API.Repositorio.IRpositorio
{
    public interface IVillaRepositorio:IRepositorio<Villa>
    {
        Task<Villa> Actualizar(Villa entidad);
    }
}
