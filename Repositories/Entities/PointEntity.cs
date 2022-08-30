namespace Repositories.Entities
{
    public partial class PointEntity
    {
        public string Id { get; set; } = null!;
        public string? Subject { get; set; }
        public byte? Point { get; set; }

        public virtual ICollection<StudentEntity_PointEntity> StudentPoints { get; set; }
    }
}
