using KubisDeneme.DTO;

namespace KubisDeneme.Service
{
    public interface IUlkeService
    {
        List<UlkeDTO> TumUlkeleriGetir();
        UlkeDTO? UlkeGetir(int Id);
        void UlkeEkle(UlkeDTO ulke);
        void UlkeGuncelle(UlkeDTO ulke);
        void UlkeSil(int Id);
    }
}
