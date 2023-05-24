namespace ViewModels.EmployeeModels.EmployeeAccessMatrix
{
    public class AccessLevelConrolsForEmployeeViewModel
    {
        //Mapping props
        public int RelationShipId { get; set; }
        public int EmployeeAccessLevelId { get; set; }
        public int AccessMatrixControlId { get; set; }
        //Matrix
        public short EmployeeAccessLevel { get; set; }
        public string Notes { get; set; }
        //Control
        public string Page { get; set; }
        public string ControlName { get; set; }
    }
}
