using MagicVilla_API.Modelos.Dto;

namespace MagicVilla_API.Datos
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto{Id=1,Nombre="El valle de aburra",Ocupantes=300,MetrosCuadrados=50},
            new VillaDto{Id=2,Nombre="La isla San Martinez",Ocupantes=250,MetrosCuadrados=67},
            new VillaDto{Id=3,Nombre="Vista a la playa",Ocupantes=10,MetrosCuadrados=78},            
        };
    }
}
