namespace Repositories.Entities
{
    public partial class StudentEntity_PointEntity
    {
        public string Id { get; set; } = null!;
        public string StudentId { get; set; } = null!;
        public string PointId { get; set; } = null!;

        public virtual PointEntity PointEntity { get; set; } = null!;
        public virtual StudentEntity StudentEntity { get; set; } = null!;
    }
}
