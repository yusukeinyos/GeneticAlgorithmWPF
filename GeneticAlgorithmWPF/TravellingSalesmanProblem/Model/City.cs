namespace GeneticAlgorithmWPF.TravellingSalesmanProblem.Model
{
    /// <summary>
    /// 都市
    /// </summary>
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int Order { get; set; }
    }
}
