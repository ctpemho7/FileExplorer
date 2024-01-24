namespace FileExplorer
{
    public static class Utils
    {
        public static string HumanReadableFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int i = 0;
            while (bytes >= 1024)
            {
                bytes /= 1024;
                i++;
            }

            return $"{bytes} {sizes[i]}";
        }
    }
}
