using KubisDeneme.DTO;

namespace KubisDeneme.Service
{
    public interface IKitapService
    {
        List<KitapDTO> TumKitaplariGetir();
        KitapDTO? KitapGetir(int id);
        void KitapEkle(KitapDTO kitapDTO);
        void KitapGuncelle(KitapDTO kitapDTO);
        void KitapSil(int id);
    }
}
