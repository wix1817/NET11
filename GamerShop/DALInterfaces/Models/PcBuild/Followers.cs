namespace DALInterfaces.Models.PcBuild
{
    public class Followers : BaseModel
    {
        public virtual  Build Build { get; set; }
        public virtual User User { get; set; }
        public double Value { get; set; }
    }
}
