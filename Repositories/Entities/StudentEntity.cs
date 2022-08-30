namespace Repositories.Entities
{
    public partial class StudentEntity
    {
        public string Id { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? GroupId { get; set; }

        public virtual GroupEntity? Group { get; set; }

        public virtual ICollection<StudentEntity_PointEntity> StudentPoints { get; set; }
    }
}
