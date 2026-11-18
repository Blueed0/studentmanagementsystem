namespace StudentManagementSystem.Interfaces
{
    // IIdentifiable: Kimliği olan varlıklar için ortak arayüz sözleşmesi
    // Amaç: Student ve Course sınıflarının Id alanını zorunlu kılmak (interface örneği)
    public interface IIdentifiable
    {
        // Id: Varlığın benzersiz kimliğini sadece okunabilir şekilde dışarı açar
        string Id { get; }
    }
}
