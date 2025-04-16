using KubisDeneme.DTO;

namespace KubisDeneme.Service
{
    public interface IKitapTuruService
    {
        List<KitapTuruDTO>? TumKitapTurleriniGetir();
        KitapTuruDTO? KitapTuruGetir(int id);
        void KitapTuruEkle(KitapTuruDTO kitapTuruDTO);
        void KitapTuruGuncelle(KitapTuruDTO kitapTuruDTO);
        void KitapTuruSil(int id);
    }
}
