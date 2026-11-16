namespace StudentManagementSystem.Interfaces // Projenin arayüz (interface) dosyalarının bulunduğu isim alanı
{
    // IIdentifiable: Temel varlıkların benzersiz bir kimliğe (Id) sahip olmasını zorunlu kılar
    public interface IIdentifiable // Ortak kimlik sözleşmesini tanımlayan arayüz
    {
        // Id özelliği: Varlığın benzersiz kimliğini salt-okunur olarak dışarıya açar
        string Id { get; } // Sadece get; kimliğin dışarıdan değiştirilmesini engeller
    } // IIdentifiable arayüzünün sonu
} // StudentManagementSystem.Interfaces isim alanının sonu
