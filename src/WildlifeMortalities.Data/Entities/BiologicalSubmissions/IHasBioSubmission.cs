using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.Data.Entities.BiologicalSubmissions;

public interface IHasBioSubmission
{
    int Id { get; }
    int BioSubmissionId { get; }
}
