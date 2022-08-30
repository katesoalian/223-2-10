namespace Repositories.Entities
{
    public partial class GroupEntity
    {
        public GroupEntity()
        {
            Students = new HashSet<StudentEntity>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public byte? Course { get; set; }
        public string? ProfessorId { get; set; }

        public virtual ProfessorEntity? Professor { get; set; }
        public virtual ICollection<StudentEntity> Students { get; set; }
    }
}
