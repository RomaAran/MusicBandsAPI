namespace Kursovaya_var6.Models
{
    public class BandCreateDto
    {
        public string GroupName { get; set; } = string.Empty;

        public string Leader { get; set; } = string.Empty;

        public string Genre { get; set; } = string.Empty;

        public int AlbumsCount { get; set; }
    }
}