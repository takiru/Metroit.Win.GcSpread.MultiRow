namespace Metroit.Win.GcSpread.MultiRow.Test
{
    public class Record : StateKnownMultiRowItemBase
    {
        private string _item1 = string.Empty;

        // State を正しく切り替えるためのコード
        [MultiRow(0, 0)]
        public string Item1
        {
            get => _item1;
            set => SetProperty(ref _item1, value);
        }

        [MultiRow(0, 1)]
        public string Item2 { get; set; } = string.Empty;

        [MultiRow(1, 0)]
        public string Item3 { get; set; } = string.Empty;

        [MultiRow(1, 1)]
        public string Item4 { get; set; } = string.Empty;

        public Record() { }

        public Record(string item1, string item2, string item3, string item4)
        {
            _item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
            NotModifedItem();
        }

        public override string ToString()
        {
            return $"{Item1}, {Item2}, {Item3}, {Item4}";
        }
    }
}
