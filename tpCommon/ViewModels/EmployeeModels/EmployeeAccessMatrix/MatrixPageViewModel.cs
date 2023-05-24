using System.Collections.Generic;

namespace ViewModels.EmployeeModels.EmployeeAccessMatrix
{
    public class MatrixPageViewModel
    {
        public IEnumerable<EmployeeAccessMatrixViewModel> AccessMatrixViewModel { get; set; }
        public IEnumerable<EmployeeAccessMatrixControlViewModel> AccessMatrixControlViewModel { get; set; }
        public IEnumerable<AccessLevelControlViewModel> AccessLevelControlRelationshipsViewModel { get; set; }
    }
}
