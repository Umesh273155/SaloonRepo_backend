

namespace SalonSphere.Core.Entities
{
    public class Designation
    {
        public int DesignationId { get; set; }
        public string DesignationName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public ICollection<Staff> StaffMembers { get; set; } = new List<Staff>();
    }
}
