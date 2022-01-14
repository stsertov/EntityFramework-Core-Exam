namespace Artillery.DataProcessor.ExportDto
{
    public class ShellJsonExportDto
    {

        public double ShellWeight { get; set; }

        public string Caliber { get; set; }

        public GunJsonExportDto[] Guns { get; set; }
    }
}
