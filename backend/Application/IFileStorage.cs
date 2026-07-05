namespace Application
{
    // Apstrakcija skladištenja fajlova (infrastruktura se implementira u API sloju).
    public interface IFileStorage
    {
        // Snima bajtove i vraća relativnu putanju/URL (npr. "/Temp/{guid}.pdf").
        string Save(byte[] content, string originalFileName);

        void Delete(string relativePath);
    }
}
