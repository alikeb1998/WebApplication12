namespace Iz.Online.OmsModels.ResponsModels.Order
{
    public class SubSector
    {
        public int id { get; set; }
        public string gi { get; set; }
        public string ri { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public Sector sector { get; set; }
        public string ky { get; set; }
    }
}
