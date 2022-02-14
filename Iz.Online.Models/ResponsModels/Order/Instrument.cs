namespace Iz.Online.OmsModels.ResponsModels.Order
{
    public class Instrument
    {
        public int id { get; set; }
        public string gi { get; set; }
        public string ri { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public string isin { get; set; }
        public string ky { get; set; }
        public Group group { get; set; }
        public Board board { get; set; }
        public int state { get; set; }
        public int @class { get; set; }
        public Sector sector { get; set; }
        public SubSector subSector { get; set; }

        public double priceMax { get; set; }
        public double priceMin { get; set; }

        public int askMinQuantity { get; set; }
        public int bidMinQuantity { get; set; }
        public int askMaxQuantity { get; set; }
        public int bidMaxQuantity { get; set; }
        public int minQuantity { get; set; }
        public int maxQuantity { get; set; }
        public int tick { get; set; }
        public int size { get; set; }
        public int segment { get; set; }
        public Product product { get; set; }
        public DateTime updatedAt { get; set; }
        public int baseVolume { get; set; }
    }
}
