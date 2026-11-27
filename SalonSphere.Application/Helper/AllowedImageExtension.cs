


namespace SalonSphere.Application.Helper
{
    public static class AllowedImageExtension
    {
        private static readonly string[] AllowedExtensions =
        {
            ".jpg", ".jpeg", ".png", ".webp"
        };

        public static bool IsValid(string? fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            var extension = Path.GetExtension(fileName);

            return AllowedExtensions.Contains(extension,StringComparer.OrdinalIgnoreCase);
        }

        public static string[] GetAllowedExtensions()
        {
            return AllowedExtensions;
        }

    }
}
