using KubisDeneme.DTO;

namespace KubisDeneme.Service
{
    public interface IYazarService
    {
        List<YazarDTO> TumYazarlariGetir();
        YazarDTO? YazarGetir(int id);
        void YazarEkle(YazarDTO yazarDTO);
        void YazarGuncelle(YazarDTO yazarDTO);
        void YazarSil(int id);
    }
}
